using SharpLexer;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Photon
{
    class ValueNativeClassType : ValueClassType
    {                   
        Type _t;

        Package _pkg;

        internal Type Raw
        {
            get { return _t; }
        }

        internal Package Pkg
        {
            get { return _pkg; }
        }

        Dictionary<int, MethodInfo> _methods = new Dictionary<int, MethodInfo>();

        internal ValueNativeClassType(Package pkg, Type t, ObjectName name )
            : base( name )
        {            
            _t = t;
            _pkg = pkg;

            BuildMember();
        }

        void BuildMember()
        {
            Scope classScope = new Scope(_pkg.TopScope, ScopeType.Class, TokenPos.Init);

            foreach (var m in _t.GetMembers())
            {
                var attr = m.GetCustomAttribute<NativeEntryAttribute>();
                if (attr == null)
                    continue;

                if (m.DeclaringType != _t)
                    continue;

                var methodInfo = _t.GetMethod(m.Name);

                // 让导入的代码能认识这个函数
                Symbol symb = new Symbol();
                symb.Name = m.Name;
                symb.Decl = null;
                symb.Usage = SymbolUsage.Func;

                if ( methodInfo.IsStatic )
                {
                    var dele = methodInfo.CreateDelegate(typeof(NativeDelegate)) as NativeDelegate;

                    // TODO 添加自定义属性可以自定义导入后使用的名称

                    if (_pkg.TopScope.FindSymbol(m.Name) != null)
                    {
                        throw new RuntimeException("name duplicate: " + m.Name);
                    }


                    _pkg.TopScope.Insert(symb);

                    _pkg.Exe.AddFunc(new ValueNativeFunc(new ObjectName(_pkg.Name, m.Name), dele));
                }
                else
                {
                    if ( classScope.FindSymbol(m.Name ) != null )
                    {
                        throw new RuntimeException("class method symbol duplicate: " + m.Name );
                    }                    

                    classScope.Insert( symb );

                    var ci = _pkg.Exe.Constants.AddString(m.Name);

                    _methods.Add(ci, methodInfo);
                }
            }
        }

        internal NativeDelegate CreateMethodDelegate(int nameKey, object ins, out MethodInfo methodInfo)
        {            
            if ( _methods.TryGetValue(nameKey, out methodInfo ))
            {
                return Delegate.CreateDelegate(typeof(NativeDelegate), ins, methodInfo) as NativeDelegate;
            }

            return null;
        }

        internal override ValueObject CreateInstance()
        {
            var obj = Activator.CreateInstance(_t);

            return new ValueNativeClassIns(this, obj);
        }

        internal override bool Equal(Value v)
        {
            var other = v as ValueNativeClassType;

            return _t.Equals(other._t);
        }

        public override string DebugString()
        {
            return string.Format("{0}(native class type)", TypeName);
        }

        public override string TypeName
        {
            get { return _t.Name; }
        }

        public override ValueKind Kind
        {
            get { return ValueKind.NativeClassType; }
        }


    }

}

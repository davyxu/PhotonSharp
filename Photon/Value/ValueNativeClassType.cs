using SharpLexer;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Photon
{
    class ValueNativeClassType : ValueClassType
    {                   
        Type _wrapper;
        Type _target;

        Package _pkg;

        internal Type Raw
        {
            get { return _target; }
        }

        internal Package Pkg
        {
            get { return _pkg; }
        }

        Dictionary<int, ValueNativeFunc> _methods = new Dictionary<int, ValueNativeFunc>();

        internal ValueNativeClassType(Package pkg, Type wrapper, Type tgt, ObjectName name )
            : base( name )
        {            
            _wrapper = wrapper;            
            _target = tgt;
            _pkg = pkg;

            BuildMember();
        }

        void BuildMember()
        {
            Scope classScope = new Scope(_pkg.TopScope, ScopeType.Class, TokenPos.Init);



            foreach (var m in _wrapper.GetMembers())
            {
                var attr = m.GetCustomAttribute<NativeEntryAttribute>();
                if (attr == null)
                    continue;

                // 必须是本类自己的成员
                if (m.DeclaringType != _wrapper)
                    continue;

                var methodInfo = _wrapper.GetMethod(m.Name);

                if (!methodInfo.IsStatic)
                    continue;

                string methodName;
                if (string.IsNullOrEmpty(attr.EntryName))
                {
                    methodName = methodInfo.Name;
                }
                else
                {
                    methodName = attr.EntryName;
                }

                // 让导入的代码能认识这个函数
                Symbol symb = new Symbol();
                symb.Name = methodName;
                symb.Decl = null;
                symb.Usage = SymbolUsage.Func;

                var ci = _pkg.Exe.Constants.AddString(methodName);

                var dele = methodInfo.CreateDelegate(typeof(NativeDelegate)) as NativeDelegate;

                switch ( attr.Type )
                {
                    case NativeEntryType.StaticFunc:
                        {
                            // TODO 添加自定义属性可以自定义导入后使用的名称

                            if (_pkg.TopScope.FindSymbol(methodName) != null)
                            {
                                throw new RuntimeException("name duplicate: " + methodName);
                            }

                            _pkg.TopScope.Insert(symb);

                            _pkg.Exe.AddFunc(new ValueNativeFunc(new ObjectName(_pkg.Name, methodName), dele));
                        }
                        break;
                    case NativeEntryType.ClassMethod:
                        {
                            if (classScope.FindSymbol(methodName) != null)
                            {
                                throw new RuntimeException("class method symbol duplicate: " + methodName);
                            }

                            classScope.Insert(symb);

                            _methods.Add(ci, new ValueNativeFunc(new ObjectName(_pkg.Name, _target.Name, methodName), dele));
                        }
                        break;
                }
            }
        }


        internal string Key2Name(int key)
        {
            var v = Pkg.Exe.Constants.Get(key) as ValueString;
            if (v == null)
                return string.Empty;

            return v.String;
        }

        internal Value GetMethod(int nameKey)
        {
            ValueNativeFunc func;
            if (_methods.TryGetValue(nameKey, out func))
            {
                return func;
            }

            return Value.Nil;
        }

        internal override ValueObject CreateInstance()
        {
            var obj = Activator.CreateInstance(_target);

            return new ValueNativeClassIns(this, obj);
        }

        internal override bool Equal(Value v)
        {
            var other = v as ValueNativeClassType;

            return _wrapper.Equals(other._wrapper);
        }

        public override string DebugString()
        {
            return string.Format("{0}(native class type)", TypeName);
        }

        public override string TypeName
        {
            get { return _wrapper.Name; }
        }

        public override ValueKind Kind
        {
            get { return ValueKind.NativeClassType; }
        }


    }

}

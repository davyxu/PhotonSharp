using SharpLexer;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Photon
{
    class ValueNativeClassType : ValueClassType
    {                           
        Type _instClass;

        Package _pkg;

        internal Type Raw
        {
            get { return _instClass; }
        }

        internal Package Pkg
        {
            get { return _pkg; }
        }

        Dictionary<int, object> _member = new Dictionary<int, object>();

        Scope _scope;

        internal ValueNativeClassType(Package pkg, Type instClass, ObjectName name )
            : base( name )
        {                        
            _instClass = instClass;
            _pkg = pkg;

            _scope = new Scope(_pkg.TopScope, ScopeType.Class, TokenPos.Init);
        }



        static void IterateMember(Type typeToScan, MemberInfo[] minfo, Action<string, MemberInfo, NativeEntryAttribute> callback )
        {
            foreach (var m in minfo)
            {
                // 必须是本类自己的成员
                if (m.DeclaringType != typeToScan)
                    continue;

                var attr = m.GetCustomAttribute<NativeEntryAttribute>();    

                string memberName;
                if (attr != null && !string.IsNullOrEmpty(attr.EntryName))
                {
                    memberName = attr.EntryName;
                }
                else
                {
                    memberName = m.Name;
                }

                callback(memberName, m, attr);
            }
        }

        internal void BuildMember(string className, Type typeToScan)
        {
            BuildMethods(className, typeToScan);

            // 添加使用反射的成员
            //BuildProperties(className, typeToScan);
        }

        void BuildProperties(string className, Type typeToScan)
        {

            IterateMember(typeToScan, typeToScan.GetProperties(), (memberName, mi, attr) =>
            {
                PropertyInfo pi = mi as PropertyInfo;

                var ci = _pkg.Exe.Constants.AddString(memberName);

                _member.Add(ci, pi);
            });
        }

        void BuildMethods(string className, Type typeToScan)
        {

            IterateMember(typeToScan, typeToScan.GetMethods(), ( memberName, mi, attr) =>
            {
                if (attr == null)
                    return;                

                MethodInfo m = mi as MethodInfo;

                if (m.IsConstructor)
                    return;

                if (!m.IsStatic)
                    return;

                var ci = _pkg.Exe.Constants.AddString(memberName);

                switch (attr.Type)
                {
                    case NativeEntryType.StaticFunc:
                    case NativeEntryType.ClassMethod:
                        {
                            // 让导入的代码能认识这个函数
                            Symbol symb = new Symbol();
                            symb.Name = memberName;
                            symb.Decl = null;
                            symb.Usage = SymbolUsage.Func;

                            var dele = m.CreateDelegate(typeof(NativeFunction)) as NativeFunction;

                            if (attr.Type == NativeEntryType.StaticFunc)
                            {
                                if (_pkg.TopScope.FindSymbol(memberName) != null)
                                {
                                    throw new RuntimeException("name duplicate: " + memberName);
                                }

                                _pkg.TopScope.Insert(symb);

                                _pkg.Exe.AddFunc(new ValueNativeFunc(new ObjectName(_pkg.Name, memberName), dele));
                            }
                            else
                            {
                                if (_scope.FindSymbol(memberName) != null)
                                {
                                    throw new RuntimeException("class method symbol duplicate: " + memberName);
                                }

                                _scope.Insert(symb);

                                _member.Add(ci, new ValueNativeFunc(new ObjectName(_pkg.Name, className, memberName), dele));
                            }
                        }
                        break;
                    case NativeEntryType.Property:
                        {
                            var dele = m.CreateDelegate(typeof(NativeProperty)) as NativeProperty;

                            _member.Add(ci, dele);
                        }
                        break;
                }


               

            });         
        }


        internal string Key2Name(int key)
        {
            var v = Pkg.Exe.Constants.Get(key) as ValueString;
            if (v == null)
                return string.Empty;

            return v.Raw;
        }

        internal object GetMember(int nameKey)
        {
            object obj;
            if (_member.TryGetValue(nameKey, out obj))
            {
                return obj;
            }

            return null;
        }

        internal override ValueObject CreateInstance()
        {
            var obj = Activator.CreateInstance(_instClass);

            return new ValueNativeClassIns(this, obj);
        }

        internal override bool Equal(Value v)
        {
            var other = v as ValueNativeClassType;

            return _instClass.Equals(other._instClass);
        }

        public override string DebugString()
        {
            return string.Format("{0}(native class type)", TypeName);
        }

        public override string TypeName
        {
            get { return _instClass.Name; }
        }

        public override ValueKind Kind
        {
            get { return ValueKind.NativeClassType; }
        }


    }

}

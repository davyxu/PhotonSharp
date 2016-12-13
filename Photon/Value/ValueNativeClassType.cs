using SharpLexer;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Photon
{
    class ValueNativeClassType : ValueClassType
    {                           
        Type _data;

        Package _pkg;

        internal Type RawValue
        {
            get { return _data; }
        }

        internal Package Pkg
        {
            get { return _pkg; }
        }

        Dictionary<int, object> _member = new Dictionary<int, object>();

        Scope _scope;

        internal ValueNativeClassType(Package pkg, Type type, ObjectName name )
            : base( name )
        {                        
            _data = type;
            _pkg = pkg;

            _scope = new Scope(_pkg.TopScope, ScopeType.Class, TokenPos.Init);
            _scope.ClassName = _data.Name;
        }

        internal static T GetCustomAttribute<T>(MemberInfo mi) where T : class
        {
            object[] objs = mi.GetCustomAttributes(typeof(T), false);
            if (objs.Length > 0)
                return (T)objs[0];

            return null;
        }

        delegate void IterateCallback(string memberName, MemberInfo mi, NativeEntryAttribute attr);

        static void IterateMember(Type typeToScan, MemberInfo[] minfo, IterateCallback callback)
        {
            foreach (var m in minfo)
            {
                // 必须是本类自己的成员
                if (m.DeclaringType != typeToScan)
                    continue;

                var attr = GetCustomAttribute<NativeEntryAttribute>(m);    

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

                            var dele = Delegate.CreateDelegate(typeof(NativeFunction), m) as NativeFunction;

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
                            var dele = Delegate.CreateDelegate(typeof(NativeProperty), m) as NativeProperty;

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

            return v.RawValue;
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
            var obj = Activator.CreateInstance(_data);

            return new ValueNativeClassIns(this, obj);
        }        

        public override bool Equals(object v)
        {
            var other = v as ValueNativeClassType;
            if (other == null)
                return false;

            return _data.Equals(other._data);
        }

        public override int GetHashCode()
        {
            return _data.GetHashCode();
        }

        public override string DebugString()
        {
            return string.Format("{0}(native class type)", TypeName);
        }

        public override string TypeName
        {
            get { return _data.Name; }
        }

        public override ValueKind Kind
        {
            get { return ValueKind.NativeClassType; }
        }


    }

}

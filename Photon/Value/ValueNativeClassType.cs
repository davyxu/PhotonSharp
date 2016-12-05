using SharpLexer;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Photon
{
    class ValueNativeClassType : ValueClassType
    {                   
        Type _wrapper;
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
            BuildProperties(className, typeToScan);
        }

        void BuildProperties(string className, Type typeToScan)
        {

            IterateMember(typeToScan, typeToScan.GetProperties(), (memberName, mi, attr) =>
            {
                PropertyInfo pi = mi as PropertyInfo;

                var mm = typeToScan.GetMethod(mi.Name);
                if ( mm != null && mm.IsStatic )
                {
                    var dele = mm.CreateDelegate(typeof(NativePropertyDelegate)) as NativePropertyDelegate;
                }
                

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

                if ( attr.Type != NativeEntryType.StaticFunc &&
                    attr.Type != NativeEntryType.ClassMethod)
                {
                    return;
                }

                // 让导入的代码能认识这个函数
                Symbol symb = new Symbol();
                symb.Name = memberName;
                symb.Decl = null;
                symb.Usage = SymbolUsage.Func;

                var ci = _pkg.Exe.Constants.AddString(memberName);

                var dele = m.CreateDelegate(typeof(NativeDelegate)) as NativeDelegate;

                switch (attr.Type)
                {
                    case NativeEntryType.StaticFunc:
                        {
                            // TODO 添加自定义属性可以自定义导入后使用的名称

                            if (_pkg.TopScope.FindSymbol(memberName) != null)
                            {
                                throw new RuntimeException("name duplicate: " + memberName);
                            }

                            _pkg.TopScope.Insert(symb);

                            _pkg.Exe.AddFunc(new ValueNativeFunc(new ObjectName(_pkg.Name, memberName), dele));
                        }
                        break;
                    case NativeEntryType.ClassMethod:
                        {
                            if (_scope.FindSymbol(memberName) != null)
                            {
                                throw new RuntimeException("class method symbol duplicate: " + memberName);
                            }

                            _scope.Insert(symb);

                            _member.Add(ci, new ValueNativeFunc(new ObjectName(_pkg.Name, className, memberName), dele));
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

            return v.String;
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



        internal static Value NativeValue2PhoValue( Type t, object v )
        {
            if (t == typeof(Int32) || t == typeof(float))
            {
                return new ValueNumber((float)v);
            }            
            else if (t == typeof(string))
            {
                return new ValueString((string)v);
            }            
            else if ( t == typeof(void))
            {
                return Value.Nil;
            }
            else 
            {
                throw new RuntimeException("Unsupported native type mapping to language type: "+ t.ToString());
            }
        }

        internal static object PhoValue2NativeValue(Type t, Value v)
        {
            switch (v.Kind) {
                case ValueKind.Number:
                    {
                        var number = (v as ValueNumber).Number;
                    
                        if ( t == typeof(Int32) )
                        {
                            return (Int32)number;
                        }
                        else if (t == typeof(float))
                        {
                            return number;
                        }
                    }
                    break;
                case ValueKind.String:

                    if ( t != typeof(string))
                    {
                        break;
                    }

                    return (v as ValueString).String;
                case ValueKind.Nil:

                    if (t != typeof(void))
                    {
                        break;
                    }

                    return null;
            }

            throw new RuntimeException("language type can not mapping to native type : " + v.Kind.ToString() + "," + t.ToString() );            
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

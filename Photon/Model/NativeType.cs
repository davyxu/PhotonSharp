using System;

namespace Photon
{
    public delegate int NativeFunction(VMachine vm);
    public delegate void NativeProperty(object phoIns, ref object value, bool isGet);

    public enum NativeEntryType
    {
        StaticFunc,
        ClassMethod,
        Class,
        Property,
    }

    public sealed class NativeWrapperClassAttribute : Attribute
    {
        Type _bindingClassType;
        public Type BindingClass
        {
            get { return _bindingClassType; }
        }

        public NativeWrapperClassAttribute(Type bindingClass)
        {
            _bindingClassType = bindingClass;
        }
    }

    // 不生成自动绑定类
    public sealed class NoGenWrapperAttribute : Attribute
    {

    }

    // 绑定到语言的入口
    public sealed class NativeEntryAttribute : Attribute
    {

        string _entryName;
        NativeEntryType _type;

        internal NativeEntryType Type
        {
            get { return _type; }
        }

        internal string EntryName
        {
            get { return _entryName; }
        }

        public NativeEntryAttribute(NativeEntryType type)
        {
            _type = type;
        }

        public NativeEntryAttribute(NativeEntryType type, string entryName)
        {
            _type = type;
            _entryName = entryName;
        }
    }
}

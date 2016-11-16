
namespace PhotonToy
{
    class VarGuard<T>
    {
        T _value;
        object _guard = new object();

        public VarGuard(T init)
        {
            _value = init;
        }

        public T Value
        {
            get
            {
                lock (_guard)
                {
                    return _value;
                }
            }

            set
            {
                lock (_guard)
                {
                    _value = value;
                }
            }
        }
    }

}

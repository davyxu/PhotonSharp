using System;

namespace Photon
{
    class ValueObject : Value
    {
        internal override Value OperateUnary(Opcode code)
        {
            var con = Raw as IContainer;
            if (con == null)
            {
                throw new RuntimeException("Expect 'IContainer'");
            }

            switch (code)
            {
                case Opcode.LEN:
                    return new ValueInteger32(con.GetCount());
                default:
                    throw new RuntimeException("Unknown binary operator:" + code.ToString());
            }
        }

        internal override void OperateSetKeyValue(Value k, Value v)
        {
            var con = Raw as IContainer;
            if (con == null)
            {
                throw new RuntimeException("Expect 'IContainer'");
            }

            con.SetKeyValue(k, v);
        }

        internal override Value OperateGetKeyValue(Value k)
        {
            var con = Raw as IContainer;
            if (con == null)
            {
                throw new RuntimeException("Expect 'IContainer'");
            }

            return con.GetKeyValue(k);
        }

        public override bool Visit(Value iter, DataStack ds)
        {
            var con = Raw as IContainer;
            if (con == null)
            {
                throw new RuntimeException("Expect 'IContainer'");
            }

            ValueIterator iterObj = null;
            if (iter.Equals(Value.Nil))
            {
                iterObj = con.GetIterator();
            }
            else
            {
                iterObj = iter as ValueIterator;

                if ( iterObj == null )
                {
                    throw new RuntimeException("Expect 'Iterator");
                }

                iterObj.Next();
            }

            return iterObj.Iterate(ds);
        }




    }

}

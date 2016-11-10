﻿
namespace Photon.Model
{
    public class Value
    {
        public virtual bool Equal( Value other )
        {
            return false;
        }

        public static Value Nil = new ValueNil();

        public override string ToString()
        {
            return "(empty)";
        }

        public float CastNumber( )
        {
            var v = this as ValueNumber;
            if (v == null)
            {
                throw new RuntimeExcetion("expect number");                
            }

            return v.Number;
        }
        public string CastString()
        {
            var v = this as ValueString;
            if (v == null)
            {
                throw new RuntimeExcetion("expect string");
            }

            return v.String;
        }


        public ValueObject CastObject()
        {
            var v = this as ValueObject;
            if (v == null)
            {
                throw new RuntimeExcetion("expect object");
            }

            return v;
        }

        public ValueFunc CastFunc()
        {
            var v = this as ValueFunc;
            if (v == null)
            {
                throw new RuntimeExcetion("expect function");
            }

            return v;
        }
    }


    public class ValueNil : Value
    {
        public override bool Equal(Value other)
        {
            var otherT = other as ValueNil;
            if (otherT == null)
                return false;

            return true;
        }

        public override string ToString()
        {
            return "(nil)";
        }
    }

    public class Slot
    {
        Value _data = Value.Nil;

        public int ID;

        public Slot( int id )
        {            
            ID = id;
        }

        public Value Data
        {
            get { return _data; }
        }

        public void SetData( Value d )
        {
            if ( ID == 14 )
            {
                int a = 1;
            }
            
            _data = d;
        }

        public override string ToString()
        {
            return string.Format( "{0} ID:{1}", Data.ToString(), ID );
        }
    }

}

using System;

namespace Photon
{
    class ValueInteger32 : Value
    {
        [PhoSerialize]
        Int32 _data = 0;

        // 序列化用, 不要删除
        public ValueInteger32()
        {

        }

        internal ValueInteger32(Int32 data)
        {
            _data = data;
        }

        public Int32 RawValue
        {
            get { return _data; }
        }

        internal override object Raw
        {
            get { return _data; }
        }


        public override bool Equals(object other)
        {
            var otherT = other as ValueInteger32;
            if (otherT == null)
                return false;

            return otherT._data == _data;
        }
        public override int GetHashCode()
        {
            return _data.GetHashCode();
        }

        public override string DebugString()
        {
            return _data.ToString() + " (int32)";
        }

        public override ValueKind Kind
        {
            get { return ValueKind.Integer32; }
        }

        internal override Value OperateBinary(Opcode code, Value other )
        {
            var a = RawValue;

            Int32 b;

            // 类型匹配
            switch( other.Kind )
            {
                case ValueKind.Integer32:
                    b = (other as ValueInteger32).RawValue;
                    break;
                case ValueKind.Integer64:
                    return new ValueInteger64((Int64)this.RawValue).OperateBinary(code, other);
                case ValueKind.Float32:
                    return new ValueFloat32((float)this.RawValue).OperateBinary(code, other);
                case ValueKind.Float64:
                    return new ValueFloat64((double)this.RawValue).OperateBinary(code, other);
                default:
                    throw new RuntimeException("Binary operator value type not match:" + other.ToString());
            }
                        

            switch (code)
            {
                case Opcode.ADD:
                    return new ValueInteger32(a + b);
                case Opcode.SUB:
                    return new ValueInteger32(a - b);
                case Opcode.MUL:
                    return new ValueInteger32(a * b);
                case Opcode.DIV:
                    return new ValueInteger32(a / b);
                case Opcode.GT:
                    return new ValueBool(a > b);
                case Opcode.GE:
                    return new ValueBool(a >= b);                    
                case Opcode.LT:
                    return new ValueBool(a < b);                    
                case Opcode.LE:
                    return new ValueBool(a <= b);                    
                case Opcode.EQ:
                    return new ValueBool(a == b);                    
                case Opcode.NE:
                    return new ValueBool(a != b);                    
                default:
                    throw new RuntimeException("Unknown binary operator:"+ code.ToString() );
            }        
        }

        internal override Value OperateUnary(Opcode code)
        {
            var a = RawValue;            

            switch (code)
            {
                case Opcode.MINUS:
                    return new ValueInteger32(-a);
                case Opcode.INC:
                    return new ValueInteger32(++a);
                case Opcode.DEC:
                    return new ValueInteger32(--a);
                case Opcode.INT32:
                    return this;                
                case Opcode.INT64:
                    return new ValueInteger64((Int64)a);
                case Opcode.FLOAT32:
                    return new ValueFloat32((float)a);
                case Opcode.FLOAT64:
                    return new ValueFloat64((double)a);
                default:
                    throw new RuntimeException("Unknown unary operator:" + code.ToString());
            }
        }
    }



 
}

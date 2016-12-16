using System;

namespace Photon
{
    class ValueFloat32 : Value
    {
        float _data = 0;

        public ValueFloat32()
        {

        }

        internal ValueFloat32( float data )
        {
            _data = data;
        }

        public float RawValue
        {
            get { return _data; }
        }

        internal override object Raw
        {
            get { return _data; }
        }

        public override void Serialize(BinarySerializer ser)
        {
            ser.Serialize(ref _data);
        }

        public override bool Equals(object other)
        {
            var otherT = other as ValueFloat32;
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
            return _data.ToString() + " (float32)";
        }

        public override ValueKind Kind
        {
            get { return ValueKind.Float32; }
        }

        internal override Value OperateBinary(Opcode code, Value other)
        {
            var a = RawValue;

            float b;

            // 类型匹配
            switch (other.Kind)
            {
                case ValueKind.Integer32:
                    return new ValueInteger32((Int32)this.RawValue).OperateBinary(code, other);
                case ValueKind.Integer64:
                    return new ValueInteger64((Int64)this.RawValue).OperateBinary(code, other);
                case ValueKind.Float32:
                    b = (other as ValueFloat32).RawValue;
                    break;
                case ValueKind.Float64:
                    return new ValueFloat64((double)this.RawValue).OperateBinary(code, other);
                default:
                    throw new RuntimeException("Binary operator value type not match:" + other.ToString());
            }

            switch (code)
            {
                case Opcode.ADD:
                    return new ValueFloat32(a + b);
                case Opcode.SUB:
                    return new ValueFloat32(a - b);
                case Opcode.MUL:
                    return new ValueFloat32(a * b);
                case Opcode.DIV:
                    return new ValueFloat32(a / b);
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
                    throw new RuntimeException("Unknown binary operator:" + code.ToString());
            }
        }

        internal override Value OperateUnary(Opcode code)
        {
            var a = RawValue;

            switch (code)
            {
                case Opcode.MINUS:
                    return new ValueFloat32(-a);
                case Opcode.INC:
                    return new ValueFloat32(++a);
                case Opcode.DEC:
                    return new ValueFloat32(--a);
                case Opcode.INT32:
                    return new ValueInteger32((Int32)a);
                case Opcode.INT64:
                    return new ValueInteger64((Int64)a);
                case Opcode.FLOAT32:
                    return this;
                case Opcode.FLOAT64:
                    return new ValueFloat64((double)a);
                default:
                    throw new RuntimeException("Unknown unary operator:" + code.ToString());
            }
        }
    }



 
}

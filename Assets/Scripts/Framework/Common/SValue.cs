/********************************************************************
	Framework Script
	Class: 	GreyFramework::CGameObject
	Author:	
	Created:	
	Note:	$end$
*********************************************************************/


namespace GreyFramework
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public static class SValueHelp
    {
        public static int ToEnum(this SValue value)
        {
            return (int)value;
        }

        public static SValue ToSValue(this object value)
        {
            if (value != null)
            {
                if (value is bool)
                {
                    return (bool)value;
                }
                if (value is Int32)
                {
                    return (Int32)value;
                }
                if (value is SByte)
                {
                    return (SByte)value;
                }
                if (value is Byte)
                {
                    return SValue.Ctor((Byte)value);
                }
                if (value is Int16)
                {
                    return (Int16)value;
                }
                if (value is UInt16)
                {
                    return SValue.Ctor((UInt16)value);
                }
                if (value is UInt32)
                {
                    return (UInt32)value;
                }
                if (value is Int64)
                {
                    return (Int64)value;
                }
                if (value is UInt64)
                {
                    return (UInt64)value;
                }
                if (value is Single)
                {
                    return (Single)value;
                }
                if (value is char)
                {
                    return SValue.Ctor((char)value);
                }
                if (value is String)
                {
                    return (String)value;
                }
                if (value is Double)
                {
                    return (Double)value;
                }

                if (value is Vector2)
                {
                    return (Vector2)value;
                }
                if (value is Vector3)
                {
                    return (Vector3)value;
                }
                if (value is Quaternion)
                {
                    return (Quaternion)value;
                }
                if (value is Color)
                {
                    return (Color)value;
                }
            }
            return SValue.Nil;
        }

        public static SValue ToSValueByType(this object obj, bool ignoreDoubleType = false)
        {
            if (System.Convert.IsDBNull(obj))
                return SValue.Nil;

            System.TypeCode tcode = System.Convert.GetTypeCode(obj);
            if (tcode == System.TypeCode.Empty)
            {
                return SValue.Nil;
            }
            if (tcode == System.TypeCode.Boolean)
            {
                return (bool)obj;
            }
            else if (tcode == System.TypeCode.Int32)
            {
                return (int)obj;
            }
            else if (tcode == System.TypeCode.Single)
            {
                return (float)obj;
            }
            else if (tcode == System.TypeCode.String)
            {
                return (string)obj;
            }
            else if (tcode == System.TypeCode.SByte)
            {
                return (SByte)obj;
            }
            else if (tcode == System.TypeCode.Byte)
            {
                return SValue.Ctor((Byte)obj);
            }
            else if (tcode == System.TypeCode.Int16)
            {
                return (Int16)obj;
            }
            else if (tcode == System.TypeCode.UInt16)
            {
                return SValue.Ctor((UInt16)obj);
            }
            else if (tcode == System.TypeCode.UInt32)
            {
                return (UInt32)obj;
            }
            else if (tcode == System.TypeCode.Int64)
            {
                return (Int64)obj;
            }
            else if (tcode == System.TypeCode.UInt64)
            {
                return (UInt64)obj;
            }
            else if (tcode == System.TypeCode.Char)
            {
                return SValue.Ctor((char)obj);
            }
            else if (tcode == System.TypeCode.Double)
            {
                if (ignoreDoubleType)
                {
                    double cov = (double)obj;
#if UNITY_EDITOR
                    if (cov > Int32.MaxValue)
                        Debug.Log("丢失精度");
#endif
                    return (SValue)((Int32)cov);
                }
                return (Double)obj;
            }
            else if (tcode == System.TypeCode.Object)
            {
                string tpname = obj.GetType().Name;
                if (tpname == typeof(Vector2).Name) return (Vector2)obj;
                else if (tpname == typeof(Vector3).Name) return (Vector3)obj;
                else if (tpname == typeof(Color).Name) return (Color)obj;
                else if (tpname == typeof(Quaternion).Name) return (Quaternion)obj;
                else
                {
                    return SValue.FromObject(obj);
                }
            }

            return SValue.Nil;
        }

        public static T ToValueBySValue<T>(this SValue obj) where T : IEquatable<T>
        {
            SValue.Type tcode = obj.type;
            if (tcode == SValue.Type.Nil)
            {
                return default(T);
            }

            T result = default(T);

            if (tcode == SValue.Type.Boolean)
            {
                bool ret = obj;
                IEquatable<bool> toConb = ret;
                result = (T)toConb;
            }
            else if (tcode == SValue.Type.Int32)
            {
                int ret = obj;
                IEquatable<int> toConb = ret;
                result = (T)toConb;
            }
            else if (tcode == SValue.Type.Single)
            {
                float ret = obj;
                IEquatable<float> toConb = ret;
                result = (T)toConb;
            }
            else if (tcode == SValue.Type.String)
            {
                string ret = obj;
                IEquatable<string> toConb = ret;
                result = (T)toConb;
            }
            else if (tcode == SValue.Type.UInt8)
            {
                SByte ret = obj.ToInt8();
                IEquatable<SByte> toConb = ret;
                result = (T)toConb;
            }
            else if (tcode == SValue.Type.Int8)
            {
                Byte ret = obj.ToUInt8();
                IEquatable<Byte> toConb = ret;
                result = (T)toConb;
            }
            else if (tcode == SValue.Type.Int16)
            {
                Int16 ret = obj.ToInt16();
                IEquatable<Int16> toConb = ret;
                result = (T)toConb;
            }
            else if (tcode == SValue.Type.UInt16)
            {
                UInt16 ret = obj.ToUInt16();
                IEquatable<UInt16> toConb = ret;
                result = (T)toConb;
            }
            else if (tcode == SValue.Type.UInt32)
            {
                UInt32 ret = obj.ToUInt32();
                IEquatable<UInt32> toConb = ret;
                result = (T)toConb;
            }
            else if (tcode == SValue.Type.Int64)
            {
                Int64 ret = obj.ToInt64();
                IEquatable<Int64> toConb = ret;
                result = (T)toConb;
            }
            else if (tcode == SValue.Type.UInt64)
            {
                UInt64 ret = obj.ToUInt64();
                IEquatable<UInt64> toConb = ret;
                result = (T)toConb;
            }
            else if (tcode == SValue.Type.Char)
            {
                Char ret = obj.ToChar();
                IEquatable<Char> toConb = ret;
                result = (T)toConb;
            }
            else if (tcode == SValue.Type.Double)
            {
                Double ret = obj.ToDouble();
                IEquatable<Double> toConb = ret;
                result = (T)toConb;
            }
            else if (tcode == SValue.Type.Vector2)
            {
                Vector2 ret = obj.ToVector2();
                IEquatable<Vector2> toConb = ret;
                result = (T)toConb;
            }
            else if (tcode == SValue.Type.Vector3)
            {
                Vector3 ret = obj.ToVector3();
                IEquatable<Vector3> toConb = ret;
                result = (T)toConb;
            }
            else if (tcode == SValue.Type.Color)
            {
                Color ret = obj.ToColor();
                IEquatable<Color> toConb = ret;
                result = (T)toConb;
            }
            else if (tcode == SValue.Type.Quaternion)
            {
                Quaternion ret = obj.ToQuaternion();
                IEquatable<Quaternion> toConb = ret;
                result = (T)toConb;
            }
            else if (tcode == SValue.Type.Object)
            {
                object ret = obj.ToObject();
                result = (T)ret;
            }

            return result;
        }
    }

    public struct SValue
    {

        public enum Type
        {
            Nil,
            Boolean,
            Int8,
            UInt8,
            Char,
            Int16,
            UInt16,
            Int32,
            UInt32,
            Int64,
            UInt64,
            Single,
            Double,
            String,
            Object,
            Vector2,
            Vector3,
            Quaternion,
            Color,
            SingleNull,
            ColorNull,
            BooleanNull,
            Enum,
            //...
        }

        // 显式指定每个成员内存排布，通过把每个成员的
        // 内存地址偏移都设置为0，实现union的效果
        [StructLayout(LayoutKind.Explicit)]
        internal struct __Value
        {
            [FieldOffset(0)]
            internal Boolean _bool;

            [FieldOffset(0)]
            internal SByte _int8;

            [FieldOffset(0)]
            internal Byte _uint8;

            [FieldOffset(0)]
            internal Char _char;

            [FieldOffset(0)]
            internal Int16 _int16;

            [FieldOffset(0)]
            internal UInt16 _uint16;

            [FieldOffset(0)]
            internal Int32 _int32;

            [FieldOffset(0)]
            internal UInt32 _uint32;

            [FieldOffset(0)]
            internal Int64 _int64;

            [FieldOffset(0)]
            internal UInt64 _uint64;

            [FieldOffset(0)]
            internal Single _single;

            [FieldOffset(0)]
            internal Double _double;

            [FieldOffset(0)]
            internal Vector2 _vector2;

            [FieldOffset(0)]
            internal Vector3 _vector3;

            [FieldOffset(0)]
            internal Quaternion _quaternion;

            [FieldOffset(0)]
            internal float? _singleNull;

            [FieldOffset(0)]
            internal Color? _colorNull;

            [FieldOffset(0)]
            internal Color _color;

            [FieldOffset(0)]
            internal bool? _booleanNull;


            //... 更多的常用值类型数据存储
        };
        public Type type; // 用于表示当前存储的数据类型，获取值需要检查类型
        __Value _val;
        System.Object obj; // 用于存储所有的引用类型，如String
        public static SValue Ctor(Boolean val)
        {
            return new SValue
            {
                type = Type.Boolean,
                obj = null,
                _val = new __Value { _bool = val }

            };
        }
        public static SValue Ctor(Int32 val)
        {
            return new SValue
            {
                type = Type.Int32,
                obj = null,
                _val = new __Value { _int32 = val }
            };
        }
        public static SValue Ctor(SByte val)
        {
            return new SValue
            {
                type = Type.Int8,
                obj = null,
                _val = new __Value { _int8 = val }
            };
        }
        public static SValue Ctor(Byte val)
        {
            return new SValue
            {
                type = Type.UInt8,
                obj = null,
                _val = new __Value { _uint8 = val }
            };
        }
        public static SValue Ctor(Int16 val)
        {
            return new SValue
            {
                type = Type.Int16,
                obj = null,
                _val = new __Value { _int16 = val }
            };
        }
        public static SValue Ctor(UInt16 val)
        {
            return new SValue
            {
                type = Type.UInt16,
                obj = null,
                _val = new __Value { _uint16 = val }
            };
        }
        public static SValue Ctor(UInt32 val)
        {
            return new SValue
            {
                type = Type.UInt32,
                obj = null,
                _val = new __Value { _uint32 = val }
            };
        }
        public static SValue Ctor(Int64 val)
        {
            return new SValue
            {
                type = Type.Int64,
                obj = null,
                _val = new __Value { _int64 = val }
            };
        }
        public static SValue Ctor(UInt64 val)
        {
            return new SValue
            {
                type = Type.UInt64,
                obj = null,
                _val = new __Value { _uint64 = val }
            };
        }
        public static SValue Ctor(Single val)
        {
            return new SValue
            {
                type = Type.Single,
                obj = null,
                _val = new __Value { _single = val }
            };
        }
        public static SValue Ctor(char val)
        {
            return new SValue
            {
                type = Type.Char,
                obj = null,
                _val = new __Value { _char = val }
            };
        }
        public static SValue Ctor(String val)
        {
            return new SValue
            {
                type = Type.String,
                obj = val
            };
        }
        public static SValue Ctor(Double val)
        {
            return new SValue
            {
                type = Type.Double,
                obj = null,
                _val = new __Value { _double = val }
            };
        }
        public static SValue Ctor(Vector2 val)
        {
            return new SValue
            {
                type = Type.Vector2,
                obj = null,
                _val = new __Value { _vector2 = val }
            };
        }
        public static SValue Ctor(Vector3 val)
        {
            return new SValue
            {
                type = Type.Vector3,
                obj = null,
                _val = new __Value { _vector3 = val }
            };
        }
        public static SValue Ctor(Quaternion val)
        {
            return new SValue
            {
                type = Type.Quaternion,
                obj = null,
                _val = new __Value { _quaternion = val }
            };
        }
        public static SValue Ctor(Single? val)
        {
            return new SValue
            {
                type = Type.SingleNull,
                obj = null,
                _val = new __Value { _singleNull = val }
            };
        }
        public static SValue Ctor(Color? val)
        {
            return new SValue
            {
                type = Type.ColorNull,
                obj = null,
                _val = new __Value { _colorNull = val }
            };
        }
        public static SValue Ctor(Boolean? val)
        {
            return new SValue
            {
                type = Type.Boolean,
                obj = null,
                _val = new __Value { _booleanNull = val }
            };
        }
        public static SValue Ctor(Color val)
        {
            return new SValue
            {
                type = Type.Color,
                obj = null,
                _val = new __Value { _color = val }
            };
        }
        //public static SValue Ctor(Enum val)
        //{
        //    return new SValue
        //    {
        //        type = Type.Enum,
        //        obj = val
        //    };
        //}

        // 更多重载函数版本来从不同类型的值来构造自己
        // ...

        public Int32 ToInt32()
        {
            switch (type)
            {
                case Type.Int32:
                    return _val._int32;
                case Type.Int8:
                    return _val._int8;
                case Type.Int16:
                    return _val._int16;
                case Type.UInt8:
                    return _val._uint8;
                case Type.UInt16:
                    return _val._uint16;
                //case Type.UInt32:
                //    return (int)_val._uint32;
                //case Type.Int64:
                //    return (int)_val._int64;
                //case Type.UInt64:
                //    return (int)_val._uint64;
                //case Type.Single:
                //    return (int)_val._single;
                //case Type.Double:
                //    return (int)_val._double;
                default:
                    return default(Int32);
            }

        }
        public Boolean ToBoolean()
        {
            if (type == Type.Boolean)
                return _val._bool;
            else
                return default(Boolean);
        }
        public Char ToChar()
        {
            if (type == Type.Char)
                return _val._char;
            else
                return default(Char);
        }
        public Double ToDouble()
        {
            switch (type)
            {
                case Type.Int32:
                    return _val._int32;
                case Type.Int8:
                    return _val._int8;
                case Type.Int16:
                    return _val._int16;
                case Type.UInt8:
                    return _val._uint8;
                case Type.UInt16:
                    return _val._uint16;
                case Type.UInt32:
                    return _val._uint32;
                case Type.Int64:
                    return _val._int64;
                case Type.UInt64:
                    return _val._uint64;
                case Type.Single:
                    return _val._single;
                case Type.Double:
                    return _val._double;
                default:
                    return default(Double);
            }
        }
        public Int16 ToInt16()
        {
            switch (type)
            {
                //case Type.Int32:
                //    return _val._int32;
                case Type.Int8:
                    return _val._int8;
                case Type.Int16:
                    return _val._int16;
                case Type.UInt8:
                    return _val._uint8;
                //case Type.UInt16:
                //    return _val._uint16;
                //case Type.UInt32:
                //    return _val._uint32;
                //case Type.Int64:
                //    return _val._int64;
                //case Type.UInt64:
                //    return _val._uint64;
                //case Type.Single:
                //    return _val._single;
                //case Type.Double:
                //    return _val._double;
                default:
                    return default(Int16);
            }
        }
        public Int64 ToInt64()
        {
            switch (type)
            {
                case Type.Int32:
                    return _val._int32;
                case Type.Int8:
                    return _val._int8;
                case Type.Int16:
                    return _val._int16;
                case Type.UInt8:
                    return _val._uint8;
                case Type.UInt16:
                    return _val._uint16;
                case Type.UInt32:
                    return _val._uint32;
                case Type.Int64:
                    return _val._int64;
                //case Type.UInt64:
                //    return _val._uint64;
                //case Type.Single:
                //    return _val._single;
                //case Type.Double:
                //    return _val._double;
                default:
                    return default(Int64);
            }
        }
        public SByte ToInt8()
        {
            if (type == Type.Int8)
                return _val._int8;
            else
                return default(SByte);
        }
        public Quaternion ToQuaternion()
        {
            if (type == Type.Quaternion)
                return _val._quaternion;
            else
                return default(Quaternion);
        }
        public Single ToSingle()
        {
            switch (type)
            {
                case Type.Int32:
                    return _val._int32;
                case Type.Int8:
                    return _val._int8;
                case Type.Int16:
                    return _val._int16;
                case Type.UInt8:
                    return _val._uint8;
                case Type.UInt16:
                    return _val._uint16;
                case Type.UInt32:
                    return _val._uint32;
                case Type.Int64:
                    return _val._int64;
                case Type.UInt64:
                    return _val._uint64;
                case Type.Single:
                    return _val._single;
                //case Type.Double:
                //    return _val._double;
                default:
                    return default(Single);
            }
        }
        public UInt16 ToUInt16()
        {
            switch (type)
            {
                //case Type.Int32:
                //    return _val._int32;
                //case Type.Int8:
                //    return _val._int8;
                //case Type.Int16:
                //    return _val._int16;
                case Type.UInt8:
                    return _val._uint8;
                case Type.UInt16:
                    return _val._uint16;
                //case Type.UInt32:
                //    return _val._uint32;
                //case Type.Int64:
                //    return _val._int64;
                //case Type.UInt64:
                //    return _val._uint64;
                //case Type.Single:
                //    return _val._single;
                //case Type.Double:
                //    return _val._double;
                default:
                    return default(UInt16);
            }
        }
        public UInt32 ToUInt32()
        {
            switch (type)
            {
                //case Type.Int32:
                //    return _val._int32;
                //case Type.Int8:
                //    return _val._int8;
                //case Type.Int16:
                //    return _val._int16;
                case Type.UInt8:
                    return _val._uint8;
                case Type.UInt16:
                    return _val._uint16;
                case Type.UInt32:
                    return _val._uint32;
                //case Type.Int64:
                //    return _val._int64;
                //case Type.UInt64:
                //    return _val._uint64;
                //case Type.Single:
                //    return _val._single;
                //case Type.Double:
                //    return _val._double;
                default:
                    return default(UInt32);
            }
        }
        public UInt64 ToUInt64()
        {
            switch (type)
            {
                //case Type.Int32:
                //    return _val._int32;
                //case Type.Int8:
                //    return _val._int8;
                //case Type.Int16:
                //    return _val._int16;
                case Type.UInt8:
                    return _val._uint8;
                case Type.UInt16:
                    return _val._uint16;
                case Type.UInt32:
                    return _val._uint32;
                //case Type.Int64:
                //    return _val._int64;
                case Type.UInt64:
                    return _val._uint64;
                //case Type.Single:
                //    return _val._single;
                //case Type.Double:
                //    return _val._double;
                default:
                    return default(UInt64);
            }
        }
        public Byte ToUInt8()
        {
            if (type == Type.UInt8)
                return _val._uint8;
            else
                return default(Byte);
        }
        public Vector2 ToVector2()
        {
            if (type == Type.Vector2)
                return _val._vector2;
            else
                return default(Vector2);
        }
        public Vector3 ToVector3()
        {
            if (type == Type.Vector3)
                return _val._vector3;
            else
                return default(Vector3);
        }
        public Single? ToSingleNull()
        {
            if (type == Type.SingleNull)
                return _val._single;
            else
                return null;
        }
        public Color? ToColorNull()
        {
            if (type == Type.ColorNull)
                return _val._colorNull;
            else
                return null;
        }
        public Color ToColor()
        {
            if (type == Type.Color)
                return _val._color;
            else
                return default(Color);
        }
        public Boolean? ToBooleanNull()
        {
            if (type == Type.BooleanNull)
                return _val._booleanNull;
            else
                return null;
        }
        //public object ToEnum()
        //{
        //    if (type == Type.Enum)
        //        return obj ;
        //    else
        //        return default(Enum);
        //}
        // ...

        // 避免object类型影响函数重载决议，需要换个名字
        public static SValue FromObject(System.Object val)
        {
            if (val == null)
                return SValue.Nil;
            if (val.GetType().IsEnum)
                Debug.LogError("SValue {0} is  Enum ", (UnityEngine.Object)val);
            if (val.GetType().IsValueType && !(val is Enum))
                Debug.LogErrorFormat("SValue {0} is ValueType", val);
            return new SValue
            {
                type = Type.Object,
                obj = val
            };
        }

        public System.Object ToObject()
        {
            if (this.type != Type.Object && this.type != Type.Nil)
                //Debug.LogError("SValue {0} is not  ObjType ", type);
            if (type == Type.Object)
            {
                return obj;
            }
            return null;
        }
        public override System.String ToString()
        {
            if (type == Type.String && obj != null)
            {
                return obj as string;
            }
            return String.Empty;
        }
        public static implicit operator Double(SValue value)
        {
            return value.ToDouble();
        }
        public static implicit operator SValue(Double value)
        {
            return SValue.Ctor(value);
        }
        public static implicit operator Int32(SValue value)
        {
            return value.ToInt32();
        }
        public static implicit operator SValue(Int32 value)
        {
            return SValue.Ctor(value);
        }
        public static implicit operator UInt64(SValue value)
        {
            return value.ToUInt64();
        }
        public static implicit operator SValue(UInt64 value)
        {
            return SValue.Ctor(value);
        }
        public static implicit operator Int64(SValue value)
        {
            return value.ToInt64();
        }
        public static implicit operator SValue(Int64 value)
        {
            return SValue.Ctor(value);
        }
        public static implicit operator UInt32(SValue value)
        {
            return value.ToUInt32();
        }
        public static implicit operator SValue(UInt32 value)
        {
            return SValue.Ctor(value);
        }
        public static implicit operator string(SValue value)
        {
            return value.ToString();
        }
        public static implicit operator SValue(string value)
        {
            return SValue.Ctor(value);
        }
        public static implicit operator bool(SValue value)
        {
            return value.ToBoolean();
        }
        public static implicit operator SValue(bool value)
        {
            return SValue.Ctor(value);
        }
        public static implicit operator Vector3(SValue value)
        {
            return value.ToVector3();
        }
        public static implicit operator SValue(Vector3 value)
        {
            return SValue.Ctor(value);
        }
        public static implicit operator Vector2(SValue value)
        {
            return value.ToVector2();
        }
        public static implicit operator SValue(Vector2 value)
        {
            return SValue.Ctor(value);
        }
        public static implicit operator Quaternion(SValue value)
        {
            return value.ToQuaternion();
        }
        public static implicit operator SValue(Quaternion value)
        {
            return SValue.Ctor(value);
        }
        public static implicit operator float?(SValue value)
        {
            return value.ToSingleNull();
        }
        public static implicit operator SValue(float? value)
        {
            return SValue.Ctor(value);
        }
        public static implicit operator bool?(SValue value)
        {
            return value.ToBooleanNull();
        }
        public static implicit operator SValue(bool? value)
        {
            return SValue.Ctor(value);
        }
        public static implicit operator Color(SValue value)
        {
            return value.ToColor();
        }
        public static implicit operator SValue(Color value)
        {
            return SValue.Ctor(value);
        }
        public static implicit operator Color?(SValue value)
        {
            return value.ToColorNull();
        }
        public static implicit operator SValue(Color? value)
        {
            return SValue.Ctor(value);
        }
        public static implicit operator Single(SValue value)
        {
            return value.ToSingle();
        }
        public static implicit operator SValue(Single value)
        {
            return SValue.Ctor(value);
        }
        //public static implicit operator Enum(SValue value)
        //{
        //    return value.ToEnum();
        //}
        //public static implicit operator SValue(Enum value)
        //{
        //    return SValue.Ctor(value);
        //}


        public static bool operator ==(SValue lhs, SValue rhs)
        {
            if (lhs.type != rhs.type)
            {
                if (lhs.type == SValue.Type.Object)
                {
#if UNITY_EDITOR
                    if (rhs.type != SValue.Type.Nil)
                        Debug.LogWarning("compare object with value type");
#endif
                    return equalObject(rhs, lhs.ToObject());
                }
                else if (rhs.type == SValue.Type.Object)
                {
#if UNITY_EDITOR
                    if (lhs.type != SValue.Type.Nil)
                        Debug.LogWarning("compare object with value type");
#endif
                    return equalObject(lhs, rhs.ToObject());
                }
                else if (lhs.type == SValue.Type.Nil)
                {
                    return equalNil(rhs);
                }
                else if (rhs.type == SValue.Type.Nil)
                {
                    return equalNil(lhs);
                }
                else
                    return false;
            }

            else
            {
                return equal(lhs, rhs);
            }

        }
        public static bool operator !=(SValue lhs, SValue rhs)
        {
            if (lhs.type != rhs.type)
            {
                if (lhs.type == SValue.Type.Object)
                {
#if UNITY_EDITOR
                    if (rhs.type != SValue.Type.Nil)
                        Debug.LogWarning("compare object with value type");
#endif
                    return !equalObject(rhs, lhs.ToObject());
                }
                else if (rhs.type == SValue.Type.Object)
                {
#if UNITY_EDITOR
                    if (lhs.type != SValue.Type.Nil)
                        Debug.LogWarning("compare object with value type");
#endif
                    return !equalObject(lhs, rhs.ToObject());
                }
                else if (lhs.type == SValue.Type.Nil)
                {
                    return !equalNil(rhs);
                }
                else if (rhs.type == SValue.Type.Nil)
                {
                    return !equalNil(lhs);
                }
                else
                    return true;
            }

            else
            {
                return !equal(lhs, rhs);
            }
        }
        static bool equal(SValue lhs, SValue rhs)
        {
            if (lhs.type != rhs.type)
            {
                Debug.LogError("error compare :Svalue type not equal");
                return false;
            }
            switch (lhs.type)
            {
                case Type.Boolean:
                    return lhs._val._bool == rhs._val._bool;
                case Type.BooleanNull:
                    return lhs._val._booleanNull == rhs._val._booleanNull;
                case Type.Char:
                    return lhs._val._char == rhs._val._char;
                case Type.ColorNull:
                    return lhs._val._colorNull == rhs._val._colorNull;
                case Type.Double:
                    return lhs._val._double == rhs._val._double;
                case Type.Int16:
                    return lhs._val._int16 == rhs._val._int16;
                case Type.Int32:
                    return lhs._val._int32 == rhs._val._int32;
                case Type.Int64:
                    return lhs._val._int64 == rhs._val._int64;
                case Type.Int8:
                    return lhs._val._int8 == rhs._val._int8;
                case Type.Nil:
                    return true;
                case Type.Quaternion:
                    return lhs._val._quaternion == rhs._val._quaternion;
                case Type.Single:
                    return lhs._val._single == rhs._val._single;
                case Type.SingleNull:
                    return lhs._val._singleNull == rhs._val._singleNull;
                case Type.String:
                    return lhs.ToString().Equals(rhs.ToString());
                case Type.UInt16:
                    return lhs._val._uint16 == rhs._val._uint16;
                case Type.UInt32:
                    return lhs._val._uint32 == rhs._val._uint32;
                case Type.UInt64:
                    return lhs._val._uint64 == rhs._val._uint64;
                case Type.UInt8:
                    return lhs._val._uint8 == rhs._val._uint8;
                case Type.Enum:
                    return lhs.obj == rhs.obj;
                case Type.Object:
                    return lhs.ToObject() == rhs.ToObject();
                case Type.Vector2:
                    return lhs.ToVector2() == rhs.ToVector2();
                case Type.Vector3:
                    return lhs.ToVector3() == rhs.ToVector3();
                case Type.Color:
                    return lhs.ToColor() == rhs.ToColor();
                default:
                    Debug.LogWarningFormat("SValue type undef {0}", lhs.type.ToString());
                    return false;

            }
        }
        static bool equalNil(SValue value)
        {
            switch (value.type)
            {
                case Type.Boolean:
                case Type.Char:
                case Type.Double:
                case Type.Int16:
                case Type.Int32:
                case Type.Int64:
                case Type.Int8:
                case Type.Quaternion:
                case Type.Single:
                case Type.UInt16:
                case Type.UInt64:
                case Type.UInt32:
                case Type.UInt8:
                case Type.Vector2:
                case Type.Vector3:
                case Type.Color:
                    {
                        //Debug.LogWarning("SValue Compare ValueType null");
                    }
                    return false;
                case Type.BooleanNull:
                    return value._val._booleanNull == null;
                case Type.Enum:
                case Type.Object:
                case Type.String:
                    return value.obj == null;
                case Type.ColorNull:
                    return value._val._colorNull == null;
                case Type.SingleNull:
                    return value._val._singleNull == null;
                default:
                    Debug.LogWarningFormat("SValue type undef {0}", value.type.ToString());
                    return false;

            }
        }
        static bool equalObject(SValue value, System.Object obj)
        {
            switch (value.type)
            {
                case Type.Boolean:
                    return value._val._bool == (Boolean)obj;
                case Type.BooleanNull:
                    return value._val._booleanNull == (Boolean?)obj;
                case Type.Char:
                    return value._val._char == (Char)obj;
                case Type.ColorNull:
                    return value._val._colorNull == (Color?)obj;
                case Type.Double:
                    return value._val._double == (Double?)obj;
                case Type.Int16:
                    return value._val._int16 == (Int16)obj;
                case Type.Int32:
                    return value._val._int32 == (Int32)obj;
                case Type.Int64:
                    return value._val._int64 == (Int64)obj;
                case Type.Int8:
                    return value._val._int8 == (Byte)obj;
                case Type.Nil:
                    return obj == null;
                case Type.Quaternion:
                    return value._val._quaternion == (Quaternion)obj;
                case Type.Single:
                    return value._val._single == (Single)obj;
                case Type.SingleNull:
                    return value._val._singleNull == (Single?)obj;
                case Type.String:
                    return value.ToString().Equals(obj as string);
                case Type.UInt16:
                    return value._val._uint16 == (UInt16)obj;
                case Type.UInt32:
                    return value._val._uint32 == (UInt32)obj;
                case Type.UInt64:
                    return value._val._uint64 == (UInt64)obj;
                case Type.UInt8:
                    return value._val._uint8 == (SByte)obj;
                case Type.Enum:
                    return value.obj == (Enum)obj;
                case Type.Vector2:
                    return value._val._vector2 == (Vector2)obj;
                case Type.Vector3:
                    return value._val._vector3 == (Vector3)obj;
                case Type.Color:
                    return value._val._color == (Color)obj;
                default:
                    Debug.LogWarning("SValue type undef");
                    return false;

            }
        }
        //
        // 摘要:
        //     ///
        //     Returns true if the given vector is exactly equal to this vector.
        //     ///
        //
        // 参数:
        //   other:
        public override bool Equals(object other)
        {
            if (other is SValue)
                return equal(this, (SValue)other);
            else
                return equalObject(this, other);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static SValue Nil = new SValue() { type = SValue.Type.Nil };
        public static SValue Default = default(SValue);
        //public static implicit operator object(SValue value)
        //{
        //    return value.ToObject();
        //}
        //public static implicit operator SValue(object value)
        //{
        //    return SValue.FromObject(value);
        //}
    }

}
using System;
using System.Text;

namespace ErisGame
{
    public class SerializaBuffer
    {

        public static byte[] Int32ToBytes(int value)
        {
            byte[] bytes = new byte[sizeof(int)];
            bytes[3] = (byte)((value & 0xff000000) >> 24);
            bytes[2] = (byte)((value & 0x00ff0000) >> 16);
            bytes[1] = (byte)((value & 0x0000ff00) >> 8);
            bytes[0] = (byte)((value & 0x000000ff) >> 0);
            return bytes;
        }
        public static byte[] String32ToBytes(string value)
        {
            byte[] bytes = new byte[value.Length];
            bytes = Encoding.Unicode.GetBytes(value);
            return bytes;
        }
        public static byte[] LongToBytes(long value)
        {
            byte[] bytes = new byte[sizeof(long)];
            bytes = BitConverter.GetBytes(value);
            return bytes;
        }
        public static byte[] ShortToBytes(short value)
        {  
            byte[] bytes = new byte[sizeof(short)];
            bytes = BitConverter.GetBytes(value);
            return bytes;
        }
        public static byte[] BoolToBytes(bool value)
        {
            byte[] bytes = new byte[sizeof(bool)];
            bytes = BitConverter.GetBytes(value);
            return bytes;
        }
        public static byte[] FloatToBytes(float value)
        {
            byte[] bytes = new byte[sizeof(float)];
            bytes = BitConverter.GetBytes(value);
            return bytes;
        }
        public static byte[] EnumToBytes(int value)
        {  
            byte[] bytes = new byte[sizeof(int)];
            bytes = BitConverter.GetBytes(value);
            return bytes;
        }
        public static int BytesToInt32(byte[] bytes,ref int seek)
        {
            int value =0;
            value = BitConverter.ToInt32(bytes, 0);
            seek += sizeof(int);
            return value;

        }
        public static int BytesToInt32(byte[] bytes)
        {
            int value = 0;
            value = BitConverter.ToInt32(bytes, 0);
            return value;

        }
        public static string BytesToString(byte[] bytes, ref int seek)
        {
            string value = string.Empty;
            value = Encoding.Unicode.GetString(bytes);
            seek += bytes.Length;
            return value;

        }
        public static string BytesToString(byte[] bytes)
        {
            string value = string.Empty;
            value = Encoding.Unicode.GetString(bytes);
            return value;

        }
        public static long BytesToLong(byte[] bytes, ref long seek)
        {
            long value =0;
            value = BitConverter.ToInt64(bytes, 0);
            seek += sizeof(long);
            return value;

        }
        public static short BytesToShort(byte[] bytes, ref int seek)
        {
            short value = 0;
            value = BitConverter.ToInt16(bytes, 0);
            seek += sizeof(short);
            return value;

        }
        public static short BytesToShort(byte[] bytes)
        {
            short value = 0;
            value = BitConverter.ToInt16(bytes, 0);
            return value;

        }
        public static float BytesToFloat(byte[] bytes, ref int seek)
        {
            float value = 0;
            value = BitConverter.ToSingle(bytes, 0);
            seek += sizeof(float);
            return value;

        }
        public static bool BytesToBool(byte[] bytes, ref int seek)
        {
            bool value = false;
            value = BitConverter.ToBoolean(bytes, 0);
            seek += sizeof(bool);
            return value;

        }
        public static object BytesToEnum(byte[] bytes, ref int seek)
        {
            int value = 0;
            value = BitConverter.ToInt32(bytes, 0);
            seek += sizeof(int);
            return value;
        }

    }
}

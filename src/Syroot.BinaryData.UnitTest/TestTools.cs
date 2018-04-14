using System;
using System.IO;

namespace Syroot.BinaryData.UnitTest
{
    internal static class TestTools
    {
        // ---- FIELDS -------------------------------------------------------------------------------------------------

        internal static readonly ByteConverter ReverseByteConverter
            = BitConverter.IsLittleEndian ? ByteConverter.Big : ByteConverter.Little;

        // ---- METHODS (INTERNAL) -------------------------------------------------------------------------------------

        internal static void WriteDouble(Stream stream, Double value, bool reverse = false)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            if (reverse)
                Array.Reverse(buffer);
            stream.WriteBytes(buffer);
        }

        internal static void WriteInt16(Stream stream, Int16 value, bool reverse = false)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            if (reverse)
                Array.Reverse(buffer);
            stream.WriteBytes(buffer);
        }

        internal static void WriteInt32(Stream stream, Int32 value, bool reverse = false)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            if (reverse)
                Array.Reverse(buffer);
            stream.WriteBytes(buffer);
        }

        internal static void WriteInt64(Stream stream, Int64 value, bool reverse = false)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            if (reverse)
                Array.Reverse(buffer);
            stream.WriteBytes(buffer);
        }

        internal static void WriteSingle(MemoryStream stream, Single value, bool reverse = false)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            if (reverse)
                Array.Reverse(buffer);
            stream.WriteBytes(buffer);
        }

        internal static void WriteUInt16(Stream stream, UInt16 value, bool reverse = false)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            if (reverse)
                Array.Reverse(buffer);
            stream.WriteBytes(buffer);
        }

        internal static void WriteUInt32(Stream stream, UInt32 value, bool reverse = false)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            if (reverse)
                Array.Reverse(buffer);
            stream.WriteBytes(buffer);
        }

        internal static void WriteUInt64(Stream stream, UInt64 value, bool reverse = false)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            if (reverse)
                Array.Reverse(buffer);
            stream.WriteBytes(buffer);
        }
    }
}

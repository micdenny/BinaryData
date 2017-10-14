using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Syroot.BinaryData.Extensions;

namespace Syroot.BinaryData.UnitTest
{
    [TestClass]
    public class StreamExtensionsTests
    {
        // ---- CONSTANTS ----------------------------------------------------------------------------------------------

        private static readonly ByteConverter _reversedConverter
            = BitConverter.IsLittleEndian ? ByteConverter.BigEndian : ByteConverter.LittleEndian;

        // ---- FIELDS -------------------------------------------------------------------------------------------------

        private static MemoryStream _stream = new MemoryStream();

        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        [TestInitialize]
        public void Initialize()
        {
            // Create a new stream to write test data into with the .NET default writer.
            _stream?.Dispose();
            _stream = new MemoryStream();
        }

        [TestMethod]
        public void Align()
        {
            // Position 0 aligns to everything.
            _stream.Align(3);
            Assert.AreEqual(0, _stream.Position);
            Assert.AreEqual(0, _stream.Length);

            // Align but do not grow.
            _stream.Position++;
            _stream.Align(2);
            Assert.AreEqual(2, _stream.Position);
            Assert.AreEqual(0, _stream.Length);

            // Align and grow.
            _stream.Align(2, true);
            Assert.AreEqual(2, _stream.Position);
            Assert.AreEqual(2, _stream.Length);

            // Align again.
            _stream.Align(4);
            Assert.AreEqual(4, _stream.Position);
            Assert.AreEqual(2, _stream.Length);

            // Alignment must be bigger than 0.
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _stream.Align(0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _stream.Align(-1));
        }

        [TestMethod]
        public void ReadBoolean()
        {
            // Write test values.
            using (BinaryWriter writer = new BinaryWriter(_stream, Encoding.UTF8, true))
            {
                writer.Write(true);
                writer.Write(false);
                writer.Write((byte)255);
                writer.Write((byte)0);

                writer.Write((short)0);
                writer.Write((short)1);
                writer.Write((short)256);
                writer.Write((short)-5);

                writer.Write(0);
                writer.Write(1);
                writer.Write(3);
                writer.Write(-1);
            }

            // Read in system endianness.
            _stream.Position = 0;

            Assert.AreEqual(true, _stream.ReadBoolean());
            Assert.AreEqual(false, _stream.ReadBoolean(BooleanDataFormat.Byte));
            Assert.AreEqual(true, _stream.ReadBoolean());
            Assert.AreEqual(false, _stream.ReadBoolean(BooleanDataFormat.Byte));

            Assert.AreEqual(false, _stream.ReadBoolean(BooleanDataFormat.Word));
            Assert.AreEqual(true, _stream.ReadBoolean(BooleanDataFormat.Word));
            Assert.AreEqual(true, _stream.ReadBoolean(BooleanDataFormat.Word));
            Assert.AreEqual(true, _stream.ReadBoolean(BooleanDataFormat.Word));

            Assert.AreEqual(false, _stream.ReadBoolean(BooleanDataFormat.Dword));
            Assert.AreEqual(true, _stream.ReadBoolean(BooleanDataFormat.Dword));
            Assert.AreEqual(true, _stream.ReadBoolean(BooleanDataFormat.Dword));
            Assert.AreEqual(true, _stream.ReadBoolean(BooleanDataFormat.Dword));
        }

        [TestMethod]
        public void ReadDateTime()
        {
            DateTime value1 = new DateTime(2000, 12, 24);
            DateTime value2 = DateTime.Now;
            DateTime value3 = new DateTime(1970, 1, 1);
            DateTime value4 = new DateTime(2001, 09, 09, 01, 46, 40);

            // Write test values.
            using (BinaryWriter writer = new BinaryWriter(_stream, Encoding.UTF8, true))
            {
                writer.Write(value1.Ticks);
                writer.Write(value2.Ticks);
                writer.Write(0); // value3
                writer.Write((long)0); // value3
                writer.Write(1_000_000_000); // value4
            }

            // Read in system endianness.
            _stream.Position = 0;
            Assert.AreEqual(value1, _stream.ReadDateTime());
            Assert.AreEqual(value2, _stream.ReadDateTime());
            Assert.AreEqual(value3, _stream.ReadDateTime(DateTimeDataFormat.CTime));
            Assert.AreEqual(value3, _stream.ReadDateTime(DateTimeDataFormat.CTime64));
            Assert.AreEqual(value4, _stream.ReadDateTime(DateTimeDataFormat.CTime));
        }

        [TestMethod]
        public void ReadDecimal()
        {
            Decimal value1 = new Decimal(1111, 2222, 3333, false, 4);
            Decimal value2 = new Decimal(100.123);
            Decimal value3 = new Decimal(-567.890);
            Decimal value4 = Decimal.MaxValue;

            // Write test values.
            using (BinaryWriter writer = new BinaryWriter(_stream, Encoding.UTF8, true))
            {
                writer.Write(value1);
                writer.Write(value2);
                writer.Write(value3);
                writer.Write(value4);
            }

            // Read in system endianness.
            _stream.Position = 0;
            Assert.AreEqual(value1, _stream.ReadDecimal());
            Assert.AreEqual(value2, _stream.ReadDecimal());
            Assert.AreEqual(value3, _stream.ReadDecimal());
            Assert.AreEqual(value4, _stream.ReadDecimal());
        }

        [TestMethod]
        public void ReadDouble()
        {
            const Double value1 = 49210421.2421;
            const Double value2 = Double.NegativeInfinity;
            const Double value3 = -567.890;
            const Double value4 = Double.NaN;

            // Write test values.
            using (BinaryWriter writer = new BinaryWriter(_stream, Encoding.UTF8, true))
            {
                writer.Write(value1);
                writer.Write(value2);
                writer.Write(value3);
                writer.Write(value4);
            }

            // Read in system endianness.
            _stream.Position = 0;
            Assert.AreEqual(value1, _stream.ReadDouble());
            Assert.AreEqual(value2, _stream.ReadDouble());
            Assert.AreEqual(value3, _stream.ReadDouble());
            Assert.AreEqual(value4, _stream.ReadDouble());
        }

        [TestMethod]
        public void ReadInt16()
        {
            const Int16 value1 = 0x0403;
            const Int16 value2 = 0x0102;
            const Int16 value3 = unchecked((Int16)0xDDCC);
            const Int16 value4 = unchecked((Int16)0xCCDD);
            const Int16 value1r = 0x0304;
            const Int16 value2r = 0x0201;
            const Int16 value3r = unchecked((Int16)0xCCDD);
            const Int16 value4r = unchecked((Int16)0xDDCC);

            // Write test values.
            using (BinaryWriter writer = new BinaryWriter(_stream, Encoding.UTF8, true))
            {
                writer.Write(value1);
                writer.Write(value2);
                writer.Write(value3);
                writer.Write(value4);
            }

            // Read in system endianness.
            _stream.Position = 0;
            Assert.AreEqual(value1, _stream.ReadInt16());
            Assert.AreEqual(value2, _stream.ReadInt16());
            Assert.AreEqual(value3, _stream.ReadInt16());
            Assert.AreEqual(value4, _stream.ReadInt16());

            // Read in reversed endianness.
            _stream.Position = 0;
            Assert.AreEqual(value1r, _stream.ReadInt16(converter: _reversedConverter));
            Assert.AreEqual(value2r, _stream.ReadInt16(converter: _reversedConverter));
            Assert.AreEqual(value3r, _stream.ReadInt16(converter: _reversedConverter));
            Assert.AreEqual(value4r, _stream.ReadInt16(converter: _reversedConverter));
        }

        [TestMethod]
        public void ReadInt32()
        {
            const Int32 value1 = 0x04030201;
            const Int32 value2 = 0x01020304;
            const Int32 value3 = unchecked((Int32)0xDDCCBBAA);
            const Int32 value4 = unchecked((Int32)0xCCDDEEFF);
            const Int32 value1r = 0x01020304;
            const Int32 value2r = 0x04030201;
            const Int32 value3r = unchecked((Int32)0xAABBCCDD);
            const Int32 value4r = unchecked((Int32)0xFFEEDDCC);

            // Write test values.
            using (BinaryWriter writer = new BinaryWriter(_stream, Encoding.UTF8, true))
            {
                writer.Write(value1);
                writer.Write(value2);
                writer.Write(value3);
                writer.Write(value4);
            }

            // Read in system endianness.
            _stream.Position = 0;
            Assert.AreEqual(value1, _stream.ReadInt32());
            Assert.AreEqual(value2, _stream.ReadInt32());
            Assert.AreEqual(value3, _stream.ReadInt32());
            Assert.AreEqual(value4, _stream.ReadInt32());

            // Read in other endianness.
            _stream.Position = 0;
            Assert.AreEqual(value1r, _stream.ReadInt32(converter: _reversedConverter));
            Assert.AreEqual(value2r, _stream.ReadInt32(converter: _reversedConverter));
            Assert.AreEqual(value3r, _stream.ReadInt32(converter: _reversedConverter));
            Assert.AreEqual(value4r, _stream.ReadInt32(converter: _reversedConverter));
        }

        [TestMethod]
        public void ReadInt64()
        {
            const Int64 value1 = 0x0807060504030201;
            const Int64 value2 = 0x0102030405060708;
            const Int64 value3 = unchecked((Int64)0xFFEEDDCCBBAA9988);
            const Int64 value4 = unchecked((Int64)0x8899AABBCCDDEEFF);
            const Int64 value1r = 0x0102030405060708;
            const Int64 value2r = 0x0807060504030201;
            const Int64 value3r = unchecked((Int64)0x8899AABBCCDDEEFF);
            const Int64 value4r = unchecked((Int64)0xFFEEDDCCBBAA9988);

            // Write test values.
            using (BinaryWriter writer = new BinaryWriter(_stream, Encoding.UTF8, true))
            {
                writer.Write(value1);
                writer.Write(value2);
                writer.Write(value3);
                writer.Write(value4);
            }

            // Read in system endianness.
            _stream.Position = 0;
            Assert.AreEqual(value1, _stream.ReadInt64());
            Assert.AreEqual(value2, _stream.ReadInt64());
            Assert.AreEqual(value3, _stream.ReadInt64());
            Assert.AreEqual(value4, _stream.ReadInt64());

            // Read in other endianness.
            _stream.Position = 0;
            Assert.AreEqual(value1r, _stream.ReadInt64(converter: _reversedConverter));
            Assert.AreEqual(value2r, _stream.ReadInt64(converter: _reversedConverter));
            Assert.AreEqual(value3r, _stream.ReadInt64(converter: _reversedConverter));
            Assert.AreEqual(value4r, _stream.ReadInt64(converter: _reversedConverter));
        }

        [TestMethod]
        public void ReadSingle()
        {
            const Single value1 = 49210421.2421f;
            const Single value2 = Single.NegativeInfinity;
            const Single value3 = -567.890f;
            const Single value4 = Single.NaN;

            // Write test values.
            using (BinaryWriter writer = new BinaryWriter(_stream, Encoding.UTF8, true))
            {
                writer.Write(value1);
                writer.Write(value2);
                writer.Write(value3);
                writer.Write(value4);
            }

            // Read in system endianness.
            _stream.Position = 0;
            Assert.AreEqual(value1, _stream.ReadSingle());
            Assert.AreEqual(value2, _stream.ReadSingle());
            Assert.AreEqual(value3, _stream.ReadSingle());
            Assert.AreEqual(value4, _stream.ReadSingle());
        }

        [TestMethod]
        public void ReadString()
        {
            const string value1 = "크레이지레이싱 카트라이더";
            const string value2 = "äöüßÄÖÜ";
            const string value3 = "Hello World";
            const string value4 = "Trump sucks";

            // Write test values.
            using (BinaryWriter writer = new BinaryWriter(_stream, Encoding.UTF8, true))
            {
                // .NET variable-length prefix
                writer.Write(value1);
                writer.Write(value2);
                // Custom length prefix
                writer.Write(value1.Length);
                writer.Write(Encoding.UTF8.GetBytes(value1));
                writer.Write((Int16)value2.Length);
                writer.Write(Encoding.UTF8.GetBytes(value2));
                // Zero termination
                writer.Write(Encoding.Unicode.GetBytes(value1));
                writer.Write((Int16)0);
            }
            using (BinaryWriter writer = new BinaryWriter(_stream, Encoding.ASCII, true))
            {
                // .NET variable-length prefix
                writer.Write(value3);
                writer.Write(value4);
                // Custom length prefix
                writer.Write(value3.Length);
                writer.Write(Encoding.ASCII.GetBytes(value3));
                writer.Write((Byte)value4.Length);
                writer.Write(Encoding.ASCII.GetBytes(value4));
                // Zero termination
                writer.Write(Encoding.ASCII.GetBytes(value3));
                writer.Write((Byte)0);
            }

            // Read test values.
            _stream.Position = 0;
            Assert.AreEqual(value1, _stream.ReadString(StringDataFormat.DynamicByteCount, Encoding.UTF8));
            Assert.AreEqual(value2, _stream.ReadString(StringDataFormat.DynamicByteCount, Encoding.UTF8));
            Assert.AreEqual(value1, _stream.ReadString(StringDataFormat.Int32CharCount, Encoding.UTF8));
            Assert.AreEqual(value2, _stream.ReadString(StringDataFormat.Int16CharCount, Encoding.UTF8));
            Assert.AreEqual(value1, _stream.ReadString(StringDataFormat.ZeroTerminated, Encoding.Unicode));
            Assert.AreEqual(value3, _stream.ReadString(StringDataFormat.DynamicByteCount, Encoding.ASCII));
            Assert.AreEqual(value4, _stream.ReadString(StringDataFormat.DynamicByteCount, Encoding.ASCII));
            Assert.AreEqual(value3, _stream.ReadString(StringDataFormat.Int32CharCount, Encoding.ASCII));
            Assert.AreEqual(value4, _stream.ReadString(StringDataFormat.ByteCharCount, Encoding.ASCII));
            Assert.AreEqual(value3, _stream.ReadString(StringDataFormat.ZeroTerminated, Encoding.ASCII));
        }

        [TestMethod]
        public void ReadUInt16()
        {
            const UInt16 value1 = 0x0403;
            const UInt16 value2 = 0x0102;
            const UInt16 value3 = 0xDDCC;
            const UInt16 value4 = 0xCCDD;
            const UInt16 value1r = 0x0304;
            const UInt16 value2r = 0x0201;
            const UInt16 value3r = 0xCCDD;
            const UInt16 value4r = 0xDDCC;

            // Write test values.
            using (BinaryWriter writer = new BinaryWriter(_stream, Encoding.UTF8, true))
            {
                writer.Write(value1);
                writer.Write(value2);
                writer.Write(value3);
                writer.Write(value4);
            }

            // Read in system endianness.
            _stream.Position = 0;
            Assert.AreEqual(value1, _stream.ReadUInt16());
            Assert.AreEqual(value2, _stream.ReadUInt16());
            Assert.AreEqual(value3, _stream.ReadUInt16());
            Assert.AreEqual(value4, _stream.ReadUInt16());

            // Read in reversed endianness.
            _stream.Position = 0;
            Assert.AreEqual(value1r, _stream.ReadUInt16(converter: _reversedConverter));
            Assert.AreEqual(value2r, _stream.ReadUInt16(converter: _reversedConverter));
            Assert.AreEqual(value3r, _stream.ReadUInt16(converter: _reversedConverter));
            Assert.AreEqual(value4r, _stream.ReadUInt16(converter: _reversedConverter));
        }

        [TestMethod]
        public void ReadUInt32()
        {
            const UInt32 value1 = 0x04030201;
            const UInt32 value2 = 0x01020304;
            const UInt32 value3 = 0xDDCCBBAA;
            const UInt32 value4 = 0xCCDDEEFF;
            const UInt32 value1r = 0x01020304;
            const UInt32 value2r = 0x04030201;
            const UInt32 value3r = 0xAABBCCDD;
            const UInt32 value4r = 0xFFEEDDCC;

            // Write test values.
            using (BinaryWriter writer = new BinaryWriter(_stream, Encoding.UTF8, true))
            {
                writer.Write(value1);
                writer.Write(value2);
                writer.Write(value3);
                writer.Write(value4);
            }

            // Read in system endianness.
            _stream.Position = 0;
            Assert.AreEqual(value1, _stream.ReadUInt32());
            Assert.AreEqual(value2, _stream.ReadUInt32());
            Assert.AreEqual(value3, _stream.ReadUInt32());
            Assert.AreEqual(value4, _stream.ReadUInt32());

            // Read in other endianness.
            _stream.Position = 0;
            Assert.AreEqual(value1r, _stream.ReadUInt32(converter: _reversedConverter));
            Assert.AreEqual(value2r, _stream.ReadUInt32(converter: _reversedConverter));
            Assert.AreEqual(value3r, _stream.ReadUInt32(converter: _reversedConverter));
            Assert.AreEqual(value4r, _stream.ReadUInt32(converter: _reversedConverter));
        }

        [TestMethod]
        public void ReadUInt64()
        {
            const UInt64 value1 = 0x0807060504030201;
            const UInt64 value2 = 0x0102030405060708;
            const UInt64 value3 = 0xFFEEDDCCBBAA9988;
            const UInt64 value4 = 0x8899AABBCCDDEEFF;
            const UInt64 value1r = 0x0102030405060708;
            const UInt64 value2r = 0x0807060504030201;
            const UInt64 value3r = 0x8899AABBCCDDEEFF;
            const UInt64 value4r = 0xFFEEDDCCBBAA9988;

            // Write test values.
            using (BinaryWriter writer = new BinaryWriter(_stream, Encoding.UTF8, true))
            {
                writer.Write(value1);
                writer.Write(value2);
                writer.Write(value3);
                writer.Write(value4);
            }

            // Read in system endianness.
            _stream.Position = 0;
            Assert.AreEqual(value1, _stream.ReadUInt64());
            Assert.AreEqual(value2, _stream.ReadUInt64());
            Assert.AreEqual(value3, _stream.ReadUInt64());
            Assert.AreEqual(value4, _stream.ReadUInt64());

            // Read in other endianness.
            _stream.Position = 0;
            Assert.AreEqual(value1r, _stream.ReadUInt64(converter: _reversedConverter));
            Assert.AreEqual(value2r, _stream.ReadUInt64(converter: _reversedConverter));
            Assert.AreEqual(value3r, _stream.ReadUInt64(converter: _reversedConverter));
            Assert.AreEqual(value4r, _stream.ReadUInt64(converter: _reversedConverter));
        }
    }
}

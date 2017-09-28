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

        private static MemoryStream _stream = new MemoryStream();
        
        private static readonly MemoryStream _data = new MemoryStream(new byte[]
        {
            /*          |50594819 Int32 LE===|  |-1146486526 Int32 LE|  |-285221428 Int32 LE=|
            |67305985 Int32 LE===|  |16909060 Int32 LE===|  |-573785174 Int32 LE=|  |-857870593 Int32 LE=|
            |16909060 Int32 BE===|  |67305985 Int32 BE===|  |-1430532899 Int32 BE|  |-1122868 Int32 BE===|
            |72623859773407745 Int64 LE==================|  |-3684526137413944406 Int64 LE===============|
            |72623859773407745 Int64 BE==================|  |-6144092012763226676 Int64 BE===============|
            |72623859773407745 UInt64 LE=================|  |14762217936295607210 UInt64 LE==============|
            |72623859773407745 UInt64 BE=================|  |12302652060946324940 UInt64 BE==============|*/
            0x01, 0x02, 0x03, 0x04, 0x04, 0x03, 0x02, 0x01, 0xAA, 0xBB, 0xCC, 0xDD, 0xFF, 0xEE, 0xDD, 0xCC
        });
        private const string _unicodeTestString1 = "크레이지레이싱 카트라이더"; // "Crazyracing KartRider"
        private const string _unicodeTestString2 = "äöüßÄÖÜ"; // German characters
        private const string _asciiTestString1 = "Hello World"; // Dennis Ritchie
        private const string _asciiTestString2 = "Trump sucks"; // Truth

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
        public void ReadBoolean() { }

        [TestMethod]
        public void ReadBooleans() { }

        [TestMethod]
        public void ReadDateTime() { }

        [TestMethod]
        public void ReadDateTimes() { }

        [TestMethod]
        public void ReadDecimal() { }

        [TestMethod]
        public void ReadDecimals() { }

        [TestMethod]
        public void ReadDouble() { }

        [TestMethod]
        public void ReadDoubles() { }
        
        [TestMethod]
        public void ReadInt16() { }

        [TestMethod]
        public void ReadInt16s() { }
        
        [TestMethod]
        public void ReadInt32()
        {
            Int32 value;

            // Read as little endian.
            _data.Position = 0;
            value = _data.ReadInt32(converter: ByteConverter.LittleEndian);
            Assert.AreEqual(67305985, value);
            value = _data.ReadInt32(converter: ByteConverter.LittleEndian);
            Assert.AreEqual(16909060, value);
            value = _data.ReadInt32(converter: ByteConverter.LittleEndian);
            Assert.AreEqual(-573785174, value);
            value = _data.ReadInt32(converter: ByteConverter.LittleEndian);
            Assert.AreEqual(-857870593, value);

            // Read as big endian.
            _data.Position = 0;
            value = _data.ReadInt32(converter: ByteConverter.BigEndian);
            Assert.AreEqual(16909060, value);
            value = _data.ReadInt32(converter: ByteConverter.BigEndian);
            Assert.AreEqual(67305985, value);
            value = _data.ReadInt32(converter: ByteConverter.BigEndian);
            Assert.AreEqual(-1430532899, value);
            value = _data.ReadInt32(converter: ByteConverter.BigEndian);
            Assert.AreEqual(-1122868, value);
        }

        [TestMethod]
        public void ReadInt32s()
        {
            Int32[] values;

            // Read as little endian.
            _data.Position = 0;
            values = _data.ReadInt32s(4, converter: ByteConverter.LittleEndian);
            Assert.AreEqual(67305985, values[0]);
            Assert.AreEqual(16909060, values[1]);
            Assert.AreEqual(-573785174, values[2]);
            Assert.AreEqual(-857870593, values[3]);

            // Read as big endian.
            _data.Position = 0;
            values = _data.ReadInt32s(4, converter: ByteConverter.BigEndian);
            Assert.AreEqual(16909060, values[0]);
            Assert.AreEqual(67305985, values[1]);
            Assert.AreEqual(-1430532899, values[2]);
            Assert.AreEqual(-1122868, values[3]);
        }

        [TestMethod]
        public void ReadInt64()
        {
            Int64 value;

            // Read as little endian.
            _data.Position = 0;
            value = _data.ReadInt64(converter: ByteConverter.LittleEndian);
            Assert.AreEqual(72623859773407745, value);
            value = _data.ReadInt64(converter: ByteConverter.LittleEndian);
            Assert.AreEqual(-3684526137413944406, value);

            // Read as big endian.
            _data.Position = 0;
            value = _data.ReadInt64(converter: ByteConverter.BigEndian);
            Assert.AreEqual(72623859773407745, value);
            value = _data.ReadInt64(converter: ByteConverter.BigEndian);
            Assert.AreEqual(-6144092012763226676, value);
        }

        [TestMethod]
        public void ReadInt64s()
        {
            Int64[] values;

            // Read as little endian.
            _data.Position = 0;
            values = _data.ReadInt64s(2, converter: ByteConverter.LittleEndian);
            Assert.AreEqual(72623859773407745, values[0]);
            Assert.AreEqual(-3684526137413944406, values[1]);

            // Read as big endian.
            _data.Position = 0;
            values = _data.ReadInt64s(2, converter: ByteConverter.BigEndian);
            Assert.AreEqual(72623859773407745, values[0]);
            Assert.AreEqual(-6144092012763226676, values[1]);
        }
        
        [TestMethod]
        public void ReadSBytes() { }

        [TestMethod]
        public void ReadSingle() { }

        [TestMethod]
        public void ReadSingles() { }
        
        [TestMethod]
        public void ReadString()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(stream, Encoding.UTF8, true))
                {
                    // .NET variable-length prefix
                    writer.Write(_unicodeTestString1);
                    writer.Write(_unicodeTestString2);
                    // Custom length prefix
                    writer.Write(_unicodeTestString1.Length);
                    writer.Write(Encoding.UTF8.GetBytes(_unicodeTestString1));
                    writer.Write((Int16)_unicodeTestString2.Length);
                    writer.Write(Encoding.UTF8.GetBytes(_unicodeTestString2));
                    // Zero termination
                    writer.Write(Encoding.Unicode.GetBytes(_unicodeTestString1));
                    writer.Write((Int16)0);
                }
                using (BinaryWriter writer = new BinaryWriter(stream, Encoding.ASCII, true))
                {
                    // .NET variable-length prefix
                    writer.Write(_asciiTestString1);
                    writer.Write(_asciiTestString2);
                    // Custom length prefix
                    writer.Write(_asciiTestString1.Length);
                    writer.Write(Encoding.ASCII.GetBytes(_asciiTestString1));
                    writer.Write((Byte)_asciiTestString2.Length);
                    writer.Write(Encoding.ASCII.GetBytes(_asciiTestString2));
                    // Zero termination
                    writer.Write(Encoding.ASCII.GetBytes(_asciiTestString1));
                    writer.Write((Byte)0);
                }
                stream.Position = 0;
                Assert.AreEqual(_unicodeTestString1, stream.ReadString(StringDataFormat.DynamicByteCount, Encoding.UTF8));
                Assert.AreEqual(_unicodeTestString2, stream.ReadString(StringDataFormat.DynamicByteCount, Encoding.UTF8));
                Assert.AreEqual(_unicodeTestString1, stream.ReadString(StringDataFormat.Int32CharCount, Encoding.UTF8));
                Assert.AreEqual(_unicodeTestString2, stream.ReadString(StringDataFormat.Int16CharCount, Encoding.UTF8));
                Assert.AreEqual(_unicodeTestString1, stream.ReadString(StringDataFormat.ZeroTerminated, Encoding.Unicode));
                Assert.AreEqual(_asciiTestString1, stream.ReadString(StringDataFormat.DynamicByteCount, Encoding.ASCII));
                Assert.AreEqual(_asciiTestString2, stream.ReadString(StringDataFormat.DynamicByteCount, Encoding.ASCII));
                Assert.AreEqual(_asciiTestString1, stream.ReadString(StringDataFormat.Int32CharCount, Encoding.ASCII));
                Assert.AreEqual(_asciiTestString2, stream.ReadString(StringDataFormat.ByteCharCount, Encoding.ASCII));
                Assert.AreEqual(_asciiTestString1, stream.ReadString(StringDataFormat.ZeroTerminated, Encoding.ASCII));
            }
        }

        [TestMethod]
        public void ReadUInt16() { }

        [TestMethod]
        public void ReadUInt16s() { }

        [TestMethod]
        public void ReadUInt32() { }

        [TestMethod]
        public void ReadUInt32s() { }
        
        [TestMethod]
        public void ReadUInt64()
        {
            UInt64 value;

            // Read as little endian.
            _data.Position = 0;
            value = _data.ReadUInt64(converter: ByteConverter.LittleEndian);
            Assert.AreEqual(72623859773407745u, value);
            value = _data.ReadUInt64(converter: ByteConverter.LittleEndian);
            Assert.AreEqual(14762217936295607210u, value);

            // Read as big endian.
            _data.Position = 0;
            value = _data.ReadUInt64(converter: ByteConverter.BigEndian);
            Assert.AreEqual(72623859773407745u, value);
            value = _data.ReadUInt64(converter: ByteConverter.BigEndian);
            Assert.AreEqual(12302652060946324940u, value);
        }

        [TestMethod]
        public void ReadUInt64s()
        {
            UInt64[] values;

            // Read as little endian.
            _data.Position = 0;
            values = _data.ReadUInt64s(2, converter: ByteConverter.LittleEndian);
            Assert.AreEqual(72623859773407745u, values[0]);
            Assert.AreEqual(14762217936295607210u, values[1]);

            // Read as big endian.
            _data.Position = 0;
            values = _data.ReadUInt64s(2, converter: ByteConverter.BigEndian);
            Assert.AreEqual(72623859773407745u, values[0]);
            Assert.AreEqual(12302652060946324940u, values[1]);
        }

        [TestMethod]
        public void Seek() { }

        [TestMethod]
        public void TemporarySeek()
        {
            _data.Position = 1;

            // Check read value and restoration of position.
            Int32 value;
            using (_data.TemporarySeek(2, SeekOrigin.Begin))
            {
                value = _data.ReadInt32(ByteConverter.LittleEndian);
            }
            Assert.AreEqual(1, _data.Position);
            Assert.AreEqual(50594819, value);

            Int32[] values;
            using (_data.TemporarySeek(2, SeekOrigin.Begin))
            {
                values = _data.ReadInt32s(3, ByteConverter.LittleEndian);
            }
            Assert.AreEqual(1, _data.Position);
            Assert.AreEqual(50594819, values[0]);
            Assert.AreEqual(-1146486526, values[1]);
            Assert.AreEqual(-285221428, values[2]);
        }
    }
}

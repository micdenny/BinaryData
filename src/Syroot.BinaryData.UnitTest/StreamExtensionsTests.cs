using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Syroot.BinaryData.Extensions;
using Syroot.BinaryData.Serialization;

namespace Syroot.BinaryData.UnitTest
{
    [TestClass]
    public partial class StreamExtensionsTests
    {
        // ---- CLASSES, STRUCTS & ENUMS -------------------------------------------------------------------------------

        private enum TestEnum
        {
            Zero,
            One,
            Two,
            Three,
            Four,
            TwoHundred = 200
        }

        [Flags]
        private enum TestFlags : ushort
        {
            Apple,
            Banana,
            Pineapple,
            Strawberry,
            Peach
        }

        private class TestClass
        {
            [DataOrder(-2), DataArray(3)]
            public byte[] ArrayStuff = new byte[] { 0x01, 0x02, 0x03 };

            [DataOrder(-1), DataArray(3)]
            public string[] AnotherArray = new string[] { "Bla", "Two", "Three" };

            [DataOrder(0)]
            public int X = 0x33330000;

            [DataOrder(2)]
            public byte Y = 0x44;

            [DataOrder(-3), DataString(StringCoding.Int32CharCount)]
            public string Text = "Hello, Test!";

            [DataOrder(1)]
            public TestStruct Struct = new TestStruct { Green = 0x0000FF00, Red = 0xFF000000 };
        }

        private struct TestStruct
        {
            public uint Green;
            public uint Red;
            private uint _blueIgnored;
        }

        // ---- CONSTANTS ----------------------------------------------------------------------------------------------

        private static readonly ByteConverter _reversedConverter
            = BitConverter.IsLittleEndian ? ByteConverter.Big : ByteConverter.Little;

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
        public void ReadWriteObject()
        {
            TestClass origInstance = new TestClass();
            _stream.WriteObject(origInstance, ByteConverter.Big);

            _stream.Position = 0;
            TestClass readInstance = _stream.ReadObject<TestClass>(ByteConverter.Big);

            CollectionAssert.AreEqual(origInstance.ArrayStuff, readInstance.ArrayStuff);
            CollectionAssert.AreEqual(origInstance.AnotherArray, readInstance.AnotherArray);
            Assert.AreEqual(origInstance.X, readInstance.X);
            Assert.AreEqual(origInstance.Y, readInstance.Y);
            Assert.AreEqual(origInstance.Text, readInstance.Text);
            Assert.AreEqual(origInstance.Struct.Green, readInstance.Struct.Green);
            Assert.AreEqual(origInstance.Struct.Red, readInstance.Struct.Red);
        }
        
        [TestMethod]
        public void ReadWriteRawString()
        {
            string expected = "0123456789";

            _stream.Write(expected, StringCoding.Raw, Encoding.UTF8);

            _stream.Position = 0;
            string actual = _stream.ReadString(expected.Length, Encoding.UTF8);

            Assert.AreEqual(expected, actual);
        }
    }
}

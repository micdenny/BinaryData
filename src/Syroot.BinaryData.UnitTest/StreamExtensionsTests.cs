using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Syroot.BinaryData;

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
            [DataOrderAttribute(-2), DataArrayAttribute(3)]
            public byte[] ArrayStuff = new byte[] { 0x01, 0x02, 0x03 };

            [DataOrderAttribute(-1), DataArrayAttribute(3)]
            public string[] AnotherArray = new string[] { "Bla", "Two", "Three" };

            [DataOrderAttribute(0)]
            public int X = 0x33330000;

            [DataOrderAttribute(2)]
            public byte Y = 0x44;

            [DataOrderAttribute(-3), DataString(StringCoding.Int32CharCount)]
            public string Text = "Hello, Test!";

            [DataOrderAttribute(1)]
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
            TestStruct origStruct = new TestStruct
            {
                Green = 123,
                Red = 234
            };
            _stream.WriteObject(origStruct);
            _stream.Position = 0;
            TestStruct readStruct = _stream.ReadObject<TestStruct>();
            Assert.AreEqual(origStruct.Green, readStruct.Green);
            Assert.AreEqual(origStruct.Red, readStruct.Red);

            TestClass origClass = new TestClass();
            _stream.WriteObject(origClass, ByteConverter.Big);
            _stream.Position = 0;
            TestClass readClass = _stream.ReadObject<TestClass>(ByteConverter.Big);
            CollectionAssert.AreEqual(origClass.ArrayStuff, readClass.ArrayStuff);
            CollectionAssert.AreEqual(origClass.AnotherArray, readClass.AnotherArray);
            Assert.AreEqual(origClass.X, readClass.X);
            Assert.AreEqual(origClass.Y, readClass.Y);
            Assert.AreEqual(origClass.Text, readClass.Text);
            Assert.AreEqual(origClass.Struct.Green, readClass.Struct.Green);
            Assert.AreEqual(origClass.Struct.Red, readClass.Struct.Red);
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

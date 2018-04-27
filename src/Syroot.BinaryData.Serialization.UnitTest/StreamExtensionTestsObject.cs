using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Syroot.BinaryData.Serialization.UnitTest
{
    [TestClass]
    public class StreamExtensionTestsObject
    {
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
            [BinaryMember(Order = -2, Length = 3)] public byte[] ArrayStuff = new byte[] { 0x01, 0x02, 0x03 };
            [BinaryMember(Order = -1, Length = 3)] public string[] AnotherArray = new string[] { "Bla", "Two", "Three" };
            [BinaryMember(Order = 0)] public int X = 0x33330000;
            [BinaryMember(Order = 1)] public byte Y = 0x44;
            [BinaryMember(Order = 2, StringFormat = StringCoding.Int32CharCount)] public string Text = "Hello, Test!";
            [BinaryMember(Order = 3)] public TestStruct Struct = new TestStruct { Green = 0x0000FF00, Red = 0xFF000000 };
        }

        private struct TestStruct
        {
            public uint Green;
            public uint Red;
            private uint _blueIgnored;
        }
    }
}

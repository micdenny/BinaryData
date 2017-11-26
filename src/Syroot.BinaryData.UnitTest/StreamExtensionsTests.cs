using System;
using System.IO;
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
            public int X = 0x33330000;
            public byte Y = 0x44;
            [DataString(StringCoding.Int32CharCount)] public string Text = "Hello, Test!";
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
            TestClass testClass = new TestClass();
            _stream.WriteObject(testClass, ByteConverter.Big);

            _stream.Position = 0;
            TestClass readClass = _stream.ReadObject<TestClass>(ByteConverter.Big);

            Assert.AreEqual(testClass.X, readClass.X);
            Assert.AreEqual(testClass.Y, readClass.Y);
            Assert.AreEqual(testClass.Text, readClass.Text);
            Assert.AreEqual(testClass.Struct.Green, readClass.Struct.Green);
            Assert.AreEqual(testClass.Struct.Red, readClass.Struct.Red);
        }
    }
}

using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Syroot.BinaryData.Serialization.UnitTest
{
    [TestClass]
    public class StreamExtensionsTests
    {
        // ---- FIELDS -------------------------------------------------------------------------------------------------

        private static MemoryStream _stream = new MemoryStream();

        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

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

        // ---- CLASSES, STRUCTS & ENUMS -------------------------------------------------------------------------------

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
    }
}

using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Syroot.BinaryData.UnitTest
{
    [TestClass]
    public class StreamExtensionTestsBoolean
    {
        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        [TestMethod]
        public void ReadWriteBoolean()
        {
            Boolean[] values = new Boolean[] { true, false };
            using (MemoryStream stream = new MemoryStream())
            {
                // Prepare test data.
                foreach (Boolean value in values)
                    stream.WriteBoolean(value);
                foreach (Boolean value in values)
                    stream.WriteBoolean(value, BooleanCoding.Byte);
                foreach (Boolean value in values)
                    stream.WriteBoolean(value, BooleanCoding.Word);
                foreach (Boolean value in values)
                    stream.WriteBoolean(value, BooleanCoding.Dword);
                foreach (Boolean value in values)
                    stream.WriteBoolean(value, converter: TestTools.ReverseByteConverter);
                foreach (Boolean value in values)
                    stream.WriteBoolean(value, BooleanCoding.Byte, TestTools.ReverseByteConverter);
                foreach (Boolean value in values)
                    stream.WriteBoolean(value, BooleanCoding.Word, TestTools.ReverseByteConverter);
                foreach (Boolean value in values)
                    stream.WriteBoolean(value, BooleanCoding.Dword, TestTools.ReverseByteConverter);

                // Read test data.
                stream.Position = 0;
                foreach (Boolean value in values)
                    Assert.AreEqual(value, stream.ReadBoolean());
                foreach (Boolean value in values)
                    Assert.AreEqual(value, stream.ReadBoolean(BooleanCoding.Byte));
                foreach (Boolean value in values)
                    Assert.AreEqual(value, stream.ReadBoolean(BooleanCoding.Word));
                foreach (Boolean value in values)
                    Assert.AreEqual(value, stream.ReadBoolean(BooleanCoding.Dword));
                foreach (Boolean value in values)
                    Assert.AreEqual(value, stream.ReadBoolean());
                foreach (Boolean value in values)
                    Assert.AreEqual(value, stream.ReadBoolean(BooleanCoding.Byte));
                foreach (Boolean value in values)
                    Assert.AreEqual(value, stream.ReadBoolean(BooleanCoding.Word));
                foreach (Boolean value in values)
                    Assert.AreEqual(value, stream.ReadBoolean(BooleanCoding.Dword));

                // Read test data as integers.
                stream.Position = 0;
                foreach (Boolean value in values)
                    Assert.AreEqual(value ? 1 : 0, stream.Read1Byte());
                foreach (Boolean value in values)
                    Assert.AreEqual(value ? 1 : 0, stream.Read1Byte());
                foreach (Boolean value in values)
                    Assert.AreEqual(value ? 1 : 0, stream.ReadInt16());
                foreach (Boolean value in values)
                    Assert.AreEqual(value ? 1 : 0, stream.ReadInt32());
                foreach (Boolean value in values)
                    Assert.AreEqual(value ? 1 : 0, stream.Read1Byte());
                foreach (Boolean value in values)
                    Assert.AreEqual(value ? 1 : 0, stream.Read1Byte());
                foreach (Boolean value in values)
                    Assert.AreEqual(value ? 1 : 0, stream.ReadInt16(TestTools.ReverseByteConverter));
                foreach (Boolean value in values)
                    Assert.AreEqual(value ? 1 : 0, stream.ReadInt32(TestTools.ReverseByteConverter));

                // Read test data all at once. 
                stream.Position = 0;
                CollectionAssert.AreEqual(values, stream.ReadBooleans(values.Length));
                CollectionAssert.AreEqual(values, stream.ReadBooleans(values.Length, BooleanCoding.Byte));
                CollectionAssert.AreEqual(values, stream.ReadBooleans(values.Length, BooleanCoding.Word));
                CollectionAssert.AreEqual(values, stream.ReadBooleans(values.Length, BooleanCoding.Dword));
                CollectionAssert.AreEqual(values, stream.ReadBooleans(values.Length));
                CollectionAssert.AreEqual(values, stream.ReadBooleans(values.Length, BooleanCoding.Byte));
                CollectionAssert.AreEqual(values, stream.ReadBooleans(values.Length, BooleanCoding.Word));
                CollectionAssert.AreEqual(values, stream.ReadBooleans(values.Length, BooleanCoding.Dword));
            }
        }
    }
}

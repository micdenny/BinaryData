using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Syroot.BinaryData.UnitTest
{
    [TestClass]
    public class StreamExtensionTestsInt32
    {
        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        [TestMethod]
        public void ReadWriteInt32()
        {
            Int32[] values = new Int32[] { 1234567890, -1234567890, 1, 0, 251258109, Int32.MinValue, Int32.MaxValue };
            using (MemoryStream stream = new MemoryStream())
            {
                // Prepare test data.
                foreach (Int32 value in values)
                    TestTools.WriteInt32(stream, value);
                foreach (Int32 value in values)
                    TestTools.WriteInt32(stream, value, true);

                // Read test data.
                stream.Position = 0;
                foreach (Int32 value in values)
                    Assert.AreEqual(value, stream.ReadInt32());
                foreach (Int32 value in values)
                    Assert.AreEqual(value, stream.ReadInt32(TestTools.ReverseByteConverter));

                // Read test data all at once. 
                stream.Position = 0;
                CollectionAssert.AreEqual(values, stream.ReadInt32s(values.Length));
                CollectionAssert.AreEqual(values, stream.ReadInt32s(values.Length, TestTools.ReverseByteConverter));
            }
        }
    }
}

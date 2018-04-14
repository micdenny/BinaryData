using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Syroot.BinaryData.UnitTest
{
    [TestClass]
    public class StreamExtensionTestsUInt32
    {
        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        [TestMethod]
        public void ReadWriteUInt32()
        {
            UInt32[] values = new UInt32[] { 1234567890, 1, 0, 251258109, UInt32.MinValue, UInt32.MaxValue };
            using (MemoryStream stream = new MemoryStream())
            {
                // Prepare test data.
                foreach (UInt32 value in values)
                    TestTools.WriteUInt32(stream, value);
                foreach (UInt32 value in values)
                    TestTools.WriteUInt32(stream, value, true);

                // Read test data.
                stream.Position = 0;
                foreach (UInt32 value in values)
                    Assert.AreEqual(value, stream.ReadUInt32());
                foreach (UInt32 value in values)
                    Assert.AreEqual(value, stream.ReadUInt32(TestTools.ReverseByteConverter));

                // Read test data all at once. 
                stream.Position = 0;
                CollectionAssert.AreEqual(values, stream.ReadUInt32s(values.Length));
                CollectionAssert.AreEqual(values, stream.ReadUInt32s(values.Length, TestTools.ReverseByteConverter));
            }
        }
    }
}

using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Syroot.BinaryData.UnitTest
{
    [TestClass]
    public class StreamExtensionTestsUInt16
    {
        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        // ---- Read ----

        [TestMethod]
        public void ReadUInt16()
        {
            UInt16[] values = new UInt16[] { 12345, 1, 0, 25125, UInt16.MinValue, UInt16.MaxValue };
            using (MemoryStream stream = new MemoryStream())
            {
                // Prepare test data.
                foreach (UInt16 value in values)
                    TestTools.WriteUInt16(stream, value);
                foreach (UInt16 value in values)
                    TestTools.WriteUInt16(stream, value, true);

                // Read test data.
                stream.Position = 0;
                foreach (UInt16 value in values)
                    Assert.AreEqual(value, stream.ReadUInt16());
                foreach (UInt16 value in values)
                    Assert.AreEqual(value, stream.ReadUInt16(TestTools.ReverseByteConverter));

                // Read test data all at once. 
                stream.Position = 0;
                CollectionAssert.AreEqual(values, stream.ReadUInt16s(values.Length));
                CollectionAssert.AreEqual(values, stream.ReadUInt16s(values.Length, TestTools.ReverseByteConverter));
            }
        }
    }
}

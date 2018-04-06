using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Syroot.BinaryData.UnitTest
{
    [TestClass]
    public partial class StreamExtensionTestsUInt64
    {
        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        // ---- Read ----

        [TestMethod]
        public void ReadUInt64()
        {
            UInt64[] values = new UInt64[] { 1234567890123456789, 1, 0, 2512581093475885631, UInt64.MinValue,
                UInt64.MaxValue };
            using (MemoryStream stream = new MemoryStream())
            {
                // Prepare test data.
                foreach (UInt64 value in values)
                    TestTools.WriteUInt64(stream, value);
                foreach (UInt64 value in values)
                    TestTools.WriteUInt64(stream, value, true);

                // Read test data.
                stream.Position = 0;
                foreach (UInt64 value in values)
                    Assert.AreEqual(value, stream.ReadUInt64());
                foreach (UInt64 value in values)
                    Assert.AreEqual(value, stream.ReadUInt64(TestTools.ReverseByteConverter));

                // Read test data all at once. 
                stream.Position = 0;
                CollectionAssert.AreEqual(values, stream.ReadUInt64s(values.Length));
                CollectionAssert.AreEqual(values, stream.ReadUInt64s(values.Length, TestTools.ReverseByteConverter));
            }
        }
    }
}

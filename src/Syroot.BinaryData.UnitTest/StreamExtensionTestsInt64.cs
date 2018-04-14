using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Syroot.BinaryData.UnitTest
{
    [TestClass]
    public class StreamExtensionTestsInt64
    {
        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        [TestMethod]
        public void ReadWriteInt64()
        {
            Int64[] values = new Int64[] { 1234567890123456789, -1234567890123456789, 1, 0, 2512581093475885631,
                Int64.MinValue, Int64.MaxValue };
            using (MemoryStream stream = new MemoryStream())
            {
                // Prepare test data.
                foreach (Int64 value in values)
                    TestTools.WriteInt64(stream, value);
                foreach (Int64 value in values)
                    TestTools.WriteInt64(stream, value, true);

                // Read test data.
                stream.Position = 0;
                foreach (Int64 value in values)
                    Assert.AreEqual(value, stream.ReadInt64());
                foreach (Int64 value in values)
                    Assert.AreEqual(value, stream.ReadInt64(TestTools.ReverseByteConverter));

                // Read test data all at once. 
                stream.Position = 0;
                CollectionAssert.AreEqual(values, stream.ReadInt64s(values.Length));
                CollectionAssert.AreEqual(values, stream.ReadInt64s(values.Length, TestTools.ReverseByteConverter));
            }
        }
    }
}

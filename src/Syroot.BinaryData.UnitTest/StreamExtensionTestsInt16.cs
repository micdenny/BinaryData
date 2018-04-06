using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Syroot.BinaryData.UnitTest
{
    [TestClass]
    public class StreamExtensionTestsInt16
    {
        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        // ---- Read ----

        [TestMethod]
        public void ReadInt16()
        {
            Int16[] values = new Int16[] { 12345, -12345, 1, 0, 25125, Int16.MinValue, Int16.MaxValue };
            using (MemoryStream stream = new MemoryStream())
            {
                // Prepare test data.
                foreach (Int16 value in values)
                    TestTools.WriteInt16(stream, value);
                foreach (Int16 value in values)
                    TestTools.WriteInt16(stream, value, true);

                // Read test data.
                stream.Position = 0;
                foreach (Int16 value in values)
                    Assert.AreEqual(value, stream.ReadInt16());
                foreach (Int16 value in values)
                    Assert.AreEqual(value, stream.ReadInt16(TestTools.ReverseByteConverter));

                // Read test data all at once. 
                stream.Position = 0;
                CollectionAssert.AreEqual(values, stream.ReadInt16s(values.Length));
                CollectionAssert.AreEqual(values, stream.ReadInt16s(values.Length, TestTools.ReverseByteConverter));
            }
        }
    }
}

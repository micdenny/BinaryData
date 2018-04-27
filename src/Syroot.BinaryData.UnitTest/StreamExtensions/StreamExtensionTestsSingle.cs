using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Syroot.BinaryData.UnitTest
{
    [TestClass]
    public class StreamExtensionTestsSingle
    {
        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        [TestMethod]
        public void ReadWriteSingle()
        {
            Single[] values = new Single[] { 1234.567890f, -1234.567890f, 1f, 0.001f, 0f, Single.MinValue,
                Single.MaxValue, Single.Epsilon, Single.NaN, Single.NegativeInfinity, Single.PositiveInfinity };
            using (MemoryStream stream = new MemoryStream())
            {
                // Prepare test data.
                foreach (Single value in values)
                    TestTools.WriteSingle(stream, value);
                foreach (Single value in values)
                    TestTools.WriteSingle(stream, value, true);

                // Read test data.
                stream.Position = 0;
                foreach (Single value in values)
                    Assert.AreEqual(value, stream.ReadSingle());
                foreach (Single value in values)
                    Assert.AreEqual(value, stream.ReadSingle(TestTools.ReverseByteConverter));

                // Read test data all at once. 
                stream.Position = 0;
                CollectionAssert.AreEqual(values, stream.ReadSingles(values.Length));
                CollectionAssert.AreEqual(values, stream.ReadSingles(values.Length, TestTools.ReverseByteConverter));
            }
        }
    }
}

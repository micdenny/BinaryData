using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Syroot.BinaryData.UnitTest
{
    [TestClass]
    public class StreamExtensionTestsDouble
    {
        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        [TestMethod]
        public void ReadWriteDouble()
        {
            Double[] values = new Double[] { 123456.5125127890, -1234.5325567890, 1, 0, 0.000251258109,
                Double.MinValue, Double.MaxValue, Double.Epsilon, Double.NaN, Double.NegativeInfinity,
                Double.PositiveInfinity };
            using (MemoryStream stream = new MemoryStream())
            {
                // Prepare test data.
                foreach (Double value in values)
                    TestTools.WriteDouble(stream, value);
                foreach (Double value in values)
                    TestTools.WriteDouble(stream, value, true);

                // Read test data.
                stream.Position = 0;
                foreach (Double value in values)
                    Assert.AreEqual(value, stream.ReadDouble());
                foreach (Double value in values)
                    Assert.AreEqual(value, stream.ReadDouble(TestTools.ReverseByteConverter));

                // Read test data all at once. 
                stream.Position = 0;
                CollectionAssert.AreEqual(values, stream.ReadDoubles(values.Length));
                CollectionAssert.AreEqual(values, stream.ReadDoubles(values.Length, TestTools.ReverseByteConverter));
            }
        }
    }
}

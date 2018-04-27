using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Syroot.BinaryData.UnitTest
{
    [TestClass]
    public class StreamExtensionTestsDecimal
    {
        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        [TestMethod]
        public void ReadWriteDecimal()
        {
            Decimal[] values = new Decimal[] { 123.4567890m, -123456.7890m, 1, 0, 2.51258109m, Decimal.MinValue,
                Decimal.MaxValue, Decimal.MinusOne, Decimal.One, Decimal.Zero };
            using (MemoryStream stream = new MemoryStream())
            {
                // Prepare test data.
                foreach (Decimal value in values)
                    stream.WriteDecimal(value);

                // Read test data.
                stream.Position = 0;
                foreach (Decimal value in values)
                    Assert.AreEqual(value, stream.ReadDecimal());

                // Read test data all at once. 
                stream.Position = 0;
                CollectionAssert.AreEqual(values, stream.ReadDecimals(values.Length));
            }
        }
    }
}

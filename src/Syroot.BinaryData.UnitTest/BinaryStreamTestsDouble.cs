using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Syroot.BinaryData.UnitTest
{
    [TestClass]
    public class BinaryStreamTestsDouble
    {
        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        [TestMethod]
        public void ReadWriteDouble()
        {
            Double[] values = new Double[] { 0, -1, 1, -.5, .5, 158.2905, Double.MaxValue, Double.MinValue, Double.Epsilon, Double.NaN, Double.NegativeInfinity,
                Double.PositiveInfinity };

            // Test Binary Stream with default endian initialization.
            using (MemoryStream stream = new MemoryStream())
            using (BinaryStream binaryStream = new BinaryStream(stream))
            {
                // Prepare test data.
                foreach (Double value in values)
                    binaryStream.WriteDouble(value);
                foreach (Double value in values)
                    binaryStream.WriteDouble(value, ByteConverter.Big);

                // Read test data.
                binaryStream.Position = 0;
                foreach (Double value in values)
                    Assert.AreEqual(value, binaryStream.ReadDouble());
                foreach (Double value in values)
                    Assert.AreEqual(value, binaryStream.ReadDouble(ByteConverter.Big));

                // Read test data all at once. 
                binaryStream.Position = 0;
                CollectionAssert.AreEqual(values, binaryStream.ReadDoubles(values.Length));
                CollectionAssert.AreEqual(values, binaryStream.ReadDoubles(values.Length, ByteConverter.Big));

                // Confirm system endian is initialized by default
                binaryStream.Position = 0;
                foreach (Double value in values)
                    Assert.AreEqual(value, binaryStream.ReadDouble(ByteConverter.System));
            }

            // Test Binary Stream with big endian initialization.
            using (MemoryStream stream = new MemoryStream())
            using (BinaryStream binaryStream = new BinaryStream(stream, ByteConverter.Big))
            {
                // Prepare test data.
                foreach (Double value in values)
                    binaryStream.WriteDouble(value);

                // Read test data.
                binaryStream.Position = 0;
                foreach (Double value in values)
                    Assert.AreEqual(value, binaryStream.ReadDouble());

                // Read test data all at once. 
                binaryStream.Position = 0;
                CollectionAssert.AreEqual(values, binaryStream.ReadDoubles(values.Length));

                // Confirm read and write calls are using big endian.
                binaryStream.Position = 0;
                foreach (Double value in values)
                    Assert.AreEqual(value, binaryStream.ReadDouble(ByteConverter.Big));
            }

            // Test Binary Stream with little endian initialization.
            using (MemoryStream stream = new MemoryStream())
            using (BinaryStream binaryStream = new BinaryStream(stream, ByteConverter.Little))
            {
                // Prepare test data.
                foreach (Double value in values)
                    binaryStream.WriteDouble(value);

                // Read test data.
                binaryStream.Position = 0;
                foreach (Double value in values)
                    Assert.AreEqual(value, binaryStream.ReadDouble());

                // Read test data all at once. 
                binaryStream.Position = 0;
                CollectionAssert.AreEqual(values, binaryStream.ReadDoubles(values.Length));

                // Confirm read and write calls are using little endian.
                binaryStream.Position = 0;
                foreach (Double value in values)
                    Assert.AreEqual(value, binaryStream.ReadDouble(ByteConverter.Little));
            }
        }
    }
}
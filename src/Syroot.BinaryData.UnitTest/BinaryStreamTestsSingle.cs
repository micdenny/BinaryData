using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Syroot.BinaryData.UnitTest
{
    [TestClass]
    public class BinaryStreamTestsSingle
    {
        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        [TestMethod]
        public void ReadWriteSingle()
        {
            Single[] values = new Single[] { 0, -1, 1, -.5f, .5f, 158.2905f, Single.MaxValue, Single.MinValue, Single.Epsilon, Single.NaN, Single.NegativeInfinity,
                Single.PositiveInfinity };

            // Test Binary Stream with default endian initialization.
            using (MemoryStream stream = new MemoryStream())
            using (BinaryStream binaryStream = new BinaryStream(stream))
            {
                // Prepare test data.
                foreach (Single value in values)
                    binaryStream.WriteSingle(value);
                foreach (Single value in values)
                    binaryStream.WriteSingle(value, ByteConverter.Big);

                // Read test data.
                binaryStream.Position = 0;
                foreach (Single value in values)
                    Assert.AreEqual(value, binaryStream.ReadSingle());
                foreach (Single value in values)
                    Assert.AreEqual(value, binaryStream.ReadSingle(ByteConverter.Big));

                // Read test data all at once. 
                binaryStream.Position = 0;
                CollectionAssert.AreEqual(values, binaryStream.ReadSingles(values.Length));
                CollectionAssert.AreEqual(values, binaryStream.ReadSingles(values.Length, ByteConverter.Big));

                // Confirm system endian is initialized by default
                binaryStream.Position = 0;
                foreach (Single value in values)
                    Assert.AreEqual(value, binaryStream.ReadSingle(ByteConverter.System));
            }

            // Test Binary Stream with big endian initialization.
            using (MemoryStream stream = new MemoryStream())
            using (BinaryStream binaryStream = new BinaryStream(stream, ByteConverter.Big))
            {
                // Prepare test data.
                foreach (Single value in values)
                    binaryStream.WriteSingle(value);

                // Read test data.
                binaryStream.Position = 0;
                foreach (Single value in values)
                    Assert.AreEqual(value, binaryStream.ReadSingle());

                // Read test data all at once. 
                binaryStream.Position = 0;
                CollectionAssert.AreEqual(values, binaryStream.ReadSingles(values.Length));

                // Confirm read and write calls are using big endian.
                binaryStream.Position = 0;
                foreach (Single value in values)
                    Assert.AreEqual(value, binaryStream.ReadSingle(ByteConverter.Big));
            }

            // Test Binary Stream with little endian initialization.
            using (MemoryStream stream = new MemoryStream())
            using (BinaryStream binaryStream = new BinaryStream(stream, ByteConverter.Little))
            {
                // Prepare test data.
                foreach (Single value in values)
                    binaryStream.WriteSingle(value);

                // Read test data.
                binaryStream.Position = 0;
                foreach (Single value in values)
                    Assert.AreEqual(value, binaryStream.ReadSingle());

                // Read test data all at once. 
                binaryStream.Position = 0;
                CollectionAssert.AreEqual(values, binaryStream.ReadSingles(values.Length));

                // Confirm read and write calls are using little endian.
                binaryStream.Position = 0;
                foreach (Single value in values)
                    Assert.AreEqual(value, binaryStream.ReadSingle(ByteConverter.Little));
            }
        }
    }
}
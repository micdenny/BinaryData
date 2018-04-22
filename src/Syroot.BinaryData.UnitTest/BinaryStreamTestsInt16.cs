using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Syroot.BinaryData.UnitTest
{
    [TestClass]
    public class BinaryStreamTestsInt16
    {
        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        [TestMethod]
        public void ReadWriteInt16()
        {
            Int16[] values = new Int16[] { 0, Int16.MaxValue, Int16.MinValue, 0x0001, 0x1000, 0x10AB };

            // Test Binary Stream with default endian initialization.
            using (MemoryStream stream = new MemoryStream())
            using (BinaryStream binaryStream = new BinaryStream(stream))
            {
                // Prepare test data.
                foreach (Int16 value in values)
                    binaryStream.WriteInt16(value);
                foreach (Int16 value in values)
                    binaryStream.WriteInt16(value, ByteConverter.Big);

                // Read test data.
                binaryStream.Position = 0;
                foreach (Int16 value in values)
                    Assert.AreEqual(value, binaryStream.ReadInt16());
                foreach (Int16 value in values)
                    Assert.AreEqual(value, binaryStream.ReadInt16(ByteConverter.Big));

                // Read test data all at once. 
                binaryStream.Position = 0;
                CollectionAssert.AreEqual(values, binaryStream.ReadInt16s(values.Length));
                CollectionAssert.AreEqual(values, binaryStream.ReadInt16s(values.Length, ByteConverter.Big));

                // Confirm system endian is initialized by default
                binaryStream.Position = 0;
                foreach (Int16 value in values)
                    Assert.AreEqual(value, binaryStream.ReadInt16(ByteConverter.System));
            }

            // Test Binary Stream with big endian initialization.
            using (MemoryStream stream = new MemoryStream())
            using (BinaryStream binaryStream = new BinaryStream(stream, ByteConverter.Big))
            {
                // Prepare test data.
                foreach (Int16 value in values)
                    binaryStream.WriteInt16(value);

                // Read test data.
                binaryStream.Position = 0;
                foreach (Int16 value in values)
                    Assert.AreEqual(value, binaryStream.ReadInt16());

                // Read test data all at once. 
                binaryStream.Position = 0;
                CollectionAssert.AreEqual(values, binaryStream.ReadInt16s(values.Length));

                // Confirm read and write calls are using big endian.
                binaryStream.Position = 0;
                foreach (Int16 value in values)
                    Assert.AreEqual(value, binaryStream.ReadInt16(ByteConverter.Big));
            }

            // Test Binary Stream with little endian initialization.
            using (MemoryStream stream = new MemoryStream())
            using (BinaryStream binaryStream = new BinaryStream(stream, ByteConverter.Little))
            {
                // Prepare test data.
                foreach (Int16 value in values)
                    binaryStream.WriteInt16(value);

                // Read test data.
                binaryStream.Position = 0;
                foreach (Int16 value in values)
                    Assert.AreEqual(value, binaryStream.ReadInt16());

                // Read test data all at once. 
                binaryStream.Position = 0;
                CollectionAssert.AreEqual(values, binaryStream.ReadInt16s(values.Length));

                // Confirm read and write calls are using little endian.
                binaryStream.Position = 0;
                foreach (Int16 value in values)
                    Assert.AreEqual(value, binaryStream.ReadInt16(ByteConverter.Little));
            }
        }
    }
}
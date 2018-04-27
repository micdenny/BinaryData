using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Syroot.BinaryData.UnitTest
{
    [TestClass]
    public class BinaryStreamTestsInt64
    {
        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        [TestMethod]
        public void ReadWriteInt64()
        {
            Int64[] values = new Int64[] { 0, Int64.MaxValue, Int64.MinValue, 0x0000000000000001, 0x1000000000000000, 0x10AEC49B870D53F2 };

            // Test Binary Stream with default endian initialization.
            using (MemoryStream stream = new MemoryStream())
            using (BinaryStream binaryStream = new BinaryStream(stream))
            {
                // Prepare test data.
                foreach (Int64 value in values)
                    binaryStream.WriteInt64(value);
                foreach (Int64 value in values)
                    binaryStream.WriteInt64(value, ByteConverter.Big);

                // Read test data.
                binaryStream.Position = 0;
                foreach (Int64 value in values)
                    Assert.AreEqual(value, binaryStream.ReadInt64());
                foreach (Int64 value in values)
                    Assert.AreEqual(value, binaryStream.ReadInt64(ByteConverter.Big));

                // Read test data all at once. 
                binaryStream.Position = 0;
                CollectionAssert.AreEqual(values, binaryStream.ReadInt64s(values.Length));
                CollectionAssert.AreEqual(values, binaryStream.ReadInt64s(values.Length, ByteConverter.Big));

                // Confirm system endian is initialized by default
                binaryStream.Position = 0;
                foreach (Int64 value in values)
                    Assert.AreEqual(value, binaryStream.ReadInt64(ByteConverter.System));
            }

            // Test Binary Stream with big endian initialization.
            using (MemoryStream stream = new MemoryStream())
            using (BinaryStream binaryStream = new BinaryStream(stream, ByteConverter.Big))
            {
                // Prepare test data.
                foreach (Int64 value in values)
                    binaryStream.WriteInt64(value);

                // Read test data.
                binaryStream.Position = 0;
                foreach (Int64 value in values)
                    Assert.AreEqual(value, binaryStream.ReadInt64());

                // Read test data all at once. 
                binaryStream.Position = 0;
                CollectionAssert.AreEqual(values, binaryStream.ReadInt64s(values.Length));

                // Confirm read and write calls are using big endian.
                binaryStream.Position = 0;
                foreach (Int64 value in values)
                    Assert.AreEqual(value, binaryStream.ReadInt64(ByteConverter.Big));
            }

            // Test Binary Stream with little endian initialization.
            using (MemoryStream stream = new MemoryStream())
            using (BinaryStream binaryStream = new BinaryStream(stream, ByteConverter.Little))
            {
                // Prepare test data.
                foreach (Int64 value in values)
                    binaryStream.WriteInt64(value);

                // Read test data.
                binaryStream.Position = 0;
                foreach (Int64 value in values)
                    Assert.AreEqual(value, binaryStream.ReadInt64());

                // Read test data all at once. 
                binaryStream.Position = 0;
                CollectionAssert.AreEqual(values, binaryStream.ReadInt64s(values.Length));

                // Confirm read and write calls are using little endian.
                binaryStream.Position = 0;
                foreach (Int64 value in values)
                    Assert.AreEqual(value, binaryStream.ReadInt64(ByteConverter.Little));
            }
        }
    }
}
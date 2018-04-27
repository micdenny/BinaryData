using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Syroot.BinaryData.UnitTest
{
    [TestClass]
    public class BinaryStreamTestsUInt64
    {
        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        [TestMethod]
        public void ReadWriteUInt64()
        {
            UInt64[] values = new UInt64[] { 0, UInt64.MaxValue, 0x000000000000000F, 0xF000000000000000, 0x10AEC49B870D53F2 };

            // Test Binary Stream with default endian initialization.
            using (MemoryStream stream = new MemoryStream())
            using (BinaryStream binaryStream = new BinaryStream(stream))
            {
                // Prepare test data.
                foreach (UInt64 value in values)
                    binaryStream.WriteUInt64(value);
                foreach (UInt64 value in values)
                    binaryStream.WriteUInt64(value, ByteConverter.Big);

                // Read test data.
                binaryStream.Position = 0;
                foreach (UInt64 value in values)
                    Assert.AreEqual(value, binaryStream.ReadUInt64());
                foreach (UInt64 value in values)
                    Assert.AreEqual(value, binaryStream.ReadUInt64(ByteConverter.Big));

                // Read test data all at once. 
                binaryStream.Position = 0;
                CollectionAssert.AreEqual(values, binaryStream.ReadUInt64s(values.Length));
                CollectionAssert.AreEqual(values, binaryStream.ReadUInt64s(values.Length, ByteConverter.Big));

                // Confirm system endian is initialized by default
                binaryStream.Position = 0;
                foreach (UInt64 value in values)
                    Assert.AreEqual(value, binaryStream.ReadUInt64(ByteConverter.System));
            }

            // Test Binary Stream with big endian initialization.
            using (MemoryStream stream = new MemoryStream())
            using (BinaryStream binaryStream = new BinaryStream(stream, ByteConverter.Big))
            {
                // Prepare test data.
                foreach (UInt64 value in values)
                    binaryStream.WriteUInt64(value);

                // Read test data.
                binaryStream.Position = 0;
                foreach (UInt64 value in values)
                    Assert.AreEqual(value, binaryStream.ReadUInt64());

                // Read test data all at once. 
                binaryStream.Position = 0;
                CollectionAssert.AreEqual(values, binaryStream.ReadUInt64s(values.Length));

                // Confirm read and write calls are using big endian.
                binaryStream.Position = 0;
                foreach (UInt64 value in values)
                    Assert.AreEqual(value, binaryStream.ReadUInt64(ByteConverter.Big));
            }

            // Test Binary Stream with little endian initialization.
            using (MemoryStream stream = new MemoryStream())
            using (BinaryStream binaryStream = new BinaryStream(stream, ByteConverter.Little))
            {
                // Prepare test data.
                foreach (UInt64 value in values)
                    binaryStream.WriteUInt64(value);

                // Read test data.
                binaryStream.Position = 0;
                foreach (UInt64 value in values)
                    Assert.AreEqual(value, binaryStream.ReadUInt64());

                // Read test data all at once. 
                binaryStream.Position = 0;
                CollectionAssert.AreEqual(values, binaryStream.ReadUInt64s(values.Length));

                // Confirm read and write calls are using little endian.
                binaryStream.Position = 0;
                foreach (UInt64 value in values)
                    Assert.AreEqual(value, binaryStream.ReadUInt64(ByteConverter.Little));
            }
        }
    }
}
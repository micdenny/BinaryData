using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Syroot.BinaryData.UnitTest
{
    [TestClass]
    public class BinaryStreamTestsUInt32
    {
    // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        [TestMethod]
        public void ReadWriteUInt32()
        {
            UInt32[] values = new UInt32[] { 0, 0xFFFFFFFF, 0x0000000F, 0xF0000000, 0x10AB8700 };

            // Test Binary Stream with default endian initialization.
            using (MemoryStream stream = new MemoryStream())
            using (BinaryStream binaryStream = new BinaryStream(stream))
            {
                // Prepare test data.
                foreach (UInt32 value in values)
                    binaryStream.WriteUInt32(value);
                foreach (UInt32 value in values)
                    binaryStream.WriteUInt32(value, ByteConverter.Big);

                // Read test data.
                binaryStream.Position = 0;
                foreach (UInt32 value in values)
                    Assert.AreEqual(value, binaryStream.ReadUInt32());
                foreach (UInt32 value in values)
                    Assert.AreEqual(value, binaryStream.ReadUInt32(ByteConverter.Big));

                // Read test data all at once. 
                binaryStream.Position = 0;
                CollectionAssert.AreEqual(values, binaryStream.ReadUInt32s(values.Length));
                CollectionAssert.AreEqual(values, binaryStream.ReadUInt32s(values.Length, ByteConverter.Big));

                // Confirm system endian is initialized by default
                binaryStream.Position = 0;
                foreach (UInt32 value in values)
                    Assert.AreEqual(value, binaryStream.ReadUInt32(ByteConverter.System));
            }

            // Test Binary Stream with big endian initialization.
            using (MemoryStream stream = new MemoryStream())
            using (BinaryStream binaryStream = new BinaryStream(stream, ByteConverter.Big))
            {
                // Prepare test data.
                foreach (UInt32 value in values)
                    binaryStream.WriteUInt32(value);

                // Read test data.
                binaryStream.Position = 0;
                foreach (UInt32 value in values)
                    Assert.AreEqual(value, binaryStream.ReadUInt32());

                // Read test data all at once. 
                binaryStream.Position = 0;
                CollectionAssert.AreEqual(values, binaryStream.ReadUInt32s(values.Length));

                // Confirm read and write calls are using big endian.
                binaryStream.Position = 0;
                foreach (UInt32 value in values)
                    Assert.AreEqual(value, binaryStream.ReadUInt32(ByteConverter.Big));
            }

            // Test Binary Stream with little endian initialization.
            using (MemoryStream stream = new MemoryStream())
            using (BinaryStream binaryStream = new BinaryStream(stream, ByteConverter.Little))
            {
                // Prepare test data.
                foreach (UInt32 value in values)
                    binaryStream.WriteUInt32(value);

                // Read test data.
                binaryStream.Position = 0;
                foreach (UInt32 value in values)
                    Assert.AreEqual(value, binaryStream.ReadUInt32());

                // Read test data all at once. 
                binaryStream.Position = 0;
                CollectionAssert.AreEqual(values, binaryStream.ReadUInt32s(values.Length));

                // Confirm read and write calls are using little endian.
                binaryStream.Position = 0;
                foreach (UInt32 value in values)
                    Assert.AreEqual(value, binaryStream.ReadUInt32(ByteConverter.Little));
            }
        }
    }
} 
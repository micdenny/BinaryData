using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Syroot.BinaryData.UnitTest
{
    [TestClass]
    public class BinaryStreamTestsUInt16
    {
        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        [TestMethod]
        public void ReadWriteUInt16()
        {
            UInt16[] values = new UInt16[] { 0x10AB, 0xFFFF, 0x000F, 0xF000, 0 };

            // Test Binary Stream with default endian initialization.
            using (MemoryStream stream = new MemoryStream())
            using (BinaryStream binaryStream = new BinaryStream(stream))
            {
                // Prepare test data.
                foreach (UInt16 value in values)
                    binaryStream.WriteUInt16(value);
                foreach (UInt16 value in values)
                    binaryStream.WriteUInt16(value, ByteConverter.Big);

                // Read test data.
                binaryStream.Position = 0;
                foreach (UInt16 value in values)
                    Assert.AreEqual(value, binaryStream.ReadUInt16());
                foreach (UInt16 value in values)
                    Assert.AreEqual(value, binaryStream.ReadUInt16(ByteConverter.Big));

                // Read test data all at once. 
                binaryStream.Position = 0;
                CollectionAssert.AreEqual(values, binaryStream.ReadUInt16s(values.Length));
                CollectionAssert.AreEqual(values, binaryStream.ReadUInt16s(values.Length, ByteConverter.Big));

                // Confirm system endian is initialized by default
                binaryStream.Position = 0;
                foreach (UInt16 value in values)
                    Assert.AreEqual(value, binaryStream.ReadUInt16(ByteConverter.System));
            }

            // Test Binary Stream with big endian initialization.
            using (MemoryStream stream = new MemoryStream())
            using (BinaryStream binaryStream = new BinaryStream(stream, ByteConverter.Big))
            {
                // Prepare test data.
                foreach (UInt16 value in values)
                    binaryStream.WriteUInt16(value);

                // Read test data.
                binaryStream.Position = 0;
                foreach (UInt16 value in values)
                    Assert.AreEqual(value, binaryStream.ReadUInt16());

                // Read test data all at once. 
                binaryStream.Position = 0;
                CollectionAssert.AreEqual(values, binaryStream.ReadUInt16s(values.Length));

                // Confirm read and write calls are using big endian.
                binaryStream.Position = 0;
                foreach (UInt16 value in values)
                    Assert.AreEqual(value, binaryStream.ReadUInt16(ByteConverter.Big));
            }

            // Test Binary Stream with little endian initialization.
            using (MemoryStream stream = new MemoryStream())
            using (BinaryStream binaryStream = new BinaryStream(stream, ByteConverter.Little))
            {
                // Prepare test data.
                foreach (UInt16 value in values)
                    binaryStream.WriteUInt16(value);

                // Read test data.
                binaryStream.Position = 0;
                foreach (UInt16 value in values)
                    Assert.AreEqual(value, binaryStream.ReadUInt16());

                // Read test data all at once. 
                binaryStream.Position = 0;
                CollectionAssert.AreEqual(values, binaryStream.ReadUInt16s(values.Length));

                // Confirm read and write calls are using little endian.
                binaryStream.Position = 0;
                foreach (UInt16 value in values)
                    Assert.AreEqual(value, binaryStream.ReadUInt16(ByteConverter.Little));
            }
        }
    }
}
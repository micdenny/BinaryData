using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Syroot.BinaryData.UnitTest
{
    [TestClass]
    public class BinaryStreamTestsInt32
    {
        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        [TestMethod]
        public void ReadWriteInt32()
        {
            Int32[] values = new Int32[] { 0, Int32.MaxValue, Int32.MinValue, 0x00000001, 0x10000000, 0x10AB8700 };

            // Test Binary Stream with default endian initialization.
            using (MemoryStream stream = new MemoryStream())
            using (BinaryStream binaryStream = new BinaryStream(stream))
            {
                // Prepare test data.
                foreach (Int32 value in values)
                    binaryStream.WriteInt32(value);
                foreach (Int32 value in values)
                    binaryStream.WriteInt32(value, ByteConverter.Big);

                // Read test data.
                binaryStream.Position = 0;
                foreach (Int32 value in values)
                    Assert.AreEqual(value, binaryStream.ReadInt32());
                foreach (Int32 value in values)
                    Assert.AreEqual(value, binaryStream.ReadInt32(ByteConverter.Big));

                // Read test data all at once. 
                binaryStream.Position = 0;
                CollectionAssert.AreEqual(values, binaryStream.ReadInt32s(values.Length));
                CollectionAssert.AreEqual(values, binaryStream.ReadInt32s(values.Length, ByteConverter.Big));

                // Confirm system endian is initialized by default
                binaryStream.Position = 0;
                foreach (Int32 value in values)
                    Assert.AreEqual(value, binaryStream.ReadInt32(ByteConverter.System));
            }

            // Test Binary Stream with big endian initialization.
            using (MemoryStream stream = new MemoryStream())
            using (BinaryStream binaryStream = new BinaryStream(stream, ByteConverter.Big))
            {
                // Prepare test data.
                foreach (Int32 value in values)
                    binaryStream.WriteInt32(value);

                // Read test data.
                binaryStream.Position = 0;
                foreach (Int32 value in values)
                    Assert.AreEqual(value, binaryStream.ReadInt32());

                // Read test data all at once. 
                binaryStream.Position = 0;
                CollectionAssert.AreEqual(values, binaryStream.ReadInt32s(values.Length));

                // Confirm read and write calls are using big endian.
                binaryStream.Position = 0;
                foreach (Int32 value in values)
                    Assert.AreEqual(value, binaryStream.ReadInt32(ByteConverter.Big));
            }

            // Test Binary Stream with little endian initialization.
            using (MemoryStream stream = new MemoryStream())
            using (BinaryStream binaryStream = new BinaryStream(stream, ByteConverter.Little))
            {
                // Prepare test data.
                foreach (Int32 value in values)
                    binaryStream.WriteInt32(value);

                // Read test data.
                binaryStream.Position = 0;
                foreach (Int32 value in values)
                    Assert.AreEqual(value, binaryStream.ReadInt32());

                // Read test data all at once. 
                binaryStream.Position = 0;
                CollectionAssert.AreEqual(values, binaryStream.ReadInt32s(values.Length));

                // Confirm read and write calls are using little endian.
                binaryStream.Position = 0;
                foreach (Int32 value in values)
                    Assert.AreEqual(value, binaryStream.ReadInt32(ByteConverter.Little));
            }
        }
    }
}
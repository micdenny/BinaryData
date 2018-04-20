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
            }
        }
    }
} 
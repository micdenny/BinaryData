using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Syroot.BinaryData.UnitTest
{
    [TestClass]
    public class BinaryStreamTestsBoolean
    {
        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        [TestMethod]
        public void ReadWriteBoolean()
        {
            Boolean[] values = new Boolean[] { true, false };
            BooleanCoding[] encodings = new BooleanCoding[] { BooleanCoding.Byte, BooleanCoding.Word, BooleanCoding.Dword };
            ByteConverter[] endianness = new ByteConverter[] { ByteConverter.Big, ByteConverter.Little, ByteConverter.System };

            using (MemoryStream stream = new MemoryStream())
            using (BinaryStream binaryStream = new BinaryStream(stream))
            {
                // Prepare test data.

                // Test with no arguments.
                foreach (Boolean value in values)
                    binaryStream.WriteBoolean(value);

                // Test with different encodings.
                foreach (BooleanCoding encoding in encodings)
                    foreach (Boolean value in values)
                        binaryStream.WriteBoolean(value, encoding);

                // Test with different encodings and endianness.
                foreach (ByteConverter endian in endianness)
                    foreach (BooleanCoding encoding in encodings)
                        foreach (Boolean value in values)
                            binaryStream.WriteBoolean(value, coding: encoding, converter: endian);

                // Test with different endianness.
                foreach (ByteConverter endian in endianness)
                    foreach (Boolean value in values)
                        binaryStream.WriteBoolean(value, converter: endian);

                // Read test data.

                binaryStream.Position = 0;

                // Test with no arguments.
                foreach (Boolean value in values)
                    Assert.AreEqual(value, binaryStream.ReadBoolean());

                // Test with different encodings.
                foreach (BooleanCoding encoding in encodings)
                    foreach (Boolean value in values)
                        Assert.AreEqual(value, binaryStream.ReadBoolean(encoding));

                // Test with different encodings and endianness.
                foreach (ByteConverter endian in endianness)
                    foreach (BooleanCoding encoding in encodings)
                        foreach (Boolean value in values)
                            Assert.AreEqual(value, binaryStream.ReadBoolean(coding: encoding));

                // Test with different endianness.
                foreach (ByteConverter endian in endianness)
                    foreach (Boolean value in values)
                        Assert.AreEqual(value, binaryStream.ReadBoolean());


                // Read test data as integers.
                binaryStream.Position = 0;

                // Test with no arguments.
                foreach (Boolean value in values)
                    Assert.AreEqual(value ? 1 : 0, binaryStream.Read1Byte());

                // Test with different encodings.
                foreach (BooleanCoding encoding in encodings)
                {
                    if (encoding == BooleanCoding.Byte)
                    {
                        foreach (Boolean value in values)
                            Assert.AreEqual(value ? 1 : 0, binaryStream.Read1Byte());
                    }
                    else if (encoding == BooleanCoding.Word)
                    {
                        foreach (Boolean value in values)
                            Assert.AreEqual(value ? 1 : 0, binaryStream.ReadInt16());
                    }
                    else if (encoding == BooleanCoding.Dword)
                    {
                        foreach (Boolean value in values)
                            Assert.AreEqual(value ? 1 : 0, binaryStream.ReadInt32());
                    }
                }
                
                // Test with different encodings and endianness.
                foreach (ByteConverter endian in endianness)
                {
                    foreach (BooleanCoding encoding in encodings)
                    {
                        if (encoding == BooleanCoding.Byte)
                        {
                            foreach (Boolean value in values)
                                Assert.AreEqual(value ? 1 : 0, binaryStream.Read1Byte());
                        }
                        else if (encoding == BooleanCoding.Word)
                        {
                            foreach (Boolean value in values)
                                Assert.AreEqual(value ? 1 : 0, binaryStream.ReadInt16(endian));
                        }
                        else if (encoding == BooleanCoding.Dword)
                        {
                            foreach (Boolean value in values)
                                Assert.AreEqual(value ? 1 : 0, binaryStream.ReadInt32(endian));
                        }
                    }
                }

                // Test with different endianness.
                foreach (Boolean value in values)
                    Assert.AreEqual(value ? 1 : 0, binaryStream.Read1Byte());


                // Read test data all at once. 

                binaryStream.Position = 0;

                // Test with no arguments.
                CollectionAssert.AreEqual(values, binaryStream.ReadBooleans(values.Length));

                // Test with different encodings.
                foreach (BooleanCoding encoding in encodings)
                    CollectionAssert.AreEqual(values, binaryStream.ReadBooleans(values.Length, encoding));

                // Test with different encodings and endianness.
                foreach (ByteConverter endian in endianness)
                    foreach (BooleanCoding encoding in encodings)
                        CollectionAssert.AreEqual(values, binaryStream.ReadBooleans(values.Length, encoding));

                // Test with different endianness.
                CollectionAssert.AreEqual(values, binaryStream.ReadBooleans(values.Length));
            }
        }
    }
}

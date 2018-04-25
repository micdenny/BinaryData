using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Syroot.BinaryData.UnitTest
{
    [TestClass]
    public class BinaryStreamTestsString
    {
        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        [TestMethod]
        public void ReadWriteString()
        {
            String[] values = new String[]
            {
                String.Empty,
                "Hallo, Kitty!",
                "abcdefghijklmnopqrstuvwxyz",
                "ABCDEFGHIJKLMNOPQRSTUVWXYZ",
                "0123456789?!;:'[]{}-_*&^%$#@",
                "🐔🐓🥚🐣🐤🐥🐻🦆"
            };

            StringCoding[] encodings = new StringCoding[] { StringCoding.ByteCharCount, StringCoding.Int16CharCount, StringCoding.Int32CharCount, StringCoding.VariableByteCount, StringCoding.ZeroTerminated };
            ByteConverter[] endianness = new ByteConverter[] { ByteConverter.Big, ByteConverter.Little, ByteConverter.System };

            using (MemoryStream stream = new MemoryStream())
            {
                // Prepare test data.
                foreach (String value in values)
                    stream.WriteString(value);

                foreach (StringCoding encoding in encodings)
                    foreach (String value in values)
                        stream.WriteString(value, encoding);

                foreach (ByteConverter endian in endianness)
                    foreach (String value in values)
                        stream.WriteString(value, converter: endian);

                foreach (ByteConverter endian in endianness)
                    foreach (StringCoding encoding in encodings)
                        foreach (String value in values)
                            stream.WriteString(value, encoding, converter: endian);

                // Read test data.
                stream.Position = 0;
                foreach (String value in values)
                    Assert.AreEqual(value, stream.ReadString());

                foreach (StringCoding encoding in encodings)
                    foreach (String value in values)
                        Assert.AreEqual(value, stream.ReadString(encoding));

                foreach (ByteConverter endian in endianness)
                    foreach (String value in values)
                        Assert.AreEqual(value, stream.ReadString(converter: endian));

                foreach (ByteConverter endian in endianness)
                    foreach (StringCoding encoding in encodings)
                        foreach (String value in values)
                            Assert.AreEqual(value, stream.ReadString(encoding, converter: endian));

                // Read test data all at once. 
                stream.Position = 0;
                CollectionAssert.AreEqual(values, stream.ReadStrings(values.Length));
                
                foreach (StringCoding encoding in encodings)
                    CollectionAssert.AreEqual(values, stream.ReadStrings(values.Length, encoding));

                foreach (ByteConverter endian in endianness)
                    CollectionAssert.AreEqual(values, stream.ReadStrings(values.Length, converter: endian));

                foreach (ByteConverter endian in endianness)
                    foreach (StringCoding encoding in encodings)
                        CollectionAssert.AreEqual(values, stream.ReadStrings(values.Length, encoding, converter: endian));
            }
        }
    }
}

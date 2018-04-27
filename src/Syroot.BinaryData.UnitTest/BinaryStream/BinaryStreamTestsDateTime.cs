using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Syroot.BinaryData.UnitTest
{
    [TestClass]
    public class BinaryStreamTestsDateTime
    {
        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        [TestMethod]
        public void ReadWriteDateTime()
        {
            DateTime[] values = new DateTime[]
            {
                new DateTime(1998, 12, 24, 12, 22, 13),
                DateTime.Now,
                DateTime.MinValue,
                DateTime.MaxValue
            };

            ByteConverter[] endianness = new ByteConverter[]
            {
                ByteConverter.Big,
                ByteConverter.Little,
                ByteConverter.System
            };

            using (MemoryStream stream = new MemoryStream())
            using (BinaryStream binaryStream = new BinaryStream(stream))
            {
                // Prepare test data.
                foreach (DateTime value in values)
                    binaryStream.WriteDateTime(value);

                foreach (DateTime value in values)
                    binaryStream.WriteDateTime(value, DateTimeCoding.NetTicks);

                foreach (ByteConverter endian in endianness)
                    foreach (DateTime value in values)
                        binaryStream.WriteDateTime(value, converter: endian);

                foreach (ByteConverter endian in endianness)
                    foreach (DateTime value in values)
                        binaryStream.WriteDateTime(value, DateTimeCoding.NetTicks, endian);

                // Read test data.
                binaryStream.Position = 0;

                foreach (DateTime value in values)
                    Assert.AreEqual(value, binaryStream.ReadDateTime());

                foreach (DateTime value in values)
                    Assert.AreEqual(value, binaryStream.ReadDateTime(DateTimeCoding.NetTicks));

                foreach (ByteConverter endian in endianness)
                    foreach (DateTime value in values)
                        Assert.AreEqual(value, binaryStream.ReadDateTime(converter: endian));

                foreach (ByteConverter endian in endianness)
                    foreach (DateTime value in values)
                        Assert.AreEqual(value, binaryStream.ReadDateTime(DateTimeCoding.NetTicks, endian));

                // Read test data all at once. 
                binaryStream.Position = 0;

                CollectionAssert.AreEqual(values, binaryStream.ReadDateTimes(values.Length));

                CollectionAssert.AreEqual(values, binaryStream.ReadDateTimes(values.Length, DateTimeCoding.NetTicks));

                foreach (ByteConverter endian in endianness)
                    CollectionAssert.AreEqual(values, binaryStream.ReadDateTimes(values.Length, converter: endian));

                foreach (ByteConverter endian in endianness)
                    CollectionAssert.AreEqual(values, binaryStream.ReadDateTimes(values.Length, DateTimeCoding.NetTicks, endian));
            }
        }
    }
}
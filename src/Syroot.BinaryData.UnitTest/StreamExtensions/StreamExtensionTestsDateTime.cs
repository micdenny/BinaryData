using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Syroot.BinaryData.UnitTest
{
    [TestClass]
    public class StreamExtensionTestsDateTime
    {
        // ---- CONSTANTS ----------------------------------------------------------------------------------------------

        private static readonly DateTime _cTimeMin = new DateTime(1970, 1, 1);
        private static readonly DateTime _cTimeMax = _cTimeMin.AddSeconds(UInt32.MaxValue);

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

            using (MemoryStream stream = new MemoryStream())
            {
                // Prepare test data.
                foreach (DateTime value in values)
                    stream.WriteDateTime(value);
                foreach (DateTime value in values)
                    stream.WriteDateTime(value, DateTimeCoding.NetTicks);
                foreach (DateTime value in values)
                    stream.WriteDateTime(value, converter: TestTools.ReverseByteConverter);
                foreach (DateTime value in values)
                    stream.WriteDateTime(value, DateTimeCoding.NetTicks, TestTools.ReverseByteConverter);

                // Read test data.
                stream.Position = 0;
                foreach (DateTime value in values)
                    Assert.AreEqual(value, stream.ReadDateTime());
                foreach (DateTime value in values)
                    Assert.AreEqual(value, stream.ReadDateTime(DateTimeCoding.NetTicks));
                foreach (DateTime value in values)
                    Assert.AreEqual(value, stream.ReadDateTime(converter: TestTools.ReverseByteConverter));
                foreach (DateTime value in values)
                    Assert.AreEqual(value, stream.ReadDateTime(DateTimeCoding.NetTicks, TestTools.ReverseByteConverter));

                // Read test data all at once. 
                stream.Position = 0;
                CollectionAssert.AreEqual(values, stream.ReadDateTimes(values.Length));
                CollectionAssert.AreEqual(values, stream.ReadDateTimes(values.Length, DateTimeCoding.NetTicks));
                CollectionAssert.AreEqual(values, stream.ReadDateTimes(values.Length, converter: TestTools.ReverseByteConverter));
                CollectionAssert.AreEqual(values, stream.ReadDateTimes(values.Length, DateTimeCoding.NetTicks, TestTools.ReverseByteConverter));
            }
        }

        [TestMethod]
        public void ReadWriteDateTimeCTime()
        {
            DateTime[] values = new DateTime[]
            {
                new DateTime(1998, 12, 24, 12, 22, 13),
                new DateTime(2018, 04, 14, 11, 02, 59),
                _cTimeMin,
                _cTimeMax,
            };

            using (MemoryStream stream = new MemoryStream())
            {
                // Prepare test data.
                foreach (DateTime value in values)
                    stream.WriteDateTime(value, DateTimeCoding.CTime);
                foreach (DateTime value in values)
                    stream.WriteDateTime(value, DateTimeCoding.CTime, TestTools.ReverseByteConverter);

                // Read test data.
                stream.Position = 0;
                foreach (DateTime value in values)
                    Assert.AreEqual(value, stream.ReadDateTime(DateTimeCoding.CTime));
                foreach (DateTime value in values)
                    Assert.AreEqual(value, stream.ReadDateTime(DateTimeCoding.CTime, TestTools.ReverseByteConverter));

                // Read test data all at once. 
                stream.Position = 0;
                CollectionAssert.AreEqual(values, stream.ReadDateTimes(values.Length, DateTimeCoding.CTime));
                CollectionAssert.AreEqual(values, stream.ReadDateTimes(values.Length, DateTimeCoding.CTime, TestTools.ReverseByteConverter));
            }
        }

        [TestMethod]
        public void ReadWriteDateTimeCTime64()
        {
            DateTime[] values = new DateTime[]
            {
                new DateTime(1998, 12, 24, 12, 22, 13),
                new DateTime(2018, 04, 14, 11, 02, 59),
                _cTimeMin,
                _cTimeMax
            };

            using (MemoryStream stream = new MemoryStream())
            {
                // Prepare test data.
                foreach (DateTime value in values)
                    stream.WriteDateTime(value, DateTimeCoding.CTime64);
                foreach (DateTime value in values)
                    stream.WriteDateTime(value, DateTimeCoding.CTime64, TestTools.ReverseByteConverter);

                // Read test data.
                stream.Position = 0;
                foreach (DateTime value in values)
                    Assert.AreEqual(value, stream.ReadDateTime(DateTimeCoding.CTime64));
                foreach (DateTime value in values)
                    Assert.AreEqual(value, stream.ReadDateTime(DateTimeCoding.CTime64, TestTools.ReverseByteConverter));

                // Read test data all at once. 
                stream.Position = 0;
                CollectionAssert.AreEqual(values, stream.ReadDateTimes(values.Length, DateTimeCoding.CTime64));
                CollectionAssert.AreEqual(values, stream.ReadDateTimes(values.Length, DateTimeCoding.CTime64, TestTools.ReverseByteConverter));
            }
        }

        [TestMethod]
        public void ReadWriteDateTimeCTime64OutOfRange()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                // Prepare test data.
                stream.WriteUInt64(UInt64.MaxValue);

                // Read test data.
                stream.Position = 0;
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => stream.ReadDateTime(DateTimeCoding.CTime64));
            }
        }
    }
}

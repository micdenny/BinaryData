using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Syroot.BinaryData.UnitTest
{
    [TestClass]
    public class ByteConverterTests
    {
        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        [TestMethod]
        public void DetectSystemEndianness()
        {
            if (BitConverter.IsLittleEndian)
                Assert.AreEqual(ByteConverter.System.Endian, Endian.Little);
            else
                Assert.AreEqual(ByteConverter.System.Endian, Endian.Big);
        }

        [TestMethod]
        public void GetDecimal()
        {
            foreach (Decimal value in new Decimal[] { 123.4567890m, -12345.67890m, 1, 0, Decimal.MinValue,
                Decimal.MaxValue })
            {
                Int32[] decimalBits = Decimal.GetBits(value);
                byte[] decimalBytes = new byte[decimalBits.Length * sizeof(Int32)];
                Buffer.BlockCopy(decimalBits, 0, decimalBytes, 0, decimalBytes.Length);

                byte[] byteConvert = new byte[decimalBytes.Length];
                ByteConverter.GetBytes(value, byteConvert);
                CollectionAssert.AreEqual(decimalBytes, byteConvert);
            }
        }

        [TestMethod]
        public void GetBytesDouble()
        {
            foreach (Double value in new Double[] { 123.4567890f, -12345.67890f, 1, 0, Double.MinValue, Double.MaxValue,
                Double.Epsilon, Double.NaN, Double.NegativeInfinity, Double.PositiveInfinity })
            {
                byte[] bitConvert = BitConverter.GetBytes(value);

                byte[] byteConvert = new byte[bitConvert.Length];
                ByteConverter.System.GetBytes(value, byteConvert);
                CollectionAssert.AreEqual(bitConvert, byteConvert);

                Array.Reverse(bitConvert);
                GetReversedConverter().GetBytes(value, byteConvert);
                CollectionAssert.AreEqual(bitConvert, byteConvert);
            }
        }

        [TestMethod]
        public void GetBytesInt16()
        {
            foreach (Int16 value in new Int16[] { 12345, -12345, 1, 0, 25125, Int16.MinValue, Int16.MaxValue })
            {
                byte[] bitConvert = BitConverter.GetBytes(value);

                byte[] byteConvert = new byte[bitConvert.Length];
                ByteConverter.System.GetBytes(value, byteConvert);
                CollectionAssert.AreEqual(bitConvert, byteConvert);

                Array.Reverse(bitConvert);
                GetReversedConverter().GetBytes(value, byteConvert);
                CollectionAssert.AreEqual(bitConvert, byteConvert);
            }
        }

        [TestMethod]
        public void GetBytesInt32()
        {
            foreach (Int32 value in new Int32[] { 1234567890, -1234567890, 1, 0, 251258109, Int32.MinValue,
                Int32.MaxValue })
            {
                byte[] bitConvert = BitConverter.GetBytes(value);

                byte[] byteConvert = new byte[bitConvert.Length];
                ByteConverter.System.GetBytes(value, byteConvert);
                CollectionAssert.AreEqual(bitConvert, byteConvert);

                Array.Reverse(bitConvert);
                GetReversedConverter().GetBytes(value, byteConvert);
                CollectionAssert.AreEqual(bitConvert, byteConvert);
            }
        }

        [TestMethod]
        public void GetBytesInt64()
        {
            foreach (Int64 value in new Int64[] { 1234567890123456789, -1234567890123456789, 1, 0, 2512581093475885631,
                Int64.MinValue, Int64.MaxValue })
            {
                byte[] bitConvert = BitConverter.GetBytes(value);

                byte[] byteConvert = new byte[bitConvert.Length];
                ByteConverter.System.GetBytes(value, byteConvert);
                CollectionAssert.AreEqual(bitConvert, byteConvert);

                Array.Reverse(bitConvert);
                GetReversedConverter().GetBytes(value, byteConvert);
                CollectionAssert.AreEqual(bitConvert, byteConvert);
            }
        }

        [TestMethod]
        public void GetBytesSingle()
        {
            foreach (Single value in new Single[] { 123.4567890f, -12345.67890f, 1, 0, Single.MinValue, Single.MaxValue,
                Single.Epsilon, Single.NaN, Single.NegativeInfinity, Single.PositiveInfinity })
            {
                byte[] bitConvert = BitConverter.GetBytes(value);

                byte[] byteConvert = new byte[bitConvert.Length];
                ByteConverter.System.GetBytes(value, byteConvert);
                CollectionAssert.AreEqual(bitConvert, byteConvert);

                Array.Reverse(bitConvert);
                GetReversedConverter().GetBytes(value, byteConvert);
                CollectionAssert.AreEqual(bitConvert, byteConvert);
            }
        }

        [TestMethod]
        public void GetBytesUInt16()
        {
            foreach (UInt16 value in new UInt16[] { 12345, 1, 0, 25125, UInt16.MinValue, UInt16.MaxValue })
            {
                byte[] bitConvert = BitConverter.GetBytes(value);

                byte[] byteConvert = new byte[bitConvert.Length];
                ByteConverter.System.GetBytes(value, byteConvert);
                CollectionAssert.AreEqual(bitConvert, byteConvert);

                Array.Reverse(bitConvert);
                GetReversedConverter().GetBytes(value, byteConvert);
                CollectionAssert.AreEqual(bitConvert, byteConvert);
            }
        }

        [TestMethod]
        public void GetBytesUInt32()
        {
            foreach (UInt32 value in new UInt32[] { 1234567890, 1, 0, 251258109, UInt32.MinValue, UInt32.MaxValue })
            {
                byte[] bitConvert = BitConverter.GetBytes(value);

                byte[] byteConvert = new byte[bitConvert.Length];
                ByteConverter.System.GetBytes(value, byteConvert);
                CollectionAssert.AreEqual(bitConvert, byteConvert);

                Array.Reverse(bitConvert);
                GetReversedConverter().GetBytes(value, byteConvert);
                CollectionAssert.AreEqual(bitConvert, byteConvert);
            }
        }

        [TestMethod]
        public void GetBytesUInt64()
        {
            foreach (UInt64 value in new UInt64[] { 1234567890123456789, 1, 0, 2512581093475885631, UInt64.MinValue,
                UInt64.MaxValue })
            {
                byte[] bitConvert = BitConverter.GetBytes(value);

                byte[] byteConvert = new byte[bitConvert.Length];
                ByteConverter.System.GetBytes(value, byteConvert);
                CollectionAssert.AreEqual(bitConvert, byteConvert);

                Array.Reverse(bitConvert);
                GetReversedConverter().GetBytes(value, byteConvert);
                CollectionAssert.AreEqual(bitConvert, byteConvert);
            }
        }

        [TestMethod]
        public void ToDecimal()
        {
            foreach (Decimal value in new Decimal[] { 123.4567890m, -12345.67890m, 1, 0, Decimal.MinValue,
                Decimal.MaxValue })
            {
                Int32[] decimalBits = Decimal.GetBits(value);
                byte[] bytes = new byte[decimalBits.Length * sizeof(Int32)];
                Buffer.BlockCopy(decimalBits, 0, bytes, 0, bytes.Length);

                Decimal parsedValue = ByteConverter.ToDecimal(bytes);
                Assert.AreEqual(value, parsedValue);
            }
        }
        [TestMethod]
        public void ToDouble()
        {
            foreach (Double value in new Double[] { 123.4567890f, -12345.67890f, 1, 0, Double.MinValue, Double.MaxValue,
                Double.Epsilon, Double.NaN, Double.NegativeInfinity, Double.PositiveInfinity })
            {
                byte[] bytes = BitConverter.GetBytes(value);

                Double parsedValue = ByteConverter.System.ToDouble(bytes);
                Assert.AreEqual(value, parsedValue);

                Array.Reverse(bytes);
                parsedValue = GetReversedConverter().ToDouble(bytes);
                Assert.AreEqual(value, parsedValue);
            }
        }

        [TestMethod]
        public void ToInt16()
        {
            foreach (Int16 value in new Int16[] { 12345, -12345, 1, 0, 25125, Int16.MinValue, Int16.MaxValue })
            {
                byte[] bytes = BitConverter.GetBytes(value);

                Int16 parsedValue = ByteConverter.System.ToInt16(bytes);
                Assert.AreEqual(value, parsedValue);

                Array.Reverse(bytes);
                parsedValue = GetReversedConverter().ToInt16(bytes);
                Assert.AreEqual(value, parsedValue);
            }
        }

        [TestMethod]
        public void ToInt32()
        {
            foreach (Int32 value in new Int32[] { 1234567890, -1234567890, 1, 0, 251258109, Int32.MinValue,
                Int32.MaxValue })
            {
                byte[] bytes = BitConverter.GetBytes(value);

                Int32 parsedValue = ByteConverter.System.ToInt32(bytes);
                Assert.AreEqual(value, parsedValue);

                Array.Reverse(bytes);
                parsedValue = GetReversedConverter().ToInt32(bytes);
                Assert.AreEqual(value, parsedValue);
            }
        }

        [TestMethod]
        public void ToInt64()
        {
            foreach (Int64 value in new Int64[] { 1234567890123456789, -1234567890123456789, 1, 0, 2512581093475885631,
                Int64.MinValue, Int64.MaxValue })
            {
                byte[] bytes = BitConverter.GetBytes(value);

                Int64 parsedValue = ByteConverter.System.ToInt64(bytes);
                Assert.AreEqual(value, parsedValue);

                Array.Reverse(bytes);
                parsedValue = GetReversedConverter().ToInt64(bytes);
                Assert.AreEqual(value, parsedValue);
            }
        }

        [TestMethod]
        public void ToSingle()
        {
            foreach (Single value in new Single[] { 123.4567890f, -12345.67890f, 1, 0, Single.MinValue, Single.MaxValue,
                Single.Epsilon, Single.NaN, Single.NegativeInfinity, Single.PositiveInfinity })
            {
                byte[] bytes = BitConverter.GetBytes(value);

                Single parsedValue = ByteConverter.System.ToSingle(bytes);
                Assert.AreEqual(value, parsedValue);

                Array.Reverse(bytes);
                parsedValue = GetReversedConverter().ToSingle(bytes);
                Assert.AreEqual(value, parsedValue);
            }
        }

        [TestMethod]
        public void ToUInt16()
        {
            foreach (UInt16 value in new UInt16[] { 12345, 1, 0, 25125, UInt16.MinValue, UInt16.MaxValue })
            {
                byte[] bytes = BitConverter.GetBytes(value);

                UInt16 parsedValue = ByteConverter.System.ToUInt16(bytes);
                Assert.AreEqual(value, parsedValue);

                Array.Reverse(bytes);
                parsedValue = GetReversedConverter().ToUInt16(bytes);
                Assert.AreEqual(value, parsedValue);
            }
        }

        [TestMethod]
        public void ToUInt32()
        {
            foreach (UInt32 value in new UInt32[] { 1234567890, 1, 0, 251258109, UInt32.MinValue, UInt32.MaxValue })
            {
                byte[] bytes = BitConverter.GetBytes(value);

                UInt32 parsedValue = ByteConverter.System.ToUInt32(bytes);
                Assert.AreEqual(value, parsedValue);

                Array.Reverse(bytes);
                parsedValue = GetReversedConverter().ToUInt32(bytes);
                Assert.AreEqual(value, parsedValue);
            }
        }

        [TestMethod]
        public void ToUInt64()
        {
            foreach (UInt64 value in new UInt64[] { 1234567890123456789, 1, 0, 2512581093475885631, UInt64.MinValue,
                UInt64.MaxValue })
            {
                byte[] bytes = BitConverter.GetBytes(value);

                UInt64 parsedValue = ByteConverter.System.ToUInt64(bytes);
                Assert.AreEqual(value, parsedValue);

                Array.Reverse(bytes);
                parsedValue = GetReversedConverter().ToUInt64(bytes);
                Assert.AreEqual(value, parsedValue);
            }
        }

        // ---- METHODS (PRIVATE) --------------------------------------------------------------------------------------

        private ByteConverter GetReversedConverter()
        {
            return ByteConverter.System.Endian == Endian.Little ? ByteConverter.Big : ByteConverter.Little;
        }
    }
}

using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Syroot.BinaryData.UnitTest
{
    [TestClass]
    public class StreamExtensionTestsString
    {
        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        [TestMethod]
        public void ReadWriteString()
        {
            String[] values = new String[]
            {
                String.Empty,
                "Hello, World!",
                "Donald Drumpf",
                "Testing «ταБЬℓσ»: 1<2 & 4+1>3, now 20% off!",
                "Smiley balls 👨👩👧👧",
                "٩(-̮̮̃-̃)۶ ٩(●̮̮̃•̃)۶ ٩(͡๏̯͡๏)۶ ٩(-̮̮̃•̃)."
            };

            using (MemoryStream stream = new MemoryStream())
            {
                // Prepare test data.
                foreach (String value in values)
                    stream.WriteString(value);
                foreach (String value in values)
                    stream.WriteString(value, StringCoding.ByteCharCount);
                foreach (String value in values)
                    stream.WriteString(value, StringCoding.Int16CharCount);
                foreach (String value in values)
                    stream.WriteString(value, StringCoding.Int32CharCount);
                foreach (String value in values)
                    stream.WriteString(value, StringCoding.VariableByteCount);
                foreach (String value in values)
                    stream.WriteString(value, StringCoding.ZeroTerminated);

                foreach (String value in values)
                    stream.WriteString(value, converter: TestTools.ReverseByteConverter);
                foreach (String value in values)
                    stream.WriteString(value, StringCoding.ByteCharCount, converter: TestTools.ReverseByteConverter);
                foreach (String value in values)
                    stream.WriteString(value, StringCoding.Int16CharCount, converter: TestTools.ReverseByteConverter);
                foreach (String value in values)
                    stream.WriteString(value, StringCoding.Int32CharCount, converter: TestTools.ReverseByteConverter);
                foreach (String value in values)
                    stream.WriteString(value, StringCoding.VariableByteCount, converter: TestTools.ReverseByteConverter);
                foreach (String value in values)
                    stream.WriteString(value, StringCoding.ZeroTerminated, converter: TestTools.ReverseByteConverter);

                // Read test data.
                stream.Position = 0;
                foreach (String value in values)
                    Assert.AreEqual(value, stream.ReadString());
                foreach (String value in values)
                    Assert.AreEqual(value, stream.ReadString(StringCoding.ByteCharCount));
                foreach (String value in values)
                    Assert.AreEqual(value, stream.ReadString(StringCoding.Int16CharCount));
                foreach (String value in values)
                    Assert.AreEqual(value, stream.ReadString(StringCoding.Int32CharCount));
                foreach (String value in values)
                    Assert.AreEqual(value, stream.ReadString(StringCoding.VariableByteCount));
                foreach (String value in values)
                    Assert.AreEqual(value, stream.ReadString(StringCoding.ZeroTerminated));
                foreach (String value in values)
                    Assert.AreEqual(value, stream.ReadString(converter: TestTools.ReverseByteConverter));
                foreach (String value in values)
                    Assert.AreEqual(value, stream.ReadString(StringCoding.ByteCharCount, converter: TestTools.ReverseByteConverter));
                foreach (String value in values)
                    Assert.AreEqual(value, stream.ReadString(StringCoding.Int16CharCount, converter: TestTools.ReverseByteConverter));
                foreach (String value in values)
                    Assert.AreEqual(value, stream.ReadString(StringCoding.Int32CharCount, converter: TestTools.ReverseByteConverter));
                foreach (String value in values)
                    Assert.AreEqual(value, stream.ReadString(StringCoding.VariableByteCount, converter: TestTools.ReverseByteConverter));
                foreach (String value in values)
                    Assert.AreEqual(value, stream.ReadString(StringCoding.ZeroTerminated, converter: TestTools.ReverseByteConverter));

                // Read test data all at once. 
                stream.Position = 0;
                CollectionAssert.AreEqual(values, stream.ReadStrings(values.Length));
                CollectionAssert.AreEqual(values, stream.ReadStrings(values.Length, StringCoding.ByteCharCount));
                CollectionAssert.AreEqual(values, stream.ReadStrings(values.Length, StringCoding.Int16CharCount));
                CollectionAssert.AreEqual(values, stream.ReadStrings(values.Length, StringCoding.Int32CharCount));
                CollectionAssert.AreEqual(values, stream.ReadStrings(values.Length, StringCoding.VariableByteCount));
                CollectionAssert.AreEqual(values, stream.ReadStrings(values.Length, StringCoding.ZeroTerminated));

                CollectionAssert.AreEqual(values, stream.ReadStrings(values.Length, converter: TestTools.ReverseByteConverter));
                CollectionAssert.AreEqual(values, stream.ReadStrings(values.Length, StringCoding.ByteCharCount, converter: TestTools.ReverseByteConverter));
                CollectionAssert.AreEqual(values, stream.ReadStrings(values.Length, StringCoding.Int16CharCount, converter: TestTools.ReverseByteConverter));
                CollectionAssert.AreEqual(values, stream.ReadStrings(values.Length, StringCoding.Int32CharCount, converter: TestTools.ReverseByteConverter));
                CollectionAssert.AreEqual(values, stream.ReadStrings(values.Length, StringCoding.VariableByteCount, converter: TestTools.ReverseByteConverter));
                CollectionAssert.AreEqual(values, stream.ReadStrings(values.Length, StringCoding.ZeroTerminated, converter: TestTools.ReverseByteConverter));
            }
        }

        [TestMethod]
        public void ReadWriteStringRaw()
        {
            String[] values = new String[]
            {
                String.Empty,
                "Hello, World!",
                "Donald Drumpf",
                "Testing «ταБЬℓσ»: 1<2 & 4+1>3, now 20% off!",
                "Smiley balls 👨👩👧👧",
                "٩(-̮̮̃-̃)۶ ٩(●̮̮̃•̃)۶ ٩(͡๏̯͡๏)۶ ٩(-̮̮̃•̃)."
            };
            String[] valuesMultiple = new String[]
            {
                "Hello!",
                "Drumpf",
                "«ταБ»1",
                "👨👩👧", // Length isn't code points, but size
                "٩(-̮̮̃" // Length isn't code points, but size
            };

            using (MemoryStream stream = new MemoryStream())
            {
                // Prepare test data.
                foreach (String value in values)
                    stream.WriteString(value, StringCoding.Raw);
                stream.WriteStrings(valuesMultiple, StringCoding.Raw);

                // Read test data.
                stream.Position = 0;
                foreach (String value in values)
                    Assert.AreEqual(value, stream.ReadString(value.Length));

                // Read test data all at once.
                string[] read = stream.ReadStrings(valuesMultiple.Length, valuesMultiple[0].Length);
                CollectionAssert.AreEqual(valuesMultiple, read);
            }
        }
    }
}

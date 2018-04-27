using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Syroot.BinaryData.UnitTest
{
    [TestClass]
    public class StreamExtensionTestsByte
    {
        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        [TestMethod]
        public void ReadWriteByte()
        {
            Byte[] values = new Byte[] { 164, 23, 1, 0, Byte.MinValue, Byte.MaxValue };
            using (MemoryStream stream = new MemoryStream())
            {
                // Prepare test data.
                foreach (Byte value in values)
                    stream.WriteByte(value);

                // Read test data.
                stream.Position = 0;
                foreach (Byte value in values)
                    Assert.AreEqual(value, stream.Read1Byte());

                // Read test data all at once. 
                stream.Position = 0;
                CollectionAssert.AreEqual(values, stream.ReadBytes(values.Length));
            }
        }
    }
}

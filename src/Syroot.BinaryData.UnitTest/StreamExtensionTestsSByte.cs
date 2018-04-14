using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Syroot.BinaryData.UnitTest
{
    [TestClass]
    public class StreamExtensionTestsSByte
    {
        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        [TestMethod]
        public void ReadWriteSByte()
        {
            SByte[] values = new SByte[] { -124, 23, 1, 0, SByte.MinValue, SByte.MaxValue };
            using (MemoryStream stream = new MemoryStream())
            {
                // Prepare test data.
                foreach (SByte value in values)
                    stream.WriteSByte(value);

                // Read test data.
                stream.Position = 0;
                foreach (SByte value in values)
                    Assert.AreEqual(value, stream.ReadSByte());

                // Read test data all at once. 
                stream.Position = 0;
                CollectionAssert.AreEqual(values, stream.ReadSBytes(values.Length));
            }
        }
    }
}

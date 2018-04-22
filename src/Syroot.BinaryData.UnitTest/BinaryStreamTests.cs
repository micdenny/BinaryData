using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Syroot.BinaryData.UnitTest
{
    [TestClass]
    public class BinaryStreamTests
    {
        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        [TestMethod]
        public void Seek()
        {
            using (MemoryStream stream = new MemoryStream())
            using (BinaryStream binaryStream = new BinaryStream(stream))
            {
                // Prepare stream by writing data.
                binaryStream.WriteUInt32(0xFFFFFFFF);

                // Check that the stream position can be set from the Beginning of the stream.
                binaryStream.Seek(0, SeekOrigin.Begin);
                Assert.AreEqual(0, binaryStream.Position);

                // Check that the stream can seek forwards.
                binaryStream.Seek(2);
                Assert.AreEqual(2, binaryStream.Position);

                // Check that the stream can seek backwards.
                binaryStream.Seek(-2);
                Assert.AreEqual(0, binaryStream.Position);

                // Check that the stream can seek forwards from the current origin.
                binaryStream.Seek(2, SeekOrigin.Current);
                Assert.AreEqual(2, binaryStream.Position);

                // Check that the stream can seek to End of the stream.
                binaryStream.Seek(0, SeekOrigin.End);
                Assert.AreEqual(binaryStream.Length, binaryStream.Position);

                // Check that the stream can seek backwards from the end of the stream.
                binaryStream.Seek(-2, SeekOrigin.End);
                Assert.AreEqual(binaryStream.Length - 2, binaryStream.Position);

                // Check that the stream can seek forwards from the beginning origin.
                binaryStream.Seek(2, SeekOrigin.Begin);
                Assert.AreEqual(2, binaryStream.Position);

            }


        }

    }
}

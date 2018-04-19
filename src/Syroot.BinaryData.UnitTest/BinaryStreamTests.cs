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
                Assert.AreEqual(0, binaryStream.Position);

                stream.Seek(2);
                Assert.AreEqual(2, binaryStream.Position);

                stream.Seek(2);
                Assert.AreEqual(4, binaryStream.Position);
            }
        }

    }
}

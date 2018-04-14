using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Syroot.BinaryData.UnitTest
{
    [TestClass]
    public class StreamExtensionTests
    {
        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        [TestMethod]
        public void Align()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                stream.Position = 13;
                Assert.AreEqual(16, stream.Align(4));

                stream.Position = 13;
                Assert.AreEqual(12, stream.Align(-4));

                stream.Position = 13;
                Assert.AreEqual(13, stream.Align(1));

                stream.Position = 12;
                Assert.AreEqual(12, stream.Align(4));

                stream.Position = 12;
                Assert.AreEqual(12, stream.Align(-4));

                stream.Position = 12;
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => stream.Align(0));
            }
        }
    }
}

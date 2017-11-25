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
            {
                Assert.AreEqual(ByteConverter.System.ByteOrder, Endian.Little);
            }
            else
            {
                Assert.AreEqual(ByteConverter.System.ByteOrder, Endian.Big);
            }
        }
    }
}

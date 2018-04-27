using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Syroot.BinaryData.UnitTest
{
    [TestClass]
    public class StreamExtensionTests7BitInt32
    {
        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        [TestMethod]
        public void ReadWrite7BitInt32()
        {
            Int32[] values = new Int32[] { 1234567890, -1234567890, 1, 0, 251258109, Int32.MinValue, Int32.MaxValue };
            using (MemoryStream stream = new MemoryStream())
            {
                // Prepare test data.
                foreach (Int32 value in values)
                    stream.Write7BitInt32(value);

                // Read test data.
                stream.Position = 0;
                foreach (Int32 value in values)
                    Assert.AreEqual(value, stream.Read7BitInt32());

                // Read test data all at once. 
                stream.Position = 0;
                CollectionAssert.AreEqual(values, stream.Read7BitInt32s(values.Length));
            }
        }
    }
}

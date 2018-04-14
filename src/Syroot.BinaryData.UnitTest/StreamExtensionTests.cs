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

        [TestMethod]
        public void EndOfStream()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                Assert.AreEqual(true, stream.IsEndOfStream());

                stream.Write(1);
                Assert.AreEqual(true, stream.IsEndOfStream());

                stream.Position = 0;
                Assert.AreEqual(false, stream.IsEndOfStream());

                stream.Position = 2;
                Assert.AreEqual(false, stream.IsEndOfStream());

                stream.SetLength(0);
                Assert.AreEqual(true, stream.IsEndOfStream());
            }
        }

        [TestMethod]
        public void Move()
        {
            // Prepare test data.
            MemoryStream baseStream = new MemoryStream();
            for (int i = 1; i < 10; i++)
                baseStream.WriteInt32(i);
            baseStream.Position = 0;

            // Test the stream.
            using (NonSeekableStream stream = new NonSeekableStream(baseStream))
            {
                Assert.AreEqual(0, stream.BaseStream.Position);

                Assert.AreEqual(1, stream.ReadInt32());
                Assert.AreEqual(sizeof(Int32), stream.BaseStream.Position);

                stream.Move(sizeof(Int32));
                Assert.AreEqual(sizeof(Int32) * 2, stream.BaseStream.Position);

                Assert.AreEqual(3, stream.ReadInt32());

                Assert.ThrowsException<NotSupportedException>(() => stream.Move(-4));
            }
        }

        [TestMethod]
        public void Seek()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                Assert.AreEqual(0, stream.Position);

                stream.Seek(4);
                Assert.AreEqual(4, stream.Position);

                stream.Seek(4);
                Assert.AreEqual(8, stream.Position);
            }
        }

        [TestMethod]
        public void TemporarySeek()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                Assert.AreEqual(0, stream.Position);

                using (stream.TemporarySeek(4))
                {
                    Assert.AreEqual(4, stream.Position);
                }
                Assert.AreEqual(0, stream.Position);

                using (stream.TemporarySeek(5))
                {
                    Assert.AreEqual(5, stream.Position);

                    using (stream.TemporarySeek(5))
                    {
                        Assert.AreEqual(10, stream.Position);

                        using (stream.TemporarySeek(3, SeekOrigin.Begin))
                        {
                            Assert.AreEqual(3, stream.Position);
                        }

                        Assert.AreEqual(10, stream.Position);
                    }

                    Assert.AreEqual(5, stream.Position);
                }

                Assert.AreEqual(0, stream.Position);
            }
        }
    }
}

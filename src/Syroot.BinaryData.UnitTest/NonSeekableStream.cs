using System;
using System.IO;

namespace Syroot.BinaryData.UnitTest
{
    internal class NonSeekableStream : Stream
    {
        // ---- CONSTRUCTORS & DESTRUCTOR ------------------------------------------------------------------------------

        internal NonSeekableStream(Stream baseStream)
        {
            BaseStream = baseStream;
        }

        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public Stream BaseStream { get; }

        public override bool CanRead => true;

        public override bool CanSeek => false;

        public override bool CanWrite => true;

        public override long Length => throw new NotSupportedException();

        public override long Position
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        public override void Flush() => BaseStream.Flush();

        public override int Read(byte[] buffer, int offset, int count) => BaseStream.Read(buffer, offset, count);

        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();

        public override void SetLength(long value) => throw new NotSupportedException();

        public override void Write(byte[] buffer, int offset, int count) => BaseStream.Write(buffer, offset, count);
    }
}

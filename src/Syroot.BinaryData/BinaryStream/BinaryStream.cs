using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Syroot.BinaryData
{
    /// <summary>
    /// Represents a wrapper around a <see cref="Stream"/> to read from and write to it with default data format
    /// configurations.
    /// </summary>
    [DebuggerDisplay(nameof(BinaryStream) + ", " + nameof(Position) + "={" + nameof(Position) + "}")]
    public partial class BinaryStream : Stream
    {
        // ---- FIELDS -------------------------------------------------------------------------------------------------

        private ByteConverter _byteConverter;
        private Encoding _encoding;
        private bool _leaveOpen;
        private bool _disposed;

        // ---- CONSTRUCTORS -------------------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryStream"/> class with the given default configuration.
        /// </summary>
        /// <param name="baseStream">The output stream.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use. Defaults to
        /// <see cref="ByteConverter.System"/>.</param>
        /// <param name="encoding">The character encoding to use. Defaults to <see cref="Encoding.UTF8"/>.</param>
        /// <param name="booleanCoding">The <see cref="BinaryData.BooleanCoding"/> data format to use  for
        /// <see cref="Boolean"/> values.</param>
        /// <param name="dateTimeCoding">The <see cref="BinaryData.DateTimeCoding"/> data format to use for
        /// <see cref="DateTime"/> values.</param>
        /// <param name="stringCoding">The <see cref="BinaryData.StringCoding"/> data format to use for
        /// <see cref="String"/> values.</param>
        /// <param name="leaveOpen"><c>true</c> to leave the base stream open after the <see cref="BinaryStream"/>
        /// object is disposed; otherwise <c>false</c>.</param>
        /// <exception cref="ArgumentException">The stream does not support writing or is already closed.</exception>
        /// <exception cref="ArgumentNullException">output is null.</exception>
        public BinaryStream(Stream baseStream, ByteConverter converter = null, Encoding encoding = null,
            BooleanCoding booleanCoding = BooleanCoding.Byte, DateTimeCoding dateTimeCoding = DateTimeCoding.NetTicks,
            StringCoding stringCoding = StringCoding.VariableByteCount, bool leaveOpen = false)
        {
            BaseStream = baseStream;
            ByteConverter = converter;
            Encoding = encoding;
            BooleanCoding = booleanCoding;
            DateTimeCoding = dateTimeCoding;
            StringCoding = stringCoding;
            _leaveOpen = leaveOpen;
        }

        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        /// <summary>
        /// Gets the underlying <see cref="Stream"/> the instance works on.
        /// </summary>
        public Stream BaseStream { get; }

        /// <summary>
        /// Gets a value indicating whether the underlying stream supports reading.
        /// </summary>
        public override bool CanRead
            => BaseStream.CanRead;

        /// <summary>
        /// Gets a value indicating whether the underlying stream supports seeking.
        /// </summary>
        public override bool CanSeek
            => BaseStream.CanSeek;

        /// <summary>
        /// Gets a value indicating whether the underlying stream supports writing.
        /// </summary>
        public override bool CanWrite
            => BaseStream.CanWrite;

        /// <summary>
        /// Gets a value indicating whether the end of the stream has been reached.
        /// </summary>
        public bool EndOfStream
            => BaseStream.IsEndOfStream();

        /// <summary>
        /// Gets or sets the length in bytes of the stream in bytes.
        /// </summary>
        public override long Length
            => BaseStream.Length;

        /// <summary>
        /// Gets or sets the position within the current stream. This is a shortcut to the base stream Position
        /// property.
        /// </summary>
        public override long Position
        {
            get => BaseStream.Position;
            set => BaseStream.Position = value;
        }

        // ---- Configuration ----

        /// <summary>
        /// Gets or sets the <see cref="ByteConverter"/> instance used to parse multibyte binary data with.
        /// Setting this value to <c>null</c> will restore the default <see cref="ByteConverter.Big"/>.
        /// </summary>
        public ByteConverter ByteConverter
        {
            get => _byteConverter;
            set => _byteConverter = value ?? ByteConverter.System;
        }

        /// <summary>
        /// Gets or sets the encoding used for string related operations where no other encoding has been provided.
        /// Setting this value to <c>null</c> will restore the default <see cref="Encoding.UTF8"/>.
        /// </summary>
        public Encoding Encoding
        {
            get => _encoding;
            set => _encoding = value ?? Encoding.UTF8;
        }

        /// <summary>
        /// Gets the <see cref="BinaryData.BooleanCoding"/> to use for <see cref="Boolean"/> values.
        /// </summary>
        public BooleanCoding BooleanCoding { get; set; }

        /// <summary>
        /// Gets the <see cref="BinaryData.DateTimeCoding"/> to use for <see cref="DateTime"/> values.
        /// </summary>
        public DateTimeCoding DateTimeCoding { get; set; }

        /// <summary>
        /// Gets the <see cref="BinaryData.StringCoding"/> to use for <see cref="String"/> values.
        /// </summary>
        public StringCoding StringCoding { get; set; }

        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        /// <summary>
        /// Clears all buffers for this stream and causes any buffered data to be written to the underlying stream.
        /// </summary>
        public override void Flush()
            => BaseStream.Flush();

        /// <summary>
        /// Reads a sequence of bytes from the underlying stream and advances the position within the stream by the
        /// number of bytes read.
        /// </summary>
        /// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte
        /// array with the values between offset and (offset + count - 1) replaced by the bytes read from the underlying
        /// stream.</param>
        /// <param name="offset">The zero-based byte offset in <paramref name="buffer"/> at which to begin storing the
        /// data read from the underlying stream.</param>
        /// <param name="count">The maximum number of bytes to be read from the underlying stream.</param>
        /// <returns>The total number of bytes read into the buffer. This can be less than the number of bytes requested
        /// if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.
        /// </returns>
        public override int Read(byte[] buffer, int offset, int count)
            => BaseStream.Read(buffer, offset, count);

        /// <summary>
        /// Sets the position within the underlying stream.
        /// </summary>
        /// <param name="offset">A byte offset relative to the <paramref name="origin"/> parameter.</param>
        /// <param name="origin">A value of type <see cref="SeekOrigin"/> indicating the reference point used to obtain
        /// the new position.</param>
        /// <returns>The new position within the underlying stream.</returns>
        public override long Seek(long offset, SeekOrigin origin)
            => BaseStream.Seek(offset, origin);

        /// <summary>
        /// Sets the length of the underlying stream.
        /// </summary>
        /// <param name="value">The desired length of the underlying stream in bytes.</param>
        public override void SetLength(long value)
            => BaseStream.SetLength(value);

        /// <summary>
        /// Writes a sequence of bytes to the underlying stream and advances the current position within this stream by
        /// the number of bytes written.
        /// </summary>
        /// <param name="buffer">An array of bytes. This method copies count bytes from buffer to the underlying stream.
        /// </param>
        /// <param name="offset">The zero-based byte offset in buffer at which to begin copying bytes to the underlying
        /// stream.</param>
        /// <param name="count">The number of bytes to be written to the underlying stream.</param>
        public override void Write(byte[] buffer, int offset, int count)
            => BaseStream.Write(buffer, offset, count);

        // ---- METHODS (PROTECTED) ------------------------------------------------------------------------------------

        /// <summary>
        /// Optionally releases the underlying stream.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release
        /// only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing && !_leaveOpen)
                BaseStream.Dispose();

            _disposed = true;
            base.Dispose(disposing);
        }
    }
}

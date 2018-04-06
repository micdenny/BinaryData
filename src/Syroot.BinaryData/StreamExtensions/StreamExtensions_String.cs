using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Syroot.BinaryData
{
    public static partial class StreamExtensions
    {
        // ---- FIELDS -------------------------------------------------------------------------------------------------

        [ThreadStatic]
        private static char[] _charBuffer;

        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        private static char[] CharBuffer
        {
            get
            {
                if (_charBuffer == null)
                    _charBuffer = new char[16];
                return _charBuffer;
            }
        }

        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        // ---- Read ----

        /// <summary>
        /// Returns a <see cref="String"/> instance read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="coding">The <see cref="StringCoding"/> determining how the length of the string is
        /// stored.</param>
        /// <param name="encoding">The <see cref="Encoding"/> to parse the bytes with, or <c>null</c> to use
        /// <see cref="Encoding.UTF8"/>.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <returns>The value read from the current stream.</returns>
        public static String ReadString(this Stream stream, StringCoding coding = StringCoding.VariableByteCount,
            Encoding encoding = null, ByteConverter converter = null)
        {
            encoding = encoding ?? Encoding.UTF8;
            converter = converter ?? ByteConverter.System;
            switch (coding)
            {
                case StringCoding.VariableByteCount:
                    return ReadStringWithLength(stream, stream.Read7BitInt32(), false, encoding);
                case StringCoding.ByteCharCount:
                    return ReadStringWithLength(stream, stream.ReadByte(), true, encoding);
                case StringCoding.Int16CharCount:
                    return ReadStringWithLength(stream, ReadInt16(stream, converter), true, encoding);
                case StringCoding.Int32CharCount:
                    return ReadStringWithLength(stream, ReadInt32(stream, converter), true, encoding);
                case StringCoding.ZeroTerminated:
                    return ReadStringZeroPostfix(stream, encoding);
                default:
                    throw new ArgumentException($"Invalid {nameof(StringCoding)}.", nameof(coding));
            }
        }

        /// <summary>
        /// Returns a <see cref="String"/> instance read asynchronously from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="coding">The <see cref="StringCoding"/> determining how the length of the string is
        /// stored.</param>
        /// <param name="encoding">The <see cref="Encoding"/> to parse the bytes with, or <c>null</c> to use
        /// <see cref="Encoding.UTF8"/>.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The value read from the current stream.</returns>
        public static async Task<String> ReadStringAsync(this Stream stream,
            StringCoding coding = StringCoding.VariableByteCount, Encoding encoding = null,
            ByteConverter converter = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            encoding = encoding ?? Encoding.UTF8;
            converter = converter ?? ByteConverter.System;
            switch (coding)
            {
                case StringCoding.VariableByteCount:
                    return await ReadStringWithLengthAsync(stream, stream.Read7BitInt32(), false, encoding,
                        cancellationToken);
                case StringCoding.ByteCharCount:
                    return await ReadStringWithLengthAsync(stream, stream.ReadByte(), true, encoding,
                        cancellationToken);
                case StringCoding.Int16CharCount:
                    return await ReadStringWithLengthAsync(stream, ReadInt16(stream, converter), true, encoding,
                        cancellationToken);
                case StringCoding.Int32CharCount:
                    return await ReadStringWithLengthAsync(stream, ReadInt32(stream, converter), true, encoding,
                        cancellationToken);
                case StringCoding.ZeroTerminated:
                    return await ReadStringZeroPostfixAsync(stream, encoding, cancellationToken);
                default:
                    throw new ArgumentException($"Invalid {nameof(StringCoding)}.", nameof(coding));
            }
        }

        /// <summary>
        /// Returns an array of <see cref="String"/> instances read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="coding">The <see cref="StringCoding"/> determining how the length of the strings is
        /// stored.</param>
        /// <param name="encoding">The <see cref="Encoding"/> to parse the bytes with, or <c>null</c> to use
        /// <see cref="Encoding.UTF8"/>.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static String[] ReadStrings(this Stream stream, int count,
            StringCoding coding = StringCoding.VariableByteCount, Encoding encoding = null,
            ByteConverter converter = null)
        {
            converter = converter ?? ByteConverter.System;
            return ReadMany(stream, count,
                () => ReadString(stream, coding, encoding, converter));
        }

        /// <summary>
        /// Returns an array of <see cref="String"/> instances read asynchronously from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="coding">The <see cref="StringCoding"/> determining how the length of the strings is
        /// stored.</param>
        /// <param name="encoding">The <see cref="Encoding"/> to parse the bytes with, or <c>null</c> to use
        /// <see cref="Encoding.UTF8"/>.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static async Task<String[]> ReadStringsAsync(this Stream stream, int count,
            StringCoding coding = StringCoding.VariableByteCount, Encoding encoding = null,
            ByteConverter converter = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            converter = converter ?? ByteConverter.System;
            return await ReadManyAsync(stream, count,
                () => ReadStringAsync(stream, coding, encoding, converter, cancellationToken));
        }

        /// <summary>
        /// Returns a <see cref="String"/> instance read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="length">The length of the string.</param>
        /// <param name="encoding">The <see cref="Encoding"/> to parse the decode the chars with, or <c>null</c> to use
        /// <see cref="Encoding.UTF8"/>.</param>
        /// <returns>The value read from the current stream.</returns>
        public static String ReadString(this Stream stream, int length, Encoding encoding = null)
        {
            return ReadStringWithLength(stream, length, true, encoding ?? Encoding.UTF8);
        }

        /// <summary>
        /// Returns a <see cref="String"/> instance read asynchronously from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="length">The length of the string.</param>
        /// <param name="encoding">The <see cref="Encoding"/> to parse the decode the chars with, or <c>null</c> to use
        /// <see cref="Encoding.UTF8"/>.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The value read from the current stream.</returns>
        public static async Task<String> ReadStringAsync(this Stream stream, int length, Encoding encoding = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ReadStringWithLengthAsync(stream, length, true, encoding ?? Encoding.UTF8, cancellationToken);
        }

        /// <summary>
        /// Returns an array of <see cref="String"/> instances read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="length">The length of the string.</param>
        /// <param name="encoding">The <see cref="Encoding"/> to parse the bytes with, or <c>null</c> to use
        /// <see cref="Encoding.UTF8"/>.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static String[] ReadStrings(this Stream stream, int count, int length, Encoding encoding = null)
        {
            return ReadMany(stream, count,
                () => ReadStringWithLength(stream, length, true, encoding));
        }

        /// <summary>
        /// Returns an array of <see cref="String"/> instances read asynchronously from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="length">The length of the string.</param>
        /// <param name="encoding">The <see cref="Encoding"/> to parse the bytes with, or <c>null</c> to use
        /// <see cref="Encoding.UTF8"/>.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static async Task<String[]> ReadStringsAsync(this Stream stream, int count, int length,
            Encoding encoding = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ReadManyAsync(stream, count,
                () => ReadStringWithLengthAsync(stream, length, true, encoding, cancellationToken));
        }

        // ---- Write ----

        /// <summary>
        /// Writes a <see cref="String"/> value to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="coding">The <see cref="StringCoding"/> determining how the length of the string is
        /// stored.</param>
        /// <param name="encoding">The <see cref="Encoding"/> to parse the bytes with, or <c>null</c> to use
        /// <see cref="Encoding.UTF8"/>.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void Write(this Stream stream, String value, StringCoding coding = StringCoding.VariableByteCount,
            Encoding encoding = null, ByteConverter converter = null)
        {
            encoding = encoding ?? Encoding.UTF8;
            converter = converter ?? ByteConverter.System;
            byte[] textBuffer = encoding.GetBytes(value);
            switch (coding)
            {
                case StringCoding.VariableByteCount:
                    Write7BitInt32(stream, textBuffer.Length);
                    stream.Write(textBuffer, 0, textBuffer.Length);
                    break;
                case StringCoding.ByteCharCount:
                    stream.WriteByte((byte)value.Length);
                    stream.Write(textBuffer, 0, textBuffer.Length);
                    break;
                case StringCoding.Int16CharCount:
                    converter.GetBytes((Int16)value.Length, Buffer, 0);
                    stream.Write(Buffer, 0, sizeof(Int16));
                    stream.Write(textBuffer, 0, textBuffer.Length);
                    break;
                case StringCoding.Int32CharCount:
                    converter.GetBytes(value.Length, Buffer, 0);
                    stream.Write(Buffer, 0, sizeof(Int32));
                    stream.Write(textBuffer, 0, textBuffer.Length);
                    break;
                case StringCoding.ZeroTerminated:
                    stream.Write(textBuffer, 0, textBuffer.Length);
                    switch (encoding.GetByteCount("A"))
                    {
                        case sizeof(Byte):
                            stream.WriteByte(0);
                            break;
                        case sizeof(Int16):
                            stream.WriteByte(0);
                            stream.WriteByte(0);
                            break;
                    }
                    break;
                case StringCoding.Raw:
                    stream.Write(textBuffer, 0, textBuffer.Length);
                    break;
                default:
                    throw new ArgumentException($"Invalid {nameof(StringCoding)}.", nameof(coding));
            }
        }

        /// <summary>
        /// Writes an enumerable of <see cref="String"/> values to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="coding">The <see cref="StringCoding"/> determining how the length of the strings is
        /// stored.</param>
        /// <param name="encoding">The <see cref="Encoding"/> to parse the bytes with, or <c>null</c> to use
        /// <see cref="Encoding.UTF8"/>.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void Write(this Stream stream, IEnumerable<String> values,
            StringCoding coding = StringCoding.VariableByteCount, Encoding encoding = null,
            ByteConverter converter = null)
        {
            encoding = encoding ?? Encoding.UTF8;
            converter = converter ?? ByteConverter.System;
            foreach (var value in values)
                Write(stream, value, coding, encoding, converter);
        }

        /// <summary>
        /// Writes a <see cref="String"/> value asynchronously to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="coding">The <see cref="StringCoding"/> determining how the length of the string is
        /// stored.</param>
        /// <param name="encoding">The <see cref="Encoding"/> to parse the bytes with, or <c>null</c> to use
        /// <see cref="Encoding.UTF8"/>.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public static async Task WriteAsync(this Stream stream, String value,
            StringCoding coding = StringCoding.VariableByteCount, Encoding encoding = null,
            ByteConverter converter = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            encoding = encoding ?? Encoding.UTF8;
            converter = converter ?? ByteConverter.System;
            byte[] textBuffer = encoding.GetBytes(value);
            switch (coding)
            {
                case StringCoding.VariableByteCount:
                    await Write7BitInt32Async(stream, textBuffer.Length, cancellationToken);
                    await stream.WriteAsync(textBuffer, 0, textBuffer.Length, cancellationToken);
                    break;
                case StringCoding.ByteCharCount:
                    await stream.WriteByteAsync((byte)value.Length, cancellationToken);
                    await stream.WriteAsync(textBuffer, 0, textBuffer.Length, cancellationToken);
                    break;
                case StringCoding.Int16CharCount:
                    converter.GetBytes((Int16)value.Length, Buffer, 0);
                    await stream.WriteAsync(Buffer, 0, sizeof(Int16), cancellationToken);
                    await stream.WriteAsync(textBuffer, 0, textBuffer.Length, cancellationToken);
                    break;
                case StringCoding.Int32CharCount:
                    converter.GetBytes(value.Length, Buffer, 0);
                    await stream.WriteAsync(Buffer, 0, sizeof(Int32), cancellationToken);
                    await stream.WriteAsync(textBuffer, 0, textBuffer.Length, cancellationToken);
                    break;
                case StringCoding.ZeroTerminated:
                    await stream.WriteAsync(textBuffer, 0, textBuffer.Length, cancellationToken);
                    switch (encoding.GetByteCount("A"))
                    {
                        case sizeof(Byte):
                            await stream.WriteByteAsync(0, cancellationToken);
                            break;
                        case sizeof(Int16):
                            await stream.WriteByteAsync(0, cancellationToken);
                            await stream.WriteByteAsync(0, cancellationToken);
                            break;
                    }
                    break;
                case StringCoding.Raw:
                    await stream.WriteAsync(textBuffer, 0, textBuffer.Length, cancellationToken);
                    break;
                default:
                    throw new ArgumentException($"Invalid {nameof(StringCoding)}.", nameof(coding));
            }
        }

        /// <summary>
        /// Writes an enumerable of <see cref="String"/> values asynchronously to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="coding">The <see cref="StringCoding"/> determining how the length of the strings is
        /// stored.</param>
        /// <param name="encoding">The <see cref="Encoding"/> to parse the bytes with, or <c>null</c> to use
        /// <see cref="Encoding.UTF8"/>.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public static async Task WriteAsync(this Stream stream, IEnumerable<String> values,
            StringCoding coding = StringCoding.VariableByteCount, Encoding encoding = null,
            ByteConverter converter = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            encoding = encoding ?? Encoding.UTF8;
            converter = converter ?? ByteConverter.System;
            foreach (var value in values)
                await WriteAsync(stream, value, coding, encoding, converter, cancellationToken);
        }

        /// <summary>
        /// Writes a <see cref="String"/> value to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="coding">The <see cref="StringCoding"/> determining how the length of the string is
        /// stored.</param>
        /// <param name="encoding">The <see cref="Encoding"/> to parse the bytes with, or <c>null</c> to use
        /// <see cref="Encoding.UTF8"/>.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void WriteString(this Stream stream, String value,
            StringCoding coding = StringCoding.VariableByteCount, Encoding encoding = null,
            ByteConverter converter = null)
        {
            Write(stream, value, coding, encoding, converter);
        }

        /// <summary>
        /// Writes a <see cref="String"/> value asynchronously to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="coding">The <see cref="StringCoding"/> determining how the length of the string is
        /// stored.</param>
        /// <param name="encoding">The <see cref="Encoding"/> to parse the bytes with, or <c>null</c> to use
        /// <see cref="Encoding.UTF8"/>.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public static async Task WriteStringAsync(this Stream stream, String value,
            StringCoding coding = StringCoding.VariableByteCount, Encoding encoding = null,
            ByteConverter converter = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            await WriteAsync(stream, value, coding, encoding, converter, cancellationToken);
        }

        /// <summary>
        /// Writes an enumerable of <see cref="String"/> values to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="coding">The <see cref="StringCoding"/> determining how the length of the strings is
        /// stored.</param>
        /// <param name="encoding">The <see cref="Encoding"/> to parse the bytes with, or <c>null</c> to use
        /// <see cref="Encoding.UTF8"/>.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void WriteStrings(this Stream stream, IEnumerable<String> values,
            StringCoding coding = StringCoding.VariableByteCount, Encoding encoding = null,
            ByteConverter converter = null)
        {
            Write(stream, values, coding, encoding, converter);
        }

        /// <summary>
        /// Writes an enumerable of <see cref="String"/> values asynchronously to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="coding">The <see cref="StringCoding"/> determining how the length of the strings is
        /// stored.</param>
        /// <param name="encoding">The <see cref="Encoding"/> to parse the bytes with, or <c>null</c> to use
        /// <see cref="Encoding.UTF8"/>.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public static async Task WriteStringsAsync(this Stream stream, IEnumerable<String> values,
            StringCoding coding = StringCoding.VariableByteCount, Encoding encoding = null,
            ByteConverter converter = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            await WriteAsync(stream, values, coding, encoding, converter, cancellationToken);
        }

        // ---- METHODS (PRIVATE) --------------------------------------------------------------------------------------

        private static string ReadStringWithLength(Stream stream, int length, bool lengthInChars, Encoding encoding)
        {
            if (length == 0)
                return String.Empty;

            Decoder decoder = encoding.GetDecoder();
            StringBuilder builder = new StringBuilder(length);
            int totalBytesRead = 0;
            lock (stream)
            {
                byte[] buffer = Buffer;
                char[] charBuffer = CharBuffer;
                do
                {
                    int bufferOffset = 0;
                    int charsDecoded = 0;
                    while (charsDecoded == 0)
                    {
                        // Read raw bytes from the stream.
                        int bytesRead = stream.Read(buffer, bufferOffset++, 1);
                        if (bytesRead == 0)
                            throw new EndOfStreamException("Incomplete string data, missing requested length.");
                        totalBytesRead += bytesRead;
                        // Convert the bytes to chars and append them to the string being built.
                        charsDecoded = decoder.GetCharCount(buffer, 0, bufferOffset);
                        if (charsDecoded > 0)
                        {
                            decoder.GetChars(buffer, 0, bufferOffset, charBuffer, 0);
                            builder.Append(charBuffer, 0, charsDecoded);
                        }
                    }
                } while ((lengthInChars && builder.Length < length) || (!lengthInChars && totalBytesRead < length));
            }
            return builder.ToString();
        }

        private static async Task<string> ReadStringWithLengthAsync(Stream stream, int length, bool lengthInChars,
            Encoding encoding, CancellationToken cancellationToken)
        {
            if (length == 0)
                return String.Empty;

            Decoder decoder = encoding.GetDecoder();
            StringBuilder builder = new StringBuilder(length);
            int totalBytesRead = 0;
            byte[] buffer = Buffer;
            char[] charBuffer = CharBuffer;
            do
            {
                int bufferOffset = 0;
                int charsDecoded = 0;
                while (charsDecoded == 0)
                {
                    // Read raw bytes from the stream.
                    int bytesRead = await stream.ReadAsync(buffer, bufferOffset++, 1, cancellationToken);
                    if (bytesRead == 0)
                        throw new EndOfStreamException("Incomplete string data, missing requested length.");
                    totalBytesRead += bytesRead;
                    // Convert the bytes to chars and append them to the string being built.
                    charsDecoded = decoder.GetCharCount(buffer, 0, bufferOffset);
                    if (charsDecoded > 0)
                    {
                        decoder.GetChars(buffer, 0, bufferOffset, charBuffer, 0);
                        builder.Append(charBuffer, 0, charsDecoded);
                    }
                }
            } while ((lengthInChars && builder.Length < length) || (!lengthInChars && totalBytesRead < length));
            return builder.ToString();
        }

        private static string ReadStringZeroPostfix(Stream stream, Encoding encoding)
        {
            // Read byte or word values until a 0 value is found (no encoding's char surrogate should consist of 0).
            // Endianness depends on encoding, not the actual values.
            List<byte> bytes = new List<byte>();
            bool isChar = true;
            byte[] buffer = Buffer;
            lock (stream)
            {
                switch (encoding.GetByteCount("A"))
                {
                    case sizeof(Byte):
                        // Read single bytes.
                        while (isChar)
                        {
                            FillBuffer(stream, sizeof(Byte));
                            if (isChar = buffer[0] != 0)
                                bytes.Add(buffer[0]);
                        }
                        break;
                    case sizeof(Int16):
                        // Read word values of 2 bytes width.
                        while (isChar)
                        {
                            FillBuffer(stream, sizeof(Int16));
                            if (isChar = buffer[0] != 0 || buffer[1] != 0)
                            {
                                bytes.Add(buffer[0]);
                                bytes.Add(buffer[1]);
                            }
                        }
                        break;
                    default:
                        throw new NotImplementedException(
                            "Unhandled character byte count. Only 1- or 2-byte encodings are support at the moment.");
                }
            }
            // Convert to string.
            return encoding.GetString(bytes.ToArray());
        }

        private static async Task<string> ReadStringZeroPostfixAsync(Stream stream, Encoding encoding,
            CancellationToken cancellationToken)
        {
            // Read byte or word values until a 0 value is found (no encoding's char surrogate should consist of 0).
            // Endianness depends on encoding, not the actual values.
            List<byte> bytes = new List<byte>();
            bool isChar = true;
            byte[] buffer = Buffer;
            switch (encoding.GetByteCount("A"))
            {
                case sizeof(Byte):
                    // Read single bytes.
                    while (isChar)
                    {
                        await FillBufferAsync(stream, sizeof(Byte), cancellationToken);
                        if (isChar = buffer[0] != 0)
                            bytes.Add(buffer[0]);
                    }
                    break;
                case sizeof(Int16):
                    // Read word values of 2 bytes width.
                    while (isChar)
                    {
                        await FillBufferAsync(stream, sizeof(Int16), cancellationToken);
                        if (isChar = buffer[0] != 0 || buffer[1] != 0)
                        {
                            bytes.Add(buffer[0]);
                            bytes.Add(buffer[1]);
                        }
                    }
                    break;
                default:
                    throw new NotImplementedException(
                        "Unhandled character byte count. Only 1- or 2-byte encodings are support at the moment.");
            }
            // Convert to string.
            return encoding.GetString(bytes.ToArray());
        }
    }
}

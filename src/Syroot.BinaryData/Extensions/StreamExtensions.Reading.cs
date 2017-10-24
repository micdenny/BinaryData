using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Syroot.BinaryData.Extensions
{
    /// <summary>
    /// Represents static extension methods for reading data with <see cref="Stream"/> instances.
    /// </summary>  
    public static partial class StreamExtensions
    {
        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        /// <summary>
        /// Returns a <see cref="Boolean"/> instance read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="format">The <see cref="BooleanDataFormat"/> format in which the data is stored.</param>
        /// <returns>The value read from the current stream.</returns>
        public static Boolean ReadBoolean(this Stream stream,
            BooleanDataFormat format = BooleanDataFormat.Byte)
        {
            switch (format)
            {
                case BooleanDataFormat.Byte:
                    return stream.ReadByte() != 0;
                case BooleanDataFormat.Word:
                    return ReadInt16(stream) != 0;
                case BooleanDataFormat.Dword:
                    return ReadInt32(stream) != 0;
                default:
                    throw new ArgumentException($"Invalid {nameof(BooleanDataFormat)}.", nameof(format));
            }
        }

        /// <summary>
        /// Returns an array of <see cref="Boolean"/> instances read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="format">The <see cref="BooleanDataFormat"/> format in which the data is stored.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static Boolean[] ReadBooleans(this Stream stream, int count,
            BooleanDataFormat format = BooleanDataFormat.Byte)
        {
            var values = new Boolean[count];
            lock (stream)
            {
                switch (format)
                {
                    case BooleanDataFormat.Byte:
                        for (int i = 0; i < count; i++)
                        {
                            values[i] = stream.ReadByte() != 0;
                        }
                        break;
                    case BooleanDataFormat.Word:
                        for (int i = 0; i < count; i++)
                        {
                            values[i] = ReadInt16(stream) != 0;
                        }
                        break;
                    case BooleanDataFormat.Dword:
                        for (int i = 0; i < count; i++)
                        {
                            values[i] = ReadInt32(stream) != 0;
                        }
                        break;
                    default:
                        throw new ArgumentException($"Invalid {nameof(BooleanDataFormat)}.", nameof(format));
                }
            }
            return values;
        }

        /// <summary>
        /// Returns an array of <see cref="Byte"/> instances read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static Byte[] ReadBytes(this Stream stream, int count)
        {
            byte[] buffer = new byte[count];
            stream.Read(buffer, 0, count);
            return buffer;
        }

        /// <summary>
        /// Returns a <see cref="DateTime"/> instance read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="format">The <see cref="DateTimeDataFormat"/> format in which the data is stored.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <returns>The value read from the current stream.</returns>
        public static DateTime ReadDateTime(this Stream stream,
            DateTimeDataFormat format = DateTimeDataFormat.NetTicks, ByteConverter converter = null)
        {
            switch (format)
            {
                case DateTimeDataFormat.NetTicks:
                    return new DateTime(ReadInt64(stream, converter));
                case DateTimeDataFormat.CTime:
                    return _cTimeBase.AddSeconds(ReadUInt32(stream, converter));
                case DateTimeDataFormat.CTime64:
                    return _cTimeBase.AddSeconds(ReadInt64(stream, converter));
                default:
                    throw new ArgumentException($"Invalid {nameof(DateTimeDataFormat)}.", nameof(format));
            }
        }

        /// <summary>
        /// Returns an array of <see cref="DateTime"/> instances read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="format">The <see cref="DateTimeDataFormat"/> format in which the data is stored.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static DateTime[] ReadDateTimes(this Stream stream, int count,
            DateTimeDataFormat format = DateTimeDataFormat.NetTicks, ByteConverter converter = null)
        {
            var values = new DateTime[count];
            lock (stream)
            {
                switch (format)
                {
                    case DateTimeDataFormat.NetTicks:
                        for (int i = 0; i < count; i++)
                        {
                            values[i] = new DateTime(ReadInt64(stream, converter));
                        }
                        break;
                    case DateTimeDataFormat.CTime:
                        for (int i = 0; i < count; i++)
                        {
                            values[i] = _cTimeBase.AddSeconds(ReadUInt32(stream, converter));
                        }
                        break;
                    case DateTimeDataFormat.CTime64:
                        for (int i = 0; i < count; i++)
                        {
                            values[i] = _cTimeBase.AddSeconds(ReadInt64(stream, converter));
                        }
                        break;
                    default:
                        throw new ArgumentException($"Invalid {nameof(BooleanDataFormat)}.", nameof(format));
                }
            }
            return values;
        }

        /// <summary>
        /// Returns a <see cref="Decimal"/> instance read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <returns>The value read from the current stream.</returns>
        public static Decimal ReadDecimal(this Stream stream, ByteConverter converter = null)
        {
            FillBuffer(stream, sizeof(Decimal));
            return (converter ?? ByteConverter.System).ToDecimal(Buffer);
        }

        /// <summary>
        /// Returns an array of <see cref="Decimal"/> instances read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static Decimal[] ReadDecimals(this Stream stream, int count, ByteConverter converter = null)
        {
            converter = converter ?? ByteConverter.System;
            var values = new Decimal[count];
            lock (stream)
            {
                byte[] buffer = Buffer;
                for (int i = 0; i < count; i++)
                {
                    FillBuffer(stream, sizeof(Decimal));
                    values[i] = converter.ToDecimal(buffer);
                }
            }
            return values;
        }

        /// <summary>
        /// Returns a <see cref="Double"/> instance read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <returns>The value read from the current stream.</returns>
        public static Double ReadDouble(this Stream stream, ByteConverter converter = null)
        {
            FillBuffer(stream, sizeof(Double));
            return (converter ?? ByteConverter.System).ToDouble(Buffer);
        }

        /// <summary>
        /// Returns an array of <see cref="Double"/> instances read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static Double[] ReadDoubles(this Stream stream, int count, ByteConverter converter = null)
        {
            converter = converter ?? ByteConverter.System;
            var values = new Double[count];
            lock (stream)
            {
                byte[] buffer = Buffer;
                for (int i = 0; i < count; i++)
                {
                    FillBuffer(stream, sizeof(Double));
                    values[i] = converter.ToDouble(buffer);
                }
            }
            return values;
        }

        /// <summary>
        /// Returns an <see cref="Int16"/> instance read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <returns>The value read from the current stream.</returns>
        public static Int16 ReadInt16(this Stream stream, ByteConverter converter = null)
        {
            FillBuffer(stream, sizeof(Int16));
            return (converter ?? ByteConverter.System).ToInt16(Buffer);
        }

        /// <summary>
        /// Returns an array of <see cref="Int16"/> instances read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static Int16[] ReadInt16s(this Stream stream, int count, ByteConverter converter = null)
        {
            converter = converter ?? ByteConverter.System;
            var values = new Int16[count];
            lock (stream)
            {
                byte[] buffer = Buffer;
                for (int i = 0; i < count; i++)
                {
                    FillBuffer(stream, sizeof(Int16));
                    values[i] = converter.ToInt16(buffer);
                }
            }
            return values;
        }

        /// <summary>
        /// Returns an <see cref="Int32"/> instance read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <returns>The value read from the current stream.</returns>
        public static Int32 ReadInt32(this Stream stream, ByteConverter converter = null)
        {
            FillBuffer(stream, sizeof(Int32));
            return (converter ?? ByteConverter.System).ToInt32(Buffer);
        }

        /// <summary>
        /// Returns an array of <see cref="Int32"/> instances read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static Int32[] ReadInt32s(this Stream stream, int count, ByteConverter converter = null)
        {
            converter = converter ?? ByteConverter.System;
            var values = new Int32[count];
            lock (stream)
            {
                byte[] buffer = Buffer;
                for (int i = 0; i < count; i++)
                {
                    FillBuffer(stream, sizeof(Int32));
                    values[i] = converter.ToInt32(buffer);
                }
            }
            return values;
        }

        /// <summary>
        /// Returns an <see cref="Int64"/> instance read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <returns>The value read from the current stream.</returns>
        public static Int64 ReadInt64(this Stream stream, ByteConverter converter = null)
        {
            FillBuffer(stream, sizeof(Int64));
            return (converter ?? ByteConverter.System).ToInt64(Buffer);
        }

        /// <summary>
        /// Returns an array of <see cref="Int64"/> instances read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static Int64[] ReadInt64s(this Stream stream, int count, ByteConverter converter = null)
        {
            converter = converter ?? ByteConverter.System;
            var values = new Int64[count];
            lock (stream)
            {
                byte[] buffer = Buffer;
                for (int i = 0; i < count; i++)
                {
                    FillBuffer(stream, sizeof(Int64));
                    values[i] = converter.ToInt64(buffer);
                }
            }
            return values;
        }

        /// <summary>
        /// Returns an <see cref="SByte"/> instance read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <returns>The value read from the current stream.</returns>
        public static SByte ReadSByte(this Stream stream)
        {
            return (SByte)stream.ReadByte();
        }

        /// <summary>
        /// Returns an array of <see cref="SByte"/> instances read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static SByte[] ReadSBytes(this Stream stream, int count)
        {
            var values = new SByte[count];
            lock (stream)
            {
                byte[] buffer = Buffer;
                for (int i = 0; i < count; i++)
                {
                    FillBuffer(stream, sizeof(SByte));
                    values[i] = (SByte)buffer[0];
                }
            }
            return values;
        }

        /// <summary>
        /// Returns a <see cref="Single"/> instance read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <returns>The value read from the current stream.</returns>
        public static Single ReadSingle(this Stream stream, ByteConverter converter = null)
        {
            FillBuffer(stream, sizeof(Single));
            return (converter ?? ByteConverter.System).ToSingle(Buffer);
        }

        /// <summary>
        /// Returns an array of <see cref="Single"/> instances read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static Single[] ReadSingles(this Stream stream, int count, ByteConverter converter = null)
        {
            converter = converter ?? ByteConverter.System;
            var values = new Single[count];
            lock (stream)
            {
                byte[] buffer = Buffer;
                for (int i = 0; i < count; i++)
                {
                    FillBuffer(stream, sizeof(Single));
                    values[i] = converter.ToSingle(buffer);
                }
            }
            return values;
        }

        /// <summary>
        /// Returns a <see cref="String"/> instance read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="format">The <see cref="StringDataFormat"/> format determining how the length of the string is
        /// stored.</param>
        /// <param name="encoding">The <see cref="Encoding"/> to parse the bytes with, or <c>null</c> to use
        /// <see cref="Encoding.UTF8"/>.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <returns>The value read from the current stream.</returns>
        public static String ReadString(this Stream stream,
            StringDataFormat format = StringDataFormat.DynamicByteCount, Encoding encoding = null,
            ByteConverter converter = null)
        {
            encoding = encoding ?? Encoding.UTF8;
            converter = converter ?? ByteConverter.System;
            switch (format)
            {
                case StringDataFormat.DynamicByteCount:
                    return ReadStringWithLength(stream, Read7BitEncodedInt32(stream), false, encoding);
                case StringDataFormat.ByteCharCount:
                    return ReadStringWithLength(stream, stream.ReadByte(), true, encoding);
                case StringDataFormat.Int16CharCount:
                    return ReadStringWithLength(stream, ReadInt16(stream, converter), true, encoding);
                case StringDataFormat.Int32CharCount:
                    return ReadStringWithLength(stream, ReadInt32(stream, converter), true, encoding);
                case StringDataFormat.ZeroTerminated:
                    return ReadStringZeroPostfix(stream, encoding);
                default:
                    throw new ArgumentException($"Invalid {nameof(StringDataFormat)}.", nameof(format));
            }
        }

        /// <summary>
        /// Returns an array of <see cref="String"/> instances read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="format">The <see cref="StringDataFormat"/> format determining how the length of the strings is
        /// stored.</param>
        /// <param name="encoding">The <see cref="Encoding"/> to parse the bytes with, or <c>null</c> to use
        /// <see cref="Encoding.UTF8"/>.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static String[] ReadStrings(this Stream stream, int count,
            StringDataFormat format = StringDataFormat.DynamicByteCount, Encoding encoding = null,
            ByteConverter converter = null)
        {
            encoding = encoding ?? Encoding.UTF8;
            converter = converter ?? ByteConverter.System;
            var values = new String[count];
            lock (stream)
            {
                switch (format)
                {
                    case StringDataFormat.DynamicByteCount:
                        for (int i = 0; i < count; i++)
                        {
                            values[i] = ReadStringWithLength(stream, Read7BitEncodedInt32(stream), false, encoding);
                        }
                        break;
                    case StringDataFormat.ByteCharCount:
                        for (int i = 0; i < count; i++)
                        {
                            values[i] = ReadStringWithLength(stream, stream.ReadByte(), true, encoding);
                        }
                        break;
                    case StringDataFormat.Int16CharCount:
                        for (int i = 0; i < count; i++)
                        {
                            values[i] = ReadStringWithLength(stream, ReadInt16(stream, converter), true, encoding);
                        }
                        break;
                    case StringDataFormat.Int32CharCount:
                        for (int i = 0; i < count; i++)
                        {
                            values[i] = ReadStringWithLength(stream, ReadInt32(stream, converter), true, encoding);
                        }
                        break;
                    case StringDataFormat.ZeroTerminated:
                        for (int i = 0; i < count; i++)
                        {
                            values[i] = ReadStringZeroPostfix(stream, encoding);
                        }
                        break;
                    default:
                        throw new ArgumentException($"Invalid {nameof(StringDataFormat)}.", nameof(format));
                }
            }
            return values;
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
            encoding = encoding ?? Encoding.UTF8;
            var values = new String[count];
            lock (stream)
            {
                for (int i = 0; i < count; i++)
                {
                    values[i] = ReadStringWithLength(stream, length, true, encoding);
                }
            }
            return values;
        }

        /// <summary>
        /// Returns a <see cref="UInt16"/> instance read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <returns>The value read from the current stream.</returns>
        public static UInt16 ReadUInt16(this Stream stream, ByteConverter converter = null)
        {
            FillBuffer(stream, sizeof(UInt16));
            return (converter ?? ByteConverter.System).ToUInt16(Buffer);
        }

        /// <summary>
        /// Returns an array of <see cref="UInt16"/> instances read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static UInt16[] ReadUInt16s(this Stream stream, int count, ByteConverter converter = null)
        {
            converter = converter ?? ByteConverter.System;
            var values = new UInt16[count];
            lock (stream)
            {
                byte[] buffer = Buffer;
                for (int i = 0; i < count; i++)
                {
                    FillBuffer(stream, sizeof(UInt16));
                    values[i] = converter.ToUInt16(buffer);
                }
            }
            return values;
        }

        /// <summary>
        /// Returns a <see cref="UInt32"/> instance read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <returns>The value read from the current stream.</returns>
        public static UInt32 ReadUInt32(this Stream stream, ByteConverter converter = null)
        {
            FillBuffer(stream, sizeof(UInt32));
            return (converter ?? ByteConverter.System).ToUInt32(Buffer);
        }

        /// <summary>
        /// Returns an array of <see cref="UInt32"/> instances read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static UInt32[] ReadUInt32s(this Stream stream, int count, ByteConverter converter = null)
        {
            converter = converter ?? ByteConverter.System;
            var values = new UInt32[count];
            lock (stream)
            {
                byte[] buffer = Buffer;
                for (int i = 0; i < count; i++)
                {
                    FillBuffer(stream, sizeof(UInt32));
                    values[i] = converter.ToUInt32(buffer);
                }
            }
            return values;
        }

        /// <summary>
        /// Returns a <see cref="UInt64"/> instance read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <returns>The value read from the current stream.</returns>
        public static UInt64 ReadUInt64(this Stream stream, ByteConverter converter = null)
        {
            FillBuffer(stream, sizeof(UInt64));
            return (converter ?? ByteConverter.System).ToUInt64(Buffer);
        }

        /// <summary>
        /// Returns an array of <see cref="UInt64"/> instances read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static UInt64[] ReadUInt64s(this Stream stream, int count, ByteConverter converter = null)
        {
            converter = converter ?? ByteConverter.System;
            var values = new UInt64[count];
            lock (stream)
            {
                byte[] buffer = Buffer;
                for (int i = 0; i < count; i++)
                {
                    FillBuffer(stream, sizeof(UInt64));
                    values[i] = converter.ToUInt64(buffer);
                }
            }
            return values;
        }

        // ---- METHODS (PRIVATE) --------------------------------------------------------------------------------------

        private static void FillBuffer(Stream stream, int length)
        {
            if (stream.Read(Buffer, 0, length) < length)
                throw new EndOfStreamException($"Could not read {length} bytes.");
        }

        private static Int32 Read7BitEncodedInt32(Stream stream)
        {
            // Endianness should not matter, as this value is stored byte by byte.
            // While the highest bit is set, the integer requires another of a maximum of 5 bytes.
            int value = 0;
            for (int i = 0; i < sizeof(Int32) + 1; i++)
            {
                int readByte = stream.ReadByte();
                if (readByte == -1)
                    throw new EndOfStreamException("Incomplete 7-bit encoded integer.");
                value |= (readByte & 0b01111111) << i * 7;
                if ((readByte & 0b10000000) == 0)
                {
                    return value;
                }
            }
            throw new InvalidDataException("Invalid 7-bit encoded integer.");
        }

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

        private static string ReadStringZeroPostfix(Stream stream, Encoding encoding)
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
                        FillBuffer(stream, sizeof(Byte));
                        if (isChar = buffer[0] != 0)
                        {
                            bytes.Add(buffer[0]);
                        }
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
            // Convert to string.
            return encoding.GetString(bytes.ToArray());
        }
    }
}

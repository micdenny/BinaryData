using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Syroot.BinaryData
{
    public static partial class StreamExtensions
    {
        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        // ---- Read ----

        /// <summary>
        /// Returns a <see cref="Boolean"/> instance read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="coding">The <see cref="BooleanCoding"/> in which the data is stored.</param>
        /// <returns>The value read from the current stream.</returns>
        public static Boolean ReadBoolean(this Stream stream, BooleanCoding coding = BooleanCoding.Byte)
        {
            switch (coding)
            {
                case BooleanCoding.Byte:
                    return stream.ReadByte() != 0;
                case BooleanCoding.Word:
                    return ReadInt16(stream) != 0;
                case BooleanCoding.Dword:
                    return ReadInt32(stream) != 0;
                default:
                    throw new ArgumentException($"Invalid {nameof(BooleanCoding)}.", nameof(coding));
            }
        }

        /// <summary>
        /// Returns a <see cref="Boolean"/> instance read asynchronously from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="coding">The <see cref="BooleanCoding"/> in which the data is stored.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The value read from the current stream.</returns>
        public static async Task<Boolean> ReadBooleanAsync(this Stream stream,
            BooleanCoding coding = BooleanCoding.Byte, CancellationToken cancellationToken = default)
        {
            switch (coding)
            {
                case BooleanCoding.Byte:
                    return await Read1ByteAsync(stream, cancellationToken) != 0;
                case BooleanCoding.Word:
                    return await ReadInt16Async(stream, cancellationToken: cancellationToken) != 0;
                case BooleanCoding.Dword:
                    return await ReadInt32Async(stream, cancellationToken: cancellationToken) != 0;
                default:
                    throw new ArgumentException($"Invalid {nameof(BooleanCoding)}.", nameof(coding));
            }
        }

        /// <summary>
        /// Returns an array of <see cref="Boolean"/> instances read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="coding">The <see cref="BooleanCoding"/> in which the data is stored.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static Boolean[] ReadBooleans(this Stream stream, int count, BooleanCoding coding = BooleanCoding.Byte)
        {
            return ReadMany(stream, count,
                () => stream.ReadBoolean(coding));
        }

        /// <summary>
        /// Returns an array of <see cref="Boolean"/> instances read asynchronously from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="coding">The <see cref="BooleanCoding"/> in which the data is stored.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static async Task<Boolean[]> ReadBooleansAsync(this Stream stream, int count,
            BooleanCoding coding = BooleanCoding.Byte, CancellationToken cancellationToken = default)
        {
            return await ReadManyAsync(stream, count,
                () => ReadBooleanAsync(stream, coding, cancellationToken));
        }

        // ---- Write ----

        /// <summary>
        /// Writes a <see cref="Boolean"/> value to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="coding">The <see cref="BooleanCoding"/> in which the data is stored.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void Write(this Stream stream, Boolean value, BooleanCoding coding = BooleanCoding.Byte,
            ByteConverter converter = null)
        {
            converter = converter ?? ByteConverter.System;
            byte[] buffer;
            switch (coding)
            {
                case BooleanCoding.Byte:
                    stream.WriteByte((Byte)(value ? 1 : 0));
                    break;
                case BooleanCoding.Word:
                    buffer = Buffer;
                    converter.GetBytes((Int16)(value ? 1 : 0), buffer, 0);
                    stream.Write(Buffer, 0, sizeof(Int16));
                    break;
                case BooleanCoding.Dword:
                    buffer = Buffer;
                    converter.GetBytes(value ? 1 : 0, buffer, 0);
                    stream.Write(Buffer, 0, sizeof(Int32));
                    break;
                default:
                    throw new ArgumentException($"Invalid {nameof(BooleanCoding)}.", nameof(coding));
            }
        }

        /// <summary>
        /// Writes a <see cref="Boolean"/> value asynchronously to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="coding">The <see cref="BooleanCoding"/> in which the data is stored.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public static async Task WriteAsync(this Stream stream, Boolean value,
            BooleanCoding coding = BooleanCoding.Byte, ByteConverter converter = null,
            CancellationToken cancellationToken = default)
        {
            converter = converter ?? ByteConverter.System;
            byte[] buffer;
            switch (coding)
            {
                case BooleanCoding.Byte:
                    await stream.WriteAsync((Byte)(value ? 1 : 0), cancellationToken);
                    break;
                case BooleanCoding.Word:
                    buffer = Buffer;
                    converter.GetBytes((Int16)(value ? 1 : 0), buffer, 0);
                    await stream.WriteAsync(Buffer, 0, sizeof(Int16), cancellationToken);
                    break;
                case BooleanCoding.Dword:
                    buffer = Buffer;
                    converter.GetBytes(value ? 1 : 0, buffer, 0);
                    await stream.WriteAsync(Buffer, 0, sizeof(Int32), cancellationToken);
                    break;
                default:
                    throw new ArgumentException($"Invalid {nameof(BooleanCoding)}.", nameof(coding));
            }
        }

        /// <summary>
        /// Writes a <see cref="Boolean"/> value to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="coding">The <see cref="BooleanCoding"/> in which the data is stored.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void WriteBoolean(this Stream stream, Boolean value, BooleanCoding coding = BooleanCoding.Byte,
            ByteConverter converter = null)
        {
            Write(stream, value, coding, converter);
        }

        /// <summary>
        /// Writes a <see cref="Boolean"/> value asynchronously to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="coding">The <see cref="BooleanCoding"/> in which the data is stored.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public static async Task WriteBooleanAsync(this Stream stream, Boolean value,
            BooleanCoding coding = BooleanCoding.Byte, ByteConverter converter = null,
            CancellationToken cancellationToken = default)
        {
            await WriteAsync(stream, value, coding, converter, cancellationToken);
        }

        /// <summary>
        /// Writes an enumerable of <see cref="Boolean"/> values to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="coding">The <see cref="BooleanCoding"/> in which the data is stored.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void Write(this Stream stream, IEnumerable<Boolean> values,
            BooleanCoding coding = BooleanCoding.Byte, ByteConverter converter = null)
        {
            converter = converter ?? ByteConverter.System;
            foreach (var value in values)
                Write(stream, value, coding, converter);
        }

        /// <summary>
        /// Writes an enumerable of <see cref="Boolean"/> values asynchronously to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="coding">The <see cref="BooleanCoding"/> in which the data is stored.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public static async Task WriteAsync(this Stream stream, IEnumerable<Boolean> values,
            BooleanCoding coding = BooleanCoding.Byte, ByteConverter converter = null,
            CancellationToken cancellationToken = default)
        {
            converter = converter ?? ByteConverter.System;
            foreach (var value in values)
                await WriteAsync(stream, value, coding, converter, cancellationToken);
        }

        /// <summary>
        /// Writes an enumerable of <see cref="Boolean"/> values to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="coding">The <see cref="BooleanCoding"/> in which the data is stored.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void WriteBooleans(this Stream stream, IEnumerable<Boolean> values,
            BooleanCoding coding = BooleanCoding.Byte, ByteConverter converter = null)
        {
            Write(stream, values, coding, converter);
        }

        /// <summary>
        /// Writes an enumerable of <see cref="Boolean"/> values to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="coding">The <see cref="BooleanCoding"/> in which the data is stored.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public static async Task WriteBooleansAsync(this Stream stream, IEnumerable<Boolean> values,
            BooleanCoding coding = BooleanCoding.Byte, ByteConverter converter = null,
            CancellationToken cancellationToken = default)
        {
            await WriteAsync(stream, values, coding, converter, cancellationToken);
        }
    }
}

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
        /// Returns a <see cref="UInt32"/> instance read asynchronously from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The value read from the current stream.</returns>
        public static async Task<UInt32> ReadUInt32Async(this Stream stream, ByteConverter converter = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await FillBufferAsync(stream, sizeof(UInt32), cancellationToken);
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
            return ReadMany(stream, count,
                () => ReadUInt32(stream, converter));
        }

        /// <summary>
        /// Returns an array of <see cref="UInt32"/> instances read asynchronously from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static async Task<UInt32[]> ReadUInt32sAsync(this Stream stream, int count,
            ByteConverter converter = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            converter = converter ?? ByteConverter.System;
            return await ReadManyAsync(stream, count,
                () => ReadUInt32Async(stream, converter, cancellationToken));
        }

        // ---- Write ----

        /// <summary>
        /// Writes a <see cref="UInt32"/> value to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void Write(this Stream stream, UInt32 value, ByteConverter converter = null)
        {
            byte[] buffer = Buffer;
            (converter ?? ByteConverter.System).GetBytes(value, buffer, 0);
            stream.Write(buffer, 0, sizeof(UInt32));
        }

        /// <summary>
        /// Writes an enumerable of <see cref="UInt32"/> values to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void Write(this Stream stream, IEnumerable<UInt32> values, ByteConverter converter = null)
        {
            converter = converter ?? ByteConverter.System;
            foreach (var value in values)
                Write(stream, value, converter);
        }

        /// <summary>
        /// Writes a <see cref="UInt32"/> value asynchronously to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public static async Task WriteAsync(this Stream stream, UInt32 value, ByteConverter converter = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            byte[] buffer = Buffer;
            (converter ?? ByteConverter.System).GetBytes(value, buffer, 0);
            await stream.WriteAsync(buffer, 0, sizeof(UInt32), cancellationToken);
        }

        /// <summary>
        /// Writes an enumerable of <see cref="UInt32"/> values asynchronously to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public static async Task WriteAsync(this Stream stream, IEnumerable<UInt32> values,
            ByteConverter converter = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            converter = converter ?? ByteConverter.System;
            foreach (var value in values)
                await WriteAsync(stream, value, converter, cancellationToken);
        }

        /// <summary>
        /// Writes a <see cref="UInt32"/> value to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void WriteUInt32(this Stream stream, UInt32 value, ByteConverter converter = null)
        {
            Write(stream, value, converter);
        }

        /// <summary>
        /// Writes a <see cref="UInt32"/> value asynchronously to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public static async Task WriteUInt32Async(this Stream stream, UInt32 value, ByteConverter converter = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await WriteAsync(stream, value, converter, cancellationToken);
        }

        /// <summary>
        /// Writes an enumerable of <see cref="UInt32"/> values to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void WriteUInt32s(this Stream stream, IEnumerable<UInt32> values, ByteConverter converter = null)
        {
            Write(stream, values, converter);
        }

        /// <summary>
        /// Writes an enumerable of <see cref="UInt32"/> values asynchronously to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public static async Task WriteUInt32sAsync(this Stream stream, IEnumerable<UInt32> values,
            ByteConverter converter = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            await WriteAsync(stream, values, converter, cancellationToken);
        }
    }
}

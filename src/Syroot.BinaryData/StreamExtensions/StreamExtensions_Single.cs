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
        /// Returns a <see cref="Single"/> instance read asynchronously from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The value read from the current stream.</returns>
        public static async Task<Single> ReadSingleAsync(this Stream stream, ByteConverter converter = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await FillBufferAsync(stream, sizeof(Single), cancellationToken);
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
            return ReadMany(stream, count,
                () => ReadSingle(stream, converter));
        }

        /// <summary>
        /// Returns an array of <see cref="Single"/> instances read asynchronously from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static async Task<Single[]> ReadSinglesAsync(this Stream stream, int count,
            ByteConverter converter = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            converter = converter ?? ByteConverter.System;
            return await ReadManyAsync(stream, count,
                () => ReadSingleAsync(stream, converter, cancellationToken));
        }

        // ---- Write ----

        /// <summary>
        /// Writes a <see cref="Single"/> value to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void Write(this Stream stream, Single value, ByteConverter converter = null)
        {
            byte[] buffer = Buffer;
            (converter ?? ByteConverter.System).GetBytes(value, buffer, 0);
            stream.Write(buffer, 0, sizeof(Single));
        }

        /// <summary>
        /// Writes an enumerable of <see cref="Single"/> values to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void Write(this Stream stream, IEnumerable<Single> values, ByteConverter converter = null)
        {
            converter = converter ?? ByteConverter.System;
            foreach (var value in values)
                Write(stream, value, converter);
        }

        /// <summary>
        /// Writes a <see cref="Single"/> value asynchronously to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public static async Task WriteAsync(this Stream stream, Single value, ByteConverter converter = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            byte[] buffer = Buffer;
            (converter ?? ByteConverter.System).GetBytes(value, buffer, 0);
            await stream.WriteAsync(buffer, 0, sizeof(Single), cancellationToken);
        }

        /// <summary>
        /// Writes an enumerable of <see cref="Single"/> values asynchronously to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public static async Task WriteAsync(this Stream stream, IEnumerable<Single> values,
            ByteConverter converter = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            converter = converter ?? ByteConverter.System;
            foreach (var value in values)
                await WriteAsync(stream, value, converter, cancellationToken);
        }

        /// <summary>
        /// Writes a <see cref="Single"/> value to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void WriteSingle(this Stream stream, Single value, ByteConverter converter = null)
        {
            Write(stream, value, converter);
        }

        /// <summary>
        /// Writes a <see cref="Single"/> value asynchronously to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public static async Task WriteSingleAsync(this Stream stream, Single value, ByteConverter converter = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await WriteAsync(stream, value, converter, cancellationToken);
        }

        /// <summary>
        /// Writes an enumerable of <see cref="Single"/> values to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void WriteSingles(this Stream stream, IEnumerable<Single> values, ByteConverter converter = null)
        {
            Write(stream, values, converter);
        }

        /// <summary>
        /// Writes an enumerable of <see cref="Single"/> values asynchronously to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public static async Task WriteSinglesAsync(this Stream stream, IEnumerable<Single> values,
            ByteConverter converter = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            await WriteAsync(stream, values, converter, cancellationToken);
        }
    }
}

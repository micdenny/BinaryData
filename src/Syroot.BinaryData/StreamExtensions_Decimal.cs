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
        /// Returns a <see cref="Decimal"/> instance read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <returns>The value read from the current stream.</returns>
        public static Decimal ReadDecimal(this Stream stream)
        {
            FillBuffer(stream, sizeof(Decimal));
            return ByteConverter.ToDecimal(Buffer);
        }

        /// <summary>
        /// Returns a <see cref="Decimal"/> instance read asynchronously from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The value read from the current stream.</returns>
        public static async Task<Decimal> ReadDecimalAsync(this Stream stream,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await FillBufferAsync(stream, sizeof(Decimal), cancellationToken);
            return ByteConverter.ToDecimal(Buffer);
        }

        /// <summary>
        /// Returns an array of <see cref="Decimal"/> instances read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static Decimal[] ReadDecimals(this Stream stream, int count)
        {
            return ReadMany(stream, count,
                () => ReadDecimal(stream));
        }

        /// <summary>
        /// Returns an array of <see cref="Decimal"/> instances read asynchronously from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static async Task<Decimal[]> ReadDecimalsAsync(this Stream stream, int count,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ReadManyAsync(stream, count,
                () => ReadDecimalAsync(stream, cancellationToken));
        }

        // ---- Write ----

        /// <summary>
        /// Writes a <see cref="Decimal"/> value to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        public static void Write(this Stream stream, Decimal value)
        {
            byte[] buffer = Buffer;
            ByteConverter.GetBytes(value, buffer, 0);
            stream.Write(buffer, 0, sizeof(Decimal));
        }

        /// <summary>
        /// Writes a <see cref="Decimal"/> value asynchronously to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public static async Task WriteAsync(this Stream stream, Decimal value,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            byte[] buffer = Buffer;
            ByteConverter.GetBytes(value, buffer, 0);
            await stream.WriteAsync(buffer, 0, sizeof(Decimal), cancellationToken);
        }

        /// <summary>
        /// Writes an enumerable of <see cref="Decimal"/> values to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        public static void Write(this Stream stream, IEnumerable<Decimal> values)
        {
            foreach (var value in values)
                Write(stream, value);
        }

        /// <summary>
        /// Writes an enumerable of <see cref="Decimal"/> values asynchronously to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public static async Task WriteAsync(this Stream stream, IEnumerable<Decimal> values,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            foreach (var value in values)
                await WriteAsync(stream, value, cancellationToken);
        }

        /// <summary>
        /// Writes a <see cref="Decimal"/> value to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        public static void WriteDecimal(this Stream stream, Decimal value)
        {
            Write(stream, value);
        }

        /// <summary>
        /// Writes a <see cref="Decimal"/> value asynchronously to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public static async Task WriteDecimalAsync(this Stream stream, Decimal value,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await WriteAsync(stream, value, cancellationToken);
        }

        /// <summary>
        /// Writes an enumerable of <see cref="Decimal"/> values to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        public static void WriteDecimals(this Stream stream, IEnumerable<Decimal> values)
        {
            Write(stream, values);
        }

        /// <summary>
        /// Writes an enumerable of <see cref="Decimal"/> values asynchronously to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public static async Task WriteDecimalsAsync(this Stream stream, IEnumerable<Decimal> values,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await WriteAsync(stream, values, cancellationToken);
        }
    }
}

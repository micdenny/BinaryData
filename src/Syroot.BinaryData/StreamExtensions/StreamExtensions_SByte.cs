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
        /// Returns an <see cref="SByte"/> instance read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <returns>The value read from the current stream.</returns>
        public static SByte ReadSByte(this Stream stream)
        {
            return (SByte)stream.ReadByte();
        }

        /// <summary>
        /// Returns an <see cref="SByte"/> instance read asynchronously from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The value read from the current stream.</returns>
        public static async Task<SByte> ReadSByteAsync(this Stream stream,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return (SByte)await stream.Read1ByteAsync(cancellationToken);
        }

        /// <summary>
        /// Returns an array of <see cref="SByte"/> instances read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static SByte[] ReadSBytes(this Stream stream, int count)
        {
            return ReadMany(stream, count,
                () => ReadSByte(stream));
        }

        /// <summary>
        /// Returns an array of <see cref="SByte"/> instances read asynchronously from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static async Task<SByte[]> ReadSBytesAsync(this Stream stream, int count,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ReadManyAsync(stream, count,
                () => ReadSByteAsync(stream, cancellationToken));
        }

        // ---- Write ----

        /// <summary>
        /// Writes an <see cref="SByte"/> value to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        public static void Write(this Stream stream, SByte value)
        {
            byte[] buffer = Buffer;
            buffer[0] = (byte)value;
            stream.Write(buffer, 0, sizeof(SByte));
        }

        /// <summary>
        /// Writes an enumerable of <see cref="SByte"/> values to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        public static void Write(this Stream stream, IEnumerable<SByte> values)
        {
            foreach (var value in values)
                Write(stream, value);
        }

        /// <summary>
        /// Writes an <see cref="SByte"/> value asynchronously to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public static async Task WriteAsync(this Stream stream, SByte value,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            byte[] buffer = Buffer;
            buffer[0] = (byte)value;
            await stream.WriteAsync(buffer, 0, sizeof(SByte), cancellationToken);
        }

        /// <summary>
        /// Writes an enumerable of <see cref="SByte"/> values asynchronously to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public static async Task WriteAsync(this Stream stream, IEnumerable<SByte> values,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            foreach (var value in values)
                await WriteAsync(stream, value, cancellationToken);
        }

        /// <summary>
        /// Writes an <see cref="SByte"/> value to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        public static void WriteSByte(this Stream stream, SByte value)
        {
            Write(stream, value);
        }

        /// <summary>
        /// Writes an <see cref="SByte"/> value asynchronously to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public static async Task WriteSByteAsync(this Stream stream, SByte value,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await WriteAsync(stream, value, cancellationToken);
        }

        /// <summary>
        /// Writes an enumerable of <see cref="SByte"/> values to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        public static void WriteSBytes(this Stream stream, IEnumerable<SByte> values)
        {
            Write(stream, values);
        }

        /// <summary>
        /// Writes an enumerable of <see cref="SByte"/> values asynchronously to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public static async Task WriteSBytesAsync(this Stream stream, IEnumerable<SByte> values,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await WriteAsync(stream, values, cancellationToken);
        }
    }
}

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
        /// Returns a <see cref="Byte"/> instance read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <returns>The value read from the current stream.</returns>
        public static Byte Read1Byte(this Stream stream)
        {
            return (Byte)stream.ReadByte();
        }

        /// <summary>
        /// Returns a <see cref="Byte"/> instance read asynchronously from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The value read from the current stream.</returns>
        public static async Task<Byte> Read1ByteAsync(this Stream stream,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await FillBufferAsync(stream, 1, cancellationToken);
            return Buffer[0];
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
            int bytesRead = stream.Read(buffer, 0, count);
            if (bytesRead < count)
                throw new EndOfStreamException($"Could not read {count} bytes.");
            return buffer;
        }

        /// <summary>
        /// Returns an array of <see cref="Byte"/> instances read asynchronously from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static async Task<Byte[]> ReadBytesAsync(this Stream stream, int count,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            byte[] buffer = new byte[count];
            int bytesRead = await stream.ReadAsync(buffer, 0, count, cancellationToken);
            if (bytesRead < count)
                throw new EndOfStreamException($"Could not read {count} bytes.");
            return buffer;
        }

        // ---- Write ----

        /// <summary>
        /// Writes a <see cref="Byte"/> value to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        public static void Write(this Stream stream, Byte value)
        {
            stream.WriteByte(value);
        }

        /// <summary>
        /// Writes a <see cref="Byte"/> value asynchronously to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public static async Task WriteAsync(this Stream stream, Byte value,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            byte[] buffer = Buffer;
            buffer[0] = value;
            await stream.WriteAsync(buffer, 0, sizeof(Byte), cancellationToken);
        }

        /// <summary>
        /// Writes a <see cref="Byte"/> value to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        public static void WriteByte(this Stream stream, Byte value)
        {
            Write(stream, value);
        }

        /// <summary>
        /// Writes a <see cref="Byte"/> value asynchronously to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public static async Task WriteByteAsync(this Stream stream, Byte value,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await WriteAsync(stream, value, cancellationToken);
        }

        /// <summary>
        /// Writes an enumerable of <see cref="Byte"/> values to the <paramref name="stream"/>. This method writes bytes
        /// one-by-one.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        public static void Write(this Stream stream, IEnumerable<Byte> values)
        {
            foreach (var value in values)
                stream.WriteByte(value);
        }

        /// <summary>
        /// Writes an enumerable of <see cref="Byte"/> values to the <paramref name="stream"/>. This method writes bytes
        /// one-by-one.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public static async Task WriteAsync(this Stream stream, IEnumerable<Byte> values,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            foreach (var value in values)
                await stream.WriteAsync(value);
        }

        /// <summary>
        /// Writes an enumerable of <see cref="Byte"/> values to the <paramref name="stream"/>. This method writes bytes
        /// one-by-one.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        public static void WriteBytes(this Stream stream, IEnumerable<Byte> values)
        {
            Write(stream, values);
        }

        /// <summary>
        /// Writes an enumerable of <see cref="Byte"/> values to the <paramref name="stream"/>. This method writes bytes
        /// one-by-one.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public static async Task WriteBytesAsync(this Stream stream, IEnumerable<Byte> values,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await WriteAsync(stream, values, cancellationToken);
        }

        /// <summary>
        /// Writes an array of <see cref="Byte"/> values to the <paramref name="stream"/>. This method writes all bytes
        /// in one call.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        public static void Write(this Stream stream, Byte[] values)
        {
            stream.Write(values, 0, values.Length);
        }

        /// <summary>
        /// Writes an array of <see cref="Byte"/> values to the <paramref name="stream"/>. This method writes all bytes
        /// in one call.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public static async Task WriteAsync(this Stream stream, Byte[] values,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await WriteAsync(stream, values, cancellationToken);
        }

        /// <summary>
        /// Writes an array of <see cref="Byte"/> values to the <paramref name="stream"/>. This method writes all bytes
        /// in one call.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        public static void WriteBytes(this Stream stream, Byte[] values)
        {
            Write(stream, values);
        }

        /// <summary>
        /// Writes an array of <see cref="Byte"/> values to the <paramref name="stream"/>. This method writes all bytes
        /// in one call.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public static async Task WriteBytesAsync(this Stream stream, Byte[] values,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await WriteAsync(stream, values, cancellationToken);
        }
    }
}

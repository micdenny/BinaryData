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
        /// Returns a variable-length <see cref="Int32"/> instance read from the given <paramref name="stream"/> which
        /// can require up to 5 bytes.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <returns>The value read from the current stream.</returns>
        public static Int32 Read7BitInt32(this Stream stream)
        {
            // Endianness should not matter, as this value is stored byte by byte.
            // While the highest bit is set, the integer requires another of a maximum of 5 bytes.
            int value = 0;
            for (int i = 0; i < sizeof(Int32) + 1; i++)
            {
                byte readByte = stream.Read1Byte();
                if (readByte == 0xFF)
                    throw new EndOfStreamException("Incomplete 7-bit encoded Int32.");
                value |= (readByte & 0b01111111) << i * 7;
                if ((readByte & 0b10000000) == 0)
                    return value;
            }
            throw new InvalidDataException("Invalid 7-bit encoded Int32.");
        }

        /// <summary>
        /// Returns a variable-length <see cref="Int32"/> instance read asynchronously from the given
        /// <paramref name="stream"/> which can require up to 5 bytes.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The value read from the current stream.</returns>
        public static async Task<Int32> Read7BitInt32Async(this Stream stream,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            // Endianness should not matter, as this value is stored byte by byte.
            // While the highest bit is set, the integer requires another of a maximum of 5 bytes.
            int value = 0;
            for (int i = 0; i < sizeof(Int32) + 1; i++)
            {
                byte readByte = await stream.Read1ByteAsync(cancellationToken);
                if (readByte == 0xFF)
                    throw new EndOfStreamException("Incomplete 7-bit encoded Int32.");
                value |= (readByte & 0b01111111) << i * 7;
                if ((readByte & 0b10000000) == 0)
                    return value;
            }
            throw new InvalidDataException("Invalid 7-bit encoded Int32.");
        }

        /// <summary>
        /// Returns an array of variable-length <see cref="Int32"/> instances read from the <paramref name="stream"/>
        /// which can require to 5 bytes each.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static Int32[] Read7BitInt32s(this Stream stream, int count)
        {
            return ReadMany(stream, count,
                () => Read7BitInt32(stream));
        }

        /// <summary>
        /// Returns an array of variable-length <see cref="Int32"/> instances read asynchronously from the
        /// <paramref name="stream"/> which can require to 5 bytes each.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static async Task<Int32[]> Read7BitInt32sAsync(this Stream stream, int count,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ReadManyAsync(stream, count,
                () => Read7BitInt32Async(stream, cancellationToken));
        }

        // ---- Write ----

        /// <summary>
        /// Writes a variable-length <see cref="Int32"/> value to the <paramref name="stream"/> which can require up to
        /// 5 bytes.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        public static void Write7BitInt32(this Stream stream, Int32 value)
        {
            byte[] buffer = Buffer;
            int size = Get7BitInt32Bytes(value, buffer);
            stream.Write(buffer, 0, size);
        }

        /// <summary>
        /// Writes a variable-length <see cref="Int32"/> value asynchronously to the <paramref name="stream"/> which can
        /// require up to 5 bytes.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public static async Task Write7BitInt32Async(this Stream stream, Int32 value,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            byte[] buffer = Buffer;
            int size = Get7BitInt32Bytes(value, buffer);
            await stream.WriteAsync(buffer, 0, size, cancellationToken);
        }

        /// <summary>
        /// Writes an enumerable of <see cref="Int32"/> values to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        public static void Write7BitInt32s(this Stream stream, IEnumerable<Int32> values)
        {
            foreach (var value in values)
                Write7BitInt32(stream, value);
        }

        /// <summary>
        /// Writes an enumerable of <see cref="Int32"/> values asynchronously to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public static async Task Write7BitInt32Async(this Stream stream, IEnumerable<Int32> values,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            foreach (var value in values)
                await Write7BitInt32Async(stream, value, cancellationToken);
        }

        // ---- METHODS (PRIVATE) --------------------------------------------------------------------------------------

        private static int Get7BitInt32Bytes(int value, byte[] buffer)
        {
            // The highest bit determines whether to continue writing more bytes to form the Int32 value.
            int i = 0;
            while (value >= 0b10000000)
            {
                buffer[i++] = (byte)(value | 0b10000000);
                value >>= 7;
            }
            buffer[i] = (byte)value;
            return i + 1;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Syroot.BinaryData
{
    public partial class BinaryStream
    {
        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        // ---- Read ----

        /// <summary>
        /// Returns an <see cref="Int64"/> instance read from the underlying stream.
        /// </summary>
        /// <returns>The value read from the current stream.</returns>
        public Int64 ReadInt64()
            => BaseStream.ReadInt64(ByteConverter);

        /// <summary>
        /// Returns an <see cref="Int64"/> instance read asynchronously from the underlying stream.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The value read from the current stream.</returns>
        public async Task<Int64> ReadInt64Async(CancellationToken cancellationToken = default)
            => await BaseStream.ReadInt64Async(ByteConverter, cancellationToken);

        /// <summary>
        /// Returns an array of <see cref="Int64"/> instances read from the underlying stream.
        /// </summary>
        /// <param name="count">The number of values to read.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public Int64[] ReadInt64s(int count)
            => BaseStream.ReadInt64s(count, ByteConverter);

        /// <summary>
        /// Returns an array of <see cref="Int64"/> instances read asynchronously from the underlying stream.
        /// </summary>
        /// <param name="count">The number of values to read.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public async Task<Int64[]> ReadInt64sAsync(int count,
            CancellationToken cancellationToken = default)
            => await BaseStream.ReadInt64sAsync(count, ByteConverter, cancellationToken);

        // ---- Write ----

        /// <summary>
        /// Writes an <see cref="Int64"/> value to the underlying stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void Write(Int64 value)
            => BaseStream.Write(value, ByteConverter);

        /// <summary>
        /// Writes an enumerable of <see cref="Int64"/> values to the underlying stream.
        /// </summary>
        /// <param name="values">The values to write.</param>
        public void Write(IEnumerable<Int64> values)
            => BaseStream.Write(values, ByteConverter);

        /// <summary>
        /// Writes an <see cref="Int64"/> value asynchronously to the underlying stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public async Task WriteAsync(Int64 value, CancellationToken cancellationToken = default)
            => await BaseStream.WriteAsync(value, ByteConverter, cancellationToken);

        /// <summary>
        /// Writes an enumerable of <see cref="Int64"/> values to the underlying stream.
        /// </summary>
        /// <param name="values">The values to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public async Task WriteAsync(IEnumerable<Int64> values,
            CancellationToken cancellationToken = default)
            => await BaseStream.WriteAsync(values, ByteConverter, cancellationToken);

        /// <summary>
        /// Writes an <see cref="Int64"/> value to the underlying stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void WriteInt64(Int64 value)
            => BaseStream.Write(value, ByteConverter);

        /// <summary>
        /// Writes an <see cref="Int64"/> value to the underlying stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public async Task WriteInt64Async(Int64 value, CancellationToken cancellationToken = default)
            => await BaseStream.WriteAsync(value, ByteConverter, cancellationToken);

        /// <summary>
        /// Writes an enumerable of <see cref="Int64"/> values to the underlying stream.
        /// </summary>
        /// <param name="values">The values to write.</param>
        public void WriteInt64s(IEnumerable<Int64> values)
            => BaseStream.Write(values, ByteConverter);

        /// <summary>
        /// Writes an enumerable of <see cref="Int64"/> values to the underlying stream.
        /// </summary>
        /// <param name="values">The values to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public async Task WriteInt64sAsync(IEnumerable<Int64> values,
            CancellationToken cancellationToken = default)
            => await BaseStream.WriteAsync(values, ByteConverter, cancellationToken);
    }
}

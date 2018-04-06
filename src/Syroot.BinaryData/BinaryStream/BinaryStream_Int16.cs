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
        /// Returns an <see cref="Int16"/> instance read from the underlying stream.
        /// </summary>
        /// <returns>The value read from the current stream.</returns>
        public Int16 ReadInt16()
            => BaseStream.ReadInt16(ByteConverter);

        /// <summary>
        /// Returns an <see cref="Int16"/> instance read asynchronously from the underlying stream.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The value read from the current stream.</returns>
        public async Task<Int16> ReadInt16Async(CancellationToken cancellationToken = default(CancellationToken))
            => await BaseStream.ReadInt16Async(ByteConverter, cancellationToken);

        /// <summary>
        /// Returns an array of <see cref="Int16"/> instances read from the underlying stream.
        /// </summary>
        /// <param name="count">The number of values to read.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public Int16[] ReadInt16s(int count)
            => BaseStream.ReadInt16s(count, ByteConverter);

        /// <summary>
        /// Returns an array of <see cref="Int16"/> instances read asynchronously from the underlying stream.
        /// </summary>
        /// <param name="count">The number of values to read.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public async Task<Int16[]> ReadInt16sAsync(int count,
            CancellationToken cancellationToken = default(CancellationToken))
            => await BaseStream.ReadInt16sAsync(count, ByteConverter, cancellationToken);

        // ---- Write ----

        /// <summary>
        /// Writes an <see cref="Int16"/> value to the underlying stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void Write(Int16 value)
            => BaseStream.Write(value, ByteConverter);

        /// <summary>
        /// Writes an enumerable of <see cref="Int16"/> values to the underlying stream.
        /// </summary>
        /// <param name="values">The values to write.</param>
        public void Write(IEnumerable<Int16> values)
            => BaseStream.Write(values, ByteConverter);

        /// <summary>
        /// Writes an <see cref="Int16"/> value asynchronously to the underlying stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public async Task WriteAsync(Int16 value, CancellationToken cancellationToken = default(CancellationToken))
            => await BaseStream.WriteAsync(value, ByteConverter, cancellationToken);

        /// <summary>
        /// Writes an enumerable of <see cref="Int16"/> values to the underlying stream.
        /// </summary>
        /// <param name="values">The values to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public async Task WriteAsync(IEnumerable<Int16> values,
            CancellationToken cancellationToken = default(CancellationToken))
            => await BaseStream.WriteAsync(values, ByteConverter, cancellationToken);

        /// <summary>
        /// Writes an <see cref="Int16"/> value to the underlying stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void WriteInt16(Int16 value)
            => BaseStream.Write(value, ByteConverter);

        /// <summary>
        /// Writes an <see cref="Int16"/> value to the underlying stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public async Task WriteInt16Async(Int16 value, CancellationToken cancellationToken = default(CancellationToken))
            => await BaseStream.WriteAsync(value, ByteConverter, cancellationToken);

        /// <summary>
        /// Writes an enumerable of <see cref="Int16"/> values to the underlying stream.
        /// </summary>
        /// <param name="values">The values to write.</param>
        public void WriteInt16s(IEnumerable<Int16> values)
            => BaseStream.Write(values, ByteConverter);

        /// <summary>
        /// Writes an enumerable of <see cref="Int16"/> values to the underlying stream.
        /// </summary>
        /// <param name="values">The values to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public async Task WriteInt16sAsync(IEnumerable<Int16> values,
            CancellationToken cancellationToken = default(CancellationToken))
            => await BaseStream.WriteAsync(values, ByteConverter, cancellationToken);
    }
}

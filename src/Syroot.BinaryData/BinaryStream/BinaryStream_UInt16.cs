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
        /// Returns an <see cref="UInt16"/> instance read from the underlying stream.
        /// </summary>
        /// <returns>The value read from the current stream.</returns>
        public UInt16 ReadUInt16()
            => BaseStream.ReadUInt16(ByteConverter);

        /// <summary>
        /// Returns an <see cref="UInt16"/> instance read asynchronously from the underlying stream.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The value read from the current stream.</returns>
        public async Task<UInt16> ReadUInt16Async(CancellationToken cancellationToken = default(CancellationToken))
            => await BaseStream.ReadUInt16Async(ByteConverter, cancellationToken);

        /// <summary>
        /// Returns an array of <see cref="UInt16"/> instances read from the underlying stream.
        /// </summary>
        /// <param name="count">The number of values to read.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public UInt16[] ReadUInt16s(int count)
            => BaseStream.ReadUInt16s(count, ByteConverter);

        /// <summary>
        /// Returns an array of <see cref="UInt16"/> instances read asynchronously from the underlying stream.
        /// </summary>
        /// <param name="count">The number of values to read.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public async Task<UInt16[]> ReadUInt16sAsync(int count,
            CancellationToken cancellationToken = default(CancellationToken))
            => await BaseStream.ReadUInt16sAsync(count, ByteConverter, cancellationToken);

        // ---- Write ----

        /// <summary>
        /// Writes an <see cref="UInt16"/> value to the underlying stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void Write(UInt16 value)
            => BaseStream.Write(value, ByteConverter);

        /// <summary>
        /// Writes an enumerable of <see cref="UInt16"/> values to the underlying stream.
        /// </summary>
        /// <param name="values">The values to write.</param>
        public void Write(IEnumerable<UInt16> values)
            => BaseStream.Write(values, ByteConverter);

        /// <summary>
        /// Writes an <see cref="UInt16"/> value asynchronously to the underlying stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public async Task WriteAsync(UInt16 value, CancellationToken cancellationToken = default(CancellationToken))
            => await BaseStream.WriteAsync(value, ByteConverter, cancellationToken);

        /// <summary>
        /// Writes an enumerable of <see cref="UInt16"/> values to the underlying stream.
        /// </summary>
        /// <param name="values">The values to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public async Task WriteAsync(IEnumerable<UInt16> values,
            CancellationToken cancellationToken = default(CancellationToken))
            => await BaseStream.WriteAsync(values, ByteConverter, cancellationToken);

        /// <summary>
        /// Writes an <see cref="UInt16"/> value to the underlying stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void WriteUInt16(UInt16 value)
            => BaseStream.Write(value, ByteConverter);

        /// <summary>
        /// Writes an <see cref="UInt16"/> value to the underlying stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public async Task WriteUInt16Async(UInt16 value, CancellationToken cancellationToken = default(CancellationToken))
            => await BaseStream.WriteAsync(value, ByteConverter, cancellationToken);

        /// <summary>
        /// Writes an enumerable of <see cref="UInt16"/> values to the underlying stream.
        /// </summary>
        /// <param name="values">The values to write.</param>
        public void WriteUInt16s(IEnumerable<UInt16> values)
            => BaseStream.Write(values, ByteConverter);

        /// <summary>
        /// Writes an enumerable of <see cref="UInt16"/> values to the underlying stream.
        /// </summary>
        /// <param name="values">The values to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public async Task WriteUInt16sAsync(IEnumerable<UInt16> values,
            CancellationToken cancellationToken = default(CancellationToken))
            => await BaseStream.WriteAsync(values, ByteConverter, cancellationToken);
    }
}

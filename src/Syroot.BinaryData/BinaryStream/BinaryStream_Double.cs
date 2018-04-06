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
        /// Returns a <see cref="Double"/> instance read from the underlying stream.
        /// </summary>
        /// <returns>The value read from the current stream.</returns>
        public Double ReadDouble()
            => BaseStream.ReadDouble(ByteConverter);

        /// <summary>
        /// Returns a <see cref="Double"/> instance read asynchronously from the underlying stream.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The value read from the current stream.</returns>
        public async Task<Double> ReadDoubleAsync(CancellationToken cancellationToken = default(CancellationToken))
            => await BaseStream.ReadDoubleAsync(ByteConverter, cancellationToken);

        /// <summary>
        /// Returns an array of <see cref="Double"/> instances read from the underlying stream.
        /// </summary>
        /// <param name="count">The number of values to read.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public Double[] ReadDoubles(int count)
            => BaseStream.ReadDoubles(count, ByteConverter);

        /// <summary>
        /// Returns an array of <see cref="Double"/> instances read asynchronously from the underlying stream.
        /// </summary>
        /// <param name="count">The number of values to read.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public async Task<Double[]> ReadDoublesAsync(int count,
            CancellationToken cancellationToken = default(CancellationToken))
            => await BaseStream.ReadDoublesAsync(count, ByteConverter, cancellationToken);

        // ---- Write ----

        /// <summary>
        /// Writes a <see cref="Double"/> value to the underlying stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void Write(Double value)
            => BaseStream.Write(value, ByteConverter);

        /// <summary>
        /// Writes an enumerable of <see cref="Double"/> values to the underlying stream.
        /// </summary>
        /// <param name="values">The values to write.</param>
        public void Write(IEnumerable<Double> values)
            => BaseStream.Write(values, ByteConverter);

        /// <summary>
        /// Writes a <see cref="Double"/> value asynchronously to the underlying stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public async Task WriteAsync(Double value, CancellationToken cancellationToken = default(CancellationToken))
            => await BaseStream.WriteAsync(value, ByteConverter, cancellationToken);

        /// <summary>
        /// Writes an enumerable of <see cref="Double"/> values asynchronously to the underlying stream.
        /// </summary>
        /// <param name="values">The values to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public async Task WriteAsync(IEnumerable<Double> values,
            CancellationToken cancellationToken = default(CancellationToken))
            => await BaseStream.WriteAsync(values, ByteConverter, cancellationToken);

        /// <summary>
        /// Writes a <see cref="Double"/> value to the underlying stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void WriteDouble(Double value)
            => BaseStream.Write(value, ByteConverter);

        /// <summary>
        /// Writes a <see cref="Double"/> value asynchronously to the underlying stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public async Task WriteDoubleAsync(Double value,
            CancellationToken cancellationToken = default(CancellationToken))
            => await BaseStream.WriteAsync(value, ByteConverter, cancellationToken);

        /// <summary>
        /// Writes an enumerable of <see cref="Double"/> values to the underlying stream.
        /// </summary>
        /// <param name="values">The values to write.</param>
        public void WriteDoubles(IEnumerable<Double> values)
            => BaseStream.Write(values, ByteConverter);

        /// <summary>
        /// Writes an enumerable of <see cref="Double"/> values asynchronously to the underlying stream.
        /// </summary>
        /// <param name="values">The values to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public async Task WriteDoublesAsync(IEnumerable<Double> values,
            CancellationToken cancellationToken = default(CancellationToken))
            => await BaseStream.WriteAsync(values, ByteConverter, cancellationToken);
    }
}

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
        /// Returns an <see cref="Single"/> instance read from the underlying stream.
        /// </summary>
        /// <returns>The value read from the current stream.</returns>
        public Single ReadSingle()
            => BaseStream.ReadSingle(ByteConverter);

        /// <summary>
        /// Returns an <see cref="Single"/> instance read asynchronously from the underlying stream.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The value read from the current stream.</returns>
        public async Task<Single> ReadSingleAsync(CancellationToken cancellationToken = default)
            => await BaseStream.ReadSingleAsync(ByteConverter, cancellationToken);

        /// <summary>
        /// Returns an array of <see cref="Single"/> instances read from the underlying stream.
        /// </summary>
        /// <param name="count">The number of values to read.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public Single[] ReadSingles(int count)
            => BaseStream.ReadSingles(count, ByteConverter);

        /// <summary>
        /// Returns an array of <see cref="Single"/> instances read asynchronously from the underlying stream.
        /// </summary>
        /// <param name="count">The number of values to read.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public async Task<Single[]> ReadSinglesAsync(int count,
            CancellationToken cancellationToken = default)
            => await BaseStream.ReadSinglesAsync(count, ByteConverter, cancellationToken);

        // ---- Write ----

        /// <summary>
        /// Writes an <see cref="Single"/> value to the underlying stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void Write(Single value)
            => BaseStream.Write(value, ByteConverter);

        /// <summary>
        /// Writes an enumerable of <see cref="Single"/> values to the underlying stream.
        /// </summary>
        /// <param name="values">The values to write.</param>
        public void Write(IEnumerable<Single> values)
            => BaseStream.Write(values, ByteConverter);

        /// <summary>
        /// Writes an <see cref="Single"/> value asynchronously to the underlying stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public async Task WriteAsync(Single value, CancellationToken cancellationToken = default)
            => await BaseStream.WriteAsync(value, ByteConverter, cancellationToken);

        /// <summary>
        /// Writes an enumerable of <see cref="Single"/> values to the underlying stream.
        /// </summary>
        /// <param name="values">The values to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public async Task WriteAsync(IEnumerable<Single> values,
            CancellationToken cancellationToken = default)
            => await BaseStream.WriteAsync(values, ByteConverter, cancellationToken);

        /// <summary>
        /// Writes an <see cref="Single"/> value to the underlying stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void WriteSingle(Single value)
            => BaseStream.Write(value, ByteConverter);

        /// <summary>
        /// Writes an <see cref="Single"/> value to the underlying stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public async Task WriteSingleAsync(Single value, CancellationToken cancellationToken = default)
            => await BaseStream.WriteAsync(value, ByteConverter, cancellationToken);

        /// <summary>
        /// Writes an enumerable of <see cref="Single"/> values to the underlying stream.
        /// </summary>
        /// <param name="values">The values to write.</param>
        public void WriteSingles(IEnumerable<Single> values)
            => BaseStream.Write(values, ByteConverter);

        /// <summary>
        /// Writes an enumerable of <see cref="Single"/> values to the underlying stream.
        /// </summary>
        /// <param name="values">The values to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public async Task WriteSinglesAsync(IEnumerable<Single> values,
            CancellationToken cancellationToken = default)
            => await BaseStream.WriteAsync(values, ByteConverter, cancellationToken);
    }
}

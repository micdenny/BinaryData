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
        /// Returns a <see cref="Boolean"/> instance read from the underlying stream.
        /// </summary>
        /// <returns>The value read from the current stream.</returns>
        public Boolean ReadBoolean()
            => BaseStream.ReadBoolean(BooleanCoding);

        /// <summary>
        /// Returns a <see cref="Boolean"/> instance read asynchronously from the underlying stream.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The value read from the current stream.</returns>
        public async Task<Boolean> ReadBooleanAsync(CancellationToken cancellationToken = default)
            => await BaseStream.ReadBooleanAsync(BooleanCoding, cancellationToken);

        /// <summary>
        /// Returns an array of <see cref="Boolean"/> instances read from the underlying stream.
        /// </summary>
        /// <param name="count">The number of values to read.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public Boolean[] ReadBooleans(int count)
            => BaseStream.ReadBooleans(count, BooleanCoding);

        /// <summary>
        /// Returns an array of <see cref="Boolean"/> instances read asynchronously from the underlying stream.
        /// </summary>
        /// <param name="count">The number of values to read.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public async Task<Boolean[]> ReadBooleans(int count,
            CancellationToken cancellationToken = default)
            => await BaseStream.ReadBooleansAsync(count, BooleanCoding, cancellationToken);

        // ---- Write ----

        /// <summary>
        /// Writes a <see cref="Boolean"/> value to the underlying stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void Write(Boolean value)
            => BaseStream.Write(value, BooleanCoding, ByteConverter);

        /// <summary>
        /// Writes a <see cref="Boolean"/> value asynchronously to the underlying stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public async Task WriteAsync(Boolean value, CancellationToken cancellationToken = default)
            => await BaseStream.WriteAsync(value, BooleanCoding, ByteConverter, cancellationToken);

        /// <summary>
        /// Writes a <see cref="Boolean"/> value to the underlying stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void WriteBoolean(Boolean value)
            => BaseStream.Write(value, BooleanCoding, ByteConverter);

        /// <summary>
        /// Writes a <see cref="Boolean"/> value asynchronously to the underlying stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public async Task WriteBooleanAsync(Boolean value,
            CancellationToken cancellationToken = default)
            => await BaseStream.WriteAsync(value, BooleanCoding, ByteConverter, cancellationToken);

        /// <summary>
        /// Writes an enumerable of <see cref="Boolean"/> values to the underlying stream.
        /// </summary>
        /// <param name="values">The values to write.</param>
        public void Write(IEnumerable<Boolean> values)
            => BaseStream.Write(values, BooleanCoding, ByteConverter);

        /// <summary>
        /// Writes an enumerable of <see cref="Boolean"/> values asynchronously to the underlying stream.
        /// </summary>
        /// <param name="values">The values to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public async Task WriteAsync(IEnumerable<Boolean> values,
            CancellationToken cancellationToken = default)
            => await BaseStream.WriteAsync(values, BooleanCoding, ByteConverter, cancellationToken);

        /// <summary>
        /// Writes an enumerable of <see cref="Boolean"/> values to the underlying stream.
        /// </summary>
        /// <param name="values">The values to write.</param>
        public void WriteBooleans(IEnumerable<Boolean> values)
            => BaseStream.Write(values, BooleanCoding, ByteConverter);

        /// <summary>
        /// Writes an enumerable of <see cref="Boolean"/> values to the underlying stream.
        /// </summary>
        /// <param name="values">The values to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public async Task WriteBooleansAsync(IEnumerable<Boolean> values,
            CancellationToken cancellationToken = default)
            => await BaseStream.WriteAsync(values, BooleanCoding, ByteConverter, cancellationToken);
    }
}

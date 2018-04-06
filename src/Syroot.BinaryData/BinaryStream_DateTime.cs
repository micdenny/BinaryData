using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Syroot.BinaryData
{
    public partial class BinaryStream
    {
        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        // --- Read ----

        /// <summary>
        /// Returns a <see cref="DateTime"/> instance read from the underlying stream.
        /// </summary>
        /// <returns>The value read from the current stream.</returns>
        public DateTime ReadDateTime()
            => BaseStream.ReadDateTime(DateTimeCoding, ByteConverter);

        /// <summary>
        /// Returns a <see cref="DateTime"/> instance read asynchronously from the underlying stream.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The value read from the current stream.</returns>
        public async Task<DateTime> ReadDateTimeAsync(CancellationToken cancellationToken = default(CancellationToken))
            => await BaseStream.ReadDateTimeAsync(DateTimeCoding, ByteConverter, cancellationToken);

        /// <summary>
        /// Returns an array of <see cref="DateTime"/> instances read from the underlying stream.
        /// </summary>
        /// <param name="count">The number of values to read.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public DateTime[] ReadDateTimes(int count)
            => BaseStream.ReadDateTimes(count, DateTimeCoding, ByteConverter);

        /// <summary>
        /// Returns an array of <see cref="DateTime"/> instances read asynchronously from the underlying stream.
        /// </summary>
        /// <param name="count">The number of values to read.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public async Task<DateTime[]> ReadDateTimesAsync(int count,
            CancellationToken cancellationToken = default(CancellationToken))
            => await BaseStream.ReadDateTimesAsync(count, DateTimeCoding, ByteConverter, cancellationToken);

        // ---- Write ----

        /// <summary>
        /// Writes a <see cref="DateTime"/> value to the underlying stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void Write(DateTime value)
            => BaseStream.Write(value, DateTimeCoding, ByteConverter);

        /// <summary>
        /// Writes an enumerable of <see cref="DateTime"/> values to the underlying stream.
        /// </summary>
        /// <param name="values">The values to write.</param>
        public void Write(IEnumerable<DateTime> values)
            => BaseStream.Write(values, DateTimeCoding, ByteConverter);

        /// <summary>
        /// Writes a <see cref="DateTime"/> value asynchronously to the underlying stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public async Task WriteAsync(DateTime value, CancellationToken cancellationToken = default(CancellationToken))
            => await BaseStream.WriteAsync(value, DateTimeCoding, ByteConverter, cancellationToken);

        /// <summary>
        /// Writes an enumerable of <see cref="DateTime"/> asynchronously values to the underlying stream.
        /// </summary>
        /// <param name="values">The values to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public async Task WriteAsync(IEnumerable<DateTime> values,
            CancellationToken cancellationToken = default(CancellationToken))
            => await BaseStream.WriteAsync(values, DateTimeCoding, ByteConverter, cancellationToken);

        /// <summary>
        /// Writes a <see cref="DateTime"/> value to the underlying stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void WriteDateTime(DateTime value)
            => BaseStream.Write(value, DateTimeCoding, ByteConverter);

        /// <summary>
        /// Writes a <see cref="DateTime"/> value asynchronously to the underlying stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public async Task WriteDateTimeAsync(DateTime value,
            CancellationToken cancellationToken = default(CancellationToken))
            => await BaseStream.WriteAsync(value, DateTimeCoding, ByteConverter, cancellationToken);

        /// <summary>
        /// Writes an enumerable of <see cref="DateTime"/> values to the underlying stream.
        /// </summary>
        /// <param name="values">The values to write.</param>
        public void WriteDateTimes(IEnumerable<DateTime> values)
            => BaseStream.Write(values, DateTimeCoding, ByteConverter);

        /// <summary>
        /// Writes an enumerable of <see cref="DateTime"/> values asynchronously to the underlying stream.
        /// </summary>
        /// <param name="values">The values to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public async Task WriteDateTimesAsync(IEnumerable<DateTime> values,
            CancellationToken cancellationToken = default(CancellationToken))
            => await BaseStream.WriteAsync(values, DateTimeCoding, ByteConverter, cancellationToken);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Syroot.BinaryData
{
    public partial class BinaryStream
    {
        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        /// <summary>
        /// Returns a <see cref="Decimal"/> instance read from the underlying stream.
        /// </summary>
        /// <returns>The value read from the current stream.</returns>
        public Decimal ReadDecimal()
            => BaseStream.ReadDecimal(ByteConverter);

        /// <summary>
        /// Returns a <see cref="Decimal"/> instance read asynchronously from the underlying stream.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The value read from the current stream.</returns>
        public async Task<Decimal> ReadDecimalAsync(CancellationToken cancellationToken = default(CancellationToken))
            => await BaseStream.ReadDecimalAsync(ByteConverter, cancellationToken);

        /// <summary>
        /// Returns an array of <see cref="Decimal"/> instances read from the underlying stream.
        /// </summary>
        /// <param name="count">The number of values to read.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public Decimal[] ReadDecimals(int count)
            => BaseStream.ReadDecimals(count, ByteConverter);

        /// <summary>
        /// Returns an array of <see cref="Decimal"/> instances read asynchronously from the underlying stream.
        /// </summary>
        /// <param name="count">The number of values to read.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public async Task<Decimal[]> ReadDecimalsAsync(int count,
            CancellationToken cancellationToken = default(CancellationToken))
            => await BaseStream.ReadDecimalsAsync(count, ByteConverter, cancellationToken);

        // ---- Write ----

        /// <summary>
        /// Writes a <see cref="Decimal"/> value to the underlying stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void Write(Decimal value)
            => BaseStream.Write(value, ByteConverter);

        /// <summary>
        /// Writes a <see cref="Decimal"/> value asynchronously to the underlying stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public async Task WriteAsync(Decimal value, CancellationToken cancellationToken = default(CancellationToken))
            => await BaseStream.WriteAsync(value, ByteConverter, cancellationToken);

        /// <summary>
        /// Writes an enumerable of <see cref="Decimal"/> values to the underlying stream.
        /// </summary>
        /// <param name="values">The values to write.</param>
        public void Write(IEnumerable<Decimal> values)
            => BaseStream.Write(values, ByteConverter);

        /// <summary>
        /// Writes an enumerable of <see cref="Decimal"/> values asynchronously to the underlying stream.
        /// </summary>
        /// <param name="values">The values to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public async Task WriteAsync(IEnumerable<Decimal> values,
            CancellationToken cancellationToken = default(CancellationToken))
            => await BaseStream.WriteAsync(values, ByteConverter, cancellationToken);

        /// <summary>
        /// Writes a <see cref="Decimal"/> value to the underlying stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void WriteDecimal(Decimal value)
            => BaseStream.Write(value, ByteConverter);

        /// <summary>
        /// Writes a <see cref="Decimal"/> value asynchronously to the underlying stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public async Task WriteDecimalAsync(Decimal value,
            CancellationToken cancellationToken = default(CancellationToken))
            => await BaseStream.WriteDecimalAsync(value, ByteConverter, cancellationToken);

        /// <summary>
        /// Writes an enumerable of <see cref="Decimal"/> values to the underlying stream.
        /// </summary>
        /// <param name="values">The values to write.</param>
        public void WriteDecimals(IEnumerable<Decimal> values)
            => BaseStream.WriteDecimals(values, ByteConverter);

        /// <summary>
        /// Writes an enumerable of <see cref="Decimal"/> values asynchronously to the underlying stream.
        /// </summary>
        /// <param name="values">The values to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public async Task WriteDecimalsAsync(IEnumerable<Decimal> values,
            CancellationToken cancellationToken = default(CancellationToken))
            => await BaseStream.WriteAsync(values, ByteConverter, cancellationToken);
    }
}

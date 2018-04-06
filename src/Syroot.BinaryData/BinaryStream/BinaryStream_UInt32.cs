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
        /// Returns an <see cref="UInt32"/> instance read from the underlying stream.
        /// </summary>
        /// <returns>The value read from the current stream.</returns>
        public UInt32 ReadUInt32()
            => BaseStream.ReadUInt32(ByteConverter);

        /// <summary>
        /// Returns an <see cref="UInt32"/> instance read asynchronously from the underlying stream.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The value read from the current stream.</returns>
        public async Task<UInt32> ReadUInt32Async(CancellationToken cancellationToken = default(CancellationToken))
            => await BaseStream.ReadUInt32Async(ByteConverter, cancellationToken);

        /// <summary>
        /// Returns an array of <see cref="UInt32"/> instances read from the underlying stream.
        /// </summary>
        /// <param name="count">The number of values to read.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public UInt32[] ReadUInt32s(int count)
            => BaseStream.ReadUInt32s(count, ByteConverter);

        /// <summary>
        /// Returns an array of <see cref="UInt32"/> instances read asynchronously from the underlying stream.
        /// </summary>
        /// <param name="count">The number of values to read.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public async Task<UInt32[]> ReadUInt32sAsync(int count,
            CancellationToken cancellationToken = default(CancellationToken))
            => await BaseStream.ReadUInt32sAsync(count, ByteConverter, cancellationToken);

        // ---- Write ----

        /// <summary>
        /// Writes an <see cref="UInt32"/> value to the underlying stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void Write(UInt32 value)
            => BaseStream.Write(value, ByteConverter);

        /// <summary>
        /// Writes an enumerable of <see cref="UInt32"/> values to the underlying stream.
        /// </summary>
        /// <param name="values">The values to write.</param>
        public void Write(IEnumerable<UInt32> values)
            => BaseStream.Write(values, ByteConverter);

        /// <summary>
        /// Writes an <see cref="UInt32"/> value asynchronously to the underlying stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public async Task WriteAsync(UInt32 value, CancellationToken cancellationToken = default(CancellationToken))
            => await BaseStream.WriteAsync(value, ByteConverter, cancellationToken);

        /// <summary>
        /// Writes an enumerable of <see cref="UInt32"/> values to the underlying stream.
        /// </summary>
        /// <param name="values">The values to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public async Task WriteAsync(IEnumerable<UInt32> values,
            CancellationToken cancellationToken = default(CancellationToken))
            => await BaseStream.WriteAsync(values, ByteConverter, cancellationToken);

        /// <summary>
        /// Writes an <see cref="UInt32"/> value to the underlying stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void WriteUInt32(UInt32 value)
            => BaseStream.Write(value, ByteConverter);

        /// <summary>
        /// Writes an <see cref="UInt32"/> value to the underlying stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public async Task WriteUInt32Async(UInt32 value, CancellationToken cancellationToken = default(CancellationToken))
            => await BaseStream.WriteAsync(value, ByteConverter, cancellationToken);

        /// <summary>
        /// Writes an enumerable of <see cref="UInt32"/> values to the underlying stream.
        /// </summary>
        /// <param name="values">The values to write.</param>
        public void WriteUInt32s(IEnumerable<UInt32> values)
            => BaseStream.Write(values, ByteConverter);

        /// <summary>
        /// Writes an enumerable of <see cref="UInt32"/> values to the underlying stream.
        /// </summary>
        /// <param name="values">The values to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public async Task WriteUInt32sAsync(IEnumerable<UInt32> values,
            CancellationToken cancellationToken = default(CancellationToken))
            => await BaseStream.WriteAsync(values, ByteConverter, cancellationToken);
    }
}

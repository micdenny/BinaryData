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
        /// Returns a <see cref="String"/> instance read from the underlying stream.
        /// </summary>
        /// <returns>The value read from the current stream.</returns>
        public String ReadString()
            => BaseStream.ReadString(StringCoding, Encoding, ByteConverter);

        /// <summary>
        /// Returns a <see cref="String"/> instance read asynchronously from the underlying stream.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The value read from the current stream.</returns>
        public async Task<String> ReadStringAsync(CancellationToken cancellationToken = default(CancellationToken))
            => await BaseStream.ReadStringAsync(StringCoding, Encoding, ByteConverter, cancellationToken);

        /// <summary>
        /// Returns an array of <see cref="String"/> instances read from the underlying stream.
        /// </summary>
        /// <param name="count">The number of values to read.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public String[] ReadStrings(int count)
            => BaseStream.ReadStrings(count, StringCoding, Encoding, ByteConverter);

        /// <summary>
        /// Returns an array of <see cref="String"/> instances read asynchronously from the underlying stream.
        /// </summary>
        /// <param name="count">The number of values to read.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public async Task<String[]> ReadStringsAsync(int count,
            CancellationToken cancellationToken = default(CancellationToken))
            => await BaseStream.ReadStringsAsync(count, StringCoding, Encoding, ByteConverter, cancellationToken);

        /// <summary>
        /// Returns a <see cref="String"/> instance read from the underlying stream.
        /// </summary>
        /// <param name="length">The length of the string.</param>
        /// <returns>The value read from the current stream.</returns>
        public String ReadString(int length)
            => BaseStream.ReadString(length, Encoding);

        /// <summary>
        /// Returns a <see cref="String"/> instance read asynchronously from the underlying stream.
        /// </summary>
        /// <param name="length">The length of the string.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The value read from the current stream.</returns>
        public async Task<String> ReadStringAsync(int length,
            CancellationToken cancellationToken = default(CancellationToken))
            => await BaseStream.ReadStringAsync(length, Encoding, cancellationToken);

        /// <summary>
        /// Returns an array of <see cref="String"/> instances read from the underlying stream.
        /// </summary>
        /// <param name="count">The number of values to read.</param>
        /// <param name="length">The length of the string.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public String[] ReadStrings(int count, int length)
            => BaseStream.ReadStrings(count, length, Encoding);

        /// <summary>
        /// Returns an array of <see cref="String"/> instances read asynchronously from the underlying stream.
        /// </summary>
        /// <param name="count">The number of values to read.</param>
        /// <param name="length">The length of the string.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public async Task<String[]> ReadStringsAsync(int count, int length,
            CancellationToken cancellationToken = default(CancellationToken))
            => await BaseStream.ReadStringsAsync(count, length, Encoding, cancellationToken);

        // ---- Write ----

        /// <summary>
        /// Writes a <see cref="String"/> value to the underlying stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void Write(String value)
            => BaseStream.Write(value, StringCoding, Encoding, ByteConverter);

        /// <summary>
        /// Writes an enumerable of <see cref="String"/> values to the underlying stream.
        /// </summary>
        /// <param name="values">The values to write.</param>
        public void Write(IEnumerable<String> values)
            => BaseStream.Write(values, StringCoding, Encoding, ByteConverter);

        /// <summary>
        /// Writes a <see cref="String"/> value asynchronously to the underlying stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public async Task WriteAsync(String value, CancellationToken cancellationToken = default(CancellationToken))
            => await BaseStream.WriteAsync(value, StringCoding, Encoding, ByteConverter, cancellationToken);

        /// <summary>
        /// Writes an enumerable of <see cref="String"/> values asynchronously to the underlying stream.
        /// </summary>
        /// <param name="values">The values to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public async Task WriteAsync(IEnumerable<String> values,
            CancellationToken cancellationToken = default(CancellationToken))
            => await BaseStream.WriteAsync(values, StringCoding, Encoding, ByteConverter, cancellationToken);

        /// <summary>
        /// Writes a <see cref="String"/> value to the underlying stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void WriteString(String value)
            => BaseStream.Write(value, StringCoding, Encoding, ByteConverter);

        /// <summary>
        /// Writes a <see cref="String"/> value asynchronously to the underlying stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public async Task WriteStringAsync(String value,
            CancellationToken cancellationToken = default(CancellationToken))
            => await BaseStream.WriteAsync(value, StringCoding, Encoding, ByteConverter, cancellationToken);

        /// <summary>
        /// Writes an enumerable of <see cref="String"/> values to the underlying stream.
        /// </summary>
        /// <param name="values">The values to write.</param>
        public void WriteStrings(IEnumerable<String> values)
            => BaseStream.Write(values, StringCoding, Encoding, ByteConverter);

        /// <summary>
        /// Writes an enumerable of <see cref="String"/> values asynchronously to the underlying stream.
        /// </summary>
        /// <param name="values">The values to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public async Task WriteStringsAsync(IEnumerable<String> values,
             CancellationToken cancellationToken = default(CancellationToken))
            => await BaseStream.WriteAsync(values, StringCoding, Encoding, ByteConverter, cancellationToken);
    }
}

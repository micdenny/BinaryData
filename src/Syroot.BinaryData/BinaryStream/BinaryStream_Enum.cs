using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;

namespace Syroot.BinaryData
{
    public partial class BinaryStream
    {
        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        // ---- Read ----

        /// <summary>
        /// Returns an <see cref="Enum"/> instance of type <typeparamref name="T"/> read from the underlying stream.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="strict"><c>true</c> to raise an <see cref="ArgumentOutOfRangeException"/> if a value is not
        /// defined in the enum type.</param>
        /// <returns>The value read from the current stream.</returns>
        public T ReadEnum<T>(bool strict = false)
            where T : Enum
            => (T)BaseStream.ReadEnum(typeof(T), strict, ByteConverter);

        /// <summary>
        /// Returns an <see cref="Enum"/> instance of type <typeparamref name="T"/> read asynchronously from the
        /// underlying stream.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="strict"><c>true</c> to raise an <see cref="ArgumentOutOfRangeException"/> if a value is not
        /// defined in the enum type.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The value read from the current stream.</returns>
        public async Task<T> ReadEnumAsync<T>(bool strict = false,
            CancellationToken cancellationToken = default)
            where T : Enum
            => (T)await BaseStream.ReadEnumAsync(typeof(T), strict, ByteConverter, cancellationToken);

        /// <summary>
        /// Returns an array of <see cref="Enum"/> instances of type <typeparamref name="T"/> read from the
        /// underlying stream.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="count">The number of values to read.</param>
        /// <param name="strict"><c>true</c> to raise an <see cref="ArgumentOutOfRangeException"/> if a value is not
        /// defined in the enum type.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public T[] ReadEnums<T>(int count, bool strict = false)
            where T : Enum
            => BaseStream.ReadEnums<T>(count, strict, ByteConverter);

        /// <summary>
        /// Returns an array of <see cref="Enum"/> instances of type <typeparamref name="T"/> read asynchronously from
        /// the underlying stream.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="count">The number of values to read.</param>
        /// <param name="strict"><c>true</c> to raise an <see cref="ArgumentOutOfRangeException"/> if a value is not
        /// defined in the enum type.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public async Task<T[]> ReadEnumsAsync<T>(int count, bool strict = false,
            CancellationToken cancellationToken = default)
            where T : Enum
            => await BaseStream.ReadEnumsAsync<T>(count, strict, ByteConverter, cancellationToken);

        /// <summary>
        /// Returns an <see cref="Enum"/> instance of the given <paramref name="type"/> read from the
        /// underlying stream.
        /// </summary>
        /// <param name="type">The type of the enum.</param>
        /// <param name="strict"><c>true</c> to raise an <see cref="ArgumentOutOfRangeException"/> if a value is not
        /// defined in the enum type.</param>
        /// <returns>The value read from the current stream.</returns>
        public object ReadEnum(Type type, bool strict = false)
            => BaseStream.ReadEnum(type, strict, ByteConverter);

        /// <summary>
        /// Returns an <see cref="Enum"/> instance of the given <paramref name="type"/> read asynchronously from the
        /// underlying stream.
        /// </summary>
        /// <param name="type">The type of the enum.</param>
        /// <param name="strict"><c>true</c> to raise an <see cref="ArgumentOutOfRangeException"/> if a value is not
        /// defined in the enum type.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The value read from the current stream.</returns>
        public async Task<object> ReadEnumAsync(Type type, bool strict = false,
            CancellationToken cancellationToken = default)
            => await BaseStream.ReadEnumAsync(type, strict, ByteConverter, cancellationToken);

        /// <summary>
        /// Returns an array of <see cref="Enum"/> instances of the given <paramref name="type"/> read from the
        /// underlying stream.
        /// </summary>
        /// <param name="type">The type of the enum.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="strict"><c>true</c> to raise an <see cref="ArgumentOutOfRangeException"/> if a value is not
        /// defined in the enum type.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public object[] ReadEnums(Type type, int count, bool strict = false)
            => BaseStream.ReadEnums(type, count, strict, ByteConverter);

        /// <summary>
        /// Returns an array of <see cref="Enum"/> instances of the given <paramref name="type"/> read asynchronously
        /// from the underlying stream.
        /// </summary>
        /// <param name="type">The type of the enum.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="strict"><c>true</c> to raise an <see cref="ArgumentOutOfRangeException"/> if a value is not
        /// defined in the enum type.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public async Task<object[]> ReadEnumsAsync(Type type, int count, bool strict = false,
            CancellationToken cancellationToken = default)
            => await BaseStream.ReadEnumsAsync(type, count, strict, ByteConverter, cancellationToken);

        // ---- Write ----

        /// <summary>
        /// Writes an <see cref="Enum"/> value of type <typeparamref name="T"/> to the underlying stream.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="value">The value to write.</param>
        /// <param name="strict"><c>true</c> to raise an <see cref="ArgumentOutOfRangeException"/> if the value is not
        /// defined in the enum type.</param>
        public void WriteEnum<T>(T value, bool strict = false)
            where T : Enum
            => BaseStream.WriteEnum(value, strict, ByteConverter);

        /// <summary>
        /// Writes an <see cref="Enum"/> value of type <typeparamref name="T"/> asynchronously to the
        /// underlying stream.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="value">The value to write.</param>
        /// <param name="strict"><c>true</c> to raise an <see cref="ArgumentOutOfRangeException"/> if the value is not
        /// defined in the enum type.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public async Task WriteEnumAsync<T>(T value, bool strict = false,
            CancellationToken cancellationToken = default)
            where T : Enum
            => await BaseStream.WriteEnumAsync(value, strict, ByteConverter, cancellationToken);

        /// <summary>
        /// Writes an <see cref="Enum"/> value of the given <paramref name="type"/> to the underlying stream.
        /// </summary>
        /// <param name="type">The type of the enum.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="strict"><c>true</c> to raise an <see cref="ArgumentOutOfRangeException"/> if the value is not
        /// defined in the enum type.</param>
        public void WriteEnum(Type type, object value, bool strict = false)
            => BaseStream.WriteEnum(type, value, strict, ByteConverter);

        /// <summary>
        /// Writes an <see cref="Enum"/> value of the given <paramref name="type"/> asynchronously to the
        /// underlying stream.
        /// </summary>
        /// <param name="type">The type of the enum.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="strict"><c>true</c> to raise an <see cref="ArgumentOutOfRangeException"/> if the value is not
        /// defined in the enum type.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public async Task WriteEnumAsync(Type type, object value, bool strict = false,
            CancellationToken cancellationToken = default)
            => await BaseStream.WriteEnumAsync(type, value, strict, ByteConverter, cancellationToken);

        /// <summary>
        /// Writes an enumerable of <see cref="Enum"/> values of type <typeparamref name="T"/> to the
        /// underlying stream.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="values">The values to write.</param>
        /// <param name="strict"><c>true</c> to raise an <see cref="ArgumentOutOfRangeException"/> if the value is not
        /// defined in the enum type.</param>
        public void WriteEnums<T>(IEnumerable values, bool strict = false)
            where T : Enum
            => BaseStream.WriteEnums(typeof(T), values, strict, ByteConverter);

        /// <summary>
        /// Writes an enumerable of <see cref="Enum"/> values of type <typeparamref name="T"/> asynchronously to the
        /// underlying stream.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="values">The values to write.</param>
        /// <param name="strict"><c>true</c> to raise an <see cref="ArgumentOutOfRangeException"/> if the value is not
        /// defined in the enum type.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public async Task WriteEnumsAsync<T>(IEnumerable values, bool strict = false,
            CancellationToken cancellationToken = default)
            where T : Enum
            => await BaseStream.WriteEnumsAsync(typeof(T), values, strict, ByteConverter, cancellationToken);

        /// <summary>
        /// Writes an enumerable of <see cref="Enum"/> values of the given <paramref name="type"/> to the underlying
        /// stream.
        /// </summary>
        /// <param name="type">The type of the enum.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="strict"><c>true</c> to raise an <see cref="ArgumentOutOfRangeException"/> if the value is not
        /// defined in the enum type.</param>
        public void WriteEnums(Type type, IEnumerable values, bool strict = false)
            => BaseStream.WriteEnums(type, values, strict, ByteConverter);

        /// <summary>
        /// Writes an enumerable of <see cref="Enum"/> values of the given <paramref name="type"/> asynchronously to the
        /// underlying stream.
        /// </summary>
        /// <param name="type">The type of the enum.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="strict"><c>true</c> to raise an <see cref="ArgumentOutOfRangeException"/> if the value is not
        /// defined in the enum type.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public async Task WriteEnumsAsync(Type type, IEnumerable values, bool strict = false,
            CancellationToken cancellationToken = default)
            => await BaseStream.WriteEnumsAsync(type, values, strict, ByteConverter, cancellationToken);
    }
}

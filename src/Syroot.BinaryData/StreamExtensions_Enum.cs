using System;
using System.Collections;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Syroot.BinaryData
{
    public static partial class StreamExtensions
    {
        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        // ---- Read ----

        /// <summary>
        /// Returns an <see cref="Enum"/> instance of type <typeparamref name="T"/> read from the
        /// <paramref name="stream"/>.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="strict"><c>true</c> to raise an <see cref="ArgumentOutOfRangeException"/> if a value is not
        /// defined in the enum type.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <returns>The value read from the current stream.</returns>
        public static T ReadEnum<T>(this Stream stream, bool strict = false, ByteConverter converter = null)
            where T : struct, IComparable, IFormattable
        {
            return (T)ReadEnum(stream, typeof(T), strict, converter);
        }

        /// <summary>
        /// Returns an <see cref="Enum"/> instance of type <typeparamref name="T"/> read asynchronously from the
        /// <paramref name="stream"/>.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="strict"><c>true</c> to raise an <see cref="ArgumentOutOfRangeException"/> if a value is not
        /// defined in the enum type.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The value read from the current stream.</returns>
        public static async Task<T> ReadEnumAsync<T>(this Stream stream, bool strict = false,
            ByteConverter converter = null, CancellationToken cancellationToken = default(CancellationToken))
            where T : struct, IComparable, IFormattable
        {
            return (T)await ReadEnumAsync(stream, typeof(T), strict, converter, cancellationToken);
        }

        /// <summary>
        /// Returns an array of <see cref="Enum"/> instances of type <typeparamref name="T"/> read from the
        /// <paramref name="stream"/>.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="strict"><c>true</c> to raise an <see cref="ArgumentOutOfRangeException"/> if a value is not
        /// defined in the enum type.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static T[] ReadEnums<T>(this Stream stream, int count, bool strict = false,
            ByteConverter converter = null)
            where T : struct, IComparable, IFormattable
        {
            converter = converter ?? ByteConverter.System;
            return ReadMany(stream, count,
                () => (T)ReadEnum(stream, typeof(T), strict, converter));
        }

        /// <summary>
        /// Returns an array of <see cref="Enum"/> instances of type <typeparamref name="T"/> read asynchronously from
        /// the <paramref name="stream"/>.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="strict"><c>true</c> to raise an <see cref="ArgumentOutOfRangeException"/> if a value is not
        /// defined in the enum type.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static async Task<T[]> ReadEnumsAsync<T>(this Stream stream, int count, bool strict = false,
            ByteConverter converter = null, CancellationToken cancellationToken = default(CancellationToken))
            where T : struct, IComparable, IFormattable
        {
            converter = converter ?? ByteConverter.System;
            return await ReadManyAsync(stream, count,
                () => ReadEnumAsync<T>(stream, strict, converter, cancellationToken));
        }

        /// <summary>
        /// Returns an <see cref="Enum"/> instance of the given <paramref name="type"/> read from the
        /// <paramref name="stream"/>.
        /// </summary>
        /// <param name="type">The type of the enum.</param>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="strict"><c>true</c> to raise an <see cref="ArgumentOutOfRangeException"/> if a value is not
        /// defined in the enum type.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <returns>The value read from the current stream.</returns>
        public static object ReadEnum(this Stream stream, Type type,
            bool strict = false, ByteConverter converter = null)
        {
            converter = converter ?? ByteConverter.System;

            // Read enough bytes to form an enum value.
            Type valueType = Enum.GetUnderlyingType(type);
            object value;
            if (valueType == typeof(Byte))
            {
                FillBuffer(stream, sizeof(Byte));
                value = Buffer[0];
            }
            else if (valueType == typeof(SByte))
            {
                FillBuffer(stream, sizeof(SByte));
                value = (SByte)Buffer[0];
            }
            else if (valueType == typeof(Int16))
            {
                FillBuffer(stream, sizeof(Int16));
                value = converter.ToInt16(Buffer);
            }
            else if (valueType == typeof(Int32))
            {
                FillBuffer(stream, sizeof(Int32));
                value = converter.ToInt32(Buffer);
            }
            else if (valueType == typeof(Int64))
            {
                FillBuffer(stream, sizeof(Int64));
                value = converter.ToInt64(Buffer);
            }
            else if (valueType == typeof(UInt16))
            {
                FillBuffer(stream, sizeof(UInt16));
                value = converter.ToUInt16(Buffer);
            }
            else if (valueType == typeof(UInt32))
            {
                FillBuffer(stream, sizeof(UInt32));
                value = converter.ToUInt32(Buffer);
            }
            else if (valueType == typeof(UInt64))
            {
                FillBuffer(stream, sizeof(UInt64));
                value = converter.ToUInt64(Buffer);
            }
            else
            {
                throw new NotImplementedException($"Unsupported enum type {valueType}.");
            }

            // Check if the value is defined in the enumeration, if requested.
            if (strict)
                ValidateEnumValue(type, value);
            return value;
        }

        /// <summary>
        /// Returns an <see cref="Enum"/> instance of the given <paramref name="type"/> read asynchronously from the
        /// <paramref name="stream"/>.
        /// </summary>
        /// <param name="type">The type of the enum.</param>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="strict"><c>true</c> to raise an <see cref="ArgumentOutOfRangeException"/> if a value is not
        /// defined in the enum type.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The value read from the current stream.</returns>
        public static async Task<object> ReadEnumAsync(this Stream stream, Type type,
            bool strict = false, ByteConverter converter = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            converter = converter ?? ByteConverter.System;

            // Read enough bytes to form an enum value.
            Type valueType = Enum.GetUnderlyingType(type);
            object value;
            if (valueType == typeof(Byte))
            {
                await FillBufferAsync(stream, sizeof(Byte), cancellationToken);
                value = Buffer[0];
            }
            else if (valueType == typeof(SByte))
            {
                await FillBufferAsync(stream, sizeof(SByte), cancellationToken);
                value = (SByte)Buffer[0];
            }
            else if (valueType == typeof(Int16))
            {
                await FillBufferAsync(stream, sizeof(Int16), cancellationToken);
                value = converter.ToInt16(Buffer);
            }
            else if (valueType == typeof(Int32))
            {
                await FillBufferAsync(stream, sizeof(Int32), cancellationToken);
                value = converter.ToInt32(Buffer);
            }
            else if (valueType == typeof(Int64))
            {
                await FillBufferAsync(stream, sizeof(Int64), cancellationToken);
                value = converter.ToInt64(Buffer);
            }
            else if (valueType == typeof(UInt16))
            {
                await FillBufferAsync(stream, sizeof(UInt16), cancellationToken);
                value = converter.ToUInt16(Buffer);
            }
            else if (valueType == typeof(UInt32))
            {
                await FillBufferAsync(stream, sizeof(UInt32), cancellationToken);
                value = converter.ToUInt32(Buffer);
            }
            else if (valueType == typeof(UInt64))
            {
                await FillBufferAsync(stream, sizeof(UInt64), cancellationToken);
                value = converter.ToUInt64(Buffer);
            }
            else
            {
                throw new NotImplementedException($"Unsupported enum type {valueType}.");
            }

            // Check if the value is defined in the enumeration, if requested.
            if (strict)
                ValidateEnumValue(type, value);
            return value;
        }

        /// <summary>
        /// Returns an array of <see cref="Enum"/> instances of the given <paramref name="type"/> read from the
        /// <paramref name="stream"/>.
        /// </summary>
        /// <param name="type">The type of the enum.</param>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="strict"><c>true</c> to raise an <see cref="ArgumentOutOfRangeException"/> if a value is not
        /// defined in the enum type.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static object[] ReadEnums(this Stream stream, Type type, int count, bool strict = false,
            ByteConverter converter = null)
        {
            converter = converter ?? ByteConverter.System;
            return ReadMany(stream, count,
                () => ReadEnum(stream, type, strict, converter));
        }

        /// <summary>
        /// Returns an array of <see cref="Enum"/> instances of the given <paramref name="type"/> read asynchronously
        /// from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="type">The type of the enum.</param>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="strict"><c>true</c> to raise an <see cref="ArgumentOutOfRangeException"/> if a value is not
        /// defined in the enum type.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static async Task<object[]> ReadEnumsAsync(this Stream stream, Type type, int count, bool strict = false,
            ByteConverter converter = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            converter = converter ?? ByteConverter.System;
            return await ReadManyAsync(stream, count,
                () => ReadEnumAsync(stream, type, strict, converter, cancellationToken));
        }

        // ---- Write ----

        /// <summary>
        /// Writes an <see cref="Enum"/> value of type <typeparamref name="T"/> to the <paramref name="stream"/>.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="strict"><c>true</c> to raise an <see cref="ArgumentOutOfRangeException"/> if the value is not
        /// defined in the enum type.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void WriteEnum<T>(this Stream stream, T value, bool strict = false,
            ByteConverter converter = null)
            where T : struct, IComparable, IFormattable
        {
            WriteEnum(stream, typeof(T), value, strict, converter);
        }

        /// <summary>
        /// Writes an <see cref="Enum"/> value of type <typeparamref name="T"/> asynchronously to the
        /// <paramref name="stream"/>.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="strict"><c>true</c> to raise an <see cref="ArgumentOutOfRangeException"/> if the value is not
        /// defined in the enum type.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public static async Task WriteEnumAsync<T>(this Stream stream, T value, bool strict = false,
            ByteConverter converter = null, CancellationToken cancellationToken = default(CancellationToken))
            where T : struct, IComparable, IFormattable
        {
            await WriteEnumAsync(stream, typeof(T), value, strict, converter, cancellationToken);
        }

        /// <summary>
        /// Writes an <see cref="Enum"/> value of the given <paramref name="type"/> to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="type">The type of the enum.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="strict"><c>true</c> to raise an <see cref="ArgumentOutOfRangeException"/> if the value is not
        /// defined in the enum type.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void WriteEnum(this Stream stream, Type type, object value, bool strict = false,
            ByteConverter converter = null)
        {
            byte[] buffer = Buffer;
            int size = GetEnumBytes(type, value, strict, converter, buffer);
            stream.Write(buffer, 0, size);
        }

        /// <summary>
        /// Writes an <see cref="Enum"/> value of the given <paramref name="type"/> asynchronously to the
        /// <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="type">The type of the enum.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="strict"><c>true</c> to raise an <see cref="ArgumentOutOfRangeException"/> if the value is not
        /// defined in the enum type.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public static async Task WriteEnumAsync(this Stream stream, Type type, object value, bool strict = false,
            ByteConverter converter = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            byte[] buffer = Buffer;
            int size = GetEnumBytes(type, value, strict, converter ?? ByteConverter.System, buffer);
            await stream.WriteAsync(buffer, 0, size, cancellationToken);
        }

        /// <summary>
        /// Writes an enumerable of <see cref="Enum"/> values of type <typeparamref name="T"/> to the
        /// <paramref name="stream"/>.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="strict"><c>true</c> to raise an <see cref="ArgumentOutOfRangeException"/> if the value is not
        /// defined in the enum type.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void WriteEnums<T>(this Stream stream, IEnumerable values, bool strict = false,
            ByteConverter converter = null)
            where T : struct, IComparable, IFormattable
        {
            Type type = typeof(T);
            converter = converter ?? ByteConverter.System;
            foreach (var value in values)
                WriteEnum(stream, type, value, strict, converter);
        }

        /// <summary>
        /// Writes an enumerable of <see cref="Enum"/> values of type <typeparamref name="T"/> asynchronously to the
        /// <paramref name="stream"/>.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="strict"><c>true</c> to raise an <see cref="ArgumentOutOfRangeException"/> if the value is not
        /// defined in the enum type.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public static async Task WriteEnumsAsync<T>(this Stream stream, IEnumerable values, bool strict = false,
            ByteConverter converter = null, CancellationToken cancellationToken = default(CancellationToken))
            where T : struct, IComparable, IFormattable
        {
            Type type = typeof(T);
            converter = converter ?? ByteConverter.System;
            foreach (var value in values)
                await WriteEnumAsync(stream, type, value, strict, converter, cancellationToken);
        }

        /// <summary>
        /// Writes an enumerable of <see cref="Enum"/> values of the given <paramref name="type"/> to the
        /// <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="type">The type of the enum.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="strict"><c>true</c> to raise an <see cref="ArgumentOutOfRangeException"/> if the value is not
        /// defined in the enum type.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void WriteEnums(this Stream stream, Type type, IEnumerable values, bool strict = false,
            ByteConverter converter = null)
        {
            converter = converter ?? ByteConverter.System;
            foreach (var value in values)
                WriteEnum(stream, type, value, strict, converter);
        }

        /// <summary>
        /// Writes an enumerable of <see cref="Enum"/> values of the given <paramref name="type"/> asynchronously to the
        /// <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="type">The type of the enum.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="strict"><c>true</c> to raise an <see cref="ArgumentOutOfRangeException"/> if the value is not
        /// defined in the enum type.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public static async Task WriteEnumsAsync(this Stream stream, Type type, IEnumerable values, bool strict = false,
            ByteConverter converter = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            converter = converter ?? ByteConverter.System;
            foreach (var value in values)
                await WriteEnumAsync(stream, type, value, strict, converter, cancellationToken);
        }

        // ---- METHODS (PRIVATE) --------------------------------------------------------------------------------------

        private static int GetEnumBytes(Type type, object value, bool strict, ByteConverter converter, byte[] buffer)
        {
            // Check if the value is defined in the enumeration, if requested.
            if (strict)
                ValidateEnumValue(type, value);

            converter = converter ?? ByteConverter.System;
            Type valueType = Enum.GetUnderlyingType(type);

            // Get the bytes of the enum value and return the size of it.
            if (valueType == typeof(Byte))
            {
                buffer[0] = (Byte)value;
                return sizeof(Byte);
            }
            else if (valueType == typeof(SByte))
            {
                buffer[0] = (Byte)(SByte)value;
                return sizeof(SByte);
            }
            else if (valueType == typeof(Int16))
            {
                converter.GetBytes((Int16)value, buffer, 0);
                return sizeof(Int16);
            }
            else if (valueType == typeof(Int32))
            {
                converter.GetBytes((Int32)value, buffer, 0);
                return sizeof(Int32);
            }
            else if (valueType == typeof(Int64))
            {
                converter.GetBytes((Int64)value, buffer, 0);
                return sizeof(Int64);
            }
            else if (valueType == typeof(UInt16))
            {
                converter.GetBytes((UInt16)value, buffer, 0);
                return sizeof(UInt16);
            }
            else if (valueType == typeof(UInt32))
            {
                converter.GetBytes((UInt32)value, buffer, 0);
                return sizeof(UInt32);
            }
            else if (valueType == typeof(UInt64))
            {
                converter.GetBytes((UInt64)value, buffer, 0);
                return sizeof(UInt64);
            }
            else
            {
                throw new NotImplementedException($"Unsupported enum type {valueType}.");
            }
        }

        private static void ValidateEnumValue(Type enumType, object value)
        {
            if (!EnumTools.IsValid(enumType, value))
                throw new InvalidDataException($"Read value {value} is not defined in the enum type {enumType}.");
        }
    }
}

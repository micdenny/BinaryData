using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Syroot.BinaryData
{
    public static partial class StreamExtensions
    {
        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        /// <summary>
        /// Returns <paramref name="count"/> instances of type <typeparamref name="T"/> continually read from the
        /// <paramref name="stream"/> by calling the <paramref name="readFunc"/>.
        /// </summary>
        /// <typeparam name="T">The type of the instances to read.</typeparam>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of instances to read.</param>
        /// <param name="readFunc">The read callback function invoked for each instance read.</param>
        /// <returns>The array of read instances.</returns>
        public static T[] ReadMany<T>(this Stream stream, int count, Func<T> readFunc)
        {
            T[] values = new T[count];
            for (int i = 0; i < count; i++)
                values[i] = readFunc();
            return values;
        }

        /// <summary>
        /// Returns <paramref name="count"/> instances of type <typeparamref name="T"/> continually read asynchronously
        /// from the <paramref name="stream"/> by calling the <paramref name="readFunc"/>.
        /// </summary>
        /// <typeparam name="T">The type of the instances to read.</typeparam>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of instances to read.</param>
        /// <param name="readFunc">The read callback function invoked for each instance read.</param>
        /// <returns>The array of read instances.</returns>
        public static async Task<T[]> ReadManyAsync<T>(this Stream stream, int count, Func<Task<T>> readFunc)
        {
            T[] values = new T[count];
            for (int i = 0; i < count; i++)
                values[i] = await readFunc();
            return values;
        }

        // ---- 7BitInt32 ----

        /// <summary>
        /// Returns a variable-length <see cref="Int32"/> instance read from the given <paramref name="stream"/> which
        /// can require up to 5 bytes.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <returns>The value read from the current stream.</returns>
        public static Int32 Read7BitInt32(this Stream stream)
        {
            // Endianness should not matter, as this value is stored byte by byte.
            // While the highest bit is set, the integer requires another of a maximum of 5 bytes.
            int value = 0;
            for (int i = 0; i < sizeof(Int32) + 1; i++)
            {
                byte readByte = stream.Read1Byte();
                if (readByte == 0xFF)
                    throw new EndOfStreamException("Incomplete 7-bit encoded Int32.");
                value |= (readByte & 0b01111111) << i * 7;
                if ((readByte & 0b10000000) == 0)
                    return value;
            }
            throw new InvalidDataException("Invalid 7-bit encoded Int32.");
        }

        /// <summary>
        /// Returns a variable-length <see cref="Int32"/> instance read asynchronously from the given
        /// <paramref name="stream"/> which can require up to 5 bytes.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The value read from the current stream.</returns>
        public static async Task<Int32> Read7BitInt32Async(this Stream stream,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            // Endianness should not matter, as this value is stored byte by byte.
            // While the highest bit is set, the integer requires another of a maximum of 5 bytes.
            int value = 0;
            for (int i = 0; i < sizeof(Int32) + 1; i++)
            {
                byte readByte = await stream.Read1ByteAsync(cancellationToken);
                if (readByte == 0xFF)
                    throw new EndOfStreamException("Incomplete 7-bit encoded Int32.");
                value |= (readByte & 0b01111111) << i * 7;
                if ((readByte & 0b10000000) == 0)
                    return value;
            }
            throw new InvalidDataException("Invalid 7-bit encoded Int32.");
        }

        /// <summary>
        /// Returns an array of variable-length <see cref="Int32"/> instances read from the <paramref name="stream"/>
        /// which can require to 5 bytes each.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static Int32[] Read7BitInt32s(this Stream stream, int count)
        {
            return ReadMany(stream, count,
                () => Read7BitInt32(stream));
        }

        /// <summary>
        /// Returns an array of variable-length <see cref="Int32"/> instances read asynchronously from the
        /// <paramref name="stream"/> which can require to 5 bytes each.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static async Task<Int32[]> Read7BitInt32sAsync(this Stream stream, int count,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ReadManyAsync(stream, count,
                () => Read7BitInt32Async(stream, cancellationToken));
        }

        // ---- Boolean ----

        /// <summary>
        /// Returns a <see cref="Boolean"/> instance read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="coding">The <see cref="BooleanCoding"/> format in which the data is stored.</param>
        /// <returns>The value read from the current stream.</returns>
        public static Boolean ReadBoolean(this Stream stream, BooleanCoding coding = BooleanCoding.Byte)
        {
            switch (coding)
            {
                case BooleanCoding.Byte:
                    return stream.ReadByte() != 0;
                case BooleanCoding.Word:
                    return ReadInt16(stream) != 0;
                case BooleanCoding.Dword:
                    return ReadInt32(stream) != 0;
                default:
                    throw new ArgumentException($"Invalid {nameof(BooleanCoding)}.", nameof(coding));
            }
        }

        /// <summary>
        /// Returns a <see cref="Boolean"/> instance read asynchronously from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="coding">The <see cref="BooleanCoding"/> format in which the data is stored.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The value read from the current stream.</returns>
        public static async Task<Boolean> ReadBooleanAsync(this Stream stream,
            BooleanCoding coding = BooleanCoding.Byte, CancellationToken cancellationToken = default(CancellationToken))
        {
            switch (coding)
            {
                case BooleanCoding.Byte:
                    return await Read1ByteAsync(stream, cancellationToken) != 0;
                case BooleanCoding.Word:
                    return await ReadInt16Async(stream, cancellationToken: cancellationToken) != 0;
                case BooleanCoding.Dword:
                    return await ReadInt32Async(stream, cancellationToken: cancellationToken) != 0;
                default:
                    throw new ArgumentException($"Invalid {nameof(BooleanCoding)}.", nameof(coding));
            }
        }

        /// <summary>
        /// Returns an array of <see cref="Boolean"/> instances read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="coding">The <see cref="BooleanCoding"/> format in which the data is stored.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static Boolean[] ReadBooleans(this Stream stream, int count, BooleanCoding coding = BooleanCoding.Byte)
        {
            return ReadMany(stream, count,
                () => stream.ReadBoolean(coding));
        }

        /// <summary>
        /// Returns an array of <see cref="Boolean"/> instances read asynchronously from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="coding">The <see cref="BooleanCoding"/> format in which the data is stored.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static async Task<Boolean[]> ReadBooleansAsync(this Stream stream, int count,
            BooleanCoding coding = BooleanCoding.Byte, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ReadManyAsync(stream, count,
                () => ReadBooleanAsync(stream, coding, cancellationToken));
        }

        // ---- Byte ----

        /// <summary>
        /// Returns a <see cref="Byte"/> instance read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <returns>The value read from the current stream.</returns>
        public static Byte Read1Byte(this Stream stream)
        {
            return (Byte)stream.ReadByte();
        }

        /// <summary>
        /// Returns a <see cref="Byte"/> instance read asynchronously from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The value read from the current stream.</returns>
        public static async Task<Byte> Read1ByteAsync(this Stream stream,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await FillBufferAsync(stream, 1, cancellationToken);
            return Buffer[0];
        }

        /// <summary>
        /// Returns an array of <see cref="Byte"/> instances read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static Byte[] ReadBytes(this Stream stream, int count)
        {
            byte[] buffer = new byte[count];
            int bytesRead = stream.Read(buffer, 0, count);
            if (bytesRead < count)
                throw new EndOfStreamException($"Could not read {count} bytes.");
            return buffer;
        }

        /// <summary>
        /// Returns an array of <see cref="Byte"/> instances read asynchronously from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static async Task<Byte[]> ReadBytesAsync(this Stream stream, int count,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            byte[] buffer = new byte[count];
            int bytesRead = await stream.ReadAsync(buffer, 0, count, cancellationToken);
            if (bytesRead < count)
                throw new EndOfStreamException($"Could not read {count} bytes.");
            return buffer;
        }

        // ---- DateTime ----

        /// <summary>
        /// Returns a <see cref="DateTime"/> instance read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="coding">The <see cref="DateTimeCoding"/> format in which the data is stored.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <returns>The value read from the current stream.</returns>
        public static DateTime ReadDateTime(this Stream stream, DateTimeCoding coding = DateTimeCoding.NetTicks,
            ByteConverter converter = null)
        {
            switch (coding)
            {
                case DateTimeCoding.NetTicks:
                    return ReadDateTimeAsNetTicks(stream, converter);
                case DateTimeCoding.CTime:
                    return ReadDateTimeAsCTime(stream, converter);
                case DateTimeCoding.CTime64:
                    return ReadDateTimeAsCTime64(stream, converter);
                default:
                    throw new ArgumentException($"Invalid {nameof(DateTimeCoding)}.", nameof(coding));
            }
        }

        /// <summary>
        /// Returns a <see cref="DateTime"/> instance read asynchronously from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="coding">The <see cref="DateTimeCoding"/> format in which the data is stored.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The value read from the current stream.</returns>
        public static async Task<DateTime> ReadDateTimeAsync(this Stream stream,
            DateTimeCoding coding = DateTimeCoding.NetTicks, ByteConverter converter = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            switch (coding)
            {
                case DateTimeCoding.NetTicks:
                    return await ReadDateTimeAsNetTicksAsync(stream, converter, cancellationToken);
                case DateTimeCoding.CTime:
                    return await ReadDateTimeAsCTimeAsync(stream, converter, cancellationToken);
                case DateTimeCoding.CTime64:
                    return await ReadDateTimeAsCTime64Async(stream, converter, cancellationToken);
                default:
                    throw new ArgumentException($"Invalid {nameof(DateTimeCoding)}.", nameof(coding));
            }
        }

        /// <summary>
        /// Returns an array of <see cref="DateTime"/> instances read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="coding">The <see cref="DateTimeCoding"/> format in which the data is stored.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static DateTime[] ReadDateTimes(this Stream stream, int count,
            DateTimeCoding coding = DateTimeCoding.NetTicks, ByteConverter converter = null)
        {
            converter = converter ?? ByteConverter.System;
            return ReadMany(stream, count,
                () => ReadDateTime(stream, coding, converter));
        }

        /// <summary>
        /// Returns an array of <see cref="DateTime"/> instances read asynchronously from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="coding">The <see cref="DateTimeCoding"/> format in which the data is stored.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static async Task<DateTime[]> ReadDateTimesAsync(this Stream stream, int count,
            DateTimeCoding coding = DateTimeCoding.NetTicks, ByteConverter converter = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            converter = converter ?? ByteConverter.System;
            return await ReadManyAsync(stream, count,
                () => ReadDateTimeAsync(stream, coding, converter, cancellationToken));
        }

        // ---- Decimal ----

        /// <summary>
        /// Returns a <see cref="Decimal"/> instance read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <returns>The value read from the current stream.</returns>
        public static Decimal ReadDecimal(this Stream stream, ByteConverter converter = null)
        {
            FillBuffer(stream, sizeof(Decimal));
            return (converter ?? ByteConverter.System).ToDecimal(Buffer);
        }

        /// <summary>
        /// Returns a <see cref="Decimal"/> instance read asynchronously from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The value read from the current stream.</returns>
        public static async Task<Decimal> ReadDecimalAsync(this Stream stream, ByteConverter converter = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await FillBufferAsync(stream, sizeof(Decimal), cancellationToken);
            return (converter ?? ByteConverter.System).ToDecimal(Buffer);
        }

        /// <summary>
        /// Returns an array of <see cref="Decimal"/> instances read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static Decimal[] ReadDecimals(this Stream stream, int count, ByteConverter converter = null)
        {
            converter = converter ?? ByteConverter.System;
            return ReadMany(stream, count,
                () => ReadDecimal(stream, converter));
        }

        /// <summary>
        /// Returns an array of <see cref="Decimal"/> instances read asynchronously from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static async Task<Decimal[]> ReadDecimalsAsync(this Stream stream, int count,
            ByteConverter converter = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            converter = converter ?? ByteConverter.System;
            return await ReadManyAsync(stream, count,
                () => ReadDecimalAsync(stream, converter, cancellationToken));
        }

        // ---- Double ----

        /// <summary>
        /// Returns a <see cref="Double"/> instance read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <returns>The value read from the current stream.</returns>
        public static Double ReadDouble(this Stream stream,
            ByteConverter converter = null)
        {
            FillBuffer(stream, sizeof(Double));
            return (converter ?? ByteConverter.System).ToDouble(Buffer);
        }

        /// <summary>
        /// Returns a <see cref="Double"/> instance read asynchronously from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The value read from the current stream.</returns>
        public static async Task<Double> ReadDoubleAsync(this Stream stream,
            ByteConverter converter = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            await FillBufferAsync(stream, sizeof(Double), cancellationToken);
            return (converter ?? ByteConverter.System).ToDouble(Buffer);
        }

        /// <summary>
        /// Returns an array of <see cref="Double"/> instances read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static Double[] ReadDoubles(this Stream stream, int count, ByteConverter converter = null)
        {
            converter = converter ?? ByteConverter.System;
            return ReadMany(stream, count,
                () => ReadDouble(stream, converter));
        }

        /// <summary>
        /// Returns an array of <see cref="Double"/> instances read asynchronously from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static async Task<Double[]> ReadDoublesAsync(this Stream stream, int count,
            ByteConverter converter = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            converter = converter ?? ByteConverter.System;
            return await ReadManyAsync(stream, count,
                () => ReadDoubleAsync(stream, converter, cancellationToken));
        }

        // ---- Enum ----

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
                () => ReadEnum<T>(stream, strict, converter));
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
            ByteConverter converter = null,CancellationToken cancellationToken = default(CancellationToken))
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

        // ---- Int16 ----

        /// <summary>
        /// Returns an <see cref="Int16"/> instance read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <returns>The value read from the current stream.</returns>
        public static Int16 ReadInt16(this Stream stream, ByteConverter converter = null)
        {
            FillBuffer(stream, sizeof(Int16));
            return (converter ?? ByteConverter.System).ToInt16(Buffer);
        }

        /// <summary>
        /// Returns an <see cref="Int16"/> instance read asynchronously from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The value read from the current stream.</returns>
        public static async Task<Int16> ReadInt16Async(this Stream stream, ByteConverter converter = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await FillBufferAsync(stream, sizeof(Int16), cancellationToken);
            return (converter ?? ByteConverter.System).ToInt16(Buffer);
        }

        /// <summary>
        /// Returns an array of <see cref="Int16"/> instances read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static Int16[] ReadInt16s(this Stream stream, int count, ByteConverter converter = null)
        {
            converter = converter ?? ByteConverter.System;
            return ReadMany(stream, count,
                () => ReadInt16(stream, converter));
        }

        /// <summary>
        /// Returns an array of <see cref="Int16"/> instances read asynchronously from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static async Task<Int16[]> ReadInt16sAsync(this Stream stream, int count, ByteConverter converter = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            converter = converter ?? ByteConverter.System;
            return await ReadManyAsync(stream, count,
                () => ReadInt16Async(stream, converter, cancellationToken));
        }

        // ---- Int32 ----

        /// <summary>
        /// Returns an <see cref="Int32"/> instance read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <returns>The value read from the current stream.</returns>
        public static Int32 ReadInt32(this Stream stream, ByteConverter converter = null)
        {
            FillBuffer(stream, sizeof(Int32));
            return (converter ?? ByteConverter.System).ToInt32(Buffer);
        }

        /// <summary>
        /// Returns an <see cref="Int32"/> instance read asynchronously from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The value read from the current stream.</returns>
        public static async Task<Int32> ReadInt32Async(this Stream stream, ByteConverter converter = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await FillBufferAsync(stream, sizeof(Int32), cancellationToken);
            return (converter ?? ByteConverter.System).ToInt32(Buffer);
        }

        /// <summary>
        /// Returns an array of <see cref="Int32"/> instances read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static Int32[] ReadInt32s(this Stream stream, int count, ByteConverter converter = null)
        {
            converter = converter ?? ByteConverter.System;
            return ReadMany(stream, count,
                () => ReadInt32(stream, converter));
        }

        /// <summary>
        /// Returns an array of <see cref="Int32"/> instances read asynchronously from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static async Task<Int32[]> ReadInt32sAsync(this Stream stream, int count, ByteConverter converter = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            converter = converter ?? ByteConverter.System;
            return await ReadManyAsync(stream, count,
                () => ReadInt32Async(stream, converter, cancellationToken));
        }

        // ---- Int64 ----

        /// <summary>
        /// Returns an <see cref="Int64"/> instance read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <returns>The value read from the current stream.</returns>
        public static Int64 ReadInt64(this Stream stream, ByteConverter converter = null)
        {
            FillBuffer(stream, sizeof(Int64));
            return (converter ?? ByteConverter.System).ToInt64(Buffer);
        }

        /// <summary>
        /// Returns an <see cref="Int64"/> instance read asynchronously from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The value read from the current stream.</returns>
        public static async Task<Int64> ReadInt64Async(this Stream stream, ByteConverter converter = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await FillBufferAsync(stream, sizeof(Int64), cancellationToken);
            return (converter ?? ByteConverter.System).ToInt64(Buffer);
        }

        /// <summary>
        /// Returns an array of <see cref="Int64"/> instances read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static Int64[] ReadInt64s(this Stream stream, int count, ByteConverter converter = null)
        {
            converter = converter ?? ByteConverter.System;
            return ReadMany(stream, count,
                () => ReadInt64(stream, converter));
        }

        /// <summary>
        /// Returns an array of <see cref="Int64"/> instances read asynchronously from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static async Task<Int64[]> ReadInt64sAsync(this Stream stream, int count,
            ByteConverter converter = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            converter = converter ?? ByteConverter.System;
            return await ReadManyAsync(stream, count,
                () => ReadInt64Async(stream, converter, cancellationToken));
        }

        // ---- SByte ----
        
        /// <summary>
        /// Returns an <see cref="SByte"/> instance read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <returns>The value read from the current stream.</returns>
        public static SByte ReadSByte(this Stream stream)
        {
            return (SByte)stream.ReadByte();
        }

        /// <summary>
        /// Returns an <see cref="SByte"/> instance read asynchronously from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The value read from the current stream.</returns>
        public static async Task<SByte> ReadSByteAsync(this Stream stream,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return (SByte)await stream.Read1ByteAsync(cancellationToken);
        }

        /// <summary>
        /// Returns an array of <see cref="SByte"/> instances read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static SByte[] ReadSBytes(this Stream stream, int count)
        {
            return ReadMany(stream, count,
                () => ReadSByte(stream));
        }

        /// <summary>
        /// Returns an array of <see cref="SByte"/> instances read asynchronously from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static async Task<SByte[]> ReadSBytesAsync(this Stream stream, int count,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ReadManyAsync(stream, count,
                () => ReadSByteAsync(stream, cancellationToken));
        }

        // ---- Single ----

        /// <summary>
        /// Returns a <see cref="Single"/> instance read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <returns>The value read from the current stream.</returns>
        public static Single ReadSingle(this Stream stream, ByteConverter converter = null)
        {
            FillBuffer(stream, sizeof(Single));
            return (converter ?? ByteConverter.System).ToSingle(Buffer);
        }

        /// <summary>
        /// Returns a <see cref="Single"/> instance read asynchronously from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The value read from the current stream.</returns>
        public static async Task<Single> ReadSingleAsync(this Stream stream, ByteConverter converter = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await FillBufferAsync(stream, sizeof(Single), cancellationToken);
            return (converter ?? ByteConverter.System).ToSingle(Buffer);
        }

        /// <summary>
        /// Returns an array of <see cref="Single"/> instances read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static Single[] ReadSingles(this Stream stream, int count, ByteConverter converter = null)
        {
            converter = converter ?? ByteConverter.System;
            return ReadMany(stream, count,
                () => ReadSingle(stream, converter));
        }

        /// <summary>
        /// Returns an array of <see cref="Single"/> instances read asynchronously from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static async Task<Single[]> ReadSinglesAsync(this Stream stream, int count,
            ByteConverter converter = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            converter = converter ?? ByteConverter.System;
            return await ReadManyAsync(stream, count,
                () => ReadSingleAsync(stream, converter, cancellationToken));
        }

        // ---- String ----

        /// <summary>
        /// Returns a <see cref="String"/> instance read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="coding">The <see cref="StringCoding"/> format determining how the length of the string is
        /// stored.</param>
        /// <param name="encoding">The <see cref="Encoding"/> to parse the bytes with, or <c>null</c> to use
        /// <see cref="Encoding.UTF8"/>.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <returns>The value read from the current stream.</returns>
        public static String ReadString(this Stream stream, StringCoding coding = StringCoding.VariableByteCount,
            Encoding encoding = null, ByteConverter converter = null)
        {
            encoding = encoding ?? Encoding.UTF8;
            converter = converter ?? ByteConverter.System;
            switch (coding)
            {
                case StringCoding.VariableByteCount:
                    return ReadStringWithLength(stream, stream.Read7BitInt32(), false, encoding);
                case StringCoding.ByteCharCount:
                    return ReadStringWithLength(stream, stream.ReadByte(), true, encoding);
                case StringCoding.Int16CharCount:
                    return ReadStringWithLength(stream, ReadInt16(stream, converter), true, encoding);
                case StringCoding.Int32CharCount:
                    return ReadStringWithLength(stream, ReadInt32(stream, converter), true, encoding);
                case StringCoding.ZeroTerminated:
                    return ReadStringZeroPostfix(stream, encoding);
                default:
                    throw new ArgumentException($"Invalid {nameof(StringCoding)}.", nameof(coding));
            }
        }

        /// <summary>
        /// Returns a <see cref="String"/> instance read asynchronously from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="coding">The <see cref="StringCoding"/> format determining how the length of the string is
        /// stored.</param>
        /// <param name="encoding">The <see cref="Encoding"/> to parse the bytes with, or <c>null</c> to use
        /// <see cref="Encoding.UTF8"/>.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The value read from the current stream.</returns>
        public static async Task<String> ReadStringAsync(this Stream stream,
            StringCoding coding = StringCoding.VariableByteCount, Encoding encoding = null,
            ByteConverter converter = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            encoding = encoding ?? Encoding.UTF8;
            converter = converter ?? ByteConverter.System;
            switch (coding)
            {
                case StringCoding.VariableByteCount:
                    return await ReadStringWithLengthAsync(stream, stream.Read7BitInt32(), false, encoding,
                        cancellationToken);
                case StringCoding.ByteCharCount:
                    return await ReadStringWithLengthAsync(stream, stream.ReadByte(), true, encoding,
                        cancellationToken);
                case StringCoding.Int16CharCount:
                    return await ReadStringWithLengthAsync(stream, ReadInt16(stream, converter), true, encoding,
                        cancellationToken);
                case StringCoding.Int32CharCount:
                    return await ReadStringWithLengthAsync(stream, ReadInt32(stream, converter), true, encoding,
                        cancellationToken);
                case StringCoding.ZeroTerminated:
                    return await ReadStringZeroPostfixAsync(stream, encoding, cancellationToken);
                default:
                    throw new ArgumentException($"Invalid {nameof(StringCoding)}.", nameof(coding));
            }
        }

        /// <summary>
        /// Returns an array of <see cref="String"/> instances read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="coding">The <see cref="StringCoding"/> format determining how the length of the strings is
        /// stored.</param>
        /// <param name="encoding">The <see cref="Encoding"/> to parse the bytes with, or <c>null</c> to use
        /// <see cref="Encoding.UTF8"/>.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static String[] ReadStrings(this Stream stream, int count,
            StringCoding coding = StringCoding.VariableByteCount, Encoding encoding = null,
            ByteConverter converter = null)
        {
            converter = converter ?? ByteConverter.System;
            return ReadMany(stream, count,
                () => ReadString(stream, coding, encoding, converter));
        }

        /// <summary>
        /// Returns an array of <see cref="String"/> instances read asynchronously from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="coding">The <see cref="StringCoding"/> format determining how the length of the strings is
        /// stored.</param>
        /// <param name="encoding">The <see cref="Encoding"/> to parse the bytes with, or <c>null</c> to use
        /// <see cref="Encoding.UTF8"/>.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static async Task<String[]> ReadStringsAsync(this Stream stream, int count,
            StringCoding coding = StringCoding.VariableByteCount, Encoding encoding = null,
            ByteConverter converter = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            converter = converter ?? ByteConverter.System;
            return await ReadManyAsync(stream, count,
                () => ReadStringAsync(stream, coding, encoding, converter, cancellationToken));
        }

        /// <summary>
        /// Returns a <see cref="String"/> instance read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="length">The length of the string.</param>
        /// <param name="encoding">The <see cref="Encoding"/> to parse the decode the chars with, or <c>null</c> to use
        /// <see cref="Encoding.UTF8"/>.</param>
        /// <returns>The value read from the current stream.</returns>
        public static String ReadString(this Stream stream, int length, Encoding encoding = null)
        {
            return ReadStringWithLength(stream, length, true, encoding ?? Encoding.UTF8);
        }

        /// <summary>
        /// Returns a <see cref="String"/> instance read asynchronously from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="length">The length of the string.</param>
        /// <param name="encoding">The <see cref="Encoding"/> to parse the decode the chars with, or <c>null</c> to use
        /// <see cref="Encoding.UTF8"/>.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The value read from the current stream.</returns>
        public static async Task<String> ReadStringAsync(this Stream stream, int length, Encoding encoding = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ReadStringWithLengthAsync(stream, length, true, encoding ?? Encoding.UTF8, cancellationToken);
        }

        /// <summary>
        /// Returns an array of <see cref="String"/> instances read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="length">The length of the string.</param>
        /// <param name="encoding">The <see cref="Encoding"/> to parse the bytes with, or <c>null</c> to use
        /// <see cref="Encoding.UTF8"/>.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static String[] ReadStrings(this Stream stream, int count, int length, Encoding encoding = null)
        {
            return ReadMany(stream, count,
                () => ReadStringWithLength(stream, length, true, encoding));
        }

        /// <summary>
        /// Returns an array of <see cref="String"/> instances read asynchronously from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="length">The length of the string.</param>
        /// <param name="encoding">The <see cref="Encoding"/> to parse the bytes with, or <c>null</c> to use
        /// <see cref="Encoding.UTF8"/>.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static async Task<String[]> ReadStringsAsync(this Stream stream, int count, int length,
            Encoding encoding = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ReadManyAsync(stream, count,
                () => ReadStringWithLengthAsync(stream, length, true, encoding, cancellationToken));
        }

        // ---- UInt16 ----

        /// <summary>
        /// Returns a <see cref="UInt16"/> instance read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <returns>The value read from the current stream.</returns>
        public static UInt16 ReadUInt16(this Stream stream, ByteConverter converter = null)
        {
            FillBuffer(stream, sizeof(UInt16));
            return (converter ?? ByteConverter.System).ToUInt16(Buffer);
        }

        /// <summary>
        /// Returns a <see cref="UInt16"/> instance read asynchronously from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The value read from the current stream.</returns>
        public static async Task<UInt16> ReadUInt16Async(this Stream stream, ByteConverter converter = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await FillBufferAsync(stream, sizeof(UInt16), cancellationToken);
            return (converter ?? ByteConverter.System).ToUInt16(Buffer);
        }

        /// <summary>
        /// Returns an array of <see cref="UInt16"/> instances read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static UInt16[] ReadUInt16s(this Stream stream, int count, ByteConverter converter = null)
        {
            converter = converter ?? ByteConverter.System;
            return ReadMany(stream, count,
                () => ReadUInt16(stream, converter));
        }

        /// <summary>
        /// Returns an array of <see cref="UInt16"/> instances read asynchronously from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static async Task<UInt16[]> ReadUInt16sAsync(this Stream stream, int count,
            ByteConverter converter = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            converter = converter ?? ByteConverter.System;
            return await ReadManyAsync(stream, count,
                () => ReadUInt16Async(stream, converter, cancellationToken));
        }

        // ---- UInt32 ----

        /// <summary>
        /// Returns a <see cref="UInt32"/> instance read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <returns>The value read from the current stream.</returns>
        public static UInt32 ReadUInt32(this Stream stream, ByteConverter converter = null)
        {
            FillBuffer(stream, sizeof(UInt32));
            return (converter ?? ByteConverter.System).ToUInt32(Buffer);
        }

        /// <summary>
        /// Returns a <see cref="UInt32"/> instance read asynchronously from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The value read from the current stream.</returns>
        public static async Task<UInt32> ReadUInt32Async(this Stream stream, ByteConverter converter = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await FillBufferAsync(stream, sizeof(UInt32), cancellationToken);
            return (converter ?? ByteConverter.System).ToUInt32(Buffer);
        }

        /// <summary>
        /// Returns an array of <see cref="UInt32"/> instances read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static UInt32[] ReadUInt32s(this Stream stream, int count, ByteConverter converter = null)
        {
            converter = converter ?? ByteConverter.System;
            return ReadMany(stream, count,
                () => ReadUInt32(stream, converter));
        }

        /// <summary>
        /// Returns an array of <see cref="UInt32"/> instances read asynchronously from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static async Task<UInt32[]> ReadUInt32sAsync(this Stream stream, int count,
            ByteConverter converter = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            converter = converter ?? ByteConverter.System;
            return await ReadManyAsync(stream, count,
                () => ReadUInt32Async(stream, converter, cancellationToken));
        }

        // ---- UInt64 ----

        /// <summary>
        /// Returns a <see cref="UInt64"/> instance read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <returns>The value read from the current stream.</returns>
        public static UInt64 ReadUInt64(this Stream stream, ByteConverter converter = null)
        {
            FillBuffer(stream, sizeof(UInt64));
            return (converter ?? ByteConverter.System).ToUInt64(Buffer);
        }

        /// <summary>
        /// Returns a <see cref="UInt64"/> instance read asynchronously from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The value read from the current stream.</returns>
        public static async Task<UInt64> ReadUInt64Async(this Stream stream, ByteConverter converter = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await FillBufferAsync(stream, sizeof(UInt64), cancellationToken);
            return (converter ?? ByteConverter.System).ToUInt64(Buffer);
        }

        /// <summary>
        /// Returns an array of <see cref="UInt64"/> instances read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static UInt64[] ReadUInt64s(this Stream stream, int count, ByteConverter converter = null)
        {
            converter = converter ?? ByteConverter.System;
            return ReadMany(stream, count,
                () => ReadUInt64(stream, converter));
        }

        /// <summary>
        /// Returns an array of <see cref="UInt64"/> instances read asynchronously from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static async Task<UInt64[]> ReadUInt64sAsync(this Stream stream, int count,
            ByteConverter converter = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ReadManyAsync(stream, count,
                () => ReadUInt64Async(stream, converter, cancellationToken));
        }

        // ---- METHODS (PRIVATE) --------------------------------------------------------------------------------------

        private static void FillBuffer(Stream stream, int length)
        {
            if (stream.Read(Buffer, 0, length) < length)
                throw new EndOfStreamException($"Could not read {length} bytes.");
        }

        private static async Task FillBufferAsync(Stream stream, int length, CancellationToken cancellationToken)
        {
            if (await stream.ReadAsync(Buffer, 0, length, cancellationToken) < length)
                throw new EndOfStreamException($"Could not read {length} bytes.");
        }

        // ---- DateTime ----

        private static DateTime ReadDateTimeAsCTime(Stream stream, ByteConverter converter)
        {
            return _cTimeBase.AddSeconds(ReadUInt32(stream, converter));
        }

        private static async Task<DateTime> ReadDateTimeAsCTimeAsync(Stream stream, ByteConverter converter,
            CancellationToken cancellationToken)
        {
            return _cTimeBase.AddSeconds(await ReadUInt32Async(stream, converter, cancellationToken));
        }

        private static DateTime ReadDateTimeAsCTime64(Stream stream, ByteConverter converter)
        {
            return _cTimeBase.AddSeconds(ReadUInt64(stream, converter));
        }

        private static async Task<DateTime> ReadDateTimeAsCTime64Async(Stream stream, ByteConverter converter,
            CancellationToken cancellationToken)
        {
            return _cTimeBase.AddSeconds(await ReadInt64Async(stream, converter, cancellationToken));
        }

        private static DateTime ReadDateTimeAsNetTicks(Stream stream, ByteConverter converter)
        {
            return new DateTime(ReadInt64(stream, converter));
        }

        private static async Task<DateTime> ReadDateTimeAsNetTicksAsync(Stream stream, ByteConverter converter,
            CancellationToken cancellationToken)
        {
            return new DateTime(await ReadInt64Async(stream, converter, cancellationToken));
        }

        // ---- String ----

        private static string ReadStringWithLength(Stream stream, int length, bool lengthInChars, Encoding encoding)
        {
            if (length == 0)
                return String.Empty;

            Decoder decoder = encoding.GetDecoder();
            StringBuilder builder = new StringBuilder(length);
            int totalBytesRead = 0;
            lock (stream)
            {
                byte[] buffer = Buffer;
                char[] charBuffer = CharBuffer;
                do
                {
                    int bufferOffset = 0;
                    int charsDecoded = 0;
                    while (charsDecoded == 0)
                    {
                        // Read raw bytes from the stream.
                        int bytesRead = stream.Read(buffer, bufferOffset++, 1);
                        if (bytesRead == 0)
                            throw new EndOfStreamException("Incomplete string data, missing requested length.");
                        totalBytesRead += bytesRead;
                        // Convert the bytes to chars and append them to the string being built.
                        charsDecoded = decoder.GetCharCount(buffer, 0, bufferOffset);
                        if (charsDecoded > 0)
                        {
                            decoder.GetChars(buffer, 0, bufferOffset, charBuffer, 0);
                            builder.Append(charBuffer, 0, charsDecoded);
                        }
                    }
                } while ((lengthInChars && builder.Length < length) || (!lengthInChars && totalBytesRead < length));
            }
            return builder.ToString();
        }

        private static async Task<string> ReadStringWithLengthAsync(Stream stream, int length, bool lengthInChars, 
            Encoding encoding, CancellationToken cancellationToken)
        {
            if (length == 0)
                return String.Empty;

            Decoder decoder = encoding.GetDecoder();
            StringBuilder builder = new StringBuilder(length);
            int totalBytesRead = 0;
            byte[] buffer = Buffer;
            char[] charBuffer = CharBuffer;
            do
            {
                int bufferOffset = 0;
                int charsDecoded = 0;
                while (charsDecoded == 0)
                {
                    // Read raw bytes from the stream.
                    int bytesRead = await stream.ReadAsync(buffer, bufferOffset++, 1, cancellationToken);
                    if (bytesRead == 0)
                        throw new EndOfStreamException("Incomplete string data, missing requested length.");
                    totalBytesRead += bytesRead;
                    // Convert the bytes to chars and append them to the string being built.
                    charsDecoded = decoder.GetCharCount(buffer, 0, bufferOffset);
                    if (charsDecoded > 0)
                    {
                        decoder.GetChars(buffer, 0, bufferOffset, charBuffer, 0);
                        builder.Append(charBuffer, 0, charsDecoded);
                    }
                }
            } while ((lengthInChars && builder.Length < length) || (!lengthInChars && totalBytesRead < length));
            return builder.ToString();
        }

        private static string ReadStringZeroPostfix(Stream stream, Encoding encoding)
        {
            // Read byte or word values until a 0 value is found (no encoding's char surrogate should consist of 0).
            // Endianness depends on encoding, not the actual values.
            List<byte> bytes = new List<byte>();
            bool isChar = true;
            byte[] buffer = Buffer;
            lock (stream)
            {
                switch (encoding.GetByteCount("A"))
                {
                    case sizeof(Byte):
                        // Read single bytes.
                        while (isChar)
                        {
                            FillBuffer(stream, sizeof(Byte));
                            if (isChar = buffer[0] != 0)
                                bytes.Add(buffer[0]);
                        }
                        break;
                    case sizeof(Int16):
                        // Read word values of 2 bytes width.
                        while (isChar)
                        {
                            FillBuffer(stream, sizeof(Int16));
                            if (isChar = buffer[0] != 0 || buffer[1] != 0)
                            {
                                bytes.Add(buffer[0]);
                                bytes.Add(buffer[1]);
                            }
                        }
                        break;
                    default:
                        throw new NotImplementedException(
                            "Unhandled character byte count. Only 1- or 2-byte encodings are support at the moment.");
                }
            }
            // Convert to string.
            return encoding.GetString(bytes.ToArray());
        }

        private static async Task<string> ReadStringZeroPostfixAsync(Stream stream, Encoding encoding,
            CancellationToken cancellationToken)
        {
            // Read byte or word values until a 0 value is found (no encoding's char surrogate should consist of 0).
            // Endianness depends on encoding, not the actual values.
            List<byte> bytes = new List<byte>();
            bool isChar = true;
            byte[] buffer = Buffer;
            switch (encoding.GetByteCount("A"))
            {
                case sizeof(Byte):
                    // Read single bytes.
                    while (isChar)
                    {
                        await FillBufferAsync(stream, sizeof(Byte), cancellationToken);
                        if (isChar = buffer[0] != 0)
                            bytes.Add(buffer[0]);
                    }
                    break;
                case sizeof(Int16):
                    // Read word values of 2 bytes width.
                    while (isChar)
                    {
                        await FillBufferAsync(stream, sizeof(Int16), cancellationToken);
                        if (isChar = buffer[0] != 0 || buffer[1] != 0)
                        {
                            bytes.Add(buffer[0]);
                            bytes.Add(buffer[1]);
                        }
                    }
                    break;
                default:
                    throw new NotImplementedException(
                        "Unhandled character byte count. Only 1- or 2-byte encodings are support at the moment.");
            }
            // Convert to string.
            return encoding.GetString(bytes.ToArray());
        }
    }
}

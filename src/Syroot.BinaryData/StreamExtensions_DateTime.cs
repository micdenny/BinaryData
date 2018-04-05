using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Syroot.BinaryData
{
    public static partial class StreamExtensions
    {
        // ---- FIELDS -------------------------------------------------------------------------------------------------

        private static readonly DateTime _cTimeBase = new DateTime(1970, 1, 1);

        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        // ---- Read ----

        /// <summary>
        /// Returns a <see cref="DateTime"/> instance read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="coding">The <see cref="DateTimeCoding"/> in which the data is stored.</param>
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
        /// <param name="coding">The <see cref="DateTimeCoding"/> in which the data is stored.</param>
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
        /// <param name="coding">The <see cref="DateTimeCoding"/> in which the data is stored.</param>
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
        /// <param name="coding">The <see cref="DateTimeCoding"/> in which the data is stored.</param>
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

        // ---- Write ----

        /// <summary>
        /// Writes a <see cref="DateTime"/> value to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="coding">The <see cref="DateTimeCoding"/> in which the data is stored.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void Write(this Stream stream, DateTime value, DateTimeCoding coding = DateTimeCoding.NetTicks,
            ByteConverter converter = null)
        {
            converter = converter ?? ByteConverter.System;
            switch (coding)
            {
                case DateTimeCoding.NetTicks:
                    Write(stream, value.Ticks, converter);
                    break;
                case DateTimeCoding.CTime:
                    Write(stream, (UInt32)GetCTimeTicks(value), converter);
                    break;
                case DateTimeCoding.CTime64:
                    Write(stream, (UInt64)GetCTimeTicks(value), converter);
                    break;
                default:
                    throw new ArgumentException($"Invalid {nameof(DateTimeCoding)}.", nameof(coding));
            }
        }

        /// <summary>
        /// Writes an enumerable of <see cref="DateTime"/> values to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="coding">The <see cref="DateTimeCoding"/> in which the data is stored.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void Write(this Stream stream, IEnumerable<DateTime> values,
            DateTimeCoding coding = DateTimeCoding.NetTicks, ByteConverter converter = null)
        {
            converter = converter ?? ByteConverter.System;
            foreach (var value in values)
                Write(stream, value, coding, converter);
        }

        /// <summary>
        /// Writes a <see cref="DateTime"/> value asynchronously to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="coding">The <see cref="DateTimeCoding"/> in which the data is stored.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public static async Task WriteAsync(this Stream stream, DateTime value,
            DateTimeCoding coding = DateTimeCoding.NetTicks, ByteConverter converter = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            converter = converter ?? ByteConverter.System;
            switch (coding)
            {
                case DateTimeCoding.NetTicks:
                    await WriteAsync(stream, value.Ticks, converter, cancellationToken);
                    break;
                case DateTimeCoding.CTime:
                    await WriteAsync(stream, (UInt32)GetCTimeTicks(value), converter,
                        cancellationToken);
                    break;
                case DateTimeCoding.CTime64:
                    await WriteAsync(stream, (UInt64)GetCTimeTicks(value), converter,
                        cancellationToken);
                    break;
                default:
                    throw new ArgumentException($"Invalid {nameof(DateTimeCoding)}.", nameof(coding));
            }
        }

        /// <summary>
        /// Writes an enumerable of <see cref="DateTime"/> asynchronously values to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="coding">The <see cref="DateTimeCoding"/> in which the data is stored.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public static async Task WriteAsync(this Stream stream, IEnumerable<DateTime> values,
            DateTimeCoding coding = DateTimeCoding.NetTicks, ByteConverter converter = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            converter = converter ?? ByteConverter.System;
            foreach (var value in values)
                await WriteAsync(stream, value, coding, converter, cancellationToken);
        }

        /// <summary>
        /// Writes a <see cref="DateTime"/> value to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="coding">The <see cref="DateTimeCoding"/> in which the data is stored.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void WriteDateTime(this Stream stream, DateTime value,
            DateTimeCoding coding = DateTimeCoding.NetTicks, ByteConverter converter = null)
        {
            Write(stream, value, coding, converter);
        }

        /// <summary>
        /// Writes a <see cref="DateTime"/> value asynchronously to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="coding">The <see cref="DateTimeCoding"/> in which the data is stored.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public static async Task WriteDateTimeAsync(this Stream stream, DateTime value,
            DateTimeCoding coding = DateTimeCoding.NetTicks, ByteConverter converter = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await WriteAsync(stream, value, coding, converter, cancellationToken);
        }

        /// <summary>
        /// Writes an enumerable of <see cref="DateTime"/> values to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="coding">The <see cref="DateTimeCoding"/> in which the data is stored.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void WriteDateTimes(this Stream stream, IEnumerable<DateTime> values,
            DateTimeCoding coding = DateTimeCoding.NetTicks, ByteConverter converter = null)
        {
            Write(stream, values, coding, converter);
        }

        /// <summary>
        /// Writes an enumerable of <see cref="DateTime"/> values asynchronously to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="coding">The <see cref="DateTimeCoding"/> in which the data is stored.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public static async Task WriteDateTimesAsync(this Stream stream, IEnumerable<DateTime> values,
            DateTimeCoding coding = DateTimeCoding.NetTicks, ByteConverter converter = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await WriteAsync(stream, values, coding, converter, cancellationToken);
        }

        // ---- METHODS (PRIVATE) --------------------------------------------------------------------------------------

        private static Double GetCTimeTicks(DateTime value)
        {
            return (_cTimeBase - value).TotalSeconds;
        }

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
    }
}

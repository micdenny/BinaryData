using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Syroot.BinaryData.Extensions
{
    /// <summary>
    /// Represents static extension methods for writing data with <see cref="Stream"/> instances.
    /// </summary>
    public static partial class StreamExtensions
    {
        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        /// <summary>
        /// Writes a <see cref="Boolean"/> value to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="format">The <see cref="BooleanDataFormat"/> format in which the data is stored.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void Write(this Stream stream, Boolean value,
            BooleanDataFormat format = BooleanDataFormat.Byte, ByteConverter converter = null)
        {
            converter = converter ?? ByteConverter.System;
            switch (format)
            {
                case BooleanDataFormat.Byte:
                    stream.WriteByte((Byte)(value ? 1 : 0));
                    break;
                case BooleanDataFormat.Word:
                    converter.GetBytes((Int16)(value ? 1 : 0), _buffer, 0);
                    stream.Write(_buffer, 0, sizeof(Int16));
                    break;
                case BooleanDataFormat.Dword:
                    converter.GetBytes(value ? 1 : 0, _buffer, 0);
                    stream.Write(_buffer, 0, sizeof(Int32));
                    break;
                default:
                    throw new ArgumentException($"Invalid {nameof(BooleanDataFormat)}.", nameof(format));
            }
        }

        /// <summary>
        /// Writes an array of <see cref="Boolean"/> values to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="format">The <see cref="BooleanDataFormat"/> format in which the data is stored.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void Write(this Stream stream, IEnumerable<Boolean> values,
            BooleanDataFormat format = BooleanDataFormat.Byte, ByteConverter converter = null)
        {
            converter = converter ?? ByteConverter.System;
            lock (stream)
            {
                switch (format)
                {
                    case BooleanDataFormat.Byte:
                        foreach (var value in values)
                        {
                            stream.WriteByte((Byte)(value ? 1 : 0));
                        }
                        break;
                    case BooleanDataFormat.Word:
                        foreach (var value in values)
                        {
                            converter.GetBytes((Int16)(value ? 1 : 0), _buffer, 0);
                            stream.Write(_buffer, 0, sizeof(Int16));
                        }
                        break;
                    case BooleanDataFormat.Dword:
                        foreach (var value in values)
                        {
                            converter.GetBytes(value ? 1 : 0, _buffer, 0);
                            stream.Write(_buffer, 0, sizeof(Int32));
                        }
                        break;
                    default:
                        throw new ArgumentException($"Invalid {nameof(BooleanDataFormat)}.", nameof(format));
                }
            }
        }

        /// <summary>
        /// Writes a <see cref="Byte"/> value to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        public static void Write(this Stream stream, Byte value)
        {
            stream.WriteByte(value);
        }

        /// <summary>
        /// Writes an array of <see cref="Byte"/> values to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        public static void Write(this Stream stream, IEnumerable<Byte> values)
        {
            lock (stream)
            {
                foreach (var value in values)
                {
                    stream.WriteByte(value);
                }
            }
        }

        /// <summary>
        /// Writes a <see cref="DateTime"/> value to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="format">The <see cref="DateTimeDataFormat"/> format in which the data is stored.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void Write(this Stream stream, DateTime value,
            DateTimeDataFormat format = DateTimeDataFormat.NetTicks, ByteConverter converter = null)
        {
            converter = converter ?? ByteConverter.System;
            switch (format)
            {
                case DateTimeDataFormat.NetTicks:
                    Write(stream, value.Ticks, converter);
                    break;
                case DateTimeDataFormat.CTime:
                    Write(stream, (uint)(new DateTime(1970, 1, 1) - value).TotalSeconds, converter);
                    break;
                case DateTimeDataFormat.CTime64:
                    Write(stream, (ulong)(new DateTime(1970, 1, 1) - value).TotalSeconds, converter);
                    break;
                default:
                    throw new ArgumentException($"Invalid {nameof(DateTimeDataFormat)}.", nameof(format));
            }
        }

        /// <summary>
        /// Writes an array of <see cref="DateTime"/> values to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="format">The <see cref="DateTimeDataFormat"/> format in which the data is stored.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void Write(this Stream stream, IEnumerable<DateTime> values,
            DateTimeDataFormat format = DateTimeDataFormat.NetTicks, ByteConverter converter = null)
        {
            converter = converter ?? ByteConverter.System;
        }

        /// <summary>
        /// Writes a <see cref="Decimal"/> value to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void Write(this Stream stream, Decimal value, ByteConverter converter = null)
        {
            (converter ?? ByteConverter.System).GetBytes(value, _buffer, 0);
            stream.Write(_buffer, 0, sizeof(Decimal));
        }

        /// <summary>
        /// Writes an array of <see cref="Decimal"/> values to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void Write(this Stream stream, IEnumerable<Decimal> values, ByteConverter converter = null)
        {
            converter = converter ?? ByteConverter.System;
            lock (stream)
            {
                foreach (var value in values)
                {
                    converter.GetBytes(value, _buffer, 0);
                    stream.Write(_buffer, 0, sizeof(Decimal));
                }
            }
        }

        /// <summary>
        /// Writes a <see cref="Double"/> value to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void Write(this Stream stream, Double value, ByteConverter converter = null)
        {
            (converter ?? ByteConverter.System).GetBytes(value, _buffer, 0);
            stream.Write(_buffer, 0, sizeof(Double));
        }

        /// <summary>
        /// Writes an array of <see cref="Double"/> values to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void Write(this Stream stream, IEnumerable<Double> values, ByteConverter converter = null)
        {
            converter = converter ?? ByteConverter.System;
            lock (stream)
            {
                foreach (var value in values)
                {
                    converter.GetBytes(value, _buffer, 0);
                    stream.Write(_buffer, 0, sizeof(Double));
                }
            }
        }

        /// <summary>
        /// Writes an <see cref="Int16"/> value to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void Write(this Stream stream, Int16 value, ByteConverter converter = null)
        {
            (converter ?? ByteConverter.System).GetBytes(value, _buffer, 0);
            stream.Write(_buffer, 0, sizeof(Int16));
        }

        /// <summary>
        /// Writes an array of <see cref="Int16"/> values to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void Write(this Stream stream, IEnumerable<Int16> values, ByteConverter converter = null)
        {
            converter = converter ?? ByteConverter.System;
            lock (stream)
            {
                foreach (var value in values)
                {
                    converter.GetBytes(value, _buffer, 0);
                    stream.Write(_buffer, 0, sizeof(Int16));
                }
            }
        }

        /// <summary>
        /// Writes an <see cref="Int32"/> value to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void Write(this Stream stream, Int32 value, ByteConverter converter = null)
        {
            (converter ?? ByteConverter.System).GetBytes(value, _buffer, 0);
            stream.Write(_buffer, 0, sizeof(Int32));
        }

        /// <summary>
        /// Writes an array of <see cref="Int32"/> values to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void Write(this Stream stream, IEnumerable<Int32> values, ByteConverter converter = null)
        {
            converter = converter ?? ByteConverter.System;
            lock (stream)
            {
                foreach (var value in values)
                {
                    converter.GetBytes(value, _buffer, 0);
                    stream.Write(_buffer, 0, sizeof(Int32));
                }
            }
        }

        /// <summary>
        /// Writes an <see cref="Int64"/> value to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void Write(this Stream stream, Int64 value, ByteConverter converter = null)
        {
            (converter ?? ByteConverter.System).GetBytes(value, _buffer, 0);
            stream.Write(_buffer, 0, sizeof(Int64));
        }

        /// <summary>
        /// Writes an array of <see cref="Int64"/> values to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void Write(this Stream stream, IEnumerable<Int64> values, ByteConverter converter = null)
        {
            converter = converter ?? ByteConverter.System;
            lock (stream)
            {
                foreach (var value in values)
                {
                    converter.GetBytes(value, _buffer, 0);
                    stream.Write(_buffer, 0, sizeof(Int64));
                }
            }
        }

        /// <summary>
        /// Writes an <see cref="SByte"/> value to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        public static void Write(this Stream stream, SByte value)
        {
            _buffer[0] = (byte)value;
            stream.Write(_buffer, 0, sizeof(SByte));
        }

        /// <summary>
        /// Writes an array of <see cref="SByte"/> values to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        public static void Write(this Stream stream, IEnumerable<SByte> values)
        {
            lock (stream)
            {
                foreach (var value in values)
                {
                    _buffer[0] = (byte)value;
                    stream.Write(_buffer, 0, sizeof(SByte));
                }
            }
        }

        /// <summary>
        /// Writes a <see cref="Single"/> value to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void Write(this Stream stream, Single value, ByteConverter converter = null)
        {
            (converter ?? ByteConverter.System).GetBytes(value, _buffer, 0);
            stream.Write(_buffer, 0, sizeof(Single));
        }

        /// <summary>
        /// Writes an array of <see cref="Single"/> values to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void Write(this Stream stream, IEnumerable<Single> values, ByteConverter converter = null)
        {
            converter = converter ?? ByteConverter.System;
            lock (stream)
            {
                foreach (var value in values)
                {
                    converter.GetBytes(value, _buffer, 0);
                    stream.Write(_buffer, 0, sizeof(Single));
                }
            }
        }

        /// <summary>
        /// Writes a <see cref="String"/> value to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="format">The <see cref="StringDataFormat"/> format determining how the length of the string is
        /// stored.</param>
        /// <param name="encoding">The <see cref="Encoding"/> to parse the bytes with, or <c>null</c> to use
        /// <see cref="Encoding.UTF8"/>.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void Write(this Stream stream, String value,
            StringDataFormat format = StringDataFormat.DynamicByteCount, Encoding encoding = null,
            ByteConverter converter = null)
        {
            encoding = encoding ?? Encoding.UTF8;
            converter = converter ?? ByteConverter.System;
            byte[] buffer = encoding.GetBytes(value);
            switch (format)
            {
                case StringDataFormat.DynamicByteCount:
                    Write7BitEncodedInt(stream, buffer.Length);
                    stream.Write(buffer, 0, buffer.Length);
                    break;
                case StringDataFormat.ByteCharCount:
                    stream.WriteByte((byte)value.Length);
                    stream.Write(buffer, 0, buffer.Length);
                    break;
                case StringDataFormat.Int16CharCount:
                    converter.GetBytes((Int16)value.Length, _buffer, 0);
                    stream.Write(_buffer, 0, sizeof(Int16));
                    stream.Write(buffer, 0, buffer.Length);
                    break;
                case StringDataFormat.Int32CharCount:
                    converter.GetBytes(value.Length, _buffer, 0);
                    stream.Write(_buffer, 0, sizeof(Int32));
                    stream.Write(buffer, 0, buffer.Length);
                    break;
                case StringDataFormat.ZeroTerminated:
                    stream.Write(buffer, 0, buffer.Length);
                    switch (encoding.GetByteCount("A"))
                    {
                        case sizeof(Byte):
                            stream.WriteByte(0);
                            break;
                        case sizeof(Int16):
                            stream.WriteByte(0);
                            stream.WriteByte(0);
                            break;
                    }
                    break;
                default:
                    throw new ArgumentException($"Invalid {nameof(StringDataFormat)}.", nameof(format));
            }
        }

        /// <summary>
        /// Writes an array of <see cref="String"/> values to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="format">The <see cref="StringDataFormat"/> format determining how the length of the strings is
        /// stored.</param>
        /// <param name="encoding">The <see cref="Encoding"/> to parse the bytes with, or <c>null</c> to use
        /// <see cref="Encoding.UTF8"/>.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void Write(this Stream stream, IEnumerable<String> values,
            StringDataFormat format = StringDataFormat.DynamicByteCount, Encoding encoding = null, 
            ByteConverter converter = null)
        {
        }

        /// <summary>
        /// Writes an <see cref="UInt16"/> value to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void Write(this Stream stream, UInt16 value, ByteConverter converter = null)
        {
            (converter ?? ByteConverter.System).GetBytes(value, _buffer, 0);
            stream.Write(_buffer, 0, sizeof(UInt16));
        }

        /// <summary>
        /// Writes an array of <see cref="UInt16"/> values to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void Write(this Stream stream, IEnumerable<UInt16> values, ByteConverter converter = null)
        {
            converter = converter ?? ByteConverter.System;
            lock (stream)
            {
                foreach (var value in values)
                {
                    converter.GetBytes(value, _buffer, 0);
                    stream.Write(_buffer, 0, sizeof(UInt16));
                }
            }
        }

        /// <summary>
        /// Writes a <see cref="UInt32"/> value to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void Write(this Stream stream, UInt32 value, ByteConverter converter = null)
        {
            (converter ?? ByteConverter.System).GetBytes(value, _buffer, 0);
            stream.Write(_buffer, 0, sizeof(UInt32));
        }

        /// <summary>
        /// Writes an array of <see cref="UInt32"/> values to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void Write(this Stream stream, IEnumerable<UInt32> values, ByteConverter converter = null)
        {
            converter = converter ?? ByteConverter.System;
            lock (stream)
            {
                foreach (var value in values)
                {
                    converter.GetBytes(value, _buffer, 0);
                    stream.Write(_buffer, 0, sizeof(UInt32));
                }
            }
        }

        /// <summary>
        /// Writes a <see cref="UInt64"/> value to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void Write(this Stream stream, UInt64 value, ByteConverter converter = null)
        {
            (converter ?? ByteConverter.System).GetBytes(value, _buffer, 0);
            stream.Write(_buffer, 0, sizeof(UInt64));
        }

        /// <summary>
        /// Writes an array of <see cref="UInt64"/> values to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void Write(this Stream stream, IEnumerable<UInt64> values, ByteConverter converter = null)
        {
            converter = converter ?? ByteConverter.System;
            lock (stream)
            {
                foreach (var value in values)
                {
                    converter.GetBytes(value, _buffer, 0);
                    stream.Write(_buffer, 0, sizeof(UInt64));
                }
            }
        }

        // ---- METHODS (PRIVATE) --------------------------------------------------------------------------------------

        private static void Write7BitEncodedInt(Stream stream, int value)
        {
            // The highest bit determines whether to continue writing more bytes to form the Int32 value.
            while (value >= 0b10000000)
            {
                stream.WriteByte((byte)(value | 0b10000000));
                value >>= 7;
            }
            stream.WriteByte((byte)value);
        }
    }
}

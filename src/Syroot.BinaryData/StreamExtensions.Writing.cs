using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Syroot.BinaryData
{
    public static partial class StreamExtensions
    {
        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        /// <summary>
        /// Writes the <paramref name="values"/> to the <paramref name="stream"/> through the
        /// <paramref name="writeCallback"/> invoked for each value.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="writeCallback">The callback invoked to write each value.</param>
        public static void WriteMany<T>(this Stream stream, IEnumerable<T> values, Action<T> writeCallback)
        {
            foreach (T value in values)
                writeCallback(value);
        }

        /// <summary>
        /// Writes the <paramref name="values"/> to the <paramref name="stream"/> asynchronously through the
        /// <paramref name="writeCallback"/> invoked for each value.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="writeCallback">The callback invoked to write each value.</param>
        public static async void WriteManyAsync<T>(this Stream stream, IEnumerable<T> values,
            Func<T, Task> writeCallback)
        {
            foreach (T value in values)
                await writeCallback(value);
        }

        // ---- 7BitInt32 ----

        /// <summary>
        /// Writes a variable-length <see cref="Int32"/> value to the <paramref name="stream"/> which can require up to
        /// 5 bytes.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        public static void Write7BitInt32(this Stream stream, int value)
        {
            // The highest bit determines whether to continue writing more bytes to form the Int32 value.
            while (value >= 0b10000000)
            {
                stream.WriteByte((byte)(value | 0b10000000));
                value >>= 7;
            }
            stream.WriteByte((byte)value);
        }

        /// <summary>
        /// Writes an enumerable of <see cref="Int32"/> values to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        public static void Write7BitInt32s(this Stream stream, IEnumerable<Int32> values)
        {
            foreach (var value in values)
                Write7BitInt32(stream, value);
        }

        // ---- Boolean ----

        /// <summary>
        /// Writes a <see cref="Boolean"/> value to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="coding">The <see cref="BooleanCoding"/> in which the data is stored.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void Write(this Stream stream, Boolean value, BooleanCoding coding = BooleanCoding.Byte,
            ByteConverter converter = null)
        {
            converter = converter ?? ByteConverter.System;
            byte[] buffer;
            switch (coding)
            {
                case BooleanCoding.Byte:
                    stream.WriteByte((Byte)(value ? 1 : 0));
                    break;
                case BooleanCoding.Word:
                    buffer = Buffer;
                    converter.GetBytes((Int16)(value ? 1 : 0), buffer, 0);
                    stream.Write(Buffer, 0, sizeof(Int16));
                    break;
                case BooleanCoding.Dword:
                    buffer = Buffer;
                    converter.GetBytes(value ? 1 : 0, buffer, 0);
                    stream.Write(Buffer, 0, sizeof(Int32));
                    break;
                default:
                    throw new ArgumentException($"Invalid {nameof(BooleanCoding)}.", nameof(coding));
            }
        }

        /// <summary>
        /// Writes an enumerable of <see cref="Boolean"/> values to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="coding">The <see cref="BooleanCoding"/> in which the data is stored.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void Write(this Stream stream, IEnumerable<Boolean> values,
            BooleanCoding coding = BooleanCoding.Byte, ByteConverter converter = null)
        {
            converter = converter ?? ByteConverter.System;
            foreach (var value in values)
                Write(stream, value, coding, converter);
        }

        /// <summary>
        /// Writes a <see cref="Boolean"/> value to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="coding">The <see cref="BooleanCoding"/> in which the data is stored.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void WriteBoolean(this Stream stream, Boolean value, BooleanCoding coding = BooleanCoding.Byte,
            ByteConverter converter = null)
        {
            Write(stream, value, coding, converter);
        }

        /// <summary>
        /// Writes an enumerable of <see cref="Boolean"/> values to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="coding">The <see cref="BooleanCoding"/> in which the data is stored.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void WriteBooleans(this Stream stream, IEnumerable<Boolean> values,
            BooleanCoding coding = BooleanCoding.Byte, ByteConverter converter = null)
        {
            Write(stream, values, coding, converter);
        }

        // ---- Byte ----

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
        /// Writes an enumerable of <see cref="Byte"/> values to the <paramref name="stream"/>. This method writes bytes
        /// one-by-one.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        public static void Write(this Stream stream, IEnumerable<Byte> values)
        {
            foreach (var value in values)
                stream.WriteByte(value);
        }

        /// <summary>
        /// Writes an array of <see cref="Byte"/> values to the <paramref name="stream"/>. This method writes all bytes
        /// in one call.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        public static void Write(this Stream stream, Byte[] values)
        {
            stream.Write(values, 0, values.Length);
        }
        
        /// <summary>
        /// Writes a <see cref="Byte"/> value to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        public static void WriteByte(this Stream stream, Byte value)
        {
            Write(stream, value);
        }

        /// <summary>
        /// Writes an enumerable of <see cref="Byte"/> values to the <paramref name="stream"/>. This method writes bytes
        /// one-by-one.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        public static void WriteBytes(this Stream stream, IEnumerable<Byte> values)
        {
            Write(stream, values);
        }

        /// <summary>
        /// Writes an array of <see cref="Byte"/> values to the <paramref name="stream"/>. This method writes all bytes
        /// in one call.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        public static void WriteBytes(this Stream stream, Byte[] values)
        {
            Write(stream, values);
        }

        // ---- DateTime ----

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
                    Write(stream, (uint)(new DateTime(1970, 1, 1) - value).TotalSeconds, converter);
                    break;
                case DateTimeCoding.CTime64:
                    Write(stream, (ulong)(new DateTime(1970, 1, 1) - value).TotalSeconds, converter);
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

        // ---- Decimal ----

        /// <summary>
        /// Writes a <see cref="Decimal"/> value to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void Write(this Stream stream, Decimal value, ByteConverter converter = null)
        {
            byte[] buffer = Buffer;
            (converter ?? ByteConverter.System).GetBytes(value, buffer, 0);
            stream.Write(buffer, 0, sizeof(Decimal));
        }

        /// <summary>
        /// Writes an enumerable of <see cref="Decimal"/> values to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void Write(this Stream stream, IEnumerable<Decimal> values, ByteConverter converter = null)
        {
            converter = converter ?? ByteConverter.System;
            foreach (var value in values)
                Write(stream, value, converter);
        }

        /// <summary>
        /// Writes a <see cref="Decimal"/> value to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void WriteDecimal(this Stream stream, Decimal value, ByteConverter converter = null)
        {
            Write(stream, value, converter);
        }

        /// <summary>
        /// Writes an enumerable of <see cref="Decimal"/> values to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void WriteDecimals(this Stream stream, IEnumerable<Decimal> values,
            ByteConverter converter = null)
        {
            Write(stream, values, converter);
        }

        // ---- Double ----

        /// <summary>
        /// Writes a <see cref="Double"/> value to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void Write(this Stream stream, Double value, ByteConverter converter = null)
        {
            byte[] buffer = Buffer;
            (converter ?? ByteConverter.System).GetBytes(value, buffer, 0);
            stream.Write(buffer, 0, sizeof(Double));
        }

        /// <summary>
        /// Writes an enumerable of <see cref="Double"/> values to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void Write(this Stream stream, IEnumerable<Double> values, ByteConverter converter = null)
        {
            converter = converter ?? ByteConverter.System;
            foreach (var value in values)
                Write(stream, value, converter);
        }

        /// <summary>
        /// Writes a <see cref="Double"/> value to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void WriteDouble(this Stream stream, Double value, ByteConverter converter = null)
        {
            Write(stream, value, converter);
        }

        /// <summary>
        /// Writes an enumerable of <see cref="Double"/> values to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void WriteDoubles(this Stream stream, IEnumerable<Double> values, ByteConverter converter = null)
        {
            Write(stream, values, converter);
        }

        // ---- Enum ----

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
            // Check if the value is defined in the enumeration, if requested.
            if (strict)
                ValidateEnumValue(type, value);

            converter = converter ?? ByteConverter.System;
            Type valueType = Enum.GetUnderlyingType(type);

            // Get the bytes of the enum value and write them to the stream.
            byte[] buffer = Buffer;
            if (valueType == typeof(Byte))
                Buffer[0] = (byte)value;
            else if (valueType == typeof(SByte))
                Buffer[0] = (byte)(sbyte)value;
            else if (valueType == typeof(Int16))
                converter.GetBytes((Int16)value, buffer, 0);
            else if (valueType == typeof(Int32))
                converter.GetBytes((Int32)value, buffer, 0);
            else if (valueType == typeof(Int64))
                converter.GetBytes((Int64)value, buffer, 0);
            else if (valueType == typeof(UInt16))
                converter.GetBytes((UInt16)value, buffer, 0);
            else if (valueType == typeof(UInt32))
                converter.GetBytes((UInt32)value, buffer, 0);
            else if (valueType == typeof(UInt64))
                converter.GetBytes((UInt64)value, buffer, 0);
            else
                throw new NotImplementedException($"Unsupported enum type {valueType}.");

            stream.Write(buffer, 0, Marshal.SizeOf(valueType));
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
        {
            Type type = typeof(T);
            converter = converter ?? ByteConverter.System;
            foreach (var value in values)
                WriteEnum(stream, type, value, strict, converter);
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

        // ---- Int16 ----

        /// <summary>
        /// Writes an <see cref="Int16"/> value to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void Write(this Stream stream, Int16 value, ByteConverter converter = null)
        {
            byte[] buffer = Buffer;
            (converter ?? ByteConverter.System).GetBytes(value, buffer, 0);
            stream.Write(buffer, 0, sizeof(Int16));
        }

        /// <summary>
        /// Writes an enumerable of <see cref="Int16"/> values to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void Write(this Stream stream, IEnumerable<Int16> values, ByteConverter converter = null)
        {
            converter = converter ?? ByteConverter.System;
            foreach (var value in values)
                Write(stream, value, converter);
        }

        /// <summary>
        /// Writes an <see cref="Int16"/> value to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void WriteInt16(this Stream stream, Int16 value, ByteConverter converter = null)
        {
            Write(stream, value, converter);
        }

        /// <summary>
        /// Writes an enumerable of <see cref="Int16"/> values to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void WriteInt16s(this Stream stream, IEnumerable<Int16> values, ByteConverter converter = null)
        {
            Write(stream, values, converter);
        }

        // ---- Int32 ----

        /// <summary>
        /// Writes an <see cref="Int32"/> value to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void Write(this Stream stream, Int32 value, ByteConverter converter = null)
        {
            byte[] buffer = Buffer;
            (converter ?? ByteConverter.System).GetBytes(value, buffer, 0);
            stream.Write(buffer, 0, sizeof(Int32));
        }

        /// <summary>
        /// Writes an enumerable of <see cref="Int32"/> values to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void Write(this Stream stream, IEnumerable<Int32> values, ByteConverter converter = null)
        {
            converter = converter ?? ByteConverter.System;
            foreach (var value in values)
                Write(stream, value, converter);
        }

        /// <summary>
        /// Writes an <see cref="Int32"/> value to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void WriteInt32(this Stream stream, Int32 value, ByteConverter converter = null)
        {
            Write(stream, value, converter);
        }

        /// <summary>
        /// Writes an enumerable of <see cref="Int32"/> values to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void WriteInt32s(this Stream stream, IEnumerable<Int32> values, ByteConverter converter = null)
        {
            Write(stream, values, converter);
        }

        // ---- Int64 ----

        /// <summary>
        /// Writes an <see cref="Int64"/> value to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void Write(this Stream stream, Int64 value, ByteConverter converter = null)
        {
            byte[] buffer = Buffer;
            (converter ?? ByteConverter.System).GetBytes(value, buffer, 0);
            stream.Write(buffer, 0, sizeof(Int64));
        }

        /// <summary>
        /// Writes an enumerable of <see cref="Int64"/> values to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void Write(this Stream stream, IEnumerable<Int64> values, ByteConverter converter = null)
        {
            converter = converter ?? ByteConverter.System;
            foreach (var value in values)
                Write(stream, value, converter);
        }

        /// <summary>
        /// Writes an <see cref="Int64"/> value to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void WriteInt64(this Stream stream, Int64 value, ByteConverter converter = null)
        {
            Write(stream, value, converter);
        }

        /// <summary>
        /// Writes an enumerable of <see cref="Int64"/> values to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void WriteInt64s(this Stream stream, IEnumerable<Int64> values, ByteConverter converter = null)
        {
            Write(stream, values, converter);
        }

        // ---- SByte ----

        /// <summary>
        /// Writes an <see cref="SByte"/> value to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        public static void Write(this Stream stream, SByte value)
        {
            byte[] buffer = Buffer;
            buffer[0] = (byte)value;
            stream.Write(buffer, 0, sizeof(SByte));
        }

        /// <summary>
        /// Writes an enumerable of <see cref="SByte"/> values to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        public static void Write(this Stream stream, IEnumerable<SByte> values)
        {
            foreach (var value in values)
                Write(stream, value);
        }

        /// <summary>
        /// Writes an <see cref="SByte"/> value to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        public static void WriteSByte(this Stream stream, SByte value)
        {
            Write(stream, value);
        }

        /// <summary>
        /// Writes an enumerable of <see cref="SByte"/> values to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        public static void WriteSBytes(this Stream stream, IEnumerable<SByte> values)
        {
            Write(stream, values);
        }

        // ---- Single ----

        /// <summary>
        /// Writes a <see cref="Single"/> value to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void Write(this Stream stream, Single value, ByteConverter converter = null)
        {
            byte[] buffer = Buffer;
            (converter ?? ByteConverter.System).GetBytes(value, buffer, 0);
            stream.Write(buffer, 0, sizeof(Single));
        }

        /// <summary>
        /// Writes an enumerable of <see cref="Single"/> values to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void Write(this Stream stream, IEnumerable<Single> values, ByteConverter converter = null)
        {
            converter = converter ?? ByteConverter.System;
            foreach (var value in values)
                Write(stream, value, converter);
        }

        /// <summary>
        /// Writes a <see cref="Single"/> value to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void WriteSingle(this Stream stream, Single value, ByteConverter converter = null)
        {
            Write(stream, value, converter);
        }

        /// <summary>
        /// Writes an enumerable of <see cref="Single"/> values to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void WriteSingles(this Stream stream, IEnumerable<Single> values, ByteConverter converter = null)
        {
            Write(stream, values, converter);
        }

        // ---- String ----

        /// <summary>
        /// Writes a <see cref="String"/> value to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="coding">The <see cref="StringCoding"/> determining how the length of the string is
        /// stored.</param>
        /// <param name="encoding">The <see cref="Encoding"/> to parse the bytes with, or <c>null</c> to use
        /// <see cref="Encoding.UTF8"/>.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void Write(this Stream stream, String value, StringCoding coding = StringCoding.VariableByteCount,
            Encoding encoding = null, ByteConverter converter = null)
        {
            encoding = encoding ?? Encoding.UTF8;
            converter = converter ?? ByteConverter.System;
            byte[] textBuffer = encoding.GetBytes(value);
            switch (coding)
            {
                case StringCoding.VariableByteCount:
                    Write7BitInt32(stream, textBuffer.Length);
                    stream.Write(textBuffer, 0, textBuffer.Length);
                    break;
                case StringCoding.ByteCharCount:
                    stream.WriteByte((byte)value.Length);
                    stream.Write(textBuffer, 0, textBuffer.Length);
                    break;
                case StringCoding.Int16CharCount:
                    converter.GetBytes((Int16)value.Length, Buffer, 0);
                    stream.Write(Buffer, 0, sizeof(Int16));
                    stream.Write(textBuffer, 0, textBuffer.Length);
                    break;
                case StringCoding.Int32CharCount:
                    converter.GetBytes(value.Length, Buffer, 0);
                    stream.Write(Buffer, 0, sizeof(Int32));
                    stream.Write(textBuffer, 0, textBuffer.Length);
                    break;
                case StringCoding.ZeroTerminated:
                    stream.Write(textBuffer, 0, textBuffer.Length);
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
                case StringCoding.Raw:
                    stream.Write(textBuffer, 0, textBuffer.Length);
                    break;
                default:
                    throw new ArgumentException($"Invalid {nameof(StringCoding)}.", nameof(coding));
            }
        }

        /// <summary>
        /// Writes an enumerable of <see cref="String"/> values to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="coding">The <see cref="StringCoding"/> determining how the length of the strings is
        /// stored.</param>
        /// <param name="encoding">The <see cref="Encoding"/> to parse the bytes with, or <c>null</c> to use
        /// <see cref="Encoding.UTF8"/>.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void Write(this Stream stream, IEnumerable<String> values,
            StringCoding coding = StringCoding.VariableByteCount, Encoding encoding = null,
            ByteConverter converter = null)
        {
            encoding = encoding ?? Encoding.UTF8;
            converter = converter ?? ByteConverter.System;
            foreach (var value in values)
                Write(stream, value, coding, encoding, converter);
        }

        /// <summary>
        /// Writes a <see cref="String"/> value to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="coding">The <see cref="StringCoding"/> determining how the length of the string is
        /// stored.</param>
        /// <param name="encoding">The <see cref="Encoding"/> to parse the bytes with, or <c>null</c> to use
        /// <see cref="Encoding.UTF8"/>.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void WriteString(this Stream stream, String value,
            StringCoding coding = StringCoding.VariableByteCount, Encoding encoding = null,
            ByteConverter converter = null)
        {
            Write(stream, value, coding, encoding, converter);
        }

        /// <summary>
        /// Writes an enumerable of <see cref="String"/> values to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="coding">The <see cref="StringCoding"/> determining how the length of the strings is
        /// stored.</param>
        /// <param name="encoding">The <see cref="Encoding"/> to parse the bytes with, or <c>null</c> to use
        /// <see cref="Encoding.UTF8"/>.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void WriteStrings(this Stream stream, IEnumerable<String> values,
            StringCoding coding = StringCoding.VariableByteCount, Encoding encoding = null,
            ByteConverter converter = null)
        {
            Write(stream, values, coding, encoding, converter);
        }

        // ---- UInt16 ----

        /// <summary>
        /// Writes an <see cref="UInt16"/> value to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void Write(this Stream stream, UInt16 value, ByteConverter converter = null)
        {
            byte[] buffer = Buffer;
            (converter ?? ByteConverter.System).GetBytes(value, buffer, 0);
            stream.Write(buffer, 0, sizeof(UInt16));
        }

        /// <summary>
        /// Writes an enumerable of <see cref="UInt16"/> values to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void Write(this Stream stream, IEnumerable<UInt16> values, ByteConverter converter = null)
        {
            converter = converter ?? ByteConverter.System;
            foreach (var value in values)
                Write(stream, value, converter);
        }

        /// <summary>
        /// Writes an <see cref="UInt16"/> value to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void WriteUInt16(this Stream stream, UInt16 value, ByteConverter converter = null)
        {
            Write(stream, value, converter);
        }

        /// <summary>
        /// Writes an enumerable of <see cref="UInt16"/> values to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void WriteUInt16s(this Stream stream, IEnumerable<UInt16> values, ByteConverter converter = null)
        {
            Write(stream, values, converter);
        }

        // ---- UInt32 ----

        /// <summary>
        /// Writes a <see cref="UInt32"/> value to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void Write(this Stream stream, UInt32 value, ByteConverter converter = null)
        {
            byte[] buffer = Buffer;
            (converter ?? ByteConverter.System).GetBytes(value, buffer, 0);
            stream.Write(buffer, 0, sizeof(UInt32));
        }

        /// <summary>
        /// Writes an enumerable of <see cref="UInt32"/> values to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void Write(this Stream stream, IEnumerable<UInt32> values, ByteConverter converter = null)
        {
            converter = converter ?? ByteConverter.System;
            foreach (var value in values)
                Write(stream, value, converter);
        }

        /// <summary>
        /// Writes a <see cref="UInt32"/> value to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void WriteUInt32(this Stream stream, UInt32 value, ByteConverter converter = null)
        {
            Write(stream, value, converter);
        }

        /// <summary>
        /// Writes an enumerable of <see cref="UInt32"/> values to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void WriteUInt32s(this Stream stream, IEnumerable<UInt32> values, ByteConverter converter = null)
        {
            Write(stream, values, converter);
        }

        // ---- UInt64 ----

        /// <summary>
        /// Writes a <see cref="UInt64"/> value to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void Write(this Stream stream, UInt64 value, ByteConverter converter = null)
        {
            byte[] buffer = Buffer;
            (converter ?? ByteConverter.System).GetBytes(value, buffer, 0);
            stream.Write(buffer, 0, sizeof(UInt64));
        }

        /// <summary>
        /// Writes an enumerable of <see cref="UInt64"/> values to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void Write(this Stream stream, IEnumerable<UInt64> values, ByteConverter converter = null)
        {
            converter = converter ?? ByteConverter.System;
            foreach (var value in values)
                Write(stream, value, converter);
        }

        /// <summary>
        /// Writes a <see cref="UInt64"/> value to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void WriteUInt64(this Stream stream, UInt64 value, ByteConverter converter = null)
        {
            Write(stream, value, converter);
        }

        /// <summary>
        /// Writes an enumerable of <see cref="UInt64"/> values to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void WriteUInt64s(this Stream stream, IEnumerable<UInt64> values, ByteConverter converter = null)
        {
            Write(stream, values, converter);
        }

        // ---- METHODS (PRIVATE) --------------------------------------------------------------------------------------

    }
}

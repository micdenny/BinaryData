using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Syroot.BinaryData
{  
    public static partial class StreamExtensions
    {
        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        // ---- Object ----

        /// <summary>
        /// Returns an object of type <typeparamref name="T"/> read from the <paramref name="stream"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object to read.</typeparam>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <returns>The value read from the current stream.</returns>
        public static T ReadObject<T>(this Stream stream, ByteConverter converter = null)
        {
            return (T)ReadObject(stream, null, BinaryMemberAttribute.Default, typeof(T), converter);
        }

        /// <summary>
        /// Returns an array of objects of type <typeparamref name="T"/> read from the <paramref name="stream"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object to load.</typeparam>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static T[] ReadObjects<T>(this Stream stream, int count, ByteConverter converter = null)
        {
            converter = converter ?? ByteConverter.System;
            return stream.ReadMany(count,
                () => ReadObject<T>(stream, converter));
        }

        // ---- METHODS (PRIVATE) --------------------------------------------------------------------------------------

        private static void FillBuffer(Stream stream, int length)
        {
            if (stream.Read(Buffer, 0, length) < length)
                throw new EndOfStreamException($"Could not read {length} bytes.");
        }

        private static object ReadEnum(Stream stream, Type enumType, bool strict, ByteConverter converter)
        {
            converter = converter ?? ByteConverter.System;

            Type valueType = Enum.GetUnderlyingType(enumType);
            int valueSize = Marshal.SizeOf(valueType);
            object value;

            // Read enough bytes to form an enum value.
            FillBuffer(stream, valueSize);
            if (valueType == typeof(Byte))
                value = Buffer[0];
            else if (valueType == typeof(SByte))
                value = (sbyte)Buffer[0];
            else if (valueType == typeof(Int16))
                value = converter.ToInt16(Buffer);
            else if (valueType == typeof(Int32))
                value = converter.ToInt32(Buffer);
            else if (valueType == typeof(Int64))
                value = converter.ToInt64(Buffer);
            else if (valueType == typeof(UInt16))
                value = converter.ToUInt16(Buffer);
            else if (valueType == typeof(UInt32))
                value = converter.ToUInt32(Buffer);
            else if (valueType == typeof(UInt64))
                value = converter.ToUInt64(Buffer);
            else
                throw new NotImplementedException($"Unsupported enum type {valueType}.");
            
            // Check if the value is defined in the enumeration, if requested.
            if (strict)
                EnumTools.IsValid(enumType, value);
            return value;
        }

        private static object ReadObject(Stream stream, object instance, BinaryMemberAttribute attribute,
            Type type, ByteConverter converter)
        {
            if (attribute.Converter == null)
            {
                if (type == typeof(String))
                {
                    if (attribute.StringCoding == StringCoding.Raw)
                        return stream.ReadString(attribute.Length);
                    else
                        return stream.ReadString(attribute.StringCoding, converter: converter);
                }
                else if (type.IsEnumerable())
                    throw new InvalidOperationException("Multidimensional arrays cannot be read directly.");
                else if (type == typeof(Boolean))
                    return stream.ReadBoolean(attribute.BooleanCoding);
                else if (type == typeof(Byte))
                    return stream.Read1Byte();
                else if (type == typeof(DateTime))
                    return stream.ReadDateTime(attribute.DateTimeCoding, converter);
                else if (type == typeof(Decimal))
                    return stream.ReadDecimal();
                else if (type == typeof(Double))
                    return stream.ReadDouble(converter);
                else if (type == typeof(Int16))
                    return stream.ReadInt16(converter);
                else if (type == typeof(Int32))
                    return stream.ReadInt32(converter);
                else if (type == typeof(Int64))
                    return stream.ReadInt64(converter);
                else if (type == typeof(SByte))
                    return stream.ReadSByte();
                else if (type == typeof(Single))
                    return stream.ReadSingle(converter);
                else if (type == typeof(UInt16))
                    return stream.ReadUInt16(converter);
                else if (type == typeof(UInt32))
                    return stream.ReadUInt32(converter);
                else if (type == typeof(UInt64))
                    return stream.ReadUInt64(converter);
                else if (type.IsEnum)
                    return ReadEnum(stream, type, attribute.Strict, converter);
                else
                {
                    if (stream.CanSeek)
                        return ReadCustomObject(stream, type, null, stream.Position, converter);
                    else
                        return ReadCustomObject(stream, type, null, -1, converter);
                }
            }
            else
            {
                // Let a binary converter do all the work.
                IBinaryConverter binaryConverter = BinaryConverterCache.GetConverter(attribute.Converter);
                return binaryConverter.Read(stream, instance, attribute, converter);
            }
        }

        private static object ReadCustomObject(Stream stream, Type type, object instance, long startOffset,
            ByteConverter converter)
        {
            TypeData typeData = TypeData.GetTypeData(type);
            instance = instance ?? typeData.GetInstance();

            // Read inherited members first if required.
            if (typeData.Attribute.Inherit && typeData.Type.BaseType != null)
            {
                ReadCustomObject(stream, typeData.Type.BaseType, instance, startOffset, converter);
            }

            // Read members.
            foreach (KeyValuePair<int, MemberData> orderedMember in typeData.OrderedMembers)
            {
                ReadMember(stream, instance, startOffset, converter, orderedMember.Value);
            }
            foreach (KeyValuePair<string, MemberData> unorderedMember in typeData.UnorderedMembers)
            {
                ReadMember(stream, instance, startOffset, converter, unorderedMember.Value);
            }

            return instance;
        }

        private static void ReadMember(Stream stream, object instance, long startOffset, ByteConverter converter,
            MemberData member)
        {
            // If possible, reposition the stream according to offset.
            if (stream.CanSeek)
            {
                if (member.Attribute.OffsetOrigin == OffsetOrigin.Begin)
                    stream.Position = startOffset + member.Attribute.Offset;
                else if (member.Attribute.Offset != 0)
                    stream.Position += member.Attribute.Offset;
            }
            else
            {
                if (member.Attribute.OffsetOrigin == OffsetOrigin.Begin || member.Attribute.Offset < 0)
                    throw new NotSupportedException("Cannot reposition the stream as it is not seekable.");
                else if (member.Attribute.Offset > 0) // Simulate moving forward by reading bytes.
                    stream.ReadBytes(member.Attribute.Offset);
            }

            // Read the value and respect settings stored in the member attribute.
            object value;
            Type elementType = member.Type.GetEnumerableElementType();
            if (elementType == null)
            {
                value = ReadObject(stream, instance, member.Attribute, member.Type, converter);
            }
            else
            {
                Array values = Array.CreateInstance(elementType, member.Attribute.Length);
                for (int i = 0; i < values.Length; i++)
                {
                    values.SetValue(ReadObject(stream, instance, member.Attribute, elementType, converter), i);
                }
                value = values;
            }

            // Set the read value.
            switch (member.MemberInfo)
            {
                case FieldInfo field:
                    field.SetValue(instance, value);
                    break;
                case PropertyInfo property:
                    property.SetValue(instance, value);
                    break;
            }
        }
    }
}

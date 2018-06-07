using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Syroot.BinaryData
{
    public static partial class StreamExtensions
    {
        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        // ---- Object ----

        /// <summary>
        /// Writes an object or enumerable of objects to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The object or enumerable of objects to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void WriteObject(this Stream stream, object value, ByteConverter converter = null)
            => WriteObject(stream, null, BinaryMemberAttribute.Default, value.GetType(), value, converter);

        // ---- METHODS (PRIVATE) --------------------------------------------------------------------------------------

        private static void WriteEnum(Stream stream, Type enumType, object value, bool strict, ByteConverter converter)
        {
            converter = converter ?? ByteConverter.System;
            Type valueType = Enum.GetUnderlyingType(enumType);

            // Write the enum value.
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

            // Check if the value is defined in the enumeration, if requested.
            if (strict)
                EnumTools.IsValid(enumType, value);
            stream.Write(buffer, 0, Marshal.SizeOf(valueType));
        }

        private static void WriteObject(Stream stream, object instance, BinaryMemberAttribute attribute, Type type,
            object value, ByteConverter converter)
        {
            converter = converter ?? ByteConverter.System;

            if (attribute.Converter == null)
            {
                if (value == null)
                    return;
                else if (type == typeof(String))
                    stream.Write((String)value, attribute.StringCoding, encoding: attribute.StringEncoding != null ? Encoding.GetEncoding(attribute.StringEncoding) : null, converter: converter);
                else if (type.TryGetEnumerableElementType(out Type elementType))
                {
                    foreach (object element in (IEnumerable)value)
                        WriteObject(stream, null, BinaryMemberAttribute.Default, elementType, element, converter);
                }
                else if (type == typeof(Boolean))
                    stream.Write((Boolean)value, attribute.BooleanCoding, converter);
                else if (type == typeof(Byte))
                    stream.Write((Byte)value);
                else if (type == typeof(DateTime))
                    stream.Write((DateTime)value, attribute.DateTimeCoding, converter);
                else if (type == typeof(Decimal))
                    stream.Write((Decimal)value);
                else if (type == typeof(Double))
                    stream.Write((Double)value, converter);
                else if (type == typeof(Int16))
                    stream.Write((Int16)value, converter);
                else if (type == typeof(Int32))
                    stream.Write((Int32)value, converter);
                else if (type == typeof(Int64))
                    stream.Write((Int64)value, converter);
                else if (type == typeof(SByte))
                    stream.Write((SByte)value);
                else if (type == typeof(Single))
                    stream.Write((Single)value, converter);
                else if (type == typeof(UInt16))
                    stream.Write((UInt16)value, converter);
                else if (type == typeof(UInt32))
                    stream.Write((UInt32)value, converter);
                else if (type == typeof(UInt64))
                    stream.Write((UInt64)value, converter);
                else if (type.IsEnum)
                    WriteEnum(stream, type, value, attribute.Strict, converter);
                else
                {
                    if (stream.CanSeek)
                        WriteCustomObject(stream, type, value, stream.Position, converter);
                    else
                        WriteCustomObject(stream, type, value, -1, converter);
                }
            }
            else
            {
                // Let a binary converter do all the work.
                IBinaryConverter binaryConverter = BinaryConverterCache.GetConverter(attribute.Converter);
                binaryConverter.Write(stream, instance, attribute, value, converter);
            }
        }

        private static void WriteCustomObject(Stream stream, Type type, object instance, long startOffset,
            ByteConverter converter)
        {
            TypeData typeData = TypeData.GetTypeData(type);

            // Write inherited members first if required.
            if (typeData.Attribute.Inherit && typeData.Type.BaseType != null)
                WriteCustomObject(stream, typeData.Type.BaseType, instance, startOffset, converter);

            // Write members.
            foreach (KeyValuePair<int, MemberData> orderedMember in typeData.OrderedMembers)
                WriteMember(stream, instance, startOffset, converter, orderedMember.Value);
            foreach (KeyValuePair<string, MemberData> unorderedMember in typeData.UnorderedMembers)
                WriteMember(stream, instance, startOffset, converter, unorderedMember.Value);
        }

        private static void WriteMember(Stream stream, object instance, long startOffset, ByteConverter converter,
            MemberData member)
        {
            // If possible, reposition the stream according to offset.
            if (stream.CanSeek)
            {
                if (member.Attribute.OffsetOrigin == OffsetOrigin.Begin)
                    stream.Position = startOffset + member.Attribute.Offset;
                else
                    stream.Position += member.Attribute.Offset;
            }
            else
            {
                if (member.Attribute.OffsetOrigin == OffsetOrigin.Begin || member.Attribute.Offset < 0)
                    throw new NotSupportedException("Cannot reposition the stream as it is not seekable.");
                else if (member.Attribute.Offset > 0) // Simulate moving forward by writing bytes.
                    stream.Write(new byte[member.Attribute.Offset]);
            }

            // Get the value to write.
            object value;
            switch (member.MemberInfo)
            {
                case FieldInfo field:
                    value = field.GetValue(instance);
                    break;
                case PropertyInfo property:
                    value = property.GetValue(instance);
                    break;
                default:
                    throw new InvalidOperationException($"Tried to write an invalid member {member.MemberInfo}.");
            }

            // Write the value and respect settings stored in the member attribute.
            Type elementType = member.Type.GetEnumerableElementType();
            if (elementType == null)
            {
                WriteObject(stream, instance, member.Attribute, member.Type, value, converter);
            }
            else
            {
                foreach (object element in (IEnumerable)value)
                    WriteObject(stream, instance, member.Attribute, elementType, element, converter);
            }
        }
    }
}

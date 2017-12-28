using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Syroot.BinaryData.Core;
using Syroot.BinaryData.Extensions;

namespace Syroot.BinaryData.Serialization
{
    /// <summary>
    /// Represents logic to serialize and deserialize objects of any type.
    /// </summary>
    internal static class BinarySerialization
    {
        // ---- FIELDS -------------------------------------------------------------------------------------------------

        private static readonly Dictionary<RuntimeTypeHandle, Func<Stream, ByteConverter, MemberData, object>>
            _typeReaders = new Dictionary<RuntimeTypeHandle, Func<Stream, ByteConverter, MemberData, object>>
            {
                [typeof(Boolean).TypeHandle] = ReadBoolean,
                [typeof(Byte).TypeHandle] = ReadByte,
                [typeof(DateTime).TypeHandle] = ReadDateTime,
                [typeof(Decimal).TypeHandle] = ReadDecimal,
                [typeof(Double).TypeHandle] = ReadDouble,
                [typeof(Int16).TypeHandle] = ReadInt16,
                [typeof(Int32).TypeHandle] = ReadInt32,
                [typeof(Int64).TypeHandle] = ReadInt64,
                [typeof(SByte).TypeHandle] = ReadSByte,
                [typeof(Single).TypeHandle] = ReadSingle,
                [typeof(String).TypeHandle] = ReadString,
                [typeof(UInt16).TypeHandle] = ReadUInt16,
                [typeof(UInt32).TypeHandle] = ReadUInt32,
                [typeof(UInt64).TypeHandle] = ReadUInt64
            };

        // ---- METHODS (INTERNAL) -------------------------------------------------------------------------------------

        internal static object ReadClass(Stream stream, TypeData typeData, object instance, ByteConverter byteConverter)
        {
            instance = instance ?? typeData.Instantiate();
            long instanceOffset = stream.CanSeek ? stream.Position : -1;

            // Read inherited members.
            if (typeData.Inherit && typeData.Type.BaseType != null)
            {
                ReadClass(stream, TypeData.Get(typeData.Type.BaseType), instance, byteConverter);
            }

            // Read ordered members.
            foreach (KeyValuePair<int, MemberData> orderedMember in typeData.OrderedMembers)
            {
                ReadMember(stream, orderedMember.Value, instance, byteConverter, instanceOffset);
            }

            // Read unordered / alphabetically sorted members.
            foreach (KeyValuePair<string, MemberData> unorderedMember in typeData.UnorderedMembers)
            {
                ReadMember(stream, unorderedMember.Value, instance, byteConverter, instanceOffset);
            }
            
            return instance;
        }

        private static void ReadMember(Stream stream, MemberData memberData, object instance,
            ByteConverter byteConverter, long instanceOffset)
        {
            object value;
            int arrayCount = memberData.GetArrayCount(stream, byteConverter, instance);
            if (arrayCount == 0)
            {
                // Read a single value.
                value = ReadClass(stream, TypeData.Get(memberData.Type), null, byteConverter);
            }
            else
            {
                // Read an array of the retrieved number of elements.
                Type elementType = memberData.Type.GetEnumerableElementType();
                Array values = Array.CreateInstance(elementType, arrayCount);
                for (int i = 0; i < values.Length; i++)
                {
                    values.SetValue(ReadMemberValue(stream, byteConverter, elementType, memberData, stream.Position), i);
                }
                value = values;
            }

            // Set the read value.
            switch (memberData.MemberInfo)
            {
                case FieldInfo fieldInfo:
                    fieldInfo.SetValue(instance, value);
                    break;
                case PropertyInfo propertyInfo:
                    propertyInfo.SetValue(instance, value);
                    break;
            }
        }

        // ---- METHODS (PRIVATE) --------------------------------------------------------------------------------------

        private static object ReadMemberValue(Stream stream, ByteConverter byteConverter, Type type, MemberData memberData,
            long parentOffset)
        {
            byteConverter = memberData.Endian == 0 ? byteConverter : ByteConverter.GetConverter(memberData.Endian);

            // Offset the stream before reading the member.
            ApplyOffset(stream, parentOffset, memberData.OffsetOrigin, memberData.OffsetDelta);

            // Read the data.
            object value;
            if (memberData.ConverterType == null)
            {
                // Let a converter handle all the data after adjusting to the offset.
                TypeData typeData = TypeData.Get(type);
                ApplyOffset(stream, parentOffset, typeData.StartOrigin, typeData.StartDelta);
                value = memberData.ConverterAttrib.Read(stream, memberData);
                ApplyOffset(stream, parentOffset, typeData.EndOrigin, typeData.EndDelta);
            }
            else if (_typeReaders.TryGetValue(type.TypeHandle, out var reader))
            {
                // Read a primitive type.
                value = reader(stream, byteConverter, memberData);
            }
            else if (type.IsEnum)
            {
                // Read an enumerable type.
                value = stream.ReadEnum(type, memberData.EnumStrict, byteConverter);
            }
            else
            {
                // Read a custom type.
                TypeData typeData = TypeData.Get(type);
                ApplyOffset(stream, parentOffset, typeData.StartOrigin, typeData.StartDelta);
                value = ReadClass(stream, typeData, typeData.Instantiate(), byteConverter);
                ApplyOffset(stream, parentOffset, typeData.EndOrigin, typeData.EndDelta);
            }
            return value;
        }

        private static object ReadString(Stream stream, ByteConverter byteConverter, MemberData memberData)
        {
            Encoding encoding = memberData.StringCodePage == 0 ? null : Encoding.GetEncoding(memberData.StringCodePage);

            if (memberData.StringCoding == StringCoding.Raw)
            {
                return stream.ReadString(memberData.StringLength, encoding);
            }
            else
            {
                return stream.ReadString(memberData.StringCoding, encoding, byteConverter);
            }
        }

        private static object ReadBoolean(Stream stream, ByteConverter byteConverter, MemberData memberData)
            => stream.ReadBoolean(memberData.BooleanCoding);

        private static object ReadByte(Stream stream, ByteConverter byteConverter, MemberData memberData)
            => stream.Read1Byte();

        private static object ReadDateTime(Stream stream, ByteConverter byteConverter, MemberData memberData)
            => stream.ReadDateTime(memberData.DateTimeCoding, byteConverter);

        private static object ReadDecimal(Stream stream, ByteConverter byteConverter, MemberData memberData)
            => stream.ReadDecimal(byteConverter);

        private static object ReadDouble(Stream stream, ByteConverter byteConverter, MemberData memberData)
            => stream.ReadDouble(byteConverter);

        private static object ReadInt16(Stream stream, ByteConverter byteConverter, MemberData memberData)
            => stream.ReadInt16(byteConverter);

        private static object ReadInt32(Stream stream, ByteConverter byteConverter, MemberData memberData)
            => stream.ReadInt32(byteConverter);

        private static object ReadInt64(Stream stream, ByteConverter byteConverter, MemberData memberData)
            => stream.ReadInt64(byteConverter);

        private static object ReadSByte(Stream stream, ByteConverter byteConverter, MemberData memberData)
            => stream.ReadSByte();

        private static object ReadSingle(Stream stream, ByteConverter byteConverter, MemberData memberData)
            => stream.ReadSingle(byteConverter);

        private static object ReadUInt16(Stream stream, ByteConverter byteConverter, MemberData memberData)
            => stream.ReadUInt16(byteConverter);

        private static object ReadUInt32(Stream stream, ByteConverter byteConverter, MemberData memberData)
            => stream.ReadUInt16(byteConverter);

        private static object ReadUInt64(Stream stream, ByteConverter byteConverter, MemberData memberData)
            => stream.ReadUInt64(byteConverter);

        private static void ApplyOffset(Stream stream, long parentOffset, Origin origin, long delta)
        {
            switch (origin)
            {
                case Origin.Add:
                    stream.Move(delta);
                    break;
                case Origin.Align:
                    stream.Align(delta);
                    break;
                case Origin.Set:
                    stream.Position = parentOffset + delta;
                    break;
            }
        }
    }
}
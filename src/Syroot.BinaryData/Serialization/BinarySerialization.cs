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

        internal static object Read(Stream stream, ByteConverter byteConverter, Type type, MemberData memberData,
            long parentOffset)
        {
            byteConverter = memberData.Endian == 0 ? byteConverter : ByteConverter.GetConverter(memberData.Endian);

            // Offset the stream before reading the member.
            ApplyOffset(stream, parentOffset, memberData.OffsetOrigin, memberData.OffsetDelta);

            // Read the data.
            object value;
            if (memberData.ConverterType == null)
            {
                // Let a converter handle all the data after adjusting to the object offset.
                TypeData typeData = TypeData.Get(type);
                if (typeData.OffsetStartConfig != null)
                    ApplyOffset(stream, parentOffset, typeData.OffsetStartConfig.Origin, typeData.OffsetStartConfig.Delta);
                value = memberData.ConverterAttrib.Read(stream, memberData);
                if (typeData.OffsetEndConfig != null)
                    ApplyOffset(stream, parentOffset, typeData.OffsetEndConfig.Origin, typeData.OffsetEndConfig.Delta);
            }
            else if (_typeReaders.TryGetValue(type.TypeHandle, out var reader))
            {
                // Read a primitive type.
                value = reader(stream, byteConverter, memberData);
            }
            else if (type.IsEnum)
            {
                // Read an enumerable type.
                value = memberData.EnumAttrib.Read(type, stream, byteConverter);
            }
            else
            {
                // Read a custom type.
                TypeData typeData = TypeData.Get(type);
                typeData.OffsetStartConfig?.Apply(stream, parentOffset);
                value = ReadClassData(stream, byteConverter, typeData, typeData.Instantiate());
                typeData.OffsetEndConfig?.Apply(stream, parentOffset);
            }
            return value;
        }

        // ---- METHODS (PRIVATE) --------------------------------------------------------------------------------------

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

        private static object ReadClassData(Stream stream, ByteConverter byteConverter, TypeData typeData,
            object instance)
        {
            // Read inherited members first if required.
            if (typeData.ClassAttrib.Inherit && typeData.Type.BaseType != null)
            {
                ReadClassData(stream, byteConverter, TypeData.Get(typeData.Type.BaseType), instance);
            }

            // Read members.
            foreach (MemberData memberData in typeData.Members)
            {
                // Read the value and respect settings stored in the member attribute.
                object value;
                if (memberData.ArrayCount == 0 && memberData.ArrayCountProvider == null)
                {
                    value = ReadClassData(stream, byteConverter, TypeData.Get(memberData.Type), null);
                }
                else
                {
                    Type elementType = memberData.Type.GetEnumerableElementType();
                    Array values = Array.CreateInstance(elementType, memberData.GetArrayCount(instance));
                    for (int i = 0; i < values.Length; i++)
                    {
                        values.SetValue(Read(stream, byteConverter, elementType, memberData, stream.Position), i);
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

            return instance;
        }

        private static void ApplyOffset(Stream stream, long parentOffset, Origin origin, int delta)
        {
            switch (origin)
            {
                // TODO: Rework Position and Align so it can work in seekable streams.
                case Origin.Add:
                    stream.Position += delta;
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
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Syroot.BinaryData
{
    /// <summary>
    /// Represents logic to serialize and deserialize objects of any type.
    /// </summary>
    internal class Serializer
    {
        // ---- CONSTANTS ----------------------------------------------------------------------------------------------

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

        // ---- FIELDS -------------------------------------------------------------------------------------------------

        private readonly Dictionary<Type, TypeData> _typeCache;
        private readonly Dictionary<Type, IDataConverter> _converterCache;
        
        // ---- CONSTRUCTORS & DESTRUCTOR ------------------------------------------------------------------------------

        internal Serializer()
        {
            _typeCache = new Dictionary<Type, TypeData>();
            _converterCache = new Dictionary<Type, IDataConverter>();
        }

        // ---- METHODS (INTERNAL) -------------------------------------------------------------------------------------

        /// <summary>
        /// Reads the object of the given <paramref name="type"/> from the <paramref name="stream"/> and overrides the
        /// <paramref name="byteConverter"/> used.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to read the data from.</param>
        /// <param name="type">The <see cref="Type"/> of the object to read.</param>
        /// <param name="byteConverter">The <see cref="ByteConverter"/> to use.</param>
        /// <returns>The read instance.</returns>
        internal object ReadObject(Stream stream, Type type, ByteConverter byteConverter)
            => ReadObject(stream, GetTypeData(type), byteConverter, null);

        /// <summary>
        /// Writes the <paramref name="value"/> to the <paramref name="stream"/> and overrides the
        /// <paramref name="byteConverter"/> used.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to write the data to.</param>
        /// <param name="value">The object to write.</param>
        /// <param name="byteConverter">The <see cref="ByteConverter"/> to use.</param>
        internal void WriteObject(Stream stream, object value, ByteConverter byteConverter)
            => throw new NotImplementedException();

        // ---- METHODS (PRIVATE) --------------------------------------------------------------------------------------

        // ---- Caches ----

        private TypeData GetTypeData(Type type)
        {
            lock (_typeCache)
            {
                if (!_typeCache.TryGetValue(type, out TypeData typeData))
                {
                    typeData = new TypeData(type);
                    _typeCache.Add(type, typeData);
                }
                return typeData;
            }
        }

        private IDataConverter GetConverter(Type type)
        {
            lock (_converterCache)
            {
                if (!_converterCache.TryGetValue(type, out IDataConverter converter))
                {
                    converter = (IDataConverter)Activator.CreateInstance(type);
                    _converterCache.Add(type, converter);
                }
                return converter;
            }
        }

        // ---- Reading ----

        private object ReadObject(Stream stream, TypeData typeData, ByteConverter byteConverter, object instance)
        {
            // Instantiate the object if no child class instance was passed.
            instance = instance ?? typeData.Instantiate();

            // Read inherited members first.
            lock (stream)
            {
                if (typeData.Inherit && typeData.Type.BaseType != null)
                {
                    ReadObject(stream, GetTypeData(typeData.Type.BaseType), byteConverter, instance);
                }

                // Read members.
                long offset = stream.CanSeek ? stream.Position : -1;
                foreach (KeyValuePair<int, MemberData> orderedMember in typeData.OrderedMembers)
                {
                    ReadAndSetMember(stream, orderedMember.Value, instance, byteConverter, offset);
                }
                foreach (KeyValuePair<string, MemberData> unorderedMember in typeData.UnorderedMembers)
                {
                    ReadAndSetMember(stream, unorderedMember.Value, instance, byteConverter, offset);
                }
            }

            return instance;
        }

        private void ReadAndSetMember(Stream stream, MemberData memberData, object instance,
            ByteConverter byteConverter, long instanceOffset)
        {
            object value;
            int arrayCount = memberData.GetArrayCount(stream, byteConverter, instance);
            if (arrayCount == 0)
            {
                // Read a single value.
                value = ReadObject(stream, GetTypeData(memberData.Type), byteConverter, null);
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
        
        private object ReadMemberValue(Stream stream, ByteConverter byteConverter, Type type, MemberData memberData,
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
                TypeData typeData = GetTypeData(type);
                ApplyOffset(stream, parentOffset, typeData.StartOrigin, typeData.StartDelta);
                value = null;// TODO: memberData.ConverterAttrib.Read(stream, memberData);
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
                TypeData typeData = GetTypeData(type);
                ApplyOffset(stream, parentOffset, typeData.StartOrigin, typeData.StartDelta);
                value = ReadObject(stream, type, byteConverter);
                ApplyOffset(stream, parentOffset, typeData.EndOrigin, typeData.EndDelta);
            }
            return value;
        }

        private static object ReadString(Stream stream, ByteConverter byteConverter, MemberData memberData)
        {
            Encoding encoding = memberData.StringCodePage == 0 ? null : Encoding.GetEncoding(memberData.StringCodePage);

            if (memberData.StringCoding == StringCoding.Raw)
                return stream.ReadString(memberData.StringLength, encoding);
            else
                return stream.ReadString(memberData.StringCoding, encoding, byteConverter);
        }

        private static object ReadBoolean(Stream stream, ByteConverter byteConverter, MemberData memberData)
            => stream.ReadBoolean(memberData.BooleanCoding);

        private static object ReadByte(Stream stream, ByteConverter byteConverter, MemberData memberData)
            => stream.Read1Byte();

        private static object ReadDateTime(Stream stream, ByteConverter byteConverter, MemberData memberData)
            => stream.ReadDateTime(memberData.DateTimeCoding, byteConverter);

        private static object ReadDecimal(Stream stream, ByteConverter byteConverter, MemberData memberData)
            => stream.ReadDecimal();

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
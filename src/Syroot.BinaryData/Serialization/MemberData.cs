using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using Syroot.BinaryData.Core;
using Syroot.BinaryData.Extensions;

namespace Syroot.BinaryData.Serialization
{
    /// <summary>
    /// Represents information on a member of a type read and written through binary serialiazion.
    /// </summary>
    [DebuggerDisplay(nameof(MemberData) + " " + nameof(MemberInfo) + "={" + nameof(MemberInfo) + "}")]
    public class MemberData
    {
        // ---- CONSTANTS ----------------------------------------------------------------------------------------------

        internal static readonly MemberData Default = new MemberData();

        // ---- CONSTRUCTORS & DESTRUCTOR ------------------------------------------------------------------------------

        internal MemberData(FieldInfo fieldInfo) : this(fieldInfo.FieldType, fieldInfo) { }

        internal MemberData(PropertyInfo propertyInfo) : this(propertyInfo.PropertyType, propertyInfo) { }

        private MemberData(Type memberType, MemberInfo memberInfo)
        {
            Type = memberType;
            MemberInfo = memberInfo;

            // Get any possible attribute.
            DataArrayAttribute arrayAttrib = MemberInfo.GetCustomAttribute<DataArrayAttribute>();
            if (arrayAttrib != null)
            {
                IsExported = true;
                ArrayCount = arrayAttrib.Count;
                ArrayCountProvider = arrayAttrib.CountProvider;
                ArrayCountCoding = arrayAttrib.Coding;
                ArrayCountCodingEndian = arrayAttrib.CodingEndian;
            }
            DataBooleanAttribute booleanAttrib = MemberInfo.GetCustomAttribute<DataBooleanAttribute>();
            if (booleanAttrib != null)
            {
                IsExported = true;
                BooleanCoding = booleanAttrib.Coding;
            }
            DataConverterAttribute converterAttrib = MemberInfo.GetCustomAttribute<DataConverterAttribute>();
            if (converterAttrib != null)
            {
                IsExported = true;
                ConverterType = converterAttrib.ConverterType;
            }
            DataDateTimeAttribute dateTimeAttrib = MemberInfo.GetCustomAttribute<DataDateTimeAttribute>();
            if (dateTimeAttrib != null)
            {
                IsExported = true;
                DateTimeCoding = dateTimeAttrib.Coding;
            }
            DataEndianAttribute endianAttrib = MemberInfo.GetCustomAttribute<DataEndianAttribute>();
            if (endianAttrib != null)
            {
                IsExported = true;
                Endian = endianAttrib.Endian;
            }
            DataEnumAttribute enumAttrib = MemberInfo.GetCustomAttribute<DataEnumAttribute>();
            if (enumAttrib != null)
            {
                IsExported = true;
                EnumStrict = enumAttrib.Strict;
            }
            DataMemberAttribute memberAttrib = MemberInfo.GetCustomAttribute<DataMemberAttribute>();
            if (memberAttrib != null)
            {
                IsExported = true;
            }
            DataOffsetAttribute offsetAttrib = MemberInfo.GetCustomAttribute<DataOffsetAttribute>();
            if (offsetAttrib != null)
            {
                IsExported = true;
                OffsetDelta = offsetAttrib.Delta;
                OffsetOrigin = offsetAttrib.Origin;
            }
            DataOrderAttribute orderAttrib = MemberInfo.GetCustomAttribute<DataOrderAttribute>();
            if (orderAttrib != null)
            {
                IsExported = true;
                Index = orderAttrib.Index;
            }
            DataStringAttribute stringAttrib = MemberInfo.GetCustomAttribute<DataStringAttribute>();
            if (stringAttrib != null)
            {
                IsExported = true;
                StringCodePage = stringAttrib.CodePage;
                StringCoding = stringAttrib.Coding;
                StringLength = stringAttrib.Length;
            }

            // Members of enumerable type must be decorated with the DataArrayAttribute.
            if (Type.IsEnumerable() && arrayAttrib == null)
            {
                throw new InvalidOperationException(
                    $"Enumerable member \"{MemberInfo}\" must be decorated with a {nameof(DataArrayAttribute)}.");
            }
        }

        private MemberData() { }

        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        internal Type Type { get; }

        internal MemberInfo MemberInfo { get; }

        internal bool IsExported { get; }

        // ---- DataArrayAttribute ----

        /// <summary>
        /// Gets or sets the number of elements to read.
        /// </summary>
        public int ArrayCount { get; }

        /// <summary>
        /// Gets or sets the name of the member or method to use for retrieving the number of elements to read.
        /// </summary>
        public string ArrayCountProvider { get; }

        /// <summary>
        /// Gets or sets the data format in which the length of the array is read or written.
        /// </summary>
        public ArrayLengthCoding? ArrayCountCoding { get; }

        /// <summary>
        /// Gets or sets the endianness of the array length value.
        /// </summary>
        public Endian ArrayCountCodingEndian { get; }

        // ---- DataBooleanAttribute ----

        /// <summary>
        /// Gets or sets the data format in which the value is read or written.
        /// </summary>
        public BooleanCoding BooleanCoding { get; }

        // ---- DataConverterAttribute ----

        /// <summary>
        /// Gets or sets the type of the <see cref="IDataConverter"/> to use.
        /// </summary>
        public Type ConverterType { get; }

        // ---- DataDateTimeCoding ----

        /// <summary>
        /// Gets or sets the data format in which the value is read or written.
        /// </summary>
        public DateTimeCoding DateTimeCoding { get; }

        // ---- DataEndianAttribute ----

        /// <summary>
        /// Gets or sets the endianness in which to read or write the value.
        /// </summary>
        public Endian Endian { get; }

        // ---- DataEnumAttribute

        /// <summary>
        /// Gets or sets a value indicating whether the value is validated before it is read or written.
        /// </summary>
        public bool EnumStrict { get; }

        // ---- DataOffsetAttribute ----

        /// <summary>
        /// Gets or sets the number of bytes to manipulate the stream position with.
        /// </summary>
        public long OffsetDelta { get; }

        /// <summary>
        /// Gets or sets the anchor from which to manipulate the stream position by the delta.
        /// </summary>
        public Origin OffsetOrigin { get; }

        // ---- DataOrderAttribute ----

        /// <summary>
        /// Gets or sets a value determining the order when the value is read or written.
        /// </summary>
        public int Index { get; } = Int32.MinValue;

        // ---- DataStringAttribute ----

        /// <summary>
        /// Gets or sets the code page of the <see cref="Encoding"/> to use for reading or writing the string.
        /// </summary>
        public int StringCodePage { get; }

        /// <summary>
        /// Gets or sets the data format in which the value is read or written.
        /// </summary>
        public StringCoding StringCoding { get; }

        /// <summary>
        /// Gets or sets the length of the string to read.
        /// </summary>
        public int StringLength { get; }

        // ---- METHODS (INTERNAL) -------------------------------------------------------------------------------------

        internal int GetArrayCount(Stream stream, ByteConverter byteConverter, object instance)
        {
            // Retrieve from stream.
            if (ArrayCountCoding.HasValue)
            {
                byteConverter = ArrayCountCodingEndian == Endian.None
                    ? byteConverter : ByteConverter.GetConverter(ArrayCountCodingEndian);
                switch (ArrayCountCoding)
                {
                    case ArrayLengthCoding.DynamicCount:
                        return stream.Read7BitEncodedInt32();
                    case ArrayLengthCoding.ByteCount:
                        return stream.ReadByte();
                    case ArrayLengthCoding.Int16Count:
                        return stream.ReadInt16(byteConverter);
                    case ArrayLengthCoding.Int32Count:
                        return stream.ReadInt32(byteConverter);
                    case ArrayLengthCoding.UInt16Count:
                        return stream.ReadUInt16(byteConverter);
                }
            }
            
            // Retrieve the numerical count from a member.
            if (ArrayCountProvider != null)
            {
                MemberInfo memberInfo = instance.GetType().GetMember(ArrayCountProvider, BindingFlags.Instance)[0];
                switch (memberInfo)
                {
                    case FieldInfo fieldInfo:
                        return (int)fieldInfo.GetValue(instance);
                    case PropertyInfo propertyInfo:
                        return (int)propertyInfo.GetValue(instance);
                    case MethodInfo methodInfo:
                        return (int)methodInfo.Invoke(instance, null);
                }
                throw new InvalidOperationException($"Array count cannot be retrieved from {memberInfo}.");
            }

            // Retrieve a numerical count.
            return ArrayCount;
        }
    }
}

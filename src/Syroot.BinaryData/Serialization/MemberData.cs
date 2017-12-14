using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using Syroot.BinaryData.Extensions;

namespace Syroot.BinaryData.Serialization
{
    /// <summary>
    /// Represents information on a member of a type read and written through binary serialiazion.
    /// </summary>
    [DebuggerDisplay(nameof(MemberData) + " " + nameof(MemberInfo) + "={" + nameof(MemberInfo) + "}")]
    public class MemberData
    {
        // ---- CONSTRUCTORS & DESTRUCTOR ------------------------------------------------------------------------------

        internal MemberData(MemberInfo memberInfo, Type type, DataArrayAttribute arrayAttrib,
            DataBooleanAttribute booleanAttrib, DataConverterAttribute converterAttrib,
            DataDateTimeAttribute dateTimeAttrib, DataEndianAttribute endianAttrib, DataEnumAttribute enumAttrib,
            DataMemberAttribute memberAttrib, DataOffsetAttribute offsetAttrib, DataStringAttribute stringAttrib)
        {
            MemberInfo = memberInfo;
            Type = type;

            if (arrayAttrib != null)
            {
                ArrayCount = arrayAttrib.Count;
                ArrayCountProvider = arrayAttrib.CountProvider;
            }
            if (booleanAttrib != null)
            {
                BooleanCoding = booleanAttrib.Coding;
            }
            if (converterAttrib != null)
            {
                ConverterType = converterAttrib.ConverterType;
            }
            if (dateTimeAttrib != null)
            {
                DateTimeCoding = dateTimeAttrib.Coding;
            }
            if (endianAttrib != null)
            {
                Endian = endianAttrib.Endian;
            }
            if (enumAttrib != null)
            {
                EnumStrict = enumAttrib.Strict;
            }
            if (offsetAttrib != null)
            {
                OffsetDelta = offsetAttrib.Delta;
                OffsetOrigin = offsetAttrib.Origin;
            }
            if (stringAttrib != null)
            {
                StringCodePage = stringAttrib.CodePage;
                StringCoding = stringAttrib.Coding;
                StringLength = stringAttrib.Length;
            }
        }

        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        internal MemberInfo MemberInfo { get; }

        internal Type Type { get; }

        // ---- DataArrayAttribute ----

        /// <summary>
        /// Gets or sets the number of elements to read.
        /// </summary>
        public int ArrayCount { get; } = 0;

        /// <summary>
        /// Gets or sets the name of the member or method to use for retrieving the number of elements to read.
        /// </summary>
        public string ArrayCountProvider { get; } = null;

        // ---- DataBooleanAttribute ----

        /// <summary>
        /// Gets or sets the data format in which the value is read or written.
        /// </summary>
        public BooleanCoding BooleanCoding { get; } = BooleanCoding.Byte;

        // ---- DataConverterAttribute ----

        /// <summary>
        /// Gets or sets the type of the <see cref="IDataConverter"/> to use.
        /// </summary>
        public Type ConverterType { get; } = null;

        // ---- DataDateTimeCoding ----

        /// <summary>
        /// Gets or sets the data format in which the value is read or written.
        /// </summary>
        public DateTimeCoding DateTimeCoding { get; } = DateTimeCoding.NetTicks;

        // ---- DataEndianAttribute ----

        /// <summary>
        /// Gets or sets the endianness in which to read or write the value.
        /// </summary>
        public Endian Endian { get; } = 0;

        // ---- DataEnumAttribute

        /// <summary>
        /// Gets or sets a value indicating whether the value is validated before it is read or written.
        /// </summary>
        public bool EnumStrict { get; } = false;

        // ---- DataOffsetAttribute ----

        /// <summary>
        /// Gets or sets the number of bytes to manipulate the stream position with.
        /// </summary>
        public int OffsetDelta { get; } = 0;

        /// <summary>
        /// Gets or sets the anchor from which to manipulate the stream position by the delta.
        /// </summary>
        public Origin OffsetOrigin { get; } = Origin.Add;

        // ---- DataStringAttribute ----

        /// <summary>
        /// Gets or sets the code page of the <see cref="Encoding"/> to use for reading or writing the string.
        /// </summary>
        public int StringCodePage { get; } = 0;

        /// <summary>
        /// Gets or sets the data format in which the value is read or written.
        /// </summary>
        public StringCoding StringCoding { get; } = 0;

        /// <summary>
        /// Gets or sets the length of the string to read.
        /// </summary>
        public int StringLength { get; } = 0;

        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------
        
        public int GetArrayCount(object instance)
        {
            if (ArrayCount == 0 || ArrayCountProvider == null)
                return 0;

            // Prefer a numerical count if provided.
            if (ArrayCount > 0)
                return ArrayCount;

            // Retrieve the numerical count from a member.
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
    }
}

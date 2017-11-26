using System;
using System.Diagnostics;
using System.Reflection;

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
            ArrayAttrib = arrayAttrib;
            BooleanAttrib = booleanAttrib;
            ConverterAttrib = converterAttrib;
            DateTimeAttrib = dateTimeAttrib;
            EndianAttrib = endianAttrib;
            EnumAttrib = enumAttrib;
            MemberAttrib = memberAttrib;
            OffsetAttrib = offsetAttrib;
            StringAttrib = stringAttrib;
    }

        // ---- PROPERTIES ---------------------------------------------------------------------------------------------
        
        /// <summary>
        /// Gets the <see cref="DataArrayAttribute"/> the member was possibly decorated with.
        /// </summary>
        public DataArrayAttribute ArrayAttrib { get; }

        /// <summary>
        /// Gets the <see cref="DataBooleanAttribute"/> the member was possibly decorated with.
        /// </summary>
        public DataBooleanAttribute BooleanAttrib { get; }

        /// <summary>
        /// Gets the <see cref="DataConverterAttribute"/> the member was possibly decorated with.
        /// </summary>
        public DataConverterAttribute ConverterAttrib { get; }

        /// <summary>
        /// Gets the <see cref="DataDateTimeAttribute"/> the member was possibly decorated with.
        /// </summary>
        public DataDateTimeAttribute DateTimeAttrib { get; }

        /// <summary>
        /// Gets the <see cref="DataEndianAttribute"/> the member was possibly decorated with.
        /// </summary>
        public DataEndianAttribute EndianAttrib { get; }

        /// <summary>
        /// Gets the <see cref="DataEnumAttribute"/> the member was possibly decorated with.
        /// </summary>
        public DataEnumAttribute EnumAttrib { get; }

        /// <summary>
        /// Gets the <see cref="DataMemberAttribute"/> the member was possibly decorated with.
        /// </summary>
        public DataMemberAttribute MemberAttrib { get; }

        /// <summary>
        /// Gets the <see cref="DataOffsetAttribute"/> the member was possibly decorated with.
        /// </summary>
        public DataOffsetAttribute OffsetAttrib { get; }

        /// <summary>
        /// Gets the <see cref="DataStringAttribute"/> the member was possibly decorated with.
        /// </summary>
        public DataStringAttribute StringAttrib { get; }

        internal MemberInfo MemberInfo { get; }

        internal Type Type { get; }
    }
}

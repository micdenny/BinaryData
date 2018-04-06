namespace Syroot.BinaryData
{
    /// <summary>
    /// Represents the possible byte order of binary data.
    /// </summary>
    public enum Endian : ushort
    {
        /// <summary>
        /// Indicates that the endianness will not be changed for this operation.
        /// </summary>
        None,

        /// <summary>
        /// Indicates the byte order of the system executing the assembly.
        /// </summary>
        System = 1,

        /// <summary>
        /// Indicates big endian byte order.
        /// </summary>
        Big = 0xFEFF,

        /// <summary>
        /// Indicates little endian byte order.
        /// </summary>
        Little = 0xFFFE
    }
}

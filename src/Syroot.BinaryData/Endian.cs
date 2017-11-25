namespace Syroot.BinaryData
{
    /// <summary>
    /// Represents the possible byte order of binary data.
    /// </summary>
    public enum Endian : ushort
    {
        /// <summary>
        /// Indicates the byte order of the system executing the assembly.
        /// </summary>
        System,

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

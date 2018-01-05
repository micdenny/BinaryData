namespace Syroot.BinaryData
{
    /// <summary>
    /// Represents formats of array length encodings.
    /// </summary>
    public enum ArrayLengthCoding
    {
        /// <summary>
        /// The array  has a prefix of a 7-bit encoded integer of variable size determining the number of elements out
        /// of which the array consists.
        /// </summary>
        DynamicCount,

        /// <summary>
        /// The array has a prefix of 1 byte determining the number of elements out of which the array consists.
        /// </summary>
        ByteCount,

        /// <summary>
        /// The array has a prefix of 2 bytes determining the number of elements out of which the array consists.
        /// </summary>
        Int16Count,

        /// <summary>
        /// The array has a prefix of 4 bytes determining number of elements out of which the array consists.
        /// </summary>
        Int32Count,

        /// <summary>
        /// The array has a prefix of 2 bytes determining the number of elements out of which the array consists.
        /// </summary>
        UInt16Count
    }
}

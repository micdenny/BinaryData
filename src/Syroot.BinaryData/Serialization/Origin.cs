namespace Syroot.BinaryData.Serialization
{
    /// <summary>
    /// Represents the origins of offsets of a class, structure, or member.
    /// </summary>
    public enum Origin
    {
        /// <summary>
        /// The origin is relative to the most recent position of the data stream.
        /// </summary>
        Add,

        /// <summary>
        /// The origin is relative to the start of the class or structure, which is the position at which reading or
        /// writing the instance of the top-most base type has been initiated.
        /// </summary>
        Set,

        /// <summary>
        /// Aligns the current stream position to the given byte multiple.
        /// </summary>
        Align
    }
}

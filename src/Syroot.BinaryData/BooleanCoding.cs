namespace Syroot.BinaryData
{
    /// <summary>
    /// Represents the set of formats of binary boolean encodings.
    /// </summary>
    public enum BooleanCoding
    {
        /// <summary>
        /// The boolean is stored in 1 byte and is <c>true</c> when the value is not 0. This is the .NET default.
        /// </summary>
        Byte,

        /// <summary>
        /// The boolean is stored in 2 bytes and is <c>true</c> when the value is not 0.
        /// </summary>
        Word,

        /// <summary>
        /// The boolean is stored in 4 bytes and is <c>true</c> when the value is not 0.
        /// </summary>
        Dword
    }
}

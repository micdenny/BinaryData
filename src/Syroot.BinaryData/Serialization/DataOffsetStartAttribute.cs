using System;

namespace Syroot.BinaryData.Serialization
{
    /// <summary>
    /// Configures an offset which is seeked to before reading the instance through binary serialization.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class DataOffsetStartAttribute : DataOffsetAttribute
    {
        // ---- CONSTRUCTORS & DESTRUCTOR ------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="DataOffsetStartAttribute"/> class with the given configuration.
        /// </summary>
        /// <param name="origin">The anchor from which to manipulate the stream position by the given delta.</param>
        /// <param name="delta">The number of bytes to manipulate the stream position with.</param>
        public DataOffsetStartAttribute(Origin origin, long delta) : base(origin, delta) { }
    }
}

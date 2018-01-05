using System;

namespace Syroot.BinaryData
{
    /// <summary>
    /// Configures an offset which is seeked to after reading the instance through binary serialization.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class OffsetEndAttribute : OffsetAttribute
    {
        // ---- CONSTRUCTORS & DESTRUCTOR ------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="OffsetEndAttribute"/> class with the given configuration.
        /// </summary>
        /// <param name="origin">The anchor from which to manipulate the stream position by the given delta.</param>
        /// <param name="delta">The number of bytes to manipulate the stream position with.</param>
        public OffsetEndAttribute(Origin origin, long delta) : base(origin, delta) { }
    }
}

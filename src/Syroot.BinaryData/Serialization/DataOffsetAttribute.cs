using System;

namespace Syroot.BinaryData.Serialization
{
    /// <summary>
    /// Configures an offset at which the value is read or written through binary serialization.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class DataOffsetAttribute : Attribute
    {
        // ---- CONSTRUCTORS & DESTRUCTOR ------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="DataOffsetAttribute"/> class with the given configuration.
        /// </summary>
        /// <param name="origin">The anchor from which to manipulate the stream position by the given delta.</param>
        /// <param name="delta">The number of bytes to manipulate the stream position with.</param>
        public DataOffsetAttribute(Origin origin, int delta)
        {
            Origin = origin;
            Delta = delta;
        }
        
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        /// <summary>
        /// Gets or sets the anchor from which to manipulate the stream position by the delta.
        /// </summary>
        public Origin Origin { get; }
        
        /// <summary>
        /// Gets or sets the number of bytes to manipulate the stream position with.
        /// </summary>
        public int Delta { get; }
    }
}

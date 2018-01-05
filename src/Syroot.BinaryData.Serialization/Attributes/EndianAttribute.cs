using System;

namespace Syroot.BinaryData
{
    /// <summary>
    /// Configures in which endianness a value is read or written through binary serialization.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class EndianAttribute : Attribute
    {
        // ---- CONSTRUCTORS & DESTRUCTOR ------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="EndianAttribute"/> class with the given configuration.
        /// </summary>
        /// <param name="endian">The endianness in which to read or write the value.</param>
        public EndianAttribute(Endian endian)
        {
            Endian = endian;
        }
        
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        /// <summary>
        /// Gets or sets the endianness in which to read or write the value.
        /// </summary>
        public Endian Endian { get; }
    }
}

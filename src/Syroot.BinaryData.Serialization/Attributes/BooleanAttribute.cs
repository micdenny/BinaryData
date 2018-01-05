using System;

namespace Syroot.BinaryData
{
    /// <summary>
    /// Configures how a <see cref="Boolean"/> member is read or written through binary serialization.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class BooleanAttribute : Attribute
    {
        // ---- CONSTRUCTORS & DESTRUCTOR ------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="BooleanAttribute"/> class with the given configuration.
        /// </summary>
        /// <param name="coding">The <see cref="BooleanCoding"/> to read or write the value in.</param>
        public BooleanAttribute(BooleanCoding coding)
        {
            Coding = coding;
        }

        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        /// <summary>
        /// Gets or sets the data format in which the value is read or written.
        /// </summary>
        public BooleanCoding Coding { get; }
    }
}

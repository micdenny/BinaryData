using System;

namespace Syroot.BinaryData
{
    /// <summary>
    /// Configures the index determining the order in which members of an instance are read or written through binary
    /// serialization.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class DataOrderAttribute : Attribute
    {
        // ---- CONSTRUCTORS & DESTRUCTOR ------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="DataOrderAttribute"/> class with the given configuration.
        /// </summary>
        /// <param name="index">The index determining the order when the value is read or written.</param>
        public DataOrderAttribute(int index)
        {
            Index = index;
        }

        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        /// <summary>
        /// Gets or sets a value determining the order when the value is read or written.
        /// </summary>
        public int Index { get; }
    }
}

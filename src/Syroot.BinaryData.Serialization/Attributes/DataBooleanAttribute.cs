using System;

namespace Syroot.BinaryData
{
    /// <summary>
    /// Configures how a <see cref="Boolean"/> member is read or written through binary serialization.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class DataBooleanAttribute : Attribute
    {
        // ---- CONSTRUCTORS & DESTRUCTOR ------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="DataBooleanAttribute"/> class with the given configuration.
        /// </summary>
        /// <param name="coding">The <see cref="BooleanCoding"/> to read or write the value in.</param>
        public DataBooleanAttribute(BooleanCoding coding)
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

using System;

namespace Syroot.BinaryData
{
    /// <summary>
    /// Configures how an <see cref="Enum"/> member is read or written through binary serialization.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class DataEnumAttribute : Attribute
    {
        // ---- CONSTRUCTORS & DESTRUCTOR ------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="DataEnumAttribute"/> class with the given configuration.
        /// </summary>
        /// <param name="strict"><c>true</c> to validate the value when it is read or written.</param>
        public DataEnumAttribute(bool strict)
        {
            Strict = strict;
        }

        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        /// <summary>
        /// Gets or sets a value indicating whether the value is validated before it is read or written.
        /// </summary>
        public bool Strict { get; }
    }
}

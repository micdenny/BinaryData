using System;
using System.IO;

namespace Syroot.BinaryData
{
    /// <summary>
    /// Configures how a <see cref="DateTime"/> member is read or written through binary serialization.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class DateTimeAttribute : Attribute
    {
        // ---- CONSTRUCTORS & DESTRUCTOR ------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeAttribute"/> class with the given configuration.
        /// </summary>
        /// <param name="coding">The <see cref="DateTimeCoding"/> to read or write the value in.</param>
        public DateTimeAttribute(DateTimeCoding coding)
        {
            Coding = coding;
        }

        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        /// <summary>
        /// Gets or sets the data format in which the value is read or written.
        /// </summary>
        public DateTimeCoding Coding { get; }
    }
}

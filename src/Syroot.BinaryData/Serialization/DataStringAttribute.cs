using System;
using System.Text;

namespace Syroot.BinaryData.Serialization
{
    /// <summary>
    /// Configures how a <see cref="String"/> member is read or written through binary serialization.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class DataStringAttribute : Attribute
    {
        // ---- CONSTRUCTORS & DESTRUCTOR ------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="DataStringAttribute"/> class with the given configuration.
        /// </summary>
        /// <param name="coding">The <see cref="StringCoding"/> to read or write the value in.</param>
        /// <param name="codePage">The code page of the <see cref="Encoding"/> to use.</param>
        public DataStringAttribute(StringCoding coding, int codePage = 0)
        {
            Coding = coding;
            CodePage = codePage;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataStringAttribute"/> class with the given configuration.
        /// </summary>
        /// <param name="length">The length of the string to read. Handles the string with
        /// <see cref="StringCoding.Raw"/>.</param>
        /// <param name="codePage">The code page of the <see cref="Encoding"/> to use.</param>
        public DataStringAttribute(int length, int codePage = 0)
        {
            Length = length;
            CodePage = codePage;
        }

        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        /// <summary>
        /// Gets or sets the data format in which the value is read or written.
        /// </summary>
        public StringCoding Coding { get; }

        /// <summary>
        /// Gets or sets the length of the string to read.
        /// </summary>
        public int Length { get; }

        /// <summary>
        /// Gets or sets the code page of the <see cref="Encoding"/> to use for reading or writing the string.
        /// </summary>
        public int CodePage { get; }
    }
}

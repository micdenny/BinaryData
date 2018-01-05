using System;
using System.Collections;

namespace Syroot.BinaryData
{
    /// <summary>
    /// Configures how many elements in an <see cref="IEnumerable"/> member are read through binary serialization.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ArrayAttribute : Attribute
    {
        // ---- CONSTRUCTORS & DESTRUCTOR ------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="ArrayAttribute"/> class with the given configuration.
        /// </summary>
        /// <param name="count">The number of elements to read.</param>
        public ArrayAttribute(int count)
        {
            Count = count;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArrayAttribute"/> class with the given configuration.
        /// </summary>
        /// <param name="countProvider">The name of the member or method to use for retrieving the number of elements to
        /// read.</param>
        public ArrayAttribute(string countProvider)
        {
            CountProvider = countProvider;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArrayAttribute"/> class with the given configuration.
        /// </summary>
        /// <param name="coding">The <see cref="ArrayLengthCoding"/> to read or write the value in.</param>
        /// <param name="endian">The <see cref="Endian"/> of this value.</param>
        public ArrayAttribute(ArrayLengthCoding coding, Endian endian = Endian.None)
        {
            Coding = coding;
            CodingEndian = endian;
        }

        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        /// <summary>
        /// Gets or sets the number of elements to read.
        /// </summary>
        public int Count { get; }

        /// <summary>
        /// Gets or sets the name of the member or method to use for retrieving the number of elements to read.
        /// </summary>
        public string CountProvider { get; }

        /// <summary>
        /// Gets or sets the data format in which the length of the array is read or written.
        /// </summary>
        public ArrayLengthCoding Coding { get; }

        /// <summary>
        /// Gets or sets the endianness of the length value.
        /// </summary>
        public Endian CodingEndian { get; }
    }
}

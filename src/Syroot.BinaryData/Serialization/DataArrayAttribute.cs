using System;
using System.Collections;

namespace Syroot.BinaryData.Serialization
{
    /// <summary>
    /// Configures how many elements in an <see cref="IEnumerable"/> member are read through binary serialization.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class DataArrayAttribute : Attribute
    {
        // ---- CONSTRUCTORS & DESTRUCTOR ------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="DataArrayAttribute"/> class with the given configuration.
        /// </summary>
        /// <param name="count">The number of elements to read.</param>
        public DataArrayAttribute(int count)
        {
            Count = count;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataArrayAttribute"/> class with the given configuration.
        /// </summary>
        /// <param name="countProvider">The name of the member or method to use for retrieving the number of elements to
        /// read.</param>
        public DataArrayAttribute(string countProvider)
        {
            CountProvider = countProvider;
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
    }
}

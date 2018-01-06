using System;

namespace Syroot.BinaryData
{
    /// <summary>
    /// Configures an <see cref="IDataConverter"/> to read or write the member with through binary serialization.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class DataConverterAttribute : Attribute
    {
        // ---- CONSTRUCTORS & DESTRUCTOR ------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="DataConverterAttribute"/> class with the given configuration.
        /// </summary>
        /// <param name="converterType">The type of the <see cref="IDataConverter"/> to use.</param>
        public DataConverterAttribute(Type converterType)
        {
            ConverterType = converterType;
        }

        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        /// <summary>
        /// Gets or sets the type of the <see cref="IDataConverter"/> to use.
        /// </summary>
        public Type ConverterType { get; }
    }
}

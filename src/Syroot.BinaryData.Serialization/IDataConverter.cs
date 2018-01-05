using System.IO;

namespace Syroot.BinaryData
{
    /// <summary>
    /// Represents a converter for reading and writing custom values.
    /// </summary>
    public interface IDataConverter
    {
        // ---- METHODS ------------------------------------------------------------------------------------------------

        /// <summary>
        /// Reads the value from the given <paramref name="stream"/> and returns it to set the corresponding member.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to read the value from.</param>
        /// <param name="instance">The instance to which the value belongs.</param>
        /// <param name="memberData">The <see cref="MemberData"/> configuring the field via attributes.</param>
        /// <returns>The read value.</returns>
        object Read(Stream stream, object instance, MemberData memberData);

        /// <summary>
        /// Writes the value with the given <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to write the value with.</param>
        /// <param name="instance">The instance to which the value belongs.</param>
        /// <param name="memberData">The <see cref="MemberData"/> configuring the field via attributes.</param>
        /// <param name="value">The value to write.</param>
        void Write(Stream stream, object instance, MemberData memberData, object value);
    }
}

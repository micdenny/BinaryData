using System.IO;

namespace Syroot.BinaryData
{
    /// <summary>
    /// Represents a space of 4 bytes reserved in the underlying stream which can be comfortably satisfied later on.
    /// </summary>
    public class Offset
    {
        // ---- CONSTRUCTORS & DESTRUCTOR ------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="Offset"/> class reserving an offset with the specified <paramref name="stream"/> at the current position.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> in which the offset will be reserved.</param>
        public Offset(Stream stream)
        {
            Stream = stream;
            Position = (uint)Stream.Position;
            Stream.Position += sizeof(uint);
        }

        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        /// <summary>
        /// Gets the <see cref="Stream"/> in which the allocation is made.
        /// </summary>
        public Stream Stream { get; private set; }

        /// <summary>
        /// Gets the address at which the allocation is made.
        /// </summary>
        public uint Position { get; private set; }

        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        /// <summary>
        /// Satisfies the offset by writing the current position of the underlying stream at the reserved <see cref="Position"/>, then seeking back to the current position.
        /// </summary>
        public void Satisfy()
        {
            Satisfy((int)Stream.Position);
        }

        /// <summary>
        /// Satisfies the offset by writing the given value of the underlying stream at the reserved <see cref="Position"/>, then seeking back to the current position.
        /// </summary>
        public void Satisfy(int value)
        {
            // Temporarily seek back to the allocation offset and write the given value there, then seek back.
            uint oldPosition = (uint)Stream.Position;
            Stream.Position = Position;
            Stream.Write(value);
            Stream.Position = oldPosition;
        }
    }
}

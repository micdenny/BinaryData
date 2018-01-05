using System.IO;

namespace Syroot.BinaryData
{
    public static partial class StreamExtensions
    {
        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        // ---- Object ----

        /// <summary>
        /// Writes an object or enumerable of objects to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="value">The object or enumerable of objects to write.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        public static void WriteObject(this Stream stream, object value, ByteConverter converter = null)
            => WriteObject(value.GetType(), stream, null, BinaryMemberAttribute.Default, value, converter);
    }
}

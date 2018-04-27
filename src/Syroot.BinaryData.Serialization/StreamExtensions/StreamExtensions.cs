using System;
using System.IO;

namespace Syroot.BinaryData
{
    /// <summary>
    /// Represents extension methods for read and write operations on <see cref="Stream"/> instances.
    /// </summary>
    public static partial class StreamExtensions
    {
        // ---- FIELDS -------------------------------------------------------------------------------------------------

        [ThreadStatic] private static byte[] _buffer;

        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        private static byte[] Buffer
        {
            get
            {
                if (_buffer == null)
                    _buffer = new byte[16];
                return _buffer;
            }
        }
    }
}

using System;
using System.IO;

namespace Syroot.BinaryData
{
    /// <summary>
    /// Represents static extension methods for read and write operations on <see cref="Stream"/> instances.
    /// </summary>
    public static partial class StreamExtensions
    {
        // ---- FIELDS -------------------------------------------------------------------------------------------------

        [ThreadStatic] private static byte[] _buffer;
        [ThreadStatic] private static char[] _charBuffer;

        private static readonly DateTime _cTimeBase = new DateTime(1970, 1, 1);

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

        private static char[] CharBuffer
        {
            get
            {
                if (_charBuffer == null)
                    _charBuffer = new char[16];
                return _charBuffer;
            }
        }
    }
}

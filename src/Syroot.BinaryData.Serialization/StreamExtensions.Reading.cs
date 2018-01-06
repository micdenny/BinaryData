using System;
using System.IO;

namespace Syroot.BinaryData
{
    public static partial class StreamExtensions
    {
        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        /// <summary>
        /// Returns an object of type <typeparamref name="T"/> read from the <paramref name="stream"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object to read.</typeparam>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <returns>The value read from the current stream.</returns>
        public static T ReadObject<T>(this Stream stream, ByteConverter converter = null)
            => (T)_serializer.ReadObject(stream, typeof(T), converter ?? ByteConverter.System);

        /// <summary>
        /// Returns an object of the given <paramref name="type"/> read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="type">The type of the object to read.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <returns>The value read from the current stream.</returns>
        public static object ReadObject(this Stream stream, Type type, ByteConverter converter = null)
            => _serializer.ReadObject(stream, type, converter ?? ByteConverter.System);

        /// <summary>
        /// Returns an array of objects of type <typeparamref name="T"/> read from the <paramref name="stream"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object to load.</typeparam>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static T[] ReadObjects<T>(this Stream stream, int count, ByteConverter converter = null)
        {
            converter = converter ?? ByteConverter.System;
            var values = new T[count];
            lock (stream)
            {
                for (int i = 0; i < count; i++)
                {
                    values[i] = stream.ReadObject<T>(converter);
                }
            }
            return values;
        }

        /// <summary>
        /// Returns an array of objects of the given <paramref name="type"/> read from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="type">The type of the object to load.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use for converting multibyte data.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static object[] ReadObjects(this Stream stream, Type type, int count, ByteConverter converter = null)
        {
            converter = converter ?? ByteConverter.System;
            var values = new object[count];
            lock (stream)
            {
                for (int i = 0; i < count; i++)
                {
                    values[i] = stream.ReadObject(type, converter);
                }
            }
            return values;
        }

        // ---- METHODS (PRIVATE) --------------------------------------------------------------------------------------

        private static void FillBuffer(Stream stream, int length)
        {
            if (stream.Read(Buffer, 0, length) < length)
                throw new EndOfStreamException($"Could not read {length} bytes.");
        }
    }
}

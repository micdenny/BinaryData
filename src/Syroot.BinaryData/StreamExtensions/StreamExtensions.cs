using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Syroot.BinaryData
{
    /// <summary>
    /// Represents extension methods for read and write operations on <see cref="Stream"/> instances.
    /// </summary>
    public static partial class StreamExtensions
    {
        // ---- FIELDS -------------------------------------------------------------------------------------------------

        [ThreadStatic]
        private static byte[] _buffer;

        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        private static byte[] Buffer
        {
            get
            {
                // Instantiate here as inline initialization only runs for the first thread requiring the buffer.
                if (_buffer == null)
                    _buffer = new byte[16];
                return _buffer;
            }
        }

        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        /// <summary>
        /// Aligns the stream to the given byte multiple.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="alignment">The byte multiple to align to. If negative, the position is decreased to the
        /// previous multiple rather than the next one.</param>
        /// <param name="grow"><c>true</c> to enlarge the stream size to include the final position in case it is larger
        /// than the current stream length.</param>
        /// <returns>The new position within the current stream.</returns>
        public static long Align(this Stream stream, long alignment, bool grow = false)
        {
            if (alignment == 0)
                throw new ArgumentOutOfRangeException("Alignment must not be 0.");

            long position = stream.Seek((-stream.Position % alignment + alignment) % alignment, SeekOrigin.Current);

            if (grow && position > stream.Length)
                stream.SetLength(position);

            return position;
        }

        /// <summary>
        /// Gets a value indicating whether the end of the <paramref name="stream"/> has been reached and no more data
        /// can be read.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <returns>A value indicating whether the end of the stream has been reached.</returns>
        public static bool IsEndOfStream(this Stream stream)
        {
            return stream.Position >= stream.Length;
        }

        /// <summary>
        /// Sets the position within the current <paramref name="stream"/> relative to the current position. If the
        /// stream is not seekable, it tries to simulates advancing the position by reading or writing 0-bytes.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="offset">A byte offset relative to the origin parameter.</param>
        public static void Move(this Stream stream, long offset)
        {
            if (stream.CanSeek)
            {
                // No need to run any simulation.
                stream.Seek(offset);
            }
            else
            {
                // Impossible to simulate seeking backwards in non-seekable stream.
                if (offset < 0)
                    throw new NotSupportedException("Cannot simulate moving backwards in a non-seekable stream.");

                // Simulate move by reading or writing bytes.
                if (stream.CanRead)
                {
                    stream.ReadBytes((int)offset);
                }
                else if (stream.CanWrite)
                {
                    for (int i = 0; i < offset; i++)
                        stream.WriteByte(0);
                }
            }
        }

        /// <summary>
        /// Sets the position within the current <paramref name="stream"/> relative to the current position.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="offset">A byte offset relative to the current position in the stream.</param>
        /// <returns>The new position within the current stream.</returns>
        public static long Seek(this Stream stream, long offset)
        {
            return stream.Seek(offset, SeekOrigin.Current);
        }

        /// <summary>
        /// Creates a <see cref="BinaryData.Seek"/> with the given parameters. As soon as the returned
        /// <see cref="BinaryData.Seek"/> is disposed, the previous stream position will be restored.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="offset">A byte offset relative to the origin parameter.</param>
        /// <param name="origin">A value of type <see cref="SeekOrigin"/> indicating the reference point used to obtain
        /// the new position.</param>
        /// <returns>The <see cref="BinaryData.Seek"/> to be disposed to undo the seek.</returns>
        public static Seek TemporarySeek(this Stream stream, long offset = 0, SeekOrigin origin = SeekOrigin.Current)
        {
            return new Seek(stream, offset, origin);
        }

        // ---- Read ----

        /// <summary>
        /// Returns <paramref name="count"/> instances of type <typeparamref name="T"/> continually read from the
        /// <paramref name="stream"/> by calling the <paramref name="readCallback"/>.
        /// </summary>
        /// <typeparam name="T">The type of the instances to read.</typeparam>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of instances to read.</param>
        /// <param name="readCallback">The read callback function invoked for each instance read.</param>
        /// <returns>The array of read instances.</returns>
        public static T[] ReadMany<T>(this Stream stream, int count, Func<T> readCallback)
        {
            T[] values = new T[count];
            for (int i = 0; i < count; i++)
                values[i] = readCallback();
            return values;
        }

        /// <summary>
        /// Returns <paramref name="count"/> instances of type <typeparamref name="T"/> continually read asynchronously
        /// from the <paramref name="stream"/> by calling the <paramref name="readCallback"/>.
        /// </summary>
        /// <typeparam name="T">The type of the instances to read.</typeparam>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of instances to read.</param>
        /// <param name="readCallback">The read callback function invoked for each instance read.</param>
        /// <returns>The array of read instances.</returns>
        public static async Task<T[]> ReadManyAsync<T>(this Stream stream, int count, Func<Task<T>> readCallback)
        {
            T[] values = new T[count];
            for (int i = 0; i < count; i++)
                values[i] = await readCallback();
            return values;
        }

        // ---- Write ----

        /// <summary>
        /// Writes the <paramref name="values"/> to the <paramref name="stream"/> through the
        /// <paramref name="writeCallback"/> invoked for each value.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="writeCallback">The callback invoked to write each value.</param>
        public static void WriteMany<T>(this Stream stream, IEnumerable<T> values, Action<T> writeCallback)
        {
            foreach (T value in values)
                writeCallback(value);
        }

        /// <summary>
        /// Writes the <paramref name="values"/> to the <paramref name="stream"/> asynchronously through the
        /// <paramref name="writeCallback"/> invoked for each value.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="writeCallback">The callback invoked to write each value.</param>
        public static async Task WriteManyAsync<T>(this Stream stream, IEnumerable<T> values,
            Func<T, Task> writeCallback)
        {
            foreach (T value in values)
                await writeCallback(value);
        }

        // ---- METHODS (PRIVATE) --------------------------------------------------------------------------------------

        private static void FillBuffer(Stream stream, int length)
        {
            if (stream.Read(Buffer, 0, length) < length)
                throw new EndOfStreamException($"Could not read {length} bytes.");
        }

        private static async Task FillBufferAsync(Stream stream, int length, CancellationToken cancellationToken)
        {
            if (await stream.ReadAsync(Buffer, 0, length, cancellationToken) < length)
                throw new EndOfStreamException($"Could not read {length} bytes.");
        }
    }
}

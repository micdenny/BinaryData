using System;
using System.IO;

namespace Syroot.BinaryData
{
    /// <summary>
    /// Represents a temporary seek to another position which is undone after the task has been disposed.
    /// </summary>
    public class Seek : IDisposable
    {
        // ---- FIELDS -------------------------------------------------------------------------------------------------

        private bool _disposed;

        // ---- CONSTRUCTORS -------------------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="Seek"/> class to temporarily seek the given
        /// <see cref="Stream"/> to the specified position. The <see cref="System.IO.Stream"/> is rewound to its
        /// previous position after the task is disposed.
        /// </summary>
        /// <param name="stream">A <see cref="System.IO.Stream"/> to temporarily seek.</param>
        /// <param name="offset">A byte offset relative to the origin parameter.</param>
        /// <param name="origin">A value of type <see cref="SeekOrigin"/> indicating the reference point used to obtain
        /// the new position.</param>
        public Seek(Stream stream, long offset, SeekOrigin origin)
        {
            Stream = stream;
            PreviousPosition = Stream.Position;
            Stream.Seek(offset, origin);
        }

        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        /// <summary>
        /// Gets the <see cref="Stream"/> which is temporarily sought to another position.
        /// </summary>
        public Stream Stream { get; private set; }

        /// <summary>
        /// Gets the absolute position to which the <see cref="Stream"/> will be rewound when this task is disposed.
        /// </summary>
        public long PreviousPosition { get; private set; }

        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing).
            Dispose(true);
        }

        // ---- METHODS (PROTECTED) ------------------------------------------------------------------------------------

        /// <summary>
        /// Rewinds the stream to t he previous position.
        /// </summary>
        /// <param name="disposing"><c>true</c> to rewind the stream.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                    Stream.Seek(PreviousPosition, SeekOrigin.Begin);

                _disposed = true;
            }
        }
    }
}

namespace Syroot.BinaryData
{
    /// <summary>
    /// Represents a collection of extension methods for the <see cref="BinaryDataWriter"/> class.
    /// </summary>
    public static class BinaryDataWriterExtensions
    {
        // ---- Object ----

        /// <summary>
        /// Writes an object or enumerable of objects to this stream.
        /// </summary>
        /// <param name="self">The extended <see cref="BinaryDataWriter"/> instance.</param>
        /// <param name="value">The object or enumerable of objects to write.</param>
        public static void WriteObject(this BinaryDataWriter self, object value)
            => self.BaseStream.WriteObject(value, self.ByteConverter);
    }
}

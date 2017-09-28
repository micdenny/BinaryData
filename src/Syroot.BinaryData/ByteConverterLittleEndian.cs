using System;

namespace Syroot.BinaryData
{
    /// <summary>
    /// Represents a <see cref="BitConverter"/> which handles little endianness.
    /// </summary>
    public class ByteConverterLittleEndian : ByteConverter
    {
        // ---- CONSTRUCTORS & DESTRUCTOR ------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="ByteConverterLittleEndian"/> class.
        /// </summary>
        internal ByteConverterLittleEndian()
            : base(ByteOrder.LittleEndian)
        {
        }

        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------
        
        /// <summary>
        /// Stores the specified <see cref="Int16"/> value as bytes in the given <paramref name="array"/>, starting at
        /// the provided <paramref name="startIndex"/>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="array">The byte array to store the value in.</param>
        /// <param name="startIndex">The index at which to start storing the value.</param>
        /// <returns>An array of bytes representing the value.</returns>
        public override void GetBytes(Int16 value, byte[] array, int startIndex)
        {
            array[startIndex] = (byte)value;
            array[startIndex + 1] = (byte)(value >> 8);
        }

        /// <summary>
        /// Stores the specified <see cref="Int32"/> value as bytes in the given <paramref name="array"/>, starting at
        /// the provided <paramref name="startIndex"/>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="array">The byte array to store the value in.</param>
        /// <param name="startIndex">The index at which to start storing the value.</param>
        /// <returns>An array of bytes representing the value.</returns>
        public override void GetBytes(Int32 value, byte[] array, int startIndex)
        {
            array[startIndex] = (byte)value;
            array[startIndex + 1] = (byte)(value >> 8);
            array[startIndex + 2] = (byte)(value >> 16);
            array[startIndex + 3] = (byte)(value >> 24);
        }

        /// <summary>
        /// Stores the specified <see cref="Int64"/> value as bytes in the given <paramref name="array"/>, starting at
        /// the provided <paramref name="startIndex"/>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="array">The byte array to store the value in.</param>
        /// <param name="startIndex">The index at which to start storing the value.</param>
        /// <returns>An array of bytes representing the value.</returns>
        public override void GetBytes(Int64 value, byte[] array, int startIndex)
        {
            array[startIndex] = (byte)value;
            array[startIndex + 1] = (byte)(value >> 8);
            array[startIndex + 2] = (byte)(value >> 16);
            array[startIndex + 3] = (byte)(value >> 24);
            array[startIndex + 4] = (byte)(value >> 32);
            array[startIndex + 5] = (byte)(value >> 40);
            array[startIndex + 6] = (byte)(value >> 48);
            array[startIndex + 7] = (byte)(value >> 56);
        }
        
        /// <summary>
        /// Stores the specified <see cref="UInt16"/> value as bytes in the given <paramref name="array"/>, starting at
        /// the provided <paramref name="startIndex"/>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="array">The byte array to store the value in.</param>
        /// <param name="startIndex">The index at which to start storing the value.</param>
        /// <returns>An array of bytes representing the value.</returns>
        public override void GetBytes(UInt16 value, byte[] array, int startIndex)
        {
            array[startIndex] = (byte)value;
            array[startIndex + 1] = (byte)(value >> 8);
        }

        /// <summary>
        /// Stores the specified <see cref="UInt32"/> value as bytes in the given <paramref name="array"/>, starting at
        /// the provided <paramref name="startIndex"/>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="array">The byte array to store the value in.</param>
        /// <param name="startIndex">The index at which to start storing the value.</param>
        /// <returns>An array of bytes representing the value.</returns>
        public override void GetBytes(UInt32 value, byte[] array, int startIndex)
        {
            array[startIndex] = (byte)value;
            array[startIndex + 1] = (byte)(value >> 8);
            array[startIndex + 2] = (byte)(value >> 16);
            array[startIndex + 3] = (byte)(value >> 24);
        }

        /// <summary>
        /// Stores the specified <see cref="UInt64"/> value as bytes in the given <paramref name="array"/>, starting at
        /// the provided <paramref name="startIndex"/>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="array">The byte array to store the value in.</param>
        /// <param name="startIndex">The index at which to start storing the value.</param>
        /// <returns>An array of bytes representing the value.</returns>
        public override void GetBytes(UInt64 value, byte[] array, int startIndex)
        {
            array[startIndex] = (byte)value;
            array[startIndex + 1] = (byte)(value >> 8);
            array[startIndex + 2] = (byte)(value >> 16);
            array[startIndex + 3] = (byte)(value >> 24);
            array[startIndex + 4] = (byte)(value >> 32);
            array[startIndex + 5] = (byte)(value >> 40);
            array[startIndex + 6] = (byte)(value >> 48);
            array[startIndex + 7] = (byte)(value >> 56);
        }
        
        /// <summary>
        /// Returns an <see cref="Int16"/> instance converted from the bytes at the specific
        /// <paramref name="startIndex"/> in the given <paramref name="array"/>.
        /// </summary>
        /// <param name="array">The byte array storing the raw data.</param>
        /// <param name="startIndex">The index of the first byte to convert from.</param>
        /// <returns>The converted value.</returns>
        public override Int16 ToInt16(byte[] array, int startIndex)
        {
            return (Int16)(array[startIndex]
                | array[startIndex + 1] << 8);
        }

        /// <summary>
        /// Returns an <see cref="Int32"/> instance converted from the bytes at the specific
        /// <paramref name="startIndex"/> in the given <paramref name="array"/>.
        /// </summary>
        /// <param name="array">The byte array storing the raw data.</param>
        /// <param name="startIndex">The index of the first byte to convert from.</param>
        /// <returns>The converted value.</returns>
        public override Int32 ToInt32(byte[] array, int startIndex)
        {
            return array[startIndex]
                | array[startIndex + 1] << 8
                | array[startIndex + 2] << 16
                | array[startIndex + 3] << 24;
        }

        /// <summary>
        /// Returns an <see cref="Int64"/> instance converted from the bytes at the specific
        /// <paramref name="startIndex"/> in the given <paramref name="array"/>.
        /// </summary>
        /// <param name="array">The byte array storing the raw data.</param>
        /// <param name="startIndex">The index of the first byte to convert from.</param>
        /// <returns>The converted value.</returns>
        public override Int64 ToInt64(byte[] array, int startIndex)
        {
            return array[startIndex]
                | (long)array[startIndex + 1] << 8
                | (long)array[startIndex + 2] << 16
                | (long)array[startIndex + 3] << 24
                | (long)array[startIndex + 4] << 32
                | (long)array[startIndex + 5] << 40
                | (long)array[startIndex + 6] << 48
                | (long)array[startIndex + 7] << 56;
        }
        
        /// <summary>
        /// Returns an <see cref="UInt16"/> instance converted from the bytes at the specific
        /// <paramref name="startIndex"/> in the given <paramref name="array"/>.
        /// </summary>
        /// <param name="array">The byte array storing the raw data.</param>
        /// <param name="startIndex">The index of the first byte to convert from.</param>
        /// <returns>The converted value.</returns>
        public override UInt16 ToUInt16(byte[] array, int startIndex)
        {
            return (UInt16)(array[startIndex]
                | array[startIndex + 1] << 8);
        }

        /// <summary>
        /// Returns an <see cref="UInt32"/> instance converted from the bytes at the specific
        /// <paramref name="startIndex"/> in the given <paramref name="array"/>.
        /// </summary>
        /// <param name="array">The byte array storing the raw data.</param>
        /// <param name="startIndex">The index of the first byte to convert from.</param>
        /// <returns>The converted value.</returns>
        public override UInt32 ToUInt32(byte[] array, int startIndex)
        {
            return (UInt32)(array[startIndex]
                | array[startIndex + 1] << 8
                | array[startIndex + 2] << 16
                | array[startIndex + 3] << 24);
        }

        /// <summary>
        /// Returns an <see cref="UInt64"/> instance converted from the bytes at the specific
        /// <paramref name="startIndex"/> in the given <paramref name="array"/>.
        /// </summary>
        /// <param name="array">The byte array storing the raw data.</param>
        /// <param name="startIndex">The index of the first byte to convert from.</param>
        /// <returns>The converted value.</returns>
        public override UInt64 ToUInt64(byte[] array, int startIndex)
        {
            return array[startIndex]
                | (ulong)array[startIndex + 1] << 8
                | (ulong)array[startIndex + 2] << 16
                | (ulong)array[startIndex + 3] << 24
                | (ulong)array[startIndex + 4] << 32
                | (ulong)array[startIndex + 5] << 40
                | (ulong)array[startIndex + 6] << 48
                | (ulong)array[startIndex + 7] << 56;
        }
    }
}

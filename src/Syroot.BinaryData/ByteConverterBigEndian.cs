using System;

namespace Syroot.BinaryData
{
    /// <summary>
    /// Represents a <see cref="BitConverter"/> which handles big endianness.
    /// </summary>
    public class ByteConverterBigEndian : ByteConverter
    {
        // ---- CONSTRUCTORS & DESTRUCTOR ------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="ByteConverterBigEndian"/> class.
        /// </summary>
        internal ByteConverterBigEndian()
            : base(ByteOrder.BigEndian)
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
            array[startIndex] = (byte)(value >> 8);
            array[startIndex + 1] = (byte)value;
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
            array[startIndex] = (byte)(value >> 24);
            array[startIndex + 1] = (byte)(value >> 16);
            array[startIndex + 2] = (byte)(value >> 8);
            array[startIndex + 3] = (byte)value;
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
            array[startIndex] = (byte)(value >> 56);
            array[startIndex + 1] = (byte)(value >> 48);
            array[startIndex + 2] = (byte)(value >> 40);
            array[startIndex + 3] = (byte)(value >> 32);
            array[startIndex + 4] = (byte)(value >> 24);
            array[startIndex + 5] = (byte)(value >> 16);
            array[startIndex + 6] = (byte)(value >> 8);
            array[startIndex + 7] = (byte)value;
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
            array[startIndex] = (byte)(value >> 8);
            array[startIndex + 1] = (byte)value;
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
            array[startIndex] = (byte)(value >> 24);
            array[startIndex + 1] = (byte)(value >> 16);
            array[startIndex + 2] = (byte)(value >> 8);
            array[startIndex + 3] = (byte)value;
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
            array[startIndex] = (byte)(value >> 56);
            array[startIndex + 1] = (byte)(value >> 48);
            array[startIndex + 2] = (byte)(value >> 40);
            array[startIndex + 3] = (byte)(value >> 32);
            array[startIndex + 4] = (byte)(value >> 24);
            array[startIndex + 5] = (byte)(value >> 16);
            array[startIndex + 6] = (byte)(value >> 8);
            array[startIndex + 7] = (byte)value;
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
            return (Int16)(array[startIndex] << 8
                | array[startIndex]);
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
            return array[startIndex] << 24
                | array[startIndex + 1] << 16
                | array[startIndex + 2] << 8
                | array[startIndex + 3];
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
            return (long)array[startIndex] << 56
                | (long)array[startIndex + 1] << 48
                | (long)array[startIndex + 2] << 40
                | (long)array[startIndex + 3] << 32
                | (long)array[startIndex + 4] << 24
                | (long)array[startIndex + 5] << 16
                | (long)array[startIndex + 6] << 8
                | array[startIndex + 7];
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
            return (UInt16)(array[startIndex] << 8
                | array[startIndex]);
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
            return (UInt32)(array[startIndex] << 24
                | array[startIndex + 1] << 16
                | array[startIndex + 2] << 8
                | array[startIndex + 3]);
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
            return (ulong)array[startIndex] << 56
                | (ulong)array[startIndex + 1] << 48
                | (ulong)array[startIndex + 2] << 40
                | (ulong)array[startIndex + 3] << 32
                | (ulong)array[startIndex + 4] << 24
                | (ulong)array[startIndex + 5] << 16
                | (ulong)array[startIndex + 6] << 8
                | array[startIndex + 7];
        }
    }
}

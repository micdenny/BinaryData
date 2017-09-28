using System;

namespace Syroot.BinaryData
{
    /// <summary>
    /// Represents a <see cref="BitConverter"/> which handles a specific endianness.
    /// </summary>
    public abstract class ByteConverter
    {
        // ---- CONSTRUCTORS & DESTRUCTOR ------------------------------------------------------------------------------

        /// <summary>
        /// Initializes static members of the <see cref="ByteConverter"/> class.
        /// </summary>
        static ByteConverter()
        {
            LittleEndian = new ByteConverterLittleEndian();
            BigEndian = new ByteConverterBigEndian();
            System = BitConverter.IsLittleEndian ? LittleEndian : BigEndian;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ByteConverter"/> class.
        /// </summary>
        internal ByteConverter(ByteOrder byteOrder)
        {
            ByteOrder = byteOrder;
        }

        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        /// <summary>
        /// Gets a <see cref="ByteConverter"/> instance converting data stored in little endian byte order.
        /// </summary>
        public static ByteConverter LittleEndian { get; }

        /// <summary>
        /// Gets a <see cref="ByteConverter"/> instance converting data stored in big endian byte order.
        /// </summary>
        public static ByteConverter BigEndian { get; }

        /// <summary>
        /// Gets a <see cref="ByteConverter"/> instance converting data stored in the byte order of the system
        /// executing the assembly.
        /// </summary>
        public static ByteConverter System { get; }

        /// <summary>
        /// Gets the <see cref="ByteOrder"/> in which data is stored as converted by this instance.
        /// </summary>
        public ByteOrder ByteOrder { get; }

        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        /// <summary>
        /// Returns a <see cref="ByteConverter"/> for the given <paramref name="byteOrder"/>.
        /// </summary>
        /// <param name="byteOrder">The <see cref="ByteOrder"/> to retrieve a converter for.</param>
        /// <returns>The corresponding <see cref="ByteConverter"/> instance.</returns>
        public static ByteConverter GetConverter(ByteOrder byteOrder)
        {
            switch (byteOrder)
            {
                case ByteOrder.BigEndian:
                    return BigEndian;
                case ByteOrder.LittleEndian:
                    return LittleEndian;
                case ByteOrder.System:
                    return System;
                default:
                    throw new ArgumentException($"Invalid {nameof(ByteOrder)}.", nameof(byteOrder));
            }
        }

        /// <summary>
        /// Returns the specified <see cref="Decimal"/> value as an array of bytes.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>An array of bytes representing the value.</returns>
        public byte[] GetBytes(Decimal value)
        {
            byte[] array = new byte[sizeof(Decimal)];
            GetBytes(value, array, 0);
            return array;
        }

        /// <summary>
        /// Returns the specified <see cref="Double"/> value as an array of bytes.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>An array of bytes representing the value.</returns>
        public byte[] GetBytes(Double value)
        {
            byte[] array = new byte[sizeof(Double)];
            GetBytes(value, array, 0);
            return array;
        }

        /// <summary>
        /// Returns the specified <see cref="Int16"/> value as an array of bytes.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>An array of bytes representing the value.</returns>
        public byte[] GetBytes(Int16 value)
        {
            byte[] array = new byte[sizeof(Int16)];
            GetBytes(value, array, 0);
            return array;
        }

        /// <summary>
        /// Returns the specified <see cref="Int32"/> value as an array of bytes.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>An array of bytes representing the value.</returns>
        public byte[] GetBytes(Int32 value)
        {
            byte[] array = new byte[sizeof(Int32)];
            GetBytes(value, array, 0);
            return array;
        }

        /// <summary>
        /// Returns the specified <see cref="Int64"/> value as an array of bytes.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>An array of bytes representing the value.</returns>
        public byte[] GetBytes(Int64 value)
        {
            byte[] array = new byte[sizeof(Int64)];
            GetBytes(value, array, 0);
            return array;
        }

        /// <summary>
        /// Returns the specified <see cref="Single"/> value as an array of bytes.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>An array of bytes representing the value.</returns>
        public byte[] GetBytes(Single value)
        {
            byte[] array = new byte[sizeof(Single)];
            GetBytes(value, array, 0);
            return array;
        }

        /// <summary>
        /// Returns the specified <see cref="UInt16"/> value as an array of bytes.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>An array of bytes representing the value.</returns>
        public byte[] GetBytes(UInt16 value)
        {
            byte[] array = new byte[sizeof(UInt16)];
            GetBytes(value, array, 0);
            return array;
        }

        /// <summary>
        /// Returns the specified <see cref="UInt32"/> value as an array of bytes.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>An array of bytes representing the value.</returns>
        public byte[] GetBytes(UInt32 value)
        {
            byte[] array = new byte[sizeof(UInt32)];
            GetBytes(value, array, 0);
            return array;
        }

        /// <summary>
        /// Returns the specified <see cref="UInt64"/> value as an array of bytes.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>An array of bytes representing the value.</returns>
        public byte[] GetBytes(UInt64 value)
        {
            byte[] array = new byte[sizeof(UInt64)];
            GetBytes(value, array, 0);
            return array;
        }
        
        /// <summary>
        /// Stores the specified <see cref="Decimal"/> value as bytes in the given <paramref name="array"/>, starting at
        /// the provided <paramref name="startIndex"/>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="array">The byte array to store the value in.</param>
        /// <param name="startIndex">The index at which to start storing the value.</param>
        public void GetBytes(Decimal value, byte[] array, int startIndex)
        {
            int[] values = Decimal.GetBits(value);
            for (int i = 0; i < values.Length; i++)
            {
                GetBytes(values[i], array, startIndex + i * sizeof(int));
            }
        }

        /// <summary>
        /// Stores the specified <see cref="Double"/> value as bytes in the given <paramref name="array"/>, starting at
        /// the provided <paramref name="startIndex"/>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="array">The byte array to store the value in.</param>
        /// <param name="startIndex">The index at which to start storing the value.</param>
        public void GetBytes(Double value, byte[] array, int startIndex)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            if (ByteOrder != System.ByteOrder)
            {
                Array.Reverse(buffer);
            }
            Buffer.BlockCopy(buffer, 0, array, startIndex, sizeof(Double));
        }

        /// <summary>
        /// Stores the specified <see cref="Int16"/> value as bytes in the given <paramref name="array"/>, starting at
        /// the provided <paramref name="startIndex"/>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="array">The byte array to store the value in.</param>
        /// <param name="startIndex">The index at which to start storing the value.</param>
        public abstract void GetBytes(Int16 value, byte[] array, int startIndex);

        /// <summary>
        /// Stores the specified <see cref="Int32"/> value as bytes in the given <paramref name="array"/>, starting at
        /// the provided <paramref name="startIndex"/>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="array">The byte array to store the value in.</param>
        /// <param name="startIndex">The index at which to start storing the value.</param>
        public abstract void GetBytes(Int32 value, byte[] array, int startIndex);

        /// <summary>
        /// Stores the specified <see cref="Int64"/> value as bytes in the given <paramref name="array"/>, starting at
        /// the provided <paramref name="startIndex"/>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="array">The byte array to store the value in.</param>
        /// <param name="startIndex">The index at which to start storing the value.</param>
        public abstract void GetBytes(Int64 value, byte[] array, int startIndex);

        /// <summary>
        /// Stores the specified <see cref="Single"/> value as bytes in the given <paramref name="array"/>, starting at
        /// the provided <paramref name="startIndex"/>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="array">The byte array to store the value in.</param>
        /// <param name="startIndex">The index at which to start storing the value.</param>
        public void GetBytes(Single value, byte[] array, int startIndex)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            if (ByteOrder != System.ByteOrder)
            {
                Array.Reverse(buffer);
            }
            Buffer.BlockCopy(buffer, 0, array, startIndex, sizeof(Single));
        }

        /// <summary>
        /// Stores the specified <see cref="UInt16"/> value as bytes in the given <paramref name="array"/>, starting at
        /// the provided <paramref name="startIndex"/>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="array">The byte array to store the value in.</param>
        /// <param name="startIndex">The index at which to start storing the value.</param>
        public abstract void GetBytes(UInt16 value, byte[] array, int startIndex);

        /// <summary>
        /// Stores the specified <see cref="UInt32"/> value as bytes in the given <paramref name="array"/>, starting at
        /// the provided <paramref name="startIndex"/>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="array">The byte array to store the value in.</param>
        /// <param name="startIndex">The index at which to start storing the value.</param>
        public abstract void GetBytes(UInt32 value, byte[] array, int startIndex);

        /// <summary>
        /// Stores the specified <see cref="UInt64"/> value as bytes in the given <paramref name="array"/>, starting at
        /// the provided <paramref name="startIndex"/>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="array">The byte array to store the value in.</param>
        /// <param name="startIndex">The index at which to start storing the value.</param>
        public abstract void GetBytes(UInt64 value, byte[] array, int startIndex);
        
        /// <summary>
        /// Returns an <see cref="Decimal"/> instance converted from the bytes in the given <paramref name="array"/>.
        /// </summary>
        /// <param name="array">The byte array storing the raw data.</param>
        /// <returns>The converted value.</returns>
        public Decimal ToDecimal(byte[] array)
        {
            return ToDecimal(array, 0);
        }

        /// <summary>
        /// Returns an <see cref="Double"/> instance converted from the bytes in the given <paramref name="array"/>.
        /// </summary>
        /// <param name="array">The byte array storing the raw data.</param>
        /// <returns>The converted value.</returns>
        public Double ToDouble(byte[] array)
        {
            return ToDouble(array, 0);
        }

        /// <summary>
        /// Returns an <see cref="Int16"/> instance converted from the bytes in the given <paramref name="array"/>.
        /// </summary>
        /// <param name="array">The byte array storing the raw data.</param>
        /// <returns>The converted value.</returns>
        public Int16 ToInt16(byte[] array)
        {
            return ToInt16(array, 0);
        }

        /// <summary>
        /// Returns an <see cref="Int32"/> instance converted from the bytes in the given <paramref name="array"/>.
        /// </summary>
        /// <param name="array">The byte array storing the raw data.</param>
        /// <returns>The converted value.</returns>
        public Int32 ToInt32(byte[] array)
        {
            return ToInt32(array, 0);
        }

        /// <summary>
        /// Returns an <see cref="Int64"/> instance converted from the bytes in the given <paramref name="array"/>.
        /// </summary>
        /// <param name="array">The byte array storing the raw data.</param>
        /// <returns>The converted value.</returns>
        public Int64 ToInt64(byte[] array)
        {
            return ToInt64(array, 0);
        }

        /// <summary>
        /// Returns an <see cref="Single"/> instance converted from the bytes in the given <paramref name="array"/>.
        /// </summary>
        /// <param name="array">The byte array storing the raw data.</param>
        /// <returns>The converted value.</returns>
        public Single ToSingle(byte[] array)
        {
            return ToSingle(array, 0);
        }

        /// <summary>
        /// Returns an <see cref="UInt16"/> instance converted from the bytes in the given <paramref name="array"/>.
        /// </summary>
        /// <param name="array">The byte array storing the raw data.</param>
        /// <returns>The converted value.</returns>
        public UInt16 ToUInt16(byte[] array)
        {
            return ToUInt16(array, 0);
        }

        /// <summary>
        /// Returns an <see cref="UInt32"/> instance converted from the bytes in the given <paramref name="array"/>.
        /// </summary>
        /// <param name="array">The byte array storing the raw data.</param>
        /// <returns>The converted value.</returns>
        public UInt32 ToUInt32(byte[] array)
        {
            return ToUInt32(array, 0);
        }

        /// <summary>
        /// Returns an <see cref="UInt64"/> instance converted from the bytes in the given <paramref name="array"/>.
        /// </summary>
        /// <param name="array">The byte array storing the raw data.</param>
        /// <returns>The converted value.</returns>
        public UInt64 ToUInt64(byte[] array)
        {
            return ToUInt64(array, 0);
        }
        
        /// <summary>
        /// Returns an <see cref="Decimal"/> instance converted from the bytes at the specific
        /// <paramref name="startIndex"/> in the given <paramref name="array"/>.
        /// </summary>
        /// <param name="array">The byte array storing the raw data.</param>
        /// <param name="startIndex">The index of the first byte to convert from.</param>
        /// <returns>The converted value.</returns>
        public Decimal ToDecimal(byte[] array, int startIndex)
        {
            int[] values = new int[4];
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = ToInt32(array, startIndex + i * sizeof(int));
            }
            return new Decimal(values);
        }

        /// <summary>
        /// Returns an <see cref="Double"/> instance converted from the bytes at the specific
        /// <paramref name="startIndex"/> in the given <paramref name="array"/>.
        /// </summary>
        /// <param name="array">The byte array storing the raw data.</param>
        /// <param name="startIndex">The index of the first byte to convert from.</param>
        /// <returns>The converted value.</returns>
        public Double ToDouble(byte[] array, int startIndex)
        {
            if (ByteOrder == System.ByteOrder)
            {
                return BitConverter.ToDouble(array, 0);
            }
            else
            {
                byte[] reversed = new byte[array.Length];
                for (int i = 0; i < array.Length; i++)
                {
                    reversed[reversed.Length - i - 1] = array[i];
                }
                return BitConverter.ToDouble(array, 0);
            }
        }

        /// <summary>
        /// Returns an <see cref="Int16"/> instance converted from the bytes at the specific
        /// <paramref name="startIndex"/> in the given <paramref name="array"/>.
        /// </summary>
        /// <param name="array">The byte array storing the raw data.</param>
        /// <param name="startIndex">The index of the first byte to convert from.</param>
        /// <returns>The converted value.</returns>
        public abstract Int16 ToInt16(byte[] array, int startIndex);

        /// <summary>
        /// Returns an <see cref="Int32"/> instance converted from the bytes at the specific
        /// <paramref name="startIndex"/> in the given <paramref name="array"/>.
        /// </summary>
        /// <param name="array">The byte array storing the raw data.</param>
        /// <param name="startIndex">The index of the first byte to convert from.</param>
        /// <returns>The converted value.</returns>
        public abstract Int32 ToInt32(byte[] array, int startIndex);

        /// <summary>
        /// Returns an <see cref="Int64"/> instance converted from the bytes at the specific
        /// <paramref name="startIndex"/> in the given <paramref name="array"/>.
        /// </summary>
        /// <param name="array">The byte array storing the raw data.</param>
        /// <param name="startIndex">The index of the first byte to convert from.</param>
        /// <returns>The converted value.</returns>
        public abstract Int64 ToInt64(byte[] array, int startIndex);

        /// <summary>
        /// Returns an <see cref="Single"/> instance converted from the bytes at the specific
        /// <paramref name="startIndex"/> in the given <paramref name="array"/>.
        /// </summary>
        /// <param name="array">The byte array storing the raw data.</param>
        /// <param name="startIndex">The index of the first byte to convert from.</param>
        /// <returns>The converted value.</returns>
        public Single ToSingle(byte[] array, int startIndex)
        {
            if (ByteOrder == System.ByteOrder)
            {
                return BitConverter.ToSingle(array, 0);
            }
            else
            {
                byte[] reversed = new byte[array.Length];
                for (int i = 0; i < array.Length; i++)
                {
                    reversed[reversed.Length - i - 1] = array[i];
                }
                return BitConverter.ToSingle(array, 0);
            }
        }

        /// <summary>
        /// Returns an <see cref="UInt16"/> instance converted from the bytes at the specific
        /// <paramref name="startIndex"/> in the given <paramref name="array"/>.
        /// </summary>
        /// <param name="array">The byte array storing the raw data.</param>
        /// <param name="startIndex">The index of the first byte to convert from.</param>
        /// <returns>The converted value.</returns>
        public abstract UInt16 ToUInt16(byte[] array, int startIndex);

        /// <summary>
        /// Returns an <see cref="UInt32"/> instance converted from the bytes at the specific
        /// <paramref name="startIndex"/> in the given <paramref name="array"/>.
        /// </summary>
        /// <param name="array">The byte array storing the raw data.</param>
        /// <param name="startIndex">The index of the first byte to convert from.</param>
        /// <returns>The converted value.</returns>
        public abstract UInt32 ToUInt32(byte[] array, int startIndex);

        /// <summary>
        /// Returns an <see cref="UInt64"/> instance converted from the bytes at the specific
        /// <paramref name="startIndex"/> in the given <paramref name="array"/>.
        /// </summary>
        /// <param name="array">The byte array storing the raw data.</param>
        /// <param name="startIndex">The index of the first byte to convert from.</param>
        /// <returns>The converted value.</returns>
        public abstract UInt64 ToUInt64(byte[] array, int startIndex);
    }
}

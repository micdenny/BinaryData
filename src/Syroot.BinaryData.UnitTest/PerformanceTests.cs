using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Syroot.BinaryData.UnitTest
{
    [TestClass]
    public class PerformanceTests
    {
        // ---- CONSTANTS ----------------------------------------------------------------------------------------------

        private const int _writeCount = 10_000_000;

        private static readonly Random _random = new Random();
        private static readonly byte[] _buffer = new byte[sizeof(Int32)];
        
        // ---- FIELDS -------------------------------------------------------------------------------------------------

        private static MemoryStream _stream = new MemoryStream();

        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        [TestInitialize]
        public void Initialize()
        {
            // Create a new stream to write test data into with the .NET default writer.
            _stream?.Dispose();
            _stream = new MemoryStream();
        }

        // ---- System Endianness ----

        [TestMethod]
        public void Writing_System_BinaryStream()
        {
            using (BinaryStream stream = new BinaryStream(_stream))
            {
                for (int i = 0; i < _writeCount; i++)
                    stream.Write(_random.Next(Int32.MaxValue));
            }
        }

        [TestMethod]
        public void Writing_System_BinaryWriter()
        {
            using (BinaryWriter writer = new BinaryWriter(_stream))
            {
                for (int i = 0; i < _writeCount; i++)
                    writer.Write(_random.Next(Int32.MaxValue));
            }
        }

        [TestMethod]
        public void Writing_System_StreamExtension()
        {
            for (int i = 0; i < _writeCount; i++)
                _stream.Write(_random.Next(Int32.MaxValue));
        }

        [TestMethod]
        public void Writing_System_StreamExtension_ExplicitConverter()
        {
            for (int i = 0; i < _writeCount; i++)
                _stream.Write(_random.Next(Int32.MaxValue), ByteConverter.System);
        }

        // ---- Non-System Endianness ----

        [TestMethod]
        public void Writing_NonSystem_BinaryStream()
        {
            using (BinaryStream stream = new BinaryStream(_stream, TestTools.ReverseByteConverter))
            {
                for (int i = 0; i < _writeCount; i++)
                    stream.Write(_random.Next(Int32.MaxValue));
            }
        }

        [TestMethod]
        public void Writing_NonSystem_BinaryStream_ExplicitConverter()
        {
            using (BinaryStream stream = new BinaryStream(_stream))
            {
                for (int i = 0; i < _writeCount; i++)
                    stream.Write(_random.Next(Int32.MaxValue), TestTools.ReverseByteConverter);
            }
        }

        [TestMethod]
        public void Writing_NonSystem_BinaryWriter_BitConverter()
        {
            using (BinaryWriter writer = new BinaryWriter(_stream))
            {
                for (int i = 0; i < _writeCount; i++)
                {
                    byte[] buffer = BitConverter.GetBytes(_random.Next(Int32.MaxValue));
                    Array.Reverse(buffer);
                    writer.Write(buffer);
                }
            }
        }

        [TestMethod]
        public void Writing_NonSystem_BinaryWriter_ByteConverter()
        {
            using (BinaryWriter writer = new BinaryWriter(_stream))
            {
                for (int i = 0; i < _writeCount; i++)
                {
                    TestTools.ReverseByteConverter.GetBytes(_random.Next(Int32.MaxValue), _buffer);
                    writer.Write(_buffer);
                }
            }
        }

        [TestMethod]
        public void Writing_NonSystem_StreamExtension()
        {
            for (int i = 0; i < _writeCount; i++)
                _stream.Write(_random.Next(Int32.MaxValue), TestTools.ReverseByteConverter);
        }
    }
}

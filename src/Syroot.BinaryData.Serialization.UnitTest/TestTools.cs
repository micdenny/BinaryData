using System;

namespace Syroot.BinaryData.Serialization.UnitTest
{
    internal static class TestTools
    {
        // ---- FIELDS -------------------------------------------------------------------------------------------------

        internal static readonly ByteConverter ReverseByteConverter
            = BitConverter.IsLittleEndian ? ByteConverter.Big : ByteConverter.Little;
    }
}

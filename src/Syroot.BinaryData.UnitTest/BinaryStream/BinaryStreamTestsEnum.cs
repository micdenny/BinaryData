using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Syroot.BinaryData.UnitTest
{
    [TestClass]
    public class BinaryStreamTestsEnum
    {
        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        [TestMethod]
        public void ReadWriteEnum()
        {
            Chicken[] values = new Chicken[] 
            {
                Chicken.RhodeIslandRed,
                Chicken.Silkie,
                Chicken.JerseyGiant,
                Chicken.Leghorn,
                Chicken.GoldenPolish,
                Chicken.Brahma,
                Chicken.PlymouthRock,
                0
            };

            ByteConverter[] endianness = new ByteConverter[] 
            {
                ByteConverter.Big,
                ByteConverter.Little,
                ByteConverter.System
            };

            using (BinaryStream binaryStreamDefault = new BinaryStream(new MemoryStream()))
            using (BinaryStream binaryStreamLittle = new BinaryStream(new MemoryStream(), ByteConverter.Little))
            using (BinaryStream binaryStreamBig = new BinaryStream(new MemoryStream(), ByteConverter.Big))
            using (BinaryStream binaryStreamSystem = new BinaryStream(new MemoryStream(), ByteConverter.System))
            {
                // Collect streams with different initializations to test.
                BinaryStream[] binaryStreams = new BinaryStream[] { binaryStreamDefault, binaryStreamLittle, binaryStreamBig, binaryStreamSystem };

                foreach (BinaryStream binaryStream in binaryStreams)
                {

                    // Prepare test data.

                    foreach (Chicken value in values)
                        binaryStream.WriteEnum(value, true);

                    foreach (ByteConverter endian in endianness)
                        foreach (Chicken value in values)
                            binaryStream.WriteEnum(value, true, endian);

                    // Read test data.
                    binaryStream.Position = 0;

                    foreach (Chicken value in values)
                        Assert.AreEqual(value, binaryStream.ReadEnum<Chicken>(true));

                    foreach (ByteConverter endian in endianness)
                        foreach (Chicken value in values)
                            Assert.AreEqual(value, binaryStream.ReadEnum<Chicken>(true, endian));

                    // Read test data as integers.

                    binaryStream.Position = 0;

                    foreach (Chicken value in values)
                        Assert.AreEqual(value, (Chicken)binaryStream.ReadUInt16());

                    foreach (ByteConverter endian in endianness)
                        foreach (Chicken value in values)
                            Assert.AreEqual(value, (Chicken)binaryStream.ReadUInt16(endian));

                    // Read test data all at once. 

                    binaryStream.Position = 0;

                    CollectionAssert.AreEqual(values, binaryStream.ReadEnums<Chicken>(values.Length, true));

                    foreach (ByteConverter endian in endianness)
                        CollectionAssert.AreEqual(values, binaryStream.ReadEnums<Chicken>(values.Length, true, endian));
                }
            }
        }


        // ---- CLASSES, STRUCTS & ENUMS -------------------------------------------------------------------------------

        private enum Chicken : UInt16
        {
            RhodeIslandRed,
            Silkie = 1,
            JerseyGiant = 7,
            Leghorn = 10,
            GoldenPolish = 147,
            Brahma,
            PlymouthRock = UInt16.MaxValue
        }
    }
}

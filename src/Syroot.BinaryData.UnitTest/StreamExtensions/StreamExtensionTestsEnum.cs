using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Syroot.BinaryData.UnitTest
{
    [TestClass]
    public class StreamExtensionTestsEnum
    {
        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        [TestMethod]
        public void ReadWriteEnum()
        {
            Fruit[] values = new Fruit[] { 0/*Apple*/, Fruit.Banana, Fruit.Pineapple, Fruit.Cherry };
            using (MemoryStream stream = new MemoryStream())
            {
                // Prepare test data.
                foreach (Fruit value in values)
                    stream.WriteEnum(value, true);
                foreach (Fruit value in values)
                    stream.WriteEnum(value, true, TestTools.ReverseByteConverter);

                // Read test data.
                stream.Position = 0;
                foreach (Fruit value in values)
                    Assert.AreEqual(value, stream.ReadEnum<Fruit>(true));
                foreach (Fruit value in values)
                    Assert.AreEqual(value, stream.ReadEnum<Fruit>(true, TestTools.ReverseByteConverter));

                // Read test data as integers.
                stream.Position = 0;
                foreach (Fruit value in values)
                    Assert.AreEqual(value, (Fruit)stream.ReadUInt16());
                foreach (Fruit value in values)
                    Assert.AreEqual(value, (Fruit)stream.ReadUInt16(TestTools.ReverseByteConverter));

                // Read test data all at once. 
                stream.Position = 0;
                CollectionAssert.AreEqual(values, stream.ReadEnums<Fruit>(values.Length, true));
                CollectionAssert.AreEqual(values, stream.ReadEnums<Fruit>(values.Length, true, TestTools.ReverseByteConverter));
            }
        }

        [TestMethod]
        public void ReadWriteEnumFailStrict()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                // Prepare test data.
                stream.Write((UInt16)123);
                stream.Write((UInt16)123);

                // Read test data.
                stream.Position = 0;
                Assert.AreEqual((FruitState)123, stream.ReadEnum<FruitState>(false));
                Assert.ThrowsException<InvalidDataException>(() => stream.ReadEnum<FruitState>(true));
            }
        }

        [TestMethod]
        public void ReadWriteEnumFlags()
        {
            FruitState[] values = new FruitState[]
            {
                FruitState.Ripe | FruitState.HasWorm,
                FruitState.Rotten | FruitState.IsRed | FruitState.HasWorm | FruitState.Ripe,
                FruitState.Ripe,
                0
            };
            using (MemoryStream stream = new MemoryStream())
            {
                // Prepare test data.
                foreach (FruitState value in values)
                    stream.WriteEnum(value, true);
                foreach (FruitState value in values)
                    stream.WriteEnum(value, true, TestTools.ReverseByteConverter);

                // Read test data.
                stream.Position = 0;
                foreach (FruitState value in values)
                    Assert.AreEqual(value, stream.ReadEnum<FruitState>(true));
                foreach (FruitState value in values)
                    Assert.AreEqual(value, stream.ReadEnum<FruitState>(true, TestTools.ReverseByteConverter));

                // Read test data all at once. 
                stream.Position = 0;
                CollectionAssert.AreEqual(values, stream.ReadEnums<FruitState>(values.Length, true));
                CollectionAssert.AreEqual(values, stream.ReadEnums<FruitState>(values.Length, true, TestTools.ReverseByteConverter));
            }
        }

        // ---- CLASSES, STRUCTS & ENUMS -------------------------------------------------------------------------------

        private enum Fruit : UInt16
        {
            Apple,
            Banana = 5,
            Pineapple = 8,
            Strawberry,
            Cherry = UInt16.MaxValue
        }

        [Flags]
        private enum FruitState : Int16
        {
            Ripe = 1 << 0,
            HasWorm = 1 << 1,
            IsRed = 1 << 2,
            Rotten = 1 << 3
        }
    }
}

using System.IO;

namespace Syroot.BinaryData.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            TestObject bla = new TestObject() { X = Shit.sdkjasld };

            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryDataWriter writer = new BinaryDataWriter(stream, true))
                {
                    writer.WriteObject(bla);
                }
                
                stream.Position = 0;
                using (BinaryDataReader reader = new BinaryDataReader(stream, true))
                {
                    bla = reader.ReadObject<TestObject>();
                }
            }
        }
    }

    class TestObjectBase
    {
        public int w;
    }
    
    class TestObject
    {
        public Shit X { get; set; }
    }

    enum Shit
    {
        Bla = 0,
        sdkjasld = 1
    }
}
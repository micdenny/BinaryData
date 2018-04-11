using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Syroot.BinaryData.Serialization.UnitTest
{
    [TestClass]
    public class StreamExtensionsTestsObject
    {

    }

    [DataOffsetStart(Origin.Set, 10)]
    [DataClass(Explicit = true, Inherit = false)]
    public class TestBaseClass
    {

    }

    [DataOffsetStart(Origin.Add, 2)]
    [DataClass(Explicit = false, Inherit = true)]
    public class TestClass : TestBaseClass
    {
    }
}

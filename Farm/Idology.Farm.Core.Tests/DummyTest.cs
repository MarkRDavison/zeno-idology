namespace Idology.Farm.Core.Tests;

[TestClass]
public class DummyTest
{
    [TestMethod]
    public void Dummy_Test()
    {
#pragma warning disable MSTEST0032 // Assertion condition is always true
        Assert.AreEqual(1, 1);
#pragma warning restore MSTEST0032 // Assertion condition is always true
    }
}

using System.Reflection;

namespace MockingWithRespect.Tests
{
    [TestFixture]
    public class MockBaseTests
    {
        [Test]
        public void MockBaseTest_NoImplementationDefined()
        {
            var methodsImplementation = new Dictionary<MethodBase, Delegate>();
            var mock = new MockExample(methodsImplementation);

            Assert.AreEqual(default(int), mock.GetUsersCount());
        }

        [Test]
        public void MockBaseTest_ImplementationDefined()
        {
            var methodsImplementation = new Dictionary<MethodBase, Delegate>();
            var mock = new MockExample(methodsImplementation);
            
            Assert.AreEqual(default(int), mock.GetUsersCount());

            const int mockedResult = 15;
            var methodInfo = typeof(IFancyService).GetMethod(nameof(IFancyService.GetUsersCount));
            methodsImplementation.Add(methodInfo, () => mockedResult);

            Assert.AreEqual(mockedResult, mock.GetUsersCount());
        }
    }
}
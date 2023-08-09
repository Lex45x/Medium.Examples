namespace MockingWithRespect.Tests;

[TestFixture]
public class MockMeTests
{
    [Test]
    public void MockingTest()
    {
        var mockMe = new MockMe<IFancyService>();

        Assert.AreEqual(default(int), mockMe.Instance.GetUsersCount());

        mockMe.Verify(service => service.GetUsersCount());

        mockMe.Setup(service => service.GetUsersCount(), () => 20);

        Assert.AreEqual(20, mockMe.Instance.GetUsersCount());

        mockMe.Verify(service => service.GetUsersCount(), 2);
    }

    [Test]
    public void MoreComplexMockingTest()
    {
        var mockMe = new MockMe<IMoreComplexService>();

        Assert.AreEqual(null, mockMe.Instance.CreateFancyService());

        mockMe.Verify(service => service.CreateFancyService());

        var anotherMock = new MockMe<IFancyService>().Instance;

        mockMe.Setup(service => service.CreateFancyService(), () => anotherMock);

        Assert.AreEqual(anotherMock, mockMe.Instance.CreateFancyService());

        mockMe.Verify(service => service.CreateFancyService(), 2);
    }

}
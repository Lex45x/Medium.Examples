namespace MockingWithRespect;

public static class UsageExample
{
    public static void Example()
    {
        var mockMe = new MockMe<IFancyService>();
        mockMe.Setup(service => service.GetUsersCount(), () => 15);
        var usersCount = mockMe.Instance.GetUsersCount(); //this one should be 15
        mockMe.Verify(service => service.GetUsersCount(), 1);
    }
}
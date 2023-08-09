using System.Reflection;

namespace MockingWithRespect;

public class MockExample : MockBase, IFancyService
{
    public MockExample(Dictionary<MethodBase, Delegate> methodsImplementation) : base(methodsImplementation)
    {
    }

    public T GetUsersCount<T>() // this is implementation of our service
    {
        var methodInfo = typeof(IFancyService).GetMethod("GetUsersCount");

        if (IsImplemented(methodInfo, out var implementation))
        {
            return (T)implementation.DynamicInvoke(new object[5]);
        }

        return default;
    }

    public int GetUsersCount() // this is implementation of our service
    {
        var methodInfo = typeof(IFancyService).GetMethod("GetUsersCount");

        if (IsImplemented(methodInfo, out var implementation))
        {
            return (int)implementation.DynamicInvoke(new object[5]);
        }

        return default;
    }
}
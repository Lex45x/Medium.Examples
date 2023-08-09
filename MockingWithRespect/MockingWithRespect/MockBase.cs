using System.Reflection;

namespace MockingWithRespect;

public abstract class MockBase
{
    private readonly Dictionary<MethodBase, Delegate> methodsImplementation; //a thing we are configuring
    private readonly Dictionary<MethodBase, int> methodCalls = new(); //tracks amount of method calls

    protected MockBase(Dictionary<MethodBase, Delegate> methodsImplementation)
    {
        this.methodsImplementation = methodsImplementation;
    }

    /// <summary>
    /// This method increases executions counter and provides implementation for a given <paramref name="method"/>
    /// </summary>
    /// <param name="method"></param>
    /// <param name="implementation"></param>
    /// <returns></returns>
    protected bool IsImplemented(MethodBase method, out Delegate implementation)
    {
        if (methodCalls.ContainsKey(method))
        {
            methodCalls[method] += 1;
        }
        else
        {
            methodCalls[method] = 1;
        }

        return methodsImplementation.TryGetValue(method, out implementation);
    }

    public int GetCallsCount(MethodBase method)
    {
        return methodCalls.TryGetValue(method, out var callsCount) ? callsCount : 0;
    }
}
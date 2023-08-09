using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Cryptography;

namespace MockingWithRespect;

public class MockMe<TType> where TType : class
{
    private TypeBuilder typeBuilder;
    private const string AssemblyName = "MockingWithRespect.Mocks";

    private static readonly AssemblyBuilder MocksAssembly =
        AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(AssemblyName), AssemblyBuilderAccess.Run);

    private static readonly ModuleBuilder MocksModule = MocksAssembly.DefineDynamicModule(AssemblyName);

    private Dictionary<MethodBase, Delegate> implementations = new();

    public MockMe()
    {
        if (!typeof(TType).IsInterface)
        {
            throw new InvalidOperationException($"{typeof(TType)} is expected to be an Interface!");
        }

        typeBuilder = MocksModule.DefineType($"MockMe{Guid.NewGuid().ToString().Replace('-', '0')}",
            TypeAttributes.Class, typeof(MockBase));
        typeBuilder.AddInterfaceImplementation(typeof(TType));

        CreateConstructor();
        ImplementInterface();

        Instance = (TType)Activator.CreateInstance(typeBuilder.CreateType(), implementations);
    }

    private void CreateConstructor()
    {
        var constructorInfo = typeof(MockBase).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance).First();

        var constructorParameters = constructorInfo.GetParameters().Select(info => info.ParameterType).ToArray();

        var constructor = typeBuilder.DefineConstructor(MethodAttributes.Public
                                                        | MethodAttributes.SpecialName
                                                        | MethodAttributes.RTSpecialName,
            CallingConventions.Standard, constructorParameters);

        var generator = constructor.GetILGenerator();

        for (var i = 0; i < constructorParameters.Length + 1; i++)
        {
            generator.Emit(OpCodes.Ldarg, i);
        }

        generator.Emit(OpCodes.Call, constructorInfo); //call to base() constructor
        generator.Emit(OpCodes.Ret);
    }

    private void ImplementInterface()
    {
        var methods = typeof(TType).GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod);

        var isImplementedMethod =
            typeof(MockBase).GetMethod("IsImplemented", BindingFlags.NonPublic | BindingFlags.Instance);

        var dynamicInvoke =
            typeof(Delegate).GetMethod(nameof(Delegate.DynamicInvoke), new[]{ typeof(object[]) });

        var getMethodFromHandle = typeof(MethodBase).GetMethod("GetMethodFromHandle", new[] { typeof(RuntimeMethodHandle) });

        var arrayEmptyCall = typeof(Array)
            .GetMethod("Empty", BindingFlags.Static | BindingFlags.Public)
            .MakeGenericMethod(typeof(object));

        foreach (var interfaceMethod in methods)
        {
            var parameters = interfaceMethod.GetParameters().Select(info => info.ParameterType).ToArray();
            var methodBuilder = typeBuilder.DefineMethod(interfaceMethod.Name,
                MethodAttributes.Public
                | MethodAttributes.Virtual
                | MethodAttributes.HideBySig
                | MethodAttributes.NewSlot
                | MethodAttributes.Final,
                interfaceMethod.ReturnType, parameters);
            
            var ilGenerator = methodBuilder.GetILGenerator();

            var local0 = ilGenerator.DeclareLocal(typeof(MethodBase)); //will contain current interface method
            var local1 = ilGenerator.DeclareLocal(typeof(Delegate)); //will contain current interface method
            var local2 = ilGenerator.DeclareLocal(interfaceMethod.ReturnType); // will contain default value for our return type.
            
            ilGenerator.Emit(OpCodes.Ldtoken, interfaceMethod);
            ilGenerator.Emit(OpCodes.Call, getMethodFromHandle);
            ilGenerator.Emit(OpCodes.Stloc_0);

            var fallbackWithDefault = ilGenerator.DefineLabel();

            ilGenerator.Emit(OpCodes.Ldarg_0); //this
            ilGenerator.Emit(OpCodes.Ldloc_0); //interface method
            ilGenerator.Emit(OpCodes.Ldloca_S, local1); //interface method local variable
            ilGenerator.Emit(OpCodes.Call, isImplementedMethod);
            ilGenerator.Emit(OpCodes.Brfalse_S, fallbackWithDefault);

            ilGenerator.Emit(OpCodes.Ldloc_1); //delegate
            ilGenerator.Emit(OpCodes.Call, arrayEmptyCall); //get empty array of objects
            ilGenerator.Emit(OpCodes.Callvirt, dynamicInvoke); //call dynamic invoke
            ilGenerator.Emit(OpCodes.Unbox_Any, interfaceMethod.ReturnType); //cast object to the type we need
            ilGenerator.Emit(OpCodes.Ret);

            //somewhere here is default fallback
            ilGenerator.MarkLabel(fallbackWithDefault);
            ilGenerator.Emit(OpCodes.Ldloca_S, local2);
            ilGenerator.Emit(OpCodes.Initobj, interfaceMethod.ReturnType);
            ilGenerator.Emit(OpCodes.Ldloc_2);
            ilGenerator.Emit(OpCodes.Ret); 
        }
    }

    /// <summary>
    /// Actual instance of the Mocked <typeparamref name="TType"/>
    /// </summary>
    public TType Instance { get; }

    public void Verify(Expression<Action<TType>> methodCall, int count = 1)
    {
        if (methodCall.Body is not MethodCallExpression methodCallExpression)
        {
            throw new InvalidOperationException("Only methods can be verified!");
        }

        var methodInfo = methodCallExpression.Method;

        var mockBase = Instance as MockBase;

        var callsCount = mockBase!.GetCallsCount(methodInfo);

        if (callsCount != count)
        {
            throw new InvalidOperationException($"Mock of type {typeof(TType)} expected to have {count} calls of the method {methodInfo} but got {callsCount}");
        }
    }

    public void Setup<TResult>(Expression<Func<TType, TResult>> methodCall,
        Expression<Func<TResult>> implementation)
    {
        if (methodCall.Body is not MethodCallExpression methodCallExpression)
        {
            throw new InvalidOperationException("Only methods are supported in the Setup!");
        }

        var methodInfo = methodCallExpression.Method;

        implementations[methodInfo] = implementation.Compile();
    }
}
using System;
using System.Collections.Generic;

public static class ServiceLocator
{
    //create a dictionary which can take in services like SFX or Item Manager
    private static Dictionary<Type, object> services = new Dictionary<Type, object> ();

    //
    public static void Register<T>(T service) where T : class {
        services[ typeof ( T ) ] = service;
    }

    public static T Get<T>() where T : class
    {
        if (services.TryGetValue( typeof ( T ), out object service )) {
            return service as T;
        }
        
        throw new Exception( $"Service{typeof( T ).Name} not registered. " +
        "Make sure all services are registered before accessing through the service locator." );
    }

    public static void Unregister<T>() where T : class
    {
        services.Remove( typeof ( T ) );
    }
}

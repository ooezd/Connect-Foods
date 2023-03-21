using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Locator
{
    private static readonly Dictionary<Type, object> Services = new Dictionary<Type, object>();

    public static void Register<T>(T service)
    {
        var serviceType = typeof(T);

        if (IsRegistered(serviceType))
        {
            Debug.LogWarning($"{serviceType} is already registered");
        }

        Services[typeof(T)] = service;
    }

    public static T Resolve<T>()
    {
        var serviceType = typeof(T);

        if (IsRegistered(serviceType))
        {
            return (T)Services[serviceType];
        }

        Debug.LogError($"{serviceType.Name} hasn't been registered!");
        return default(T);
    }

    public static bool IsRegistered(Type t)
    {
        return Services.ContainsKey(t);
    }
}

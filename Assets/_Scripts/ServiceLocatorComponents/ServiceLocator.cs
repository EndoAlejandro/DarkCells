using System;
using System.Collections.Generic;
using UnityEngine;

namespace DarkHavoc.ServiceLocatorComponents
{
    public class ServiceLocator
    {
        private static ServiceLocator Instance => _instance ??= new ServiceLocator();
        private static ServiceLocator _instance;
        private readonly Dictionary<Type, object> _services;

        private ServiceLocator() => _services = new Dictionary<Type, object>();

        public static bool TryToRegisterService<T>(T service)
        {
            Type type = service.GetType();
            if (!Instance._services.TryAdd(type, service)) return false;
            return true;
        }

        public static void RemoveService<T>(T service)
        {
            var type = service.GetType();
            if (!Instance._services.ContainsKey(type)) return;
            Instance._services.Remove(type);
        }

        public static T GetService<T>() where T : class
        {
            var type = typeof(T);
            if (Instance._services.TryGetValue(type, out var service)) return (T)service;
            
            Debug.LogError($"{type} service not found.");
            return null;
        }
    }
}
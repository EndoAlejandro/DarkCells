using System;
using System.Collections.Generic;

namespace DarkHavoc.ServiceLocatorComponents
{
    public class ServiceLocator
    {
        private static ServiceLocator Instance => _instance ??= new ServiceLocator();
        private static ServiceLocator _instance;
        private readonly Dictionary<Type, object> _services = new();

        public static bool TryToRegisterService<T>(T service)
        {
            Type type = service.GetType();
            if (!Instance._services.TryAdd(type, service)) return false;
            // Debug.Log($"Register {type} Service.");
            return true;
        }

        public static void RemoveService<T>(T service)
        {
            var type = service.GetType();
            if (!Instance._services.ContainsKey(type)) return;
            // Debug.Log($"Remove {type} Service.");
            Instance._services.Remove(type);
        }

        public static T GetService<T>() where T : class
        {
            var type = typeof(T);
            if (!Instance._services.TryGetValue(type, out var service))
            {
                // Debug.LogWarning($"{type} Not found.");
                return null;
                // throw new Exception($"{type} Not found.");
            }

            return (T)service;
        }
    }
}
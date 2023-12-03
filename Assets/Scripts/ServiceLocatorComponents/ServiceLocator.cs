using System;
using System.Collections.Generic;
using UnityEngine;

namespace DarkHavoc.ServiceLocatorComponents
{
    public class ServiceLocator
    {
        public static ServiceLocator Instance => _instance ??= new ServiceLocator();
        private static ServiceLocator _instance;
        private readonly Dictionary<Type, object> _services = new();

        public bool TryToRegisterService<T>(T service)
        {
            Type type = service.GetType();
            if (_services.ContainsKey(type)) return false;
            Debug.Log($"Register {type} Service.");
            _services.Add(type, service);
            return true;
        }

        public void RemoveService<T>(T service)
        {
            var type = service.GetType();
            if (!_services.ContainsKey(type)) return;
            Debug.Log($"Remove {type} Service.");
            _services.Remove(type);
        }

        public T GetService<T>() where T : class
        {
            var type = typeof(T);
            if (!_services.TryGetValue(type, out var service))
            {
                Debug.LogError($"{type} Not found.");
                return null;
                // throw new Exception($"{type} Not found.");
            }

            return (T)service;
        }
    }
}
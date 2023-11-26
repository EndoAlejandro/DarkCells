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

        public void AddService<T>(T service)
        {
            Debug.Log($"Register {service.GetType()} Service.");
            var type = service.GetType();
            if (_services.ContainsKey(type)) return;
            _services.Add(type, service);
        }
        
        public void RemoveService<T>(T t)
        {
            var type = t.GetType();
            if (!_services.ContainsKey(type)) return;
            _services.Remove(type);
        }

        public T GetService<T>()
        {
            var type = typeof(T);
            if (!_services.TryGetValue(type, out var service))
                throw new Exception($"{type} Not found.");
            return (T)service;
        }
    }
}
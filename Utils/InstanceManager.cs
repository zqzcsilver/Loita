using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Loita.Utils
{
    internal abstract class InstanceManager
    {
        protected static ConcurrentDictionary<Type, List<object>> instances = new ConcurrentDictionary<Type, List<object>>();

        public static object GetInstance(Type type) => instances.ContainsKey(type) && instances[type].Count > 0 ? instances[type][0] : null;

        public static void RegisterInstance(object instance)
        {
            instances.GetOrAdd(instance.GetType(), new List<object>()).Add(instance);
        }
    }

    internal class InstanceManager<T> : InstanceManager
    {
        public static T Instance => (T)(GetInstance(typeof(T)));

        public static void RegisterInstance(T instance)
        {
            instances.GetOrAdd(typeof(T), new List<object>()).Add(instance);
        }

        public static List<T> GetInstances()
        {
            return instances.GetOrAdd(typeof(T), new List<object>()).ConvertAll<T>(new Converter<object, T>(obj => (T)obj));
        }
    }
}
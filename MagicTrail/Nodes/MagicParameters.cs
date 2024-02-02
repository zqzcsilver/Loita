using System.Collections.Generic;

namespace Loita.MagicTrail.Nodes
{
    internal class MagicParameters
    {
        private readonly Dictionary<string, object> parameters = new Dictionary<string, object>();

        public object this[string key]
        {
            get
            {
                return parameters.ContainsKey(key) ? parameters[key] : null;
            }
            set
            {
                if (parameters.ContainsKey(key))
                    parameters[key] = value;
                else
                    parameters.Add(key, value);
            }
        }

        public bool TryGetValue<T>(string key, out T t)
        {
            if (parameters.ContainsKey(key) && parameters[key] is T to)
            {
                t = to;
                return true;
            }
            t = default;
            return false;
        }

        public T GetValue<T>(string key, T defaultValue = default) => TryGetValue(key, out T t) ? t : defaultValue;
    }
}
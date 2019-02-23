using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRTest.Domain
{
    public class ConnectionMap<T>
    {
        //private readonly ILogger<ConnectionMap<T>> _logger;
        private readonly Dictionary<T, HashSet<string>> _connections;

        public ConnectionMap(/*ILogger<ConnectionMap<T>> logger*/)
        {
            //_logger = logger;
            _connections = new Dictionary<T, HashSet<string>>();
        }

        public int Count()
        {
            return _connections.Count();
        }

        public void Add(T key, HashSet<string> value)
        {
            _connections.Add(key, value);
        }

        public void Add(T key, string value)
        {
            if (ContainsKey(key))
            {
                //_logger.LogWarning($"Connection map already contains key '{key}'");
                return;
            }

            var set = new HashSet<string>
            {
                value
            };
            _connections.Add(key, set);
        }

        public void AddValueToSet(T key, string value)
        {
            if (!ContainsKey(key))
            {
                //_logger.LogWarning($"Connection map does not contain key '{key}'");
                return;
            }

            var values = GetValues(key);

            if (!ContainsValue(values, value))
            {
                //_logger.LogWarning($"Connection map's values at key '{key}' already contain value '{value}'");
                return;
            }

            //_logger.LogInformation($"Added connectionId '{value}' to set.");
            values.Add(value);
        }

        public void Remove(T key)
        {
            _connections.Remove(key);
        }

        public void RemoveValueFromSet(T key, string value)
        {
            if (!ContainsKey(key))
            {
                return;
            }

            var values = GetValues(key);

            if (!ContainsValue(values, value))
            {
                return;
            }

            values.Remove(value);
        }

        public bool ContainsKey(T key)
        {
            return _connections.ContainsKey(key);
        }

        public bool ContainsValueSet(HashSet<string> value)
        {
            return _connections.ContainsValue(value);
        }

        public bool ContainsValue(HashSet<string> values, string value)
        {
            return values.Contains(value);
        }

        public ICollection<HashSet<string>> Values()
        {
            return _connections.Values;
        }

        public ICollection<T> Keys()
        {
            return _connections.Keys;
        }

        public HashSet<string> GetValues(T key)
        {
            return _connections[key];
        }
    }
}

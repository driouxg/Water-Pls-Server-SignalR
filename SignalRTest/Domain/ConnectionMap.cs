using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRTest.Domain
{
    public class ConnectionMap<T>
    {
        private readonly Dictionary<T, HashSet<string>> _connections;

        public ConnectionMap()
        {
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

        public void Remove(T key)
        {
            _connections.Remove(key);
        }

        public bool ContainsKey(T key)
        {
            return _connections.ContainsKey(key);
        }

        public bool ContainsValue(HashSet<string> value)
        {
            return _connections.ContainsValue(value);
        }

        public ICollection<HashSet<string>> Values()
        {
            return _connections.Values;
        }

        public ICollection<T> Keys()
        {
            return _connections.Keys;
        }

        public ICollection<T, HashSet<string>> Pairs()
        {
            return _connections.
        }
    }
}

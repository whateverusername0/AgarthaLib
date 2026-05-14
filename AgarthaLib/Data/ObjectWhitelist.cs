using System;
using System.Collections.Generic;
using System.Linq;

namespace AgarthaLib.Data
{
    [Serializable] public class ObjectWhitelist<T> where T : IComparable
    {
        public List<T> Whitelist = new();
        public List<T> Blacklist = new();

        public bool IsWhitelistPass(T t)
            => Whitelist.Any(q => t.Equals(q));

        public bool IsWhitelistPass(List<T> l)
            => Whitelist.Any(q => l.Contains(q));

        public bool IsBlacklistPass(T t)
            => !Blacklist.Any(q => t.Equals(q));

        public bool IsBlacklistPass(List<T> l)
            => !Blacklist.Any(q => l.Contains(q));

        public bool Pass(T t)
        {
            if (Whitelist.Count > 0) return IsWhitelistPass(t);
            if (Blacklist.Count > 0) return IsBlacklistPass(t);
            return true;
        }

        public bool Pass(List<T> l)
        {
            if (Whitelist.Count > 0) return IsWhitelistPass(l);
            if (Blacklist.Count > 0) return IsBlacklistPass(l);
            return true;
        }
    }
}

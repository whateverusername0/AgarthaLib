using System;

namespace AgarthaLib.Localization
{
    [Serializable] public class LocId
    {
        public string ID;
        public string Locale;

        public LocId(string id, string locale)
        {
            ID = id;
            Locale = locale;
        }

        #region Operators

        public static bool operator ==(LocId a, LocId b)
            => a.ID == b.ID;

        public static bool operator !=(LocId a, LocId b)
            => !(a == b);

        public override bool Equals(object obj)
            => base.Equals(obj);

        public override int GetHashCode()
            => ID.GetHashCode();

        public static implicit operator LocId(string a)
            => new(a, string.Empty);

        #endregion
    }
}

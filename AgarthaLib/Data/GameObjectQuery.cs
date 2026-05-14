using AgarthaLib.Extensions;
using System;
using UnityEngine;

namespace AgarthaLib.Data
{
    [Serializable] public class GameObjectQuery
    {
        public LayerMask Mask;
        public ObjectWhitelist<string> TagQuery;

        public virtual bool Matches(GameObject go)
            => (Mask == 0 || Mask.IsInLayerMask(go.layer)) && TagQuery.Pass(go.tag);
    }
}

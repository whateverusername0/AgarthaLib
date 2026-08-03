using AgarthaLib.Attributes;
using AgarthaLib.MonoBehavior;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AgarthaLib._2D.Grids
{
    // oh my fucking god
    // at least the implementation is as stupid as wood bark
    public abstract class MapManager<TMap, TGrid, TTilemap, TLayer>
        : AgarthanSingleton<MapManager<TMap, TGrid, TTilemap, TLayer>>
        where TMap : Map<TGrid, TTilemap, TLayer>
        where TGrid : MapGrid<TTilemap, TLayer>
        where TTilemap : MapGridLayer<TLayer>
        where TLayer : Enum
    {
        [SerializeField, EditorReadOnly] protected List<TMap> _maps = new();
        [SerializeField, EditorReadOnly] protected TMap _activeMap;

        public List<TMap> Maps => _maps;
        public TMap ActiveMap => _activeMap;

        public virtual void ResolveMaps()
            => _maps = FindObjectsOfType<TMap>(includeInactive: true).ToList();

        public virtual TMap ResolveActiveMap()
        {
            if (_maps == null || _maps.Count == 0)
                ResolveMaps();

            var map = _maps.FirstOrDefault();
            SetActiveMap(map);
            return map;
        }

        public virtual void SetActiveMap(TMap map)
        {
            if (map == null) return;

            if (_activeMap != null)
                _activeMap.DisableRendering();

            map.EnableRendering();

            var lastMap = _activeMap;
            _activeMap = map;
            ActiveMapChanged(lastMap, map);
        }

        public abstract void ActiveMapChanged(TMap lastMap, TMap newMap);
    }
}

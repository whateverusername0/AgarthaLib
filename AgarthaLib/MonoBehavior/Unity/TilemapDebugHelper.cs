using AgarthaLib.Extensions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace AgarthaLib.MonoBehavior.Unity
{
    [RequireComponent(typeof(Tilemap))]
    public class TilemapDebugHelper : MonoBehaviour
    {
        private Tilemap _tilemap;

        private void Awake()
            => _tilemap = GetComponent<Tilemap>();

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            _tilemap = _tilemap == null ? GetComponent<Tilemap>() : _tilemap;
            foreach (var pos in _tilemap.GetAllTilesPositions())
                Handles.Label(pos - new Vector3(.5f, .5f, .5f), $"(x:{pos.x}, y:{pos.y}, z:{pos.z})");
        }
#endif
    }
}

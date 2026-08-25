using AgarthaLib.Animation.Sprites;
using AgarthaLib.Attributes;
using AgarthaLib.Data;
using AgarthaLib.Extensions;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace AgarthaLib._2D.Tilemaps.RuleTiles
{
    [CreateAssetMenu(menuName = "AgarthaLib / Tiles / Animated tile")]
    public class AnimatedTile : TileBase
    {
        [ScriptableObjectIcon(ConstColor.black)] public Sprite _soi
            => Animation != null && Animation.Frames.Count > 0 
            ? Animation.Frames[0] : null;

        [Header(nameof(AnimatedTile))]
        public SpriteAnimation Animation;
        [SerializeField] private GameObject _prefabReference;
        [EditorReadOnly] public GameObject PrefabInstance;

        public TileAnimationData GetAnimationData()
        {
            var sprites = Animation.Frames.ToArray();
            var speed = Animation.FPS / sprites.Length;
            var duration = sprites.Length / speed;

            var tad = new TileAnimationData
            {
                animatedSprites = sprites,
                animationSpeed = Animation.FPS,
                animationStartTime = 0 // guess what
            };

            return tad;
        }

        public override bool GetTileAnimationData(Vector3Int position, ITilemap tilemap, ref TileAnimationData tad)
        {
            if (Animation == null) return false;
            var gad = GetAnimationData();

            tad.animatedSprites = gad.animatedSprites;
            tad.animationSpeed = gad.animationSpeed;
            tad.animationStartTime = gad.animationStartTime;
            return true;
        }

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            if (Animation == null || Animation.Frames.Count == 0)
            {
                base.GetTileData(position, tilemap, ref tileData);
                return;
            }

            tileData.sprite = Animation.Frames[0];
            tileData.gameObject = _prefabReference;
        }

        public override bool StartUp(Vector3Int pos, ITilemap it, GameObject go)
        {
            PrefabInstance = go;
            if (PrefabInstance == null)
                return false;

            if (PrefabInstance.TryGetComponent<SpriteRenderer>(out var sr)
            && it.GetTilemap().TryGetComponent<TilemapRenderer>(out var tr))
                sr.sortingOrder = tr.sortingOrder;

            return true;
        }
    }
}

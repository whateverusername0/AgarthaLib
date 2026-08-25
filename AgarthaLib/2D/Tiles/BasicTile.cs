using AgarthaLib.Attributes;
using AgarthaLib.Data;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace AgarthaLib._2D.Tiles
{
    /// <summary>
    ///     This exists because Unity hides their basic tile asset creation button thing.
    /// </summary>
    [CreateAssetMenu(menuName = "2D/Tiles/Basic Tile")]
    public class BasicTile : Tile
    {
        [ScriptableObjectIcon(ConstColor.black)] public Sprite _soi
            => sprite;
    }
}

using AgarthaLib.Attributes;
using System.Collections.Generic;
using UnityEngine;

namespace AgarthaLib.Sprites.Direction
{
    [CreateAssetMenu(menuName = "AgarthaLib / Sprites / Directional Sprite")]
    public class DirectionalSprite : ScriptableObject
    {
        [ScriptableObjectIcon] public Sprite Icon;

        [SerializeField] private List<Sprite> _directions;
        public Sprite Up, Down, Left, Right;

        private void OnValidate()
        {
            TryAutoUnwrap();
        }

        private void TryAutoUnwrap()
        {
            if (_directions == null || _directions.Count != 4) return;

            Up = _directions[1];
            Down = _directions[0];
            Left = _directions[2];
            Right = _directions[3];
        }
    }
}
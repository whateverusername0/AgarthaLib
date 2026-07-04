using AgarthaLib.Attributes;
using AgarthaLib.Data;
using System.Collections.Generic;
using UnityEngine;

namespace AgarthaLib._2D.Sprites.Direction
{
    [CreateAssetMenu(menuName = "AgarthaLib / Sprites / Directional Sprite")]
    public class DirectionalSprite : ScriptableObject
    {
        [SerializeField] private Sprite _icon;

        [ScriptableObjectIcon(ConstColor.clear)]
        public Sprite Icon
        {
            get => _icon;
            set => _icon = value;
        }

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
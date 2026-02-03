using AgarthaLib.Attributes;
using AgarthaLib.MonoBehavior;
using UnityEngine;

namespace AgarthaLib.Sprites.Direction
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class DirectionalSpriteRenderer : AgarthanBehaviour
    {
        [SerializeField, ValidateNull] private SpriteRenderer SR;
        public DirectionalSprite Sprite;
        public Transform Pivot;

        private void Update()
        {
            if (Pivot == null) return;

            var rot = Mathf.RoundToInt(Pivot.eulerAngles.z / 90);
            this.transform.rotation = Quaternion.identity;

            if (SR == null || Sprite == null) return;
            switch (rot)
            {
                case 0: default: SR.sprite = Sprite.Up; break;
                case 1: SR.sprite = Sprite.Right; break;
                case 2: SR.sprite = Sprite.Down; break;
                case 3: SR.sprite = Sprite.Left; break;
            }
        }
    }
}
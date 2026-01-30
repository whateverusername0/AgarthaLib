using AgarthaLib.MonoBehavior;
using UnityEngine;

namespace AgarthaLib.Sprites.Direction
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class DirectionalSpriteRenderer : AgarthanBehaviour
    {
        private SpriteRenderer _sr;
        public DirectionalSprite Sprite;
        public Transform Pivot;

        private void Start()
        {
            _sr = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            if (Pivot == null) return;
            var rot = Mathf.FloorToInt(Pivot.eulerAngles.z / 90);
            this.transform.rotation = Quaternion.identity;

            if (Sprite == null) return;
            switch (rot)
            {
                case 0: default: _sr.sprite = Sprite.Up; break;
                case 1: _sr.sprite = Sprite.Right; break;
                case 2: _sr.sprite = Sprite.Down; break;
                case 3: _sr.sprite = Sprite.Left; break;
            }
        }
    }
}
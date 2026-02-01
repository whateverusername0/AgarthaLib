using AgarthaLib.Attributes;
using AgarthaLib.MonoBehavior;
using UnityEngine;

namespace AgarthaLib.Cameras
{
    [RequireComponent(typeof(Camera))]
    public class RestrictedCamera : AgarthanBehaviour
    {
        [SerializeField, ValidateNull] private Camera Camera;

        public int FPS = 12;
        private float _updateTimer = 0f;

        protected override void Start()
        {
            base.Start();
            Camera.enabled = false;
        }

        // Update is called once per frame
        private void Update()
        {
            _updateTimer += Time.deltaTime;
            if (_updateTimer >= 1 / FPS)
            {
                _updateTimer = 0f;
                UpdateCamera();
            }
        }

        private void UpdateCamera()
        {
            Camera.Render();
        }
    }
}
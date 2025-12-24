using UnityEngine;

namespace AgarthaLib.MonoBehavior
{
    public class CopyTransform : MonoBehaviour
    {
        public Transform Origin;
        public bool Position, Rotation, Scale;

        [Header("Lerp")]
        public bool Interpolate = true;
        public float LerpSpeed = 5f;

        private void Update()
        {
            if (Position)
            {
                transform.position = Interpolate
                    ? Vector3.Lerp(transform.position, Origin.position, Time.deltaTime * LerpSpeed)
                    : Origin.position;
            }

            if (Rotation)
            {
                transform.eulerAngles = Interpolate
                    ? Vector3.Lerp(transform.eulerAngles, Origin.eulerAngles, Time.deltaTime * LerpSpeed)
                    : Origin.eulerAngles;
            }

            if (Scale)
            {
                transform.localScale = Interpolate
                    ? Vector3.Lerp(transform.localScale, Origin.localScale, Time.deltaTime * LerpSpeed)
                    : Origin.localScale;
            }
        }
    }
}
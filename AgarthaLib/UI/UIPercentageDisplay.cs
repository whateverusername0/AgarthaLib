using AgarthaLib.MonoBehavior;
using TMPro;
using UnityEngine;

namespace AgarthaLib.UI
{
    public class UIPercentageDisplay : MonoBehaviour
    {
        public ValueRange Range;
        public TMP_Text Text;

        public bool UseLerp = true;
        public float Speed = 2.5f;

        private float _lerp = 0f;
        private float _range = 0f;

        private void Update()
        {
            var perc = (Range.Value - Range.Min) / Range.Max;

            _range = perc;

            if (UseLerp)
            {
                _lerp = Mathf.Lerp(_lerp, perc, Time.deltaTime * Speed);
                _range = _lerp;
            }

            Text.text = $"{Mathf.RoundToInt(_range * 100)}%";
        }
    }
}
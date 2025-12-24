using UnityEngine;

namespace Agartha.MonoBehavior
{
    // oh yeah?
    public class DoNotDestroyOnLoad : MonoBehaviour
    {
        private void Start()
        {
            // yeah.
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
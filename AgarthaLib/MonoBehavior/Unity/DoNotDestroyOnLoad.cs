using UnityEngine;

namespace AgarthaLib.MonoBehavior.Unity
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
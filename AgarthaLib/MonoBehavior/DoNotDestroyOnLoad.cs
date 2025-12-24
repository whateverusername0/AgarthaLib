using UnityEngine;

namespace AgarthaLib.MonoBehavior
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
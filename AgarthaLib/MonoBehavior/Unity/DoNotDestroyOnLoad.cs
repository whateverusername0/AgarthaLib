using AgarthaLib.Data;
using UnityEngine;

namespace AgarthaLib.MonoBehavior.Unity
{
    // oh yeah?
    public class DoNotDestroyOnLoad : MonoBehaviour, IDoNotDestroyOnLoad
    {
        private void Start()
        {
            // yeah.
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace AgarthaLib.HTN
{
    public class HTNAgent : MonoBehaviour
    {
        /// <summary>
        ///     The higher the number, the higher it's priority is.
        ///     Means 0 is the lowest.
        /// </summary>
        public Dictionary<int, HTNTaskComposite> PriorityTaskPool;

        private void Update()
        {
            for (int i = PriorityTaskPool.Count - 1; i >= 0; i--)
            {
                if (!PriorityTaskPool[i].CanExecute)
                    continue;

                PriorityTaskPool[i].UpdateTask();
            }
            
            // todo add more behavior here if necessary.
        }
    }
}

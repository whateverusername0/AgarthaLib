using System.Collections.Generic;
using UnityEngine;

namespace AgarthaLib.HTN
{
    public abstract class HTNTaskSequence : HTNTask
    {
        public List<HTNTask> Tasks;
        public HTNTask CurrentTask;

        public float ThinkDelay = 0.5f;
        [SerializeField] private float _thinkTimer = 0f;

        public override IEnumerator<HTNTaskStatus> TaskUpdateEnumerator(HTNAgent agent)
        {
            _thinkTimer += Time.deltaTime;
            while (_thinkTimer < ThinkDelay)
                yield return HTNTaskStatus.Continuing;

            _thinkTimer = 0f;

            foreach (var task in Tasks)
            {
                CurrentTask = task;
                var e = task.TaskUpdateEnumerator(agent);
                while (e.MoveNext())
                    yield return e.Current;
            }

            CurrentTask = null;
        }
    }
}

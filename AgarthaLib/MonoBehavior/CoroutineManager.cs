using AgarthaLib.Attributes;
using AgarthaLib.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AgarthaLib.MonoBehavior
{
    [Serializable] public class CoroutineData
    {
        public IEnumerator Enumerator;
        public Coroutine Routine;
        public bool QueueCancel = false;
        public bool Finished = false;

        public CoroutineData(IEnumerator routine)
            => Enumerator = routine;
    }

    public class CoroutineManager : AgarthanSingleton<CoroutineManager>
    {
        // TODO change to a dictionary that includes Invoker.
        [SerializeField, EditorReadOnly] private List<CoroutineData> _pool = new();

        protected override void Update()
        {
            base.Update();

            var pool = new List<CoroutineData>(_pool);
            foreach (var cd in _pool)
            {
                if (cd.Routine == null)
                    cd.Routine = StartCoroutine(WrappedCoroutine(cd));

                // cleanup
                if (cd.QueueCancel || cd.Finished)
                {
                    if (cd.Routine != null) StopCoroutine(cd.Routine);
                    pool.Remove(cd);
                }
            }
            _pool = pool;
        }

        private IEnumerator WrappedCoroutine(CoroutineData cd)
        {
            if (cd.Finished || cd.QueueCancel)
                yield break;

            while (cd.Enumerator.MoveNext())
            {
                yield return cd.Enumerator.Current;

                // check again
                if (cd.Finished || cd.QueueCancel)
                    yield break;
            }

            // make assumptions
            cd.Finished = true;
            yield break;
        }

        public CoroutineData Add(IEnumerator ie, bool @override = false, bool allowDuplicate = false)
        {
            if (TryGet(ie, out var running))
            {
                if (@override) Remove(running);
                if (!allowDuplicate) return running;
            }

            var cd = new CoroutineData(ie);
            _pool.Add(cd);
            return cd;
        }

        public void Remove(CoroutineData cd)
        {
            cd.QueueCancel = true;
            // removal from list happens in Update
        }

        public void Remove(IEnumerator ie)
        {
            var cd = _pool.Where(q => q.Enumerator == ie).ToList();
            if (cd.Count > 0) Remove(cd.FirstOrDefault());
        }

        public bool Contains(CoroutineData cd)
            => _pool.Contains(cd);

        public bool Contains(IEnumerator ie)
            => _pool.Contains(_pool.Where(q => q.Enumerator == ie).FirstOrDefault());

        public CoroutineData Get(IEnumerator ie)
            => _pool.Where(q => q.Enumerator.Compare(ie)).FirstOrDefault();

        public bool TryGet(IEnumerator ie, out CoroutineData cd)
        {
            cd = Get(ie);
            return cd != null;
        }
    }
}
using AgarthaLib.Attributes;
using UnityEngine;

namespace AgarthaLib.Rendering
{
    /// <summary>
    ///     Manages the renderer based on a hierarchy.
    ///     Very convenient for scripting.
    /// </summary>
    [RequireComponent(typeof(Renderer))]
    public class ManagedRenderer : MonoBehaviour
    {
        public bool Enabled = true;

        [SerializeField, EditorReadOnly] private Renderer _renderer;
        [SerializeField, EditorReadOnly] private ManagedRenderer _parent;

        /// <summary>
        ///     If it should get rendered.
        /// </summary>
        public bool IsEnabled => (_parent == null || _parent.IsEnabled) && Enabled;

        private void Awake()
            => _renderer = GetComponent<Renderer>();

        private void Update()
            => _parent = GetComponentInParent<ManagedRenderer>();
    }
}

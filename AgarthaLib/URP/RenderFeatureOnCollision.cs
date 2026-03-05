using AgarthaLib.Collision;
using AgarthaLib.MonoBehavior;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace AgarthaLib.URP
{
    public class RenderFeatureOnCollision : AgarthanBehaviour
    {
        public UniversalRendererData URPD;

        public FullScreenPassRendererFeature.InjectionPoint InjectionPoint;
        public Material FullscreenMaterial;
        private FullScreenPassRendererFeature Feature;


        protected override void Start()
        {
            base.Start();

            Feature = ScriptableObject.CreateInstance<FullScreenPassRendererFeature>();
            Feature.name = FullscreenMaterial.name;
            Feature.injectionPoint = InjectionPoint;
            Feature.passMaterial = FullscreenMaterial;
            Feature.requirements =
                ScriptableRenderPassInput.Depth
                | ScriptableRenderPassInput.Normal
                | ScriptableRenderPassInput.Color
                | ScriptableRenderPassInput.Motion; // everything

            Feature.Create();

            SubscribeEvent<CollisionEnterEvent>(OnCollisionEnterEvent);
            SubscribeEvent<CollisionExitEvent>(OnCollisionExitEvent);
        }

        private void OnCollisionEnterEvent(GameObject invoker, ref CollisionEnterEvent args)
        {
            Feature.Create();
            URPD.rendererFeatures.Add(Feature);
            URPD.SetDirty();
        }

        private void OnCollisionExitEvent(GameObject invoker, ref CollisionExitEvent args)
        {
            URPD.rendererFeatures.Remove(Feature);
            URPD.SetDirty();
        }

        private void OnDestroy()
        {
            URPD.rendererFeatures.Remove(Feature);
            URPD.SetDirty();
            Destroy(Feature);
        }
    }
}

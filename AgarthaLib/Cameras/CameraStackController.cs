using AgarthaLib.Attributes;
using AgarthaLib.MonoBehavior;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace AgarthaLib.Cameras
{
    [RequireComponent(typeof(Camera))]
    public class CameraStackController : AgarthanBehaviour
    {
        [SerializeField, OnChangedCall(nameof(Start))] private Camera BaseCamera;
        [SerializeField, ReadOnly] private List<Camera> CameraStack;

        private void Start()
        {
            if (BaseCamera == null) return;
            CameraStack = BaseCamera.GetUniversalAdditionalCameraData().cameraStack;
        }

        private void Update()
        {
            if (BaseCamera == null) return;
            foreach (var cam in CameraStack)
                ProcessCamera(cam, BaseCamera);
        }

        public void ProcessCamera(Camera cam, Camera @base)
        {
            if (cam == null) return;
            cam.fieldOfView = @base.fieldOfView;
            cam.nearClipPlane = @base.nearClipPlane;
            cam.farClipPlane = @base.farClipPlane;
            cam.transform.position = @base.transform.position;
            cam.transform.rotation = @base.transform.rotation;
        }
    }
}
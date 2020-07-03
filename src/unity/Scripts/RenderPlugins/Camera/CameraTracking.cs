using System;
using UnityEngine;

namespace UnityKinematics
{
    [RequireComponent(typeof(Camera))]
    public class CameraTracking : MonoBehaviour
    {
        public string trackingName = "root";
        public Vector3 sphereCenterOffset = new Vector3(0, 2, 0);
        public float sphereRadius = 7.7f;
        public float longitude = 0;
        public float latitude = 0;
        public float longitudeAdjustmentStep = 0.02f;
        public float latitudeAdjustmentStep = 0.02f;
        public float radiusAdjustmentStep = 0.1f;
        public float offsetAdjustmentStep = 0.1f;
        public bool trackX = true;
        public bool trackY = false;
        public bool trackZ = true;

        private void UpdateTracking()
        {
            GameObject obj = GameObject.Find(trackingName);
            if (obj)
            {
                Vector3 trackingOffsetPos = sphereRadius * new Vector3(
                    Mathf.Cos(latitude) * Mathf.Cos(longitude), 
                    Mathf.Sin(latitude),
                    Mathf.Cos(latitude) * Mathf.Sin(longitude));
                Vector3 transformMask = new Vector3(Convert.ToSingle(trackX), Convert.ToSingle(trackY), Convert.ToSingle(trackZ));
                Vector3 sphereCenter = Vector3.Scale(transformMask, obj.transform.position) + sphereCenterOffset;
                sphereCenter.y = sphereCenterOffset.y;
                transform.position = trackingOffsetPos + sphereCenter;
                transform.LookAt(sphereCenter, Vector3.up);
            }
        }

        void Start()
        {
            KinematicsServerEvents.OnAfterNewFrame += f => UpdateTracking();
        }

        void Update()
        {
            HandleKeyPress();
            UpdateTracking();
        }

        private void HandleKeyPress()
        {
            if (InputController.GetKey("Camera Latitude +")) 
            {
                latitude = Mathf.Clamp(latitude + latitudeAdjustmentStep, -Mathf.PI / 2, Mathf.PI / 2);
            }
            if (InputController.GetKey("Camera Latitude -")) 
            {
                latitude = Mathf.Clamp(latitude - latitudeAdjustmentStep, -Mathf.PI / 2, Mathf.PI / 2);
            }
            if (InputController.GetKey("Camera Longitude -")) 
            {
                longitude -= longitudeAdjustmentStep;
                if (longitude < 0) longitude += Mathf.PI * 2;
                if (longitude > Mathf.PI * 2) longitude -= Mathf.PI * 2;
            }
            if (InputController.GetKey("Camera Longitude +")) 
            {
                longitude += longitudeAdjustmentStep;
                if (longitude < 0) longitude += Mathf.PI * 2;
                if (longitude > Mathf.PI * 2) longitude -= Mathf.PI * 2;
            }
            if (InputController.GetKey("Camera Radius +")) 
            {
                sphereRadius += radiusAdjustmentStep;
            }
            if (InputController.GetKey("Camera Radius -")) 
            {
                sphereRadius -= radiusAdjustmentStep;
            }
            if (InputController.GetKey("Camera Center Downward")) 
            {
                sphereCenterOffset.y -= offsetAdjustmentStep;
            }
            if (InputController.GetKey("Camera Center Upward")) 
            {
                sphereCenterOffset.y += offsetAdjustmentStep;
            }
            if (InputController.GetKey("Camera Center Forward")) 
            {
                sphereCenterOffset += new Vector3(transform.forward.x, 0, transform.forward.z).normalized * offsetAdjustmentStep;
            }
            if (InputController.GetKey("Camera Center Backward")) 
            {
                sphereCenterOffset -= new Vector3(transform.forward.x, 0, transform.forward.z).normalized * offsetAdjustmentStep;
            }
            if (InputController.GetKey("Camera Center Leftward")) 
            {
                sphereCenterOffset -= new Vector3(transform.right.x, 0, transform.right.z).normalized * offsetAdjustmentStep;
            }
            if (InputController.GetKey("Camera Center Rightward")) 
            {
                sphereCenterOffset += new Vector3(transform.right.x, 0, transform.right.z).normalized * offsetAdjustmentStep;
            }
        }
    }
}

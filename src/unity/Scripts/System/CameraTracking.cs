using UnityEngine;

namespace UnityKinematics
{
    public class CameraTracking : MonoBehaviour
    {
        public CameraSettings settings = new CameraSettings();

        internal void InitSettings(CameraSettings settings)
        {
            this.settings = settings;
            var camera = gameObject.GetComponent<Camera>();
            camera.nearClipPlane = 0.01f;
            camera.fieldOfView = settings.fieldOfView;
        }

        void Update()
        {
            HandleKeyPress();

            GameObject obj = GameObject.Find(settings.TrackingName);
            if (obj)
            {
                Vector3 trackingOffsetPos = settings.SphereRadius * new Vector3(
                    Mathf.Cos(settings.Latitude) * Mathf.Cos(settings.Longitude), 
                    Mathf.Sin(settings.Latitude),
                    Mathf.Cos(settings.Latitude) * Mathf.Sin(settings.Longitude));
                Vector3 sphereCenter = obj.transform.position + settings.SphereCenterOffset;
                sphereCenter.y = settings.SphereCenterOffset.y;
                transform.position = trackingOffsetPos + sphereCenter;
                transform.LookAt(sphereCenter, Vector3.up);
            }
        }

        private void HandleKeyPress()
        {
            if (InputController.GetKey("Camera Latitude +")) 
            {
                settings.Latitude = Mathf.Clamp(settings.Latitude + settings.LatitudeAdjustmentStep, -Mathf.PI / 2, Mathf.PI / 2);
            }
            if (InputController.GetKey("Camera Latitude -")) 
            {
                settings.Latitude = Mathf.Clamp(settings.Latitude - settings.LatitudeAdjustmentStep, -Mathf.PI / 2, Mathf.PI / 2);
            }
            if (InputController.GetKey("Camera Longitude -")) 
            {
                settings.Longitude -= settings.LongitudeAdjustmentStep;
                if (settings.Longitude < 0) settings.Longitude += Mathf.PI * 2;
                if (settings.Longitude > Mathf.PI * 2) settings.Longitude -= Mathf.PI * 2;
            }
            if (InputController.GetKey("Camera Longitude +")) 
            {
                settings.Longitude += settings.LongitudeAdjustmentStep;
                if (settings.Longitude < 0) settings.Longitude += Mathf.PI * 2;
                if (settings.Longitude > Mathf.PI * 2) settings.Longitude -= Mathf.PI * 2;
            }
            if (InputController.GetKey("Camera Radius +")) 
            {
                settings.SphereRadius += settings.RadiusAdjustmentStep;
            }
            if (InputController.GetKey("Camera Radius -")) 
            {
                settings.SphereRadius -= settings.RadiusAdjustmentStep;
            }
            if (InputController.GetKey("Camera Center Downward")) 
            {
                settings.SphereCenterOffset.y -= settings.OffsetAdjustmentStep;
            }
            if (InputController.GetKey("Camera Center Upward")) 
            {
                settings.SphereCenterOffset.y += settings.OffsetAdjustmentStep;
            }
            if (InputController.GetKey("Camera Center Forward")) 
            {
                settings.SphereCenterOffset += new Vector3(transform.forward.x, 0, transform.forward.z).normalized * settings.OffsetAdjustmentStep;
            }
            if (InputController.GetKey("Camera Center Backward")) 
            {
                settings.SphereCenterOffset -= new Vector3(transform.forward.x, 0, transform.forward.z).normalized * settings.OffsetAdjustmentStep;
            }
            if (InputController.GetKey("Camera Center Leftward")) 
            {
                settings.SphereCenterOffset -= new Vector3(transform.right.x, 0, transform.right.z).normalized * settings.OffsetAdjustmentStep;
            }
            if (InputController.GetKey("Camera Center Rightward")) 
            {
                settings.SphereCenterOffset += new Vector3(transform.right.x, 0, transform.right.z).normalized * settings.OffsetAdjustmentStep;
            }
        }
    }
}

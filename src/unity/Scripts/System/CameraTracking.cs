using UnityEngine;

namespace UnityKinematics
{
    public class CameraTracking : MonoBehaviour
    {
        public string TrackingName = "root";
        public Vector3 SphereCenterOffset = new Vector3(0, 2, 0);
        public float SphereRadius = 7.7f;
        public float Longitude = 0;
        public float Latitude = 0;
        public float LongitudeAdjustmentStep = 0.02f;
        public float LatitudeAdjustmentStep = 0.02f;
        public float RadiusAdjustmentStep = 0.1f;
        public float OffsetAdjustmentStep = 0.1f;

        void Update()
        {
            HandleKeyPress();

            GameObject obj = GameObject.Find(TrackingName);
            if (obj)
            {
                Vector3 trackingOffsetPos = SphereRadius * new Vector3(
                    Mathf.Cos(Latitude) * Mathf.Cos(Longitude), 
                    Mathf.Sin(Latitude),
                    Mathf.Cos(Latitude) * Mathf.Sin(Longitude));
                Vector3 sphereCenter = obj.transform.position + SphereCenterOffset;
                sphereCenter.y = SphereCenterOffset.y;
                transform.position = trackingOffsetPos + sphereCenter;
                transform.LookAt(sphereCenter, Vector3.up);
            }
        }

        private void HandleKeyPress()
        {
            if (InputController.GetKey("Camera Latitude +")) 
            {
                Latitude = Mathf.Clamp(Latitude + LatitudeAdjustmentStep, -Mathf.PI / 2, Mathf.PI / 2);
            }
            if (InputController.GetKey("Camera Latitude -")) 
            {
                Latitude = Mathf.Clamp(Latitude - LatitudeAdjustmentStep, -Mathf.PI / 2, Mathf.PI / 2);
            }
            if (InputController.GetKey("Camera Longitude -")) 
            {
                Longitude -= LongitudeAdjustmentStep;
                if (Longitude < 0) Longitude += Mathf.PI * 2;
                if (Longitude > Mathf.PI * 2) Longitude -= Mathf.PI * 2;
            }
            if (InputController.GetKey("Camera Longitude +")) 
            {
                Longitude += LongitudeAdjustmentStep;
                if (Longitude < 0) Longitude += Mathf.PI * 2;
                if (Longitude > Mathf.PI * 2) Longitude -= Mathf.PI * 2;
            }
            if (InputController.GetKey("Camera Radius +")) 
            {
                SphereRadius += RadiusAdjustmentStep;
            }
            if (InputController.GetKey("Camera Radius -")) 
            {
                SphereRadius -= RadiusAdjustmentStep;
            }
            if (InputController.GetKey("Camera Center Downward")) 
            {
                SphereCenterOffset.y -= OffsetAdjustmentStep;
            }
            if (InputController.GetKey("Camera Center Upward")) 
            {
                SphereCenterOffset.y += OffsetAdjustmentStep;
            }
            if (InputController.GetKey("Camera Center Forward")) 
            {
                SphereCenterOffset += new Vector3(transform.forward.x, 0, transform.forward.z).normalized * OffsetAdjustmentStep;
            }
            if (InputController.GetKey("Camera Center Backward")) 
            {
                SphereCenterOffset -= new Vector3(transform.forward.x, 0, transform.forward.z).normalized * OffsetAdjustmentStep;
            }
            if (InputController.GetKey("Camera Center Leftward")) 
            {
                SphereCenterOffset -= new Vector3(transform.right.x, 0, transform.right.z).normalized * OffsetAdjustmentStep;
            }
            if (InputController.GetKey("Camera Center Rightward")) 
            {
                SphereCenterOffset += new Vector3(transform.right.x, 0, transform.right.z).normalized * OffsetAdjustmentStep;
            }
        }
    }
}

using UnityEngine;

namespace UnityKinematics
{
    public class CameraTracking : MonoBehaviour
    {
        public string TrackingName = "root";
        public float SphereCenterY = 3.5f;
        public float SphereRadius = 7.7f;
        public float Longitude = 0;
        public float Latitude = 0;
        public float LongitudeAdjustmentStep = 0.02f;
        public float LatitudeAdjustmentStep = 0.02f;
        public float RadiusAdjustmentStep = 0.1f;
        public float CenterYAdjustmentStep = 0.1f;

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
                Vector3 sphereCenter = new Vector3(obj.transform.position.x, SphereCenterY, obj.transform.position.z);
                transform.position = trackingOffsetPos + sphereCenter;
                transform.LookAt(sphereCenter, Vector3.up);
            }
        }

        private void HandleKeyPress()
        {
            if (Input.GetKey(KeyCode.W)) 
            {
                Latitude = Mathf.Clamp(Latitude + LatitudeAdjustmentStep, -Mathf.PI / 2, Mathf.PI / 2);
            }
            if (Input.GetKey(KeyCode.S)) 
            {
                Latitude = Mathf.Clamp(Latitude - LatitudeAdjustmentStep, -Mathf.PI / 2, Mathf.PI / 2);
            }
            if (Input.GetKey(KeyCode.A)) 
            {
                Longitude -= LongitudeAdjustmentStep;
                if (Longitude < 0) Longitude += Mathf.PI * 2;
                if (Longitude > Mathf.PI * 2) Longitude -= Mathf.PI * 2;
            }
            if (Input.GetKey(KeyCode.D)) 
            {
                Longitude += LongitudeAdjustmentStep;
                if (Longitude < 0) Longitude += Mathf.PI * 2;
                if (Longitude > Mathf.PI * 2) Longitude -= Mathf.PI * 2;
            }
            if (Input.GetKey(KeyCode.LeftShift)) 
            {
                SphereRadius += RadiusAdjustmentStep;
            }
            if (Input.GetKey(KeyCode.F)) 
            {
                SphereRadius -= RadiusAdjustmentStep;
            }
            if (Input.GetKey(KeyCode.LeftControl)) 
            {
                SphereCenterY -= CenterYAdjustmentStep;
            }
            if (Input.GetKey(KeyCode.LeftAlt)) 
            {
                SphereCenterY += CenterYAdjustmentStep;
            }
        }
    }
}

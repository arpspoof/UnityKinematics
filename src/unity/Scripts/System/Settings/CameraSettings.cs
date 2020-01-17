using System;
using UnityEngine;

namespace UnityKinematics
{
    [Serializable]
    public class CameraSettings
    {
        public string TrackingName = "root";
        public float fieldOfView = 45;
        public Vector3 SphereCenterOffset = new Vector3(0, 2, 0);
        public float SphereRadius = 7.7f;
        public float Longitude = 0;
        public float Latitude = 0;
        public float LongitudeAdjustmentStep = 0.02f;
        public float LatitudeAdjustmentStep = 0.02f;
        public float RadiusAdjustmentStep = 0.1f;
        public float OffsetAdjustmentStep = 0.1f;
    }
}

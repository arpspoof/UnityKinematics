using UnityEngine;

namespace UnityKinematics
{
    [RequireComponent(typeof(Camera))]
    public class Background : MonoBehaviour
    {
        [Header("Background")]
        public CameraClearFlags cameraClearFlags = CameraClearFlags.Skybox;
        public Color backgroundColor;
        
        [Header("Fog")]
        public bool fogEnabled = false;
        public float fogDensity;

        private void UpdateSettings()
        {
            var mainCamera = GetComponent<Camera>();
            mainCamera.clearFlags = cameraClearFlags;
            mainCamera.backgroundColor = backgroundColor;

            RenderSettings.fog = fogEnabled;
            RenderSettings.fogColor = backgroundColor;
            RenderSettings.fogDensity = fogDensity;
        }

        void Update()
        {
            UpdateSettings();
        }

        void OnValidate()
        {
            UpdateSettings();
        }
    }
}

using UnityEngine;

namespace UnityKinematics
{
    public class Style : MonoBehaviour
    {
        public StyleSettings settings;

        internal void InitSettings(StyleSettings settings)
        {
            this.settings = settings;

            var mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
            mainCamera.clearFlags = settings.cameraClearFlags;
            mainCamera.backgroundColor = settings.backgroundColor;

            RenderSettings.fog = settings.fogEnabled;
            RenderSettings.fogColor = settings.fogColor;
            RenderSettings.fogDensity = settings.fogDensity;
        }
    }
}

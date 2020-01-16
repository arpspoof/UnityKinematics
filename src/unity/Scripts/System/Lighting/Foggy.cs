using UnityEngine;

namespace UnityKinematics
{
    public static partial class LightingInitializer
    {
        public static Foggy InitFoggy(Controller controller)
        {
            InitSurrounding(controller);
            return controller.gameObject.AddComponent<Foggy>();
        }
    }

    public class Foggy : MonoBehaviour
    {
        Color color = new Color(250 / 256.0f, 250 / 256.0f, 244 / 256.0f);

        void Start()
        {
            var mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
            mainCamera.clearFlags = CameraClearFlags.SolidColor;
            mainCamera.backgroundColor = color;

            RenderSettings.fog = true;
            RenderSettings.fogColor = color;
            RenderSettings.fogDensity = 0.03f;
        }
    }
}
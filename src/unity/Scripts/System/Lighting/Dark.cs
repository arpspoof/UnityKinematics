using UnityEngine;

namespace UnityKinematics
{
    public static partial class LightingInitializer
    {
        public static Dark InitDark(Controller controller)
        {
            InitSurrounding(controller);
            return controller.gameObject.AddComponent<Dark>();
        }
    }

    public class Dark : MonoBehaviour
    {
        void Start()
        {
            var mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
            mainCamera.clearFlags = CameraClearFlags.SolidColor;
            mainCamera.backgroundColor = Color.black;

            RenderSettings.fog = true;
            RenderSettings.fogColor = Color.black;
            RenderSettings.fogDensity = 0.03f;
        }
    }
}
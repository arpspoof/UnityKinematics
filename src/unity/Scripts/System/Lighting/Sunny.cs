using UnityEngine;

namespace UnityKinematics
{
    public static partial class LightingInitializer
    {
        public static Sunny InitSunny(Controller controller)
        {
            var common = InitSurrounding(controller);
            common.nDirectionalLights = 20;
            common.sunLightIntensity = 0.4f;
            common.downwardIntensity = 0;
            common.directionalIntensity = 0.38f;
            common.directionalOffsetY = -0.5f;

            return controller.gameObject.AddComponent<Sunny>();
        }
    }

    public class Sunny : MonoBehaviour
    {
        void Start()
        {
            var mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
            mainCamera.clearFlags = CameraClearFlags.Skybox;

            RenderSettings.fog = false;
        }
    }
}

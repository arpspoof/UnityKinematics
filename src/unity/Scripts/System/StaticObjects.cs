using UnityEngine;

namespace UnityKinematics
{
    public static partial class StaticObjects
    {
        public static bool ShowUI { get; set; }

        public static void Init(Controller controller)
        {
            InitCamera(controller);
            InitPlane(controller);
            InitLightings(controller);
            
            UI.InitUI();
            UI.ShowUI = controller.generalSettings.showUI;
            
            CustomizeStartUp();
        }

        private static void InitLightings(Controller controller)
        {
            var lightControllerObj = new GameObject("LightController");
            if (controller.generalSettings.lightingPreset != LightingPreset.None)
            {
                GameObject.Destroy(GameObject.Find("Directional Light"));

                var surrounding = lightControllerObj.AddComponent<Surrounding>();
                surrounding.InitSettings(controller.GetLightSettings().surroundingSettings, lightControllerObj);

                var style = lightControllerObj.AddComponent<Style>();
                style.InitSettings(controller.GetLightSettings().styleSettings);
            }
        }

        private static void InitCamera(Controller controller)
        {
            var cameraObj = GameObject.Find("Main Camera");
            var tracking = cameraObj.AddComponent<CameraTracking>();
            tracking.InitSettings(controller.cameraSettings);
        }

        private static void InitPlane(Controller controller)
        {
            PlaneSettings planeSettings = controller.planeSettings;
            StyleSettings styleSettings = controller.GetLightSettings().styleSettings;
            var texturePlane = (Texture2D)Resources.Load(styleSettings.planeTextureName);

            GameObject existingPlane = GameObject.Find("plane");
            if (existingPlane) GameObject.Destroy(existingPlane);

            var plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            plane.name = "Plane";
            plane.transform.localScale = new Vector3(planeSettings.scale, planeSettings.scale, planeSettings.scale);
            plane.GetComponent<Renderer>().material.mainTexture = texturePlane;
            plane.GetComponent<Renderer>().material.mainTextureScale = new Vector2(planeSettings.textureScale, planeSettings.textureScale);
            plane.GetComponent<Renderer>().material.SetFloat("_Metallic", planeSettings.metallic);
            plane.GetComponent<Renderer>().material.SetFloat("_Glossiness", planeSettings.smoothness);
        }
    }
}

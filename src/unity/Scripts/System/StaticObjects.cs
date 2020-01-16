using UnityEngine;

namespace UnityKinematics
{
    public static partial class StaticObjects
    {
        private static Texture2D texturePlane;
        public static bool ShowUI { get; set; }

        public static void Init(Controller controller)
        {
            InitCamera();
            switch (controller.lightingSettings)
            {
                case LightingSettings.Sunny: 
                    InitPlane();
                    LightingInitializer.InitSunny(); 
                    break;
                case LightingSettings.Dark: 
                    InitPlane("floor-dark");
                    LightingInitializer.InitDark(controller); 
                    break;
            }
            UI.InitUI();
            CustomizeStartUp();
        }

        private static void InitCamera()
        {
            var cameraObj = GameObject.Find("Main Camera");
            cameraObj.GetComponent<Camera>().nearClipPlane = 0.01f;
            cameraObj.GetComponent<Camera>().fieldOfView = 45;
            cameraObj.AddComponent<CameraTracking>();
        }

        private static void InitPlane(string resourcesName = "floor")
        {
            texturePlane = (Texture2D)Resources.Load(resourcesName);

            GameObject existingPlane = GameObject.Find("plane");
            if (existingPlane) GameObject.Destroy(existingPlane);

            var plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            plane.name = "Plane";
            plane.transform.localScale = new Vector3(200, 200, 200);
            plane.GetComponent<Renderer>().material.mainTexture = texturePlane;
            plane.GetComponent<Renderer>().material.mainTextureScale = new Vector2(100, 100);
            plane.GetComponent<Renderer>().material.SetFloat("_Metallic", 0.252f);
            plane.GetComponent<Renderer>().material.SetFloat("_Glossiness", 0.458f);
        }
    }
}

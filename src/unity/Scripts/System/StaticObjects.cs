using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UnityKinematics
{
    public static partial class StaticObjects
    {
        private static Texture2D texturePlane;
        public static bool ShowUI { get; set; }

        public static void Init(Controller controller)
        {
            InitCamera();
            InitPlane();
            if (controller.UseDefaultLightSettings) InitLights();
            UI.InitUI();
            CustomizeStartUp();
        }

        public static void InitCamera()
        {
            var cameraObj = GameObject.Find("Main Camera");
            cameraObj.GetComponent<Camera>().nearClipPlane = 0.01f;
            cameraObj.AddComponent<CameraTracking>();
        }

        public static void InitPlane()
        {
            texturePlane = (Texture2D)Resources.Load("floor");

            var plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            plane.name = "Plane";
            plane.transform.localScale = new Vector3(200, 200, 200);
            plane.GetComponent<Renderer>().material.mainTexture = texturePlane;
            plane.GetComponent<Renderer>().material.mainTextureScale = new Vector2(80, 80);
            plane.GetComponent<Renderer>().material.SetFloat("_Metallic", 0.252f);
            plane.GetComponent<Renderer>().material.SetFloat("_Glossiness", 0.458f);
        }

        public static void InitLights()
        {
            GameObject.Destroy(GameObject.Find("Directional Light"));

            var lights = new GameObject("Lights");
            float sideLightsIntensity = 0.5f;

            var downwardLightObj = new GameObject("DownwardLight");
            downwardLightObj.transform.rotation = Quaternion.Euler(90, 0, 0);
            downwardLightObj.transform.parent = lights.transform;

            var downwardLight = downwardLightObj.AddComponent<Light>();
            downwardLight.type = LightType.Directional;
            downwardLight.intensity = 0.5f;

            var sunLightObj = new GameObject("SunLight");
            sunLightObj.transform.rotation = Quaternion.Euler(60, 0, 0);
            sunLightObj.transform.parent = lights.transform;

            var sunLight = sunLightObj.AddComponent<Light>();
            sunLight.type = LightType.Directional;
            sunLight.shadows = LightShadows.Soft;
            sunLight.intensity = 0.6f;

            var forwardLightObj = new GameObject("ForwardLight");
            forwardLightObj.transform.rotation = Quaternion.Euler(0, 0, 0);
            forwardLightObj.transform.parent = lights.transform;
            
            var forwardLight = forwardLightObj.AddComponent<Light>();
            forwardLight.type = LightType.Directional;
            forwardLight.intensity = sideLightsIntensity;

            var backwardLightObj = new GameObject("BackwardLight");
            backwardLightObj.transform.rotation = Quaternion.Euler(180, 0, 0);
            backwardLightObj.transform.parent = lights.transform;
            
            var backwardLight = backwardLightObj.AddComponent<Light>();
            backwardLight.type = LightType.Directional;
            backwardLight.intensity = sideLightsIntensity;

            var leftLightObj = new GameObject("LeftLight");
            leftLightObj.transform.rotation = Quaternion.Euler(0, 90, 0);
            leftLightObj.transform.parent = lights.transform;
            
            var leftLight = leftLightObj.AddComponent<Light>();
            leftLight.type = LightType.Directional;
            leftLight.intensity = sideLightsIntensity;

            var rightLightObj = new GameObject("RightLight");
            rightLightObj.transform.rotation = Quaternion.Euler(0, -90, 0);
            rightLightObj.transform.parent = lights.transform;
            
            var rightLight = rightLightObj.AddComponent<Light>();
            rightLight.type = LightType.Directional;
            rightLight.intensity = sideLightsIntensity;
        }
    }
}

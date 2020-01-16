using UnityEngine;
using System.Collections.Generic;

namespace UnityKinematics
{
    public static partial class LightingInitializer
    {
        public static Surrounding InitSurrounding(Controller controller)
        {
            return controller.gameObject.AddComponent<Surrounding>();
        }
    }

    public class Surrounding : MonoBehaviour
    {
        public uint nDirectionalLights = 20;
        public float directionalIntensity = 0.38f;
        public float directionalOffsetY = -1.5f;
        public float downwardIntensity = 0;
        public float sunLightIntensity = 0.38f;

        private GameObject lightGroupContainer;
        private GameObject sunLightObj, downwardLightObj;
        private List<GameObject> directionalLightObjects = new List<GameObject>();

        void Start()
        {
            GameObject.Destroy(GameObject.Find("Directional Light"));

            lightGroupContainer = new GameObject("Lights");

            downwardLightObj = new GameObject("DownwardLight");
            downwardLightObj.transform.rotation = Quaternion.Euler(90, 0, 0);
            downwardLightObj.transform.parent = lightGroupContainer.transform;
            downwardLightObj.AddComponent<Light>();

            sunLightObj = new GameObject("SunLight");
            sunLightObj.transform.rotation = Quaternion.Euler(60, 0, 0);
            sunLightObj.transform.parent = lightGroupContainer.transform;
            sunLightObj.AddComponent<Light>();
        }

        void Update()
        {
            if (nDirectionalLights < directionalLightObjects.Count)
            {
                for (uint i = nDirectionalLights; i < directionalLightObjects.Count; i++) 
                {
                    GameObject obj = directionalLightObjects[(int)i];
                    directionalLightObjects.Remove(obj);
                    GameObject.Destroy(obj);
                }
            }
            if (nDirectionalLights > directionalLightObjects.Count)
            {
                for (int i = directionalLightObjects.Count; i < nDirectionalLights; i++)
                {
                    GameObject obj = new GameObject($"dir_{i}");
                    obj.transform.SetParent(lightGroupContainer.transform);
                    obj.AddComponent<Light>();
                    directionalLightObjects.Add(obj);
                }
            }

            var downwardLight = downwardLightObj.GetComponent<Light>();
            downwardLight.type = LightType.Directional;
            downwardLight.intensity = downwardIntensity;

            var sunLight = sunLightObj.GetComponent<Light>();
            sunLight.type = LightType.Directional;
            sunLight.shadows = LightShadows.Soft;
            sunLight.intensity = sunLightIntensity;

            for (int i = 0; i < nDirectionalLights; i++)
            {
                float angle = (float)i / nDirectionalLights * Mathf.PI * 2;

                directionalLightObjects[i].transform.position = new Vector3(Mathf.Cos(angle) * 5, 0, Mathf.Sin(angle) * 5);
                directionalLightObjects[i].transform.LookAt(new Vector3(0, directionalOffsetY, 0));

                Light light = directionalLightObjects[i].GetComponent<Light>();
                light.type = LightType.Directional;
                light.color = Color.white;
                light.intensity = directionalIntensity;
                light.shadows = LightShadows.None;
            }
        }
    }
}
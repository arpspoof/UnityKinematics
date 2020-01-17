using UnityEngine;
using System.Collections.Generic;

namespace UnityKinematics
{
    public class Surrounding : MonoBehaviour
    {
        public SurroundingSettings settings;

        private GameObject lightGroupContainer;
        private GameObject sunLightObj, downwardLightObj;
        private List<GameObject> directionalLightObjects = new List<GameObject>();

        internal void InitSettings(SurroundingSettings surroundingSettings, GameObject lightGroupContainer)
        {
            this.settings = surroundingSettings;
            this.lightGroupContainer = lightGroupContainer;

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
            if (settings.nDirectionalLights < directionalLightObjects.Count)
            {
                for (uint i = settings.nDirectionalLights; i < directionalLightObjects.Count; i++) 
                {
                    GameObject obj = directionalLightObjects[(int)i];
                    directionalLightObjects.Remove(obj);
                    GameObject.Destroy(obj);
                }
            }
            if (settings.nDirectionalLights > directionalLightObjects.Count)
            {
                for (int i = directionalLightObjects.Count; i < settings.nDirectionalLights; i++)
                {
                    GameObject obj = new GameObject($"dir_{i}");
                    obj.transform.SetParent(lightGroupContainer.transform);
                    obj.AddComponent<Light>();
                    directionalLightObjects.Add(obj);
                }
            }

            var downwardLight = downwardLightObj.GetComponent<Light>();
            downwardLight.type = LightType.Directional;
            downwardLight.intensity = settings.downwardIntensity;

            var sunLight = sunLightObj.GetComponent<Light>();
            sunLight.type = LightType.Directional;
            if (settings.enableSunlightShadow) sunLight.shadows = LightShadows.Soft;
            else sunLight.shadows = LightShadows.None;
            sunLight.intensity = settings.sunLightIntensity;

            for (int i = 0; i < settings.nDirectionalLights; i++)
            {
                float angle = (float)i / settings.nDirectionalLights * Mathf.PI * 2;

                directionalLightObjects[i].transform.position = new Vector3(Mathf.Cos(angle) * 5, 0, Mathf.Sin(angle) * 5);
                directionalLightObjects[i].transform.LookAt(new Vector3(0, settings.directionalOffsetY, 0));

                Light light = directionalLightObjects[i].GetComponent<Light>();
                light.type = LightType.Directional;
                light.color = Color.white;
                light.intensity = settings.directionalIntensity;
                light.shadows = LightShadows.None;
            }
        }
    }
}
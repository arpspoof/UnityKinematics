using UnityEngine;

// Require: Empty game object as container for lights
// Note: You may want to change Project Settings -> Quality -> Pixel Light Count to higher value

namespace UnityKinematics
{
    public class SurroundingLights : MonoBehaviour
    {
        public LightType lightsType = LightType.Directional;
        public LightShadows sunLightShadowType = LightShadows.Soft;
        public uint nSurroundingLights = 20;
        public float surroundingIntensity = 0.38f;
        public float surroundingOffsetY = -1.5f;
        public float downwardIntensity = 0;
        public float sunLightIntensity = 0.38f;

        private GameObject GetLightObj(int i, bool createIfNotExist)
        {
            string name = $"dir_{i}";
            Transform trans = transform.Find(name);
            if (!trans) 
            {
                if (!createIfNotExist) return null;
                GameObject obj = new GameObject(name);
                obj.transform.SetParent(transform);
                obj.AddComponent<Light>();
                trans = obj.transform;
            }
            return trans.gameObject;
        }

        private void UpdateLightsProperties()
        {
            var downwardLightObj = transform.Find("DownwardLight").gameObject;
            var sunLightObj = transform.Find("SunLight").gameObject;

            var downwardLight = downwardLightObj.GetComponent<Light>();
            downwardLight.type = LightType.Directional;
            downwardLight.intensity = downwardIntensity;

            var sunLight = sunLightObj.GetComponent<Light>();
            sunLight.type = LightType.Directional;
            sunLight.shadows = sunLightShadowType;
            sunLight.intensity = sunLightIntensity;

            for (int i = 0; i < nSurroundingLights; i++)
            {
                float angle = (float)i / nSurroundingLights * Mathf.PI * 2;

                var lightObj = GetLightObj(i, true);
                lightObj.SetActive(true);
                lightObj.transform.position = new Vector3(Mathf.Cos(angle) * 5, 0, Mathf.Sin(angle) * 5);
                lightObj.transform.LookAt(new Vector3(0, surroundingOffsetY, 0));

                Light light = lightObj.GetComponent<Light>();
                light.type = lightsType;
                light.color = Color.white;
                light.intensity = surroundingIntensity;
                light.shadows = LightShadows.None;
            }

            for (int i = (int)nSurroundingLights; ; i++)
            {
                var lightObj = GetLightObj(i, false);
                if (lightObj) lightObj.SetActive(false);
                else break;
            }
        }

        private void CreateDownwardAndSunlight()
        {
            var downwardLightObj = new GameObject("DownwardLight");
            downwardLightObj.transform.rotation = Quaternion.Euler(90, 0, 0);
            downwardLightObj.transform.parent = transform;
            downwardLightObj.AddComponent<Light>();

            var sunLightObj = new GameObject("SunLight");
            sunLightObj.transform.rotation = Quaternion.Euler(60, 0, 0);
            sunLightObj.transform.parent = transform;
            sunLightObj.AddComponent<Light>();
        }

        void Start()
        {
            foreach (Transform child in transform) 
            {
                Destroy(child.gameObject);
            }
            CreateDownwardAndSunlight();
            UpdateLightsProperties();
        }

        void Update()
        {
            UpdateLightsProperties();
        }

        void Reset()
        {
            CreateDownwardAndSunlight();
            UpdateLightsProperties();
        }

        private void UpdateLightsEdit()
        {
            if (Application.isPlaying) 
            {
                UnityEditor.EditorApplication.delayCall -= UpdateLightsEdit;
                return;
            }
            UpdateLightsProperties();
        }

        void OnValidate()
        {
            if (Application.isPlaying) UnityEditor.EditorApplication.delayCall -= UpdateLightsEdit;
            else UnityEditor.EditorApplication.delayCall += UpdateLightsEdit;
        }
    }
}

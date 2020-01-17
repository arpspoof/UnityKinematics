using System;
using UnityEngine;
using UnityEditor;

namespace UnityKinematics
{
    [Serializable]
    public class SurroundingSettings
    {
        public uint nDirectionalLights = 20;
        public float directionalIntensity = 0.38f;
        public float directionalOffsetY = -1.5f;
        public float downwardIntensity = 0;
        public float sunLightIntensity = 0.38f;
        public bool enableSunlightShadow = true;

        public SurroundingSettings(LightingPreset preset)
        {
            switch (preset)
            {
                case LightingPreset.Sunny:
                    nDirectionalLights = 20;
                    sunLightIntensity = 0.4f;
                    downwardIntensity = 0;
                    directionalIntensity = 0.38f;
                    directionalOffsetY = -0.5f;
                    break;
            }
        }
    }

    [Serializable]
    public class StyleSettings
    {
        [Header("Plane Styles")]
        public string planeTextureName;

        [Header("Background")]
        public CameraClearFlags cameraClearFlags = CameraClearFlags.Skybox;
        public Color backgroundColor;
        
        [Header("Fog")]
        public bool fogEnabled = false;
        public float fogDensity;
        public Color fogColor;

        public StyleSettings(LightingPreset preset)
        {
            planeTextureName = "floor";

            switch (preset)
            {
                case LightingPreset.Dark:
                    planeTextureName = "floor-dark";
                    cameraClearFlags = CameraClearFlags.SolidColor;
                    backgroundColor = Color.black;
                    fogEnabled = true;
                    fogDensity = 0.03f;
                    fogColor = backgroundColor;
                    break;
                case LightingPreset.Foggy:
                    planeTextureName = "floor-single";
                    cameraClearFlags = CameraClearFlags.SolidColor;
                    backgroundColor = new Color(250 / 256.0f, 250 / 256.0f, 244 / 256.0f);
                    fogEnabled = true;
                    fogDensity = 0.03f;
                    fogColor = backgroundColor;
                    break;
            }
        }
    }

    [Serializable]
    public class LightSettingsForPreset
    {
        public LightingPreset preset;
        public SurroundingSettings surroundingSettings;
        public StyleSettings styleSettings;

        public LightSettingsForPreset(LightingPreset preset)
        {
            this.preset = preset;
            surroundingSettings = new SurroundingSettings(preset);
            styleSettings = new StyleSettings(preset);
        }
    }
}

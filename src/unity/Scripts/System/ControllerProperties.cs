using UnityEngine;
using System;

namespace UnityKinematics
{
    [Serializable]
    public struct RegisteredMaterials 
    {
        public string name;
        public Material material;
    }

    [Serializable]
    public struct ShortcutMap
    {
        public string name;
        public KeyCode key;
    }

    public enum LightingSettings
    {
        Sunny, Dark, Foggy, None
    }

    public partial class Controller : MonoBehaviour
    {
        [Header("RPC")]
        public ushort ServerPort = 8080;

        [Header("Lighting")]
        public LightingSettings lightingSettings = LightingSettings.Sunny;

        [Header("Camera")]
        public string CameraTrackingObjectName = "root";

        [Header("Materials")]
        public RegisteredMaterials[] registeredMaterials;

        [Header("Keyboard")]
        public ShortcutMap[] CameraControl = new ShortcutMap[]
        {
            new ShortcutMap { name = "Camera Longitude -", key = KeyCode.A },
            new ShortcutMap { name = "Camera Longitude +", key = KeyCode.D },
            new ShortcutMap { name = "Camera Latitude +", key = KeyCode.W },
            new ShortcutMap { name = "Camera Latitude -", key = KeyCode.S },
            new ShortcutMap { name = "Camera Radius +", key = KeyCode.LeftShift },
            new ShortcutMap { name = "Camera Radius -", key = KeyCode.F },
            new ShortcutMap { name = "Camera Center Upward", key = KeyCode.LeftAlt },
            new ShortcutMap { name = "Camera Center Downward", key = KeyCode.LeftControl },
            new ShortcutMap { name = "Camera Center Forward", key = KeyCode.I },
            new ShortcutMap { name = "Camera Center Backward", key = KeyCode.K },
            new ShortcutMap { name = "Camera Center Leftward", key = KeyCode.J },
            new ShortcutMap { name = "Camera Center Rightward", key = KeyCode.L },
        };
        public ShortcutMap[] FPSControl = new ShortcutMap[]
        {
            new ShortcutMap { name = "FPS Adjustment Acceleration", key = KeyCode.RightControl },
            new ShortcutMap { name = "Physical FPS -", key = KeyCode.LeftArrow },
            new ShortcutMap { name = "Physical FPS +", key = KeyCode.RightArrow },
            new ShortcutMap { name = "Render FPS -", key = KeyCode.DownArrow },
            new ShortcutMap { name = "Render FPS +", key = KeyCode.UpArrow },
            new ShortcutMap { name = "Pause", key = KeyCode.Space },
        };
        public ShortcutMap[] UIControl = new ShortcutMap[]
        {
            new ShortcutMap { name = "Show / Hide UI", key = KeyCode.U },
        };
        public ShortcutMap[] SceneControl = new ShortcutMap[]
        {
            new ShortcutMap { name = "Reset Scene", key = KeyCode.R },
        };
    }
}

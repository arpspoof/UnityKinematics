using System;
using UnityEngine;

namespace UnityKinematics
{
    [Serializable]
    public struct ShortcutMap
    {
        public string name;
        public KeyCode key;
    }

    [Serializable]
    public class ShortcutSettings
    {
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

using UnityEngine;
using System.Collections.Generic;

namespace UnityKinematics
{
    public static class InputController
    {
        private static Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();

        public static void InitKeyMapping(Controller controller)
        {
            foreach (var map in controller.shortcutSettings.CameraControl) keys.Add(map.name, map.key);
            foreach (var map in controller.shortcutSettings.FPSControl) keys.Add(map.name, map.key);
            foreach (var map in controller.shortcutSettings.UIControl) keys.Add(map.name, map.key);
            foreach (var map in controller.shortcutSettings.SceneControl) keys.Add(map.name, map.key);
        }

        public static bool GetKey(string keyName) => Input.GetKey(keys[keyName]);
        public static bool GetKeyDown(string keyName) => Input.GetKeyDown(keys[keyName]);
        public static KeyCode GetKeyCode(string keyName) => keys[keyName];
    }
}

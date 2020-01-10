using UnityEngine;
using System.Collections.Generic;

namespace UnityKinematics
{
    public static class InputController
    {
        private static Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();

        public static void InitKeyMapping(Controller controller)
        {
            foreach (var map in controller.CameraControl) keys.Add(map.name, map.key);
            foreach (var map in controller.FPSControl) keys.Add(map.name, map.key);
            foreach (var map in controller.UIControl) keys.Add(map.name, map.key);
            foreach (var map in controller.SceneControl) keys.Add(map.name, map.key);
        }

        public static bool GetKey(string keyName) => Input.GetKey(keys[keyName]);
        public static bool GetKeyDown(string keyName) => Input.GetKeyDown(keys[keyName]);
        public static KeyCode GetKeyCode(string keyName) => keys[keyName];
    }
}

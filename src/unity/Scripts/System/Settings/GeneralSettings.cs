using System;
using UnityEngine;

namespace UnityKinematics
{
    [Serializable]
    public struct RegisteredMaterials 
    {
        public string name;
        public Material material;
    }

    [Serializable]
    public class GeneralSettings
    {
        [Header("RPC")]
        public ushort ServerPort = 8080;

        [Header("Materials")]
        public RegisteredMaterials[] registeredMaterials;
    }
}

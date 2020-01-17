﻿using UnityEngine;
using System.Collections.Generic;

namespace UnityKinematics
{
    public partial class Controller : MonoBehaviour
    {
        public GeneralSettings generalSettings = new GeneralSettings();
        public CameraSettings cameraSettings = new CameraSettings();
        public PlaneSettings planeSettings = new PlaneSettings();
        public LightSettingsForPreset[] lightSettingsForPresets = new LightSettingsForPreset[]
        {
            new LightSettingsForPreset(LightingPreset.Sunny),
            new LightSettingsForPreset(LightingPreset.Dark),
            new LightSettingsForPreset(LightingPreset.Foggy),
        };
        public ShortcutSettings shortcutSettings = new ShortcutSettings();

        public static int SkipRate = 1;
        public static Dictionary<string, Material> RegisteredMaterialsMap = new Dictionary<string, Material>();
        public static HashSet<string> groupNames = new HashSet<string>();

        private int frameCount = -1;
        private CommandHandler commandHandler;
        private List<string> monitoredKeys = new List<string> { "Physical FPS -", "Physical FPS +", "Pause" };

        internal LightSettingsForPreset GetLightSettings()
        {
            LightingPreset preset = generalSettings.lightingPreset;
            foreach (var x in lightSettingsForPresets)
            {
                if (x.preset == preset) return x;
            }

            Debug.LogError($"No settings for preset {preset.ToString("g")}");
            return null;
        }

        void Start()
        {
            StaticObjects.Init(this);
            InputController.InitKeyMapping(this);

            commandHandler = new CommandHandler();
            commandHandler.Init();

            foreach (var kvp in generalSettings.registeredMaterials)
            {
                RegisteredMaterialsMap.Add(kvp.name, kvp.material);
            }

            Renderer renderer = gameObject.AddComponent<MeshRenderer>();
            renderer.material.mainTexture = (Texture2D)Resources.Load("wood");;
            renderer.material.SetFloat("_Metallic", 0.1f);
            renderer.material.SetFloat("_Glossiness", 0.1f);   

            RegisteredMaterialsMap.Add("_sys_default", renderer.material);

            UnityServerAPI.RPCStartServer(generalSettings.ServerPort);
            Debug.Log("Waiting for data");
        }

        void OnApplicationQuit()
        {
            UnityServerAPI.RPCStopServer();
            Debug.Log("RPC Stopped");
        }

        private void CheckKeyboard()
        {
            foreach (string code in monitoredKeys)
            {
                if (InputController.GetKeyDown(code))
                {
                    Command cmd = new Command("_sys_key");
                    cmd.ps.Add(code);
                    if (InputController.GetKey("FPS Adjustment Acceleration")) cmd.pi.Add(10);
                    else cmd.pi.Add(1);
                    CommandSender.Send(cmd);
                }
            }

            if (InputController.GetKeyDown("Render FPS -"))
            {
                SkipRate++;
            }
            if (InputController.GetKeyDown("Render FPS +"))
            {
                if (SkipRate > 1) SkipRate--;
            }

            if (InputController.GetKeyDown("Reset Scene"))
            {
                UI.PhysicalPaused = UI.MaxPhysicalFPS = UI.PhysicalFPS = 0;
                foreach (string name in groupNames)
                {
                    var group = GameObject.Find(name);
                    if (group) GameObject.Destroy(group);
                }
                groupNames.Clear();
            }

            if (InputController.GetKeyDown("Show / Hide UI"))
            {
                UI.ShowUI = !UI.ShowUI;
            }
        }

        void Update()
        {
            RPCCommandBuffer cmdBuffer = UnityServerAPI.RPCGetCommandBuffer();
            int nCmd = cmdBuffer.GetNumOfAvailableElements();
            for (int i = 0; i < nCmd; i++)
            {
                Command cmd = cmdBuffer.ReadAndErase(0);
                commandHandler.HandleCommand(cmd);
            }

            CheckKeyboard();
            UI.UpdateUI();

            frameCount++;
            if (frameCount % SkipRate != 0) 
            {
                return;
            }

            RPCFrameBuffer frameBuffer = UnityServerAPI.RPCGetFrameBuffer();
            int n = frameBuffer.GetNumOfAvailableElements();
            if (n > 0)
            {
                FrameState data = frameBuffer.ReadAndErase(0);
                foreach (GroupState groupState in data.groups)
                {
                    groupNames.Add(groupState.groupName);

                    GameObject groupObj = GameObject.Find(groupState.groupName);
                    if (groupObj == null)
                    {
                        Debug.LogWarning($"RPC: Group {groupState.groupName} does not exist");
                        return;
                    }

                    foreach (ObjectState state in groupState.objectStates) 
                    {
                        string name = state.objectName;
                        Transform trans = groupObj.transform.Find(name);
                        if (trans)
                        {
                            GameObject obj = trans.gameObject;
                            obj.transform.position = new Vector3(state.x, state.y, -state.z);
                            Vector3 axis; 
                            float angle;
                            new Quaternion(state.qx, state.qy, state.qz, state.qw).ToAngleAxis(out angle, out axis);
                            axis.z = -axis.z;
                            obj.transform.rotation = Quaternion.AngleAxis(-angle, axis);
                        }
                        else
                        {
                            Debug.LogWarning($"RPC: Object with name {name} does not exist");
                        }
                    }
                }
            }
        }
    }
}

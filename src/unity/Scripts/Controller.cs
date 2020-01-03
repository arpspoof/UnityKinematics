using UnityEngine;
using System;
using System.Collections.Generic;

namespace UnityKinematics
{
    [Serializable]
    public struct RegisteredMaterials 
    {
        public string name;
        public Material material;
    }

    public class Controller : MonoBehaviour
    {
        public ushort ServerPort = 8080;
        public string CameraTrackingObjectName = "root";
        public RegisteredMaterials[] registeredMaterials;

        public static int SkipRate = 1;
        public static Dictionary<string, Material> RegisteredMaterialsMap = new Dictionary<string, Material>();
        public static HashSet<string> groupNames = new HashSet<string>();

        private int frameCount = -1;
        private CommandHandler commandHandler;
        private List<KeyCode> monitoredKeys = new List<KeyCode> { KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.Space };

        void Start()
        {
            StaticObjects.Init();

            var cameraTracking = GameObject.Find("Main Camera")?.GetComponent<CameraTracking>();
            if (cameraTracking) cameraTracking.TrackingName = CameraTrackingObjectName;

            commandHandler = new CommandHandler();
            commandHandler.Init();

            foreach (var kvp in registeredMaterials)
            {
                RegisteredMaterialsMap.Add(kvp.name, kvp.material);
            }

            Renderer renderer = gameObject.AddComponent<MeshRenderer>();
            renderer.material.mainTexture = (Texture2D)Resources.Load("wood");;
            renderer.material.SetFloat("_Metallic", 0.1f);
            renderer.material.SetFloat("_Glossiness", 0.1f);   

            RegisteredMaterialsMap.Add("_sys_default", renderer.material);

            UnityServerAPI.RPCStartServer(ServerPort);
            Debug.Log("Waiting for data");
        }

        void OnApplicationQuit()
        {
            UnityServerAPI.RPCStopServer();
            Debug.Log("RPC Stopped");
        }

        private void CheckKeyboard()
        {
            foreach (KeyCode code in monitoredKeys)
            {
                if (Input.GetKeyDown(code))
                {
                    Command cmd = new Command("_sys_key");
                    cmd.ps.Add(code.ToString("g"));
                    if (Input.GetKey(KeyCode.RightControl)) cmd.pi.Add(10);
                    else cmd.pi.Add(1);
                    CommandSender.Send(cmd);
                }
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                SkipRate++;
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (SkipRate > 1) SkipRate--;
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                UI.PhysicalPaused = UI.MaxPhysicalFPS = UI.PhysicalFPS = 0;
                foreach (string name in groupNames)
                {
                    var group = GameObject.Find(name);
                    if (group) GameObject.Destroy(group);
                }
                groupNames.Clear();
            }

            if (Input.GetKeyDown(KeyCode.U))
            {
                UI.ShowUI = !UI.ShowUI;
            }
        }

        void Update()
        {
            CommandBuffer cmdBuffer = UnityServerAPI.RPCGetCommandBuffer();
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

            FrameBuffer frameBuffer = UnityServerAPI.RPCGetFrameBuffer();
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

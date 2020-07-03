using UnityEngine;
using System.Collections.Generic;

namespace UnityKinematics
{
    public partial class KinematicsServer : MonoBehaviour
    {
        public static KinematicsServer instance;

        public GeneralSettings generalSettings = new GeneralSettings();
        public ShortcutSettings shortcutSettings = new ShortcutSettings();

        public int skipRate = 1;
        public Material defaultMaterial = null;

        public bool enableSpeedMatch = true;

        public int nModels = 1;

        public int AccumulatedFrames { get; private set; } = 0;
        public double AccumulatedFrameDuration { get; private set; } = 0;

        public static Dictionary<string, Material> RegisteredMaterialsMap = new Dictionary<string, Material>();
        public static HashSet<string> groupNames = new HashSet<string>();

        private int renderFrameCount = -1;
        private int physicalFrameCount = 0;
        private CommandHandler commandHandler;
        private List<string> monitoredKeys = new List<string> { "Physical FPS -", "Physical FPS +", "Pause" };

        void Awake()
        {
            instance = this;
        }

        void Start()
        {
            InputController.InitKeyMapping(this);

            commandHandler = new CommandHandler();
            commandHandler.Init();

            foreach (var kvp in generalSettings.registeredMaterials)
            {
                RegisteredMaterialsMap.Add(kvp.name, kvp.material);
            }

            Renderer renderer = gameObject.AddComponent<MeshRenderer>();
            renderer.material.SetFloat("_Metallic", 0.1f);
            renderer.material.SetFloat("_Glossiness", 0.1f);
            if (defaultMaterial == null)
            {
                renderer.material.mainTexture = (Texture2D)Resources.Load("wood");
                defaultMaterial = renderer.material;
            }

            RegisteredMaterialsMap.Add("_sys_default", defaultMaterial);

            UnityServerAPI.RPCStartServer(generalSettings.ServerPort);
            Debug.Log("Waiting for data");

            KinematicsServerEvents.InvokeOnServerInitialize();
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
                skipRate++;
            }
            if (InputController.GetKeyDown("Render FPS +"))
            {
                if (skipRate > 1) skipRate--;
            }

            if (InputController.GetKeyDown("Reset Scene"))
            {
                foreach (string name in groupNames)
                {
                    var group = GameObject.Find(name);
                    if (group) GameObject.Destroy(group);
                }
                groupNames.Clear();
            }
        }

        public void ExecutePendingCommands()
        {
            RPCCommandBuffer cmdBuffer = UnityServerAPI.RPCGetCommandBuffer();
            int nCmd = cmdBuffer.GetNumOfAvailableElements();
            
            for (int i = 0; i < nCmd; i++)
            {
                Command cmd = cmdBuffer.ReadAndErase(0);
                commandHandler.HandleCommand(cmd);
            }
        }

        void Update()
        {
            ExecutePendingCommands();
            CheckKeyboard();

            renderFrameCount++;
            if (renderFrameCount % skipRate != 0) 
            {
                return;
            }
            if (enableSpeedMatch && Time.time < AccumulatedFrameDuration) return;

            RPCFrameBuffer frameBuffer = UnityServerAPI.RPCGetFrameBuffer();
            int n = frameBuffer.GetNumOfAvailableElements();
            if (n > 0)
            {
                KinematicsServerEvents.InvokeOnBeforeNewFrame(physicalFrameCount++);

                FrameState data = frameBuffer.ReadAndErase(0);

                AccumulatedFrames++;
                AccumulatedFrameDuration += data.duration;

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
                            Vector3 p = new Vector3(state.x, state.y, state.z) * generalSettings.scalingFactor;
                            Quaternion q = new Quaternion(state.qx, state.qy, state.qz, state.qw);
                            bool pHasNaN = float.IsNaN(p.x) || float.IsNaN(p.y) || float.IsNaN(p.z);
                            bool qHasNaN = float.IsNaN(q.x) || float.IsNaN(q.y) || float.IsNaN(q.z) || float.IsNaN(q.w);
                            if (!pHasNaN) obj.transform.position = CoordinateTransform.RightHandToLeftHand(p);
                            if (!qHasNaN) obj.transform.rotation = CoordinateTransform.RightHandToLeftHand(q);
                            if (pHasNaN || qHasNaN) Debug.LogWarning("Receive NaN in frame data, discard");
                        }
                        else
                        {
                            Debug.LogWarning($"RPC: Object with name {name} does not exist");
                        }
                    }
                }

                KinematicsServerEvents.InvokeOnAfterNewFrame(data);
            }
        }
    }
}

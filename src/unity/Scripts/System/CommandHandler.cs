using UnityEngine;

namespace UnityKinematics
{
    public partial class CommandHandler
    {
        private Texture2D texture;

        internal void Init()
        {
            texture = (Texture2D)Resources.Load("wood");
        }

        internal void HandleCommand(Command cmd)
        {
            switch (cmd.name)
            {
                case "_sys_create_primitive":
                    CmdCreatePrimitive(cmd);
                    break; 
                default:
                    KinematicsServerEvents.InvokeOnCommand(cmd);
                    return;
            }
            KinematicsServerEvents.InvokeOnSystemCommand(cmd);
        }

        private void CmdCreatePrimitive(Command cmd)
        {
            string primitiveType = cmd.ps[0];
            string groupName = cmd.ps[1];
            string givenName = cmd.ps[2];

            KinematicsServer.groupNames.Add(groupName);

            GameObject groupObj = GameObject.Find(groupName);
            if (groupObj == null) groupObj = new GameObject(groupName);
            
            groupObj.transform.position = Vector3.zero;
            groupObj.transform.rotation = Quaternion.identity;

            Transform objectTransform = groupObj.transform.Find(givenName);
            if (objectTransform)
            {
                Debug.Log($"Object with name '{givenName}' already exists in stream '{groupName}', recreate.");
                GameObject.Destroy(objectTransform.gameObject);
            }

            PrimitiveObject obj = null;

            switch (primitiveType)
            {
                case "sphere":
                    obj = new Sphere(givenName, cmd.pf[0]);
                    break;
                case "capsule":
                    obj = new Capsule(givenName, cmd.pf[0], cmd.pf[1]);
                    break;
                case "box":
                    obj = new Box(givenName, cmd.pf[0], cmd.pf[1], cmd.pf[2]);
                    break;
            }

            Material material;
            if (KinematicsServer.RegisteredMaterialsMap.ContainsKey(groupName)) material = KinematicsServer.RegisteredMaterialsMap[groupName];
            else material = KinematicsServer.RegisteredMaterialsMap["_sys_default"];

            obj.GetGameObject().transform.SetParent(groupObj.transform);
            foreach (GameObject renderableObj in obj.GetRenderableObjects())
            {
                renderableObj.GetComponent<Renderer>().material = material;
                renderableObj.GetComponent<Renderer>().receiveShadows = false;
            }
        }
    }
}

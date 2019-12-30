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

        public void HandleCommand(Command cmd)
        {
            if (cmd.name.IndexOf("_sys_") == 0) HandleSystemCommand(cmd);
            else HandleCustomCommand(cmd);
        }

        private void HandleSystemCommand(Command cmd)
        {
            switch (cmd.name)
            {
                case "_sys_create_primitive":
                    CmdCreatePrimitive(cmd);
                    break; 
                case "_sys_physical_fps":
                    CmdPhysicalFPS(cmd);
                    break;
            }
        }

        private void CmdCreatePrimitive(Command cmd)
        {
            string primitiveType = cmd.ps[0];
            string groupName = cmd.ps[1];
            string givenName = cmd.ps[2];

            Controller.groupNames.Add(groupName);

            GameObject groupObj = GameObject.Find(groupName);
            if (groupObj == null) groupObj = new GameObject(groupName);
            
            groupObj.transform.position = Vector3.zero;
            groupObj.transform.rotation = Quaternion.identity;

            if (groupObj.transform.Find(givenName))
            {
                Debug.Log($"Object with name '{givenName}' already exists in stream '{groupName}', skip.");
                return;
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
            if (Controller.RegisteredMaterialsMap.ContainsKey(groupName)) material = Controller.RegisteredMaterialsMap[groupName];
            else material = Controller.RegisteredMaterialsMap["_sys_default"];

            obj.GetGameObject().transform.SetParent(groupObj.transform);
            foreach (GameObject renderableObj in obj.GetRenderableObjects())
            {
                renderableObj.GetComponent<Renderer>().material = material;
            }
        }

        private void CmdPhysicalFPS(Command cmd)
        {
            UI.PhysicalFPS = cmd.pi[0];
            UI.MaxPhysicalFPS = cmd.pi[1];
            UI.PhysicalPaused = cmd.pi[2];
        }
    }
}

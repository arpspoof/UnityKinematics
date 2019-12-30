using UnityEngine;

namespace UnityKinematics
{
    public static class CommandSender
    {
        public static void Send(Command cmd)
        {
            int code = UnityServerAPI.SendCommand(cmd);
            if (code == -1) 
            {
                Debug.LogWarning("Command failed, remote is not responding");
            }
        }
    }
}

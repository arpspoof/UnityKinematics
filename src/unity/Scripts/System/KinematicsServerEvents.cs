namespace UnityKinematics
{
    public static class KinematicsServerEvents
    {
        public delegate void ServerInitializeHandlerDelegate();
        public delegate void CommandHandlerDelegate(Command cmd);
        public delegate void SystemCommandHandlerDelegate(Command cmd);
        public delegate void BeforeNewFrameHandlerDelegate(int frameId);
        public delegate void AfterNewFrameHandlerDelegate(FrameState frame);

        public static event ServerInitializeHandlerDelegate OnServerInitialize;
        public static event CommandHandlerDelegate OnCommand;
        public static event SystemCommandHandlerDelegate OnSystemCommand;
        public static event BeforeNewFrameHandlerDelegate OnBeforeNewFrame;
        public static event AfterNewFrameHandlerDelegate OnAfterNewFrame;

        internal static void InvokeOnServerInitialize()
        {
            OnServerInitialize?.Invoke();
        }

        internal static void InvokeOnCommand(Command cmd)
        {
            OnCommand?.Invoke(cmd);
        }

        internal static void InvokeOnSystemCommand(Command cmd)
        {
            OnSystemCommand?.Invoke(cmd);
        }

        internal static void InvokeOnBeforeNewFrame(int frameId)
        {
            OnBeforeNewFrame?.Invoke(frameId);
        }

        internal static void InvokeOnAfterNewFrame(FrameState frame)
        {
            OnAfterNewFrame?.Invoke(frame);
        }
    }
}

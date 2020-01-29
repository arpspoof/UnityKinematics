### Server API List

#### Coordinate Transform
[Explanations for coordinate transform](CoordinateTransform.md)
```c#
namespace UnityKinematics
{
    public static class CoordinateTransform
    {
        public static Vector3 RightHandToLeftHand(Vector3 v);

        public static Quaternion RightHandToLeftHand(Quaternion q);
        
        public static Vector3 LeftHandToRightHand(Vector3 v);

        public static Quaternion LeftHandToRightHand(Quaternion q);
    }
}
```

#### Event System and Command System
[Explanations for event system](CustomPlugins.md#Server-Event-System)

[Explanations for command system](CommandSystem.md)
```C#
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
    }

    public static class CommandSender
    {
        public static void Send(Command cmd);
    }
}
```

#### Server Functionalities
```C#
namespace UnityKinematics
{
    public class KinematicsServer : MonoBehaviour
    {
        // Get the instance of KinematicsServer in current Unity project.
        public static KinematicsServer instance;

        // Poll and execute all pending unprocessed incoming commands. If
        // no new commands available, this function does nothing.
        public void ExecutePendingCommands();
    }
}
```

#### Offline Rendering
[Explanations for offline rendering](OfflineRendering.md)
```C#
namespace UnityKinematics
{
    public class DataRecorder : MonoBehaviour
    {
        // Initialize the data recorder.
        // Online data will be automatically captured.
        public void StartRecording();

        // Write the accumulated data to specified file.
        // Please specify file name and other properties in Unity Inspector.
        public void StopRecordingAndWriteToFile();

        // Manually record one data frame.
        public void RecordFrame(FrameState frame);

        // Manually record one command.
        public void RecordCommand(Command cmd);
    }
```

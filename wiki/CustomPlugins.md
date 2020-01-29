## Custom Plugins

In this project, we provided only limited amount of render plugins and system plugins. If you need to take advantage of the data transmission ability of the system to achieve more advanced visual effects or system control, please go through this tutorial and [server API](ServerAPI.md) list. 

### Prerequisites
+ If your desired plugin does not depend on any data transmission related functionality of this project, this is then a pure visual effect plugin. Checkout Unity tutorials instead.
+ If you need to transmit custom data from / to either side, please go through [command system](CommandSystem.md) page first.
+ If you need to transmit coordinates, please also go through [coordinate transform](CoordinateTransform.md) page first.
+ A fairly good understanding of C# and Unity is recommended. 

### Server Event System
If the system, we will raise certain C# events at certain key points. Your plugin may listen to these events and react correspondingly. Full list of events are listed in the following:
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
}
```
+ OnServerInitialize
  + Called when the server initialize everything. No data or command can be received before this point.
  + No arguments.
+ OnCommand
  + Called when a new **custom** command is available. Internal system commands are excluded.
  + Single argument *cmd* : The received custom command.
+ OnSystemCommand
  + Called when a internal system command is received. The system command has already been processed. Use this event for further post processing. Custom commands are excluded.
  + Single argument *cmd* : The processed system command.
+ OnBeforeNewFrame
  + Called when a new frame of data is coming. This frame has not been processed at this point.
  + Single argument *frameId* : The integer id of this frame.
+ OnAfterNewFrame
  + Called after a new frame of data has been processed. The frame is already processed internally at this point. 
  + Single argument *frame* : The data frame.

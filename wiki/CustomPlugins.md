## Custom Plugins

In this project, we provided only limited amount of render plugins and system plugins. If you need to take advantage of the data transmission ability of the system to achieve more advanced visual effects or system control, please go through this tutorial and [server API](ServerAPI.md) list. 

### Prerequisites
+ If your desired plugin does not depend on any data transmission related functionality of this project, this is then a pure visual effect plugin. Checkout Unity tutorials instead.
+ If you need to transmit custom data from / to either side, please go through [command system](CommandSystem.md) page first.
+ If you need to transmit coordinates, please also go through [coordinate transform](CoordinateTransform.md) page first.
+ A fairly good understanding of C# and Unity is recommended. 

### Server Event System
In the system, we will raise certain C# events at certain key points. Your plugin may listen to these events and react correspondingly. Full list of events are listed in the following:
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

### A Concrete Example
Here we show a concrete example of creating and using a custom plugin. This plugin is a client side controlled line renderer which can draw line segments according to client specified data points. The workflow is: 
1. Client send a set of points to the server via custom command.
2. Server receive the set of points and render them as line segments through line renderer.

#### Writing the plugin
```C#
using System.Collections.Generic;
using UnityEngine;
using UnityKinematics;

[RequireComponent(typeof(LineRenderer))]
public class ClientControllerLineRenderer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // In initialization stage, listen to the custom command event.
        KinematicsServerEvents.OnCommand += OnCommand;
    }

    // Custom command handler
    void OnCommand(Command cmd)
    {
        // Check if the name of the command is the one we want.
        // Note that client and server have to agree on the same command name.
        if (cmd.name == "client-controlled-line-renderer")
        {
            // All data points (coordinates) are stored in the floating point parameter array.
            for (int i = 0; i < cmd.pf.Count; i += 3)
            {
                // Extract point coordinate.
                Vector3 v = new Vector3(cmd.pf[i], cmd.pf[i + 1], cmd.pf[i + 2]);
                // Coordinate transform.
                v = CoordinateTransform.RightHandToLeftHand(v);
                
                // Add the data point to the renderer.
                var line = GetComponent<LineRenderer>();
                line.positionCount++;
                line.SetPosition(line.positionCount - 1, v);
            }
        }
    }
}
```

#### Using the plugin
Simply drag this script to a Unity GameObject with a LineRenderer already attached to it. Please also tune rendering parameters for LineRenderer before usage.

#### Client side usage
Add the following code somewhere after client initialization.
```python
# Command name should match.
cmd = Command("client-controlled-line-renderer")

# Put all the point coordinated inside the floating point parameter array.
# Use regular right-handed coordinate system here.
for i in range(num_points):
    cmd.pf.push_back(points_array[i][0])
    cmd.pf.push_back(points_array[i][1])
    cmd.pf.push_back(points_array[i][2])

SendCustomCommand(cmd)
```


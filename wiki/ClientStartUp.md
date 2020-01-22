### Some Concepts
+ **Object State**: As mentioned in the previous section, an ObjectState is the state of an object in a specific frame. We describe and ObjectState by the name of the object as well as the position and quaternion of the object. 
+ **Group State**: A group is a set of related objects. State of a group is just combination of states of all objects in this group. A group has a group name. In Unity server side, each group will correspond to a top level dummy object. All objects inside the group will be children of the dummy group object. 
  + **Note**: Better use different names for objects even inside different groups to avoid Unity side confusions.
+ **Frame State**: The scene may contain multiple groups. In each frame, we need to specify states of each group inside the scene. Therefore, state of a frame is just combination of all group states in the scene. A frame does not have a name.
+ **Data Provider**: A data provider is a general agent for constantly producing physical frames. Each frame is described by a FrameState structure (previous item). A frame has a physical time duration, called the _time step_. Suppose the time step for each frame is 1/600 second, then we'll have 600 data frames per physical second. 

### A Quick Python Client Tutorial
In this tutorial, we'll do something really simple. We'll create a cube in the scene and move it constantly towards one specific direction. Before we actually start, create a python file called pytest.py and put it in the build folder containing python client API. This folder is called 'pyUnityRenderer' by default. Our first line of code is
```python
from pyUnityRenderer import *
```
#### Establish a data provider
In the API provided, we have defined a class called ```AbstractDataProvider```. It has only one abstract function to be implemented, namely ```GetCurrentState```. This function is used as a call back function when the system needs to send the kinematic data to the server. Therefore, in this function, we need to provide what the frame looks like. In other words, we need to construct and return a ```FrameState``` Object. Here we should create a class called ```DataProvider``` which will inherit from ```AbstractDataProvider```. 
```python
class DataProvider(AbstractDataProvider):
    def __init__(self):
        AbstractDataProvider.__init__(self)
        self.x = 0

    def GetCurrentState(self):
        frameState = FrameState()
        frameState.groups.push_back(GroupState("ref"))
        firstGroup = frameState.groups[0]
        firstGroup.objectStates.push_back(ObjectState("root", self.x, 2, self.x, 1, 0, 0, 0))
        return frameState

dataProvider = DataProvider()
```
In the override ```GetCurrentState``` function, we first create a ```FrameState``` instance. Then we need to create a group inside the frame. We shall call this group "ref" here. To create a group, we just need to call ```frameState.groups.push_back``` (C++ style). Then we put an object inside this group. We get the just created group via ```frameState.groups[0]``` and then create an instance of ```ObjectState``` inside it by calling ```firstGroup.objectStates.push_back```. The constructor of ObjectState takes 8 arguments. The first one is the name. Here we call it "root" because by default our camera will look at object with name "root". The second to forth argument correspond to the global position of the object. And the last four arguments correspond to the global quaternion (w, x, y, z). In this example, we use ```self.x``` to create some simple moving effects. If we increment ```self.x```, we are actually changing the global position of the object. If you work on simulators, these quantities should be able to be directly queried every frame.

#### Establish a command handler
We haven't introduce the command system and what is a command handler yet. Please look up for these material in page [System Extensions](https://github.com/arpspoof/UnityKinematics/wiki/System-Extensions)
. In fact, here we just need a dummy one.
```python
class CommandHandler(AbstractCommandHandler):
    def HandleCommand(self, cmd):
        pass

commandHandler = CommandHandler()
```

#### Initialize the client
We should always initialize the client before performing any actual operations. This must be done via the ```InitRenderController``` function. This function takes 7 arguments:
+ _serverAddr_: IP address of the remote server.
+ _serverPort_: Running port of the remote server.
+ _localAddr_: IP address of this local machine. This IP address should be accessible by the remote server. If the client and the server are in different LANs, this will be a little bit complicated but in general, techniques like upnp port forwarding will help. If indeed this address is not accessible by the server, some functionalities like FPS control will not work. 
+ _commandHandlingPort_: Specify an arbitrary available port on this local machine for remote command handling. If this is not set up correctly, some functionalities like FPS control will not work.
+ _maxPhysicalFPS_: This specifies how many physical frames can have in one physical second. If the simulation is running in constant time step, just set this to ```(int)(1.0 / timeStep)``` (it has to be an integer). 
+ _dataProvider_: An instance to a data provider.
+ _commandHandler_: An instance to a command handler. 
A sample call looks like:
```python
InitRenderController("localhost", 8080, "localhost", 8081, 1000, dataProvider, commandHandler)
```

#### Create objects in the scene (if needed)
There are mainly two ways of determining how your objects will look like in the scene. 
+ **Option 1**: For simple debugging purpose, we just use primitive objects (box, capsule, sphere). We need to tell the server sizes of these objects by calling ```CreatePrimitive``` function. No actions needed on server side, the server will automatically create these objects for you.
+ **Option 2**: For fancy rendering purpose, we can also use skinned fancy 3d models. We need to manually create these objects in server side, give them with correct names, and put them into correct groups. No actions needed on client side. We just need to tell the server transform data for these objects in each frame. Be careful that group names and object names must match for things to work.

We will use **Option 1** here. We will only create one object, our cube, with name "root", like this:
```python 
CreatePrimitive("box", "ref", "root", 0.5, 1, 1.5)
```
In general, ```CreatePrimitive``` function takes 6 parameters:
+ _type_: "box", "sphere", or "capsule".
+ _groupName_: Specify which group shall we put the object in.
+ _objectName_: Give a name to the object.
+ _param0_: 
  + Length along x-axis for box.
  + Radius for sphere.
  + Radius for capsule.
+ _param1_:
  + Length along y-axis for box.
  + Unused for sphere.
  + Length for capsule.
+ _param2_:
  + Length along z-axis for box.
  + Unused for sphere.
  + Unused for capsule.

#### Driving the time forward
The client side should constantly producing physical frames. Whenever a physical frame is ready, we should call ```Tick``` immediately. It takes only one argument which is the physical time step of the frame. In our example:
```python
while True:
    dataProvider.x += 0.001
    Tick(0.001)
```
Our physical state is just the 'x' variable. So we keep increasing x and call ```Tick``` to indicate our physical frame is ready. Note that we have flow control technique in place so we don't need to worry that the loop will overflow the server. Actually, we are required to call ```Tick``` for each physical frame but only some of them will be rendered. This is determined by the frequency control technique. Play around the client and the server and you'll find how it works.

#### Final notes
+ Always start your server before running client. This is a small project and we don't have time for more sophisticated network error handling...
+ Make sure you call ```Tick``` for each physical frame, otherwise some features will work randomly.

### Notes on C++ Client
Basically, there's no difference with usage of these API functions except language differences. The concepts are the same. However, there are some extra steps needed to set up a C++ client. 
#### Get the API headers and static libraries
After building this project, you should run ```make install``` to generate these headers and libraries. In the install folder, you'll see two folders, ```include``` and ```lib```. You'll need to add the ```lib``` directory to your compiler linking library path, and link library with name ```client``` to your project. The include directory should be ```[install]/include/KinematicsClient```. Add this path to your compiler include directory definition. In your code, just put this statement to access all API function:
```c++
#include <KinematicsClient.hpp>
```
#### Link rpclib to your client project
In general, you also need to link rpclib to your project. You can refer to our build instructions on how to get the rpclib libraries.
#### Side notes
Server headers and libraries are not used unless you would like to develop C++ rendering server yourself. This is generally unwise since Unity can already get the job done really straightforwardly. 

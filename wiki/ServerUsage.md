
### Prerequisite
+ Unity Editor for Linux (Please upgrade to newest version, at least 2019.2)

### Setup
+ Create an empty unity project (3d)
+ Copy every thing in the ```Assets``` folder generated from build process to the ```Assets``` folder of the unity project
+ Open Unity Editor
+ Delete the default 'Sample Scene' and create a new one. (Don't ignore this step)
+ Create and empty GameObject, rename it to 'Controller' (does not matter)
+ Drag script ```Controller.cs``` from Scripts folder to this object
+ Now the Unity rendering server is ready to use!

### Building Unity Binaries
+ Please make sure x86_64 is selected in build settings otherwise things may not work

### Basic Usage
#### Start and stop
You can click on the Play button to start the server and click again to stop it. You can also build the scene to binary files to get higher performance. Note that the server will run on some specific network ports, please make sure you do not start two servers on the same port.
#### Configure the server network port
This is very easy. In unity editor, there's an option in the 'Controller' object for that. Default is 8080.
#### Camera tracking system
By default, we have a spherical camera tracking system. The camera will always stay on the surface of a sphere and pointing to the tracked object in the scene. By default, the tracked object should have name **'root'**. X and Z coordinate of the center of the sphere is determined by the tracked object. The Y coordinate is configurable by keys ```LeftCtrl```(Lower) and ```LeftAlt```(Higher). The radius of the sphere is controlled by keys ```LeftShift```(Farther) and ```F``` (Nearer). To move the camera on the sphere surface, use keys ```W``` and ```S``` to adjust the latitude and ```A``` and ```D``` to adjust the longitude. You can also adjust the horizontal camera tracking offset by keys ```I```(forward), ```K```(backward), ```J```(leftward) and ```L```(rightward).
#### FPS Control
In the top left corner of the screen, you can see something like 'Render: 60 RF/s' which is the current rendering frequency. You can adjust this by keys ```ArrowUp```(Increase) and ```ArrowDown```(Decrease). 

When client side data is available, you can also see something like 'Physical: 60/3000 PF/s' which is the physical data frequency. Here 3000 means one physical second will correspond to 3000 physical frames. And 60 means that only 60 of the total 3000 physical frames will be rendered (uniformly spaced selected frames). If more than 60 physical frames are rendered per physical second, the animation will look 'slowed down' or 'more detailed' like. You can adjust this by keys ```ArrowRight```(Increase, slow down) and ```ArrowLeft```(Decrease, speed up). You can perform more aggressive adjustments by holding ```RightCtrl``` at the same time.

You can also pause the animation when client side data is available by pressing key ```Space```. 

**Note**: FPS control involves network interaction between the server and the client, if there is no client please do not use these buttons otherwise the server will crash. If the client has disconnected, these buttons have no effects but will slow down the server for a while. 

#### Show / hide UI
If you do not want to see the UI, you can press key ```U``` to disable it. You can press again to re-enable it.

#### Clear the scene
If you want to remove everything left in the scene, you can press key ```R```. This will remove all client side data objects. 

**Note**: Do not use this when client is still sending data, just waste bandwidth...

### Customize Your Server

#### Change material / texture for primitives
If you don't like the wood default style primitive objects, you can definitely create your own style. You first need to register your own materials in 'Controller' object. There's a property called 'Registered Materials' there for you to register the materials. For each custom material, give a name, and then drag the material to the slot. Next, objects using custom material with name X should be under group with the exact same name X. You need to create the dummy group container yourself from Unity Editor. Then any primitive objects created under this group will use your selected material.

#### Skinning
If you don't like primitive objects, you can use your complete skinned 3d model. You can treat the model as a group with objects with different names in it. Your client just need to tell you the **GLOBAL** transform of each individual object of your model. Then things will work as you expect. 

### Server Behind NAT
If your server is behind a NAT (usually a router) and wants to communicate with clients outside the NAT, you will need to configure upnp port forwarding on your router. A upnp port forwarding maps a port along with your public IP address to your local IP address and port. Your client needs to know the public IP and port. 

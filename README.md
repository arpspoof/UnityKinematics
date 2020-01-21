# Unity Kinematics

### Preview
![Screenshot from 2019-12-29 21-04-43](https://user-images.githubusercontent.com/37004963/71568834-1339f500-2a7f-11ea-84c3-a84638a1c3f1.png)
Side note: Unity does not have native capsule support, this is the only drawback I can think of...

### Overview
This project aims to build an easy to use and powerful renderer for all kinematic data provider (including simulators or even bvh players). We simply pass the kinematic state through the network and let Unity do the rendering job. 

### Cool Feature List
+ Full view spherical tracking camera with keyboard interactions
+ Primitive object creation 
+ Easy to change texture or material
+ Dual mode speed control / Pausing
+ Multiple data stream handling
+ Client side python binding

### How it Works
#### Idea Behind the Architecture
Most recent physics simulators do not support fancy rendering because that's not their native job. The simulators are just there to provide data, kinematic data. To render the scene, we simply need to grab the kinematic data and use something to draw them.

To make things generic and extensible, we will not change anything in the simulators. Instead, we create a generic kinematic data consumer, or server, to receive and render data from all possible simulators (even other kinds of data providers). We shall call these simulators or data providers, client. 

So in a big picture, our design idea is the following: Client sends kinematic data through network. This is just indeed a set of object states (position + quaternion). When the server receives the data, it sees that object with name 'abc' is in a specific kinematic state (position + quaternion), it creates that object (if not exist) and set the transform of that object correspondingly. 

### Wiki Pages
1. [Build Instrutions](wiki/Build.md)
2. [Client Side Start Up](wiki/ClientStartUp.md)
3. [Client Side API List](wiki/ClientAPI.md)
4. [Server Side Usage](wiki/ServerUsage.md)
5. [System Extensions](wiki/SystemExtensions.md)
6. [Video Recording](wiki/VideoRecording.md)

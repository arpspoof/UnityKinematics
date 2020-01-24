# Get Started

### Wiki pages
1. [Build Instrutions](Build.md)
2. [Client Side Start Up](ClientStartUp.md)
3. [Client Side API List](ClientAPI.md)
4. [Server Side Usage](ServerUsage.md)
5. [Command System](CommandSystem.md)
6. [Video Recording](VideoRecording.md)

### You may consider this project if ...
+ You think the renderer for your physics simulator is too ugly, almost unsuitable for daily developing and debugging. 
+ You think the renderer for your physics simulator is fair, however, it's too difficult to customize, change settings, or add your own stuff.
+ You want a clear view of various type of motion data, when the motions are applied on a specific character model, possibly skinned.
+ You want high quality 3d images and videos rendered for your animation / graphics paper.

### What does this project do
+ Render any rigid motions, in Unity.
+ Provide a highly customizable framework suitable for various motion rendering tasks.

### How it works
+ A program, called a *client*, constantly sends motion data in a predefined format to Unity (which we call it the *server*).
+ The Unity server creates 3d objects and moves objects around according to the motion data.
+ Motion data transmission is done via networks, allowing rendering data from remote providers.

### Prerequisites
+ A fairly powerful desktop / laptop (Windows / Linux) which is capable of running Unity fluently. 
+ Basic knowledge of Python *or* C++ for client side data provider programming.
+ (*Optional*) Basic knowledge of Unity and C# for server side customizing.

### Step by step tutorials
[This tutorial for beginners](TutorialBeginner.md) will go through all the essential steps to build up your own rendering project.

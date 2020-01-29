# Get Started

### Wiki pages

#### Tutorials
0. [Build Instructions](Build.md)
1. [Tutorial for beginner](TutorialBeginner.md)
2. [Tutorial on basic visual effects](TutorialVisualEffects.md)
3. [Video Recording](VideoRecording.md)

#### System Description
1. [Coordinate transform](CoordinateTransform.md)
2. [Render plugins](DefaultRenderPlugins.md)
3. [Keyboard Shortcuts](KeyboardShortcuts.md)
4. [Using C++ client](CppClient.md)

#### Advanced
1. [Offline rendering](OfflineRendering.md)
2. [Command system](CommandSystem.md)
3. [Custom plugins](CustomPlugins.md)

#### API List
1. [Client API list](ClientAPI.md)
2. [Server API list](ServerAPI.md)

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
[This tutorial for beginners](TutorialBeginner.md) will go through all the essential steps to build up your own rendering project. You'll probably need to add more visual effects and rendering components to the Unity project. Checkout [this tutorial on visual effects](TutorialVisualEffects.md) for more step by step instructions.

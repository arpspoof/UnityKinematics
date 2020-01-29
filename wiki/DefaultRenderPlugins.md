# Render Plugins

## Introduction
Breifly, a render plugin is a C# script to be applied on a Unity GameObject to produce certain visual effects, while depending on the system's ability to transmit data from client to server. If the script does not depend on any client side data transmission, it's then a pure visual effect script. Pure visual effect scripts have nothing to do with this project. They can live in Unity in absence of our system. 

This page will introduce all default render plugins provided. To learn how to write a custom render plugin, please refer to [custom render plugins](CustomPlugins.md) page.

## Prerequisite
+ A Unity project with ```KinematicsServer``` script attached to a GameObject with presumed name 'Controller'.
+ Going through [beginner's tutorial](TutorialBeginner.md) is recommended.

## Default Render Plugins

In this section, we introduce all default render plugins. Each plugin is a C# script. To enable the plugin, attach the script to the appropriate GameObject in Unity. Here 'appropriate' means the GameObject should already contain the 'Required component' for the plugin. 

A plugin (script) may have options all parameters to be set up. When a script is attached to an object, you will be able to see and adjust the parameters in the Unity **Inspector**. 

Some plugins have default keyboard shortcuts. You can change these though *Controller->Shortcut Settings*.

**Very Important Note**: You can adjust parameters in both edit mode and play mode. Changes in edit mode are preserved. Make sure you **save the scene** frequently so that the changes won't be lost when Unity crashes (it happens a lot on Linux) or when your custom code accidentally runs into infinite loop. **Changes in play mode are discarded when exiting play mode**. It is sometimes more convenient to tune parameters in play mode. If you want to keep the changes, make sure you remember all parameters by pencil and paper (sounds stupid), or copy the component in play mode and paste it in edit mode. 

----
### Camera tracking

+ Script: RenderPlugins/Camera/CameraTracking.cs
+ Required component: Camera

#### Introduction
This is a spherical camera tracking system. The camera will always stay on the surface of a sphere and pointing to the tracked object in the scene. 

#### Parameters
+ *Tracking Name* : Name of the object being tracked. If two more objects in the scene have this name, the first one is chosen.
+ *Sphere Center Offset*:  Partially determines the center position of the sphere. Suppose the tracked object is in position (x0, y0, z0), the center of the sphere will be (x0, 0, z0) + 'Sphere Center Offset'. 
+ *Sphere Radius* : Radius of the sphere. (Far / Near)
+ *Longitude* : Determines the camera position on the sphere surface.
+ *Latitude* : Determines the camera position on the sphere surface.
+ *\*\* Adjustment Step* : Determines how fast a parameter is changed when using keyboard shortcuts.

#### Shortcuts 
+ *Camera Longitude -* : A
+ *Camera Longitude +* : D
+ *Camera Latitude +* : W
+ *Camera Latitude -* : S
+ *Camera Radius +* : Left Shift
+ *Camera Radius -* : F
+ *Camera Center Upward* : Left Alt
+ *Camera Center Downward* : Left Control
+ *Camera Center Forward* : I
+ *Camera Center Backward* : K
+ *Camera Center Leftward* : J
+ *Camera Center Rightward* : L

----
### FPS text box

+ Script: RenderPlugins/UI/FPSTextBox.cs
+ Required component: Canvas

#### Introduction
A text box in the top left corner showing rendering and physical FPS. Please refer to [FPS control](KeyboardShortcuts.md#FPS-Control) page for more details about FPS control. This is useful to visualize play speed of the motion. This may be also useful for performance debugging.

#### Parameters
+ *Show UI* : Show / hide the text box.

#### Shortcuts
+ *Show / Hide UI* : U

----
### Trajectory Marker

+ Script: RenderPlugins/Visuals/TrajectoryMarker.cs
+ Required component: LineRenderer

#### Introduction
A trajectory marker can mark the movement of some object by 3d line segments (can approximate curves when control points are closed to each other). Line segments are created dynamically while the object is moving. 

#### Parameters
+ *Frames Per Marker* : Determine the frequency of creating line segment control points while the object is moving. One control point is created for every 'Frames Per Marker' data frames. Set this to one to achieve the smoothest  curve.
+  *Tracked Object Name* : Name of the GameObject of which trajectory will be tracked. 

#### Notes
You may want to tune parameters for Line Renderer component first. You may want to change line width, line textures, lighting effects, etc.

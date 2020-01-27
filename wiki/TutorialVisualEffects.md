## Step by step tutorial - Visual Effects

### Prerequisite
After finishing [the first tutorial](TutorialBeginner.md), you finally get a moving cube. However, the scene is ugly. In this page, we'll go through some basic visual effects including static objects, lighting, background adjustment and camera tracking. These will be roughly enough for daily debugging purpose and some simple formal rendering requirement. 

----
### Step 1: Add a Floor
A floor is a static object in the Unity scene. This is for purely visualization purpose which has really nothing to do with this project. You can do this yourself if you have reasonably amount of knowledge on Unity.

+ In **Hierarchy** window, right click the empty space and create a 3d object, plane.
+ In **Inspector** window, reset the transform if it's not at the origin. Then change the scale to (200, 200, 200).
+ In **Project** window, right click the empty space and create a material in a directory of your choice. There are several options for the material in inspector window to be changed. 
+ Choose a texture. We have provided some sample textures in *Resources* folder. Drag anyone you like into the box before 'Albedo' field in the inspector.
+ Tune surface properties. Play around with *Metalic* and *Smoothness* value of the meterial. This will generate various metalic surface effects.
+ Change texture tiling. In inspector of the material, change tiling to (100, 100).
+ Finally, drag the created material into empty space of the inspector window of the plane.

Since this is not a Unity tutorial, we won't go though anything like this in future. Please look up relevant Unity tutorials on the Internet.

----
### Step 2: Background Effects
The giant checker board floor is likely to cause aliasing. We can work around this by changing backgound style. Again, this is pure Unity side visual effects which has nothing to do with this project. If you are a Unity  expert, you can add your effects yourself. Here we use a script to modify relevant parameters conveniently.

+ Drag *Scripts/VisualEffects/Background.cs* into Main Camera GameObject.
+ In inspector, there are several parameters to be set.
  + *Camera Clear Flags* : Here we change from skybox to solid color.
  + *Background Color* : Changing it to white or near white is probabaly an option to start from.
  + *Fog Enabled* : Please enable it.
  + *Fog Density* : 0.02 or 0.03 is good. 

Till this point, you should probably see something nicer, with much less aliasing effect. A sample screenshot likes this:
![Screenshot from 2020-01-27 17-08-14](https://user-images.githubusercontent.com/37004963/73227217-a79f8200-4127-11ea-87fe-b242b131b097.png)

----
### Step 3: Lighting [Deprecated]
This is again a pure Unity side visual effects which has nothing to do with this project. I have no professional experience on light settings. If you don't have either, you can try out the lighting script provided.

+ Delete default 'Directional Light'.
+ Drag *Scripts/VisualEffects/SurroundingLights.cs* into an empty GameObject. This is a script which creates whole bunch of directional lights surrounding to any object. This can create some 'fake' bright style lighting effects on characters. Also, a 60 degree sun light is used to generate shadows. There's also an optional downward light which may be used to brighten the floor.
+ In inspector, there are several parameters to be set.
  + *Sun Light Shadow Type* : Recommended one is 'Soft'.
  + *N Surrounding Lights* : Number of directional lights surrounding the character.
  + *Surrounding Intensity* : Intensity of the surrounding lights.
  + *Surrounding Offset Y* : By default, the surrounding lights are horizontal. Adjust this to make the surrounding lights pointing upward or downward.
  + *Downward Intensity* : Intensity of the downward light. Mostly unused.
  + *Sun Light Intensity* : Intensity of the sun light.

----
### Step 4: Camera tracking
We have prepared a spherical camera tracking script. It's usage is introduced in [render plugins](DefaultRenderPlugins.md) page. You may also apply other render plugins according to your requirement. 

----
### Step 5: Quality Settings
If you would like render the scene to images or videos, please use the highest quality settings. All quality settings are availabe through *Edit -> Project Settings -> Quality*. Some basic settings are:
+ *Pixel Light Count* : The number of lights calculated per pixel. Other lights are simply approximated. Set this to higher value to achieve best lighting effect. This may affect performance.
+ *Anti Aliasing* : Change to *8x Multi Sampling* when rendering images or videos. 
+ *Shadow Resolution* : This can be changed to 'Very High'. 

If you need extra fancy images or videos, checkout Unity's *Post Processing* package. This package includes functionalities like depth of field, advanced anti-aliasing techniques, and more. Checkout the introduction tutorial [here](https://www.youtube.com/watch?v=a0OQvWAPeuo).

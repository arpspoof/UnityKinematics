## Notes on Coordinate Transforms

#### Coordinate system in Unity
Unity uses a left-handed coordinate system. Unity defines the positive z axis to be the forward direction, positive y axis to be the upward direction and positive x axis to be the positive rightward direction. 

#### Coordinate system used in client API
In this project, we assume a right-handed coordinate system which is normal for most simulators. We define the transform between this coordinate system and Unity's left-handed system by flipping the z axis.

In ```GetCurrentState``` function in ```AbstractDataProvider```, please specify the object transform (position + quaternion) using the right-handed coordinate system with y axis pointing upwards. If you need to send custom coordinates to be handled in Unity via custom ```Command```, you need to handle coordinate transforms yourself. We have provided conversion function in Unity for convenience. The prototypes are:
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
```RightHandToLeftHand``` transforms from the right-handed system used in client API to Unity left-handed system. ```LeftHandToRightHand``` does the converse. A little bit math shows these functions are the same. 

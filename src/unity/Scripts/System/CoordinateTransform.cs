using UnityEngine;

namespace UnityKinematics
{
    public static class CoordinateTransform
    {
        public static Vector3 RightHandToLeftHand(Vector3 v)
        {
            return new Vector3(v.x, v.y, -v.z);
        }

        public static Quaternion RightHandToLeftHand(Quaternion q)
        {
            Vector3 axis; 
            float angle;
            q.ToAngleAxis(out angle, out axis);
            Vector3 newAxis = new Vector3(axis.x, axis.y, -axis.z);
            return Quaternion.AngleAxis(-angle, newAxis);
        }
        
        public static Vector3 LeftHandToRightHand(Vector3 v)
        {
            return RightHandToLeftHand(v);
        }

        public static Quaternion LeftHandToRightHand(Quaternion q)
        {
            return RightHandToLeftHand(q);
        }
    }
}

using UnityEngine;
using System.Collections.Generic;

namespace UnityKinematics
{
    public abstract class PrimitiveObject
    {
        protected GameObject obj;
        public GameObject GetGameObject() => obj;
        public virtual IEnumerable<GameObject> GetRenderableObjects() => new List<GameObject> { obj };
    }

    public class Sphere : PrimitiveObject
    {
        private float radius;

        public float Radius 
        {
            get => radius;
            set { radius = value; UpdateTransform(); }
        }

        public Sphere(string name, float radius)
        {
            this.radius = radius;
            obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            obj.name = name;
            UpdateTransform();
        }

        public void UpdateTransform()
        {
            obj.transform.localScale = new Vector3(radius * 2, radius * 2, radius * 2);
        }
    }

    public class Capsule : PrimitiveObject
    {
        private float radius, length;
        private GameObject sphereTop, sphereBottom, cylinder;

        public float Radius 
        {
            get => radius;
            set { radius = value; UpdateTransform(); }
        }

        public float Length 
        {
            get => length;
            set { length = value; UpdateTransform(); }
        }

        public Capsule(string name, float radius, float length)
        {
            this.radius = radius;
            this.length = length;

            obj = new GameObject(name);
            obj.name = name;
            
            sphereTop = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphereBottom = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

            sphereTop.transform.SetParent(obj.transform);
            sphereBottom.transform.SetParent(obj.transform);
            cylinder.transform.SetParent(obj.transform);

            UpdateTransform();
        }

        private void UpdateTransform()
        {
            cylinder.transform.localScale = new Vector3(radius * 2, length / 2, radius * 2);

            sphereTop.transform.localScale = new Vector3(radius * 2, radius * 2, radius * 2);
            sphereTop.transform.localPosition = new Vector3(0, length / 2, 0);

            sphereBottom.transform.localScale = new Vector3(radius * 2, radius * 2, radius * 2);
            sphereBottom.transform.localPosition = new Vector3(0, -length / 2, 0);
        }

        public override IEnumerable<GameObject> GetRenderableObjects()
        {
            List<GameObject> list = new List<GameObject>();
            list.Add(cylinder);
            list.Add(sphereTop);
            list.Add(sphereBottom);
            return list;
        }
    }

    public class Box : PrimitiveObject
    {
        private float lengthX, lengthY, lengthZ;

        public float LengthX 
        {
            get => lengthX;
            set { lengthX = value; UpdateTransform(); }
        }

        public float LengthY 
        {
            get => lengthY;
            set { lengthY = value; UpdateTransform(); }
        }

        public float LengthZ
        {
            get => lengthZ;
            set { lengthZ = value; UpdateTransform(); }
        }

        public Box(string name, float lengthX, float lengthY, float lengthZ)
        {
            this.lengthX = lengthX;
            this.lengthY = lengthY;
            this.lengthZ = lengthZ;
            obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obj.name = name;
            UpdateTransform();
        }

        public void UpdateTransform()
        {
            obj.transform.localScale = new Vector3(lengthX, lengthY, lengthZ);
        }
    }
}

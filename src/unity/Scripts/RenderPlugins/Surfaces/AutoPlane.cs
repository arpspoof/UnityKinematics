using UnityEngine;

namespace UnityKinematics
{
    public class AutoPlane : MonoBehaviour
    {
        public Texture2D texture = null;
        public string dynamicTextureName = "";
        public float scale = 200;
        public float textureScale = 100;
        public float metallic = 0.252f;
        public float smoothness = 0.1f;

        void Start()
        {
            if (texture == null)
            {
                texture = (Texture2D)Resources.Load(dynamicTextureName);
            }
        }

        void Update()
        {
            transform.localScale = new Vector3(scale, scale, scale);

            var renderer = GetComponent<Renderer>();
            renderer.material.mainTexture = texture;
            renderer.material.mainTextureScale = new Vector2(textureScale, textureScale);
            renderer.material.SetFloat("_Metallic", metallic);
            renderer.material.SetFloat("_Glossiness", smoothness);
        }
    }
}

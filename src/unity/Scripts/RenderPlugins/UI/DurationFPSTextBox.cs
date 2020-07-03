using UnityEngine;
using UnityEngine.UI;

namespace UnityKinematics
{
    [RequireComponent(typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster))]
    public class DurationFPSTextBox : MonoBehaviour
    {
        private static GameObject FPSTextObj;
        private static Font defaultFont;

        void Start()
        {
            defaultFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");

            FPSTextObj = new GameObject("txt", typeof(RectTransform));
            InitTextbox(FPSTextObj, new Color(0f, 0f, 0f));
        }

        void Update()
        {
            var server = KinematicsServer.instance;
            var s = "s";
            if (server.nModels == 1) s = "";
            string txt = $"{(int)(server.AccumulatedFrames / server.AccumulatedFrameDuration)} FPS, {server.nModels} character{s}";
            FPSTextObj.GetComponent<Text>().text = txt;
        }

        private void InitTextbox(GameObject obj, Color color)
        {
            Text txt = obj.AddComponent<Text>();
            obj.transform.SetParent(transform);

            txt.color = color;
            txt.font = defaultFont;
            txt.fontStyle = FontStyle.Normal;
            txt.fontSize = 56;
            txt.alignment = TextAnchor.UpperCenter;

            RectTransform trans = txt.GetComponent<RectTransform>();
            trans.anchorMin = new Vector2(0.01f, 0.99f);
            trans.anchorMax = new Vector2(0, 0.99f);
            trans.pivot = new Vector2(0, 0);
            trans.sizeDelta = new Vector2(1080, 100);
            trans.anchoredPosition = new Vector2(0, -90);
        }
    }
}

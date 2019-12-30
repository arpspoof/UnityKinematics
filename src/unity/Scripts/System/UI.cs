using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

namespace UnityKinematics
{
    public static class UI
    {
        public static bool ShowUI { get; set; }
        public static int PhysicalFPS { get; set; } = 0;
        public static int MaxPhysicalFPS { get; set; } = 0;
        public static int PhysicalPaused { get; set; } = 0;

        private static GameObject canvasObj;
        private static GameObject physicalFPSTextObj;
        private static GameObject renderFPSTextObj;
        private static Font defaultFont;

        public static void InitUI()
        {
            ShowUI = true;

            canvasObj = new GameObject("Canvas");

            Canvas canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();

            var eventSystemObj = new GameObject("EventSystem");
            eventSystemObj.AddComponent<EventSystem>();
            eventSystemObj.AddComponent<StandaloneInputModule>();
            eventSystemObj.AddComponent<BaseInput>();

            defaultFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");

            renderFPSTextObj = new GameObject("txt___render", typeof(RectTransform));
            InitTextbox(renderFPSTextObj, new Color(0.7f, 1, 0.9f));

            physicalFPSTextObj = new GameObject("txt___physical", typeof(RectTransform));
            InitTextbox(physicalFPSTextObj, new Color(1, 0.9f, 0.8f));
        }

        public static void UpdateUI()
        {
            if (!ShowUI)
            {
                canvasObj.SetActive(false);
                return;
            }
            else if (!canvasObj.activeSelf)
            {
                canvasObj.SetActive(true);
            }

            int posY = -20;

            if (MaxPhysicalFPS > 0)
            {
                string txt = $"Physical: {PhysicalFPS} / {MaxPhysicalFPS} PF/s";
                if (PhysicalPaused == 1)
                {
                    txt += "  <b><color=#FF8888>Paused</color></b>";
                }

                physicalFPSTextObj.GetComponent<Text>().text = txt;
                physicalFPSTextObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, posY);
                posY -= 20;
            }
            else
            {
                physicalFPSTextObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 60);
            }

            int fps = Mathf.CeilToInt(1.0f / Time.deltaTime);
            renderFPSTextObj.GetComponent<Text>().text = $"Render: {(fps + (fps & 1)) / Controller.SkipRate} RF/s";
            renderFPSTextObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, posY);
        }

        private static void InitTextbox(GameObject obj, Color color)
        {
            Text txt = obj.AddComponent<Text>();
            obj.transform.SetParent(canvasObj.transform);

            txt.color = color;
            txt.font = defaultFont;
            txt.fontStyle = FontStyle.Normal;

            RectTransform trans = txt.GetComponent<RectTransform>();
            trans.anchorMin = new Vector2(0.005f, 0.99f);
            trans.anchorMax = new Vector2(0, 0.99f);
            trans.pivot = new Vector2(0, 0);
            trans.sizeDelta = new Vector2(500, 20);
            trans.anchoredPosition = new Vector2(0, 0);
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

namespace UnityKinematics
{
    [RequireComponent(typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster))]
    public class FPSTextBox : MonoBehaviour
    {
        public bool showUI = true;

        public static int PhysicalFPS { get; set; } = 0;
        public static int MaxPhysicalFPS { get; set; } = 0;
        public static int PhysicalPaused { get; set; } = 0;

        private static GameObject physicalFPSTextObj;
        private static GameObject renderFPSTextObj;
        private static Font defaultFont;

        void Start()
        {
            defaultFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");

            renderFPSTextObj = new GameObject("txt___render", typeof(RectTransform));
            InitTextbox(renderFPSTextObj, new Color(0.7f, 1, 0.9f));

            physicalFPSTextObj = new GameObject("txt___physical", typeof(RectTransform));
            InitTextbox(physicalFPSTextObj, new Color(1, 0.9f, 0.8f));

            KinematicsServerEvents.OnCommand += OnCommand;
        }

        void OnCommand(Command cmd)
        {
            switch (cmd.name)
            {
                case "_sys_physical_fps":
                    PhysicalFPS = cmd.pi[0];
                    MaxPhysicalFPS = cmd.pi[1];
                    PhysicalPaused = cmd.pi[2];
                    break;
            }
        }

        void Update()
        {
            if (InputController.GetKeyDown("Reset Scene"))
            {
                FPSTextBox.PhysicalPaused = 0;
                FPSTextBox.MaxPhysicalFPS = 0;
                FPSTextBox.PhysicalFPS = 0;
            }
            if (InputController.GetKeyDown("Show / Hide UI"))
            {
                showUI = !showUI;
            }

            if (!showUI)
            {
                if (physicalFPSTextObj) physicalFPSTextObj.SetActive(false);
                if (renderFPSTextObj) renderFPSTextObj.SetActive(false);
                return;
            }
            else 
            {
                if (physicalFPSTextObj) physicalFPSTextObj.SetActive(true);
                if (renderFPSTextObj) renderFPSTextObj.SetActive(true);
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
            renderFPSTextObj.GetComponent<Text>().text = $"Render: {(fps + (fps & 1)) / KinematicsServer.instance.skipRate} RF/s";
            renderFPSTextObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, posY);
        }

        private void InitTextbox(GameObject obj, Color color)
        {
            Text txt = obj.AddComponent<Text>();
            obj.transform.SetParent(transform);

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

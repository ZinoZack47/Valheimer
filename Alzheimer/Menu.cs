using UnityEngine;

namespace Alzheimer
{
    class Menu
    {
        public void Start()
        {
            /*
            GameObject myGO;
            GameObject myText;
            Canvas myCanvas;
            Text text;
            RectTransform rectTransform;

            // Canvas
            myGO = new GameObject();
            myGO.name = "TestCanvas";
            myGO.AddComponent<Canvas>();

            myCanvas = myGO.GetComponent<Canvas>();
            myGO.AddComponent<CanvasScaler>();
            myGO.AddComponent<GraphicRaycaster>();

            // Text
            myText = new GameObject();
            myText.transform.parent = myGO.transform;
            myText.name = "wibble";

            text = myText.AddComponent<Text>();
            text.text = "wobble";
            text.fontSize = 100;

            // Text position
            rectTransform = text.GetComponent<RectTransform>();
            rectTransform.localPosition = new Vector3(0, 0, 0);
            rectTransform.sizeDelta = new Vector2(400, 200);
            */
        }

        private static Menu singletonInstance = null;
        public static Menu Instance
        {
            get
            {
                singletonInstance = singletonInstance ?? new Menu();
                return singletonInstance;
            }
        }
    }
}

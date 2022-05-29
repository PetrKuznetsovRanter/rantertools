using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RanterTools
{
    [RequireComponent(typeof(Text))]
    public class FPSCounter : MonoBehaviour
    {
        const float fpsMeasurePeriod = 0.5f;
        int fpsAccumulator = 0;
        float fpsNextPeriod = 0;
        int currentFps;
        const string display = "{0} FPS";
        TextMeshProUGUI text;

        TextMeshProUGUI Text
        {
            get
            {
                if (text == null)
                {
                    text = GetComponent<TextMeshProUGUI>();

                    if (text == null)
                    {
                        text = gameObject.AddComponent<TextMeshProUGUI>();
                        RectTransform rectTransform = GetComponent<RectTransform>();
                        rectTransform.anchorMin = new Vector2(0.05f, 0.9f);
                        rectTransform.anchorMax = new Vector2(0.1f, 0.95f);
                    }
                }

                return text;
            }
        }

        private void Start()
        {
            fpsNextPeriod = Time.realtimeSinceStartup + fpsMeasurePeriod;
        }

        private void Update()
        {
            // measure average frames per second
            fpsAccumulator++;

            if (Time.realtimeSinceStartup > fpsNextPeriod)
            {
                currentFps = (int) (fpsAccumulator / fpsMeasurePeriod);
                fpsAccumulator = 0;
                fpsNextPeriod += fpsMeasurePeriod;
                Text.text = string.Format(display, currentFps);
            }
        }
    }
}
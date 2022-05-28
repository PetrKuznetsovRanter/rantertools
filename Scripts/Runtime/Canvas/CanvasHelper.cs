using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RanterTools.Canvas
{
    [RequireComponent(typeof(UnityEngine.Canvas))]
    public class CanvasHelper : MonoBehaviour
    {
        private static readonly UnityEvent OrientationChangeEvent = new UnityEvent();
        private static readonly UnityEvent ResolutionChangeEvent = new UnityEvent();
        private static readonly List<CanvasHelper> Helpers = new List<CanvasHelper>();
        private static bool ScreenChangeVarsInitialized;
        private static ScreenOrientation LastOrientation = ScreenOrientation.Portrait;
        private static Vector2 LastResolution = Vector2.zero;
        private static Rect LastSafeArea = Rect.zero;

        private UnityEngine.Canvas _canvas;
        private RectTransform _rectTransform;

        /// <summary>
        ///     Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            if (!Helpers.Contains(this))
                Helpers.Add(this);

            _canvas = GetComponent<UnityEngine.Canvas>();
            _rectTransform = GetComponent<RectTransform>();

            
            if (!ScreenChangeVarsInitialized)
            {
                LastOrientation = Screen.orientation;
                LastResolution.x = Screen.width;
                LastResolution.y = Screen.height;
                LastSafeArea = Screen.safeArea;

                ScreenChangeVarsInitialized = true;
            }
        }

        /// <summary>
        ///     Start is called on the frame when a script is enabled just before
        ///     any of the Update methods is called the first time.
        /// </summary>
        private void Start()
        {
            ApplySafeArea();
        }

        /// <summary>
        ///     This function is called when the MonoBehaviour will be destroyed.
        /// </summary>
        private void OnDestroy()
        {
            if (Helpers != null && Helpers.Contains(this))
                Helpers.Remove(this);
        }

        /// <summary>
        ///     Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void Update()
        {
            if (Helpers.Count == 0)
                return;

            if (Helpers[0] != this)
                return;

            if (Screen.orientation != LastOrientation)
                OrientationChanged();

            if (Screen.safeArea != LastSafeArea)
                SafeAreaChanged();

            if (Screen.width != LastResolution.x || Screen.height != LastResolution.y)
                ResolutionChanged();
        }

        public static bool isLandscape { get; private set; }

        public static void ForceUpdate()
        {
            OrientationChanged();
            ResolutionChanged(true);
            SafeAreaChanged(true);
        }

        public static Vector2 GetCanvasSize()
        {
            return Helpers[0]._rectTransform.sizeDelta;
        }

        public static Vector2 GetSafeAreaSize()
        {
            foreach (var s in SafeArea.All)
                if (s.RectTransform != null)
                    return s.RectTransform.sizeDelta;

            return GetCanvasSize();
        }

        private static void OrientationChanged()
        {
            //Debug.Log("Orientation changed from " + lastOrientation + " to " + Screen.orientation + " at " + Time.time);

            LastOrientation = Screen.orientation;
            LastResolution.x = Screen.width;
            LastResolution.y = Screen.height;

            isLandscape = LastOrientation == ScreenOrientation.LandscapeLeft || LastOrientation == ScreenOrientation.LandscapeRight || LastOrientation == ScreenOrientation.LandscapeLeft;
            OrientationChangeEvent.Invoke();
        }

        private static void ResolutionChanged(bool force = false)
        {
            if (LastResolution.x == Screen.width && LastResolution.y == Screen.height && !force)
                return;

            //Debug.Log("Resolution changed from " + lastResolution + " to (" + Screen.width + ", " + Screen.height + ") at " + Time.time);

            LastResolution.x = Screen.width;
            LastResolution.y = Screen.height;

            isLandscape = Screen.width > Screen.height;
            ResolutionChangeEvent.Invoke();
        }

        private static void SafeAreaChanged(bool force = false)
        {
            if (LastSafeArea == Screen.safeArea && !force)
                return;

            //Debug.Log("Safe Area changed from " + lastSafeArea + " to " + Screen.safeArea.size + " at " + Time.time);

            LastSafeArea = Screen.safeArea;

            for (var i = 0; i < Helpers.Count; i++)
                Helpers[i].ApplySafeArea();
        }

        private void ApplySafeArea()
        {
            var safeArea = Screen.safeArea;
            var anchorMin = safeArea.position;
            var anchorMax = safeArea.position + safeArea.size;
            anchorMin.x /= _canvas.pixelRect.width;
            anchorMin.y /= _canvas.pixelRect.height;
            anchorMax.x /= _canvas.pixelRect.width;
            anchorMax.y /= _canvas.pixelRect.height;

            foreach (var s in SafeArea.All)
            {
                s.RectTransform.anchorMin = anchorMin;
                s.RectTransform.anchorMax = anchorMax;
            }
        }
    }
}
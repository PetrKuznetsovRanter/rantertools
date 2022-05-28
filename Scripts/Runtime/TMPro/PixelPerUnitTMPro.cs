using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RanterTools.TMPro
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class PixelPerUnitTMPro : MonoBehaviour
    {
#if UNITY_EDITOR
        private static readonly List<PixelPerUnitTMPro> All = new List<PixelPerUnitTMPro>();
#endif

        [SerializeField] private float fontSize = -1;
        
        private float _font;

        /// <summary>
        ///     Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            TextMeshProUGUI = GetComponent<TextMeshProUGUI>();
            InputField = GetComponentInParent<TMP_InputField>();
#if UNITY_EDITOR
            All.Add(this);
#endif
#if UNITY_EDITOR
            InitFont();

            if (Math.Abs(_width - Mathf.Min(Screen.width, Screen.height)) > float.Epsilon || Math.Abs(_oldFont - fontSize) > float.Epsilon)
            {
                _width = Mathf.Min(Screen.width, Screen.height);
                _oldFont = _font;
#endif
                UpdateParameters();
#if UNITY_EDITOR
            }
#endif
        }

        /// <summary>
        ///     This function is called when the MonoBehaviour will be destroyed.
        /// </summary>
        private void OnDestroy()
        {
#if UNITY_EDITOR
            All.Remove(this);
#endif
        }
#if UNITY_EDITOR
        /// <summary>
        ///     This function is called when the object becomes enabled and active.
        /// </summary>
        private void OnEnable()
        {
            if (TextMeshProUGUI == null)
                TextMeshProUGUI = GetComponent<TextMeshProUGUI>();

            if (InputField == null)
                InputField = GetComponentInParent<TMP_InputField>();
            InitFont();

            if (Math.Abs(_width - Mathf.Min(Screen.width, Screen.height)) > float.Epsilon || Math.Abs(_oldFont - fontSize) > float.Epsilon)
            {
                _width = Mathf.Min(Screen.width, Screen.height);
                _oldFont = _font;
                UpdateParameters();
            }
        }
#endif
#if UNITY_EDITOR
        /// <summary>
        ///     Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void Update()
        {
            if (Math.Abs(_width - Mathf.Min(Screen.width, Screen.height)) > float.Epsilon || Math.Abs(_oldFont - fontSize) > float.Epsilon)
            {
                UpdateParameters();
                _width = Mathf.Min(Screen.width, Screen.height);
                _oldFont = _font;
            }
        }
#endif

        private TextMeshProUGUI TextMeshProUGUI { get; set; }
        private TMP_InputField InputField { get; set; }

#if UNITY_EDITOR
        [MenuItem("RanterTools/UI/DPI/ForceUpdate")]
        private static void UpdateAll()
        {
            foreach (var p in All)
                p.UpdateParameters();
        }
#endif

        private void UpdateParameters()
        {
            _font = fontSize * Mathf.Min(Screen.width, Screen.height) / 1080.0f;

            if (InputField != null)
                InputField.SetGlobalPointSize(_font);
            TextMeshProUGUI.fontSize = _font;
        }

        private void InitFont()
        {
            if (Math.Abs(fontSize - (-1)) < float.Epsilon)
            {
                if (InputField != null)
                {
                    if (InputField.textComponent == TextMeshProUGUI)
                        InputField.textComponent = null;
                    TextMeshProUGUI.enableAutoSizing = true;
                    TextMeshProUGUI.ForceMeshUpdate();
                }

                fontSize = TextMeshProUGUI.fontSize;

                if (InputField != null)
                {
                    if (InputField.textComponent == null)
                        InputField.textComponent = TextMeshProUGUI;
                    InputField.SetGlobalPointSize(_font);
                }
            }

            TextMeshProUGUI.enableAutoSizing = false;
        }
#if UNITY_EDITOR
        private float _width;
        private float _oldFont;
#endif
    }
}
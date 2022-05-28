using RanterTools.PassowrdInputField;
using TMPro;
using TMPro.EditorUtilities;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEditor.UI;
using UnityEngine;

namespace RanterTools.Editor
{
    /// <summary>
    ///     Password input field context menu class
    /// </summary>
    public class PasswordInputFieldMenu
    {
        /// <summary>
        ///     Create new password input field
        /// </summary>
        [MenuItem("GameObject/UI/PasswordInputField (TMP)", false, 10)]
        public static void CreatePasswordInputField()
        {
            var passwordinputfield = (GameObject) Object.Instantiate(Resources.Load("Editor/Prefabs/PasswordInputField (TMP)"));
            GameObject parent = null;

            if (Selection.activeObject != null)
                parent = Selection.activeObject as GameObject;

            if (parent == null)
                passwordinputfield.transform.parent = null;
            else
                passwordinputfield.transform.parent = parent.transform;
            passwordinputfield.name = "PasswordInputField (TMP)";
            (passwordinputfield.transform as RectTransform).anchoredPosition = Vector2.zero;
            Selection.activeObject = passwordinputfield;
        }
    }

    [CanEditMultipleObjects]
    [CustomEditor(typeof(PasswordInputField), true)]
    public class PasswordInputFieldEditor : SelectableEditor
    {
        private SerializedProperty m_CaretBlinkRate;
        private SerializedProperty m_CaretColor;
        private SerializedProperty m_CaretWidth;
        private SerializedProperty m_CharacterLimit;
        private SerializedProperty m_CharacterValidation;
        private SerializedProperty m_ContentType;
        private SerializedProperty m_CustomCaretColor;

        private AnimBool m_CustomColor;
        private SerializedProperty m_GlobalFontAsset;
        private SerializedProperty m_GlobalPointSize;
        private SerializedProperty m_HideMobileInput;
        private SerializedProperty m_HideMobileKeyboard;
        private SerializedProperty m_InputType;
        private SerializedProperty m_InputValidator;
        private SerializedProperty m_KeyboardType;
        private SerializedProperty m_LineLimit;
        private SerializedProperty m_LineType;
        private SerializedProperty m_OnDeselect;
        private SerializedProperty m_OnEndEdit;

        private SerializedProperty m_OnFocusSelectAll;
        private SerializedProperty m_OnSelect;
        private SerializedProperty m_OnValueChanged;
        private SerializedProperty m_Placeholder;
        private SerializedProperty m_ReadOnly;
        private SerializedProperty m_RegexValue;
        private SerializedProperty m_ResetOnDeActivation;
        private SerializedProperty m_RestoreOriginalTextOnEscape;
        private SerializedProperty m_RichText;
        private SerializedProperty m_RichTextEditingAllowed;
        private SerializedProperty m_ScrollbarScrollSensitivity;
        private SerializedProperty m_SelectionColor;
        private SerializedProperty m_StarPrefab;
        private SerializedProperty m_Stars;

        private SerializedProperty m_SwitchButton;
        private SerializedProperty m_Text;
        private SerializedProperty m_TextComponent;

        private SerializedProperty m_TextViewport;
        private SerializedProperty m_VerticalScrollbar;

        //TMP_InputValidator m_ValidationScript;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_SwitchButton = serializedObject.FindProperty("switchButton");
            m_StarPrefab = serializedObject.FindProperty("starPrefab");
            m_Stars = serializedObject.FindProperty("stars");
            m_TextViewport = serializedObject.FindProperty("m_TextViewport");
            m_TextComponent = serializedObject.FindProperty("m_TextComponent");
            m_Text = serializedObject.FindProperty("m_Text");
            m_ContentType = serializedObject.FindProperty("m_ContentType");
            m_LineType = serializedObject.FindProperty("m_LineType");
            m_LineLimit = serializedObject.FindProperty("m_LineLimit");
            m_InputType = serializedObject.FindProperty("m_InputType");
            m_CharacterValidation = serializedObject.FindProperty("m_CharacterValidation");
            m_InputValidator = serializedObject.FindProperty("m_InputValidator");
            m_RegexValue = serializedObject.FindProperty("m_RegexValue");
            m_KeyboardType = serializedObject.FindProperty("m_KeyboardType");
            m_CharacterLimit = serializedObject.FindProperty("m_CharacterLimit");
            m_CaretBlinkRate = serializedObject.FindProperty("m_CaretBlinkRate");
            m_CaretWidth = serializedObject.FindProperty("m_CaretWidth");
            m_CaretColor = serializedObject.FindProperty("m_CaretColor");
            m_CustomCaretColor = serializedObject.FindProperty("m_CustomCaretColor");
            m_SelectionColor = serializedObject.FindProperty("m_SelectionColor");

            m_HideMobileKeyboard = serializedObject.FindProperty("m_HideSoftKeyboard");
            m_HideMobileInput = serializedObject.FindProperty("m_HideMobileInput");

            m_Placeholder = serializedObject.FindProperty("m_Placeholder");
            m_VerticalScrollbar = serializedObject.FindProperty("m_VerticalScrollbar");
            m_ScrollbarScrollSensitivity = serializedObject.FindProperty("m_ScrollSensitivity");

            m_OnValueChanged = serializedObject.FindProperty("m_OnValueChanged");
            m_OnEndEdit = serializedObject.FindProperty("m_OnEndEdit");
            m_OnSelect = serializedObject.FindProperty("m_OnSelect");
            m_OnDeselect = serializedObject.FindProperty("m_OnDeselect");
            m_ReadOnly = serializedObject.FindProperty("m_ReadOnly");
            m_RichText = serializedObject.FindProperty("m_RichText");
            m_RichTextEditingAllowed = serializedObject.FindProperty("m_isRichTextEditingAllowed");
            m_ResetOnDeActivation = serializedObject.FindProperty("m_ResetOnDeActivation");
            m_RestoreOriginalTextOnEscape = serializedObject.FindProperty("m_RestoreOriginalTextOnEscape");

            m_OnFocusSelectAll = serializedObject.FindProperty("m_OnFocusSelectAll");
            m_GlobalPointSize = serializedObject.FindProperty("m_GlobalPointSize");
            m_GlobalFontAsset = serializedObject.FindProperty("m_GlobalFontAsset");

            m_CustomColor = new AnimBool(m_CustomCaretColor.boolValue);
            m_CustomColor.valueChanged.AddListener(Repaint);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            m_CustomColor.valueChanged.RemoveListener(Repaint);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            base.OnInspectorGUI();

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(m_SwitchButton);
            EditorGUILayout.PropertyField(m_StarPrefab);
            EditorGUILayout.PropertyField(m_Stars);

            EditorGUILayout.PropertyField(m_TextViewport);

            EditorGUILayout.PropertyField(m_TextComponent);

            TextMeshProUGUI text = null;

            if (m_TextComponent != null && m_TextComponent.objectReferenceValue != null)
                text = m_TextComponent.objectReferenceValue as TextMeshProUGUI;

            //if (text.supportRichText)
            //{
            //    EditorGUILayout.HelpBox("Using Rich Text with input is unsupported.", MessageType.Warning);
            //}

            EditorGUI.BeginDisabledGroup(m_TextComponent == null || m_TextComponent.objectReferenceValue == null);

            // TEXT INPUT BOX
            EditorGUILayout.PropertyField(m_Text);

            // INPUT FIELD SETTINGS

            #region INPUT FIELD SETTINGS

            m_foldout.fontSettings = EditorGUILayout.Foldout(m_foldout.fontSettings, "Input Field Settings", true, TMP_UIStyleManager.boldFoldout);

            if (m_foldout.fontSettings)
            {
                EditorGUI.indentLevel++;
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(m_GlobalFontAsset, new GUIContent("Font Asset", "Set the Font Asset for both Placeholder and Input Field text object."));

                if (EditorGUI.EndChangeCheck())
                {
                    var inputField = target as TMP_InputField;
                    inputField.SetGlobalFontAsset(m_GlobalFontAsset.objectReferenceValue as TMP_FontAsset);
                }

                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(m_GlobalPointSize, new GUIContent("Point Size", "Set the point size of both Placeholder and Input Field text object."));

                if (EditorGUI.EndChangeCheck())
                {
                    var inputField = target as TMP_InputField;
                    inputField.SetGlobalPointSize(m_GlobalPointSize.floatValue);
                }

                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(m_CharacterLimit);

                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(m_ContentType);

                if (!m_ContentType.hasMultipleDifferentValues)
                {
                    EditorGUI.indentLevel++;

                    if (m_ContentType.enumValueIndex == (int) TMP_InputField.ContentType.Standard || m_ContentType.enumValueIndex == (int) TMP_InputField.ContentType.Autocorrected ||
                        m_ContentType.enumValueIndex == (int) TMP_InputField.ContentType.Custom)
                    {
                        EditorGUI.BeginChangeCheck();
                        EditorGUILayout.PropertyField(m_LineType);

                        if (EditorGUI.EndChangeCheck())
                            if (text != null)
                            {
                                if (m_LineType.enumValueIndex == (int) TMP_InputField.LineType.SingleLine)
                                    text.enableWordWrapping = false;
                                else
                                    text.enableWordWrapping = true;
                            }

                        if (m_LineType.enumValueIndex != (int) TMP_InputField.LineType.SingleLine)
                            EditorGUILayout.PropertyField(m_LineLimit);
                    }

                    if (m_ContentType.enumValueIndex == (int) TMP_InputField.ContentType.Custom)
                    {
                        EditorGUILayout.PropertyField(m_InputType);
                        EditorGUILayout.PropertyField(m_KeyboardType);
                        EditorGUILayout.PropertyField(m_CharacterValidation);

                        if (m_CharacterValidation.enumValueIndex == (int) TMP_InputField.CharacterValidation.Regex)
                            EditorGUILayout.PropertyField(m_RegexValue);
                        else if (m_CharacterValidation.enumValueIndex == (int) TMP_InputField.CharacterValidation.CustomValidator)
                            EditorGUILayout.PropertyField(m_InputValidator);
                    }

                    EditorGUI.indentLevel--;
                }

                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(m_Placeholder);
                EditorGUILayout.PropertyField(m_VerticalScrollbar);

                if (m_VerticalScrollbar.objectReferenceValue != null)
                    EditorGUILayout.PropertyField(m_ScrollbarScrollSensitivity);

                EditorGUILayout.PropertyField(m_CaretBlinkRate);
                EditorGUILayout.PropertyField(m_CaretWidth);

                EditorGUILayout.PropertyField(m_CustomCaretColor);

                m_CustomColor.target = m_CustomCaretColor.boolValue;

                if (EditorGUILayout.BeginFadeGroup(m_CustomColor.faded))
                    EditorGUILayout.PropertyField(m_CaretColor);
                EditorGUILayout.EndFadeGroup();

                EditorGUILayout.PropertyField(m_SelectionColor);

                EditorGUI.indentLevel--;
            }

            #endregion

            // CONTROL SETTINGS

            #region CONTROL SETTINGS

            m_foldout.extraSettings = EditorGUILayout.Foldout(m_foldout.extraSettings, "Control Settings", true, TMP_UIStyleManager.boldFoldout);

            if (m_foldout.extraSettings)
            {
                EditorGUI.indentLevel++;

                EditorGUILayout.PropertyField(m_OnFocusSelectAll, new GUIContent("OnFocus - Select All", "Should all the text be selected when the Input Field is selected."));
                EditorGUILayout.PropertyField(m_ResetOnDeActivation, new GUIContent("Reset On DeActivation", "Should the Text and Caret position be reset when Input Field is DeActivated."));
                EditorGUILayout.PropertyField(m_RestoreOriginalTextOnEscape, new GUIContent("Restore On ESC Key", "Should the original text be restored when pressing ESC."));
                EditorGUILayout.PropertyField(m_HideMobileKeyboard, new GUIContent("Hide Soft Keyboard", "Controls the visibility of the mobile virtual keyboard."));

                EditorGUILayout.PropertyField(m_HideMobileInput,
                    new GUIContent("Hide Mobile Input", "Controls the visibility of the editable text field above the mobile virtual keyboard. Not supported on all mobile platforms."));
                EditorGUILayout.PropertyField(m_ReadOnly);
                EditorGUILayout.PropertyField(m_RichText);
                EditorGUILayout.PropertyField(m_RichTextEditingAllowed, new GUIContent("Allow Rich Text Editing"));

                EditorGUI.indentLevel--;
            }

            #endregion

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(m_OnValueChanged);
            EditorGUILayout.PropertyField(m_OnEndEdit);
            EditorGUILayout.PropertyField(m_OnSelect);
            EditorGUILayout.PropertyField(m_OnDeselect);

            EditorGUI.EndDisabledGroup();

            serializedObject.ApplyModifiedProperties();
        }

        private struct m_foldout
        {
            // Track Inspector foldout panel states, globally.
            public static bool textInput = true;
            public static bool fontSettings = true;

            public static bool extraSettings = true;

            //public static bool shadowSetting = false;
            //public static bool materialEditor = true;
        }
    }
}
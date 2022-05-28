using UnityEditor;
using UnityEngine;

namespace RanterTools.Editor
{
    /// <summary>
    ///     Context menu for combobox
    /// </summary>
    public class ComboBoxMenu
    {
        [MenuItem("GameObject/UI/ComboBox (TMP)", false, 10)]
        public static void CreateComboBox()
        {
            var combobox = (GameObject) Object.Instantiate(Resources.Load("Editor/Prefabs/ComboBox (TMP)"));
            GameObject parent = null;

            if (Selection.activeObject != null)
                parent = Selection.activeObject as GameObject;

            if (parent == null)
                combobox.transform.SetParent(null);
            else
                combobox.transform.SetParent(parent.transform);
            combobox.name = "ComboBox (TMP)";
            ((RectTransform) combobox.transform).anchoredPosition = Vector2.zero;
            Selection.activeObject = combobox;
        }
    }
}
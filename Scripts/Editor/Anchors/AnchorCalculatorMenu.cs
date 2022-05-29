using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace RanterTools.Editor
{
    [DefaultExecutionOrder(10000)]
    public static class AnchorCalculator
    {
        private static Dictionary<string, RectTransformState> States = new Dictionary<string, RectTransformState>();

        /// <summary>
        ///     Convert current size for currect screen to anchors.
        /// </summary>
        [MenuItem("CONTEXT/RectTransform/Apply anchors")]
        public static void ApplyAnchors(MenuCommand command)
        {
            if (EditorApplication.isPlaying)
                return;
            var rectTransform = (RectTransform) command.context;
            var parentRectTransform = rectTransform.parent as RectTransform;
            var path = RectTransformScenePath(rectTransform);

            if (!States.ContainsKey(path))
            {
                var state = new RectTransformState();
                state.defaultPosition = rectTransform.anchoredPosition;
                state.defaultSize = rectTransform.sizeDelta;
                state.defaultPivot = rectTransform.pivot;
                state.defaultAnchorMin = rectTransform.anchorMin;
                state.defaultAnchorMax = rectTransform.anchorMax;
                States[path] = state;
            }

            Vector2 position, pivotOffset = Vector2.zero;

            if (rectTransform.anchorMin == rectTransform.anchorMax)
                pivotOffset = rectTransform.pivot * rectTransform.rect.size;

            if (parentRectTransform != null)
            {
                Rect rect = parentRectTransform.rect;
                position = rect.size * rectTransform.anchorMin + rectTransform.anchoredPosition - pivotOffset;
                Vector2 min = new Vector2(position.x / rect.width, position.y / rect.height);
                Rect rect1 = rectTransform.rect;
                Vector2 max = new Vector2((position.x + rect1.width) / rect.width, (position.y + rect1.height) / rect.height);
                rectTransform.anchorMin = min;
                rectTransform.anchorMax = max;
            }

            rectTransform.sizeDelta = rectTransform.anchoredPosition = Vector2.zero;
            SaveStates();
        }

        /// <summary>
        ///     Revert previous state of rect transform.
        /// </summary>
        [MenuItem("CONTEXT/RectTransform/Revert anchors")]
        public static void RevertAnchors(MenuCommand command)
        {
            if (EditorApplication.isPlaying)
                return;
            var rectTransform = (RectTransform) command.context;
            var path = RectTransformScenePath(rectTransform);

            if (!States.ContainsKey(path))
            {
                ToolsDebug.Log($"RectTransformExtension can't revert anchors. {path}");
            }
            else
            {
                var state = States[path];
                rectTransform.pivot = state.defaultPivot;
                rectTransform.anchorMin = state.defaultAnchorMin;
                rectTransform.anchorMax = state.defaultAnchorMax;
                rectTransform.sizeDelta = state.defaultSize;
                rectTransform.anchoredPosition = state.defaultPosition;
                States.Remove(path);
            }

            SaveStates();
        }

        private static string RectTransformScenePath(Transform transform)
        {
            var path = $"{transform.gameObject.name}";
            var t = transform.parent;

            while (t != null)
            {
                path = path.Insert(0, $"{t.gameObject.name}\\");
                t = t.parent;
            }

            path = path.Insert(0, $"{transform.gameObject.scene.name}\\");
            return path;
        }

        private static void SaveStates()
        {
            var statesAsset = AssetDatabase.LoadAssetAtPath<RectTransformStates>(Path.Combine("Assets", "Editor", "Resources", "AnchorsHistory.asset"));

            if (statesAsset == null)
            {
                statesAsset = CreateScriptableObjectForStateOrLoad();
            }

            statesAsset.states = States.ToList().ConvertAll(s => new RectTransformStatesKeyValuePair {Key = s.Key, Value = s.Value});

            EditorUtility.SetDirty(statesAsset);
            AssetDatabase.SaveAssetIfDirty(statesAsset);
        }

        [InitializeOnLoadMethod]
        private static void LoadStates()
        {
            var statesAsset = AssetDatabase.LoadAssetAtPath<RectTransformStates>(Path.Combine("Assets", "Editor", "Resources", "AnchorsHistory.asset"));

            if (statesAsset == null)
            {
                statesAsset = CreateScriptableObjectForStateOrLoad();
            }

            States = statesAsset.states.ToDictionary(s => s.Key, s => s.Value);
        }

        private static RectTransformStates CreateScriptableObjectForStateOrLoad()
        {
            string path = Path.Combine("Editor", "Resources");
            RectTransformStates statesAsset;

            if (File.Exists(path))
            {
                statesAsset = Resources.Load<RectTransformStates>("AnchorsHistory");
            }
            else
            {
                statesAsset = ScriptableObject.CreateInstance<RectTransformStates>();

                if (!Directory.Exists(Path.Combine(Application.dataPath, "Editor")))
                {
                    Directory.CreateDirectory(Path.Combine(Application.dataPath, "Editor"));
                }

                if (!Directory.Exists(Path.Combine(Application.dataPath, path)))
                {
                    Directory.CreateDirectory(Path.Combine(Application.dataPath, path));
                }

                AssetDatabase.CreateAsset(statesAsset, Path.Combine(Path.Combine("Assets", path), "AnchorsHistory.asset"));
            }

            AssetDatabase.SaveAssets();
            return statesAsset;
        }
    }

    [Serializable]
    internal class RectTransformState
    {
        public Vector2 defaultAnchorMin, defaultAnchorMax;
        public Vector2 defaultPivot;
        public Vector2 defaultPosition;
        public Vector2 defaultSize;
    }
}
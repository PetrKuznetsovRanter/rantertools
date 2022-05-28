using System;
using System.Collections.Generic;
using UnityEngine;

namespace RanterTools.Editor
{
    internal class RectTransformStates : ScriptableObject
    {
        public List<RectTransformStatesKeyValuePair> states = new List<RectTransformStatesKeyValuePair>();
    }

    [Serializable]
    internal class RectTransformStatesKeyValuePair
    {
        public string Key;
        public RectTransformState Value;
    }
}
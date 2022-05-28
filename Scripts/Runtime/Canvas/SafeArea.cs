using System.Collections.Generic;
using UnityEngine;

namespace RanterTools.Canvas
{
    [RequireComponent(typeof(RectTransform))]
    public class SafeArea : MonoBehaviour
    {
        private RectTransform _rectTransform;

        /// <summary>
        ///     Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            All.Add(this);
        }

        /// <summary>
        ///     This function is called when the MonoBehaviour will be destroyed.
        /// </summary>
        private void OnDestroy()
        {
            All.Remove(this);
        }

        public static List<SafeArea> All { get; set; } = new List<SafeArea>();

        public RectTransform RectTransform
        {
            get { return _rectTransform = _rectTransform ? _rectTransform : GetComponent<RectTransform>(); }
        }
    }
}
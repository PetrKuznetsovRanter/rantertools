using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RanterTools.PassowrdInputField
{
    [DisallowMultipleComponent]
    [ExecuteAlways]
    public class PasswordInputField : TMP_InputField
    {
        [SerializeField] private Button switchButton;

        [SerializeField] private RectTransform starPrefab;

        [SerializeField] private float aspectRatioStar = 1;

        [SerializeField] private RectTransform stars;

        private bool _needUpdate;

        private readonly List<RectTransform> _starsContainer = new List<RectTransform>();
        private bool _visible;

        /// <summary>
        ///     Start is called on the frame when a script is enabled just before
        ///     any of the Update methods is called the first time.
        /// </summary>
        protected override void Start()
        {
            base.Start();
            _visible = true;
            Switch();
        }

        /// <summary>
        ///     This function is called when the object becomes enabled and active.
        /// </summary>
        protected override void OnEnable()
        {
            base.OnEnable();

            if (switchButton != null)
                switchButton.onClick.AddListener(Switch);
            onValueChanged.AddListener(OnValueChanged);
        }

        /// <summary>
        ///     This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        protected override void OnDisable()
        {
            base.OnDisable();

            if (switchButton != null)
                switchButton.onClick.RemoveListener(Switch);
            onValueChanged.RemoveListener(OnValueChanged);
        }

        /// <summary>
        ///     Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void Update()
        {
            textComponent.ForceMeshUpdate();
            var a = textComponent.GetRenderedValues(false);

            if (!_visible && text.Length > 0)
            {
                stars.anchoredPosition = new Vector2((textComponent.rectTransform).anchoredPosition.x, stars.anchoredPosition.y);
                stars.sizeDelta = new Vector2(a.x, stars.sizeDelta.y);
            }

            if (_needUpdate)
            {
                UpdateStars();
                _needUpdate = false;
            }
        }

        /// <summary>
        ///     Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        protected override void LateUpdate()
        {
            base.LateUpdate();
        }

        public void UpdateStars()
        {
            if (text.Length < _starsContainer.Count)
                while (text.Length != _starsContainer.Count)
                {
                    Destroy(_starsContainer[_starsContainer.Count - 1].gameObject);
                    _starsContainer.RemoveAt(_starsContainer.Count - 1);
                }
            else
                while (text.Length != _starsContainer.Count)
                {
                    var star = Instantiate(starPrefab, stars);
                    _starsContainer.Add(star);
                }

            var a = textComponent.GetRenderedValues(false);

            if (!_visible && text.Length > 0)
            {
                stars.anchoredPosition = new Vector2((textComponent.rectTransform).anchoredPosition.x, stars.anchoredPosition.y);
                stars.sizeDelta = new Vector2(a.x, stars.sizeDelta.y);
            }

            float w = a.x / _starsContainer.Count, h = w;

            for (var i = 0; i < _starsContainer.Count; i++)
            {
                _starsContainer[i].anchoredPosition = new Vector2(i * w, _starsContainer[i].anchoredPosition.y);

                if (aspectRatioStar > 0)
                    h = w / aspectRatioStar;
                else
                    h = _starsContainer[i].rect.height;
                _starsContainer[i].sizeDelta = new Vector2(w, h);

                if (!_starsContainer[i].gameObject.activeSelf)
                    _starsContainer[i].gameObject.SetActive(true);
            }
        }

        private void Switch()
        {
            _visible = !_visible;

            if (_visible)
            {
                var c = textComponent.color;
                c.a = 1;
                textComponent.color = c;
                stars.gameObject.SetActive(false);
            }
            else
            {
                stars.gameObject.SetActive(true);
                var c = textComponent.color;
                c.a = 0;
                textComponent.color = c;
                OnValueChanged(text);
            }
        }

        private void OnValueChanged(string value)
        {
            _needUpdate = true;
        }
    }
}
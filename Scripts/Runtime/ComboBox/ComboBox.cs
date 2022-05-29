using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace RanterTools.ComboBox
{
    /// <summary>
    ///     Class ComboBox
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    [AddComponentMenu("RanterTools/UI/ComboBox")]
    [DisallowMultipleComponent]
    public class ComboBox : MonoBehaviour
    {
        public static event Action<ComboBox> OnOpenPanel;

        /// <summary>
        ///     Main InputField for combobox
        /// </summary>
        [SerializeField] private TMP_InputField mainInput;

        /// <summary>
        ///     Scroll rect of items panel
        /// </summary>
        [SerializeField] private ScrollRect scrollRect;

        /// <summary>
        ///     Arrow button for show items panel
        /// </summary>
        [SerializeField] private Button arrow;

        /// <summary>
        ///     Template of option
        /// </summary>
        [SerializeField] private ComboBoxItem template;

        /// <summary>
        ///     Input field enabled
        /// </summary>
        [SerializeField] private bool input = true;

        /// <summary>
        ///     Reset input field text, after select option
        /// </summary>
        [SerializeField] private bool resetAfterSelect;

        [SerializeField] private bool closeIfAnotherOpened;

        /// <summary>
        ///     AvailableOptions for combobox
        /// </summary>
        /// <typeparam name="ComboBoxOptionString">Standart class for combobox option.</typeparam>
        /// <returns></returns>
        [FormerlySerializedAs("AvailableOptions")]
        public List<ComboBoxOptionString> availableOptions = new List<ComboBoxOptionString>();

        /// <summary>
        ///     Default panel size
        /// </summary>
        private Vector2 _defaultPanelSize;

        /// <summary>
        ///     Is panel active
        /// </summary>
        private bool _isPanelActive;

        /// <summary>
        ///     On select option
        /// </summary>
        [FormerlySerializedAs("OnSelectionChanged")]
        public SelectionChangedEvent onSelectionChanged;

        /// <summary>
        ///     Panel
        /// </summary>
        private RectTransform _panel;

        /// <summary>
        ///     Panel items
        /// </summary>
        private List<ComboBoxOptionString> _panelItems;

        /// <summary>
        ///     Panel's object
        /// </summary>
        /// <typeparam name="int">Id of option</typeparam>
        /// <typeparam name="ComboBoxItem">ComboboxItem</typeparam>
        /// <returns></returns>
        private readonly Dictionary<int, ComboBoxItem> _panelObjects = new Dictionary<int, ComboBoxItem>();

        /// <summary>
        ///     Scroll panel anchor max
        /// </summary>
        private float _scrollPanelAnchorMax;

        /// <summary>
        ///     Scroll panel bottom offset
        /// </summary>
        private float _scrollPanelBottomOffset;

        /// <summary>
        ///     Selected option, if it is.
        /// </summary>
        [FormerlySerializedAs("SelectedOption")]
        public ComboBoxOptionString selectedOption;

        /// <summary>
        ///     Text for search
        /// </summary>
        private string _text = "";

        /// <summary>
        ///     Awake is called when the script instance is being loaded.
        /// </summary>
        public void Awake()
        {
            OnOpenPanel += OnOpenPanelHandler;
            Rect rect = scrollRect.content.rect;
            ((RectTransform) template.transform).sizeDelta = new Vector2(rect.width, rect.width / template.GetComponent<AspectRatioFitter>().aspectRatio);
            _panel = scrollRect.transform.parent as RectTransform;

            if (_panel != null)
            {
                RectTransform rectTransform = (RectTransform) scrollRect.transform;
                _defaultPanelSize = new Vector2(_panel.anchorMin.y, _panel.anchorMax.y);
                _scrollPanelAnchorMax = (rectTransform).anchorMax.y;
                _scrollPanelBottomOffset = (rectTransform).anchorMin.y * _panel.rect.height;
            }

            Initialize();
            mainInput.onValueChanged.AddListener(OnValueChanged);
            mainInput.onSelect.AddListener(OnFieldSelect);

            if (arrow != null)
                arrow.onClick.AddListener(OnArrowTap);
        }

        /// <summary>
        ///     This function is called when the MonoBehaviour will be destroyed.
        /// </summary>
        private void OnDestroy()
        {
            OnOpenPanel -= OnOpenPanelHandler;
            mainInput.onValueChanged.RemoveListener(OnValueChanged);
            mainInput.onSelect.RemoveListener(OnFieldSelect);

            if (arrow != null)
                arrow.onClick.RemoveListener(OnArrowTap);
        }

        /// <summary>
        ///     Initialize for new available item
        /// </summary>
        /// <returns>Is success</returns>
        private bool Initialize()
        {
            var success = true;

            try
            {
                mainInput.enabled = input;
                _isPanelActive = false;
                _panel.gameObject.SetActive(_isPanelActive);
                _panelItems = availableOptions;

                foreach (var o in _panelObjects)
                    DestroyImmediate(o.Value.gameObject);
                _panelObjects.Clear();

                foreach (var c in availableOptions)
                {
                    _panelObjects[c.ID] = Instantiate(template, scrollRect.content);
                    _panelObjects[c.ID].Option = c;
                    _panelObjects[c.ID].gameObject.SetActive(true);
                    _panelObjects[c.ID].gameObject.name = _panelObjects[c.ID].Option.Name;
                    _panelObjects[c.ID].Button.onClick.AddListener(delegate { OnItemClicked(c); });
                }

                RedrawPanel();
            }
            catch
            {
                ToolsDebug.LogError("Something is setup incorrectly with the dropdown list component causing a Null Reference Exception");
                success = false;
            }

            return success;
        }

        /// <summary>
        ///     Select item from the list.
        /// </summary>
        /// <param name="item"></param>
        public void Select(string item)
        {
            OnItemClicked(item);
        }

        /// <summary>
        ///     What happens when an item in the list is selected
        /// </summary>
        /// <param name="item"></param>
        private void OnItemClicked(string item)
        {
            selectedOption = availableOptions.Find(s => s.Name == item);
            onSelectionChanged.Invoke(item);

            if (!resetAfterSelect)
            {
                _text = item;
                mainInput.text = item;
            }
            else
            {
                _text = mainInput.text = "";
            }

            _isPanelActive = false;
            _panel.gameObject.SetActive(false);
        }

        /// <summary>
        ///     Redraw panel
        /// </summary>
        private void RedrawPanel()
        {
            RectTransform scrollRectTransform = scrollRect.transform as RectTransform, rectTransform = transform as RectTransform;
            float height = 0, contentHeight = 0;
            var variants = false;

            foreach (var c in _panelObjects)
                if (_text == "" || c.Value.Option.Name.ToLower().StartsWith(_text.ToLower()))
                {
                    c.Value.gameObject.SetActive(true);
                    height += ((RectTransform) c.Value.transform).rect.height;
                    variants = true;
                }
                else
                {
                    c.Value.gameObject.SetActive(false);
                }

            if (variants)
            {
                float panelSize = 0;

                if (rectTransform != null && height < _scrollPanelBottomOffset - _defaultPanelSize.x * rectTransform.rect.height)
                {
                    contentHeight = height + _scrollPanelBottomOffset;
                    Rect rect = rectTransform.rect;
                    _panel.anchorMin = new Vector2(_panel.anchorMin.x, -(contentHeight / rect.height));
                    panelSize = (_panel.anchorMax.y - _panel.anchorMin.y) * rect.height;
                    _panel.sizeDelta = _panel.anchoredPosition = Vector2.zero;

                    if (scrollRectTransform != null)
                    {
                        scrollRectTransform.anchorMin = new Vector2(scrollRectTransform.anchorMin.x, _scrollPanelBottomOffset / panelSize);
                        scrollRectTransform.anchorMax = new Vector2(scrollRectTransform.anchorMax.x, 1 - rect.height / panelSize);
                        scrollRectTransform.anchoredPosition = scrollRectTransform.sizeDelta = Vector2.zero;
                    }
                }
                else
                {
                    _panel.anchorMin = new Vector2(_panel.anchorMin.x, _defaultPanelSize.x);

                    if (rectTransform != null)
                    {
                        panelSize = (_panel.anchorMax.y - _panel.anchorMin.y) * rectTransform.rect.height;
                        _panel.sizeDelta = _panel.anchoredPosition = Vector2.zero;

                        if (scrollRectTransform != null)
                        {
                            scrollRectTransform.anchorMin = new Vector2(scrollRectTransform.anchorMin.x, _scrollPanelBottomOffset / panelSize);
                            scrollRectTransform.anchorMax = new Vector2(scrollRectTransform.anchorMax.x, 1 - rectTransform.rect.height / panelSize);
                            scrollRectTransform.anchoredPosition = scrollRectTransform.sizeDelta = Vector2.zero;
                        }
                    }
                }
            }
            else
            {
                contentHeight = _scrollPanelBottomOffset;

                if (rectTransform != null)
                {
                    Rect rect = rectTransform.rect;
                    _panel.anchorMin = new Vector2(_panel.anchorMin.x, contentHeight / rect.height);
                    //panelSize = (_panel.anchorMax.y - _panel.anchorMin.y) * rect.height;
                }

                _panel.sizeDelta = _panel.anchoredPosition = Vector2.zero;
            }
        }

        /// <summary>
        ///     On input field value changed
        /// </summary>
        /// <param name="currText">New text</param>
        private void OnValueChanged(string currText)
        {
            _text = currText;

            if (_text == "")
            {
                selectedOption = null;

                if (arrow == null)
                {
                    _isPanelActive = false;
                    _panel.gameObject.SetActive(_isPanelActive);
                }
                else
                {
                    RedrawPanel();
                }
            }
            else
            {
                _isPanelActive = true;
                _panel.gameObject.SetActive(_isPanelActive);

                if (OnOpenPanel != null)
                    OnOpenPanel(this);
                RedrawPanel();
            }
        }

        /// <summary>
        ///     On option selected
        /// </summary>
        /// <param name="text">Option text</param>
        private void OnFieldSelect(string text)
        {
            mainInput.text = "";
        }

        /// <summary>
        ///     On Arrow tap handler
        /// </summary>
        private void OnArrowTap()
        {
            _isPanelActive = !_isPanelActive;
            _text = "";

            if (OnOpenPanel != null && _isPanelActive)
                OnOpenPanel(this);
            RedrawPanel();
            _panel.gameObject.SetActive(_isPanelActive);
        }

        /// <summary>
        ///     Callback for event when every combobox is opening.
        /// </summary>
        /// <param name="comboBox">Opening combobox</param>
        private void OnOpenPanelHandler(ComboBox comboBox)
        {
            if (comboBox != this && closeIfAnotherOpened)
            {
                _isPanelActive = false;
                _panel.gameObject.SetActive(_isPanelActive);
                RedrawPanel();
            }
        }

        [Serializable]
        public class SelectionChangedEvent : UnityEvent<string>
        {
        }
    }
}
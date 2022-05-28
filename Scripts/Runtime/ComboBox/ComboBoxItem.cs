using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RanterTools.ComboBox
{
    [RequireComponent(typeof(Button))]
    public class ComboBoxItem : MonoBehaviour
    {
        private Button _button;
        private ComboBoxOptionString _option;

        private TextMeshProUGUI _label;

        /// <summary>
        ///     Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
        }

        /// <summary>
        ///     This function is called when the MonoBehaviour will be destroyed.
        /// </summary>
        private void OnDestroy()
        {
        }

        /// <summary>
        ///     This function is called when the object becomes enabled and active.
        /// </summary>
        private void OnEnable()
        {
        }

        /// <summary>
        ///     This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        private void OnDisable()
        {
        }

        public ComboBoxOptionString Option
        {
            get => _option;
            set
            {
                _option = value;
                SetData(value);
            }
        }

        public Button Button
        {
            get { return _button = _button ? _button : GetComponent<Button>(); }
        }

        private TextMeshProUGUI Label
        {
            get { return _label = _label ? _label : GetComponentInChildren<TextMeshProUGUI>(true); }
        }

        private void SetData(ComboboxOption<string> option)
        {
            if (Label != null)
                Label.text = option.ToString().ToUpper();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Loyufei.UI;
using Loyufei.InputSystem;

namespace InputDemo
{
    public class InputText : MonoBehaviour, ISlot<IKeyUnit>
    {
        [SerializeField]
        private TextMeshProUGUI _TitleText;
        [SerializeField]
        private TextMeshProUGUI _KeyCodeText;
        [SerializeField]
        private string _OptionName;
        [SerializeField]
        private string _AxesName;
        [SerializeField]
        private IKeyUnit.EPositive _Positive;

        public string AxesName => this._AxesName;

        public IKeyUnit.EPositive Positive => this._Positive;

        public IKeyUnit Content { get; private set; }

        object ISlot.Content => this.Content;

        private void Start()
        {
            this._TitleText?.SetText(string.Format("Input"));
            this._KeyCodeText?.SetText(string.Format("None"));
        }

        public void SetSlot(IKeyUnit content) 
        {
            this.Content = content;

            this.UpdateSlot();
        }

        public void UpdateSlot() 
        {
            var option = this._OptionName;
            var keyCode = this.Content[this._Positive];

            this._TitleText?.SetText(string.Format("{0}", option));
            this._KeyCodeText?.SetText(string.Format("{0}", keyCode));
        }

        public void ClearSlot() 
        {
            this.Content = null;

            this._TitleText?.SetText(string.Format("Input"));
            this._KeyCodeText?.SetText(string.Format("None"));
        }
    }
}
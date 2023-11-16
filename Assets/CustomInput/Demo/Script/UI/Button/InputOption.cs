using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Loyufei.UI;
using Loyufei.InputSystem;

namespace InputDemo
{
    public class InputOption : MonoBehaviour, ISlot<IKeyUnit>
    {
        [SerializeField]
        private TextMeshProUGUI _OptionText;
        [SerializeField]
        private TextMeshProUGUI _KeyCodeText;
        [SerializeField]
        private string _OptionName;
        [SerializeField]
        private string _AxesName;
        [SerializeField]
        private IKeyUnit.EPositive _Positive;
        [SerializeField]
        private KeyCode _KeyCode;

        public string AxesName => this._AxesName;
        public IKeyUnit.EPositive Positive => this._Positive;

        public IKeyUnit Content { get; private set; }

        object ISlot.Content => this.Content;

        private void Start()
        {
            this._OptionText.SetText(_OptionName);
        }

        public void SetSlot(IKeyUnit content)
        {
            this.Content = content;
            
            this.UpdateSlot();
        }

        public void SetSlot(IInputUnit content) 
        {
            if (content is IKeyUnit unit) { SetSlot(unit); }
        }

        public void UpdateSlot()
        {
            this._KeyCode = this.Content[this._Positive];

            this._KeyCodeText.SetText(string.Format("{0}", this._KeyCode));
        }

        public void ClearSlot()
        {
            this.Content = null;
        }
    }
}
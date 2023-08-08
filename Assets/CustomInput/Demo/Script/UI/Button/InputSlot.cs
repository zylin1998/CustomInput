using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.UI;
using Custom.InputSystem;

namespace InputDemo
{
    public class InputSlot : OptionButton, ISlot<IKeyUnit>
    {
        [Header("Input Information")]
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

        public void SetSlot(IKeyUnit content) 
        {
            this.Content = content;

            this.UpdateSlot();
        }

        public void UpdateSlot() 
        {
            this._KeyCode = this.Content[this._Positive];

            this._ContentText.SetText(string.Format("{0}", this._KeyCode));
        }

        public void ClearSlot() 
        {
            this.Content = null;
        }
    } 
}
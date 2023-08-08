using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace InputDemo
{
    public class OptionButton : DemoButton
    {
        [Header("Option Text")]
        [SerializeField]
        protected TextMeshProUGUI _TitleText;
        [SerializeField]
        protected TextMeshProUGUI _ContentText;

        public TextMeshProUGUI ContentText => this._ContentText;

        protected virtual void Start()
        {
            this._TitleText?.SetText(this._Name);
        }
    }
}
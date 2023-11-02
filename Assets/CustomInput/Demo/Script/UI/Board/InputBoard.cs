using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.InputSystem;
using Loyufei.InputSystem;

namespace InputDemo
{
    public class InputBoard : MonoBehaviour
    {
        [SerializeField]
        private Transform _Content;
        [SerializeField]
        private List<InputText> _Texts;
        [SerializeField]
        private InputCenter _InputCenter;

        public IInputSubset InputSubset { get; private set; }

        private void Start()
        {
            if (!_InputCenter.CheckInputMode(EInputMode.KeyMode)) { Destroy(this.gameObject); }

            this.InputSubset = _InputCenter.InputSetting.GetSetbyName("Keyboard").OnUse;

            this._Texts = this._Content.GetComponentsInChildren<InputText>().ToList();

            InitBoard.StartButton.ClickEvent += (data) => this.SetSlots();
        }

        public void SetSlots() 
        {
            this._Texts.ForEach(f =>
            {
                if (InputSubset[f.AxesName] is IKeyUnit unit)
                {
                    f.SetSlot(unit);
                }
            });
        }
    }
}

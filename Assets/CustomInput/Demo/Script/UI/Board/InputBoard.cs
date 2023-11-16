using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Loyufei.InputSystem;

namespace InputDemo
{
    public class InputBoard : MonoBehaviour
    {
        [SerializeField]
        private Transform _Content;
        [SerializeField]
        private List<InputText> _Texts;
        
        public InputCenter InputCenter { get; private set; }

        public IInputSubset InputSubset { get; private set; }

        private void Start()
        {
            InputCenter = InputSystemProperty.InputCenter;

            if (!InputCenter.CheckInputMode(EInputMode.Keyboard)) { Destroy(gameObject); }

            InputSubset = InputCenter.InputCollection.GetSet<KeyInputList>().OnUse;

            _Texts = _Content.GetComponentsInChildren<InputText>().ToList();

            InitBoard.StartButton.onClick.AddListener(() => this.SetSlots());
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Loyufei.UI;
using Loyufei.InputSystem;
using Loyufei;

namespace Custom.InputSystem
{
    public class MobileButton : Selectable, ITouchInput
    {
        [SerializeField]
        private string _AxesName;

        public string AxesName => this._AxesName;

        public float Axis => 0f;
        public bool GetKeyDown { get; private set; }
        public bool GetKey { get; private set; }
        public bool GetKeyUp { get; private set; }

        public ITouchUnit TouchUnit { get; private set; }

        private Coroutine _Coroutine = null;

        protected override void Start()
        {
            base.Start();

            InputSystemProperty.InputCenter.SetTouchInput(this);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            InputSystemProperty.InputCenter?.ClearTouchInput(this);
        }

        public void SetTouchUnit(ITouchUnit touchUnit) 
        {
            TouchUnit = touchUnit.IsDefaultandReturn(TouchUnit);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);

            GetKeyDown = true;

            ChangeCoroutine(GettingKey());
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);

            GetKey = false;
            GetKeyUp = true;

            ChangeCoroutine(GettingKeyUp());
        }

        public void ChangeCoroutine(IEnumerator enumerator) 
        {
            if (_Coroutine != null) { StopCoroutine(_Coroutine); }

            _Coroutine = StartCoroutine(enumerator);
        }

        private IEnumerator GettingKey() 
        {
            yield return new WaitForEndOfFrame();

            GetKeyDown = false;
            GetKey = true;
        }

        private IEnumerator GettingKeyUp()
        {
            yield return new WaitForEndOfFrame();

            GetKeyUp = false;
        }
    }
}
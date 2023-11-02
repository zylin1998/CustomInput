using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Custom.UI;
using Loyufei.InputSystem;

namespace Custom.InputSystem
{
    public class MobileButton : MonoBehaviour, ISelectable, ITouchButton
    {
        [SerializeField]
        private string _AxesName;
        [SerializeField]
        private InputCenter _InputCenter;

        public string AxesName => this._AxesName;

        public bool GetKeyDown { get; private set; }
        public bool GetKey { get; private set; }
        public bool GetKeyUp { get; private set; }

        private Coroutine _Coroutine = null;

        private void Start()
        {
            _InputCenter.SetTouchInput(this);
        }

        public string Name => this._AxesName;

        public UI.Navigation Navigation { get; set; }

        public void OnPointerDown(PointerEventData eventData)
        {
            this.GetKeyDown = true;

            IEnumerator GetKey()
            {
                yield return new WaitForEndOfFrame();

                this.GetKeyDown = false;
                this.GetKey = true;
            }

            ChangeCoroutine(GetKey());
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            this.GetKey = false;
            this.GetKeyUp = true;

            IEnumerator GetKeyUp()
            {
                yield return new WaitForEndOfFrame();

                this.GetKeyUp = false;
            }

            ChangeCoroutine(GetKeyUp());
        }

        public void ChangeCoroutine(IEnumerator enumerator) 
        {
            if (this._Coroutine != null) { StopCoroutine(this._Coroutine); }

            this._Coroutine = StartCoroutine(enumerator);
        }
    }
}
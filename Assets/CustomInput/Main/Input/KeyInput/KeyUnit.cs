using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loyufei.InputSystem
{
    [System.Serializable]
    public class KeyUnit : InputUnit, IKeyUnit
    {
        public KeyUnit(string name) : base(name) 
        {
            _Positive = KeyCode.None;
            _Negative = KeyCode.None;
        }

        [SerializeField]
        private KeyCode _Positive;
        [SerializeField]
        private KeyCode _Negative;

        #region IAxes

        private float _Axis = 0;
        
        public override float Axis=> GetAxis(WholeNumber);

        public override bool GetKeyDown => Input.GetKeyDown(Positive);
        public override bool GetKey => Input.GetKey(Positive);
        public override bool GetKeyUp => Input.GetKeyUp(Positive);

        #endregion

        #region IKeyAxes

        public KeyCode Positive => this._Positive;
        public KeyCode Negative => this._Negative;

        public KeyCode this[IKeyUnit.EPositive positive]
        {
            get => positive == IKeyUnit.EPositive.Positive ? _Positive : _Negative;
        }

        public void SetAxes(IKeyUnit.EPositive positive, KeyCode keyCode)
        {
            if (positive == IKeyUnit.EPositive.Positive) { _Positive = keyCode; }

            if (positive == IKeyUnit.EPositive.Negative) { _Negative = keyCode; }
        }

        #endregion

        public override float GetAxis(bool wholeNumber) 
        {
            var speed = wholeNumber ? 1f : (1f / Sensitive) * Time.deltaTime;

            if (_Axis >= 0)
            {
                if (Input.GetKey(Positive)) { _Axis += speed; }

                else { _Axis = Convergence ? _Axis -= speed : 0f; }
                
                _Axis = Mathf.Clamp(_Axis, 0f, 1f);
            }

            if (_Axis <= 0)
            {
                if (Input.GetKey(Negative)) { _Axis -= speed; }

                else { _Axis = Convergence ? _Axis += speed : 0f; }

                _Axis = Mathf.Clamp(_Axis, -1f, 0f);
            }

            return _Axis;
        }
    }

    public interface IKeyUnit : IInputUnit
    {
        public KeyCode Positive { get; }
        public KeyCode Negative { get; }

        public KeyCode this[EPositive positive] => positive == EPositive.Positive ? this.Positive : this.Negative;

        public void SetAxes(EPositive positive, KeyCode keyCode);

        [System.Serializable]
        public enum EPositive
        {
            Positive,
            Negative
        }
    }
}
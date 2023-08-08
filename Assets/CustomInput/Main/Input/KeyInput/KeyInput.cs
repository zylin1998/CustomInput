using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom.InputSystem
{
    [System.Serializable]
    public class KeyInput : IKeyUnit
    {
        [SerializeField]
        private string _Name;
        [SerializeField]
        private KeyCode _Positive;
        [SerializeField]
        private KeyCode _Negative;
        [SerializeField]
        private bool _WholeNumber;
        [SerializeField]
        private bool _Convergence = true;
        [SerializeField, Range(0.01f, 1f)]
        private float _Sensitive = 0.5f;

        #region IAxes

        public string Name => this._Name;

        public bool WholeNumber
        {
            get => this._WholeNumber;

            set => this._WholeNumber = value;
        }

        public bool Convergence
        {
            get => this._Convergence;

            set => this._Convergence = value;
        }

        public float Sensitive
        {
            get => this._Sensitive;

            set => this._Sensitive = value;
        }
        
        private float _Axis = 0;
        
        public float Axis=> GetAxis(this._WholeNumber);

        public bool GetKeyDown => Input.GetKeyDown(this.Positive);
        public bool GetKey => Input.GetKey(this.Positive);
        public bool GetKeyUp => Input.GetKeyUp(this.Positive);

        #endregion

        #region IKeyAxes

        public KeyCode Positive => this._Positive;
        public KeyCode Negative => this._Negative;

        public KeyCode this[IKeyUnit.EPositive positive]
        {
            get => positive == IKeyUnit.EPositive.Positive ? this._Positive : this._Negative;
        }

        public void SetAxes(IKeyUnit.EPositive positive, KeyCode keyCode)
        {
            if (positive == IKeyUnit.EPositive.Positive) { this._Positive = keyCode; }

            if (positive == IKeyUnit.EPositive.Negative) { this._Negative = keyCode; }
        }

        #endregion

        public float GetAxis(bool wholeNumber) 
        {
            var speed = wholeNumber ? 1f : (1f / this._Sensitive) * Time.deltaTime;

            if (this._Axis >= 0)
            {
                if (Input.GetKey(this.Positive)) { this._Axis += speed; }

                else { this._Axis = this._Convergence ? this._Axis -= speed : 0f; }
                
                this._Axis = Mathf.Clamp(this._Axis, 0f, 1f);
            }

            if (this._Axis <= 0)
            {
                if (Input.GetKey(this.Negative)) { this._Axis -= speed; }

                else { this._Axis = this._Convergence ? this._Axis += speed : 0f; }

                this._Axis = Mathf.Clamp(this._Axis, -1f, 0f);
            }

            return this._Axis;
        }
    }

    public interface IKeyUnit : IInputUnit
    {
        public KeyCode Positive { get; }
        public KeyCode Negative { get; }

        public KeyCode this[EPositive positive] => positive == EPositive.Positive ? this.Positive : this.Negative;

        public void SetAxes(EPositive positive, KeyCode keyCode);

        public float GetAxis(bool wholeNumber);

        [System.Serializable]
        public enum EPositive
        {
            Positive,
            Negative
        }
    }
}
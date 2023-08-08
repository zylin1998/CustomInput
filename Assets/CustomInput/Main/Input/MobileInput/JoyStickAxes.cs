using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom.InputSystem
{
    [System.Serializable]
    public class JoyStickAxes : IJoyStickAxis
    {
        [SerializeField]
        private string _Name;
        [SerializeField]
        private IJoyStickAxis.EAxis _AxisType;
        [SerializeField]
        private float _Degree;
        [SerializeField]
        private bool _WholeNumber;
        [SerializeField]
        private bool _Convergence = true;

        [SerializeField, Range(0.01f, 1f)]
        private float _Sensitive = 0.5f;

        #region IJoyStickAxes

        public IJoyStickAxis.EAxis AxisType => this._AxisType;
        public float Degree => this._Degree;

        public IVJoyStick TouchInput { get; private set; }

        public void SetAxes(IVJoyStick joyStick) 
        {
            this.TouchInput = joyStick ?? this.TouchInput;
        }

        #endregion

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

        private float _Axis = 0f;

        public float Axis 
        {
            get 
            {
                var angle = this.TouchInput != null ? this.TouchInput.Angle : 0f;
                var speed = this._WholeNumber ? 1f : (1f / this._Sensitive) * Time.deltaTime;

                if (!this.TouchInput.IsOnDrag) 
                { 
                    this._Axis = this._Convergence ? AxisConvergence(speed) : 0f;

                    return this._Axis; 
                }

                if (this._AxisType == IJoyStickAxis.EAxis.Vertical) 
                {
                    if (this.Up(angle)) { this._Axis += speed; }

                    else if (this.Down(angle)) { this._Axis -= speed; }

                    else { this._Axis = this._Convergence ? AxisConvergence(speed) : 0f; }
                }

                if (this._AxisType == IJoyStickAxis.EAxis.Horizontal) 
                {
                    if (this.Right(angle)) { this._Axis += speed; }

                    else if (this.Left(angle)) { this._Axis -= speed; }

                    else { this._Axis = this._Convergence ? AxisConvergence(speed) : 0f; }
                }

                if (this._Axis > 0) { this._Axis = Mathf.Clamp(this._Axis, 0f, 1f); }
                if (this._Axis < 0) { this._Axis = Mathf.Clamp(this._Axis, -1f, 0f); }
                
                return this._Axis;
            }
        }

        public bool GetKeyDown => false;
        public bool GetKey => false;
        public bool GetKeyUp => false;

        #endregion

        private float AxisConvergence(float speed) 
        {
            if (this._Axis > 0) { return Mathf.Clamp(this._Axis -= speed, 0f, 1f); }

            if (this._Axis < 0) { return Mathf.Clamp(this._Axis += speed, -1f, 0f); }

            return 0f;
        }

        private bool Up(float angle) => Mathf.Abs(angle - 90f) <= this._Degree;
        private bool Down(float angle) => Mathf.Abs(angle + 90f) <= this._Degree;
        private bool Right(float angle) => Mathf.Abs(angle - 0f) <= this._Degree;
        private bool Left(float angle) => 180f - Mathf.Abs(angle) <= this._Degree;
    }
}
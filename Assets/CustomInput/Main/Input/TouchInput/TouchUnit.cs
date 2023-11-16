using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loyufei.InputSystem 
{
    [System.Serializable]
    public class TouchUnit : InputUnit, ITouchUnit
    {
        public TouchUnit(string name) : base(name)
        {
            _Degree = 60f;
        }

        [SerializeField]
        private float _Degree;

        public float Degree => _Degree;

        public ITouchInput TouchInput { get; private set; }

        public void SetInput(ITouchInput touchInput) 
        {
            TouchInput = touchInput.IsDefaultandReturn(TouchInput);

            TouchInput?.SetTouchUnit(this);
        }

        public void ClearInput(ITouchInput touchInput)
        {
            if (TouchInput.IsEqual(touchInput)) 
            {
                TouchInput = null;
            }
        }

        public override float Axis => GetAxis(WholeNumber);
        public override bool GetKeyDown => TouchInput.IsDefault() ? false : TouchInput.GetKeyDown;
        public override bool GetKey => TouchInput.IsDefault() ? false : TouchInput.GetKey;
        public override bool GetKeyUp => TouchInput.IsDefault() ? false : TouchInput.GetKeyUp;

        public override float GetAxis(bool wholeNumber) 
        {
            var axis = TouchInput.IsDefault() ? 0 : TouchInput.Axis;

            if (axis > 0) { return wholeNumber ?  1 : axis; }

            if (axis < 0) { return wholeNumber ? -1 : axis; }

            return 0;
        }
    }

    /// <summary>
    /// Ĳ������J�b
    /// </summary>
    public interface ITouchUnit : IInputUnit
    {
        /// <summary>
        /// �ϬM����
        /// </summary>
        public float Degree { get; }

        /// <summary>
        /// Ĳ������J
        /// </summary>
        public ITouchInput TouchInput { get; }

        /// <summary>
        /// �]�wĲ������J�s���ܿ�J�b
        /// </summary>
        /// <param name="touchInput">Ĳ������J</param>
        public void SetInput(ITouchInput touchInput);

        /// <summary>
        /// �M��Ĳ������J�s���ܿ�J�b
        /// </summary>
        /// <param name="touchInput">Ĳ������J</param>
        public void ClearInput(ITouchInput touchInput);
    }
}
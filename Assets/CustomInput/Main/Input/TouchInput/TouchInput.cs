using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Loyufei.InputSystem 
{
    /// <summary>
    /// 觸控式輸入
    /// </summary>
    public interface ITouchInput
    {
        public string AxesName { get; }
        public float Axis { get; }
        public bool GetKeyDown { get; }
        public bool GetKey { get; }
        public bool GetKeyUp { get; }

        public ITouchUnit TouchUnit { get; }

        public void SetTouchUnit(ITouchUnit unit);
    }

    /// <summary>
    /// 虛擬搖桿
    /// </summary>
    public interface IVJoyStick : IDragHandler, IEndDragHandler
    {
        /// <summary>
        /// 水平輸入
        /// </summary>
        public ITouchInput Horizontal { get; }

        /// <summary>
        /// 垂直輸入
        /// </summary>
        public ITouchInput Vertical { get; }

        /// <summary>
        /// 搖桿當前角度
        /// </summary>
        public float Angle { get; }

        /// <summary>
        /// 搖桿是否處於拖曳中
        /// </summary>
        public bool IsOnDrag { get; }

        [System.Serializable]
        protected class TouchInput : ITouchInput
        {
            [System.Serializable]
            public enum EGetKeyType 
            {
                None = 0,
                GetKeyDown = 1,
                GetKey = 2,
                GetKeyUp = 3,
            }

            [System.Serializable]
            public enum EAxis
            {
                Horizontal,
                Vertical
            }

            [SerializeField]
            private string _Name;
            [SerializeField]
            private EAxis _AxisType;

            public string AxesName => _Name;
            public EAxis AxisType => _AxisType;

            public ITouchUnit TouchUnit { get; private set; }

            public void SetTouchUnit(ITouchUnit touchUnit)
            {
                TouchUnit = touchUnit.IsDefaultandReturn(TouchUnit);
            }

            private float _Axis = 0f;
            private float _Angle = 0f;

            public float Axis
            {
                get
                {
                    var speed = (1f / TouchUnit.Sensitive) * Time.deltaTime;

                    if (_Angle == 0)
                    {
                        _Axis = TouchUnit.Convergence ? AxisConvergence(this, speed) : 0f;

                        return _Axis;
                    }

                    if (_AxisType == EAxis.Vertical)
                    {
                        if (Up(this, _Angle)) { _Axis += speed; }

                        else if (Down(this, _Angle)) { _Axis -= speed; }

                        else { _Axis = TouchUnit.Convergence ? AxisConvergence(this, speed) : 0f; }
                    }

                    if (_AxisType == EAxis.Horizontal)
                    {
                        if (Right(this, _Angle)) { _Axis += speed; }

                        else if (Left(this, _Angle)) { _Axis -= speed; }

                        else { _Axis = TouchUnit.Convergence ? AxisConvergence(this, speed) : 0f; }
                    }

                    if (_Axis > 0) { _Axis = Mathf.Clamp(_Axis, 0f, 1f); }
                    if (_Axis < 0) { _Axis = Mathf.Clamp(_Axis, -1f, 0f); }

                    return _Axis;
                }
            }

            public bool GetKeyDown { get; private set; }
            public bool GetKey { get; private set; }
            public bool GetKeyUp { get; private set; }

            public void SetAngle(float angle)
            {
                _Angle = angle;
            }

            public void SetBool(bool getKeyDown, bool getKey, bool getKeyUp)
            {
                GetKeyDown = getKeyDown;
                GetKey = getKey;
                GetKeyUp = getKeyUp;
            }

            public void SetGetKeyType(EGetKeyType getKeyType) 
            {
                GetKeyDown = getKeyType.HasFlag(EGetKeyType.GetKeyDown);
                GetKey = getKeyType.HasFlag(EGetKeyType.GetKey);
                GetKeyUp = getKeyType.HasFlag(EGetKeyType.GetKeyUp);
            }

            private static float AxisConvergence(TouchInput self, float speed)
            {
                if (self._Axis > 0) { return Mathf.Clamp(self._Axis -= speed, 0f, 1f); }

                if (self._Axis < 0) { return Mathf.Clamp(self._Axis += speed, -1f, 0f); }

                return 0f;
            }

            private static bool Up(TouchInput self, float angle) => Mathf.Abs(angle - 90f) <= self.TouchUnit.Degree;
            private static bool Down(TouchInput self, float angle) => Mathf.Abs(angle + 90f) <= self.TouchUnit.Degree;
            private static bool Right(TouchInput self, float angle) => Mathf.Abs(angle - 0f) <= self.TouchUnit.Degree;
            private static bool Left(TouchInput self, float angle) => 180f - Mathf.Abs(angle) <= self.TouchUnit.Degree;
        }
    }
}
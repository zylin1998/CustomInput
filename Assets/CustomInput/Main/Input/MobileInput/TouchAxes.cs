using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom.InputSystem 
{
    /// <summary>
    /// Ĳ������J�b
    /// </summary>
    public interface ITouchUnit : IInputUnit
    {
        /// <summary>
        /// Ĳ������J
        /// </summary>
        public ITouchInput TouchInput { get; }

        /// <summary>
        /// �]�wĲ������J�s���ܿ�J�b
        /// </summary>
        /// <param name="touchInput">Ĳ������J</param>
        public void SetAxes(ITouchInput touchInput);
    }

    /// <summary>
    /// �����n���J�b
    /// </summary>
    public interface IJoyStickAxis : ITouchUnit
    {
        /// <summary>
        /// ��J�b����
        /// </summary>
        [System.Serializable]
        public enum EAxis
        {
            Horizontal,
            Vertical
        }

        /// <summary>
        /// ��J�b����
        /// </summary>
        public EAxis AxisType { get; }

        /// <summary>
        /// �ϬM����
        /// </summary>
        public float Degree { get; }

        /// <summary>
        /// �n���J
        /// </summary>
        public new IVJoyStick TouchInput { get; }

        /// <summary>
        /// �]�w�n���J�s���ܿ�J�b
        /// </summary>
        /// <param name="joyStick">�n���J</param>
        public void SetAxes(IVJoyStick joyStick);

        ITouchInput ITouchUnit.TouchInput => this.TouchInput;

        void ITouchUnit.SetAxes(ITouchInput touchInput)
        {
            if (touchInput is IVJoyStick joyStick) { this.SetAxes(joyStick); }
        }
    }

    /// <summary>
    /// Ĳ�������s��J�b
    /// </summary>
    public interface IButtonAxis : ITouchUnit
    {
        /// <summary>
        /// Ĳ�������s��J
        /// </summary>
        public new ITouchButton TouchInput { get; }

        /// <summary>
        /// �]�wĲ�������s��J�s���ܿ�J�b
        /// </summary>
        /// <param name="touchButton"></param>
        public void SetAxes(ITouchButton touchButton);

        ITouchInput ITouchUnit.TouchInput => this.TouchInput;

        void ITouchUnit.SetAxes(ITouchInput touchInput)
        {
            if (touchInput is ITouchButton button) { this.SetAxes(button); }
        }
    }
}
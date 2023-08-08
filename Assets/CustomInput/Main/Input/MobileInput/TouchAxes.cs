using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom.InputSystem 
{
    /// <summary>
    /// 觸控式輸入軸
    /// </summary>
    public interface ITouchUnit : IInputUnit
    {
        /// <summary>
        /// 觸控式輸入
        /// </summary>
        public ITouchInput TouchInput { get; }

        /// <summary>
        /// 設定觸控式輸入連結至輸入軸
        /// </summary>
        /// <param name="touchInput">觸控式輸入</param>
        public void SetAxes(ITouchInput touchInput);
    }

    /// <summary>
    /// 虛擬搖桿輸入軸
    /// </summary>
    public interface IJoyStickAxis : ITouchUnit
    {
        /// <summary>
        /// 輸入軸種類
        /// </summary>
        [System.Serializable]
        public enum EAxis
        {
            Horizontal,
            Vertical
        }

        /// <summary>
        /// 輸入軸種類
        /// </summary>
        public EAxis AxisType { get; }

        /// <summary>
        /// 反映角度
        /// </summary>
        public float Degree { get; }

        /// <summary>
        /// 搖桿輸入
        /// </summary>
        public new IVJoyStick TouchInput { get; }

        /// <summary>
        /// 設定搖桿輸入連結至輸入軸
        /// </summary>
        /// <param name="joyStick">搖桿輸入</param>
        public void SetAxes(IVJoyStick joyStick);

        ITouchInput ITouchUnit.TouchInput => this.TouchInput;

        void ITouchUnit.SetAxes(ITouchInput touchInput)
        {
            if (touchInput is IVJoyStick joyStick) { this.SetAxes(joyStick); }
        }
    }

    /// <summary>
    /// 觸控式按鈕輸入軸
    /// </summary>
    public interface IButtonAxis : ITouchUnit
    {
        /// <summary>
        /// 觸控式按鈕輸入
        /// </summary>
        public new ITouchButton TouchInput { get; }

        /// <summary>
        /// 設定觸控式按鈕輸入連結至輸入軸
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
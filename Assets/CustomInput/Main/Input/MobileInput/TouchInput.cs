using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Custom.InputSystem 
{
    /// <summary>
    /// 觸控式輸入
    /// </summary>
    public interface ITouchInput
    {

    }

    /// <summary>
    /// 虛擬搖桿
    /// </summary>
    public interface IVJoyStick : IDragHandler, IEndDragHandler, ITouchInput
    {
        /// <summary>
        /// 水平輸入軸名稱
        /// </summary>
        public string Horizontal { get; }

        /// <summary>
        /// 垂直輸入軸名稱
        /// </summary>
        public string Vertical { get; }

        /// <summary>
        /// 搖桿當前角度
        /// </summary>
        public float Angle { get; }

        /// <summary>
        /// 搖桿是否處於拖曳中
        /// </summary>
        public bool IsOnDrag { get; }

        /// <summary>
        /// 搖桿拖曳事件
        /// </summary>
        /// <param name="eventData">指標事件資料</param>
        public new void OnDrag(PointerEventData eventData);

        /// <summary>
        /// 搖桿拖曳結束事件
        /// </summary>
        /// <param name="eventData">指標事件資料</param>
        public new void OnEndDrag(PointerEventData eventData);

        void IDragHandler.OnDrag(PointerEventData eventData) => this.OnDrag(eventData);
        void IEndDragHandler.OnEndDrag(PointerEventData eventData) => this.OnEndDrag(eventData);
    }

    public interface ITouchButton : ITouchInput
    {
        /// <summary>
        /// 輸入軸名稱
        /// </summary>
        public string AxesName { get; }

        /// <summary>
        /// 按鈕首次按壓
        /// </summary>
        public bool GetKeyDown { get; }

        /// <summary>
        /// 按鈕持續按壓
        /// </summary>
        public bool GetKey { get; }

        /// <summary>
        /// 按鈕放開
        /// </summary>
        public bool GetKeyUp { get; }
    }
}
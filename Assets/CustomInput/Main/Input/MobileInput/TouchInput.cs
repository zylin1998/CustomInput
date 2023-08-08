using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Custom.InputSystem 
{
    /// <summary>
    /// Ĳ������J
    /// </summary>
    public interface ITouchInput
    {

    }

    /// <summary>
    /// �����n��
    /// </summary>
    public interface IVJoyStick : IDragHandler, IEndDragHandler, ITouchInput
    {
        /// <summary>
        /// ������J�b�W��
        /// </summary>
        public string Horizontal { get; }

        /// <summary>
        /// ������J�b�W��
        /// </summary>
        public string Vertical { get; }

        /// <summary>
        /// �n���e����
        /// </summary>
        public float Angle { get; }

        /// <summary>
        /// �n��O�_�B��즲��
        /// </summary>
        public bool IsOnDrag { get; }

        /// <summary>
        /// �n��즲�ƥ�
        /// </summary>
        /// <param name="eventData">���Шƥ���</param>
        public new void OnDrag(PointerEventData eventData);

        /// <summary>
        /// �n��즲�����ƥ�
        /// </summary>
        /// <param name="eventData">���Шƥ���</param>
        public new void OnEndDrag(PointerEventData eventData);

        void IDragHandler.OnDrag(PointerEventData eventData) => this.OnDrag(eventData);
        void IEndDragHandler.OnEndDrag(PointerEventData eventData) => this.OnEndDrag(eventData);
    }

    public interface ITouchButton : ITouchInput
    {
        /// <summary>
        /// ��J�b�W��
        /// </summary>
        public string AxesName { get; }

        /// <summary>
        /// ���s��������
        /// </summary>
        public bool GetKeyDown { get; }

        /// <summary>
        /// ���s�������
        /// </summary>
        public bool GetKey { get; }

        /// <summary>
        /// ���s��}
        /// </summary>
        public bool GetKeyUp { get; }
    }
}
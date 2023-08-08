using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Custom.InputSystem;
using Custom.UI;

namespace InputDemo
{
    public class DemoButton : MonoBehaviour, ISelectable, IClick
    {
        [Header("Selectable Name")]
        [SerializeField]
        protected string _Name;
        [Header("Background Color")]
        [SerializeField]
        protected Image _Image;
        [SerializeField]
        protected Color _Normal;
        [SerializeField]
        protected Color _Selected;

        #region ISelectable

        public virtual string Name => this._Name;

        public Custom.UI.Navigation Navigation { get; set; }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            if (this._Image) this._Image.color = this._Selected;
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            //if (this._Image) this._Image.color = this._Normal;
        }

        public virtual void OnSelect(BaseEventData eventData)
        {
            if (this._Image) this._Image.color = this._Selected;
        }

        public virtual void OnDeselect(BaseEventData eventData)
        {
            if (this._Image) this._Image.color = this._Normal;
        }

        #endregion

        #region IClick

        protected Action<BaseEventData> _OnClick = (data) => { };

        public event Action<BaseEventData> ClickEvent
        {
            add => this._OnClick += value;

            remove => this._OnClick -= value;
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            this._OnClick?.Invoke(eventData);
        }

        #endregion
    }
}
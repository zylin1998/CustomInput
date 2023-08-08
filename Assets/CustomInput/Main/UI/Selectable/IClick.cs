using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Custom.UI
{
    public interface IClick : IPointerClickHandler
    {
        public event Action<BaseEventData> ClickEvent;

        public new void OnPointerClick(PointerEventData eventData);

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData) => this.OnPointerClick(eventData);
    }
}
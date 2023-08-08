using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Custom.UI
{
    public interface ISelectable : IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, ISelectHandler, IDeselectHandler
    {
        public string Name { get; }
        public Navigation Navigation { get; set; }

        public new void OnPointerEnter(PointerEventData eventData) { }
        public new void OnPointerExit(PointerEventData eventData) { }
        public new void OnPointerDown(PointerEventData eventData) { }
        public new void OnPointerUp(PointerEventData eventData) { }
        public new void OnSelect(BaseEventData eventData) { }
        public new void OnDeselect(BaseEventData eventData) { }

        public void Select(BaseEventData eventData) 
        {
            if (this is Component component) 
            {
                EventSystem.current.SetSelectedGameObject(component.gameObject, eventData);
            }
        }

        public void Select() => this.Select(new BaseEventData(EventSystem.current));

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData) => this.OnPointerEnter(eventData);
        void IPointerExitHandler.OnPointerExit(PointerEventData eventData) => this.OnPointerExit(eventData);
        void IPointerDownHandler.OnPointerDown(PointerEventData eventData) => this.OnPointerDown(eventData);
        void IPointerUpHandler.OnPointerUp(PointerEventData eventData) => this.OnPointerUp(eventData);
        void ISelectHandler.OnSelect(BaseEventData eventData) => this.OnSelect(eventData);
        void IDeselectHandler.OnDeselect(BaseEventData eventData) => this.OnDeselect(eventData);
    }
}
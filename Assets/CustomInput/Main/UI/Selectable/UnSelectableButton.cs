using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Loyufei.UI
{
    public class UnSelectableButton : MonoBehaviour
                                    , IPointerEnterHandler, IPointerExitHandler
                                    , IPointerDownHandler, IPointerUpHandler
                                    , IPointerClickHandler, ISubmitHandler
    {
        [SerializeField]
        private Image _Image;
        [SerializeField]
        private ColorBlock _ColorBlock;

        private Action _OnClick = () => { };

        public event Action ClickEvent
        {
            add { _OnClick += value; }

            remove { _OnClick -= value; }
        }

        private bool _Enter = false;
        private bool _Hold = false;

        public void OnPointerEnter(PointerEventData eventData)
        {
            _Image.color = _ColorBlock.highlightedColor;
            
            _Enter = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!_Hold)
            {
                _Image.color = _ColorBlock.normalColor;
            }

            _Enter = false;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _Image.color = _ColorBlock.pressedColor;

            _Hold = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _Image.color = _Enter ? _ColorBlock.highlightedColor : _ColorBlock.normalColor;

            _Hold = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Press();
        }

        public void OnSubmit(BaseEventData eventData) 
        {
            Press();
        }

        private void Press() 
        {
            _OnClick?.Invoke();
        }
    }
}
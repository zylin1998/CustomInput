using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Loyufei.InputSystem;

namespace Loyufei.UI
{
    public class VirtualJoyStick : ScrollCircle, IVJoyStick, IBeginDragHandler
    {
        [Header("Axes Name")]
        [SerializeField]
        private IVJoyStick.TouchInput _Horizontal;
        [SerializeField]
        private IVJoyStick.TouchInput _Vertical;
        
        public ITouchInput Horizontal => _Horizontal;
        public ITouchInput Vertical => _Vertical;

        private float _Angle;

        public float Angle 
        { 
            get => _Angle;

            private set 
            {
                _Angle = value;

                _Horizontal.SetAngle(Angle);
                _Vertical.SetAngle(Angle);
            }
        }

        public bool IsOnDrag { get; private set; }

        protected override void Start()
        {
            base.Start();

            InputSystemProperty.InputCenter.SetTouchInput(this);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            InputSystemProperty.InputCenter?.ClearTouchInput(this);
        }

        public override void OnBeginDrag(PointerEventData eventData) 
        {
            base.OnBeginDrag(eventData);

            SetGetKeyType(IVJoyStick.TouchInput.EGetKeyType.GetKeyDown);
        }

        public override void OnDrag(PointerEventData eventData)
        {
            base.OnDrag(eventData);

            SetGetKeyType(IVJoyStick.TouchInput.EGetKeyType.GetKey);

            IsOnDrag = true;

            var direction = content.localPosition.normalized;
                
            Angle = Vector2.SignedAngle(Vector2.right, direction);
        }

        public override void OnEndDrag(PointerEventData eventData) 
        {
            base.OnEndDrag(eventData);

            SetGetKeyType(IVJoyStick.TouchInput.EGetKeyType.GetKeyUp);
            
            Angle = 0;

            IsOnDrag = false;

            StartCoroutine(EndGetKeyUp());
        }

        private IEnumerator EndGetKeyUp() 
        {
            yield return new WaitForEndOfFrame();

            SetGetKeyType(IVJoyStick.TouchInput.EGetKeyType.None);
        }

        private void SetGetKeyType(IVJoyStick.TouchInput.EGetKeyType getKeyType) 
        {
            _Horizontal.SetGetKeyType(getKeyType);
            _Vertical.SetGetKeyType(getKeyType);
        }
    }
}
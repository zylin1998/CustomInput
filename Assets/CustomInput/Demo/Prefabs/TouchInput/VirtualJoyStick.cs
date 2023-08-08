using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Custom.InputSystem
{
    [RequireComponent(typeof(ScrollCircle))]
    public class VirtualJoyStick : MonoBehaviour, IVJoyStick
    {
        [Header("Axes Name")]
        [SerializeField]
        private string _Horizontal = "Horizontal";
        [SerializeField]
        private string _Vertical = "Vertical";

        private Transform content;

        public string Horizontal => this._Horizontal;
        public string Vertical => this._Vertical;

        public float Angle { get; private set; }
        public bool IsOnDrag { get; private set; }

        private void Awake()
        {
            this.content = this.GetComponent<ScrollCircle>()?.content;
        }

        private void Start()
        {
            InputClient.SetTouchInput(this);
        }

        public void OnDrag(PointerEventData eventData)
        {
            this.IsOnDrag = true;

            if (this.content)
            {
                var direction = this.content.localPosition.normalized;
                
                this.Angle = Vector2.SignedAngle(Vector2.right, direction);
            }
        }

        public void OnEndDrag(PointerEventData eventData) 
        {
            this.IsOnDrag = false;
        }
    }
}
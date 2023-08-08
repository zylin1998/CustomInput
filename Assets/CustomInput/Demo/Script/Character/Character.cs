using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InputDemo
{
    public class Character : MonoBehaviour, IContacts
    {
        private void Awake()
        {
            this._Controller = this.GetComponent<CharacterController>();
            this._MainCamera = Camera.main.transform;
            this.Interacts = new List<IInteract>();
        }

        #region Movement

        [Header("Dynamic Direction")]
        [SerializeField]
        private CharacterController _Controller;
        [SerializeField] 
        private float _TurnSmoothTime = 0.1f;
        [SerializeField] 
        private float _Speed = 1.5f;
        [SerializeField]
        private float _Gravity = -9.81f;
        [SerializeField] 
        private float _GroundDistance = 0.4f;
        [SerializeField] 
        private LayerMask _GroundMask;

        private bool _IsGround;
        private Vector3 _Velocity;
        private float _TurnSmoothVelocity;
        
        private Transform _MainCamera;

        private bool _CanMove = true;

        private void Start()
        {
            ReStartBoard.ReStartButton.ClickEvent += (data) =>
            {
                this.MoveTo(Vector3.zero, Quaternion.identity);
            };

            InputDemo.OnEndChanged += (end) =>
            {
                this._CanMove = false;
            };
        }

        public void Move(Vector2 direct)
        {
            if (!_CanMove) { return; }

            this.GroundCheck();

            if (direct == Vector2.zero) { return; }

            float horizontal = direct.x;
            float vertical = direct.y;

            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

            if (direction.magnitude >= 0.1f)
            {
                var atan2 = Mathf.Atan2(direction.x, direction.z);
                var targetAngle = atan2 * Mathf.Rad2Deg + this._MainCamera.eulerAngles.y;

                var angleY = this.transform.eulerAngles.y;
                var angle = Mathf.SmoothDampAngle(angleY, targetAngle, ref _TurnSmoothVelocity, _TurnSmoothTime);
                
                this.transform.rotation = Quaternion.Euler(0f, angle, 0f);

                var moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                
                this._Controller.Move(moveDir.normalized * _Speed * Time.deltaTime);
            }
        }

        private void GroundCheck()
        {
            this._IsGround = Physics.CheckSphere(this.transform.position, this._GroundDistance, this._GroundMask);

            if (this._IsGround && this._Velocity.y < 0)
            {
                this._Velocity.y = -2f;
            }

            this._Velocity.y += this._Gravity * Time.deltaTime;

            this._Controller.Move(this._Velocity * Time.deltaTime);
        }

        public void MoveTo(Vector3 position, Quaternion rotation) 
        {
            this._Controller.Move(Vector3.zero);

            this.transform.SetPositionAndRotation(position, rotation);

            this._CanMove = true;
        }

        #endregion

        [SerializeField]
        private Transform _Hint;

        public List<IInteract> Interacts { get; private set; }
        
        public void Contact(IInteract interact) 
        {
            this.Interacts.Add(interact);

            if (this.Interacts.Any()) { this._Hint?.gameObject.SetActive(true); }
        }

        public void DisContact(IInteract interact) 
        {
            this.Interacts.Remove(interact);

            if (!this.Interacts.Any()) { this._Hint?.gameObject.SetActive(false); }
        }

        public void Interact(IInteract interact) 
        {
            interact?.Interact(this);
        }
    }
}
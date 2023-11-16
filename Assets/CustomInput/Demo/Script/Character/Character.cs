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
            _Controller = GetComponent<CharacterController>();
            _MainCamera = Camera.main.transform;
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

        public CharacterController Controller => _Controller;

        public void Move(Vector2 direct)
        {
            if (!_Controller.enabled) { return; }

            GroundCheck();

            if (direct == Vector2.zero) { return; }

            float horizontal = direct.x;
            float vertical = direct.y;

            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

            if (direction.magnitude >= 0.1f)
            {
                var atan2 = Mathf.Atan2(direction.x, direction.z);
                var targetAngle = atan2 * Mathf.Rad2Deg + this._MainCamera.eulerAngles.y;

                var angleY = transform.eulerAngles.y;
                var angle = Mathf.SmoothDampAngle(angleY, targetAngle, ref _TurnSmoothVelocity, _TurnSmoothTime);
                
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                var moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                
                _Controller.Move(moveDir.normalized * _Speed * Time.deltaTime);
            }
        }

        private void GroundCheck()
        {
            _IsGround = Physics.CheckSphere(transform.position, _GroundDistance, _GroundMask);

            if (_IsGround && _Velocity.y < 0)
            {
                _Velocity.y = -2f;
            }

            _Velocity.y += _Gravity * Time.deltaTime;

            _Controller.Move(_Velocity * Time.deltaTime);
        }

        public void MoveTo(Vector3 position, Quaternion rotation) 
        {
            transform.SetPositionAndRotation(position, rotation);
        }

        #endregion

        #region IContact

        [SerializeField]
        private Transform _Hint;

        public List<IInteract> Interacts { get; private set; } = new List<IInteract>();
        
        public void Contact(IInteract interact) 
        {
            Interacts.Add(interact);

            if (Interacts.Any()) { _Hint?.gameObject.SetActive(true); }
        }

        public void DisContact(IInteract interact) 
        {
            Interacts.Remove(interact);

            if (!Interacts.Any()) { _Hint?.gameObject.SetActive(false); }
        }

        public void Interact(IInteract interact) 
        {
            interact?.Interact(this);
        }

        #endregion
    }
}
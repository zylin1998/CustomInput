using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

namespace Loyufei.InputSystem 
{
    public abstract class InputUnit : IInputUnit
    {
        public InputUnit(string name) 
        {
            _Name = name;
            _WholeNumber = false;
            _Convergence = true;
            _Sensitive = 0.5f;
        }

        [SerializeField]
        private string _Name;
        [SerializeField]
        private bool _WholeNumber;
        [SerializeField]
        private bool _Convergence;
        [SerializeField, Range(0.01f, 1f)]
        private float _Sensitive;

        public string Name => _Name;

        public virtual bool WholeNumber 
        {
            get => _WholeNumber;

            set => _WholeNumber = value;
        }

        public virtual bool Convergence 
        {
            get => _Convergence;

            set => _Convergence = value;
        }

        public virtual float Sensitive 
        {
            get => _Sensitive;

            set => _Sensitive = value;
        }

        public abstract float Axis { get; }
        public abstract bool GetKeyDown { get; }
        public abstract bool GetKey { get; }
        public abstract bool GetKeyUp { get; }

        public abstract float GetAxis(bool wholeNumber);
    }

    public interface IInputUnit
    {
        public string Name { get; }
        public float Axis { get; }
        public bool WholeNumber { get; set; }
        public float Sensitive { get; set; }
        public bool Convergence { get; set; }
        public bool GetKeyDown { get; }
        public bool GetKey { get; }
        public bool GetKeyUp { get; }
        public InputValue Value => new InputValue(this);

        public float GetAxis(bool wholeNumber);
    }

    public struct InputValue
    {
        public string Name { get; private set; }
        public float Axis { get; private set; }
        public bool GetKeyDown { get; private set; }
        public bool GetKey { get; private set; }
        public bool GetKeyUp { get; private set; }

        public InputValue(IInputUnit unit)
        {
            this.Name = unit.Name;
            this.Axis = unit.Axis;
            this.GetKeyDown = unit.GetKeyDown;
            this.GetKey = unit.GetKey;
            this.GetKeyUp = unit.GetKeyUp;
        }
    }
}

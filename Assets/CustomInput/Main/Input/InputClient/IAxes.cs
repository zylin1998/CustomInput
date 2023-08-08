using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom.InputSystem 
{
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

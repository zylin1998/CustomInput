using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom.InputSystem
{
    [System.Serializable]
    public class ButtonAxes : IButtonAxis
    {
        [SerializeField]
        private string _Name;
        
        public ITouchButton TouchInput { get; private set; }

        public void SetAxes(ITouchButton touchButton) 
        {
            this.TouchInput = touchButton ?? this.TouchInput;
        }

        public string Name => this._Name;
        public bool WholeNumber { get; set; }
        public bool Convergence { get; set; }
        public float Sensitive { get; set; }
        public float Axis => 0f;
        public bool GetKeyDown => this.TouchInput.GetKeyDown;
        public bool GetKey => this.TouchInput.GetKey;
        public bool GetKeyUp => this.TouchInput.GetKeyUp;
    }
}
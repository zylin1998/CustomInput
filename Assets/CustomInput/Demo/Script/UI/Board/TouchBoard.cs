using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Loyufei.InputSystem;

namespace Custom.InputSystem
{
    public class TouchBoard : MonoBehaviour
    {
        public InputCenter InputCenter { get; private set; }

        void Start()
        {
            InputCenter = InputSystemProperty.InputCenter;

            if (!InputCenter.CheckInputMode(EInputMode.Touch))
            {
                Destroy(gameObject);
            }
        }
    }
}
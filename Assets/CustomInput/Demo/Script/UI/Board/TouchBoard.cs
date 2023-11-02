using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Loyufei.InputSystem;

namespace Custom.InputSystem
{
    public class TouchBoard : MonoBehaviour
    {
        [SerializeField]
        private InputCenter _InputCenter;

        void Start()
        {
            if (!_InputCenter.CheckInputMode(EInputMode.TouchMode))
            {
                Destroy(this.gameObject);
            }
        }
    }
}
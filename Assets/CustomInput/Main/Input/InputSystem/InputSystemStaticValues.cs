using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace Loyufei.InputSystem
{
    public static class InputSystemProperty
    {
        public static InputCenter InputCenter { get; set; }

        private static DefaultUnits _DefaultUnits;
        
        public static DefaultUnits DefaultUnits 
        {
            get 
            {
                var defaultValue = _DefaultUnits ? _DefaultUnits.ToString() == "null" : true;
                
                if (defaultValue) 
                {
                    _DefaultUnits = null;
                }

                return _DefaultUnits; 
            }

            set 
            {
                _DefaultUnits = value;
            } 
        }
    }
}

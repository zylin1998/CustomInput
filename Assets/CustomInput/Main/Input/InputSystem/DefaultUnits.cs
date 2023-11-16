using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loyufei.InputSystem
{
    [CreateAssetMenu(fileName = "Default Units", menuName = "Input System/Default Units", order = 1)]
    public class DefaultUnits : ScriptableObject
    {
        [SerializeField]
        private List<string> _UnitNames;
        [SerializeField]
        private List<string> _UIControlUnitNames;

        public List<TUnit> GetUnits<TUnit>() where TUnit : IInputUnit
        {
            return _UnitNames.ConvertAll(name => Activator.CreateInstance(typeof(TUnit), name).IsType<TUnit>());
        }

        public List<KeyUnit> GetUIControlUnits()
        {
            return _UIControlUnitNames.ConvertAll(name => new KeyUnit(name));
        }
        
        public void OnEnable()
        {
            InputSystemProperty.DefaultUnits = this;
        }
    }
}

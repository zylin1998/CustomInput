using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loyufei.InputSystem
{
    [CreateAssetMenu(fileName = "Touch Input", menuName = "Input System/Input List/Touch Input", order = 1)]
    public class TouchInputList : InputSet
    {
        [SerializeField]
        private List<Subset> _UnitSubsets;

        public override IEnumerator<IInputSubset> GetEnumerator() => _UnitSubsets.GetEnumerator();

        protected virtual void Reset()
        {
            Debug.Log(InputSystemProperty.DefaultUnits.IsDefault());

            InputMode = EInputMode.Touch;
            _UnitSubsets = new List<Subset> { new Subset() };
        }

        [System.Serializable]
        private new class Subset : InputSet.Subset 
        {
            public Subset()
            {
                var defaultUnits = InputSystemProperty.DefaultUnits;

                _Units = !defaultUnits.IsDefault() ? defaultUnits.GetUnits<TouchUnit>() : new List<TouchUnit>();
            }

            [SerializeField]
            private List<TouchUnit> _Units;

            public override IEnumerator<IInputUnit> GetEnumerator() => this._Units.GetEnumerator();
        }
    }
}
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loyufei.InputSystem
{
    [CreateAssetMenu(fileName = "Key Input", menuName = "Input System/Input List/Key Input", order = 1)]
    public class KeyInputList : InputSet
    {
        [SerializeField]
        private EChangeKey _ChangeType = EChangeKey.Exchange;
        [SerializeField]
        private List<Subset> _UnitSubsets;
        [SerializeField]
        private bool _IndependentUIControl;
        [SerializeField]
        private Subset _UIControl;

        public IInputUnit UIControlUnit(string axisName)
            => _IndependentUIControl ? _UIControl[axisName] : OnUse[axisName];

        public (IKeyUnit change, IKeyUnit exchange) ChangeKey(string axisName, IKeyUnit.EPositive positive, KeyCode keyCode) 
        {
            if (OnUse is Subset subset) 
            {
                var change = subset[axisName];
                var exchange = subset[keyCode];
                
                if (!exchange.IsDefault()) 
                {
                    var exchangePositive = exchange.Positive == keyCode ? IKeyUnit.EPositive.Positive : IKeyUnit.EPositive.Negative;
                    var exchangeKeyCode = _ChangeType == EChangeKey.Delete ? KeyCode.None : change[positive];

                    exchange.SetAxes(exchangePositive, exchangeKeyCode);
                }

                change.SetAxes(positive, keyCode);
                
                return (change, exchange);
            }

            return (default, default);
        }

        public override IEnumerator<IInputSubset> GetEnumerator() => this._UnitSubsets.GetEnumerator();

        private void Reset()
        {
            var defaultUnit = InputSystemProperty.DefaultUnits;

            InputMode = EInputMode.Keyboard;
            _ChangeType = EChangeKey.Exchange;
            _UnitSubsets = new List<Subset>() { new Subset() };
            _IndependentUIControl = true;
            _UIControl = new Subset(defaultUnit?.GetUIControlUnits());
        }

        [System.Serializable]
        private new class Subset : InputSet.Subset
        {
            public Subset() 
            {
                var defaultUnits = InputSystemProperty.DefaultUnits;

                _Units = defaultUnits ? defaultUnits.GetUnits<KeyUnit>() : new List<KeyUnit>();
            }

            public Subset(IEnumerable<KeyUnit> units) 
            {
                _Units = !units.IsDefault() ? units.ToList() : new List<KeyUnit>();
            }

            [SerializeField]
            private List<KeyUnit> _Units;

            public new IKeyUnit this[string name] => _Units.Find(f => f.Name == name);
            
            public IKeyUnit this[KeyCode keyCode]
            {
                get => _Units.Find(f => f.Positive == keyCode || f.Negative == keyCode);
            }

            public override IEnumerator<IInputUnit> GetEnumerator() => _Units.GetEnumerator();
        }

        [System.Serializable]
        private enum EChangeKey 
        {
            Exchange,
            Delete
        }
    }
}
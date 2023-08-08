using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom.InputSystem
{
    [CreateAssetMenu(fileName = "Key List", menuName = "Custum Input/Key List", order = 1)]
    public class KeyInputList : InputSet
    {
        [SerializeField]
        private EChangeKey _ChangeType = EChangeKey.Exchange;
        [SerializeField]
        private List<Subset> _AxesList;
        [SerializeField]
        private UIControlInput _UIControl;

        public UIControlInput UIControl => this._UIControl;

        public IKeyUnit ChangeKey(string axisName, IKeyUnit.EPositive positive, KeyCode keyCode) 
        {
            if (this.OnUse is Subset subset) 
            {
                if (this._UIControl.DisableKeyCodes.Exists(e => e == keyCode)) { return null; }

                var target = subset[axisName];
                var same = subset[keyCode];
                
                if (same != null) 
                {
                    var samePositive = same.Positive == keyCode ? IKeyUnit.EPositive.Positive : IKeyUnit.EPositive.Negative;
                    var sameKeyCode = this._ChangeType == EChangeKey.Delete ? KeyCode.None : target[positive];

                    same.SetAxes(samePositive, sameKeyCode);
                }

                target.SetAxes(positive, keyCode);
                
                return same;
            }

            return null;
        }

        public override IEnumerator<IInputSubset> GetEnumerator() => this._AxesList.GetEnumerator();

        [System.Serializable]
        private new class Subset : InputSet.Subset
        {
            [SerializeField]
            private List<KeyInput> _AxesList;

            public new IKeyUnit this[string name] => this._AxesList.Find(f => f.Name == name);
            
            public IKeyUnit this[KeyCode keyCode]
            {
                get => this._AxesList.Find(f => f.Positive == keyCode || f.Negative == keyCode);
            }

            public override IEnumerator<IInputUnit> GetEnumerator() => this._AxesList.GetEnumerator();
        }

        [System.Serializable]
        public class UIControlInput : IInputSubset
        {
            [SerializeField]
            private KeyInput _Horizontal;
            [SerializeField]
            private KeyInput _Vertical;
            [SerializeField]
            private List<KeyCode> _Confirm;
            [SerializeField]
            private List<KeyCode> _Cancel;

            public KeyInput Horizontal => this._Horizontal;
            public KeyInput Vertical => this._Vertical;

            public bool IsConfirm => this._Confirm.Any(k => Input.GetKeyDown(k));
            public bool IsCancel => this._Cancel.Any(k => Input.GetKeyDown(k));

            public List<KeyCode> DisableKeyCodes => this._Confirm.Concat(this._Cancel).ToList();

            public IInputUnit this[string name] => this.ToList().Find(f => f.Name == name);

            private List<KeyInput> _Axes;

            public bool IsOnSet => true;

            public IEnumerator<IInputUnit> GetEnumerator()
            {
                if (this._Axes == null) 
                {
                    this._Axes = new List<KeyInput>(2)
                    {
                        this._Horizontal,
                        this._Vertical
                    };
                }

                return this._Axes.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
        }

        [System.Serializable]
        private enum EChangeKey 
        {
            Exchange,
            Delete
        }
    }
}
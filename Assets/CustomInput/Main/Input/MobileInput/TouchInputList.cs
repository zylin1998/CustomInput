using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom.InputSystem
{
    [CreateAssetMenu(fileName = "Touch List", menuName = "Custum Input/Touch List", order = 1)]
    public class TouchInputList : InputSet
    {
        [SerializeField]
        private List<Subset> _AxesList;

        public override IEnumerator<IInputSubset> GetEnumerator() => this._AxesList.GetEnumerator();

        [System.Serializable]
        private new class Subset : InputSet.Subset 
        {
            [SerializeField]
            private List<JoyStickAxes> _JoySticks;
            [SerializeField]
            private List<ButtonAxes> _Buttons;

            private List<IInputUnit> _Axes;

            public override IEnumerator<IInputUnit> GetEnumerator()
            {
                if (this._Axes == null)
                {
                    this._Axes = new List<IInputUnit>();

                    this._Axes.AddRange(this._JoySticks);
                    this._Axes.AddRange(this._Buttons);
                }

                return this._Axes.GetEnumerator();
            }
        }
    }
}
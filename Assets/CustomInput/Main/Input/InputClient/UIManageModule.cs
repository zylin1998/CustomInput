using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Custom.UI;

namespace Custom.InputSystem
{
    public class UIManageModule : MonoBehaviour, IInputRequest
    {
        [SerializeField]
        private float _HoldMax = 0.5f;
        [SerializeField]
        private float _HoldMin = 0.1f;
        
        public List<KeyInputList.UIControlInput> UIControls { get; private set; }

        private SelectableMovement Horizontal { get; set; }
        private SelectableMovement Vertical { get; set; }

        private void Start()
        {
            this.UIControls = InputClient.InputSetting.GetSets<KeyInputList>().ConvertAll(c => c.UIControl);

            var horiUnits = this.UIControls.ConvertAll(c => c.Horizontal);
            var vertUnits = this.UIControls.ConvertAll(c => c.Vertical);

            this.Horizontal = new SelectableMovement(horiUnits, SelectableMovement.EAxis.Horizontal);
            this.Vertical = new SelectableMovement(vertUnits, SelectableMovement.EAxis.Vertical);

            SelectableMovement.HoldMax = this._HoldMax;
            SelectableMovement.HoldMin = this._HoldMin;
        }

        public void GetAxes()
        {
            if (this.Group.Interactable)
            {
                this.Move();

                if (this.Confirm()) { return; }

                this.ExpandInput();

                this.Cancel();
            }
        }

        private void Move()
        {
            this.Horizontal.Move();
            this.Vertical.Move();
        }

        private bool Confirm() 
        {
            if (this.UIControls.Any(list => list.IsConfirm))
            {
                if (this.Group.Last is IClick click)
                {
                    click.OnPointerClick(new PointerEventData(EventSystem.current));

                    return true;
                }
            }

            return false;
        }

        private void ExpandInput ()
        {
            if (this.Group.Last is IExpandInput expand)
            {
                expand.ExpandInput();
            }
        }

        private void Cancel() 
        {
            if (this.UIControls.Any(list => list.IsCancel))
            {
                this.Group.Cancel();
            }
        }

        public SelectableGroup Group { get; private set; }

        public void SetSelectableGroup(SelectableGroup group)
        {
            if (group == null) { return; }

            SelectableMovement.Group = group;

            this.Group = group;

            this.Group.OnSet();
        }

        private class SelectableMovement 
        {
            public List<IKeyUnit> KeyUnits { get; private set; }
            public EAxis AxisType { get; private set; }
            
            private float _Hold;
            private float _HoldTime;

            public static float HoldMax { get; set; }
            public static float HoldMin { get; set; }

            public static SelectableGroup Group { get; set; }

            public SelectableMovement(IEnumerable<IKeyUnit> keyUnits, EAxis axis) 
            {
                this.KeyUnits = keyUnits.ToList();
                this.AxisType = axis;
            }

            private float _Current = 0f;

            public void Move()
            {
                var input = this.GetInput();

                this.CheckCurrent(input);

                var direct = this.GetDirect(input);

                if (direct != Vector2.zero)
                {
                    if (this._HoldTime == 0f)
                    {
                        var s = Group.Select(direct);

                        if (!s.selected && Group.OnBorder(direct)) 
                        {
                            InputClient.UIManageModule.SetSelectableGroup(Group.State.Transfer(direct));

                            this.Reset();
                        }
                    }

                    this._HoldTime += Time.deltaTime;

                    if (this._HoldTime >= this._Hold)
                    {
                        this._Hold = HoldMin;
                        this._HoldTime = 0f;
                    }
                }

                if (direct == Vector2.zero) { this.Reset(); }
            }

            private void Reset() 
            {
                this._Hold = HoldMax;
                this._HoldTime = 0f;
            }

            private float GetInput() 
            {
                foreach (var input in this.KeyUnits) 
                {
                    var axis = input.GetAxis(true);

                    if (axis != 0) { return axis; }
                }

                return 0f;
            }

            private void CheckCurrent(float input) 
            {
                if (input != this._Current)
                {
                    this._Current = input;

                    this.Reset();
                }
            }

            private Vector2 GetDirect(float direct) 
            {
                if (this.AxisType == EAxis.Horizontal) 
                {
                    if (direct > 0) { return Vector2.right; }
                    if (direct < 0) { return Vector2.left; }
                }

                if (this.AxisType == EAxis.Vertical)
                {
                    if (direct > 0) { return Vector2.up; }
                    if (direct < 0) { return Vector2.down; }
                }

                return Vector2.zero;
            }

            [System.Serializable]
            public enum EAxis
            {
                Horizontal,
                Vertical
            }
        }
    }

    public interface IExpandInput 
    {
        public void ExpandInput();
    }
}
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using static Loyufei.UI.SelectableGroup;
using UnityEngine.EventSystems;

namespace Loyufei.UI
{
    public class SelectableGroup : MonoBehaviour, IEnumerable<Selectable>
    {
        [Header("Transfers between Groups")]
        [SerializeField]
        private State _State;
        [Header("Group Detail")]
        [SerializeField]
        private Transform _Content;
        [SerializeField]
        private Vector2Int _Capacity;
        [SerializeField]
        private Vector2Int _Location;
        [SerializeField]
        private EBorder _Border;
        [SerializeField]
        private NavigationSetting _Navigation;
        [SerializeField]
        private List<Selectable> _Selectables;
        
        private Selectable _Last;
        
        public State State => this._State;
        public Transform Content => _Content;
        public Vector2Int Capacity => _Capacity;
        public Vector2Int Location => _Location;
        public EBorder Border => _Border;
        public NavigationSetting Navigation => this._Navigation;
        public List<Selectable> Selectables => this._Selectables;
        
        public Selectable this[int x, int y] => _Selectables[this.LocationToIndex(x, y)];
        public Selectable First => _Selectables.First();
        public Selectable Last 
        {
            get
            {
                if (_Last.IsDefault()) { Last = First; }

                return _Last;
            }

            private set 
            {
                _Last = value.IsDefault() ? _Last : value;
                
                _Location = this.IndexToLocation(_Selectables.IndexOf(_Last));
                
                SetBorder(this);

                _Last.Select();
            } 
        }
        
        private void OnDisable()
        {
            _Last?.OnDeselect(null);
        }

        public List<Selectable> GetSelectables() 
        {
            var selectables = _Content.GetComponentsInChildren<Selectable>();

            _Selectables = selectables.ToList();

            return _Selectables;
        }

        public void Select(Selectable selectable)
        {
            this.Last = this._Selectables.Find(f => f == selectable);
        }

        private static void SetBorder(SelectableGroup self)
        {
            var cx = self._Capacity.x;
            var cy = self._Capacity.y;
            var lx = self._Location.x;
            var ly = self._Location.y;

            var flag = new int[4]
                {
                    ly == 1 ? (int)EBorder.Up : 0,
                    ly == cy ? (int)EBorder.Down : 0,
                    lx == 1 ? (int)EBorder.Left : 0,
                    lx == cx ? (int)EBorder.Right : 0
                }.Sum();

            self._Border = (EBorder)flag;
        }

        #region IEnumerable

        public virtual IEnumerator<Selectable> GetEnumerator() => this._Selectables.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        #endregion

        #region Navigation Setting

        [Serializable]
        public class NavigationSetting
        {
            [SerializeField]
            private UnityEngine.UI.Navigation.Mode _NavigationMode;
            [SerializeField]
            private bool _WrapAround;
            [SerializeField]
            private bool _StartSelect;

            public UnityEngine.UI.Navigation.Mode NavigationMode => this._NavigationMode;

            public bool WrapAround => this._WrapAround;
            public bool StartSelect => this._StartSelect;
        }

        [Flags]
        public enum EBorder 
        {
            None = 0,
            Up = 1,
            Down = 2,
            Right = 4,
            Left = 8
        }

        #endregion
    }

    #region Relative Class

    [Serializable]
    public class Transfer
    {
        [SerializeField]
        private SelectableGroup _Group;
        [SerializeField]
        private EBorder _Border;

        public SelectableGroup Group => this._Group;
        public EBorder Border => this._Border;

        public Transfer(SelectableGroup group, EBorder border) 
        {
            this._Group = group;
            this._Border = border;
        }
    }

    [Serializable]
    public class State
    {
        [SerializeField]
        private List<Transfer> _Transfers;

        public Transfer this[EBorder border] => this._Transfers?.Find(f => f.Border == border);

        public List<Transfer> Transfers => this._Transfers;

        public SelectableGroup Transfer(Vector2 direct) 
        {
            if (direct.y > 0) { return this[EBorder.Up]?.Group; }
            if (direct.y < 0) { return this[EBorder.Down]?.Group; }
            if (direct.x > 0) { return this[EBorder.Right]?.Group; }
            if (direct.x < 0) { return this[EBorder.Left]?.Group; }

            return null;
        }
    }

    #endregion
}
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom.UI
{
    public class SelectableGroup : MonoBehaviour, IEnumerable<ISelectable>
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
        
        private List<ISelectable> _Selectables;
        
        private Action OnCancel = () => { };

        public State State => this._State;

        /// <summary>
        /// returns the First ISelectable.
        /// </summary>
        public ISelectable First => this._Selectables[0];

        private ISelectable _Last;

        /// <summary>
        /// returns the Last selected ISelectable. If is null, returns the First.
        /// </summary>
        public ISelectable Last 
        {
            get
            {
                if (this._Last == null)
                {
                    this.Last = this.First;
                }

                return this._Last;
            }

            private set 
            {
                this._Last = value != null ? value : this._Last;
                
                this._Location = this.IndexToLocation(this._Selectables.IndexOf(this._Last));
                
                this.SetBorder();

                this._Last.Select();
            } 
        }
        
        public ISelectable this[string name] => this._Selectables.Find(s => s.Name == name);
        public ISelectable this[int x, int y] => this._Selectables[this.LocationToIndex(x, y)];

        public Transform Content => this._Content;
        public Vector2Int Capacity => this._Capacity;
        public Vector2Int Location => this._Location;
        public EBorder Border => this._Border;

        public bool Interactable { get; set; }

        public event Action CancelEvent 
        {
            add => this.OnCancel += value;
            
            remove => this.OnCancel -= value;
        }

        private void Awake()
        {
            if (_Capacity != Vector2Int.zero) { this.ListInit(); }

            this.Interactable = true;
        }

        private void OnDisable()
        {
            this._Last?.OnDeselect(null);
        }

        #region Selectable Selection

        /// <summary>
        /// get all ISelectables from children.
        /// </summary>
        /// <returns>
        /// ISelectable list from this group.
        /// </returns>
        public List<ISelectable> GetSelectables() 
        {
            this.ListInit().Clear();
            
            this._Selectables.AddRange(this._Content.GetComponentsInChildren<ISelectable>());
            
            return this._Selectables;
        }

        /// <summary>
        /// set up the Navigation between all ISelectables.
        /// </summary>
        public void SetNavigation() 
        {
            var naviMode = this._Navigation.NavigationMode;
            var wrapX = this._Navigation.WrapAroundX;
            var wrapY = this._Navigation.WrapAroundY;
            // capacity must minus one because Location is from (1, 1) to Capacity(x, y)
            var capacity = this._Capacity - Vector2Int.one;
            var capacityX = capacity.x;
            var capacityY = capacity.y;

            var count = 0;
            this._Selectables.ForEach(selectable => 
            {
                // locate must minus one because Location is from (1, 1) to Capacity(x, y)
                var locate = this.IndexToLocation(count) - Vector2Int.one;
                var x = locate.x;
                var y = locate.y;
                
                var navi = new Navigation();

                if (naviMode.HasFlag(ENavigationMode.Horizontal)) 
                {
                    navi.Right = x == capacityX ? wrapX ? this[0, y] : null : this[x + 1, y];
                    navi.Left = x == 0 ? wrapX ? this[capacityX, y] : null : this[x - 1, y];
                }

                if (naviMode.HasFlag(ENavigationMode.Vertical))
                {
                    navi.Up = y == 0 ? wrapX ? this[x, capacityY] : null : this[x, y - 1];
                    navi.Down = y == capacityY ? wrapX ? this[x, 0] : null : this[x, y + 1];
                }

                selectable.Navigation = navi;

                count++;
            });
        }

        /// <summary>
        /// select next ISelectable by Vector2.
        /// </summary>
        /// <returns>
        /// returns current ISelectable and does any ISelectable been selected.
        /// </returns>
        public (ISelectable selectable, bool selected) Select(Vector2 direct)
        {
            var selectable = this.Last.Navigation.Select(direct);
            var selected = selectable != null;

            if (selected) { this.Last = selectable; }

            return (this.Last, selected);
        }

        public void Select(ISelectable selectable)
        {
            this.Last = this._Selectables.Find(f => f == selectable);
        }

        #endregion

        #region Border Check

        public bool OnBorder(EBorder border) 
        {
            return this._Border.HasFlag(border);
        }

        public bool OnBorder(Vector2 direct) 
        {
            bool[] isBorder =
            {
                (direct.y > 0 && this.OnBorder(EBorder.Up)),
                (direct.y < 0 && this.OnBorder(EBorder.Down)),
                (direct.x > 0 && this.OnBorder(EBorder.Right)),
                (direct.x < 0 && this.OnBorder(EBorder.Left))
            };
            
            return isBorder.Any(a => a);
        }

        #endregion

        #region Events

        /// <summary>
        /// invoke events when the group was canceled.
        /// </summary>
        public void Cancel() 
        {
            this.OnCancel?.Invoke();
        }

        /// <summary>
        /// set up the group when setting to 
        /// <see cref="InputSystem.UIManageModule"/>
        /// </summary>
        public void OnSet()
        {
            if (_Navigation.StartSelect) 
            {
                this.Last.Select(); 
            }
        }

        #endregion

        #region Private Method

        private List<ISelectable> ListInit() 
        {
            if (this._Selectables == null) 
            {
                var capacity = this._Capacity.x * this._Capacity.y;

                this._Selectables = new List<ISelectable>(capacity);
            }

            return this._Selectables;
        }

        private Vector2Int IndexToLocation(int interger) 
        {
            int y = interger / this._Capacity.x + 1;
            int x = interger % this._Capacity.x + 1;

            return Vector2Int.Min(new Vector2Int(x, y), this._Capacity);
        }

        private int LocationToIndex(int x, int y) 
        {
            var result = x + this._Capacity.x * y;

            return result >= this._Selectables.Count ? this._Selectables.Count - 1 : result; 
        }

        private void SetBorder()
        {
            var cx = this._Capacity.x;
            var cy = this._Capacity.y;
            var lx = this._Location.x;
            var ly = this._Location.y;

            var flag = new int[4]
                {
                    ly == 1 ? (int)EBorder.Up : 0,
                    ly == cy ? (int)EBorder.Down : 0,
                    lx == 1 ? (int)EBorder.Left : 0,
                    lx == cx ? (int)EBorder.Right : 0
                }.Sum();

            this._Border = (EBorder)flag;
        }

        #endregion

        #region IEnumerable

        public virtual IEnumerator<ISelectable> GetEnumerator() => this._Selectables.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        #endregion

        #region Object Definition

        [Serializable]
        public class NavigationSetting
        {
            [SerializeField]
            private ENavigationMode _NavigationMode;
            [SerializeField]
            private bool _WrapAroundX;
            [SerializeField]
            private bool _WrapAroundY;
            [SerializeField]
            private bool _StartSelect;

            public ENavigationMode NavigationMode => this._NavigationMode;

            public bool WrapAroundX => this._WrapAroundX;
            public bool WrapAroundY => this._WrapAroundY;
            public bool StartSelect => this._StartSelect;
        }

        [Flags]
        public enum ENavigationMode
        {
            None = 0,
            Horizontal = 1,
            Vertical = 2,
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
        private SelectableGroup.EBorder _Border;

        public SelectableGroup Group => this._Group;
        public SelectableGroup.EBorder Border => this._Border;

        public Transfer(SelectableGroup group, SelectableGroup.EBorder border) 
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

        public Transfer this[SelectableGroup.EBorder border] => this._Transfers?.Find(f => f.Border == border);

        public List<Transfer> Transfers => this._Transfers;

        public SelectableGroup Transfer(Vector2 direct) 
        {
            if (direct.y > 0) { return this[SelectableGroup.EBorder.Up]?.Group; }
            if (direct.y < 0) { return this[SelectableGroup.EBorder.Down]?.Group; }
            if (direct.x > 0) { return this[SelectableGroup.EBorder.Right]?.Group; }
            if (direct.x < 0) { return this[SelectableGroup.EBorder.Left]?.Group; }

            return null;
        }
    }

    #endregion
}
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Loyufei.InputSystem;

namespace Loyufei.InputSystem
{
    public abstract class InputSet : ScriptableObject, IInputSet
    {
        [SerializeField]
        private EInputMode _InputMode;
        
        public EInputMode InputMode 
        {
            get => _InputMode; 
            
            protected set => _InputMode = value; 
        }
        
        public Dictionary<string, IInputUnit> Dictionary => this.OnUse.ToDictionary(x => x.Name, y => y);

        private Subset _OnUse;

        public IInputSubset OnUse
        {
            get
            {
                if (this._OnUse == null)
                {
                    this.SetOnUse(0);
                }

                return this._OnUse;
            }

            protected set
            {
                if (value is Subset axes)
                {
                    this._OnUse?.OnUse(false);

                    this._OnUse = axes;

                    this._OnUse.OnUse(true);
                }
            }
        }

        public IInputSubset SetOnUse(int index)
        {
            var list = this.ToList();

            if (index >= list.Count) { return default; }

            this.OnUse = list[index];

            return this.OnUse;
        }

        public abstract IEnumerator<IInputSubset> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        [System.Serializable]
        protected abstract class Subset : IInputSubset
        {
            [SerializeField]
            protected bool _IsOnSet;

            public bool IsOnSet => this._IsOnSet;

            public IInputUnit this[string name] => this.ToList().Find(f => f.Name == name);

            public void OnUse(bool onUse) 
            {
                this._IsOnSet = onUse;
            }

            public abstract IEnumerator<IInputUnit> GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
        }
    }

    public interface IInputSet : IEnumerable<IInputSubset>
    {
        public EInputMode InputMode { get; }
        public IInputSubset OnUse { get; }
        public Dictionary<string, IInputUnit> Dictionary { get; }

        public IInputSubset SetOnUse(int index);
    }

    public interface IInputSubset : IEnumerable<IInputUnit>
    {
        public bool IsOnSet { get; }

        public IInputUnit this[string name] { get; }
    }
}
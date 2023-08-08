using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom.UI
{
    public interface ISlot 
    {
        public object Content { get; }

        public void SetSlot(object content);
        public void UpdateSlot();
        public void ClearSlot();
    }

    public interface ISlot<TContent> : ISlot
    {
        public new TContent Content { get; }

        public void SetSlot(TContent content);
        public new void UpdateSlot();
        public new void ClearSlot();

        void ISlot.SetSlot(object content) 
        {
            if (content is TContent c) { this.SetSlot(c); }
        }

        void ISlot.UpdateSlot() => this.UpdateSlot();
        void ISlot.ClearSlot() => this.ClearSlot();
    }
}
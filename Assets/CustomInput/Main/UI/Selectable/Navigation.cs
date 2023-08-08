using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom.UI
{
    public struct Navigation
    {
        public ISelectable Up { get; set; }
        public ISelectable Down { get; set; }
        public ISelectable Right { get; set; }
        public ISelectable Left { get; set; }

        /// <summary>
        /// set ISelectable navigation by
        /// <see cref="EDirect"/>
        /// </summary>
        /// <param name="selectable">選擇物件</param>
        /// <param name="direct">設定方向</param>
        public void SetSelectable(ISelectable selectable, EDirect direct)
        {
            if (direct == EDirect.Up) { this.Up = selectable; }
            if (direct == EDirect.Down) { this.Down = selectable; }
            if (direct == EDirect.Right) { this.Right = selectable; }
            if (direct == EDirect.Left) { this.Left = selectable; }
        }

        /// <summary>
        /// select ISelectable by
        /// <see cref="EDirect"/>
        /// </summary>
        /// <param name="direct"></param>
        /// <returns></returns>
        public ISelectable GetSelectable(EDirect direct)
        {
            if (direct == EDirect.Up) { return this.Up; }
            if (direct == EDirect.Down) { return this.Down; }
            if (direct == EDirect.Right) { return this.Right; }
            if (direct == EDirect.Left) { return this.Left; }

            return default;
        }

        /// <summary>
        /// select ISelectable by
        /// <see cref="EDirect"/>
        /// </summary>
        /// <param name="direct"></param>
        /// <param name="eventData"></param>
        public ISelectable Select(EDirect direct)
        {
            return this.GetSelectable(direct);
        }

        /// <summary>
        /// select ISelectable by
        /// <see cref="Vector2"/>
        /// </summary>
        /// <param name="direct"></param>
        /// <param name="eventData"></param>
        /// <returns></returns>
        public ISelectable Select(Vector2 direct) 
        {
            if (direct.y > 0) { return this.Select(EDirect.Up); }
            if (direct.y < 0) { return this.Select(EDirect.Down); }
            if (direct.x > 0) { return this.Select(EDirect.Right); }
            if (direct.x < 0) { return this.Select(EDirect.Left); }

            return default;
        }

        public override string ToString() => string.Format(
            "Up: {0}\nDown: {1}\nRight: {2}\nLeft: {3}"
            , this.Up is Component up ? up.name : "None"
            , this.Down is Component down ? down.name : "None"
            , this.Right is Component right ? right.name : "None"
            , this.Left is Component left ? left.name : "None");
    }

    [System.Serializable]
    public enum EDirect
    {
        Up,
        Down,
        Right,
        Left
    }
}
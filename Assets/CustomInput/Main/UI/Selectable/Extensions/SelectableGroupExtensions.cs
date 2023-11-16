using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Loyufei.UI
{
    using static Loyufei.UI.SelectableGroup;

    public static class SelectableGroupExtensions
    {
        public static Vector2Int IndexToLocation(this SelectableGroup self, int interger)
        {
            int y = interger / self.Capacity.x + 1;
            int x = interger % self.Capacity.x + 1;

            return Vector2Int.Min(new Vector2Int(x, y), self.Capacity);
        }

        public static int LocationToIndex(this SelectableGroup self, int x, int y)
        {
            var result = x + self.Capacity.x * y;

            return result >= self.Selectables.Count ? self.Selectables.Count - 1 : result;
        }

        public static void SetNavigation(this SelectableGroup self)
        {
            var naviMode = self.Navigation.NavigationMode;
            var wrapAround = self.Navigation.WrapAround;

            if (naviMode.HasFlag(Navigation.Mode.Explicit)) { return; }

            self.Selectables.ForEach(selectable =>
            {
                var navigation = new Navigation();

                navigation.mode = naviMode;
                navigation.wrapAround = wrapAround;

                selectable.navigation = navigation;
            });
        }

        public static Selectable Select(this SelectableGroup self, Vector2 direct)
        {
            var selectable = self.Last.Select(direct);
            
            if (selectable) { self.Select(selectable); }

            return self.Last;
        }

        public static bool OnBorder(this SelectableGroup self, EBorder border)
        {
            return self.Border.HasFlag(border);
        }

        public static bool OnBorder(this SelectableGroup self, Vector2 direct)
        {
            bool[] isBorder =
            {
                (direct.y > 0 && self.OnBorder(EBorder.Up)),
                (direct.y < 0 && self.OnBorder(EBorder.Down)),
                (direct.x > 0 && self.OnBorder(EBorder.Right)),
                (direct.x < 0 && self.OnBorder(EBorder.Left))
            };

            return isBorder.Any(a => a);
        }
    }
}

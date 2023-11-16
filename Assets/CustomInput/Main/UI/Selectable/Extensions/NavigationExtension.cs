using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Loyufei.UI
{
    public static class NavigationExtension
    {
        public static Selectable Select(this Selectable self, Vector2 direct)
        {
            if (direct.y > 0) { return self.FindSelectableOnUp(); }
            if (direct.y < 0) { return self.FindSelectableOnDown(); }
            if (direct.x > 0) { return self.FindSelectableOnRight(); }
            if (direct.x < 0) { return self.FindSelectableOnLeft(); }

            return default;
        }

        public static Selectable Select(this Navigation navigation, Vector2 direct) 
        {
            if (direct.y > 0) { return navigation.selectOnUp; }
            if (direct.y < 0) { return navigation.selectOnDown; }
            if (direct.x > 0) { return navigation.selectOnRight; }
            if (direct.x < 0) { return navigation.selectOnLeft; }
            
            return default;
        }

        public static void SetNavigation(this Navigation navigation, Selectable selectable, Vector2 direct) 
        {
            if (direct.y > 0) { navigation.selectOnUp = selectable; }
            if (direct.y < 0) { navigation.selectOnDown = selectable; }
            if (direct.x > 0) { navigation.selectOnRight = selectable; }
            if (direct.x < 0) { navigation.selectOnLeft = selectable; }
        }
    }
}

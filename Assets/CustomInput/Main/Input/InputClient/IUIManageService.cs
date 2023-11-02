using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Custom.UI;

namespace Loyufei.InputSystem
{
    public interface IUIManageService
    {
        public SelectableGroup Current { get; }

        public void Transfer(Vector2 direct);
    }
}

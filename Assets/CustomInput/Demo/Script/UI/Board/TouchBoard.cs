using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom.InputSystem
{
    public class TouchBoard : MonoBehaviour
    {
        void Start()
        {
            if (!InputClient.InputMode.HasFlag(InputClient.EInputMode.TouchMode))
            {
                Destroy(this.gameObject);
            }
        }
    }
}
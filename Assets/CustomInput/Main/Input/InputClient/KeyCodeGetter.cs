using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.InputSystem;

namespace Loyufei.InputSystem
{
    public class KeyCodeGetter : MonoBehaviour
    {
        private bool _WaitKey;
        private KeyCode _KeyCode;
        private Event _Event;

        private void OnGUI()
        {
            _Event = Event.current;

            if (!_WaitKey) { return; }

            if (_Event.isKey)
            {
                _KeyCode = _Event.keyCode;
            }

            if (_Event.isMouse)
            {
                _KeyCode = (KeyCode)(_Event.button + 323);
            }
        }

        private IEnumerator WaitKey(Action<KeyCode> onGet)
        {
            yield return CheckKeyRelease();

            _WaitKey = true;
            UIManageModule.Current.Current.Interactable = false;

            yield return CheckKeyPress();

            _WaitKey = false;

            onGet.Invoke(_KeyCode);
            
            _KeyCode = KeyCode.None;

            yield return CheckKeyRelease();

            this.gameObject.SetActive(false);
            UIManageModule.Current.Current.Interactable = true;
        }

        private IEnumerator CheckKeyRelease() 
        {
            for (; Input.anyKeyDown || Input.anyKey;)
            {
                yield return null;
            }
        }

        private IEnumerator CheckKeyPress()
        {
            for (; !Input.anyKeyDown;)
            {
                yield return null;
            }
        }

        public void GetKeyCode(Action<KeyCode> onGet)
        {
            this.gameObject.SetActive(true);

            StartCoroutine(WaitKey(onGet));
        }
    }
}

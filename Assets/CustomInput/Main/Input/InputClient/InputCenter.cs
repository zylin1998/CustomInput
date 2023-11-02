using Custom.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Custom.InputSystem;
using System;

namespace Loyufei.InputSystem
{
    [RequireComponent(typeof(UIManageModule))]
    public class InputCenter : MonoBehaviour
    {
        [SerializeField]
        private InputSetting _InputSetting;

        public IInputRequest Core { get; private set; }
        public Stack<IInputRequest> Requests { get; private set; } = new Stack<IInputRequest>();
        
        public IInputRequest Current 
            => Requests.Any() ? Requests.Peek() : Core;

        public IInputSetting InputSetting => _InputSetting;
        public UIManageModule UIManageModule { get; private set; }

        public bool CheckInputMode(EInputMode inputMode) 
        {
            return InputSetting.ToList().Exists(set => set.InputMode.Equals(inputMode));
        }

        private void Awake()
        {
            InputManager.SetInputSetting(this._InputSetting);

            var gameobj = new GameObject("KeyCodeGetter");

            KeyCodeGetter = gameobj.AddComponent<KeyCodeGetter>();

            gameobj.SetActive(false);
        }

        private void Start()
        {
            UIManageModule = GetComponent<UIManageModule>();
        }

        private void Update()
        {
            Current?.GetAxes();
        }

        public void SetInput(InputSetting setting) 
        {
            this._InputSetting = setting;

            InputManager.SetInputSetting(this._InputSetting);
        }

        #region Request Setting

        public void SetRequest(IInputRequest request, bool core = false) 
        {
            if (core) { Core = request; }

            else { Requests.Push(request); }
        }

        public void ClearRequest(IInputRequest request)
        {
            if(Current == request) { Requests.Pop(); }

            else { Debug.Log("Wrong Request"); }
        }

        public void ClearRequest(IUIManageService request)
        {
            UIManageModule.CleaerRequest(request);  
        }

        public void ClearRequests(bool clearCore = false)
        {
            if (clearCore) { Core = null; }

            Requests.Clear();
        }

        public void SetRequest(IUIManageService service, bool isCore = false)
        {
            if (UIManageModule == null) { UIManageModule = GetComponent<UIManageModule>(); }

            UIManageModule.SetRequest(service, isCore);
        }

        #endregion

        #region Set Touch Input

        public void SetTouchInput(string name, ITouchInput touchInput)
        {
            var axes = InputSetting.GetSet<TouchInputList>().OnUse[name];

            if (axes is ITouchUnit mobileAxes) { mobileAxes.SetAxes(touchInput); }
        }

        public void SetTouchInput(ITouchButton touchButton)
        {
            SetTouchInput(touchButton.AxesName, touchButton);
        }

        public void SetTouchInput(IVJoyStick joyStick)
        {
            SetTouchInput(joyStick.Horizontal, joyStick);
            SetTouchInput(joyStick.Vertical, joyStick);
        }

        #endregion

        #region Change Key

        public KeyCodeGetter KeyCodeGetter { get; private set; } 

        public void GetKeyCode(Action<KeyCode> onGet) 
        {
            KeyCodeGetter.GetKeyCode(onGet);
        }

        #endregion
    }

    [Flags]
    public enum EInputMode
    {
        KeyMode = 1,
        TouchMode = 2
    }

    public interface IInputRequest
    {
        public void GetAxes();

        public static bool operator true(IInputRequest inputClient) => inputClient != null;
        public static bool operator false(IInputRequest inputClient) => inputClient == null;
        public static bool operator !(IInputRequest inputClient) => inputClient == null;
    }
}
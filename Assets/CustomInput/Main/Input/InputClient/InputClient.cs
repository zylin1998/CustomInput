using System.Linq;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Custom.UI;

namespace Custom.InputSystem
{
    [RequireComponent(typeof(UIManageModule))]
    public class InputClient : MonoBehaviour
    {
        [SerializeField]
        private InputSetting _InputSetting;
        [SerializeField, ReadOnly]
        private RuntimePlatform _Platform;
        [SerializeField, ReadOnly]
        private EInputMode _InputMode;
        [SerializeField, ReadOnly]
        private UIManageModule _UIManageModule;

        private List<IInputSet> _InputSets = new List<IInputSet>();
        private IInputRequest _Main;
        private IInputRequest _Current;

        public bool Pause => this._InputSets.Count <= 0 || this.Current == null;

        public IInputRequest Main
        {
            get => this._Main;

            private set
            {
                this._Main = !value ? this._Current : value;

                if (!this._Current)
                {
                    if (this._Main) { this._Current = this._Main; }
                }
            }
        }

        public IInputRequest Current
        {
            get => this._Current;

            private set
            {
                this._Current = !value ? this._Main : value;

                if (!this._Main)
                {
                    if (this._Current) { this._Main = this._Current; }
                }
            }
        }

        #region Behavior

        private void Awake()
        {
            this._Platform = Application.platform;
            this._UIManageModule = this.GetComponent<UIManageModule>();

            if (DontDestroy) { DontDestroyOnLoad(this.gameObject); }
        }

        private void Update()
        {
            if (!Pause) { Client.Current?.GetAxes(); }
        }

        public void OnDestroy()
        {
            _Client = null;
        }

        #endregion

        #region Static Properties

        private static InputClient _Client;

        public static InputClient Client => GetInstance();
        public static IInputSetting InputSetting => Client._InputSetting;
        public static EInputMode InputMode => Client._InputMode;
        public static RuntimePlatform Platform => Client._Platform;
        public static List<IInputSet> InputSets => Client._InputSets;
        public static UIManageModule UIManageModule => Client._UIManageModule;
        public static bool DontDestroy { get; private set; }

        #endregion

        #region GetValue

        public static List<IInputUnit> GetAxes(string name)
        {
            return InputSets.FindAll(l => l.OnUse[name] != null).ConvertAll(c => c.OnUse[name]);
        }

        public static List<TAxes> GetAxes<TAxes>(string name) where TAxes : IInputUnit
        {
            return GetAxes(name).FindAll(f => f is TAxes).ConvertAll(c => (TAxes)c);
        }

        public static List<InputValue> GetAxesValue(string name)
        {
            return GetAxes(name).ConvertAll(c => c.Value);
        }

        public static float GetAxis(string name)
        {
            var hasAxis = GetAxesValue(name).FindAll(f => f.Axis != 0);

            return hasAxis.Count > 0 ? hasAxis[0].Axis : 0f;
        }

        public static bool GetKeyDown(string name) => GetAxesValue(name).Any(axes => axes.GetKeyDown);
        public static bool GetKey(string name) => GetAxesValue(name).Any(axes => axes.GetKey);
        public static bool GetKeyUp(string name) => GetAxesValue(name).Any(axes => axes.GetKeyUp);

        #endregion

        #region Setting

        public static InputClient GetInstance(bool dontDestroy = true) 
        {
            DontDestroy = dontDestroy;

            if (!_Client) { _Client = new GameObject("InputClient").AddComponent<InputClient>(); }
            
            return _Client;
        }

        public static void SetRequest(IInputRequest request, bool isBasic = false) 
        {
            if (isBasic) { Client.Main = request; }

            else { Client.Current = request; }
        }

        public static void SetRequest(SelectableGroup client, bool isBasic = false)
        {
            if (Client._UIManageModule == null) { Client._UIManageModule = _Client.GetComponent<UIManageModule>(); }

            Client._UIManageModule.SetSelectableGroup(client);
            
            SetRequest(Client._UIManageModule, isBasic);
        }

        public static void SetPlatform(RuntimePlatform platform) 
        {
            Client._Platform = platform;
            Client._InputSets = InputSetting[Client._Platform];

            var key = InputSets.Exists(l => l.InputMode.HasFlag(EInputMode.KeyMode)) ? 1 : 0;
            var touch = InputSets.Exists(l => l.InputMode.HasFlag(EInputMode.TouchMode)) ? 2 : 0;
            
            Client._InputMode = (EInputMode)(key + touch);
        }

        public static void SetInput(InputSetting inputSetting) 
        {
            Client._InputSetting = inputSetting;
            Client._InputSets = InputSetting[Client._Platform];

            var key = InputSets.Exists(l => l.InputMode.HasFlag(EInputMode.KeyMode)) ? 1 : 0;
            var touch = InputSets.Exists(l => l.InputMode.HasFlag(EInputMode.TouchMode)) ? 2 : 0;
            
            Client._InputMode = (EInputMode)(key + touch);
        }

        public static void ClearCurrentRequest(IInputRequest inputRequest) 
        {
            if (!_Client) { return; }

            if (Client.Current == inputRequest) 
            {
                Client.Current = null;
            }
        }

        public static void ClearCurrentRequest()
        {
            if (!_Client) { return; }

            Client.Current = null;
        }

        public static void ClearAll() 
        {
            if (!_Client) { return; }

            Client._Main = null;
            Client._Current = null;
        }

        #region Set Touch Input

        public static void SetTouchInput(string name, ITouchInput touchInput) 
        {
            var axes = InputSetting.GetSet<TouchInputList>().OnUse[name];

            if (axes is ITouchUnit mobileAxes) { mobileAxes.SetAxes(touchInput); }
        }

        public static void SetTouchInput(ITouchButton touchButton)
        {
            SetTouchInput(touchButton.AxesName, touchButton);
        }
        
        public static void SetTouchInput(IVJoyStick joyStick)
        {
            SetTouchInput(joyStick.Horizontal, joyStick);
            SetTouchInput(joyStick.Vertical, joyStick);
        }

        private static bool _WaitKey;
        private static KeyCode _KeyCode;

        private void OnGUI()
        {
            if (!_WaitKey) { return; }

            var keyEvent = Event.current;

            if (keyEvent.isKey)
            {
                _KeyCode = keyEvent.keyCode;
            }

            if (keyEvent.isMouse)
            {
                _KeyCode = (KeyCode)(keyEvent.button + 323);
            }
        }

        public static async Task<KeyCode> GetKeyCode()
        {
            await Task.Delay(250);

            _WaitKey = true;

            var result = await Task<KeyCode>.Factory.StartNew(() => 
            {
                for (; _KeyCode == KeyCode.None;)
                {
                    //Debug
                }
                
                return _KeyCode;
            });
            
            _KeyCode = KeyCode.None;
            _WaitKey = false;

            return result;
        }

        #endregion

        #endregion

        [System.Flags]
        public enum EInputMode 
        {
            KeyMode = 1,
            TouchMode = 2
        }
    }

    public interface IInputRequest
    {
        public void GetAxes();

        public static bool operator true(IInputRequest inputClient) => inputClient != null;
        public static bool operator false(IInputRequest inputClient) => inputClient == null;
        public static bool operator !(IInputRequest inputClient) => inputClient == null;
    }
}
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Loyufei.InputSystem
{
    public class InputCenter : MonoBehaviour
    {
        [SerializeField]
        private InputCollection _InputCollection;
        [SerializeField]
        private EInputMode _InputMode;

        public IInputRequest Major { get; private set; }
        public Stack<IInputRequest> Requests { get; private set; } = new Stack<IInputRequest>();
        
        public IInputRequest Current 
            => Requests.Any() ? Requests.Peek() : Major;

        public InputCollection InputCollection => _InputCollection;

        public EInputMode InputMode
        {
            get => _InputMode;
            set => _InputMode = value;
        }

        public static Dictionary<string, List<IInputUnit>> AxesDictionary { get; private set; }
            = new Dictionary<string, List<IInputUnit>>();

        #region Script Behavior

        private void Awake()
        {
            Catcher = SetCatcher();
            
            SetInput(_InputCollection);

            InputSystemProperty.InputCenter = this;
        }

        private void Update()
        {
            Current?.GetAxes();
        }

        #endregion

        #region InputCenter Setting

        public void SetInput(InputCollection inputSetting) 
        {
            if (inputSetting.IsDefault()) { return; }

            _InputCollection = inputSetting;

            AxesDictionary = InputCollection?.Dictionary;

            gameObject.AddComponent<UIControlInput>().KeyInputList = InputCollection?.GetSet<KeyInputList>();
        }

        public bool CheckInputMode(EInputMode inputMode) 
        {
            return InputMode.HasFlag(inputMode);
        }

        private KeyCodeCatcher SetCatcher() 
        {
            var catcher = new GameObject("KeyCodeCatcher");

            catcher.transform.parent = transform;
            catcher.SetActive(false);

            return catcher.AddComponent<KeyCodeCatcher>();
        }

        #endregion

        #region Request Setting

        public void SetRequest(IInputRequest request) => SetRequest(request, false);

        public void SetRequest(IInputRequest request, bool isMajor) 
        {
            if (request.IsDefault()) { return; }

            if (isMajor) { Major = request; }

            else { Requests.Push(request); }

            Current.Setup();
        }

        public void ClearRequest(IInputRequest request) => ClearRequest(request, false);

        public void ClearRequest(IInputRequest request, bool clearMajor = false)
        {
            if (request.IsDefault()) { return; }

            if (!request.Equals(Current)) { return; }

            if (clearMajor && !request.Equals(Major)) { return; }

            var remove = Current;

            if (clearMajor) { Major = request; }

            else { Requests.Pop(); }

            remove.UnSet();
        }

        #endregion

        #region Set Touch Input

        public void SetTouchInput(ITouchInput touchInput)
        {
            var unit = InputCollection.GetSet<TouchInputList>().OnUse[touchInput.AxesName];

            if (unit is ITouchUnit touchUnit) { touchUnit.SetInput(touchInput); }
        }

        public void SetTouchInput(IVJoyStick joyStick)
        {
            SetTouchInput(joyStick.Horizontal);
            SetTouchInput(joyStick.Vertical);
        }

        public void ClearTouchInput(ITouchInput touchInput) 
        {
            var unit = InputCollection.GetSet<TouchInputList>().OnUse[touchInput.AxesName];

            if (unit is ITouchUnit touchUnit) { touchUnit.ClearInput(touchInput); }
        }

        public void ClearTouchInput(IVJoyStick joyStick)
        {
            SetTouchInput(joyStick.Horizontal);
            SetTouchInput(joyStick.Vertical);
        }

        #endregion

        #region Change Key

        private KeyCodeCatcher Catcher { get; set; }

        public void ChangeKey(string unitName, IKeyUnit.EPositive positive, Action<IKeyUnit, IKeyUnit> onChange) 
        {
            var keyList = InputCollection.GetSet<KeyInputList>();

            Catcher.GetKeyCode((keyCode =>
            {
                var response = keyList.ChangeKey(unitName, positive, keyCode);
                
                onChange.Invoke(response.change, response.exchange);
            }));
        }

        #endregion

        [AddComponentMenu("UI Cintroll Input")]
        internal class UIControlInput : BaseInput 
        {
            public KeyInputList KeyInputList { get; set; }

            protected override void Awake()
            {
                base.Awake();

                FindAnyObjectByType<BaseInputModule>().inputOverride = this;
            }

            public override float GetAxisRaw(string axisName)
            {
                return KeyInputList.UIControlUnit(axisName).GetAxis(true);
            }

            public override bool GetButtonDown(string buttonName)
            {
                return KeyInputList.UIControlUnit(buttonName).GetKeyDown;
            }
        }

        [AddComponentMenu("KeyCode Catcher")]
        internal class KeyCodeCatcher : MonoBehaviour
        {
            private bool _WaitKey;
            private KeyCode _KeyCode;

            private void OnGUI()
            {
                var e = Event.current;

                if (!_WaitKey) { return; }

                if (e.isKey) { _KeyCode = e.keyCode; }

                if (e.isMouse) { _KeyCode = (KeyCode)(e.button + 323); }
            }

            private IEnumerator WaitKey(Action<KeyCode> onGet)
            {
                yield return WaitKeyRelease();

                _WaitKey = true;

                yield return WaitKeyPress();

                _WaitKey = false;

                onGet.Invoke(_KeyCode);

                _KeyCode = KeyCode.None;

                yield return WaitKeyRelease();

                gameObject.SetActive(false);
            }

            private IEnumerator WaitKeyRelease()
            {
                for (; Input.anyKeyDown || Input.anyKey;)
                {
                    yield return null;
                }
            }

            private IEnumerator WaitKeyPress()
            {
                for (; _KeyCode == KeyCode.None;)
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

    [Flags]
    public enum EInputMode
    {
        Touch = 1,
        Keyboard = 2,
        GameController = 4,
    }

    public interface IInputRequest
    {
        public void GetAxes();

        public void Setup();
        public void UnSet();
        
        public static bool operator true(IInputRequest inputClient) => inputClient != null;
        public static bool operator false(IInputRequest inputClient) => inputClient == null;
        public static bool operator !(IInputRequest inputClient) => inputClient == null;
    }
}
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.UI;
using Custom.InputSystem;
using Loyufei.InputSystem;
using Loyufei;

namespace InputDemo
{
    public class InitBoard : MonoBehaviour, IUIManageService
    {
        [SerializeField]
        private SelectableGroup _Input;
        [SerializeField]
        private SelectableGroup _Start;
        [SerializeField]
        private CanvasGroup _CanvasGroup;
        [SerializeField]
        private InputCenter _InputCenter;

        public static IClick StartButton { get; private set; }

        public KeyInputList KeyInputList { get; private set; }
        public List<InputSlot> InputSlots { get; private set;}

        public SelectableGroup Current { get; set; }

        public void Transfer(Vector2 direct)
        { 
            var transfer = Current.State.Transfer(direct);

            Current = transfer.IsDefaultandReturn(Current);
            Current.Last.Select();
        }

        private void Awake()
        {
            this._Input.GetSelectables();
            this._Start.GetSelectables();
            
            if (this._Start["Start"] is IClick start)
            {
                StartButton = start;

                start.ClickEvent += (data) => this.Started();
            }
        }

        private void Start()
        {
            this.CheckInputMode();

            this._Input.SetNavigation();
            this._Start.SetNavigation();

            this.KeyInputList = _InputCenter.InputSetting.GetSetbyName<KeyInputList>("Keyboard");

            this.InputSlots = this._Input.OfType<InputSlot>().ToList();

            this.InputSlots.ForEach(f => this.SetSlot(f));

            ReStartBoard.ReStartButton.ClickEvent += (data) => this.ReStarted();

            this.Current = _Input.gameObject.activeSelf ? _Input : _Start;

            _InputCenter.SetRequest(this);
        }

        private void Started() 
        {
            this.gameObject.SetActive(false);

            _InputCenter.ClearRequest(this);
        }

        private void ReStarted() 
        {
            this.gameObject.SetActive(true);

            _InputCenter.SetRequest(this);

            this._Input.Select(this._Input.First);
        }

        private void SetSlot(InputSlot slot) 
        {
            var unit = this.KeyInputList.OnUse[slot.AxesName] as IKeyUnit;

            slot.SetSlot(unit);

            slot.ClickEvent += (data) =>
            {
                _InputCenter.GetKeyCode(keyCode =>
                {
                    var same = this.KeyInputList.ChangeKey(slot.AxesName, slot.Positive, keyCode);

                    this.InputSlots
                        .FindAll(f => f.AxesName == same?.Name)
                        .ForEach(f => f.UpdateSlot());
                });
            };
        }

        private void CheckInputMode() 
        {
            if (!_InputCenter.CheckInputMode(EInputMode.KeyMode))
            {
                this._Start.State.Transfers.Clear();

                this._Input.Content.gameObject.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            StartButton = null;
        }
    }
}
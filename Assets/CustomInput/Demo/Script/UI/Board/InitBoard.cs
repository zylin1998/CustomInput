using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.UI;
using Custom.InputSystem;

namespace InputDemo
{
    public class InitBoard : MonoBehaviour
    {
        [SerializeField]
        private SelectableGroup _Input;
        [SerializeField]
        private SelectableGroup _Start;
        [SerializeField]
        private CanvasGroup _CanvasGroup;

        public static IClick StartButton { get; private set; }

        public KeyInputList KeyInputList { get; private set; }
        public List<InputSlot> InputSlots { get; private set;}

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

            this.KeyInputList = InputClient.InputSetting.GetSetbyName<KeyInputList>("Keyboard");

            this.InputSlots = this._Input.OfType<InputSlot>().ToList();

            this.InputSlots.ForEach(f => this.SetSlot(f));

            ReStartBoard.ReStartButton.ClickEvent += (data) => this.ReStarted();

            InputClient.SetRequest(this._Input.gameObject.activeSelf ? this._Input : this._Start);
        }

        private void Started() 
        {
            this.gameObject.SetActive(false);

            InputClient.ClearCurrentRequest();
        }

        private void ReStarted() 
        {
            this.gameObject.SetActive(true);

            InputClient.SetRequest(this._Input);

            this._Input.Select(this._Input.First);
        }

        private void SetSlot(InputSlot slot) 
        {
            var unit = this.KeyInputList.OnUse[slot.AxesName] as IKeyUnit;

            slot.SetSlot(unit);

            slot.ClickEvent += (data) =>
            {
                this.ChangeKey(slot);
            };
        }

        private void CheckInputMode() 
        {
            if (!InputClient.InputMode.HasFlag(InputClient.EInputMode.KeyMode))
            {
                var transfors = this._Start.State.Transfers;

                transfors.Remove(transfors.Find(f => f.Group == this._Input));

                this._Input.Content.gameObject.SetActive(false);
            }
        }

        private async void ChangeKey(InputSlot slot) 
        {
            this._Input.Interactable = false;

            var keyCode = await InputClient.GetKeyCode();
            
            var same = this.KeyInputList.ChangeKey(slot.AxesName, slot.Positive, keyCode);
            
            this.InputSlots
                .FindAll(f => f.AxesName == same?.Name)
                .ForEach(f => f.UpdateSlot());

            slot.UpdateSlot();

            await System.Threading.Tasks.Task.Delay(250);

            this._Input.Interactable = true;
        }

        private void OnDestroy()
        {
            StartButton = null;
        }
    }
}
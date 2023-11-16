using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Loyufei;
using Loyufei.UI;
using Loyufei.InputSystem;

namespace InputDemo
{
    public class InitBoard : MonoBehaviour, IInputRequest
    {
        [SerializeField]
        private List<InputButton> _InputButtons;
        [SerializeField]
        private Button _StartButton;
        
        public InputCenter InputCenter { get; private set; }

        public static Button StartButton { get; private set; }

        public KeyInputList KeyInputList { get; private set; }
        
        public void GetAxes() 
        {
            if (KeyInputList.UIControlUnit("Cancel").GetKeyDown) { Escape(); }
        }

        private void Awake()
        {
            if (_StartButton)
            {
                StartButton = _StartButton;

                StartButton.onClick.AddListener(() => Started());
            }
        }

        private void Start()
        {
            InputCenter = InputSystemProperty.InputCenter;

            KeyInputList = InputCenter.InputCollection.GetSet<KeyInputList>();

            _InputButtons.ForEach(button => SetSlot(button));

            ReStartBoard.ReStartButton.onClick.AddListener(() => ReStarted());

            InputCenter.SetRequest(this);
        }

        private void Started() => InputCenter.ClearRequest(this);

        private void ReStarted() => InputCenter.SetRequest(this);

        private void SetSlot(InputButton inputButton) 
        {
            if (!InputCenter.CheckInputMode(EInputMode.Keyboard))
            {
                inputButton.gameObject.SetActive(false);

                return;
            }


            var button = inputButton.Button;
            var slot = inputButton.Option;

            slot.SetSlot(KeyInputList.OnUse[slot.AxesName]);

            button.ClickEvent += () =>
            {
                InputCenter.ChangeKey(slot.AxesName, slot.Positive, (change, exchange) =>
                {
                    _InputButtons.ForEach(s =>
                    {
                        var option = s.Option;

                        if (option && !exchange.IsDefault()) 
                        {
                            if (option.AxesName == exchange.Name)
                            {
                                option.UpdateSlot();
                            }
                        }
                    });

                    slot.UpdateSlot();
                });
            };
        }

        public void Setup()
        {
            gameObject.SetActive(true);

            var keyMode = InputCenter.CheckInputMode(EInputMode.Keyboard);

            if (keyMode) _InputButtons.First().Select();
        }

        public void UnSet() 
        {
            gameObject.SetActive(false);
        }

        public void Escape() 
        {
            StartButton.onClick.Invoke();
        }

        private void OnDestroy()
        {
            StartButton = null;
        }
    }
}
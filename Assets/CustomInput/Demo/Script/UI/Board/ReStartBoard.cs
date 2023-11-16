using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Loyufei.InputSystem;
using UnityEngine.EventSystems;

namespace InputDemo
{
    public class ReStartBoard : MonoBehaviour, IInputRequest
    {
        [SerializeField]
        private Button _RestartButton;
        
        public static Button ReStartButton { get; private set; }

        private void Awake()
        {
            ReStartButton = _RestartButton;

            ReStartButton.onClick.AddListener(() => { InputSystemProperty.InputCenter.ClearRequest(this); });
        }

        private void Start()
        {
            InputDemo.OnEndChanged += (end) => { InputSystemProperty.InputCenter.SetRequest(this); };

            UnSet();
        }

        public void GetAxes() 
        {
            var input = FindObjectOfType<BaseInputModule>().inputOverride;
            
            if (input && input.GetButtonDown("Cancel")) 
            {
                Escape();
            }
        }

        public void Setup()
        {
            gameObject.SetActive(true);

            if (InputSystemProperty.InputCenter.CheckInputMode(EInputMode.Keyboard)) 
            {
                _RestartButton.Select(); 
            }
        }

        public void UnSet() 
        {
            gameObject.SetActive(false);
        }

        public void Escape() 
        {
            ReStartButton.onClick.Invoke();
        }

        private void OnDestroy()
        {
            ReStartButton = null;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.UI;
using Custom.InputSystem;

namespace InputDemo
{
    public class ReStartBoard : MonoBehaviour
    {
        [SerializeField]
        private SelectableGroup _ReStart;
        
        public static IClick ReStartButton { get; private set; }

        private void Awake()
        {
            this._ReStart.GetSelectables();

            if (this._ReStart["ReStart"] is IClick reStart) 
            {
                ReStartButton = reStart;
            }
        }

        private void Start()
        {
            InputDemo.OnEndChanged += (end) =>
            {
                this.gameObject.SetActive(true);

                InputClient.SetRequest(this._ReStart);
                
                this._ReStart.Select(this._ReStart.First);
            };

            ReStartButton.ClickEvent += (data) =>
            {
                this.gameObject.SetActive(false);
            };

            this.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            ReStartButton = null;
        }
    }
}
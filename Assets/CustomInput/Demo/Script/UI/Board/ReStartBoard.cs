using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.UI;
using Custom.InputSystem;
using Loyufei.InputSystem;

namespace InputDemo
{
    public class ReStartBoard : MonoBehaviour, IUIManageService
    {
        [SerializeField]
        private SelectableGroup _ReStart;
        [SerializeField]
        private InputCenter _InputCenter;
        
        public static IClick ReStartButton { get; private set; }

        public SelectableGroup Current => this._ReStart;

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

                _InputCenter.SetRequest(this);
                
                this._ReStart.Select(this._ReStart.First);
            };

            ReStartButton.ClickEvent += (data) =>
            {
                this.gameObject.SetActive(false);
            };

            this.gameObject.SetActive(false);
        }

        public void Transfer(Vector2 direct) { Debug.Log("One Group Only."); }

        private void OnDestroy()
        {
            ReStartButton = null;
        }
    }
}
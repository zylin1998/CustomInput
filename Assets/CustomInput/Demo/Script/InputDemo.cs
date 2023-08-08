using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.InputSystem;

namespace InputDemo
{
    public class InputDemo : MonoBehaviour, IInputRequest
    {
        [Header("Demo Platform")]
        [SerializeField]
        private bool _PlatformTest;
        [SerializeField]
        private RuntimePlatform _Platform = RuntimePlatform.WindowsEditor;
        [Header("Character Control")]
        [SerializeField]
        private InputSetting _InputSetting;
        [SerializeField]
        private Character _Character;
        [Header("Item  Creation")]
        [SerializeField]
        private ItemCreator _ItemCreator;
        [SerializeField]
        private float _CreateTime = 5f;
        [Header("Game State")]
        [SerializeField]
        private bool _End;
        [SerializeField]
        private int _MaxScore = 5;
        [SerializeField]
        private int _CurrentScore = 0;

        private static int _Score = 0;

        public static Action<int> OnScoreChanged { get; set; }
        public static Action<bool> OnEndChanged { get; set; }

        public bool End 
        {
            get => this._End;

            set 
            {
                this._End = value;

                if (this._End) { OnEndChanged?.Invoke(this._End); }
            }
        }

        public static int Score 
        { 
            get => _Score;
            
            set
            {
                _Score = value;
                
                OnScoreChanged?.Invoke(_Score);
            }
        }

        public static int MaxScore { get; private set; }

        #region Script Behaviour

        private void Awake()
        {
            InputClient.SetInput(_InputSetting);

            if (this._PlatformTest)
            {
                InputClient.SetPlatform(this._Platform); 
            }

            OnScoreChanged += (s) =>
            { 
                this._CurrentScore = s;

                this.End = this._CurrentScore == this._MaxScore;
            };

            MaxScore = this._MaxScore;
            Score = this._CurrentScore;
        }

        private void Start()
        {
            InputClient.SetRequest(this, true);

            InitBoard.StartButton.ClickEvent += (data) =>
            {
                this._CurrentScore = 0;
                this._BallCount = 0;
                this._PassTime = this._CreateTime;

                Score = this._CurrentScore;

                StartCoroutine(this.CreateItem());
            };
        }

        #endregion

        public void GetAxes() 
        {
            var x = InputClient.GetAxis("Horizontal");
            var y = InputClient.GetAxis("Vertical");

            this._Character.Move(new Vector2(x, y));

            if (InputClient.GetKeyDown("Interact") && this._Character.Interacts.Any()) 
            {
                this._Character.Interact(this._Character.Interacts.First());
            }
        }

        private int _BallCount;
        private float _PassTime;

        private IEnumerator CreateItem() 
        {
            while(true) 
            {
                if (this._End) { yield break; }

                if (this._BallCount == this._MaxScore) { yield break; }

                this._PassTime += Time.deltaTime;

                if (this._PassTime >= this._CreateTime) 
                {
                    this._ItemCreator.RandomCreate();
                    
                    this._PassTime = 0f;

                    this._BallCount++;
                }

                yield return null;
            }
        }

        private void OnDestroy()
        {
            OnScoreChanged = (score) => { };
            OnEndChanged = (end) => { };

            Score = 0;
            MaxScore = 0;
        }
    }
}
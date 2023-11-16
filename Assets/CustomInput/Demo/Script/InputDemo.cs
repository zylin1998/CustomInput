using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Loyufei.InputSystem;

namespace InputDemo
{
    public class InputDemo : MonoBehaviour, IInputRequest
    {
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
            get => _End;

            set
            {
                _End = value;

                if (End) { OnEndChanged?.Invoke(End); }
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
            OnScoreChanged += (s) =>
            { 
                _CurrentScore = s;

                End = _CurrentScore == _MaxScore;
            };

            MaxScore = _MaxScore;
            Score = _CurrentScore;
        }

        private void Start()
        {
            InputSystemProperty.InputCenter.SetRequest(this, true);

            InitBoard.StartButton.onClick.AddListener(() =>
            {
                _CurrentScore = 0;
                _BallCount = 0;
                _PassTime = _CreateTime;

                Score = _CurrentScore;

                StartCoroutine(CreateItem());

                _Character.Controller.enabled = true;
            });

            ReStartBoard.ReStartButton.onClick.AddListener(() =>
            {
                _Character.Controller.enabled = false;

                _Character.MoveTo(Vector3.zero, Quaternion.identity);
            });
        }

        private void OnDestroy()
        {
            OnScoreChanged = (score) => { };
            OnEndChanged = (end) => { };

            Score = 0;
            MaxScore = 0;
        }

        #endregion

        #region Character Control

        [Header("Character Control")]
        [SerializeField]
        private Character _Character;

        public void GetAxes() 
        {
            var x = InputManager.GetAxis("Horizontal");
            var y = InputManager.GetAxis("Vertical");
            
            _Character.Move(new Vector2(x, y));

            if (InputManager.GetKeyDown("Interact") && _Character.Interacts.Any()) 
            {
                _Character.Interact(_Character.Interacts.First());
            }
        }

        public void Setup() { }

        public void UnSet() { }

        #endregion

        #region Create Items

        [Header("Item  Creation")]
        [SerializeField]
        private ItemCreator _ItemCreator;
        [SerializeField]
        private float _CreateTime = 5f;
        
        private int _BallCount;
        private float _PassTime;

        private IEnumerator CreateItem() 
        {
            for(; !_End && _BallCount < _MaxScore; _PassTime += Time.deltaTime) 
            {
                if (_PassTime >= _CreateTime)
                {
                    _ItemCreator.RandomCreate();

                    _PassTime = 0f;

                    _BallCount++;

                }

                yield return null;
            }
        }

        #endregion
    }
}
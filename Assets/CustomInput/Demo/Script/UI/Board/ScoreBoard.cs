using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace InputDemo
{
    public class ScoreBoard : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _ScoreText;

        private void Start()
        {
            InputDemo.OnScoreChanged += (score) => this.SetScore(score);

            this.SetScore(InputDemo.Score);
        }

        public void SetScore(int current) 
        {
            this._ScoreText?.SetText(string.Format("{0, 2}/{1, 2}", current, InputDemo.MaxScore));
        }
    }
}

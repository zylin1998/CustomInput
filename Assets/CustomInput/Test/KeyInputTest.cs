using System.Linq;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Loyufei.InputSystem;

namespace Custom.InputSystem.Test
{
    public class KeyInputTest
    {
        private class TestKeyInput : IInputRequest
        {
            public void GetAxes()
            {
                Assert.AreEqual(KeyCode.Space, InputManager.GetAxes<KeyInput>("Confirm").First().Positive);
                Assert.AreEqual(KeyCode.Escape, InputManager.GetAxes<KeyInput>("Cancel").First().Positive);

                Assert.AreEqual(0f, InputManager.GetAxis("Horizontal"));
                Assert.AreEqual(0f, InputManager.GetAxis("Vertical"));
            }
        }

        [UnityTest]
        public IEnumerator KeyInputTestPasses()
        {
            var input = new TestKeyInput();
            var inputCenter = new GameObject("InputCenter").AddComponent<InputCenter>();
            
            inputCenter.SetRequest(input);

            for (int frameCount = 0; frameCount <= 100; frameCount++)
            {
                yield return null;
            }
        }

    }
}
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Custom.InputSystem.Test
{
    public class KeyInputTest
    {
        private class TestKeyInput : IInputRequest
        {
            public void GetAxes()
            {
                Assert.AreEqual(KeyCode.Space, InputClient.GetAxes<KeyInput>("Confirm").First().Positive);
                Assert.AreEqual(KeyCode.Escape, InputClient.GetAxes<KeyInput>("Cancel").First().Positive);

                Assert.AreEqual(0f, InputClient.GetAxis("Horizontal"));
                Assert.AreEqual(0f, InputClient.GetAxis("Vertical"));
            }
        }

        [UnityTest]
        public IEnumerator KeyInputTestPasses()
        {
            var input = new TestKeyInput();

            InputClient.SetRequest(input);

            for (int frameCount = 0; frameCount <= 100; frameCount++)
            {
                yield return null;
            }
        }

    }
}
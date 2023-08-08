using System.Linq;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.EventSystems;

namespace Custom.InputSystem.Test
{
    class TouchInputTest
    {
        [UnityTest]
        public IEnumerator TouchInputTestPasses()
        {
            var input = new TestTouchInput();
            var joyStick = new VirtualJoyStick();
            var confirm = new TouchButton("Confirm");
            var cancel = new TouchButton("Cancel");

            InputClient.SetRequest(input);

            InputClient.SetTouchInput(joyStick);
            InputClient.SetTouchInput(confirm);
            InputClient.SetTouchInput(cancel);

            for (int frameCount = 0; frameCount <= 240; frameCount++)
            {
                yield return null;
            }
        }

        private class TestTouchInput : IInputRequest
        {
            public void GetAxes()
            {
                Assert.IsNotNull(InputClient.GetAxes<ITouchUnit>("Confirm").First().TouchInput);
                Assert.IsNotNull(InputClient.GetAxes<ITouchUnit>("Cancel").First().TouchInput);

                Assert.AreEqual(0f, InputClient.GetAxis("Horizontal"));
                Assert.AreEqual(0f, InputClient.GetAxis("Vertical"));
            }
        }

        private class VirtualJoyStick : IVJoyStick
        {
            public string Horizontal => "Horizontal";
            public string Vertical => "Vertical";

            public float Angle { get; private set; }

            public bool IsOnDrag { get; private set; }

            public void OnDrag(PointerEventData eventData)
            {
                this.IsOnDrag = true;
            }

            public void OnEndDrag(PointerEventData eventData)
            {
                this.IsOnDrag = false;
            }

            public void Drag(float angle) 
            {
                this.OnDrag(null);

                this.Angle = angle;
            }
        }

        private class TouchButton : ITouchButton
        {
            public string AxesName { get; private set; }

            public bool GetKeyDown { get; private set; }
            public bool GetKey { get; private set; } 
            public bool GetKeyUp { get; private set; }

            public TouchButton(string axesName) 
            {
                this.AxesName = axesName;

                this.GetKeyDown = false;
                this.GetKey = false;
                this.GetKeyUp = false;
            }

            public IEnumerator PointerDown() 
            {
                this.GetKeyDown = true;

                yield return null;

                this.GetKeyDown = false;
                this.GetKey = true;

                for(; !this.GetKeyUp;) 
                {
                    yield return null;
                }
            }

            public void PointerUp() 
            {
                this.GetKey = false;

                this.GetKeyUp = true;
            }
        }
    }
}

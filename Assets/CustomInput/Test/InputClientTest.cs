using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using Custom.InputSystem;

namespace Custom.InputSystem.Test
{
    public class TestInputClient
    {
        [Test]
        public void InputClientTestPasses()
        {
            var input = AssetDatabase.LoadAssetAtPath<InputSetting>("Assets/CustomInput/Test/TestInput/Input List.asset");

            InputClient.SetInput(input);

            Assert.IsNotNull(InputClient.Client);
            Assert.IsNotNull(InputClient.InputSetting);
            Assert.IsNotNull(InputClient.InputSets);
            Assert.AreEqual(false, InputClient.Client.Pause);
            Assert.AreEqual(RuntimePlatform.OSXEditor, InputClient.Platform);
        }
    }
}
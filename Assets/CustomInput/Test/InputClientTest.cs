using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using Loyufei.InputSystem;

namespace Custom.InputSystem.Test
{
    public class TestInputClient
    {
        [Test]
        public void InputClientTestPasses()
        {
            var input = AssetDatabase.LoadAssetAtPath<InputCollection>("Assets/CustomInput/Test/TestInput/Input List.asset");
            var inputCenter = new GameObject("InputCenter").AddComponent<InputCenter>();
            
            inputCenter.SetInput(input);

            Assert.IsNotNull(inputCenter);
            Assert.IsNotNull(inputCenter.InputCollection);
        }
    }
}
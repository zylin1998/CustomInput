using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Custom.InputSystem
{
    public class InputManager
    {
        public static Dictionary<string, List<IInputUnit>> AxesDictionary { get; private set; } 
            = new Dictionary<string, List<IInputUnit>>();

        public static void SetInputSetting(IInputSetting inputSetting) 
        {
            AxesDictionary = inputSetting.Dictionary;
        }

        #region GetValue

        public static List<IInputUnit> GetAxes(string name)
        {
            return AxesDictionary.TryGetValue(name, out var list) ? list : new List<IInputUnit>();
        }

        public static List<TAxes> GetAxes<TAxes>(string name) where TAxes : IInputUnit
        {
            return GetAxes(name).FindAll(f => f is TAxes).ConvertAll(c => (TAxes)c);
        }

        public static List<InputValue> GetAxesValue(string name)
        {
            return GetAxes(name).ConvertAll(c => c.Value);
        }

        public static float GetAxis(string name)
        {
            var hasAxis = GetAxesValue(name).FindAll(f => f.Axis != 0);

            return hasAxis.Count > 0 ? hasAxis[0].Axis : 0f;
        }

        public static bool GetKeyDown(string name) => GetAxesValue(name).Any(axes => axes.GetKeyDown);
        public static bool GetKey(string name) => GetAxesValue(name).Any(axes => axes.GetKey);
        public static bool GetKeyUp(string name) => GetAxesValue(name).Any(axes => axes.GetKeyUp);

        #endregion
    }
}

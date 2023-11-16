using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;

namespace Loyufei.UI
{
    [CustomEditor(typeof(VirtualJoyStick), true)]
    [CanEditMultipleObjects]
    public class VirtualJoyStickEditor : ScrollRectEditor
    {
        SerializedProperty _Horizontal;
        SerializedProperty _Vertical;

        protected override void OnEnable() 
        {
            base.OnEnable();

            _Horizontal = serializedObject.FindProperty("_Horizontal");
            _Vertical   = serializedObject.FindProperty("_Vertical");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.PropertyField(_Horizontal);
            EditorGUILayout.PropertyField(_Vertical);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
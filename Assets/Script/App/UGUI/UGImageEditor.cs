using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace App.UGUI{
    [CustomEditor(typeof(App.UGUI.UGImage))]
    public class UGImageEditor: Editor {
        //private SerializedProperty tofullScreenProp = null;
        void OnEnable () {
            //tofullScreenProp = serializedObject.FindProperty ("tofullScreen");
        }
        /*public override void OnInspectorGUI ()
        {
            base.OnInspectorGUI();
            serializedObject.Update ();
            EditorGUILayout.BeginHorizontal();
            //tofullScreenProp.boolValue = EditorGUILayout.Toggle("tofullScreen", tofullScreenProp.boolValue, GUILayout.MinWidth(10f));
            EditorGUILayout.EndHorizontal();
            serializedObject.ApplyModifiedProperties ();
        }*/
	}
}
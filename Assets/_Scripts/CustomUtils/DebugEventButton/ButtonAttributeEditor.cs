﻿using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace DarkHavoc.CustomUtils.DebugEventButton
{
    /*[CustomPropertyDrawer(typeof(ButtonAttribute))]
    public class ButtonAttributeEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ButtonAttribute buttonAttribute = (ButtonAttribute)attribute;
            string methodName = buttonAttribute.methodName;

            if (GUI.Button(position, methodName))
            {
                System.Type eventOwnerType = property.serializedObject.targetObject.GetType();
                System.Reflection.MethodInfo eventMethodInfo = eventOwnerType.GetMethod(methodName);
                eventMethodInfo.Invoke(property.serializedObject.targetObject, null);
            }
        }
    }*/
}
#endif
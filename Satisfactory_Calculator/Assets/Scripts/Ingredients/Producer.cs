using System;
using UnityEditor;
using UnityEngine;

[Serializable]
public class Producer
{
    public Sprite icon;
    public float production;
}

[CustomPropertyDrawer(typeof(Producer))]
public class ProducerDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        
        var iconRect = new Rect(position.x, position.y, 150, position.height);
        var productionRect = new Rect(position.x + 155, position.y, 100, position.height);
        
        EditorGUI.PropertyField(iconRect, property.FindPropertyRelative("icon"), GUIContent.none);
        EditorGUI.PropertyField(productionRect, property.FindPropertyRelative("production"), GUIContent.none);
        
        EditorGUI.EndProperty();
    }
}
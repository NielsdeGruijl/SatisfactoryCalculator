using System;
using UnityEditor;
using UnityEngine;

[Serializable]
public class PartIngredient
{
    public Part part;
    public float amount;

    public ProductionNode productionNode;
}

[CustomPropertyDrawer(typeof(PartIngredient))]
public class PartDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        
        /*
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;
        */

        var amountRect = new Rect(position.x, position.y, 150, position.height);
        var unitRect = new Rect(position.x + 155, position.y, 100, position.height);
        
        EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("part"), GUIContent.none);
        EditorGUI.PropertyField(unitRect, property.FindPropertyRelative("amount"), GUIContent.none);
        
        
        //EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}
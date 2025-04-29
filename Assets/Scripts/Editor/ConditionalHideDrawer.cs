using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ConditionalHideAttribute))]
public class ConditionalHideDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ConditionalHideAttribute condHAtt = (ConditionalHideAttribute)attribute;
        bool enabled = ShouldShowField(property, condHAtt);
        
        bool wasEnabled = GUI.enabled;
        GUI.enabled = enabled;

        if (enabled) EditorGUI.PropertyField(position, property, label, true);

        GUI.enabled = wasEnabled;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ConditionalHideAttribute condHAtt = (ConditionalHideAttribute)attribute;
        bool enabled = ShouldShowField(property, condHAtt);

        if (enabled) return EditorGUI.GetPropertyHeight(property, label);
        else return -EditorGUIUtility.standardVerticalSpacing;
    }

    private bool ShouldShowField(SerializedProperty property, ConditionalHideAttribute condHAtt)
    {
        bool enabled = true;

        // Find the referenced enum property
        string path = property.propertyPath.Replace(property.name, condHAtt.EnumFieldName);
        SerializedProperty enumProperty = property.serializedObject.FindProperty(path);

        if (enumProperty != null) enabled = enumProperty.enumValueIndex == condHAtt.EnumValue;
        else Debug.LogWarning($"Could not find a property named '{condHAtt.EnumFieldName}' in '{property.serializedObject.targetObject.GetType()}'. Make sure the property name is correct.");

        return enabled;
    }
}

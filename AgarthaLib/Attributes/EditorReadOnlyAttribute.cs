using UnityEditor;
using UnityEngine;

namespace AgarthaLib.Attributes
{
    public class EditorReadOnlyAttribute : PropertyAttribute {}

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(EditorReadOnlyAttribute))]
    public class ReadOnlyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            => EditorGUI.GetPropertyHeight(property, label, true);

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }
    }
#endif
}
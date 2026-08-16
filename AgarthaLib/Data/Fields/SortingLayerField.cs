using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace AgarthaLib.Data.Fields
{
    [Serializable] public class SortingLayerField
    {
        [SerializeField] private int m_LayerID;

        public int id
        {
            get => m_LayerID;
            set => m_LayerID = value;
        }

        public string name
        {
            get => SortingLayer.IDToName(m_LayerID);
            set => m_LayerID = SortingLayer.NameToID(value);
        }

        public int value
        {
            get => SortingLayer.GetLayerValueFromID(m_LayerID);
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(SortingLayerField))]
    public class SortingLayerFieldDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty idProp = property.FindPropertyRelative("m_LayerID");

            // Fetch all available sorting layers from the project
            SortingLayer[] layers = SortingLayer.layers;
            string[] layerNames = new string[layers.Length];
            int[] layerIDs = new int[layers.Length];

            int selectedIndex = 0;

            for (int i = 0; i < layers.Length; i++)
            {
                layerNames[i] = layers[i].name;
                layerIDs[i] = layers[i].id;

                if (layerIDs[i] == idProp.intValue)
                {
                    selectedIndex = i;
                }
            }

            // Draw the dropdown field
            EditorGUI.BeginChangeCheck();
            selectedIndex = EditorGUI.Popup(position, label.text, selectedIndex, layerNames);

            if (EditorGUI.EndChangeCheck())
            {
                idProp.intValue = layerIDs[selectedIndex];
            }
        }
    }
#endif
}
using System;
using System.Collections.Generic;
using System.Linq;
using Package.Runtime.Scripts.Attributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

namespace Package.Editor.Scripts
{
    [CustomPropertyDrawer(typeof(MixerParameter))]
    public class MixerParameterPropertyDrawer : PropertyDrawer
    {
        private const string AUDIO_MIXER_FILTER = "t:AudioMixer";
        private const string AUDIO_EXPOSED_PARAMETERS_KEY = "exposedParameters";
        private const string FIELD_IS_NOT_STRING = "Field is not a string!";
        private const string NO_MIXERS = "Can't find any audio mixer in project";
        private const string ERROR_TAG = "ERROR: ";
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.String)
            {
                DrawErrorLabel(FIELD_IS_NOT_STRING, position);
                return;
            }
            
            var menu = new GenericMenu();
            var filteredPaths = AssetDatabase.FindAssets(AUDIO_MIXER_FILTER);
            var fields = new List<object>();

            foreach (var filteredPath in filteredPaths)
            {
                var path = AssetDatabase.GUIDToAssetPath(filteredPath);
                var mixer = AssetDatabase.LoadAssetAtPath<AudioMixer>(path);

                var parameters = (Array)mixer
                    .GetType()
                    .GetProperty(AUDIO_EXPOSED_PARAMETERS_KEY)?
                    .GetValue(mixer, null);
                
                if (parameters != null)
                {
                    fields.AddRange(parameters.Cast<object>());
                }
            }
            
            if (fields.Count == 0)
            {
                DrawErrorLabel(NO_MIXERS, position);
                return;
            }

            var labelRect = position;
            labelRect.width /= 1.5f;
            EditorGUI.LabelField(labelRect, property.displayName);
            labelRect.position = new Vector2(labelRect.position.x + labelRect.width, labelRect.position.y);
            labelRect.width = position.width;

            if (GUI.Button(labelRect, property.stringValue, EditorStyles.toolbarPopup))
            {
                foreach (var stringValue in fields.Select(enumChild 
                    => (string)enumChild
                    .GetType()
                    .GetField("name")
                    .GetValue(enumChild)))
                {
                    menu.AddItem(
                        new GUIContent(stringValue),
                        stringValue == property.stringValue,
                        HandleSelect,
                        new MixerParameterDrawerValuePair(stringValue, property));
                }
                
                menu.ShowAsContext();
            }
        }

        private void DrawErrorLabel(string log, Rect position)
        {
            EditorGUI.LabelField(position, ERROR_TAG, log);
        }
        
        private static void HandleSelect(object item)
        {
            var clickedItem = (MixerParameterDrawerValuePair)item;
            var serializedProperty = clickedItem.Property;
            serializedProperty.stringValue = clickedItem.NameValue;
            serializedProperty.serializedObject.ApplyModifiedProperties();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) { return 18; }
        
        private readonly struct MixerParameterDrawerValuePair
        {
            public readonly string NameValue;
            public readonly SerializedProperty Property;

            public MixerParameterDrawerValuePair(string nameValue, SerializedProperty property)
            {
                NameValue = nameValue;
                Property = property;
            }
        }
    }
}
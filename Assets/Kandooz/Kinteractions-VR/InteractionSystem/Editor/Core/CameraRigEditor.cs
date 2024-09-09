using System;
using UnityEditor;

namespace Kandooz.InteractionSystem.Core.Editors
{
    [CustomEditor(typeof(CameraRig))]
    public class CameraRigEditor : Editor
    {
        private CameraRig rig;

        private void OnEnable()
        {
            rig = (CameraRig)target;
            var hands = rig.GetComponentsInChildren<Hand>();
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            if (rig.Config == null)
            {
                EditorGUILayout.LabelField("Select a config file to continue");
                return;
            }

            EditorGUILayout.PropertyField(serializedObject.FindProperty("initializeHands"));

            if (serializedObject.FindProperty("initializeHands").boolValue)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("handTrackingMethod"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("leftHandPivot"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("rightHandPivot"));
            }

            EditorGUILayout.PropertyField(serializedObject.FindProperty("initializeLayers"));
            serializedObject.ApplyModifiedProperties();
        }
    }
}
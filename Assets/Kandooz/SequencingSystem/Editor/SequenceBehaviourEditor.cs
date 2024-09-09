using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Kandooz.Kuest.Editors
{
    [CustomEditor(typeof(SequenceBehaviour))]
    public class SequenceBehaviourEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            var sequence = (SequenceBehaviour)target;
            if (sequence.StarOnAwake)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("delay"));
            }

            if (sequence.stepListeners is null)
            {
                var listeners = sequence.GetComponentsInChildren<StepEventListener>();
                sequence.stepListeners=listeners.ToList();
            }
            else
            {
                if (GUILayout.Button("Update Sequence"))
                {
                }
            }

        }

        private static void Initialize(SequenceBehaviour sequence)
        {
            sequence.steps = new List<SequenceBehaviour.StepEventPair>();
            foreach (var step in sequence.sequence.Steps) sequence.steps.Add(new SequenceBehaviour.StepEventPair(step));
        }
    }
}
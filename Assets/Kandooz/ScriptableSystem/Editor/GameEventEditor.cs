using UnityEditor;
using UnityEngine;

namespace Kandooz.ScriptableSystem.Editors
{
    [CustomEditor(typeof(GameEvent),true)]
    public class GameEventEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (Application.isPlaying && GUILayout.Button("Raise"))
            {
                var @event = (GameEvent)target;
                @event.Raise();
            }
        }
    }
}
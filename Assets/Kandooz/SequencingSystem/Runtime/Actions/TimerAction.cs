using Kandooz.InteractionSystem.Core;
using UnityEngine;
using UnityEngine.Events;

namespace Kandooz.Kuest
{
    [AddComponentMenu("Kandooz/SequenceSystem/Actions/TimerAction")]
    public class TimerAction : MonoBehaviour
    {
        [SerializeField] private UnityEvent onComplete;
        [SerializeField] private bool active;
        [SerializeField] private bool startOnEnable;
        [SerializeField] private float time;
        [SerializeField][ReadOnly] private float elapsed = 0;

        private void OnEnable()
        {
            if (startOnEnable) StartTimer();
        }

        public void StartTimer()
        {
            elapsed = 0;
            active = true;
        }

        private void Update()
        {
            if (!active) return;
            elapsed += Time.deltaTime;
            if (!(elapsed >= time)) return;
            onComplete.Invoke();
        }
    }
}
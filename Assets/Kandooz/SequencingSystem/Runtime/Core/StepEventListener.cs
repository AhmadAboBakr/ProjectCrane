using System;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace Kandooz.Kuest
{
    [AddComponentMenu("Kandooz/SequenceSystem/StepListener")]
    public class StepEventListener : MonoBehaviour
    {
        [SerializeField] internal Step step;
        [SerializeField] private UnityEvent onStarted;
        [SerializeField] private UnityEvent onEnded;
        private bool current;
        public bool Current => current;

        public void OnActionCompleted()
        {
            step.OnActionCompleted();
        }

        public IObservable<Unit> OnStarted => onStarted.AsObservable();
        public IObservable<Unit> OnFinished => onEnded.AsObservable();
        private IDisposable disposable;

        private void OnEnable()
        {
            disposable = step.OnRaisedData.Do(OnStatusChanged).Subscribe();
        }

        private void OnDisable()
        {
            disposable.Dispose();
        }

        public void OnStatusChanged(SequenceStatus elementStatus)
        {
            switch (elementStatus)
            {
                case SequenceStatus.Started:
                    current = true;
                    onStarted?.Invoke();
                    break;
                case SequenceStatus.Completed:
                    current = false;
                    onEnded?.Invoke();
                    break;
            }
        }
    }
}
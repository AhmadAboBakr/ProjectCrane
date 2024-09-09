using System;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace Kandooz.ScriptableSystem
{
    public class GameEventListener : MonoBehaviour,IObservable<float>
    {
        [SerializeField] private GameEvent @event;
        [SerializeField] private UnityEvent onRaised;
        private CompositeDisposable _disposable;

        private void OnEnable()
        {
            _disposable = new CompositeDisposable();
            @event.OnRaised
                .Do(_ => onRaised.Invoke())
                .Subscribe()
                .AddTo(_disposable);
        }

        private void OnDisable()
        {
            _disposable.Dispose();
        }

        public IDisposable Subscribe(IObserver<float> observer)
        {
            throw new NotImplementedException();
        }
    }
}
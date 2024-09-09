using System;
using System.Runtime.CompilerServices;
using UniRx;
using UnityEngine;

namespace Kandooz.ScriptableSystem
{
    public class ScriptableVariable<T> : ScriptableVariable, IObservable<T>
    {
        [SerializeField] private T value;
        private readonly Subject<T> _onRaised = new();
        private IObservable<T> OnRaisedData => _onRaised;
        
        public T Value
        {
            get => value;
            set
            {
                this.value = value;
                Raise();
            }
        }
        
        public override void Raise()
        {
            base.Raise();
            _onRaised.OnNext(value);
        }

        public void Raise(T data)
        {
            try
            {
                Raise();
                _onRaised.OnNext(data);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            return OnRaisedData.Subscribe(observer);
        }

        public override string ToString()
        {
            return value.ToString();
        }

    }

    public abstract class ScriptableVariable : GameEvent
    {
        public IObservable<Unit> OnValueChanged => OnRaised;

        public abstract override string ToString();
        
    }
}
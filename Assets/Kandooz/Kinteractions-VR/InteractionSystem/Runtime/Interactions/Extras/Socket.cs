using System;
using Kandooz.InteractionSystem.Core;
using Kandooz.InteractionSystem.Interactions;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace Kandooz.Kinteractions.InteractionSystem.Interactions
{
    public class Socket : MonoBehaviour
    {
        [SerializeField] private Transform pivot;
        [SerializeField] private UnityEvent onSocketConnected;
        [SerializeField] private UnityEvent onSocketDisconnected;

        [ReadOnly] [SerializeField] protected Socketable socketable;
        public IObservable<Unit> OnSocketConnected => onSocketConnected.AsObservable();
        public IObservable<Unit> OnSocketDisconnected => onSocketDisconnected.AsObservable();
        public Transform Pivot => pivot ? pivot : transform;

        public void InsertSocketable(Socketable socketable)
        {
            onSocketConnected.Invoke();
            this.socketable = socketable;
        }

        public void RemoveSocketable()
        {
            onSocketDisconnected.Invoke();
            socketable = null;
        }
    }
}
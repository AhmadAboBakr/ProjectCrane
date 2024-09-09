using System;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace Kandooz.InteractionSystem.Interactions
{
    [Serializable]
    public class FloatPairUnityEvent : UnityEvent<Vector2>
    {
    }

    [Serializable]
    public class LeverInteractable2D : AbstractLeverInteractable
    {
        [SerializeField] private Vector2 min, max;
        [HideInInspector] [SerializeField] private FloatPairUnityEvent onLeverChanged;
        public IObservable<Vector2> OnLeverChanged => onLeverChanged.AsObservable();
        private Vector2 _oldRotation, _currentRotation;
        [SerializeField] private Vector2 normalizedRotation;


        protected override Quaternion Rotate(Vector3 direction)
        {
            direction.Normalize();
            _currentRotation.x = CalculateAngle(direction, transform.right, transform.forward);
            _currentRotation.y = CalculateAngle(direction, transform.up, transform.forward);
            if (limited)
            {
                _currentRotation.x = LimitAngle(_currentRotation.x, min.x, max.x);
                _currentRotation.y = LimitAngle(_currentRotation.y, min.y, max.y);
            }
            return Quaternion.Euler(_currentRotation.x, _currentRotation.y, 0);
        }

        protected override Vector3 defaultDirection => transform.forward;

        protected override void InvokeEvents()
        {
            normalizedRotation = limited ? Normalize(_currentRotation) : _currentRotation;
            var difference = Vector2.SqrMagnitude(_oldRotation - normalizedRotation);
            if (difference < Threshold) return;
            _oldRotation = normalizedRotation;
            onLeverChanged?.Invoke(normalizedRotation);
        }

        private Vector2 Normalize(Vector2 currentRotation)
        {
            _currentRotation.x = 2 * (_currentRotation.x - min.x) / (max.x - min.x) - 1;
            _currentRotation.y = 2 * (_currentRotation.y - min.y) / (max.y - min.y) - 1;
            return _currentRotation;
        }

        private const float Threshold = .1f;
    }
}
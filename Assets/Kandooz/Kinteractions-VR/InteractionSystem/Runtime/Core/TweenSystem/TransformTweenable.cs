using System;
using UnityEngine;

namespace Kandooz.InteractionSystem.Core
{
    /// <summary>
    /// Tweens the rotation and position of an object between two pivot points
    /// </summary>
    public class TransformTweenable :ITweenable
    {
        private Vector3 _startPosition;
        private Quaternion _startRotation;
        private Transform _target;
        private Transform _transform;
        private float _time = 0;
        public event Action OnTweenComplete;
        public bool Tween(float scaledDeltaTime)
        {
            _time += scaledDeltaTime;
            _transform.position = Vector3.Lerp(_startPosition, _target.position, _time);
            _transform.rotation = Quaternion.Lerp(_startRotation, _target.rotation, _time);
            if (_time < 1) return false;
            try
            {
                OnTweenComplete?.Invoke();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
            return true;
        }
        public void Initialize(Transform transform, Transform target)
        {
            this._transform = transform;
            _startPosition = transform.position;
            _startRotation = this._transform.rotation;
            this._target = target;
            _time = 0;
        }
    }
}
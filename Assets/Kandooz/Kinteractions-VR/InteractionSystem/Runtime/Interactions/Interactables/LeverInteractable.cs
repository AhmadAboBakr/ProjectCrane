using System;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace Kandooz.InteractionSystem.Interactions
{
    public class LeverInteractable : ConstrainedInteractableBase
    {
        public IObservable<XZPair> OnLeverChanged => onLeverChanged.AsObservable();
        [SerializeField] private bool limited = true;
        [SerializeField] private bool snapToCenter;
        [SerializeField] private XZPair limits;
        [SerializeField] private AngleChangeUnityEvent onLeverChanged;

        private XZPair _currentNormalizedAngle = new(0, 0);
        private XZPair _oldNormalizedAngle = new(0, 0);


        private void Start()
        {
            OnDeselected
                .Do(_ => Rotate(0,0))
                .Do(_ => InvokeEvents())
                .Subscribe().AddTo(this);
        }

        protected override void Activate()
        {
        }

        protected override void StartHover()
        {
        }

        protected override void EndHover()
        {
        }

        private void Update()
        {
            if (!IsSelected) return;
            Rotate(CalculateAngle(transform.right), CalculateAngle(transform.forward));
            InvokeEvents();
        }
        private void Rotate(float x, float z)
        {
            var angleX = LimitAngle(x, limits.x);
            var angleZ = LimitAngle(z, limits.z);
            interactableObject.transform.localRotation = Quaternion.Euler(angleX, 0, angleZ);
            _currentNormalizedAngle.x = angleX / (limits.x / 2);
            _currentNormalizedAngle.z = angleZ / (limits.z / 2);
        }
        private void InvokeEvents()
        {
            var differenceX = _currentNormalizedAngle.x - _oldNormalizedAngle.x;
            var differenceZ = _currentNormalizedAngle.z - _oldNormalizedAngle.z;
            var absDifference = Mathf.Max(Mathf.Abs(differenceX), Mathf.Abs(differenceZ));
            if (!(Math.Abs(absDifference) > .1f)) return;
            onLeverChanged.Invoke(_currentNormalizedAngle);
            _oldNormalizedAngle = _currentNormalizedAngle;
        }

        private float CalculateAngle(Vector3 plane)
        {
            //-transform.right
            var direction = CurrentInteractor.transform.position - transform.position;
            direction = Vector3.ProjectOnPlane(direction, -plane).normalized;
            var angle = -Vector3.SignedAngle(direction, transform.up, plane);
            return angle;
        }

        private float LimitAngle(float angle, float limit)
        {
            if (!limited) return angle;
            if (angle > limit / 2)
            {
                angle = limit / 2;
            }

            if (angle < -limit / 2)
            {
                angle = -limit / 2;
            }

            return angle;
        }

        #region private classes

        [System.Serializable]
        public struct XZPair
        {
            public float x, z;

            public XZPair(float x, float z)
            {
                this.x = x;
                this.z = z;
            }
        }

        [System.Serializable]
        private class AngleChangeUnityEvent : UnityEvent<XZPair>
        {
        }

        #endregion
    }
}
using System;
using Kandooz.InteractionSystem.Core;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace Kandooz.InteractionSystem.Interactions
{
    [Serializable]
    public class FloatUnityEvent : UnityEvent<float>
    {
    }

    public class LeverInteractable1D : AbstractLeverInteractable
    {
        [SerializeField] private Axe axe = Axe.Z;
        [SerializeField] private float min, max;
        [HideInInspector] [SerializeField] private FloatUnityEvent onLeverChanged;
        [ReadOnly] [SerializeField] private float currentNormalizedAngle = 0;

        private float _oldNormalizedAngle = 0;
        public IObservable<float> OnLeverChanged => onLeverChanged.AsObservable();

        protected override Vector3 defaultDirection => transform.up;

        private void Update()
        {
            if (!Selected) return;
            var direction = CurrentInteractor.transform.position - transform.position;
            interactableObject.transform.localRotation = Rotate(direction);
            InvokeEvents();
        }

        protected override Quaternion Rotate(Vector3 direction)
        {
            var (normal, zero) = axe switch
            {
                Axe.X => (transform.right, transform.up),
                Axe.Z => (transform.forward, transform.up),
                _ => (Vector3.zero, Vector3.zero)
            };

            var angle = CalculateAngle(direction, normal, zero);
            if (limited) angle = LimitAngle(angle, min, max);
            return CalculateQuaternion();

            Quaternion CalculateQuaternion()
            {
                return axe == Axe.X ? Quaternion.Euler(angle, 0, 0) : Quaternion.Euler(0, 0, angle);
            }
        }


        protected override void InvokeEvents()
        {
            var difference = currentNormalizedAngle - _oldNormalizedAngle;
            var absDifference = Mathf.Abs(difference);
            if (absDifference < .1f) return;
            _oldNormalizedAngle = currentNormalizedAngle;
            onLeverChanged.Invoke(currentNormalizedAngle);
        }
    }

    public enum Axe
    {
        X,
        Z
    }
}
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;

namespace Kandooz.InteractionSystem.Interactions
{
    public abstract class AbstractLeverInteractable :ConstrainedInteractableBase
    {
        [SerializeField] protected bool snapToCenter;
        [SerializeField] protected bool limited;
        protected abstract Vector3 defaultDirection { get; }
       private void Update()
        {
            Vector3 direction =defaultDirection;
            if (!Selected && snapToCenter)
            {
                direction = Vector3.Lerp(direction, defaultDirection, .1f);
            }
            else
            {
                direction = CurrentInteractor.transform.position - transform.position;
            }
            interactableObject.transform.localRotation = Rotate(direction);
            InvokeEvents();
        }

        protected abstract void InvokeEvents();
        protected  abstract Quaternion Rotate(Vector3 dir);
        protected float CalculateAngle(Vector3 direction,Vector3 normal, Vector3 zero)
        {
            direction = Vector3.ProjectOnPlane(direction.normalized, normal);
            var angle = Vector3.SignedAngle(direction, zero, -normal);
            return angle;

        }

        protected float LimitAngle(float angle, float min, float max)
        {
            if (angle > max) angle = max;

            if (angle < min) angle = min;

            return angle;
        }
    }
}
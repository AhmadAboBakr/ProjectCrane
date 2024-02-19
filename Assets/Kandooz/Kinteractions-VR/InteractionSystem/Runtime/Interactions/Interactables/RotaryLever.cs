using System;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Kandooz.InteractionSystem.Interactions
{
    public class RotaryLever : ConstrainedInteractableBase
    {
        protected override void Activate()
        {
        }

        protected override void StartHover()
        {
        }

        protected override void EndHover()
        {
        }

        private float CalculateAngle(Vector3 plane)
        {
            //-transform.right
            var direction = CurrentInteractor.transform.position - transform.position;
            direction = Vector3.ProjectOnPlane(direction, -plane).normalized;
            var angle = -Vector3.SignedAngle(direction, transform.up, plane);
            return angle;
        }


    }
}
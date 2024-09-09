using System;
using Kandooz.InteractionSystem.Core;
using Kandooz.InteractionSystem.Interactions;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

namespace Kandooz
{
    [RequireComponent(typeof(Rigidbody))]
    public class CraneController : MonoBehaviour
    {
        [SerializeField] private LeverInteractable2D lever;
        [SerializeField] private float speed = 1;
        [SerializeField] private float acceleration;
        [SerializeField] private Vector3 craneDirection = Vector3.forward;
        [SerializeField] private Vector3 truckDirection = Vector3.right;
        [SerializeField] private Rigidbody craneBody;
        [SerializeField] private Rigidbody truckBody;
        [ReadOnly] [SerializeField] private Vector2 direction;

        private void Start()
        {
            lever.OnLeverChanged
                .Do(joyStick => direction = joyStick)
                .Subscribe()
                .AddTo(this);
        }

        private void FixedUpdate()
        {
            MovePart(craneBody, craneDirection, -direction.y);
            MovePart(truckBody, truckDirection, direction.x);
        }

        private void MovePart(Rigidbody body, Vector3 direction, float value)
        {
            if (value < .2f) return;
            var velocity = body.velocity;
            velocity += direction * (acceleration * Time.fixedTime * value);
            velocity = Vector3.ClampMagnitude(velocity, speed);
            body.velocity = velocity;
        }
    }
}
using System;
using Kandooz.InteractionSystem.Interactions;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

namespace Kandooz
{
    [RequireComponent(typeof(Rigidbody))]
    public class TruckController : MonoBehaviour, ICranePart
    {
        [SerializeField] private float speed = 1;
        [SerializeField] private float acceleration;
        [SerializeField] private Vector3 movementDirection;
        private Rigidbody _rigidbody;
        private float _direction = 0;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
        

        private void FixedUpdate()
        {
            var velocity = _rigidbody.velocity;
            velocity += movementDirection * (acceleration * Time.fixedTime * _direction);
            velocity = Vector3.ClampMagnitude(velocity, speed);
            _rigidbody.velocity = velocity;
        }
        public float Direction
        {
            set => _direction = value;
        }
    }
}
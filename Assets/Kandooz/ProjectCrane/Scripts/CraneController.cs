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
        private Rigidbody _rigidBody;
        private float _direction = 0;

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody>();
        }
        

        private void FixedUpdate()
        {
            var velocity = _rigidBody.velocity;
            velocity += movementDirection * (acceleration * Time.fixedTime * _direction);
            velocity = Vector3.ClampMagnitude(velocity, speed);
            _rigidBody.velocity = velocity;
        }
        public float Direction
        {
            set => _direction = value;
        }
    }
}
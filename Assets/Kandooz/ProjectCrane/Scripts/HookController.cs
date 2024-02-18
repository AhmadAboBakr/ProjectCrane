using System;
using System.Collections;
using System.Collections.Generic;
using Kandooz.InteractionSystem.Interactions;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class HookController : MonoBehaviour,ICranePart
{
    [SerializeField] private Transform hookLeft;
    [SerializeField] private Transform hookRight;
    [SerializeField] private float hookMin = 3;
    [SerializeField] private float hookMax = 6;
    [Range(0, 1)] [SerializeField] private float clamp;
    [SerializeField] private float speed = 1;

    private float _direction = 0;
    private Rigidbody _body;

    public float HookClamp
    {
        set => clamp = Mathf.Clamp01(value);
    }

    private void Start()
    {
        _body = GetComponent<Rigidbody>();
    }

    void Update()
    {
        MoveHook();
    }

    private void FixedUpdate()
    {
        _body.velocity = speed * _direction * Vector3.up;

    }
    private void MoveHook()
    {
        var hook = Mathf.Lerp(hookMin, hookMax, clamp);
        hookLeft.localPosition = Vector3.up * -hook;
        hookRight.localPosition = Vector3.up * hook;
    }
    
    

    public float Direction
    {
        set => _direction = value;
    }
}
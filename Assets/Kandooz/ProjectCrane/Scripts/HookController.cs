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
    [SerializeField] private float verticalMin = -12;
    [SerializeField] private float verticalMax = -3;
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
        DebugMove();
        VRMove();
    }

    private void FixedUpdate()
    {
        _body.velocity = speed * _direction * Vector3.up;
        if (transform.localPosition.y > verticalMax)
        {
            transform.localPosition = Vector3.forward * verticalMax;
        }

        if (transform.position.y < verticalMin)
        {
            transform.localPosition = Vector3.forward * verticalMin;
        }
    }

    private void VRMove()
    {
        if (_direction > .1)
            Raise();
        if (_direction < -.1f)
            Lower();
    }

    private void MoveHook()
    {
        var hook = Mathf.Lerp(hookMin, hookMax, clamp);
        hookLeft.localPosition = Vector3.up * -hook;
        hookRight.localPosition = Vector3.up * hook;
    }

    private void DebugMove()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            //Raise();
            _direction = -1;
        }

        if (Input.GetKey(KeyCode.E))
        {
            _direction = 1;
            //Lower();
        }
    }

    public void Raise()
    {
        transform.localPosition += Vector3.forward * (Time.deltaTime * speed);
        if (transform.localPosition.z > verticalMax)
            transform.localPosition = Vector3.forward * verticalMax;
    }

    public void Lower()
    {
        transform.localPosition -= Vector3.forward * Time.deltaTime * speed;
        if (transform.localPosition.z < verticalMin)
            transform.localPosition = Vector3.forward * verticalMin;
    }

    public float Direction
    {
        set => _direction = value;
    }
}
using System;
using Kandooz.InteractionSystem.Interactions;
using UniRx;
using UnityEngine;

public class HookController : MonoBehaviour
{
    [SerializeField] private Transform hookLeft;
    [SerializeField] private Transform hookRight;
    [SerializeField] private float hookMin = 3;
    [SerializeField] private float hookMax = 6;
    [Range(0, 1)] [SerializeField] private float clamp;
    [SerializeField] private float speed = 1;
    [SerializeField] private LeverInteractable1D _leverInteractable;
    [SerializeField] private Rigidbody currentContainer;
    private float _direction = 0;
    private Rigidbody _body;
    private GameObject _currentAttached;
    private Collider[] _colliders;


    public float HookClamp
    {
        set => clamp = Mathf.Clamp01(value);
    }

    private void Start()
    {
        _colliders = GetComponentsInChildren<Collider>();
        _body = GetComponent<Rigidbody>();
        _leverInteractable.OnLeverChanged
            .Do(value => _direction = value)
            .Subscribe().AddTo(this);
        _leverInteractable.OnActivated
            .Where(_ => currentContainer)
            .Select(_ => currentContainer.GetComponent<Rigidbody>())
            .Do(AttachContainer)
            .Subscribe().AddTo(this);
        _leverInteractable.OnDeactivated
            .Where(_ => _currentAttached is not null)
            .Select(_ => _currentAttached)
            .Do(Detach)
            .Subscribe().AddTo(this);
    }

    private void Detach(GameObject body)
    {
        body.transform.parent = null;
        body.AddComponent<Rigidbody>();
    }

    private void AttachContainer(Rigidbody body)
    {
        body.transform.parent = transform;
        _currentAttached = body.gameObject;
        Destroy(body);
        
    }

    private void Update()
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Container")) currentContainer = other.attachedRigidbody;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.attachedRigidbody == currentContainer) currentContainer = null;
    }

    public float Direction
    {
        set => _direction = value;
    }
}
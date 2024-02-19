using System;
using Kandooz.InteractionSystem.Interactions;
using UniRx;
using UnityEngine;

namespace Kandooz.ProjectCrane
{
    [RequireComponent(typeof(ICranePart))]
    [RequireComponent(typeof(MovementAudioFeedback))]
    public class LeverCranePartMover : MonoBehaviour
    {
        [SerializeField] private LeverInteractable lever;
        [SerializeField] private rotationAxe axe;
        [SerializeField] private bool keyboardDebug;
        [SerializeField] private string axisName = "horizontal";
        private ICranePart _cranePart;
        private MovementAudioFeedback _movementAudioFeedback;

        private void Awake()
        {
            _cranePart = GetComponent<ICranePart>();
            _movementAudioFeedback = GetComponent<MovementAudioFeedback>();
        }

        void Start()
        {
            if (_cranePart is null)
            {
                throw new Exception("crane part is invalid");
            }

            lever.OnDeselected
                .Do(_ => _cranePart.Direction = 0)
                .Where(_ => _movementAudioFeedback is not null)
                .Do(_ => _movementAudioFeedback.Moving = false)
                .Subscribe()
                .AddTo(this);
            lever.OnLeverChanged.AsObservable()
                .Select(SelectAxe)
                .Do(direction => _cranePart.Direction = direction)
                .Where(direction=>!Mathf.Approximately(0,direction))
                .Where(_ => _movementAudioFeedback is not null)
                .Do(_ => _movementAudioFeedback.Moving = true)
                .Subscribe()
                .AddTo(this);
        }

        private float SelectAxe(LeverInteractable.XZPair movement)
        {
            return (axe == rotationAxe.x) ? movement.x : movement.z;
        }

        private void Update()
        {
            if (!keyboardDebug && _cranePart is not null) return;
            var axe = Input.GetAxis(axisName);
            _cranePart.Direction = axe;
            _movementAudioFeedback.Moving = Mathf.Abs(axe) > .1f;
        }

        private enum rotationAxe
        {
            x,
            z
        }
    }
}
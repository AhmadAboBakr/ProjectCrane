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
            lever.OnOpen
                .Do(_ => _cranePart.Direction = 1)
                .Where(_ => _movementAudioFeedback is not null)
                .Do(_ => _movementAudioFeedback.Moving = true)
                .Subscribe()
                .AddTo(this);
            lever.OnClose
                .Do(_ => _cranePart.Direction = -1)
                .Where(_ => _movementAudioFeedback is not null)
                .Do(_ => _movementAudioFeedback.Moving = true)
                .Subscribe()
                .AddTo(this);
        }

        private void Update()
        {
            if (!keyboardDebug && _cranePart is not null) return;
            var axe = Input.GetAxis(axisName);
            _cranePart.Direction = axe;
            _movementAudioFeedback.Moving = Mathf.Abs(axe) > .1f;
        }
    }
}
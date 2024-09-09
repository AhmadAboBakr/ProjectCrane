using System;
using Kandooz.Interactions;
using Kandooz.InteractionSystem.Core;
using UniRx;
using UnityEngine;

namespace Kandooz.InteractionSystem.Interactions
{
    [RequireComponent(typeof(Hand))]
    public abstract class InteractorBase : MonoBehaviour
    {
        [SerializeField] [ReadOnly] protected InteractableBase currentInteractable;
        [SerializeField] [ReadOnly] protected bool isInteracting;
        private Hand _hand;
        private Transform _attachmentPoint;
        private readonly Subject<ButtonState> _onInteractionStateChanged = new();
        private readonly Subject<ButtonState> _onActivate = new();
        private IDisposable _hoverSubscriber, _activationSubscriber;
        private Joint _attachmentJoint;
        public Transform AttachmentPoint => _attachmentPoint;
        public HandIdentifier HandIdentifier => _hand.HandIdentifier;
        public Hand Hand => _hand;
        protected bool IsInteracting => isInteracting;

        public void ToggleHandModel(bool enable)
        {
            _hand.ToggleRenderer(enable);
        }

        private void Awake()
        {
            GetDependencies();
            InitializeAttachmentPoint();
            _onInteractionStateChanged
                .Do((state) =>
                {
                    if (currentInteractable is null) return;
                    switch (state)
                    {
                        case ButtonState.Up:
                            if (currentInteractable.CurrentState == InteractionState.Selected && currentInteractable.CurrentInteractor == this) OnDeSelect();

                            break;
                        case ButtonState.Down:
                            if (currentInteractable.CurrentState == InteractionState.Hovering) OnSelect();

                            break;
                    }
                })
                .Subscribe().AddTo(this);
            _onActivate
                .Do((state) =>
                {
                    if (currentInteractable is null) return;
                    switch (state)
                    {
                        case ButtonState.Down:
                            OnActivate();
                            break;
                    }
                })
                .Subscribe().AddTo(this);
        }


        private void GetDependencies()
        {
            _hand = GetComponent<Hand>();
        }

        private void InitializeAttachmentPoint()
        {
            var attachmentObject = new GameObject("AttachmentPoint");
            attachmentObject.transform.parent = transform;
            _attachmentPoint = attachmentObject.transform;
            _attachmentPoint.localPosition = Vector3.zero;
            _attachmentPoint.localRotation = Quaternion.identity;
        }

        protected void OnHoverStart()
        {
            if (currentInteractable == null || currentInteractable.Selected) return;
            if (!currentInteractable.IsValidHand(Hand)) return;

            currentInteractable.OnStateChanged(InteractionState.Hovering, this);
            var onInteractButtonPressed = currentInteractable.SelectionButton switch
            {
                XRButton.Grip => _hand.OnGripButtonStateChange,
                XRButton.Trigger => _hand.OnTriggerTriggerButtonStateChange,
                _ => null
            };
            _hoverSubscriber = onInteractButtonPressed.Do(_onInteractionStateChanged).Subscribe();
        }

        protected virtual void OnHoverEnd()
        {
            if (currentInteractable.CurrentState != InteractionState.Hovering) return;
            currentInteractable.OnStateChanged(InteractionState.None, this);
            _hoverSubscriber?.Dispose();
            currentInteractable = null;
        }

        protected void OnSelect()
        {
            if (currentInteractable == null || currentInteractable.Selected) return;
            isInteracting = true;
            currentInteractable.OnStateChanged(InteractionState.Selected, this);

            var onInteractButtonPressed = currentInteractable.SelectionButton switch
            {
                XRButton.Trigger => _hand.OnGripButtonStateChange,
                XRButton.Grip => _hand.OnTriggerTriggerButtonStateChange,
                _ => null
            };
            _activationSubscriber = onInteractButtonPressed.Do(_onActivate).Subscribe();
        }

        private void OnDeSelect()
        {
            isInteracting = false;
            _activationSubscriber?.Dispose();
            _hoverSubscriber?.Dispose();
            currentInteractable.OnStateChanged(InteractionState.None, this);
            OnHoverStart();
        }

        private void OnActivate()
        {
            if (!currentInteractable) return;

            currentInteractable.OnStateChanged(InteractionState.Activated, this);
        }
    }
}
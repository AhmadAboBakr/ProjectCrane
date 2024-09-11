using System;
using Kandooz.Interactions;
using Kandooz.InteractionSystem.Core;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

namespace Kandooz.InteractionSystem.Interactions
{
    [Flags]
    public enum InteractionHand
    {
        Left = 1,
        Right = 2
    }

    public abstract class InteractableBase : MonoBehaviour
    {
        [SerializeField] private InteractionHand interactionHand = InteractionHand.Left | InteractionHand.Right;
        [SerializeField] private XRButton selectionButton = XRButton.Grip;
        [SerializeField] public InteractorUnityEvent onSelected;
        [SerializeField] private InteractorUnityEvent onDeselected;
        [SerializeField] private InteractorUnityEvent onHoverStart;
        [SerializeField] private InteractorUnityEvent onHoverEnd;
        [SerializeField] private InteractorUnityEvent onActivated;
        [SerializeField] private InteractorUnityEvent onDeactivated;

        [FormerlySerializedAs("isSelected")] [SerializeField] [ReadOnly]
        private bool selected;

        [SerializeField] [ReadOnly] private InteractorBase currentInteractor;
        [SerializeField] [ReadOnly] private InteractionState currentState;
        private bool _activated;
        public bool Selected => selected;
        public IObservable<InteractorBase> OnSelected => onSelected.AsObservable();
        public IObservable<InteractorBase> OnDeselected => onDeselected.AsObservable();
        public IObservable<InteractorBase> OnHoverStarted => onHoverStart.AsObservable();
        public IObservable<InteractorBase> OnHoverEnded => onHoverEnd.AsObservable();
        public IObservable<InteractorBase> OnActivated => onActivated.AsObservable();
        public IObservable<InteractorBase> OnDeactivated => onDeactivated.AsObservable();
        public XRButton SelectionButton => selectionButton;
        public InteractionState CurrentState => currentState;
        internal InteractorBase CurrentInteractor => currentInteractor;


        public void OnStateChanged(InteractionState state, InteractorBase interactor)
        {
            if (currentState == state) return;
            currentInteractor = interactor;
            switch (state)
            {
                case InteractionState.None:
                    HandleNoneState();
                    break;
                case InteractionState.Hovering:
                    HandleHoverState();
                    break;
                case InteractionState.Selected:
                    HandleSelectionState();
                    break;
                case InteractionState.Activated:
                    HandleActiveState();
                    break;
            }
        }

        private void HandleNoneState()
        {
            switch (currentState)
            {
                case InteractionState.Selected:
                    DeSelected();
                    selected = false;
                    onDeselected.Invoke(currentInteractor);
                    currentInteractor = null;
                    break;
                case InteractionState.Hovering:
                    EndHover();
                    onHoverEnd.Invoke(currentInteractor);
                    break;
            }

            currentState = InteractionState.None;
            currentInteractor = null;
        }

        private void HandleHoverState()
        {
            if (currentState is InteractionState.Selected or InteractionState.Activated)
            {
                selected = false;
                onDeselected.Invoke(currentInteractor);
                DeSelected();
            }

            currentState = InteractionState.Hovering;
            StartHover();
            onHoverStart.Invoke(currentInteractor);
        }

        private void HandleSelectionState()
        {
            if (currentState == InteractionState.Activated)
            {
                onDeactivated.Invoke(currentInteractor);
            }

            if (currentState == InteractionState.Hovering)
            {
                onHoverEnd.Invoke(currentInteractor);
                EndHover();
                onSelected.Invoke(currentInteractor);
                Select();

            }
            
            selected = true;
            currentState = InteractionState.Selected;
        }

        private void HandleActiveState()
        {
            if (currentState == InteractionState.Selected)
            {
                _activated = true;
                onActivated?.Invoke(currentInteractor);
                currentState = InteractionState.Activated;
                Activate();
            }
        }

        protected virtual void Activate()
        {
        }

        protected virtual void StartHover()
        {
        }

        protected virtual void EndHover()
        {
        }

        protected virtual void Select()
        {
        }

        protected virtual void DeSelected()
        {
        }

        public bool IsValidHand(HandIdentifier hand)
        {
            var handID = (int)hand;
            var valid = (int)interactionHand;
            return (valid & handID) != 0;
        }
    }
}
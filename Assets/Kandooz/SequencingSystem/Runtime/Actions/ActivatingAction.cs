using Kandooz.InteractionSystem.Interactions;
using UniRx;
using UnityEngine;

namespace Kandooz.Kuest
{
    public enum ActionType {
        ActivationAction,
        AnimationAction,
        ButtonPressAction,
        GazeAction,
        InteractionAction,
        InsertionAction,
        TimerAction,
        TriggerAction,
        VoiceOverAction,
        ComplexAction
    }
    [AddComponentMenu("Kandooz/SequenceSystem/Actions/ActivationAction")]

    [RequireComponent(typeof(StepEventListener))]

    public class ActivatingAction : MonoBehaviour
    {
        [SerializeField] private ActionType action;
        [SerializeField] private InteractableBase interactableObject;
        private StepEventListener listener;
        private CompositeDisposable disposable;
        private void Awake()
        {
            listener = GetComponent<StepEventListener>();
            listener.OnStarted.Do(OnStarted).Subscribe().AddTo(this);
            listener.OnFinished.Do(_ => disposable.Dispose()).Subscribe().AddTo(this);
        }

        void OnStarted(Unit unit)
        {
            disposable = new();
            interactableObject.OnActivated.Do(OnInteractionStarted).Subscribe().AddTo(disposable);
        }

        private void OnInteractionStarted(InteractorBase interactor)
        {
            listener.OnActionCompleted();
        }
    }
}
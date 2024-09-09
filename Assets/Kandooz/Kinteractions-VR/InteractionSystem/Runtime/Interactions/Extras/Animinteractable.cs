using Kandooz.InteractionSystem.Interactions;
using UniRx;
using UnityEngine;

namespace _2_Kandooz.Kinteractions_VR.InteractionSystem.Runtime.Interactions.Extras
{
    [RequireComponent(typeof(InteractableBase))]
    [RequireComponent(typeof(Animator))]
    public class Animinteractable : MonoBehaviour
    {
        [SerializeField] private string hoverBoolName="Hovered";
        [SerializeField] private string selectedTrigger="Selected";
        [SerializeField] private string unselectedTrigger="Deselected";

        private void Awake()
        {
            
            var animator = GetComponent<Animator>();
            var intractable = GetComponent<InteractableBase>();
            intractable.OnHoverStarted.Do(_ => animator.SetBool(hoverBoolName,true)).Subscribe().AddTo(this);
            intractable.OnHoverEnded.Do(_ => animator.SetBool(hoverBoolName,false)).Subscribe().AddTo(this);
            intractable.OnSelected.Do(_ => animator.SetTrigger(selectedTrigger)).Subscribe().AddTo(this);
            intractable.OnDeselected.Do(_ => animator.SetTrigger(unselectedTrigger)).Subscribe().AddTo(this);

        }
    }
}
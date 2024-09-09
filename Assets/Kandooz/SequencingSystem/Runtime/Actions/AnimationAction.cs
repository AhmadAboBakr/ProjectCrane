using UniRx;
using UnityEngine;

namespace Kandooz.Kuest
{
    [AddComponentMenu("Kandooz/SequenceSystem/Actions/AnimationAction")]

    public class AnimationAction : MonoBehaviour
    {
        [SerializeField] private string animationTriggerName;
        [SerializeField] private Animator animator;
        void Awake()
        {
        }
        /// <summary>
        /// this function must be called from the animation 
        /// </summary>
        public void AnimationEnded()
        {
            animator.SetTrigger(animationTriggerName);
        }
    }
}
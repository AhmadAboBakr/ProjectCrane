using Kandooz.ScriptableSystem;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace Kandooz.Kuest
{
    [AddComponentMenu("Kandooz/SequenceSystem/Actions/TriggerAction")]

    public class TriggerAction : MonoBehaviour
    {
        [SerializeField]private string objectTag;
        [SerializeField] private UnityEvent onTRiggerEnter;


        private void Awake()
        {
        }

        private void OnTriggerEnter(Collider other)
        {
            
            if (string.IsNullOrEmpty(objectTag) || other.attachedRigidbody. CompareTag(objectTag))
            {
                onTRiggerEnter.Invoke();
            }
        }
    }
}
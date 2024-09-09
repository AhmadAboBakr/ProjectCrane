using Kandooz.InteractionSystem.Core;
using Kandooz.ScriptableSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace Kandooz.Kuest
{
    public enum SequenceStatus
    {
        Inactive,
        Started,
        Completed
    }
    public abstract class SequenceNode : GameEvent<SequenceStatus>
    {
        [SerializeField] protected SequenceStatus status = SequenceStatus.Inactive;
        [SerializeField,ReadOnly]internal AudioSource audioObject;
        public abstract void Begin();
        protected override SequenceStatus DefaultValue => status;
    }
}
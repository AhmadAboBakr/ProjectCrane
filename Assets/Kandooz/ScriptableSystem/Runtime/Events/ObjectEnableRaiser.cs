using UnityEngine;

namespace Kandooz.ScriptableSystem
{
    public class ObjectEnableRaiser : MonoBehaviour
    {
        [SerializeField]private GameEvent @event;
        private void OnEnable()
        {
            @event.Raise();
        }
    }
}
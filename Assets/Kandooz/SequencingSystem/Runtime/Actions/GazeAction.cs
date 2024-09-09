using UniRx;
using UnityEngine;

namespace Kandooz.Kuest
{
    [AddComponentMenu("Kandooz/SequenceSystem/Actions/Gaze Action")]

    [RequireComponent(typeof(StepEventListener))]
    [RequireComponent(typeof(Collider))]
    public class GazeAction : MonoBehaviour
    {
        private StepEventListener listener;
        private Collider collider;
        private bool started;
        private Transform player;

        private void Awake()
        {
            collider = GetComponent<Collider>();
            player = Camera.main.transform;
            listener = GetComponent<StepEventListener>();
            listener.OnStarted.Do((_) => started = true).Subscribe().AddTo(this);
            listener.OnFinished.Do((_) => started = false).Subscribe().AddTo(this);
        }

        private void Update()
        {
            if (!started) return;
            var ray = new Ray(player.position, player.forward);
            RaycastHit hitInfo;
            if (collider.Raycast(ray, out hitInfo, 20))
            {
                listener.OnActionCompleted();
            }
        }
    }
}
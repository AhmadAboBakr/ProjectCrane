using Kandooz.InteractionSystem.Core;
using Kandooz.Kinteractions.InteractionSystem.Interactions;
using UniRx;
using UnityEngine;

namespace Kandooz.InteractionSystem.Interactions
{
    [RequireComponent(typeof(InteractableBase))]
    public class Socketable : MonoBehaviour
    {
        [SerializeField] private Transform start;
        [SerializeField] private Transform end;
        [SerializeField] private float radius = .1f;
        [SerializeField] private LayerMask mask;
        [ReadOnly] [SerializeField] private Socket socket;

        private Transform defaultPivot;
        private InteractableBase interactable;
        private VariableTweener tweener;
        private TransformTweenable transformTweener;
        private Collider[] collider = new Collider[1];
        private bool socketed;

        private void Awake()
        {
            defaultPivot = new GameObject("SnapperPivot").transform;
            defaultPivot.position = transform.position;
            defaultPivot.parent = transform.parent;
            defaultPivot.rotation = transform.rotation;
            interactable = GetComponent<InteractableBase>();
            tweener = GetComponent<VariableTweener>();
            if (!tweener) tweener = gameObject.AddComponent<VariableTweener>();
            interactable.OnDeselected.Do(_ => ReturnToPivot()).Subscribe().AddTo(this);
            interactable.OnSelected.Do(_ => RemoveFromSocket()).Subscribe().AddTo(this);
        }

        public void RemoveFromSocket()
        {
            if (socket)
            {
                socketed = false;
                socket.RemoveSocketable();
                socket = null;
            }
        }


        public void ReturnToPivot()
        {
            socketed = socket is not null;
            if (socketed) socket.InsertSocketable(this);
            var pivot = socketed ? socket.Pivot : defaultPivot;
            transform.parent = pivot ? pivot.parent : null;
            transformTweener = new TransformTweenable();
            transformTweener.Initialize(transform, pivot);
            tweener.AddTweenable(transformTweener);
        }

        private void Update()
        {
            DebugInput();
                
            if (socketed) return;
            var count = Physics.OverlapCapsuleNonAlloc(start.position, end.position, radius, collider, mask);
            if (count == 0)
            {
                socket = null;
                return;
            }

            socket = collider[0].GetComponent<Socket>();
        }

        private void DebugInput()
        {
            if (Input.GetKeyDown(KeyCode.S) )
            {
                if (socketed) RemoveFromSocket();
                ReturnToPivot();
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1, 0, 0, .2f);
            Gizmos.DrawWireSphere(start.position, radius);
            Gizmos.DrawWireSphere(end.position, radius);
            
        }
    }
}
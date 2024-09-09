
using UnityEngine;
using UnityEngine.Events;
using Task = System.Threading.Tasks.Task;

namespace Kandooz.InteractionSystem.Core
{
    public class BlinderController : MonoBehaviour
    {
        [SerializeField] private Camera camera;
        [SerializeField] private float speed = 2;
        [SerializeField] private int blinkDelay=100;
        [SerializeField] private UnityEvent onBlink;

        [ReadOnly] [SerializeField] private float t = 0;

        [SerializeField] private bool blinded = false;
        private Material material;
        private Renderer meshRenderer;

        private void Start()
        {
            camera ??= Camera.main;
            meshRenderer = GetComponent<MeshRenderer>();
            material = meshRenderer.material;
        }

        private void Update()
        {
            t += (blinded ? -Time.deltaTime : Time.deltaTime) * speed;
            t = Mathf.Clamp01(t);
            material.SetFloat("_Value", t);
            meshRenderer.enabled = !(t > .98f);
            //transform.localScale = Vector3.one * Mathf.Lerp(camera.nearClipPlane + .1f, camera.farClipPlane- 10  , t);
        }

        public void Blind()
        {
            blinded = true;
        }

        public void UnBlind()
        {
            blinded = false;
        }
        public async void Blink()
        {
            blinded = true;
            while (t>0)
            {
                await Task.Yield();
            }
            onBlink.Invoke();
            await Task.Delay(blinkDelay);
            blinded = false;
        }
    }
}
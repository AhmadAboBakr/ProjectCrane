using System;
using Kandooz.InteractionSystem.Animations.Constraints;
using UniRx.Triggers;
using UnityEngine;

namespace Kandooz.InteractionSystem.Core
{
    public class Hand : MonoBehaviour
    {
        [SerializeField] private HandIdentifier hand;
        [SerializeField] private Config config;
        [SerializeField] private Renderer handModel;
        [SerializeField] private bool debug;
        private IPoseable poseDriver;
        
        public HandIdentifier HandIdentifier
        {
            get => hand;
            internal set => hand = value;
        }
        public IObservable<ButtonState> OnTriggerTriggerButtonStateChange => config.InputManager[hand].TriggerObservable ;
        public IObservable<ButtonState> OnGripButtonStateChange =>  config.InputManager[hand].GripObservable;
        public float this[FingerName index] => config.InputManager[hand][(int)index];
        public float this[int index] => config.InputManager[hand][index];
        internal Config Config
        {
            set => config = value;
        }

        public void ToggleRenderer(bool enable) => handModel.enabled = enable;

        private void Awake()
        {
            poseDriver = GetComponent<IPoseable>();
            if (handModel == null) handModel = GetComponentInChildren<MeshRenderer>();
        }

        public void Constrain(IPoseConstrainer constrainer)
        {
            switch (hand)
            {
                case HandIdentifier.Left:
                    poseDriver.Constrains = constrainer.LeftPoseConstrains;
                    break;
                case HandIdentifier.Right:
                    poseDriver.Constrains = constrainer.RightPoseConstrains;
                    break;
            }
        }
        public void Unconstrain(IPoseConstrainer constrain)
        {
            poseDriver.Constrains= PoseConstrains.Free;
        }

        private void Update()
        {
            if (debug)
            {
                this.transform.position += transform.right * Input.GetAxis("Horizontal")*Time.deltaTime/10;
                this.transform.position += transform.forward * Input.GetAxis("Vertical")*Time.deltaTime/10;
            }
        }

        public static implicit operator HandIdentifier(Hand hand) => hand.HandIdentifier;
    }
}
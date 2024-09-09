using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Kandooz.InteractionSystem.Core;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.TextCore.Text;

[assembly: InternalsVisibleTo("Kandooz.Kuest.Editor")]

namespace Kandooz.Kuest
{
    [CreateAssetMenu(menuName = "Kandooz/SequenceSystem/Sequence")]
    public class Sequence : SequenceNode
    {
        [SerializeField,Range(0.1f,2)] internal float pitch = 1;
        [SerializeField,Range(0,1)] private float volume=.5f;

        [HideInInspector] [SerializeField] private List<Step> steps;
        
        [SerializeField,ReadOnly]private int currentStepIndex;
        private bool initialized;

        public bool Started => status== SequenceStatus.Started;
        public Step CurrentStep => currentStepIndex<steps.Count?steps[currentStepIndex]:null;
        public List<Step> Steps => steps;

        private void Awake()
        {
            initialized = false;
        }

        private void OnEnable()
        {
            Awake();
        }

        public override void Begin()
        {
            Debug.Log($"starting sequence{name}");
            currentStepIndex = 0;

            status = SequenceStatus.Started;
            if (!initialized)
            {
                initialized = true;
                audioObject = new GameObject($"{name}_AudioObject").AddComponent<AudioSource>();
                audioObject.loop = false;
                audioObject.playOnAwake = false;
                audioObject.pitch = pitch;
                audioObject.volume = volume;
            }
            foreach (var step in steps)
            {
                step.audioObject = audioObject;
                step.Initialize(this);
            }

            steps[currentStepIndex].Begin();
            Raise(SequenceStatus.Started);
        }
        internal void CompleteStep(Step step)
        {
            if (steps[currentStepIndex] != step) return;
            currentStepIndex++;
            if (currentStepIndex < steps.Count)
            {
                steps[currentStepIndex].Begin();
                return;
            }

            status = SequenceStatus.Completed;
            Raise(SequenceStatus.Completed);
        }

        public void PlayClip(AudioClip clip)
        {
            audioObject.Stop();
            audioObject.clip = clip;
            audioObject.Play();
        }
    }
}
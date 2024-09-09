using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kandooz.InteractionSystem.Core;
using UniRx;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Kandooz.Kuest
{
    public class SequenceBehaviour : MonoBehaviour
    {
        [SerializeField] public Sequence sequence;
        [HideInInspector] [SerializeField] private bool startOnSpace;
        [ReadOnly] [SerializeField] private bool started;

        [SerializeField] private bool starOnAwake = false;
        [HideInInspector] [SerializeField] private float delay = 0;

        [SerializeField] private UnityEvent onSequenceStarted;
        [SerializeField] private UnityEvent onSequenceCompleted;

        [HideInInspector] [SerializeField] internal List<StepEventPair> steps;
        [HideInInspector] [SerializeField] internal List<StepEventListener> stepListeners;
        public bool listner;
        public bool StarOnAwake => starOnAwake;
        private float time = 0;

        private void Awake()
        {
            if (listner)
            {
                sequence.Steps[0].OnRaisedData.Do(_ =>
                {
                    try
                    {
                        var data = new Dictionary<string, object>();
                        time = 0;
                        data.Add("time", Time.realtimeSinceStartup);
                        data.Add("type", true);
                        data.Add("name", sequence.name);
                        var result = Analytics.CustomEvent("started", data);
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e);
                        throw;
                    }
                }).Subscribe().AddTo(this);
                sequence.Steps[sequence.Steps.Count - 1].OnRaisedData.Do(_ =>
                {
                    try
                    {

                        var data = new Dictionary<string, object>();
                        data.Add("time", Time.realtimeSinceStartup - time);
                        data.Add("type", true);
                        data.Add("name", sequence.name);
                        Analytics.CustomEvent("started", data);
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e);
                        throw;
                    }
                }).Subscribe().AddTo(this);
            }
        }

        private async void OnEnable()
        {
            sequence.OnRaisedData.Where(status => status == SequenceStatus.Started).Do(_ => onSequenceStarted.Invoke()).Subscribe().AddTo(this);
            sequence.OnRaisedData.Where(status => status == SequenceStatus.Started).Do(_ => onSequenceCompleted.Invoke()).Subscribe().AddTo(this);
            if (!StarOnAwake) return;
            while (delay > 0)
            {
                await Task.Yield();
                delay -= Time.deltaTime;
            }

            StartQuest();
        }

        public void StartQuest()
        {
            sequence.Begin();
            started = true;
        }

        private void Update()
        {
            if (startOnSpace && !started && Input.GetKeyDown(KeyCode.Space))
                StartQuest();
            else if (started && Input.GetKeyDown(KeyCode.Space)) sequence.CurrentStep.OnActionCompleted();
        }

        [Serializable]
        public class StepEventPair
        {
            public UnityEvent listeners;
            public Step step;

            public StepEventPair(Step step)
            {
                this.step = step;
            }
        }
    }
}
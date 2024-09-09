using System.Collections.Generic;
using Kandooz.InteractionSystem.Core;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Kandooz.InteractionSystem.Animations
{
    /// <summary>
    /// Control the pose of the hand and it's fingers
    /// </summary>
    [RequireComponent(typeof(VariableTweener))]
    public class HandPoseController : MonoBehaviour, IPoseable
    {

        [HideInInspector] [SerializeField] private HandData handData;

        [Range(0, 1)] [HideInInspector] [SerializeField]
        private float[] fingers = new float[5];

        [HideInInspector] [SerializeField] private int currentPoseIndex;
        private List<IPose> _poses;
        private Hand _hand;
        private VariableTweener _variableTweener;
        private AnimationMixerPlayable _handMixer;
        private Animator _animator;
        PlayableGraph _graph;
        private PoseConstrains _constrains = PoseConstrains.Free;

        public float this[FingerName index]
        {
            get => this[(int)index];
            set => this[(int)index] = value;
        }
        public float this[int index]
        {
            get => fingers[index];
            set
            {
                fingers[index] = value;
                _poses[currentPoseIndex][index] = value;
            }
        }

        public int Pose
        {
            set => currentPoseIndex = value;
        }
        
        /// <summary>
        /// sets the constraints to the fingers within the pose
        /// </summary>
        public PoseConstrains Constrains
        {
            set => _constrains = value;
        }
        public int CurrentPoseIndex
        {
            get => currentPoseIndex;
            set
            {
                _handMixer.SetInputWeight(currentPoseIndex, 0);
                _handMixer.SetInputWeight(value, 1);
                currentPoseIndex = value;
                for (int finger = 0; finger < fingers.Length; finger++)
                {
                    _poses[value][finger] = fingers[finger];
                }
            }
        }

        public HandData HandData
        {
            get => handData;
            internal set => handData = value;
        }

        public PlayableGraph Graph => _graph;
        public List<IPose> Poses => _poses;

        public void Start()
        {
            Initialize();
        }
        
        public void Initialize()
        {
            if (!handData)
            {
                Debug.LogError("please select a hand data object");
                return;
            }

            GetDependencies();
            InitializeGraph();
        }

        private void GetDependencies()
        {
            _variableTweener = GetComponent<VariableTweener>();
            _animator = GetComponentInChildren<Animator>();
            _hand = GetComponent<Hand>();
            if (!_animator)
            {
                Debug.LogError("Please add animator to the object or it's children");
            }
        }

        private void InitializeGraph()
        {
            CreateGraphAndSetItsOutputs();
            InitializePoses();
            _graph.Play();
        }

        private void CreateGraphAndSetItsOutputs()
        {
            _graph = PlayableGraph.Create(this.name);
            _graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);
            _handMixer = AnimationMixerPlayable.Create(_graph, handData.Poses.Length);
            var playableOutput = AnimationPlayableOutput.Create(_graph, "Hand mixer", _animator);
            playableOutput.SetSourcePlayable(_handMixer);
        }

        private void InitializePoses()
        {
            _poses = new List<IPose>(handData.Poses.Length + 1);
            for (int i = 0; i < handData.Poses.Length; i++)
            {
                CreateAndConnectPose(i, handData.Poses[i]);
            }
        }

        private void CreateAndConnectPose(int poseID, PoseData data)
        {
            IPose pose = data.Type == PoseData.PoseType.Dynamic ? CreateDynamicPose(poseID, data) : CreateStaticPose(poseID, data);
            _poses.Add(pose);
        }

        private IPose CreateStaticPose(int poseID, PoseData data)
        {
            var pose = new StaticPose(_graph, data);
            _graph.Connect(pose.Mixer, 0, _handMixer, poseID);
            return pose;
        }

        private IPose CreateDynamicPose(int poseID, PoseData data)
        {
            var pose = new DynamicPose(_graph, data, handData, _variableTweener);
            _graph.Connect(pose.PoseMixer, 0, _handMixer, poseID);
            pose.PoseMixer.SetInputWeight(0, 1);
            return pose;
        }

        private void Update()
        {
            UpdateGraphVariables();
            UpdateFingersFromHand();
        }

        public void UpdateGraphVariables()
        {
            if (!_graph.IsValid())
            {
                InitializeGraph();
            }

            for (int i = 0; i < fingers.Length; i++)
            {
                this[i] = fingers[i];
            }

            CurrentPoseIndex = currentPoseIndex;
            _graph.Evaluate();
        }

        private void UpdateFingersFromHand()
        {
            for (var i = 0; i < 5; i++)
            {
                this[i] = _constrains[i].GetConstrainedValue(_hand[i]);
            }
        }

    }
}
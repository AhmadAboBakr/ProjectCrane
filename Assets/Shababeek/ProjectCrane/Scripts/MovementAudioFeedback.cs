using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Kandooz
{
    [RequireComponent(typeof(AudioSource))]
    public class MovementAudioFeedback : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private int minVolume = 0;
        [SerializeField] private float maxVolume = 1;
        [SerializeField] private float lerpSpeed = 1;

        private float _volume;
        private bool _moving;

        public bool Moving
        {
            set => _moving = value;
        }

        private void Update()
        {
            if (_moving)
            {
                _volume += Time.deltaTime*lerpSpeed;
            }
            else
            {
                _volume -= Time.deltaTime*lerpSpeed;
            }

            _volume = Mathf.Clamp(_volume, minVolume, maxVolume);
            audioSource.volume = _volume;
        }
    }
}
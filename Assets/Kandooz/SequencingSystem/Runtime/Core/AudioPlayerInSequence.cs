using UnityEngine;

namespace Kandooz.Kuest
{
    public class AudioPlayerInSequence : MonoBehaviour
    {
        [SerializeField] private Sequence sequence;
        [SerializeField] private AudioClip clip;

        public void Play()
        {
            sequence.PlayClip(clip);
        }
    }
}
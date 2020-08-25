using UnityEngine;

namespace Assets.Scripts.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class OnHoverSoundPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _audioClip;

        public void PlayClip()
        {
            if (_audioSource != null)
            {
                _audioSource.clip = _audioClip;
                _audioSource.Play();
            }
        }
    }
}

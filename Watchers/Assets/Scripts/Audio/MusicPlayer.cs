using UnityEngine;

namespace Assets.Scripts.Audio
{
    public class MusicPlayer : MonoBehaviour
    {
        private AudioSource _audioSource;
        private bool _shouldFadeOut;

        public void Setup(bool shouldLoop, AudioSource audioSource, AudioClip audioClip)
        {
            _audioSource = audioSource ?? gameObject.AddComponent<AudioSource>();
            _audioSource.loop = shouldLoop;
            _audioSource.clip = audioClip ?? null;
        }

        private void Update()
        {
            if (_shouldFadeOut)
            {
                _audioSource.volume -= Time.deltaTime;
            }
        }

        public void PlayMusic()
        {
            if (_audioSource != null)
            {
                _audioSource.Play();
            }
        }

        public void FadeOutMusic()
        {
            _shouldFadeOut = true;
        }
    }
}

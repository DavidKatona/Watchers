using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _audioClips;
    [SerializeField] private bool _callOnAwake;

    private void Awake()
    {
        if (_callOnAwake)
        {
            PlayRandomClip();
        }
    }

    public void PlayRandomClip()
    {
        if (_audioSource != null)
        {
            var clipIndex = Random.Range(0, _audioClips.Length);

            _audioSource.clip = _audioClips[clipIndex];
            _audioSource.Play();
        }
    }
}

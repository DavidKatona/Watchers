using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSound : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _audioClips;

    public void PlayDamageSound()
    {
        if (_audioSource != null)
        {
            var clipIndex = Random.Range(0, _audioClips.Length);

            _audioSource.clip = _audioClips[clipIndex];
            _audioSource.Play();
        }
    }
}

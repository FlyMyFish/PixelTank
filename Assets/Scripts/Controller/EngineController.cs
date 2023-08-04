using System;
using UnityEngine;

namespace Controller
{
    public class EngineController : MonoBehaviour
    {
        public AudioClip moveSound;
        private AudioSource _audioSource;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        /*public void PlayMoveSound()
        {
            _audioSource.PlayOneShot(moveSound);
        }*/
    }
}

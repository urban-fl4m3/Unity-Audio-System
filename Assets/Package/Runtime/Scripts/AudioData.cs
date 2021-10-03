using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Package.Runtime.Scripts
{
    [Serializable]
    public struct AudioData
    {
        public AudioClip Clip
        {
            get => _clip;
            set => _clip = value;
        }
        
        public AudioMixerGroup Group
        {
            get => _group;
            set => _group = value;
        }

        public float Volume
        {
            get => _volume;
            set => _volume = value;
        }

        public bool Loop
        {
            get => _loop;
            set => _loop = value;
        }

        [SerializeField] private AudioClip _clip;
        [SerializeField] private AudioMixerGroup _group;
        [SerializeField] private float _volume;
        [SerializeField] private bool _loop;
    }
}
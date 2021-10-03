using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Package.Runtime.Scripts
{
    public class AudioSourcePool
    {
        private readonly GameObject _sourceContainer;
        private readonly Dictionary<string, AudioSource> _sourcesWithTrack = new Dictionary<string, AudioSource>();
        private readonly List<AudioSource> _sources = new List<AudioSource>();
        
        public AudioSourcePool()
        {
            _sourceContainer = new GameObject("AudioSourceContainer");
            Object.DontDestroyOnLoad(_sourceContainer);
        }

        public AudioSource GetFreeAudioSource()
        {
            var freeSource = _sources.FirstOrDefault(x => !x.isPlaying);

            if (freeSource != null)
            {
                var trackedSource = _sourcesWithTrack.FirstOrDefault(x => x.Value == freeSource);

                if (trackedSource.Value != null)
                {
                    _sourcesWithTrack.Remove(trackedSource.Key);
                }
            }
            else
            {
                freeSource = _sourceContainer.gameObject.AddComponent<AudioSource>();
                freeSource.playOnAwake = false;
                _sources.Add(freeSource);
            }

            return freeSource;
        }

        public AudioSource GetAudioSource(string audioTrack)
        {
            if (_sourcesWithTrack.ContainsKey(audioTrack))
            {
                return _sourcesWithTrack[audioTrack];
            }

            var source = GetFreeAudioSource();
            _sourcesWithTrack.Add(audioTrack, source);
            return source;
        }
    }
}
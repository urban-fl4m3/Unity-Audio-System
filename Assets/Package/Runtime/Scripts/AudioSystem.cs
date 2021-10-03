using UnityEngine;

namespace Package.Runtime.Scripts
{
    public class AudioSystem : IAudioSystem
    {
        private readonly IAudioSystemSettings _settings;
        private readonly AudioSourcePool _audioPool;
        
        public AudioSystem(IAudioSystemSettings settings)
        {
            _settings = settings;
            _audioPool = new AudioSourcePool();
        }

        public void PlayOneShot(AudioData data)
        {
            if (data.Group == null) data.Group = _settings.BaseGroup;
            data.Loop = false;
            
            PlaySound(_audioPool.GetFreeAudioSource(), data);
        }

        public void PlayInAudioTrack(AudioData data, string audioTrackName)
        {
            var source = _audioPool.GetAudioSource(audioTrackName);
            
            PlaySound(source, data);
        }

        private void PlaySound(AudioSource source, AudioData data)
        {
            source.clip = data.Clip;
            source.loop = data.Loop;
            source.volume = data.Volume;
            source.outputAudioMixerGroup = data.Group;
            source.Play();
        }
    }
}
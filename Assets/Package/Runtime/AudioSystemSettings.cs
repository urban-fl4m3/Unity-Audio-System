using Package.Runtime.Scripts;
using UnityEngine.Audio;

namespace Package.Runtime
{
    public class AudioSystemSettings : IAudioSystemSettings
    {
        public AudioMixerGroup BaseGroup { get; }

        public AudioSystemSettings(AudioMixerGroup baseGroup)
        {
            BaseGroup = baseGroup;
        }
    }
}
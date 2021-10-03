using UnityEngine.Audio;

namespace Package.Runtime.Scripts
{
    public interface IAudioSystemSettings
    {
        AudioMixerGroup BaseGroup { get; }
    }
}
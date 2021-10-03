using System.Collections;
using Package.Runtime;
using Package.Runtime.Scripts;
using UnityEngine;
using UnityEngine.Audio;

public class TestDebugger : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup _baseGroup;
    [SerializeField] private AudioData _data1;
    [SerializeField] private AudioData _data2;
    [SerializeField] private string _audioTrack;
    
    private AudioSystem _system;
    
    private IEnumerator Start()
    {
        var settings = new AudioSystemSettings(_baseGroup);
        _system = new AudioSystem(settings);

        PlaySoundData(_data1);
        yield return new WaitForSeconds(2.0f);
        PlaySoundData(_data2);
    }

    private void PlaySoundData(AudioData data)
    {
        if (string.IsNullOrEmpty(_audioTrack))
        {
            _system.PlayOneShot(data);
        }
        else
        {
            _system.PlayInAudioTrack(data, _audioTrack);
        }
    }
}

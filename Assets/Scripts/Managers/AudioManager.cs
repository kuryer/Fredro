using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    [SerializeField] float minusVolumeST;
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] int audioManagerNumber;

    private void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;

            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = s.group;
        }
    }
    private void Start()
    {
        if (audioManagerNumber == 1)
        {
            Play("MenuMusic");
        }
        else if (audioManagerNumber == 2)
        {
            Play("Soundtrack");
        }
    }
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }
    public void Pause(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Stop();
    }
    public IEnumerator FadeOut(string name, float duration)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
        float time = 0f;
        float startVolume = s.volume;
        while (time < duration)
        {
            time += Time.deltaTime;
            s.volume = Mathf.Lerp(startVolume, 0f, time/duration);
            yield return null;
        }
        yield break;
    }

    public IEnumerator FadeIn(string name, float duration)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        float time = 0f;
        float startVolume = s.volume;
        while (time < duration)
        {
            time += Time.deltaTime;
            s.volume = Mathf.Lerp(0f, 1f, time / duration);
            yield return null;
        }
        yield break;
    }
}

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;
    [Range(.1f, 3f)]
    public float pitch;

    public bool loop;

    public AudioMixerGroup group;

    [HideInInspector]
    public AudioSource source;
}

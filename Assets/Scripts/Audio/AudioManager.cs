using System;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    // Start is called before the first frame update
    void Awake()
    {
        if (FindObjectsOfType<AudioManager>().Length > 1)
            Destroy(gameObject);
        else
            DontDestroyOnLoad(gameObject);

        InitSources();
    }


    public void PlaySound(string name)
    {
        if (!TryGetSound(name, out Sound sound)) return;

        RandomizePitch(sound);
        sound.source.Play();
    }

    public void StopSound(string name)
    {
        if (!TryGetSound(name, out Sound sound)) return;

        sound.source.Stop();
    }

    public bool IsPlayingSound(string name)
    {
        if (!TryGetSound(name, out Sound sound)) return false;

        return sound.source.isPlaying;
    }

    private bool TryGetSound(string name, out Sound sound)
    {
        sound = Array.Find(sounds, sound => sound.name == name);
        if (sound == null)
        {
            Debug.LogWarning($"Sound {name} was not found");
            return false;
        }
        return true;
    }

    private void RandomizePitch(Sound sound)
    {
        float basePitch = sound.basePitch;
        float randomPitchVariation = Random.Range(-sound.pitchVariation, sound.pitchVariation);
        sound.source.pitch = basePitch + randomPitchVariation;
    }
    private void InitSources()
    {
        foreach (var sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;

            sound.source.volume = sound.volume;
            sound.source.pitch = sound.basePitch;
            sound.source.loop = sound.isLoop;
        }
    }
}

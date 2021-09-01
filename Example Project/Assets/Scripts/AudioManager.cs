using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AudioManager : MonoBehaviour {

    [Header("Audio Files:")]
    [SerializeField]
    private List<Sound> sounds;

    #region Singelton
    public static AudioManager instance;

    private void Awake() {
        // Check if instance is already defined and if this gameObject is not the current instance.
        if (instance != null) {
            Debug.LogWarning("Multiple Instances of AudioManager found. Current instance was destroyed.");
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }
    #endregion

    private void Start() {
        Play("Theme");
    }

    /// <summary>
    /// Plays the song with the given name and prints a Warning if it wasn't found.
    /// </summary>
    /// <param name="name">Name of the Song.</param>
    public void Play(string name) {
        AudioSource source = GetSource(name);

        // Couldn't find source.
        if (source == default) {
            return;
        }

        source.Play();
    }

    /// <summary>
    /// Plays the song with the given name and prints a Warning if it wasn't found.
    /// </summary>
    /// <param name="name">Name of the Song.</param>
    /// <param name="startTime">Time we want to start playing the song at.</param>
    public void PlayAtTimeStamp(string name, float startTime) {
        AudioSource source = GetSource(name);

        // Couldn't find source.
        if (source == default) {
            return;
        }

        // Sets the start playback position to the given startTime in seconds.
        source.time = startTime;
        source.Play();
    }

    /// <summary>
    /// Gets the current playback position of the given song in seconds.
    /// </summary>
    /// <param name="name">Name of the Song.</param>
    /// <returns>Amount of seconds we are in the current Song loop.</returns>
    public float GetPlaybackPosition(string name) {
        AudioSource source = GetSource(name);

        // Couldn't find source.
        if (source == default) {
            return float.NaN;
        }

        return source.time;
    }

    /// <summary>
    /// Plays the song with the given name and prints a Warning if it wasn't found.
    /// </summary>
    /// <param name="name">Name of the Song.</param>
    /// <param name="position">Position we want to place our Sound at.</param>
    /// <param name="minDistance">Distance that sound will not get louder at.</param>
    /// <param name="maxDistance">Distance that sound will still be hearable at.</param>
    /// <param name="spread">Sets the spread angles of the sound in degrees.</param>
    /// <param name="spatialBlend">Defines how much the Audio Source is affected by 3D space. (0f = 2D, 1f = 3D)</param>
    /// <param name="dopplerLevel">Defines Doppler Scale for the Audio Source.</param>
    /// <param name="rolloffMode">Sets how the Volume will be lowered over distance.</param>
    public void PlayAt3DPosition(string name, Vector3 position, float minDistance, float maxDistance, float spread = 0f, float spatialBlend = 1f, float dopplerLevel = 1f, AudioRolloffMode rolloffMode = AudioRolloffMode.Linear) {
        AudioSource source = GetSource(name);

        // Couldn't find source.
        if (source == default) {
            return;
        }

        Set3DAudioOptions(source, minDistance, maxDistance, spread, spatialBlend, dopplerLevel, rolloffMode);

        // Set position of our AudioSource.
        source.transform.position = position;
        source.Play();
    }

    /// <summary>
    /// Plays the song with the given name and prints a Warning if it wasn't found.
    /// </summary>
    /// <param name="name">Name of the Song.</param>
    /// <param name="gameObject">GameObject we want to attach our Sound too.</param>
    /// <param name="minDistance">Distance that sound will not get louder at.</param>
    /// <param name="maxDistance">Distance that sound will still be hearable at.</param>
    /// <param name="spread">Sets the spread angles of the sound in degrees.</param>
    /// <param name="spatialBlend">Defines how much the Audio Source is affected by 3D space. (0f = 2D, 1f = 3D)</param>
    /// <param name="dopplerLevel">Defines Doppler Scale for the Audio Source.</param>
    /// <param name="rolloffMode">Sets how the Volume will be lowered over distance.</param>
    public void PlayAttachedToGameObject(string name, GameObject gameObject, float minDistance, float maxDistance, float spread = 0f, float spatialBlend = 1f, float dopplerLevel = 1f, AudioRolloffMode rolloffMode = AudioRolloffMode.Linear) {
        AudioSource source = GetSource(name);

        // Couldn't find source.
        if (source == default) {
            return;
        }

        Set3DAudioOptions(source, minDistance, maxDistance, spread, spatialBlend, dopplerLevel, rolloffMode);

        // Set parent of AudioSource to the given gameObject.
        source.transform.SetParent(gameObject.transform);
        source.Play();
    }

    /// <summary>
    /// Enables and sets the possible 3D Audio Options.
    /// </summary>
    /// <param name="source">Source of the AudioFile.</param>
    /// <param name="minDistance">Distance that sound will not get louder at.</param>
    /// <param name="maxDistance">Distance that sound will still be hearable at.</param>
    /// <param name="spread">Sets the spread angles of the sound in degrees.</param>
    /// <param name="spatialBlend">Defines how much the Audio Source is affected by 3D space. (0f = 2D, 1f = 3D)</param>
    /// <param name="dopplerLevel">Defines Doppler Scale for the Audio Source.</param>
    /// <param name="rolloffMode">Sets how the Volume will be lowered over distance.</param>
    private void Set3DAudioOptions(AudioSource source, float minDistance, float maxDistance, float spread, float spatialBlend, float dopplerLevel, AudioRolloffMode rolloffMode) {
        source.spatialize = true;
        source.spatialBlend = spatialBlend;
        source.dopplerLevel = dopplerLevel;
        source.spread = spread;
        source.rolloffMode = rolloffMode;
        source.minDistance = minDistance;
        source.maxDistance = maxDistance;
    }

    /// <summary>
    /// Plays the song with the given name and prints a Warning if it wasn't found after the given delay time.
    /// </summary>
    /// <param name="name">Name of the Song.</param>
    /// <param name="delay">Delay until Song is played.</param>
    public void PlayDelayed(string name, float delay) {
        AudioSource source = GetSource(name);

        // Couldn't find source.
        if (source == default) {
            return;
        }

        source.PlayDelayed(delay);
    }

    /// <summary>
    /// Plays the song with the given name once and prints a Warning if it wasn't found.
    /// </summary>
    /// <param name="name">Name of the Song.</param>
    public void PlayOneShot(string name) {
        AudioSource source = GetSource(name);

        // Couldn't find source.
        if (source == default) {
            return;
        }

        source.PlayOneShot(source.clip);
    }

    /// <summary>
    /// Plays the song with the given name and prints a Warning if it wasn't found after the given delay time.
    /// Additionally buffer time is added to the waitTime to prepare the playback and fetch it from media.
    /// </summary>
    /// <param name="name">Name of the Song.</param>
    /// <param name="time">Delay until Song is played.</param>
    public void PlayScheduled(string name, double time) {
        AudioSource source = GetSource(name);

        // Couldn't find source.
        if (source == default) {
            return;
        }

        source.PlayScheduled(time);
    }

    /// <summary>
    /// Stops the song with the given name and prints a Warning if it wasn't found.
    /// </summary>
    /// <param name="name">Name of the Song.</param>
    public void Stop(string name) {
        AudioSource source = GetSource(name);

        // Couldn't find source.
        if (source == default) {
            return;
        }

        source.Stop();
    }

    /// <summary>
    /// Mutes or Unmutes the song with the given name and prints a Warning if it wasn't found.
    /// </summary>
    /// <param name="name">Name of the Song.</param>
    public void ToggleMute(string name) {
        AudioSource source = GetSource(name);

        // Couldn't find source.
        if (source == default) {
            return;
        }

        source.mute = !source.mute;
    }
    
    /// <summary>
    /// Returns the progress of the song with the given name from 0 to 1 where 1 is fully completed.
    /// </summary>
    /// <param name="name">Name of the Song.</param>
    /// <returns>Progress of the given Song (0 to 1).</returns>
    public float Progress(string name) {
        float progress = 0f;
        AudioSource source = GetSource(name);
        
        // Couldn't find source.
        if (source == default || source.clip == null) {
            return progress;
        }

        progress = float.Parse(source.timesamples) / float.Parse(clip.samples);
        return progress;
    }

    /// <summary>
    /// Gets the corresponding Source to the song with the given name and prints a Warning if it wasn't found or if we found multiple.
    /// </summary>
    /// <param name="name">Name of the Song.</param>
    /// <returns>AudioSource of the given Song.</returns>
    public AudioSource GetSource(string name) {
        // Find Sound with the corresponding given Name
        List<Sound> s = sounds.Where(sound => string.Equals(sound.name, name)).ToList();
        // If we found no sound print a Warning in the Console.
        if (s.Count == 0) {
            Debug.LogWarning("Sound: " + name + " not found");
        }
        // If we found more than one sound print a Warning in the Console.
        else if (s.Count > 1) {
            Debug.LogWarning("Multiple Instances of Sound: " + name + " found");
        }

        // Return the first or default values source.
        return s.FirstOrDefault().source;
    }

    /// <summary>
    /// Changes the pitch over the given amount of time to the given endValue.
    /// </summary>
    /// <param name="name">Name of the Song.</param>
    /// <param name="endValue">Value we wan't to have at the end.</param>
    /// <param name="waitTime">Total time needed to reach the given endValue.</param>
    /// <param name="granularity">Amount of steps that will be taken to decrease to the endValue (Setting to high is not advised).</param>
    public void ChangePitch(string name, float endValue, float waitTime = 1f, float granularity = 5f) {
        AudioSource source = GetSource(name);

        // Couldn't find source or the pitch is already on the wanted endValue.
        if (source == default || source.pitch == endValue) {
            return;
        }
        
        // Calculate what we need to remove or add to the pitch to achieve the endValue.
        float difference = endValue - source.pitch;
        float stepValue = difference / granularity;
        float stepTime = waitTime / granulairty;

        StartCoroutine(PitchChanger(source, stepValue, stepTime, granularity));
    }

    /// <summary>
    /// Changes the Pitch of the given Song with a certain waitTime after each de -or increase.
    /// </summary>
    /// <param name="source">Source of the AudioFile.</param>
    /// <param name="stepValue">How much we want to de -or increase the value by each step .</param>
    /// <param name="stepTime">Delay we want to have after each de -or increase.</param>
    /// <param name="steps">Amount of steps that will be taken to decrease to the endValue.</param>
    private IEnumerator PitchChanger(AudioSource source, float stepValue, float stepTime, float steps) {
        // De -or increases the given pitch with the given amount of steps           
        for (; steps >= 0; steps--) {
            source.pitch += stepValue;
            yield return new WaitForSeconds(stepTime);
        }
    }

    /// <summary>
    /// Changes the volume over the given amount of time to the given endValue.
    /// </summary>
    /// <param name="name">Name of the Song.</param>
    /// <param name="endValue">Value we wan't to have at the end.</param>
    /// <param name="waitTime">Total time needed to reach the given endValue.</param>
    /// <param name="granularity">Amount of steps that will be taken to decrease to the endValue (Setting to high is not advised).</param>
    public void ChangeVolume(string name, float endValue, float waitTime = 1f, float granularity = 5f) {
        AudioSource source = GetSource(name);

        // Couldn't find source or the pitch is already on the wanted endValue.
        if (source == default || source.volume == endValue) {
            return;
        }
        
        // Calculate what we need to remove or add to the pitch to achieve the endValue.
        float difference = endValue - source.volume;
        float stepValue = difference / granularity;
        float stepTime = waitTime / granulairty;

        StartCoroutine(VolumeChanger(source, stepValue, stepTime, granularity));
    }

    /// <summary>
    /// Changes the Volume of the given Song with a certain waitTime after each de -or increase.
    /// </summary>
    /// <param name="source">Source of the AudioFile.</param>
    /// <param name="stepValue">How much we want to de -or increase the value by each step .</param>
    /// <param name="stepTime">Delay we want to have after each de -or increase.</param>
    /// <param name="steps">Amount of steps that will be taken to decrease to the endValue.</param>
    private IEnumerator VolumeChanger(AudioSource source, float stepValue, float stepTime, float steps) {
        // De -or increases the given pitch with the given amount of steps           
        for (; steps >= 0; steps--) {
            source.volume += stepValue;
            yield return new WaitForSeconds(stepTime);
        }
    }
}

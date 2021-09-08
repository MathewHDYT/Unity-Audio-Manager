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
    /// Adds the sound to the given list of possible playable options.
    /// </summary>
    /// <param name="name">Name the new sound should have.</param>
    /// <param name="path">Path to the clip we want to add to the new sound.</param>
    /// <param name="volume">Volume we want to add to the new sound.</param>
    /// <param name="pitch">Pitch we want to add to the new sound.</param>
    /// <param name="loop">Decides wheter we want to repeat the sound after completing it or not.</param>
    /// <param name="source">Source we want to add to the new sound.</param>
    public void AddSoundFromPath(string name, string path, float volume = 1f, float pitch = 1f, bool loop = false, AudioSource source = null) {
		// Load sound clip from the Resource folder on the given path.
		var clip = Resources.Load<AudioClip>(path);
		
		// Check if the clip couldn't be loaded correctly.
		if (clip == null) {
			Debug.LogWarning("Sound couldn't be added because path: " + path + " to the clip was wrong");
			return;
		}
		// Check if the list already contains a sound with the given name.
		else if(sounds.Any(s => s.name == name)) {
			Debug.LogWarning("There already exists a sound with the name: " + name);
			return;
		}

		// Check if a source was passed already or if we need to create a new one.
		if (source == null) {
			AudioSource source = gameObject.AddComponent<AudioSource>();
		}
		Sound sound = new Sound(name, clip, volume, pitch, loop, source);
		sounds.Add(sound);
	}

    /// <summary>
    /// Plays the sound with the given name.
    /// </summary>
    /// <param name="name">Name of the sound.</param>
    public void Play(string name) {
        AudioSource source = GetSource(name);

        // Couldn't find source.
        if (source == default) {
            return;
        }

        SetStartTime(source);
        source.Play();
    }

    /// <summary>
    /// Plays the sound with the given name.
    /// </summary>
    /// <param name="name">Name of the sound.</param>
    /// <param name="startTime">Time we want to start playing the sound at.</param>
    public void PlayAtTimeStamp(string name, float startTime) {
        AudioSource source = GetSource(name);

        // Couldn't find source.
        if (source == default) {
            return;
        }

        // Sets the start playback position to the given startTime in seconds.
        SetStartTime(source, startTime);
        source.Play();
    }

    /// <summary>
    /// Gets the current playback position of the given sound in seconds.
    /// </summary>
    /// <param name="name">Name of the sound.</param>
    /// <returns>Amount of seconds we are in the current sound loop.</returns>
    public float GetPlaybackPosition(string name) {
        AudioSource source = GetSource(name);

        // Couldn't find source.
        if (source == default) {
            return float.NaN;
        }

        return source.time;
    }

    /// <summary>
    /// Plays the sound with the given name.
    /// </summary>
    /// <param name="name">Name of the sound.</param>
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
        SetStartTime(source);
        source.Play();
    }

    /// <summary>
    /// Plays the sound with the given name.
    /// </summary>
    /// <param name="name">Name of the sound.</param>
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
        SetStartTime(source);
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
    /// Plays the sound with the given name after the given delay time.
    /// </summary>
    /// <param name="name">Name of the sound.</param>
    /// <param name="delay">Delay until sound is played.</param>
    public void PlayDelayed(string name, float delay) {
        AudioSource source = GetSource(name);

        // Couldn't find source.
        if (source == default) {
            return;
        }

        SetStartTime(source);
        source.PlayDelayed(delay);
    }

    /// <summary>
    /// Plays the sound with the given name once.
    /// Multiple instances of the same sound can be run with this function.
    /// </summary>
    /// <param name="name">Name of the sound.</param>
    public void PlayOneShot(string name) {
        AudioSource source = GetSource(name);

        // Couldn't find source.
        if (source == default) {
            return;
        }

        SetStartTime(source);
        source.PlayOneShot(source.clip);
    }

    /// <summary>
    /// Plays the sound with the given name after the given delay time.
    /// Additionally buffer time is added to the waitTime to prepare the playback and fetch it from media.
    /// </summary>
    /// <param name="name">Name of the sound.</param>
    /// <param name="time">Delay until sound is played.</param>
    public void PlayScheduled(string name, double time) {
        AudioSource source = GetSource(name);

        // Couldn't find source.
        if (source == default) {
            return;
        }

        SetStartTime(source);
        source.PlayScheduled(time);
    }

    /// <summary>
    /// Stops the sound with the given name.
    /// </summary>
    /// <param name="name">Name of the sound.</param>
    public void Stop(string name) {
        AudioSource source = GetSource(name);

        // Couldn't find source.
        if (source == default) {
            return;
        }

        source.Stop();
    }

    /// <summary>
    /// Mutes or Unmutes the sound with the given name.
    /// </summary>
    /// <param name="name">Name of the sound.</param>
    public void ToggleMute(string name) {
        AudioSource source = GetSource(name);

        // Couldn't find source.
        if (source == default) {
            return;
        }

        source.mute = !source.mute;
    }
    
    /// <summary>
    /// Pauses or Unpauses the sound with the given name.
    /// </summary>
    /// <param name="name">Name of the sound.</param>
    public void TogglePause(string name) {
        AudioSource source = GetSource(name);

        // Couldn't find source.
        if (source == default) {
            return;
        }

        // Check if the sound is playing right now.
        if (source.isPlaying()) {
            source.Pause();
        }
        else {
            source.Unpause();
        }
    }
    
    /// <summary>
    /// Returns the progress of the sound with the given name from 0 to 1 where 1 is fully completed.
    /// </summary>
    /// <param name="name">Name of the sound.</param>
    /// <returns>Progress of the given sound (0 to 1).</returns>
    public float GetProgress(string name) {
        float progress = 0f;
        AudioSource source = GetSource(name);
        
        // Couldn't find source.
        if (source == default || source.clip == null) {
            return progress;
        }

        progress = (float)source.timeSamples / (float)source.clip.samples;
        return progress;
    }

    /// <summary>
    /// Gets the corresponding Source to the sound with the given name or if we found multiple.
    /// </summary>
    /// <param name="name">Name of the sound.</param>
    /// <returns>AudioSource of the given sound.</returns>
    public AudioSource GetSource(string name) {
        AudioSource source = default;

        // Find Sound with the corresponding given Name
        List<Sound> s = sounds.Where(sound => string.Equals(sound.name, name)).ToList();
        // If we found no sound print a Warning in the Console and return default value
        if (s.Count == 0) {
            Debug.LogWarning("Sound: " + name + " not found");
            return source;
        }
        // If we found more than one sound print a Warning in the Console and return default value
        else if (s.Count > 1) {
            Debug.LogWarning("Multiple Instances of Sound: " + name + " found");
        }

        source = s.FirstOrDefault().source;
        return source;
    }

    /// <summary>
    /// Changes the pitch over the given amount of time to the given endValue.
    /// </summary>
    /// <param name="name">Name of the sound.</param>
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
        float stepTime = waitTime / granularity;

        StartCoroutine(PitchChanger(source, stepValue, stepTime, granularity, endValue));
    }

    /// <summary>
    /// Changes the Pitch of the given sound with a certain waitTime after each de -or increase.
    /// </summary>
    /// <param name="source">Source of the AudioFile.</param>
    /// <param name="stepValue">How much we want to de -or increase the value by each step .</param>
    /// <param name="stepTime">Delay we want to have after each de -or increase.</param>
    /// <param name="steps">Amount of steps that will be taken to decrease to the endValue.</param>
    /// <param name="endValue">Value we wan't to have at the end.</param>
    private IEnumerator PitchChanger(AudioSource source, float stepValue, float stepTime, float steps, float endValue) {
        // De -or increases the given pitch with the given amount of steps.
        for (; steps > 0; steps--) {
            source.pitch += stepValue;
            yield return new WaitForSeconds(stepTime);
        }
        // Correct for float rounding errors.
        source.volume = endValue;
    }

    /// <summary>
    /// Changes the volume over the given amount of time to the given endValue.
    /// </summary>
    /// <param name="name">Name of the sound.</param>
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
        float stepTime = waitTime / granularity;

        StartCoroutine(VolumeChanger(source, stepValue, stepTime, granularity, endValue));
    }

    /// <summary>
    /// Changes the Volume of the given sound with a certain waitTime after each de -or increase.
    /// </summary>
    /// <param name="source">Source of the AudioFile.</param>
    /// <param name="stepValue">How much we want to de -or increase the value by each step .</param>
    /// <param name="stepTime">Delay we want to have after each de -or increase.</param>
    /// <param name="steps">Amount of steps that will be taken to decrease to the endValue.</param>
    /// <param name="endValue">Value we wan't to have at the end.</param>
    private IEnumerator VolumeChanger(AudioSource source, float stepValue, float stepTime, float steps, float endValue) {
        // De -or increases the given pitch with the given amount of steps.
        for (; steps > 0; steps--) {
            source.volume += stepValue;
            yield return new WaitForSeconds(stepTime);
        }
        // Correct for float rounding errors.
        source.volume = endValue;
    }

    /// <summary>
    /// Sets the startTime of the song. Needs to be done all the time before starting a sound to ensure we start it at the beginning
    /// and not the time set by the PlayAtTimeStamp method.
    /// </summary>
    /// <param name="source">Source of the AudioFile.</param>
    /// <param name="startTime">Moment in the sound we want to start playing at.</param>
    private void SetStartTime(AudioSource source, float startTime = 0f) {
        source.time = startTime;
    }
}

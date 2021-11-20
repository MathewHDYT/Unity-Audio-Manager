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

    public enum AudioError {
        OK,
        DOES_NOT_EXIST,
        FOUND_MULTIPLE,
        ALREADY_EXISTS,
        INVALID_PATH,
        SAME_AS_CURRENT,
        TOO_SMALL
    }

    private void Start() {
        Play("Theme");
    }
	
    /// <summary>
    /// Adds the given sound to the list of possible playable sounds.
    /// </summary>
    /// <param name="name">Name the new sound should have.</param>
    /// <param name="path">Path to the clip we want to add to the new sound in the Resource folder.</param>
    /// <param name="volume">Volume we want the new sound to have.</param>
    /// <param name="pitch">Pitch we want the new sound to have.</param>
    /// <param name="loop">Defines wheter we want to repeat the new sound after completing it or not.</param>
    /// <param name="source">Source we want to add to the new sound.</param>
    /// <returns>AudioError, showing wheter and how adding a sound from the given path with the given settings failed.</returns>
    public AudioError AddSoundFromPath(string name, string path, float volume = 1f, float pitch = 1f, bool loop = false, AudioSource source = null) {
        AudioError err = AudioError.OK;
        // Load sound clip from the Resource folder on the given path.
        var clip = Resources.Load<AudioClip>(path);
		
        // Check if the clip couldn't be loaded correctly.
        if (clip == null) {
            err = AudioError.INVALID_PATH;
            return err;
        }
        // Check if the list already contains a sound with the given name.
        else if(sounds.Any(s => s.name == name)) {
            err = AudioError.ALREADY_EXISTS;
            return err;
        }

        // Check if a source was passed already or if we need to create a new one.
        if (source == null) {
            source = gameObject.AddComponent<AudioSource>();
        }
        Sound sound = new Sound(name, clip, volume, pitch, loop, source);
        sounds.Add(sound);
        return err;
    }

    /// <summary>
    /// Plays the sound with the given name.
    /// </summary>
    /// <param name="name">Name of the sound.</param>
    /// <returns>AudioError, showing wheter and how playing the sound failed.</returns>
    public AudioError Play(string name) {
        AudioError err = TryGetSource(name, out AudioSource source);

        // Couldn't find source.
        if (err != AudioError.OK) {
            return err;
        }

        SetStartTime(source);
        source.Play();
        return err;
    }

    /// <summary>
    /// Plays the sound with the given name.
    /// </summary>
    /// <param name="name">Name of the sound.</param>
    /// <param name="startTime">Time we want to start playing the sound at.</param>
    /// <returns>AudioError, showing wheter and how playing the sound from the given startTime failed.</returns>
    public AudioError PlayAtTimeStamp(string name, float startTime) {
        AudioError err = TryGetSource(name, out AudioSource source);

        // Couldn't find source.
        if (err != AudioError.OK) {
            return err;
        }

        // Sets the start playback position to the given startTime in seconds.
        SetStartTime(source, startTime);
        source.Play();
        return err;
    }

    /// <summary>
    /// Gets the current playback position of the given sound in seconds.
    /// </summary>
    /// <param name="name">Name of the sound.</param>
    /// <returns>
    /// ValueDataError, where the value (gettable with Value), is the current playback position of the given sound in seconds
    /// and where the error (gettable with Error) is an integer representing the AudioError Enum, 
    /// showing wheter and how getting the current playback position of the sound failed.
    /// </returns>
    public ValueDataError<float> GetPlaybackPosition(string name) {
        AudioError err = TryGetSource(name, out AudioSource source);
        ValueDataError<float> valueDataError = new ValueDataError<float>(float.NaN, (int)err);

        // Couldn't find source.
        if (err != AudioError.OK) {
            return valueDataError;
        }

        valueDataError.Value = source.time;
        return valueDataError;
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
    /// <returns>AudioError, showing wheter and how playing the sound at the given position failed.</returns>
    public AudioError PlayAt3DPosition(string name, Vector3 position, float minDistance, float maxDistance, float spread = 0f, float spatialBlend = 1f, float dopplerLevel = 1f, AudioRolloffMode rolloffMode = AudioRolloffMode.Linear) {
        AudioError err = TryGetSource(name, out AudioSource source);

        // Couldn't find source.
        if (err != AudioError.OK) {
            return err;
        }

        Set3DAudioOptions(source, minDistance, maxDistance, spread, spatialBlend, dopplerLevel, rolloffMode);

        // Set position of our AudioSource.
        source.transform.position = position;
        SetStartTime(source);
        source.Play();
        return err;
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
    /// <returns>AudioError, showing wheter and how playing the sound attached to the given gameobject failed.</returns>
    public AudioError PlayAttachedToGameObject(string name, GameObject gameObject, float minDistance, float maxDistance, float spread = 0f, float spatialBlend = 1f, float dopplerLevel = 1f, AudioRolloffMode rolloffMode = AudioRolloffMode.Linear) {
        AudioError err = TryGetSource(name, out AudioSource source);

        // Couldn't find source.
        if (err != AudioError.OK) {
            return err;
        }

        Set3DAudioOptions(source, minDistance, maxDistance, spread, spatialBlend, dopplerLevel, rolloffMode);

        // Set parent of AudioSource to the given gameObject.
        source.transform.SetParent(gameObject.transform);
        SetStartTime(source);
        source.Play();
        return err;
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
    /// <returns>AudioError, showing wheter and how playing the sound after the given amount of time failed.</returns>
    public AudioError PlayDelayed(string name, float delay) {
        AudioError err = TryGetSource(name, out AudioSource source);

        // Couldn't find source.
        if (err != AudioError.OK) {
            return err;
        }

        SetStartTime(source);
        source.PlayDelayed(delay);
        return err;
    }

    /// <summary>
    /// Plays the sound with the given name once.
    /// Multiple instances of the same sound can be run with this function.
    /// </summary>
    /// <param name="name">Name of the sound.</param>
    /// <returns>AudioError, showing wheter and how playing the sound once failed.</returns>
    public AudioError PlayOneShot(string name) {
        AudioError err = TryGetSource(name, out AudioSource source);

        // Couldn't find source.
        if (err != AudioError.OK) {
            return err;
        }

        SetStartTime(source);
        source.PlayOneShot(source.clip);
        return err;
    }

    /// <summary>
    /// Plays the sound with the given name after the given delay time.
    /// Additionally buffer time is added to the waitTime to prepare the playback and fetch it from media.
    /// </summary>
    /// <param name="name">Name of the sound.</param>
    /// <param name="time">Delay until sound is played.</param>
    /// <returns>AudioError, showing wheter and how playing the sound after the given amount of time failed.</returns>
    public AudioError PlayScheduled(string name, double time) {
        AudioError err = TryGetSource(name, out AudioSource source);

        // Couldn't find source.
        if (err != AudioError.OK) {
            return err;
        }

        SetStartTime(source);
        source.PlayScheduled(time);
        return err;
    }

    /// <summary>
    /// Stops the sound with the given name.
    /// </summary>
    /// <param name="name">Name of the sound.</param>
    /// <returns>AudioError, showing wheter and how stopping the sound failed.</returns>
    public AudioError Stop(string name) {
        AudioError err = TryGetSource(name, out AudioSource source);

        // Couldn't find source.
        if (err != AudioError.OK) {
            return err;
        }

        SetStartTime(source);
        source.Stop();
        return err;
    }

    /// <summary>
    /// Mutes or Unmutes the sound with the given name.
    /// </summary>
    /// <param name="name">Name of the sound.</param>
    /// <returns>AudioError, showing wheter and how muting or unmuting the sound failed.</returns>
    public AudioError ToggleMute(string name) {
        AudioError err = TryGetSource(name, out AudioSource source);

        // Couldn't find source.
        if (err != AudioError.OK) {
            return err;
        }

        source.mute = !source.mute;
        return err;
    }

    /// <summary>
    /// Pauses or Unpauses the sound with the given name.
    /// </summary>
    /// <param name="name">Name of the sound.</param>
    /// <returns>AudioError, showing wheter and how pausing or unpausing the sound failed.</returns>
    public AudioError TogglePause(string name) {
        AudioError err = TryGetSource(name, out AudioSource source);

        // Couldn't find source.
        if (err != AudioError.OK) {
            return err;
        }

        // Check if the sound is playing right now.
        if (source.isPlaying) {
            source.Pause();
        }
        else {
            source.UnPause();
        }
        return err;
    }

    /// <summary>
    /// Returns the progress of the sound with the given name from 0 to 1 where 1 is fully completed.
    /// </summary>
    /// <param name="name">Name of the sound.</param>
    /// <returns>
    /// ValueDataError, where the value (gettable with Value), is the progress of the given sound, which is a float from 0 to 1
    /// and where the error (gettable with Error) is an integer representing the AudioError Enum, 
    /// showing wheter and how getting the current playback position of the sound failed.
    /// </returns>
    public ValueDataError<float> GetProgress(string name) {
        AudioError err = TryGetSource(name, out AudioSource source);
        ValueDataError<float> valueDataError = new ValueDataError<float>(float.NaN, (int)err);

        // Couldn't find source.
        if (err != AudioError.OK || source.clip == null) {
            return valueDataError;
        }

        valueDataError.Value = (float)source.timeSamples / (float)source.clip.samples;
        return valueDataError;
    }

    /// <summary>
    /// Gets the corresponding source to the sound with the given name or if we found multiple.
    /// </summary>
    /// <param name="name">Name of the sound.</param>
    /// <param name="source">Variale source should be copied into.</param>
    /// <returns>AudioError, showing wheter and how getting the source of the given sound failed.</returns>
    public AudioError TryGetSource(string name, out AudioSource source) {
        AudioError err = AudioError.OK;
        source = default;

        // Find Sound with the corresponding given Name
        List<Sound> s = sounds.Where(sound => string.Equals(sound.name, name)).ToList();
        // If we found no sound print a Warning in the Console and return default value
        if (s.Count == 0) {
            err = AudioError.DOES_NOT_EXIST;
            return err;
        }
        // If we found more than one sound print a Warning in the Console and return default value
        else if (s.Count > 1) {
            err = AudioError.FOUND_MULTIPLE;
        }

        source = s.FirstOrDefault().source;
        return err;
    }

    /// <summary>
    /// Changes the pitch over the given amount of time to the given endValue.
    /// </summary>
    /// <param name="name">Name of the sound.</param>
    /// <param name="endValue">Value we wan't to have at the end.</param>
    /// <param name="waitTime">Total time needed to reach the given endValue.</param>
    /// <param name="granularity">Amount of steps that will be taken to decrease to the endValue (Setting to high is not advised).</param>
    /// <returns>AudioError, showing wheter and how changing the pitch of the given sound failed.</returns>
    public AudioError ChangePitch(string name, float endValue, float waitTime = 1f, float granularity = 5f) {
        AudioError err = TryGetSource(name, out AudioSource source);

        // Couldn't find source.
        if (err != AudioError.OK) {
            return err;
        }
        else if (source.pitch == endValue) {
            err = AudioError.SAME_AS_CURRENT;
            return err;
        }
        else if (granularity < 1f) {
            err = AudioError.TOO_SMALL;
            return err;
        }

        // Calculate what we need to remove or add to the pitch to achieve the endValue.
        float difference = endValue - source.pitch;
        float stepValue = difference / granularity;
        float stepTime = waitTime / granularity;

        StartCoroutine(PitchChanger(source, stepValue, stepTime, granularity, endValue));
        return err;
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
    /// <returns>AudioError, showing wheter and how changing the volume of the given sound failed.</returns>
    public AudioError ChangeVolume(string name, float endValue, float waitTime = 1f, float granularity = 5f) {
        AudioError err = TryGetSource(name, out AudioSource source);

        // Couldn't find source.
        if (err != AudioError.OK) {
            return err;
        }
        else if (source.pitch == endValue) {
            err = AudioError.SAME_AS_CURRENT;
            return err;
        }
        else if (granularity < 1f) {
            err = AudioError.TOO_SMALL;
            return err;
        }

        // Calculate what we need to remove or add to the pitch to achieve the endValue.
        float difference = endValue - source.volume;
        float stepValue = difference / granularity;
        float stepTime = waitTime / granularity;

        StartCoroutine(VolumeChanger(source, stepValue, stepTime, granularity, endValue));
        return err;
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

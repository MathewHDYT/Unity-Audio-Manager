using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour {
    [Header("Audio Files:")]
    [SerializeField]
    private Sound[] initalSounds;

    private Dictionary<string, AudioSource> soundDictionary = new Dictionary<string, AudioSource>();

    #region Singelton
    public static AudioManager instance;

    private void Awake() {
        // Check if instance is already defined and if this gameObject is not the current instance.
        if (instance) {
            Debug.LogWarning("Multiple Instances of AudioManager found. Already loaded instance was destroyed.");
            Destroy(instance.gameObject);
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        SetupSounds();
    }
    #endregion

    public enum AudioError {
        OK,
        DOES_NOT_EXIST,
        FOUND_MULTIPLE,
        ALREADY_EXISTS,
        INVALID_PATH,
        SAME_AS_CURRENT,
        TOO_SMALL,
        NOT_EXPOSED,
        MISSING_SOURCE,
        MISSING_MIXER_GROUP
    }

    /// <summary>
    /// Adds given sound with the given settings to the possible playable sounds,
    /// creates a new AudioSource object if not already done so with the given settings
    /// and appends it to the GameObject the AudioManager resides on as a new Component.
    /// </summary>
    /// <param name="name">Name the new sound should have.</param>
    /// <param name="path">Path to the clip we want to add to the new sound in the Resource folder.</param>
    /// <param name="volume">Volume we want the new sound to have.</param>
    /// <param name="pitch">Pitch we want the new sound to have.</param>
    /// <param name="loop">Defines wheter we want to repeat the new sound after completing it or not.</param>
    /// <param name="source">Source we want to add to the new sound.</param>
    /// <param name="mixerGroup">Mixer group the sound is influenced by.</param>
    /// <returns>AudioError, showing wheter and how adding a sound from the given path with the given settings failed.</returns>
    public AudioError AddSoundFromPath(string name, string path, float volume = 1f, float pitch = 1f, bool loop = false, AudioSource source = null, AudioMixerGroup mixerGroup = null) {
        AudioError error = AudioError.OK;
        // Load sound clip from the Resource folder on the given path.
        var clip = Resources.Load<AudioClip>(path);
		
        // Check if the clip couldn't be loaded correctly.
        if (!clip) {
            error = AudioError.INVALID_PATH;
            return error;
        }

        var sound = new Sound(name, clip, volume, pitch, loop, source, mixerGroup);
        error = AddSound(sound);

        // Check if the clip could be added correctly.
        if (error != AudioError.OK) {
            return error;
        }

        InstantiateSource(sound);
        return error;
    }

    /// <summary>
    /// Appends the given sounds AudioSource components to the dictionary.
    /// </summary>
    /// <param name="sounds">Sounds we want to append to the dictionary.</param>
    /// <returns>AudioError, showing wheter and how adding a sound failed.</returns>
    public AudioError AddSound(Sound sound) {
        AudioError error = AudioError.OK;
        // Ensure there is not already a sound with the given name in our dictionary.
        if (soundDictionary.ContainsKey(sound.name)) {
            error = AudioError.ALREADY_EXISTS;
            return error;
        }
        soundDictionary.Add(sound.name, sound.source);
        return error;
    }

    /// <summary>
    /// Plays the sound with the given name.
    /// </summary>
    /// <param name="name">Name of the sound.</param>
    /// <returns>AudioError, showing wheter and how playing the sound failed.</returns>
    public AudioError Play(string name) {
        AudioError error = TryGetSource(name, out AudioSource source);

        // Couldn't find source.
        if (error != AudioError.OK) {
            return error;
        }

        SetStartTime(source);
        source.Play();
        return error;
    }

    /// <summary>
    /// Plays the sound with the given name.
    /// </summary>
    /// <param name="name">Name of the sound.</param>
    /// <param name="startTime">Time we want to start playing the sound at.</param>
    /// <returns>AudioError, showing wheter and how playing the sound from the given startTime failed.</returns>
    public AudioError PlayAtTimeStamp(string name, float startTime) {
        AudioError error = TryGetSource(name, out AudioSource source);

        // Couldn't find source.
        if (error != AudioError.OK) {
            return error;
        }

        // Sets the start playback position to the given startTime in seconds.
        SetStartTime(source, startTime);
        source.Play();
        return error;
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
        AudioError error = TryGetSource(name, out AudioSource source);
        ValueDataError<float> valueDataError = new ValueDataError<float>(float.NaN, (int)error);

        // Couldn't find source.
        if (error != AudioError.OK) {
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
        AudioError error = TryGetSource(name, out AudioSource source);

        // Couldn't find source.
        if (error != AudioError.OK) {
            return error;
        }

        Set3DAudioOptions(source, minDistance, maxDistance, spread, spatialBlend, dopplerLevel, rolloffMode);

        // Set position of our AudioSource.
        source.transform.position = position;
        SetStartTime(source);
        source.Play();
        return error;
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
        AudioError error = TryGetSource(name, out AudioSource source);

        // Couldn't find source.
        if (error != AudioError.OK) {
            return error;
        }

        Set3DAudioOptions(source, minDistance, maxDistance, spread, spatialBlend, dopplerLevel, rolloffMode);

        // Set parent of AudioSource to the given gameObject.
        source.transform.SetParent(gameObject.transform);
        SetStartTime(source);
        source.Play();
        return error;
    }

    /// <summary>
    /// Plays the sound with the given name after the given delay time.
    /// </summary>
    /// <param name="name">Name of the sound.</param>
    /// <param name="delay">Delay until sound is played.</param>
    /// <returns>AudioError, showing wheter and how playing the sound after the given amount of time failed.</returns>
    public AudioError PlayDelayed(string name, float delay) {
        AudioError error = TryGetSource(name, out AudioSource source);

        // Couldn't find source.
        if (error != AudioError.OK) {
            return error;
        }

        SetStartTime(source);
        source.PlayDelayed(delay);
        return error;
    }

    /// <summary>
    /// Plays the sound with the given name once.
    /// Multiple instances of the same sound can be run with this function.
    /// </summary>
    /// <param name="name">Name of the sound.</param>
    /// <returns>AudioError, showing wheter and how playing the sound once failed.</returns>
    public AudioError PlayOneShot(string name) {
        AudioError error = TryGetSource(name, out AudioSource source);

        // Couldn't find source.
        if (error != AudioError.OK) {
            return error;
        }

        SetStartTime(source);
        source.PlayOneShot(source.clip);
        return error;
    }

    /// <summary>
    /// Plays the sound with the given name after the given delay time.
    /// Additionally buffer time is added to the waitTime to prepare the playback and fetch it from media.
    /// </summary>
    /// <param name="name">Name of the sound.</param>
    /// <param name="time">Delay until sound is played.</param>
    /// <returns>AudioError, showing wheter and how playing the sound after the given amount of time failed.</returns>
    public AudioError PlayScheduled(string name, double time) {
        AudioError error = TryGetSource(name, out AudioSource source);

        // Couldn't find source.
        if (error != AudioError.OK) {
            return error;
        }

        SetStartTime(source);
        source.PlayScheduled(time);
        return error;
    }

    /// <summary>
    /// Stops the sound with the given name.
    /// </summary>
    /// <param name="name">Name of the sound.</param>
    /// <returns>AudioError, showing wheter and how stopping the sound failed.</returns>
    public AudioError Stop(string name) {
        AudioError error = TryGetSource(name, out AudioSource source);

        // Couldn't find source.
        if (error != AudioError.OK) {
            return error;
        }

        SetStartTime(source);
        source.Stop();
        return error;
    }

    /// <summary>
    /// Mutes or Unmutes the sound with the given name.
    /// </summary>
    /// <param name="name">Name of the sound.</param>
    /// <returns>AudioError, showing wheter and how muting or unmuting the sound failed.</returns>
    public AudioError ToggleMute(string name) {
        AudioError error = TryGetSource(name, out AudioSource source);

        // Couldn't find source.
        if (error != AudioError.OK) {
            return error;
        }

        source.mute = !source.mute;
        return error;
    }

    /// <summary>
    /// Pauses or Unpauses the sound with the given name.
    /// </summary>
    /// <param name="name">Name of the sound.</param>
    /// <returns>AudioError, showing wheter and how pausing or unpausing the sound failed.</returns>
    public AudioError TogglePause(string name) {
        AudioError error = TryGetSource(name, out AudioSource source);

        // Couldn't find source.
        if (error != AudioError.OK) {
            return error;
        }

        // Check if the sound is playing right now.
        if (source.isPlaying) {
            source.Pause();
        }
        else {
            source.UnPause();
        }
        return error;
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
        AudioError error = TryGetSource(name, out AudioSource source);
        ValueDataError<float> valueDataError = new ValueDataError<float>(float.NaN, (int)error);

        // Couldn't find source.
        if (error != AudioError.OK || !source.clip) {
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
        AudioError error = AudioError.OK;

        // Check if the given sound name is in our soundDictionary.
        if (!soundDictionary.TryGetValue(name, out source)) {
            error = AudioError.DOES_NOT_EXIST;
        }
        // Check if the source is set.
        else if (!source) {
            error = AudioError.MISSING_SOURCE;
        }
        return error;
    }

    /// <summary>
    /// Changes the pitch of the sound with the given name over the given amount of time to the given endValue.
    /// </summary>
    /// <param name="name">Name of the sound.</param>
    /// <param name="endValue">Value we wan't to have at the end.</param>
    /// <param name="waitTime">Total time needed to reach the given endValue.</param>
    /// <param name="granularity">Amount of steps that will be taken to decrease to the endValue (Setting to high is not advised).</param>
    /// <returns>AudioError, showing wheter and how changing the pitch of the given sound failed.</returns>
    public AudioError LerpPitch(string name, float endValue, float waitTime = 1f, float granularity = 5f) {
        AudioError error = TryGetSource(name, out AudioSource source);

        // Couldn't find source.
        if (error != AudioError.OK) {
            return error;
        }
        else if (source.pitch == endValue) {
            error = AudioError.SAME_AS_CURRENT;
            return error;
        }
        else if (granularity < 1f) {
            error = AudioError.TOO_SMALL;
            return error;
        }

        // Calculate what we need to remove or add to the pitch to achieve the endValue.
        float difference = endValue - source.pitch;
        float stepValue = difference / granularity;
        float stepTime = waitTime / granularity;

        StartCoroutine(PitchChanger(source, stepValue, stepTime, granularity));
        return error;
    }

    /// <summary>
    /// Changes the volume of the sound with the given name over the given amount of time to the given endValue.
    /// </summary>
    /// <param name="name">Name of the sound.</param>
    /// <param name="endValue">Value we wan't to have at the end.</param>
    /// <param name="waitTime">Total time needed to reach the given endValue.</param>
    /// <param name="granularity">Amount of steps that will be taken to decrease to the endValue (Setting to high is not advised).</param>
    /// <returns>AudioError, showing wheter and how changing the volume of the given sound failed.</returns>
    public AudioError LerpVolume(string name, float endValue, float waitTime = 1f, float granularity = 5f) {
        AudioError error = TryGetSource(name, out AudioSource source);

        // Couldn't find source.
        if (error != AudioError.OK) {
            return error;
        }
        else if (source.volume == endValue) {
            error = AudioError.SAME_AS_CURRENT;
            return error;
        }
        else if (granularity < 1f) {
            error = AudioError.TOO_SMALL;
            return error;
        }

        // Calculate what we need to remove or add to the pitch to achieve the endValue.
        float difference = endValue - source.volume;
        float stepValue = difference / granularity;
        float stepTime = waitTime / granularity;

        StartCoroutine(VolumeChanger(source, stepValue, stepTime, granularity));
        return error;
    }

    /// <summary>
    /// Changes the value of the given exposed parameter for the complete AudioMixerGroup of the given sound to the given newValue.
    /// </summary>
    /// <param name="name">Name of the sound.</param>
    /// <param name="exposedParameterName">Name of the exposed parameter we want to change.</param>
    /// <param name="newValue">Value we want to set the exposed parameter to.</param>
    /// <returns>AudioError, showing wheter and how changing the given exposed parameter for the complete AudioMixerGroup of the given sound failed.</returns>
    public AudioError ChangeGroupValue(string name, string exposedParameterName, float newValue) {
        AudioError error = TryGetSource(name, out AudioSource source);

        // Couldn't find source.
        if (error != AudioError.OK) {
            return error;
        }
        // Check if the outputAudioGroup value is not null.
        else if (!source.outputAudioMixerGroup) {
            error = AudioError.MISSING_MIXER_GROUP;
            return error;
        }
        // Check if the AudioMixer parameter is exposed.
        else if (!source.outputAudioMixerGroup.audioMixer.SetFloat(exposedParameterName, newValue)) {
            error = AudioError.NOT_EXPOSED;
        }
        return error;
    }

    /// <summary>
    /// Gets the value of the given exposed parameter for the complete AudioMixerGroup of the given sound to the given newValue.
    /// </summary>
    /// <param name="name">Name of the sound.</param>
    /// <param name="exposedParameterName">Name of the exposed parameter we want to get.</param>
    /// <returns>
    /// ValueDataError, where the value (gettable with Value), is the current value of the given exposed parameter
    /// and where the error (gettable with Error) is an integer representing the AudioError Enum, 
    /// showing wheter and how getting the current exposed parameter value failed.
    /// </returns>
    public ValueDataError<float> GetGroupValue(string name, string exposedParameterName) {
        AudioError error = TryGetSource(name, out AudioSource source);
        float currentValue = float.NaN;
        ValueDataError<float> valueDataError = new ValueDataError<float>(currentValue, (int)error);

        // Couldn't find source.
        if (error != AudioError.OK) {
            return valueDataError;
        }
        // Check if the outputAudioGroup value is not null.
        else if (!source.outputAudioMixerGroup) {
            valueDataError.Error = (int)AudioError.MISSING_MIXER_GROUP;
            return valueDataError;
        }
        // Check if the AudioMixer parameter is exposed.
        else if (!source.outputAudioMixerGroup.audioMixer.GetFloat(exposedParameterName, out currentValue)) {
            valueDataError.Error = (int)AudioError.NOT_EXPOSED;
        }
        valueDataError.Value = currentValue;
        return valueDataError;
    }

    /// <summary>
    /// Resets the value of the given exposed parameter for the complete AudioMixerGroup of the given sound to the default value.
    /// </summary>
    /// <param name="name">Name of the sound.</param>
    /// <param name="exposedParameterName">Name of the exposed parameter we want to reset.</param>
    /// <returns>AudioError, showing wheter and how reseting the given exposed parameter for the complete AudioMixerGroup of the given sound failed.</returns>
    public AudioError ResetGroupValue(string name, string exposedParameterName) {
        AudioError error = TryGetSource(name, out AudioSource source);

        // Couldn't find source.
        if (error != AudioError.OK) {
            return error;
        }
        // Check if the outputAudioGroup value is not null.
        else if (!source.outputAudioMixerGroup) {
            error = AudioError.MISSING_MIXER_GROUP;
            return error;
        }
        // Check if the AudioMixer parameter is exposed.
        else if (!source.outputAudioMixerGroup.audioMixer.ClearFloat(exposedParameterName)) {
            error = AudioError.NOT_EXPOSED;
        }
        return error;
    }

    /// <summary>
    /// Changes the value of the given exposed parameter for the complete AudioMixerGroup of the given sound over the given amount of time to the given endValue.
    /// </summary>
    /// <param name="name">Name of the sound.</param>
    /// <param name="exposedParameterName">Name of the exposed parameter we want to change.</param>
    /// <param name="endValue">Value we wan't to have at the end.</param>
    /// <param name="waitTime">Total time needed to reach the given endValue.</param>
    /// <param name="granularity">Amount of steps that will be taken to decrease to the endValue (Setting to high is not advised).</param>
    /// <returns>AudioError, showing wheter and how changing the given exposed parameter for the complete AudioMixerGroup of the given sound failed.</returns>
    public AudioError LerpGroupValue(string name, string exposedParameterName, float endValue, float waitTime = 1f, float granularity = 5f) {
        AudioError error = TryGetSource(name, out AudioSource source);
        float startValue = float.NaN;

        // Couldn't find source.
        if (error != AudioError.OK) {
            return error;
        }
        // Check if the outputAudioGroup value is not null.
        else if (!source.outputAudioMixerGroup) {
            error = AudioError.MISSING_MIXER_GROUP;
            return error;
        }
        // Check if the AudioMixer parameter is exposed.
        else if (!source.outputAudioMixerGroup.audioMixer.GetFloat(exposedParameterName, out startValue)) {
            error = AudioError.NOT_EXPOSED;
            return error;
        }
        else if (startValue == endValue) {
            error = AudioError.SAME_AS_CURRENT;
            return error;
        }
        else if (granularity < 1f) {
            error = AudioError.TOO_SMALL;
            return error;
        }

        // Calculate what we need to remove or add to the exposed parameter to achieve the endValue.
        float difference = endValue - startValue;
        float stepValue = difference / granularity;
        float stepTime = waitTime / granularity;

        StartCoroutine(ExposedParameterChanger(source.outputAudioMixerGroup.audioMixer, exposedParameterName, stepValue, stepTime, granularity));
        return error;
    }

    /// <summary>
    /// Remove the AudioMixerGroup from the sound with the given name,
    /// so that it isn't influenced by the settings in it anymore.
    /// </summary>
    /// <param name="name">Name of the sound.</param>
    /// <returns>AudioError, showing wheter and how removing the AudioMixerGroup failed.</returns>
    public AudioError RemoveGroup(string name) {
        AudioError error = TryGetSource(name, out AudioSource source);

        // Couldn't find source.
        if (error != AudioError.OK) {
            return error;
        }
        source.outputAudioMixerGroup = null;
        return error;
    }

    /// <summary>
    /// Adding the AudioMixerGroup to the sound with the given name,
    /// so that it is influenced by the settings in it.
    /// </summary>
    /// <param name="name">Name of the sound.</param>
    /// <returns>AudioError, showing wheter and how adding the AudioMixerGroup failed.</returns>
    public AudioError AddGroup(string name, AudioMixerGroup mixerGroup) {
        AudioError error = TryGetSource(name, out AudioSource source);

        // Couldn't find source.
        if (error != AudioError.OK) {
            return error;
        }
        source.outputAudioMixerGroup = mixerGroup;
        return error;
    }

    /// <summary>
    /// Remove a sound with the given name from the AudioManager.
    /// </summary>
    /// <param name="name">Name of the sound.</param>
    /// <returns>AudioError, showing wheter and how removing the sound failed.</returns>
    public AudioError RemoveSound(string name) {
        AudioError error = AudioError.OK;

        // Check if the given sound name is in our soundDictionary.
        if (!soundDictionary.Remove(name)) {
            error = AudioError.DOES_NOT_EXIST;
        }
        return error;
    }

    //************************************************************************************************************************
    // Private Section
    //************************************************************************************************************************

    /// <summary>
    /// Setup sounds so that the user input in the AudioManager,
    /// can be used to play the given sounds with the given values.
    /// Called in Awake to ensure it is only done once,
    /// because AudioManager is a DontDestroyOnLoad Singelton.
    /// </summary>
    private void SetupSounds() {
        foreach (Sound sound in initalSounds) {
            InstantiateSource(sound);
            AddSound(sound);
        }
    }

    /// <summary>
    /// Adds a new AudioSource component to the AudioManager if needed
    /// and sets the possible given parameters in the AudioSource object.
    /// </summary>
    /// <param name="sound">Sound we want to instantiate.</param>
    private void InstantiateSource(Sound sound) {
        // Check if the source is set to null.
        if (!sound.source) {
            sound.source = gameObject.AddComponent<AudioSource>();
        }
        sound.source.outputAudioMixerGroup = sound.mixerGroup;
        sound.source.clip = sound.clip;
        sound.source.volume = sound.volume;
        sound.source.pitch = sound.pitch;
        sound.source.loop = sound.loop;
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
    /// Changes the pitch of the given sound with a certain waitTime after each de -or increase.
    /// </summary>
    /// <param name="source">Source of the AudioFile.</param>
    /// <param name="stepValue">How much we want to de -or increase the value by each step.</param>
    /// <param name="stepTime">Delay we want to have after each de -or increase.</param>
    /// <param name="steps">Amount of steps that will be taken to decrease to the endValue.</param>
    private IEnumerator PitchChanger(AudioSource source, float stepValue, float stepTime, float steps) {
        // De -or increases the given pitch with the given amount of steps.
        for (; steps > 0; steps--) {
            source.pitch += stepValue;
            yield return new WaitForSeconds(stepTime);
        }
        // Correct for float rounding errors.
        source.pitch = Mathf.Round((source.pitch) * 100f) / 100f;
    }

    /// <summary>
    /// Changes the volume of the given sound to the given value with a certain waitTime after each de -or increase.
    /// </summary>
    /// <param name="source">Source of the AudioFile.</param>
    /// <param name="stepValue">How much we want to de -or increase the value by each step.</param>
    /// <param name="stepTime">Delay we want to have after each de -or increase.</param>
    /// <param name="steps">Amount of steps that will be taken to decrease to the endValue.</param>
    private IEnumerator VolumeChanger(AudioSource source, float stepValue, float stepTime, float steps) {
        // De -or increases the given pitch with the given amount of steps.
        for (; steps > 0; steps--) {
            source.volume += stepValue;
            yield return new WaitForSeconds(stepTime);
        }
        // Correct for float rounding errors.
        source.volume = Mathf.Round((source.volume) * 100f) / 100f;
    }

    /// <summary>
    /// Sets the startTime of the song. Needs to be done all the time before starting a sound,
    /// to ensure we start it at the beginning and not the time set by the PlayAtTimeStamp method.
    /// </summary>
    /// <param name="source">Source of the AudioFile.</param>
    /// <param name="startTime">Moment in the sound we want to start playing at.</param>
    private void SetStartTime(AudioSource source, float startTime = 0f) {
        source.time = startTime;
    }

    /// <summary>
    /// Changes the exposed AudioMixer parameter of the given mixer to the given value with a certain waitTime after each de -or increase.
    /// </summary>
    /// <param name="mixer">AudioMixer our exposed parameter resides in.</param>
    /// <param name="exposedParameterName">Name of the exposed parameter we want to change.</param>
    /// <param name="stepValue">How much we want to de -or increase the value by each step.</param>
    /// <param name="stepTime">Delay we want to have after each de -or increase.</param>
    /// <param name="steps">Amount of steps that will be taken to decrease to the endValue.</param>
    private IEnumerator ExposedParameterChanger(AudioMixer mixer, string exposedParameterName, float stepValue, float stepTime, float steps) {
        mixer.GetFloat(exposedParameterName, out float currentValue);
        // De -or increases the given pitch with the given amount of steps.
        for (; steps > 0; steps--) {
            currentValue += stepValue;
            mixer.SetFloat(exposedParameterName, currentValue);
            yield return new WaitForSeconds(stepTime);
        }
        // Correct for float rounding errors.
        currentValue = Mathf.Round((currentValue) * 100f) / 100f;
        mixer.SetFloat(exposedParameterName, currentValue);
    }
}

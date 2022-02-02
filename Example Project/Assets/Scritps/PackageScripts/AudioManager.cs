using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour {
    [SerializeField]
    [Tooltip("Inital sounds that should be registered on Awake with the AudioManager and the given settings.")]
    private AudioSourceSetting[] settings;

    private Dictionary<string, AudioSource> soundDictionary = new Dictionary<string, AudioSource>();
    private Action<string, float> soundFinishedCallback = new Action<string, float>(ResetStartTime);

    // Max. progress of the sound still detactable in an IEnumerator.
    private const float MAX_PROGRESS = 0.99f;
    // Max. spatial blend value that still counts as 2D.
    private const float SPATIAL_BLEND_2D = 0f;
    // Min. granularity value that is still valid.
    private const float MIN_GRANULARITY = 1f;

    // Default values for method parameters.
    private const float DEFAULT_VOLUME = 1f;
    private const float DEFAULT_PITCH = 1f;
    private const bool DEFAULT_LOOP = false;
    private const float DEFAULT_WAIT_TIME = 1f;
    private const float DEFAULT_GRANULARITY = 5f;

    #region Singelton
    public static AudioManager instance;

    private void Awake() {
        // Check if instance is already defined and if this gameObject is not the current instance.
        if (instance) {
            Debug.LogWarning("Multiple instances of AudioManager found. Not already loaded instance was destroyed.");
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        SetupSounds();
    }
    #endregion

    /// <summary>
    /// Adds given 2D sound with the given settings to the possible playable sounds,
    /// creates a new AudioSource object if not already done so with the given settings
    /// and appends it to the GameObject the AudioManager resides on as a new Component.
    /// If 3D functionality wants to be added additionaly call the Set3DAudioOptions method.
    /// </summary>
    /// <param name="name">Name the new sound should have.</param>
    /// <param name="path">Path to the clip we want to add to the new sound in the Resource folder.</param>
    /// <param name="volume">Volume we want the new sound to have.</param>
    /// <param name="pitch">Pitch we want the new sound to have.</param>
    /// <param name="loop">Defines wheter we want to repeat the new sound after completing it or not.</param>
    /// <param name="source">Source we want to add to the new sound.</param>
    /// <param name="mixerGroup">Mixer group the sound is influenced by.</param>
    /// <returns>AudioError, showing wheter and how adding a 2D sound from the given path with the given settings failed.</returns>
    public AudioError AddSoundFromPath(string name, string path, float volume = DEFAULT_VOLUME, float pitch = DEFAULT_PITCH, bool loop = DEFAULT_LOOP, AudioSource source = null, AudioMixerGroup mixerGroup = null) {
        AudioError error = AudioError.OK;
        // Load sound clip from the Resource folder on the given path.
        var clip = Resources.Load<AudioClip>(path);

        // Check if the clip couldn't be loaded correctly.
        if (!clip) {
            error = AudioError.INVALID_PATH;
            return error;
        }
        else if (!source) {
            source = gameObject.AddComponent<AudioSource>();
        }

        error = AddSound(name, source);

        // Check if the clip could be added correctly.
        if (error != AudioError.OK) {
            return error;
        }

        error = Set2DAudioOptions(source, clip, mixerGroup, loop, volume, pitch);
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

        source.Play();
        return error;
    }

    /// <summary>
    /// Plays the sound with the given name starting at the given startTime in the sound.
    /// </summary>
    /// <param name="name">Name of the sound.</param>
    /// <param name="startTime">Time we want to start playing the sound at in seconds.</param>
    /// <returns>AudioError, showing wheter and how playing the sound from the given startTime failed.</returns>
    public AudioError PlayAtTimeStamp(string name, float startTime) {
        AudioError error = TryGetSource(name, out AudioSource source);

        // Couldn't find source.
        if (error != AudioError.OK) {
            return error;
        }

        // Sets the start playback position to the given startTime in seconds.
        error = SetStartTime(name, startTime);
        if (error != AudioError.OK) {
            return error;
        }

        source.Play();
        // Calls the given callback as soon as the song is finished.
        StartCoroutine(DetectCurrentProgress(name, MAX_PROGRESS, soundFinishedCallback));
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
    /// Plays the sound with the given name at a 3D position in space.
    /// </summary>
    /// <param name="name">Name of the sound.</param>
    /// <param name="position">Position we want to place our sound at.</param>
    /// <returns>AudioError, showing wheter and how playing the sound at the given position failed.</returns>
    public AudioError PlayAt3DPosition(string name, Vector3 position) {
        AudioError error = TryGetSource(name, out AudioSource source);

        // Couldn't find source.
        if (error != AudioError.OK) {
            return error;
        }
        // Checks if 3D was even enabled in the spatialBlend.
        else if (source.spatialBlend <= SPATIAL_BLEND_2D) {
            error = AudioError.CAN_NOT_BE_3D;
            return error;
        }

        // Set position of our AudioSource.
        source.transform.position = position;
        source.Play();
        return error;
    }

    /// <summary>
    /// Plays the sound with the given name attached to a GameObject.
    /// </summary>
    /// <param name="name">Name of the sound.</param>
    /// <param name="gameObject">GameObject we want to attach our sound too.</param>
    /// <returns>AudioError, showing wheter and how playing the sound attached to the given gameobject failed.</returns>
    public AudioError PlayAttachedToGameObject(string name, GameObject gameObject) {
        AudioError error = TryGetSource(name, out AudioSource source);

        // Couldn't find source.
        if (error != AudioError.OK) {
            return error;
        }
        // Checks if 3D was even enabled in the spatialBlend.
        else if (source.spatialBlend <= SPATIAL_BLEND_2D) {
            error = AudioError.CAN_NOT_BE_3D;
            return error;
        }

        // Set parent of AudioSource to the given gameObject.
        source.transform.SetParent(gameObject.transform);
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

        source.PlayDelayed(delay);
        return error;
    }

    /// <summary>
    /// Plays the sound with the given name once.
    /// Multiple instances of the same sound can be run at the same time with this method.
    /// </summary>
    /// <param name="name">Name of the sound.</param>
    /// <returns>AudioError, showing wheter and how playing the sound once failed.</returns>
    public AudioError PlayOneShot(string name) {
        AudioError error = TryGetSource(name, out AudioSource source);

        // Couldn't find source.
        if (error != AudioError.OK) {
            return error;
        }

        source.PlayOneShot(source.clip);
        return error;
    }

    /// <summary>
    /// Sets the pitch of the given sound to random value between the given min -and max values.
    /// </summary>
    /// <param name="name">Name of the sound.</param>
    /// <param name="minPitch">Minimum amount of pitch the sound can be played at.</param>
    /// <param name="minPitch">Maximum amount of pitch the sound can be played at.</param>
    /// <returns>AudioError, showing wheter and how chaging the pitch failed.</returns>
    public AudioError ChangePitch(string name, float minPitch, float maxPitch) {
        AudioError error = TryGetSource(name, out AudioSource source);

        // Couldn't find source.
        if (error != AudioError.OK) {
            return error;
        }

        source.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
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
    /// Subscribes the given callback, so that it will be called with the given name and remaining time as a parameter,
    /// as soon as the sound only has the given remainingTime left to play until it finishes.
    /// </summary>
    /// <param name="name">Name of the sound.</param>
    /// <param name="remainingTime">Amount of remaining playback time in seconds, we want to call the callback at.</param>
    /// <param name="callback">Callback that should be called, once the sound only has the given amount of time left.</param>
    /// <returns>AudioError, showing wheter and how subscribing the callback failed.</returns>
    public AudioError SubscribeAudioFinished(string name, float remainingTime, Action<string, float> callback) {
        AudioError error = TryGetSource(name, out AudioSource source);

        // Couldn't find source.
        if (error != AudioError.OK) {
            return error;
        }
        // Check if the given remainingTime exceeds the actual clip length.
        else if (remainingTime > source.clip.length) {
            error = AudioError.INVALID_TIME;
            return error;
        }

        // Calculate the progress we need to call the callback at.
        // Consists of our given time divided by the clips actual length,
        // this will give us a value from (0 - 1),
        // but because we get the remainingTime
        // and not the time in the song we want to call the callback at,
        // we need to switch the value so for example from 0.3 to 0.7.
        float progress = -(remainingTime / source.clip.length) + 1;

        // Check if the progress is to high.
        if (progress > MAX_PROGRESS) {
            error = AudioError.INVALID_PROGRESS;
            return error;
        }

        // Calls the given callback as soon as the song has only the given remainingTime left to play.
        StartCoroutine(DetectCurrentProgress(name, progress, callback));
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
    public AudioError LerpPitch(string name, float endValue, float waitTime = DEFAULT_WAIT_TIME, float granularity = DEFAULT_GRANULARITY) {
        AudioError error = TryGetSource(name, out AudioSource source);

        // Couldn't find source.
        if (error != AudioError.OK) {
            return error;
        }
        else if (source.pitch == endValue) {
            error = AudioError.INVALID_END_VALUE;
            return error;
        }
        else if (granularity < MIN_GRANULARITY) {
            error = AudioError.INVALID_GRANULARITY;
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
    public AudioError LerpVolume(string name, float endValue, float waitTime = DEFAULT_WAIT_TIME, float granularity = DEFAULT_GRANULARITY) {
        AudioError error = TryGetSource(name, out AudioSource source);

        // Couldn't find source.
        if (error != AudioError.OK) {
            return error;
        }
        else if (source.volume == endValue) {
            error = AudioError.INVALID_END_VALUE;
            return error;
        }
        else if (granularity < MIN_GRANULARITY) {
            error = AudioError.INVALID_GRANULARITY;
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
            error = AudioError.MIXER_NOT_EXPOSED;
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
            valueDataError.Error = (int)AudioError.MIXER_NOT_EXPOSED;
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
            error = AudioError.MIXER_NOT_EXPOSED;
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
    public AudioError LerpGroupValue(string name, string exposedParameterName, float endValue, float waitTime = DEFAULT_WAIT_TIME, float granularity = DEFAULT_GRANULARITY) {
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
            error = AudioError.MIXER_NOT_EXPOSED;
            return error;
        }
        else if (startValue == endValue) {
            error = AudioError.INVALID_END_VALUE;
            return error;
        }
        else if (granularity < MIN_GRANULARITY) {
            error = AudioError.INVALID_GRANULARITY;
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

    /// <summary>
    /// Enables and sets the possible 3D audio options.
    /// </summary>
    /// <param name="name">Name of the sound.</param>
    /// <param name="minDistance">Distance that sound will not get louder at.</param>
    /// <param name="maxDistance">Distance that sound will still be hearable at.</param>
    /// <param name="spread">Sets the spread angles of the sound in degrees. (0f - 360f)</param>
    /// <param name="spatialBlend">Defines how much the Audio Source is affected by 3D space. (0f = 2D, 1f = 3D)</param>
    /// <param name="dopplerLevel">Defines Doppler Scale for the Audio Source. (0f - 5f)</param>
    /// <param name="rolloffMode">Sets how the Volume will be lowered over distance.</param>
    /// <returns>AudioError, showing wheter and how setting the 3D audio options failed.</returns>
    public AudioError Set3DAudioOptions(string name, float minDistance = 1f, float maxDistance = 500f, float spatialBlend = 1f, float spread = 0f, float dopplerLevel = 1f, AudioRolloffMode rolloffMode = AudioRolloffMode.Logarithmic) {
        AudioError error = TryGetSource(name, out AudioSource source);

        // Couldn't find source.
        if (error != AudioError.OK) {
            return error;
        }
        // Checks if 3D was even enabled in the spatialBlend.
        else if (spatialBlend <= SPATIAL_BLEND_2D) {
            error = AudioError.CAN_NOT_BE_3D;
            return error;
        }

        error = Set3DAudioOptions(source, spatialBlend, dopplerLevel, spread, rolloffMode, minDistance, maxDistance);
        return error;
    }

    /// <summary>
    /// Sets the startTime of the given sound.
    /// Does not get reset when playing the sound again use PlayAtTimeStamp for that.
    /// </summary>
    /// <param name="name">Name of the sound.</param>
    /// <param name="startTime">Moment in the sound we want to start playing at in seconds.</param>
    /// <returns>AudioError, showing wheter and how setting the start time failed.</returns>
    public AudioError SetStartTime(string name, float startTime) {
        AudioError error = TryGetSource(name, out AudioSource source);

        // Couldn't find source.
        if (error != AudioError.OK) {
            return error;
        }
        // Check if the given startTime exceeds the actual clip length.
        else if (startTime > source.clip.length) {
            error = AudioError.INVALID_TIME;
            return error;
        }

        source.time = startTime;
        return error;
    }

    //************************************************************************************************************************
    // Private Section
    //************************************************************************************************************************

    /// <summary>
    /// Setup sounds so that the user input in the AudioSourceSetting scriptable objects,
    /// can be used to play the given sounds with the given values.
    /// Called in Awake to ensure it is only done once,
    /// because AudioManager is a DontDestroyOnLoad Singelton.
    /// </summary>
    private void SetupSounds() {
        foreach (var setting in settings) {
            // Create AudioSource object for the AudioSourceSetting scriptable object.
            setting.source = gameObject.AddComponent<AudioSource>();
            // Add the name and it's AudioSource to our dictionary.
            AddSound(setting.name, setting.source);
            // Set the 2D values of the AudioSourceSetting attached AudioSource object.
            Set2DAudioOptions(setting);
            // Set 3D options if given of the AudioSourceSetting attached AudioSource object.
            Set3DAudioOptions(setting);
        }
    }

    /// <summary>
    /// Appends the given sounds AudioSource components to the dictionary.
    /// </summary>
    /// <param name="name">Name of the sound we want to register with the AudioManager.</param>
    /// <param name="source">AudioSource that contains the settings we want the sound to have.</param>
    /// <returns>AudioError, showing wheter and how adding a sound failed.</returns>
    private AudioError AddSound(string name, AudioSource source) {
        AudioError error = AudioError.OK;
        // Ensure there is not already a sound with the given name in our dictionary.
        if (soundDictionary.ContainsKey(name)) {
            error = AudioError.ALREADY_EXISTS;
            return error;
        }
        else if (!source) {
            error = AudioError.MISSING_SOURCE;
            return error;
        }
        soundDictionary.Add(name, source);
        return error;
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

    /// <summary>
    /// Detects the current time of the song and checks if the song only has the given amount of time left until it finished or not.
    /// </summary>
    /// <param name="name">Name of the song we want to reset.</param>
    /// <param name="progress">Amount of progress, we want to call the callback at. (0 - 1)</param>
    /// <param name="callback">Callback that will be called once the given progress passed.</param>
    private IEnumerator DetectCurrentProgress(string name, float progress, Action<string, float> callback) {
        TryGetSource(name, out AudioSource source);
        yield return new WaitUntil(() => SoundFinished(source, progress));
        // Recalculate the initally given remainingTime.
        // Consists of our progress (0 - 1) multiplied by the clips actual length,
        // this will give us a value in the clip length range,
        // but because we want to get the remainingTime
        // and not the current time in the song,
        // we need to switch the value so for example from 20s to 10s.
        float remainingTime = source.clip.length - (progress * source.clip.length);
        // Invoke the callback with the given parameters as long as it isn't null.
        callback?.Invoke(name, remainingTime);
    }

    /// <summary>
    /// Detects if a source has finished to the given amount of progress.
    /// </summary>
    /// <param name="source">Source we want to check.</param>
    /// <param name="progress">Amount of progress, we want to call the callback at. (0 - 1)</param>
    /// <returns>Wheter the source has passed.</returns>
    private bool SoundFinished(AudioSource source, float progress) {
        bool result = source.isPlaying && ((float)source.timeSamples / (float)source.clip.samples >= progress);
        return result;
    }

    /// <summary>
    /// Sets the possible given 2D parameters in the AudioSource object.
    /// </summary>
    /// <param name="source">AudioSource we want to play.</param>
    /// <param name="clip">Audioclip that should be played by the AudioSource.</param>
    /// <param name="mixerGroup">AudioMixer the sound should be played through.</param>
    /// <param name="loop">Wheter the sound should automatically repeat or not.</param>
    /// <param name="volume">Overall volume of the sound.</param>
    /// <param name="pitch">Frequency of the sound. Use this to slow down or speed up the sounds.</param>
    /// <returns>AudioError, showing wheter and how setting the 2D audio source options failed.</returns>
    private AudioError Set2DAudioOptions(AudioSource source, AudioClip clip, AudioMixerGroup mixerGroup, bool loop, float volume, float pitch) {
        AudioError error = AudioError.OK;

        if (!source) {
            error = AudioError.MISSING_SOURCE;
            return error;
        }

        source.clip = clip;
        source.outputAudioMixerGroup = mixerGroup;
        source.loop = loop;
        source.volume = volume;
        source.pitch = pitch;
        return error;
    }

    /// <summary>
    /// Sets the possible given 2D parameters in the AudioSource object from the AudioSourceSettings object.
    /// </summary>
    /// <param name="setting">Object containing all settings needed to set the AudioSource options.</param>
    /// <returns>AudioError, showing wheter and how setting the 2D audio source options failed.</returns>
    private AudioError Set2DAudioOptions(AudioSourceSetting setting) {
        AudioError error = AudioError.OK;

        if (!setting.source) {
            error = AudioError.MISSING_SOURCE;
            return error;
        }

        setting.source.clip = setting.audioClip;
        setting.source.outputAudioMixerGroup = setting.mixerGroup;
        setting.source.loop = setting.loop;
        setting.source.volume = setting.volume;
        setting.source.pitch = setting.pitch;
        return error;
    }

    /// <summary>
    /// Sets the possible given 3D parameters in the AudioSource object.
    /// </summary>
    /// <param name="source">AudioSource we want to play.</param>
    /// <param name="spatialBlend">Defines how much the Audio Source is affected by 3D space. (0f = 2D, 1f = 3D)</param>
    /// <param name="dopplerLevel">Defines Doppler Scale for the Audio Source. (0f - 5f)</param>
    /// <param name="spread">Sets the spread angles of the sound in degrees. (0f - 360f)</param>
    /// <param name="rolloffMode">Sets how the Volume will be lowered over distance.</param>
    /// <param name="minDistance">Distance that sound will not get louder at.</param>
    /// <param name="maxDistance">Distance that sound will still be hearable at.</param>
    /// <returns>AudioError, showing wheter and how setting the 3D audio source options failed.</returns>
    private AudioError Set3DAudioOptions(AudioSource source, float spatialBlend, float dopplerLevel, float spread, AudioRolloffMode rolloffMode, float minDistance, float maxDistance) {
        AudioError error = AudioError.OK;

        // Check if source is null.
        if (!source) {
            error = AudioError.MISSING_SOURCE;
            return error;
        }
        // Checks if 3D was even enabled in the spatialBlend.
        else if (spatialBlend <= SPATIAL_BLEND_2D) {
            error = AudioError.CAN_NOT_BE_3D;
            return error;
        }

        source.spatialize = true;
        source.spatialBlend = spatialBlend;
        source.dopplerLevel = dopplerLevel;
        source.spread = spread;
        source.rolloffMode = rolloffMode;
        source.minDistance = minDistance;
        source.maxDistance = maxDistance;
        return error;
    }

    /// <summary>
    /// Sets the possible given 3D parameters in the AudioSource object from the AudioSourceSettings object.
    /// </summary>
    /// <param name="setting">Object containing all settings needed to set the AudioSource options.</param>
    /// <returns>AudioError, showing wheter and how setting the 3D audio source options failed.</returns>
    private AudioError Set3DAudioOptions(AudioSourceSetting setting) {
        AudioError error = AudioError.OK;

        // Check if source is null.
        if (!setting.source) {
            error = AudioError.MISSING_SOURCE;
            return error;
        }
        // Checks if 3D was even enabled in the spatialBlend.
        else if (setting.spatialBlend <= SPATIAL_BLEND_2D) {
            error = AudioError.CAN_NOT_BE_3D;
            return error;
        }

        setting.source.spatialize = true;
        setting.source.spatialBlend = setting.spatialBlend;
        setting.source.dopplerLevel = setting.dopplerLevel;
        setting.source.spread = setting.spread;
        setting.source.rolloffMode = setting.volumeRolloff;
        setting.source.minDistance = setting.minDistance;
        setting.source.maxDistance = setting.maxDistance;
        return error;
    }

    /// <summary>
    /// Resets the startTime for the given sound after we wait for the end of it,
    /// to ensure the Play() method has already started playing the sound,
    /// because if we don't, we reset the startTime before playing
    /// and therefore start at 0 instead of the given startTime.
    /// </summary>
    /// <param name="name">Name of the sound.</param>
    /// <param name="remainingTime">Only needed for Action callback, ignored in this case.</param>
    private static void ResetStartTime(string name, float remainingTime) {
        instance.TryGetSource(name, out AudioSource source);
        // Stop the sound if it isn't set to looping,
        // this is done to ensure the sound doesn't replay,
        // when it is not set to looping.
        if (!source.loop) {
            instance.Stop(name);
        }
        instance.SetStartTime(name, 0f);
    }
}

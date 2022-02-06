using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using System;
using System.Runtime.CompilerServices;

public class AudioManager : MonoBehaviour {
    [SerializeField]
    [Header("Logger Settings:")]
    private LoggingLevel loggingLevel;

    [SerializeField]
    [Header("Sound Settings:")]
    [Tooltip("Inital sounds that should be registered on Awake with the AudioManager and the given settings.")]
    private AudioSourceSetting[] settings;

    private Dictionary<AudioSource, Dictionary<string, AudioSource>> parentChildDictionary = new Dictionary<AudioSource, Dictionary<string, AudioSource>>();
    private Dictionary<string, AudioSource> soundDictionary = new Dictionary<string, AudioSource>();
    private Action<string, float> resetStartTimeCallback = new Action<string, float>(ResetStartTime);
    private Logger logger;

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

        logger = new Logger(loggingLevel);
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
        logger.Log("Attempting to register new AudioSource entry with the name: " + name, LoggingLevel.INTERMEDIATE, LoggingType.NORMAL, this);
        AudioError error = AudioError.OK;
        // Load sound clip from the Resource folder on the given path.
        logger.Log("Attempting to load AudioClip from the given path: " + path, LoggingLevel.VERBOSE, LoggingType.NORMAL, this);
        var clip = Resources.Load<AudioClip>(path);

        // Check if the clip couldn't be loaded correctly.
        if (!clip) {
            logger.Log("Can't register new AudioSource entry because the path: " + path + " does not lead to a valid audio clip", LoggingLevel.LOW, LoggingType.WARNING, this);
            error = AudioError.INVALID_PATH;
            return error;
        }
        else if (!source) {
            logger.Log("No AudioSource passed with the given method, adding a new one to the AudioManager", LoggingLevel.VERBOSE, LoggingType.NORMAL, this);
            source = gameObject.AddComponent<AudioSource>();
        }

        error = AddSound(name, source);

        // Check if the clip could be added correctly.
        if (error != AudioError.OK) {
            logger.Log("Registering new AudioSource entry with the AudioManager failed successfull", LoggingLevel.HIGH, LoggingType.NORMAL, this);
            return error;
        }

        error = Set2DAudioOptions(source, clip, mixerGroup, loop, volume, pitch);
        logger.Log("Registering new AudioSource entry with the AudioManager successfull successfull", LoggingLevel.HIGH, LoggingType.NORMAL, this);
        return error;
    }

    /// <summary>
    /// Plays the sound with the given name.
    /// </summary>
    /// <param name="name">Name of the sound.</param>
    /// <returns>AudioError, showing wheter and how playing the sound failed.</returns>
    public AudioError Play(string name) {
        logger.Log("Attempting to play the registered AudioSource entry with the name: " + name, LoggingLevel.INTERMEDIATE, LoggingType.NORMAL, this);
        AudioError error = TryGetSource(name, out AudioSource source);

        // Couldn't find source.
        if (error != AudioError.OK) {
            return error;
        }

        logger.Log("Starting to play the given registered AudioSource entry successfull", LoggingLevel.HIGH, LoggingType.NORMAL, this);
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
        logger.Log("Attempting to play the registered AudioSource entry at the given timeStamp with the name: " + name, LoggingLevel.INTERMEDIATE, LoggingType.NORMAL, this);
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

        logger.Log("Starting to play the given registered AudioSource entry successfull", LoggingLevel.HIGH, LoggingType.NORMAL, this);
        source.Play();
        // Calls the given callback as soon as the song is finished.
        StartCoroutine(DetectCurrentProgress(name, MAX_PROGRESS, resetStartTimeCallback));
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
        logger.Log("Attempting to get playBackPosition of the registered AudioSource entry with the name: " + name, LoggingLevel.INTERMEDIATE, LoggingType.NORMAL, this);
        AudioError error = TryGetSource(name, out AudioSource source);
        ValueDataError<float> valueDataError = new ValueDataError<float>(float.NaN, (int)error);

        // Couldn't find source.
        if (error != AudioError.OK) {
            return valueDataError;
        }

        logger.Log("Reading playBackPosition from the given registered AudioSource entry successfull", LoggingLevel.HIGH, LoggingType.NORMAL, this);
        valueDataError.Value = source.time;
        return valueDataError;
    }

    /// <summary>
    /// Plays the sound with the given name at a 3D position in space.
    /// </summary>
    /// <param name="name">Name of the sound.</param>
    /// <param name="position">Position we want to create an empty gameObject and play the given sound at.</param>
    /// <returns>AudioError, showing wheter and how playing the sound at the given position failed.</returns>
    public AudioError PlayAt3DPosition(string name, Vector3 position) {
        logger.Log("Attempting to play the registered AudioSource entry at the given 3D position in space with the name: " + name, LoggingLevel.INTERMEDIATE, LoggingType.NORMAL, this);
        AudioError error = TryGetSource(name, out AudioSource parentSource);

        // Couldn't find source.
        if (error != AudioError.OK) {
            return error;
        }
        // Checks if 3D was even enabled in the spatialBlend.
        else if (parentSource.spatialBlend <= SPATIAL_BLEND_2D) {
            logger.Log("The sound can not be 3D, because spatialBlend is set to be 2D instead of 3D", LoggingLevel.LOW, LoggingType.WARNING, this);
            error = AudioError.CAN_NOT_BE_3D;
            return error;
        }

        return PlayAt3DPosition(parentSource, position, false);
    }

    /// <summary>
    /// Plays the sound with the given name once at a 3D position in space.
    /// Multiple instances of the same sound can be run at the same time with this method.
    /// </summary>
    /// <param name="name">Name of the sound.</param>
    /// <param name="position">Position we want to create an empty gameObject and play the given sound at.</param>
    /// <returns>AudioError, showing wheter and how playing the sound at the given position once failed.</returns>
    public AudioError PlayOneShotAt3DPosition(string name, Vector3 position) {
        logger.Log("Attempting to play the registered AudioSource entry once at the given 3D position in space with the name: " + name, LoggingLevel.INTERMEDIATE, LoggingType.NORMAL, this);
        AudioError error = TryGetSource(name, out AudioSource parentSource);

        // Couldn't find source.
        if (error != AudioError.OK) {
            return error;
        }
        // Checks if 3D was even enabled in the spatialBlend.
        else if (parentSource.spatialBlend <= SPATIAL_BLEND_2D) {
            logger.Log("The sound can not be 3D, because spatialBlend is set to be 2D instead of 3D", LoggingLevel.LOW, LoggingType.WARNING, this);
            error = AudioError.CAN_NOT_BE_3D;
            return error;
        }

        return PlayAt3DPosition(parentSource, position, true);
    }

    /// <summary>
    /// Plays the sound with the given name attached to a GameObject.
    /// </summary>
    /// <param name="name">Name of the sound.</param>
    /// <param name="gameObject">GameObject we want to attach our sound too.</param>
    /// <returns>AudioError, showing wheter and how playing the sound attached to the given gameobject once failed.</returns>
    public AudioError PlayAttachedToGameObject(string name, GameObject gameObject) {
        logger.Log("Attempting to play the registered AudioSource entry attached to the given gameObject with the name: " + name, LoggingLevel.INTERMEDIATE, LoggingType.NORMAL, this);
        AudioError error = TryGetSource(name, out AudioSource parentSource);

        // Couldn't find source.
        if (error != AudioError.OK) {
            return error;
        }
        // Checks if 3D was even enabled in the spatialBlend.
        else if (parentSource.spatialBlend <= SPATIAL_BLEND_2D) {
            logger.Log("The sound can not be 3D, because spatialBlend is set to be 2D instead of 3D", LoggingLevel.LOW, LoggingType.WARNING, this);
            error = AudioError.CAN_NOT_BE_3D;
            return error;
        }

        return PlayAttachedToGameObject(parentSource, gameObject, false);
    }

    /// <summary>
    /// Plays the sound with the given name attached to a GameObject.
    /// Multiple instances of the same sound can be run at the same time with this method.
    /// </summary>
    /// <param name="name">Name of the sound.</param>
    /// <param name="gameObject">GameObject we want to attach our sound too.</param>
    /// <returns>AudioError, showing wheter and how playing the sound attached to the given gameobject failed.</returns>
    public AudioError PlayOneShotAttachedToGameObject(string name, GameObject gameObject) {
        logger.Log("Attempting to play the registered AudioSource entry once attached to the given gameObject with the name: " + name, LoggingLevel.INTERMEDIATE, LoggingType.NORMAL, this);
        AudioError error = TryGetSource(name, out AudioSource parentSource);

        // Couldn't find source.
        if (error != AudioError.OK) {
            return error;
        }
        // Checks if 3D was even enabled in the spatialBlend.
        else if (parentSource.spatialBlend <= SPATIAL_BLEND_2D) {
            logger.Log("The sound can not be 3D, because spatialBlend is set to be 2D instead of 3D", LoggingLevel.LOW, LoggingType.WARNING, this);
            error = AudioError.CAN_NOT_BE_3D;
            return error;
        }

        return PlayAttachedToGameObject(parentSource, gameObject, true);
    }

    /// <summary>
    /// Plays the sound with the given name after the given delay time.
    /// </summary>
    /// <param name="name">Name of the sound.</param>
    /// <param name="delay">Delay until sound is played.</param>
    /// <returns>AudioError, showing wheter and how playing the sound after the given amount of time failed.</returns>
    public AudioError PlayDelayed(string name, float delay) {
        logger.Log("Attempting to play the registered AudioSource entry delayed with the name: " + name, LoggingLevel.INTERMEDIATE, LoggingType.NORMAL, this);
        AudioError error = TryGetSource(name, out AudioSource source);

        // Couldn't find source.
        if (error != AudioError.OK) {
            return error;
        }

        logger.Log("Starting to play the given registered AudioSource entry delayed successfull", LoggingLevel.HIGH, LoggingType.NORMAL, this);
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
        logger.Log("Attempting to play the registered AudioSource entry once with the name: " + name, LoggingLevel.INTERMEDIATE, LoggingType.NORMAL, this);
        AudioError error = TryGetSource(name, out AudioSource source);

        // Couldn't find source.
        if (error != AudioError.OK) {
            return error;
        }

        logger.Log("Starting to play the given registered AudioSource entry once successfull", LoggingLevel.HIGH, LoggingType.NORMAL, this);
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
        logger.Log("Attempting to randomly change pitch of the registered AudioSource entry with the name: " + name, LoggingLevel.INTERMEDIATE, LoggingType.NORMAL, this);
        AudioError error = TryGetSource(name, out AudioSource source);

        // Couldn't find source.
        if (error != AudioError.OK) {
            return error;
        }

        logger.Log("Changing pitch of the given registered AudioSource entry to random value between the given min and max value successfull", LoggingLevel.HIGH, LoggingType.NORMAL, this);
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
        logger.Log("Attempting to play the registered AudioSource entry scheduled with the name: " + name, LoggingLevel.INTERMEDIATE, LoggingType.NORMAL, this);
        AudioError error = TryGetSource(name, out AudioSource source);

        // Couldn't find source.
        if (error != AudioError.OK) {
            return error;
        }

        logger.Log("Starting to play the given registered AudioSource entry scheduled successfull", LoggingLevel.HIGH, LoggingType.NORMAL, this);
        source.PlayScheduled(time);
        return error;
    }

    /// <summary>
    /// Stops the sound with the given name.
    /// </summary>
    /// <param name="name">Name of the sound.</param>
    /// <returns>AudioError, showing wheter and how stopping the sound failed.</returns>
    public AudioError Stop(string name) {
        logger.Log("Attempting to stop the registered AudioSource entry with the name: " + name, LoggingLevel.INTERMEDIATE, LoggingType.NORMAL, this);
        AudioError error = TryGetSource(name, out AudioSource source);

        // Couldn't find source.
        if (error != AudioError.OK) {
            return error;
        }

        logger.Log("Stopping the given registered AudioSource entry successfull", LoggingLevel.HIGH, LoggingType.NORMAL, this);
        source.Stop();
        return error;
    }

    /// <summary>
    /// Mutes or Unmutes the sound with the given name.
    /// </summary>
    /// <param name="name">Name of the sound.</param>
    /// <returns>AudioError, showing wheter and how muting or unmuting the sound failed.</returns>
    public AudioError ToggleMute(string name) {
        logger.Log("Attempting to toggle mute the registered AudioSource entry with the name: " + name, LoggingLevel.INTERMEDIATE, LoggingType.NORMAL, this);
        AudioError error = TryGetSource(name, out AudioSource source);

        // Couldn't find source.
        if (error != AudioError.OK) {
            return error;
        }

        logger.Log("Muting the given registered AudioSource entry successfull", LoggingLevel.HIGH, LoggingType.NORMAL, this);
        source.mute = !source.mute;
        return error;
    }

    /// <summary>
    /// Pauses or Unpauses the sound with the given name.
    /// </summary>
    /// <param name="name">Name of the sound.</param>
    /// <returns>AudioError, showing wheter and how pausing or unpausing the sound failed.</returns>
    public AudioError TogglePause(string name) {
        logger.Log("Attempting to toggle pause the registered AudioSource entry with the name: " + name, LoggingLevel.INTERMEDIATE, LoggingType.NORMAL, this);
        AudioError error = TryGetSource(name, out AudioSource source);

        // Couldn't find source.
        if (error != AudioError.OK) {
            return error;
        }

        // Check if the sound is playing right now.
        if (source.isPlaying) {
            logger.Log("Paused the given registered AudioSource entry successfull", LoggingLevel.HIGH, LoggingType.NORMAL, this);
            source.Pause();
            return error;
        }

        logger.Log("Unpaused the given registered AudioSource entry successfull", LoggingLevel.HIGH, LoggingType.NORMAL, this);
        source.UnPause();
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
        logger.Log("Attempting to subscribe to the registered AudioSource entry finishing to the given remainingTime with the name: " + name, LoggingLevel.INTERMEDIATE, LoggingType.NORMAL, this);
        AudioError error = TryGetSource(name, out AudioSource source);

        // Couldn't find source.
        if (error != AudioError.OK) {
            return error;
        }
        // Check if the given remainingTime exceeds the actual clip length.
        else if (remainingTime > source.clip.length) {
            logger.Log("The given time exceeds the actual length of the clip", LoggingLevel.LOW, LoggingType.WARNING, this);
            error = AudioError.INVALID_TIME;
            return error;
        }

        // Calculate the progress we need to call the callback at.
        // Consists of our given time divided by the clips actual length,
        // this will give us a value from (0 - 1),
        // but because we get the remainingTime
        // and not the time in the song we want to call the callback at,
        // we need to switch the value so for example from 0.3 to 0.7.
        logger.Log("Calculating progress we want to subscribe to", LoggingLevel.VERBOSE, LoggingType.NORMAL, this);
        float progress = -(remainingTime / source.clip.length) + 1;

        // Check if the progress is to high.
        if (progress > MAX_PROGRESS) {
            logger.Log("The given value is to close to the end of the actual clip length, therefore the given value can not be detected, because playing audio is frame rate independent", LoggingLevel.LOW, LoggingType.WARNING, this);
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
        logger.Log("Attempting to get the progress from the registered AudioSource entry with the name: " + name, LoggingLevel.INTERMEDIATE, LoggingType.NORMAL, this);
        AudioError error = TryGetSource(name, out AudioSource source);
        ValueDataError<float> valueDataError = new ValueDataError<float>(float.NaN, (int)error);

        // Couldn't find source.
        if (error != AudioError.OK || !source.clip) {
            return valueDataError;
        }

        logger.Log("Calculating the current progress of the given registered AudioSource entry", LoggingLevel.HIGH, LoggingType.NORMAL, this);
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
        logger.Log("Attempting to get registered AudioSource entry with the given name: " + name, LoggingLevel.INTERMEDIATE, LoggingType.NORMAL, this);
        AudioError error = AudioError.OK;

        logger.Log("Checking if AudioSource entry with the given name: " + name + " has been registered with the AudioManager", LoggingLevel.VERBOSE, LoggingType.NORMAL, this);
        // Check if the given sound name is in our soundDictionary.
        if (!soundDictionary.TryGetValue(name, out source)) {
            logger.Log("Sound with the given name: " + name + " has not been registerd with the AudioManager", LoggingLevel.LOW, LoggingType.WARNING, this);
            error = AudioError.DOES_NOT_EXIST;
        }
        // Check if the source is set.
        else if (!source) {
            logger.Log("Sound with the given name: " + name + " does not have an AudioSource component on the GameObject the AudioManager resides on", LoggingLevel.LOW, LoggingType.WARNING, this);
            error = AudioError.MISSING_SOURCE;
        }
        logger.Log("Getting registered AudioSource entry successfull", LoggingLevel.HIGH, LoggingType.NORMAL, this);
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
        logger.Log("Attempting to lerp pitch of the registered AudioSource entry with the given name: " + name, LoggingLevel.INTERMEDIATE, LoggingType.NORMAL, this);
        AudioError error = TryGetSource(name, out AudioSource source);

        // Couldn't find source.
        if (error != AudioError.OK) {
            return error;
        }
        else if (source.pitch == endValue) {
            logger.Log("The given endValue is already the same as the current value", LoggingLevel.LOW, LoggingType.WARNING, this);
            error = AudioError.INVALID_END_VALUE;
            return error;
        }
        else if (granularity < MIN_GRANULARITY) {
            logger.Log("The given granularity is too small, has to be higher than or equal to 1", LoggingLevel.LOW, LoggingType.WARNING, this);
            error = AudioError.INVALID_GRANULARITY;
            return error;
        }

        logger.Log("Calculate difference / stepValue and stepTime from wanted and current pitch divided by granularity", LoggingLevel.VERBOSE, LoggingType.NORMAL, this);
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
        logger.Log("Attempting to lerp volume of the registered AudioSource entry with the given name: " + name, LoggingLevel.INTERMEDIATE, LoggingType.NORMAL, this);
        AudioError error = TryGetSource(name, out AudioSource source);

        // Couldn't find source.
        if (error != AudioError.OK) {
            return error;
        }
        else if (source.volume == endValue) {
            logger.Log("The given endValue is already the same as the current value", LoggingLevel.LOW, LoggingType.WARNING, this);
            error = AudioError.INVALID_END_VALUE;
            return error;
        }
        else if (granularity < MIN_GRANULARITY) {
            logger.Log("The given granularity is too small, has to be higher than or equal to 1", LoggingLevel.LOW, LoggingType.WARNING, this);
            error = AudioError.INVALID_GRANULARITY;
            return error;
        }

        logger.Log("Calculate difference / stepValue and stepTime from wanted and current pitch divided by granularity", LoggingLevel.VERBOSE, LoggingType.NORMAL, this);
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
        logger.Log("Attempting to change group value with the name: " + exposedParameterName + " of the registered AudioSource entry with the given name: " + name, LoggingLevel.INTERMEDIATE, LoggingType.NORMAL, this);
        AudioError error = TryGetSource(name, out AudioSource source);

        // Couldn't find source.
        if (error != AudioError.OK) {
            return error;
        }
        // Check if the outputAudioGroup value is not null.
        else if (!source.outputAudioMixerGroup) {
            logger.Log("Group methods may only be called with a sound that has a set AudioMixerGroup", LoggingLevel.LOW, LoggingType.WARNING, this);
            error = AudioError.MISSING_MIXER_GROUP;
            return error;
        }
        // Check if the AudioMixer parameter is exposed.
        else if (!source.outputAudioMixerGroup.audioMixer.SetFloat(exposedParameterName, newValue)) {
            logger.Log("The given parameter with the name: " + exposedParameterName + " in the AudioMixer is not exposed or does not exist", LoggingLevel.LOW, LoggingType.WARNING, this);
            error = AudioError.MIXER_NOT_EXPOSED;
        }
        logger.Log("Changing group value of the registered AudioSource entry successfull", LoggingLevel.HIGH, LoggingType.NORMAL, this);
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
        logger.Log("Attempting to get group value with the name: " + exposedParameterName + " of the registered AudioSource entry with the given name: " + name, LoggingLevel.INTERMEDIATE, LoggingType.NORMAL, this);
        AudioError error = TryGetSource(name, out AudioSource source);
        float currentValue = float.NaN;
        ValueDataError<float> valueDataError = new ValueDataError<float>(currentValue, (int)error);

        // Couldn't find source.
        if (error != AudioError.OK) {
            return valueDataError;
        }
        // Check if the outputAudioGroup value is not null.
        else if (!source.outputAudioMixerGroup) {
            logger.Log("Group methods may only be called with a sound that has a set AudioMixerGroup", LoggingLevel.LOW, LoggingType.WARNING, this);
            valueDataError.Error = (int)AudioError.MISSING_MIXER_GROUP;
            return valueDataError;
        }
        // Check if the AudioMixer parameter is exposed.
        else if (!source.outputAudioMixerGroup.audioMixer.GetFloat(exposedParameterName, out currentValue)) {
            logger.Log("The given parameter with the name: " + exposedParameterName + " in the AudioMixer is not exposed or does not exist", LoggingLevel.LOW, LoggingType.WARNING, this);
            valueDataError.Error = (int)AudioError.MIXER_NOT_EXPOSED;
        }
        logger.Log("Getting group value of the registered AudioSource entry successfull", LoggingLevel.HIGH, LoggingType.NORMAL, this);
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
        logger.Log("Attempting to reset group value with the name: " + exposedParameterName + " of the registered AudioSource entry with the given name: " + name, LoggingLevel.INTERMEDIATE, LoggingType.NORMAL, this);
        AudioError error = TryGetSource(name, out AudioSource source);

        // Couldn't find source.
        if (error != AudioError.OK) {
            return error;
        }
        // Check if the outputAudioGroup value is not null.
        else if (!source.outputAudioMixerGroup) {
            logger.Log("Group methods may only be called with a sound that has a set AudioMixerGroup", LoggingLevel.LOW, LoggingType.WARNING, this);
            error = AudioError.MISSING_MIXER_GROUP;
            return error;
        }
        // Check if the AudioMixer parameter is exposed.
        else if (!source.outputAudioMixerGroup.audioMixer.ClearFloat(exposedParameterName)) {
            logger.Log("The given parameter with the name: " + exposedParameterName + " in the AudioMixer is not exposed or does not exist", LoggingLevel.LOW, LoggingType.WARNING, this);
            error = AudioError.MIXER_NOT_EXPOSED;
        }
        logger.Log("Resetting group value of the registered AudioSource entry successfull", LoggingLevel.HIGH, LoggingType.NORMAL, this);
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
        logger.Log("Attempting to reset group value with the name: " + exposedParameterName + " of the registered AudioSource entry with the given name: " + name, LoggingLevel.INTERMEDIATE, LoggingType.NORMAL, this);
        AudioError error = TryGetSource(name, out AudioSource source);
        float startValue = float.NaN;

        // Couldn't find source.
        if (error != AudioError.OK) {
            return error;
        }
        // Check if the outputAudioGroup value is not null.
        else if (!source.outputAudioMixerGroup) {
            logger.Log("Group methods may only be called with a sound that has a set AudioMixerGroup", LoggingLevel.LOW, LoggingType.WARNING, this);
            error = AudioError.MISSING_MIXER_GROUP;
            return error;
        }
        // Check if the AudioMixer parameter is exposed.
        else if (!source.outputAudioMixerGroup.audioMixer.GetFloat(exposedParameterName, out startValue)) {
            logger.Log("The given parameter with the name: " + exposedParameterName + " in the AudioMixer is not exposed or does not exist", LoggingLevel.LOW, LoggingType.WARNING, this);
            error = AudioError.MIXER_NOT_EXPOSED;
            return error;
        }
        else if (startValue == endValue) {
            logger.Log("The given endValue is already the same as the current value", LoggingLevel.LOW, LoggingType.WARNING, this);
            error = AudioError.INVALID_END_VALUE;
            return error;
        }
        else if (granularity < MIN_GRANULARITY) {
            logger.Log("The given granularity is too small, has to be higher than or equal to 1", LoggingLevel.LOW, LoggingType.WARNING, this);
            error = AudioError.INVALID_GRANULARITY;
            return error;
        }

        logger.Log("Calculate difference / stepValue and stepTime from wanted and current pitch divided by granularity", LoggingLevel.VERBOSE, LoggingType.NORMAL, this);
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
        logger.Log("Attempting to remove group from the registered AudioSource entry with the given name: " + name, LoggingLevel.INTERMEDIATE, LoggingType.NORMAL, this);
        AudioError error = TryGetSource(name, out AudioSource source);

        // Couldn't find source.
        if (error != AudioError.OK) {
            return error;
        }

        logger.Log("Resetting outputAudioMixerGroup property of the given registered AudioSource entry successfull", LoggingLevel.HIGH, LoggingType.NORMAL, this);
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
        logger.Log("Attempting to add group from the registered AudioSource entry with the given name: " + name, LoggingLevel.INTERMEDIATE, LoggingType.NORMAL, this);
        AudioError error = TryGetSource(name, out AudioSource source);

        // Couldn't find source.
        if (error != AudioError.OK) {
            return error;
        }

        logger.Log("Setting outputAudioMixerGroup property of the given registered AudioSource entry to the given AudioMixerGroup successfull", LoggingLevel.HIGH, LoggingType.NORMAL, this);
        source.outputAudioMixerGroup = mixerGroup;
        return error;
    }

    /// <summary>
    /// Remove a sound with the given name from the AudioManager.
    /// </summary>
    /// <param name="name">Name of the sound.</param>
    /// <returns>AudioError, showing wheter and how removing the sound failed.</returns>
    public AudioError RemoveSound(string name) {
        logger.Log("Attempting to remove registered AudioSource entry with the given name: " + name, LoggingLevel.INTERMEDIATE, LoggingType.NORMAL, this);
        AudioError error = AudioError.OK;

        // Check if the given sound name is in our soundDictionary.
        if (!soundDictionary.Remove(name)) {
            logger.Log("Sound with the given name: " + name + " has not been registerd with the AudioManager", LoggingLevel.LOW, LoggingType.WARNING, this);
            error = AudioError.DOES_NOT_EXIST;
        }
        logger.Log("Removing registered AudioSource entry successfull", LoggingLevel.HIGH, LoggingType.NORMAL, this);
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
        logger.Log("Attempting to set 3D audio options of the registered AudioSource entry with the name: " + name, LoggingLevel.INTERMEDIATE, LoggingType.NORMAL, this);
        AudioError error = TryGetSource(name, out AudioSource source);

        // Couldn't find source.
        if (error != AudioError.OK) {
            return error;
        }
        // Checks if 3D was even enabled in the spatialBlend.
        else if (spatialBlend <= SPATIAL_BLEND_2D) {
            logger.Log("The sound can not be 3D, because spatialBlend is set to be 2D instead of 3D", LoggingLevel.LOW, LoggingType.WARNING, this);
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
        logger.Log("Attempting to set start time of the registered AudioSource entry with the name: " + name, LoggingLevel.INTERMEDIATE, LoggingType.NORMAL, this);
        AudioError error = TryGetSource(name, out AudioSource source);

        // Couldn't find source.
        if (error != AudioError.OK) {
            return error;
        }
        // Check if the given startTime exceeds the actual clip length.
        else if (startTime > source.clip.length) {
            logger.Log("Given start time execceds actual clip length", LoggingLevel.LOW, LoggingType.WARNING, this);
            error = AudioError.INVALID_TIME;
            return error;
        }

        logger.Log("Setting startTime of the given registered AudioSource entry successfull", LoggingLevel.HIGH, LoggingType.NORMAL, this);
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
        logger.Log("Attempting to register given AudioSourceSettings on startup", LoggingLevel.INTERMEDIATE, LoggingType.NORMAL, this);
        foreach (var setting in settings) {
            logger.Log("Attaching empty AudioSource for the given AudioSourceSetting with the name: " + setting.name, LoggingLevel.VERBOSE, LoggingType.NORMAL, this);
            // Create AudioSource object for the AudioSourceSetting scriptable object.
            setting.source = gameObject.AddComponent<AudioSource>();
            // Add the name and it's AudioSource to our dictionary.
            AddSound(setting.soundName, setting.source);
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
        logger.Log("Attempting to add new AudioSource entry with the name: " + name, LoggingLevel.INTERMEDIATE, LoggingType.NORMAL, this);
        AudioError error = AudioError.OK;
        // Ensure there is not already a sound with the given name in our dictionary.
        logger.Log("Checking if new AudioSource entry with the given key: " + name + " is valid", LoggingLevel.VERBOSE, LoggingType.NORMAL, this);
        if (soundDictionary.ContainsKey(name)) {
            logger.Log("Can't add new AudioSource entry as there already exists a AudioSource entry with that name", LoggingLevel.LOW, LoggingType.WARNING, this);
            error = AudioError.ALREADY_EXISTS;
            return error;
        }
        else if (!source) {
            logger.Log("Sound with the given name: " + name + " does not have an AudioSource component on the GameObject the AudioManager resides on", LoggingLevel.LOW, LoggingType.WARNING, this);
            error = AudioError.MISSING_SOURCE;
            return error;
        }
        logger.Log("Registering new valid AudioSource entry with the AudioManager successfull", LoggingLevel.HIGH, LoggingType.NORMAL, this);
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
        logger.Log("Lerping pitch of the registered AudioSource entry over the given time", LoggingLevel.VERBOSE, LoggingType.NORMAL, this);
        // De -or increases the given pitch with the given amount of steps.
        for (; steps > 0; steps--) {
            source.pitch += stepValue;
            yield return new WaitForSeconds(stepTime);
        }
        // Correct for float rounding errors.
        source.pitch = Mathf.Round((source.pitch) * 100f) / 100f;
        logger.Log("Lerping pitch of the registered AudioSource entry successfull", LoggingLevel.HIGH, LoggingType.NORMAL, this);
    }

    /// <summary>
    /// Changes the volume of the given sound to the given value with a certain waitTime after each de -or increase.
    /// </summary>
    /// <param name="source">Source of the AudioFile.</param>
    /// <param name="stepValue">How much we want to de -or increase the value by each step.</param>
    /// <param name="stepTime">Delay we want to have after each de -or increase.</param>
    /// <param name="steps">Amount of steps that will be taken to decrease to the endValue.</param>
    private IEnumerator VolumeChanger(AudioSource source, float stepValue, float stepTime, float steps) {
        logger.Log("Lerping volume of the registered AudioSource entry over the given time", LoggingLevel.VERBOSE, LoggingType.NORMAL, this);
        // De -or increases the given pitch with the given amount of steps.
        for (; steps > 0; steps--) {
            source.volume += stepValue;
            yield return new WaitForSeconds(stepTime);
        }
        // Correct for float rounding errors.
        source.volume = Mathf.Round((source.volume) * 100f) / 100f;
        logger.Log("Lerping volume of the registered AudioSource entry successfull", LoggingLevel.HIGH, LoggingType.NORMAL, this);
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
        logger.Log("Lerping exposed parameter of the registered AudioSource entry over the given time", LoggingLevel.VERBOSE, LoggingType.NORMAL, this);
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
        logger.Log("Lerping exposed parameter of the registered AudioSource entry successfull", LoggingLevel.HIGH, LoggingType.NORMAL, this);
    }

    /// <summary>
    /// Detects the current time of the song and checks if the song only has the given amount of time left until it finished or not.
    /// </summary>
    /// <param name="name">Name of the song we want to reset.</param>
    /// <param name="progress">Amount of progress, we want to call the callback at. (0 - 1)</param>
    /// <param name="callback">Callback that will be called once the given progress passed.</param>
    private IEnumerator DetectCurrentProgress(string name, float progress, Action<string, float> callback) {
        logger.Log("Starting coroutine that dectects when the given registered AudioSource entry with the name: " + name + " has completed to the given degree: " + (progress * 100).ToString("0.00") + "%", LoggingLevel.VERBOSE, LoggingType.NORMAL, this);
        TryGetSource(name, out AudioSource source);
        yield return new WaitUntil(() => SoundFinished(source, progress));
        // Recalculate the initally given remainingTime.
        // Consists of our progress (0 - 1) multiplied by the clips actual length,
        // this will give us a value in the clip length range,
        // but because we want to get the remainingTime
        // and not the current time in the song,
        // we need to switch the value so for example from 20s to 10s.
        float remainingTime = source.clip.length - (progress * source.clip.length);
        logger.Log("Calling given callback with the remainingTime left to play for the registered AudioSource entry being " + remainingTime.ToString("0.00") + " seconds", LoggingLevel.VERBOSE, LoggingType.NORMAL, this);
        // Invoke the callback with the given parameters as long as it isn't null.
        callback?.Invoke(name, remainingTime);
        logger.Log("Detecting progress of the registered AudioSource entry successfull", LoggingLevel.HIGH, LoggingType.NORMAL, this);
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
    /// Copys all the given settings from the given registered AudioSource entry object to another.
    /// </summary>
    /// <param name="copyTo">Object we want to copy the settings to.</param>
    /// <param name="copyFrom">Object we want to copy the settings from.</param>
    /// <returns>AudioError, showing wheter and how copying the 2D and 3D options from one audioSource to another failed.</returns>
    private AudioError CopyAudioSourceSettings(AudioSource copyTo, AudioSource copyFrom) {
        logger.Log("Attempting to copy 2D and 3D settings from given registered AudioSource entry to another AudioSource", LoggingLevel.INTERMEDIATE, LoggingType.NORMAL, this);
        AudioError error = Set2DAudioOptions(copyTo, copyFrom.clip, copyFrom.outputAudioMixerGroup, copyFrom.loop, copyFrom.volume, copyFrom.pitch);
        if (error != AudioError.OK) {
            return error;
        }
        return Set3DAudioOptions(copyTo, copyFrom.spatialBlend, copyFrom.dopplerLevel, copyFrom.spread, copyFrom.rolloffMode, copyFrom.minDistance, copyFrom.maxDistance);
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
        logger.Log("Attempting to set 2D audio options of the given registered AudioSource entry", LoggingLevel.INTERMEDIATE, LoggingType.NORMAL, this);
        AudioError error = AudioError.OK;

        logger.Log("Checking if passed AudioSource has an AudioSource component on the GameObject the AudioManager resides on", LoggingLevel.VERBOSE, LoggingType.NORMAL, this);
        if (!source) {
            logger.Log("Sound does not have an AudioSource component on the GameObject the AudioManager resides on", LoggingLevel.LOW, LoggingType.WARNING, this);
            error = AudioError.MISSING_SOURCE;
            return error;
        }

        logger.Log("Setting clip property of our given registered AudioSource entry", LoggingLevel.VERBOSE, LoggingType.NORMAL, this);
        source.clip = clip;
        logger.Log("Setting outputAudioMixerGroup property of our given registered AudioSource entry", LoggingLevel.VERBOSE, LoggingType.NORMAL, this);
        source.outputAudioMixerGroup = mixerGroup;
        logger.Log("Setting loop property of our given registered AudioSource entry", LoggingLevel.VERBOSE, LoggingType.NORMAL, this);
        source.loop = loop;
        logger.Log("Setting volume property of our given registered AudioSource entry", LoggingLevel.VERBOSE, LoggingType.NORMAL, this);
        source.volume = volume;
        logger.Log("Setting pitch property of our given registered AudioSource entry", LoggingLevel.VERBOSE, LoggingType.NORMAL, this);
        source.pitch = pitch;
        logger.Log("Setting 2D audio options of our registered AudioSource entry successfull", LoggingLevel.HIGH, LoggingType.NORMAL, this);
        return error;
    }

    /// <summary>
    /// Sets the possible given 2D parameters in the AudioSource object from the AudioSourceSettings object.
    /// </summary>
    /// <param name="setting">Object containing all settings needed to set the AudioSource options.</param>
    /// <returns>AudioError, showing wheter and how setting the 2D audio source options failed.</returns>
    private AudioError Set2DAudioOptions(AudioSourceSetting setting) {
        logger.Log("Attempting to set 2D audio options from the given AudioSourceSetting", LoggingLevel.INTERMEDIATE, LoggingType.NORMAL, this);
        AudioError error = AudioError.OK;

        logger.Log("Checking if passed AudioSource has an AudioSource component on the GameObject the AudioManager resides on", LoggingLevel.VERBOSE, LoggingType.NORMAL, this);
        if (!setting.source) {
            logger.Log("Sound does not have an AudioSource component on the GameObject the AudioManager resides on", LoggingLevel.LOW, LoggingType.WARNING, this);
            error = AudioError.MISSING_SOURCE;
            return error;
        }

        logger.Log("Setting clip property of our given registered AudioSource entry from AudioSourceSetting", LoggingLevel.VERBOSE, LoggingType.NORMAL, this);
        setting.source.clip = setting.audioClip;
        logger.Log("Setting outputAudioMixerGroup property of our given registered AudioSource entry from AudioSourceSetting", LoggingLevel.VERBOSE, LoggingType.NORMAL, this);
        setting.source.outputAudioMixerGroup = setting.mixerGroup;
        logger.Log("Setting loop property of our given registered AudioSource entry from AudioSourceSetting", LoggingLevel.VERBOSE, LoggingType.NORMAL, this);
        setting.source.loop = setting.loop;
        logger.Log("Setting volume property of our given registered AudioSource entry from AudioSourceSettin.", LoggingLevel.VERBOSE, LoggingType.NORMAL, this);
        setting.source.volume = setting.volume;
        logger.Log("Setting pitch property of our given registered AudioSource entry from AudioSourceSetting", LoggingLevel.VERBOSE, LoggingType.NORMAL, this);
        setting.source.pitch = setting.pitch;
        logger.Log("Setting 2D audio options of our registered AudioSource entry from AudioSourceSetting successfull", LoggingLevel.HIGH, LoggingType.NORMAL, this);
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
        logger.Log("Attempting to set 3D audio options of the given registered AudioSource entry", LoggingLevel.INTERMEDIATE, LoggingType.NORMAL, this);
        AudioError error = AudioError.OK;

        logger.Log("Checking if passed AudioSource has an AudioSource component on the GameObject the AudioManager resides on", LoggingLevel.VERBOSE, LoggingType.NORMAL, this);
        // Check if source is null.
        if (!source) {
            logger.Log("Sound does not have an AudioSource component on the GameObject the AudioManager resides on", LoggingLevel.LOW, LoggingType.WARNING, this);
            error = AudioError.MISSING_SOURCE;
            return error;
        }
        // Checks if 3D was even enabled in the spatialBlend.
        else if (spatialBlend <= SPATIAL_BLEND_2D) {
            logger.Log("The sound can not be 3D, because spatialBlend is set to be 2D instead of 3D", LoggingLevel.VERBOSE, LoggingType.NORMAL, this);
            error = AudioError.CAN_NOT_BE_3D;
            return error;
        }

        logger.Log("Setting spatialize property of our given registered AudioSource entry", LoggingLevel.VERBOSE, LoggingType.NORMAL, this);
        source.spatialize = true;
        logger.Log("Setting spatialBlend property of our given registered AudioSource entry", LoggingLevel.VERBOSE, LoggingType.NORMAL, this);
        source.spatialBlend = spatialBlend;
        logger.Log("Setting dopplerLevel property of our given registered AudioSource entry", LoggingLevel.VERBOSE, LoggingType.NORMAL, this);
        source.dopplerLevel = dopplerLevel;
        logger.Log("Setting spread property of our given registered AudioSource entry", LoggingLevel.VERBOSE, LoggingType.NORMAL, this);
        source.spread = spread;
        logger.Log("Setting rolloffMode property of our given registered AudioSource entry", LoggingLevel.VERBOSE, LoggingType.NORMAL, this);
        source.rolloffMode = rolloffMode;
        logger.Log("Setting minDistance property of our given registered AudioSource entry", LoggingLevel.VERBOSE, LoggingType.NORMAL, this);
        source.minDistance = minDistance;
        logger.Log("Setting maxDistance property of our given registered AudioSource entry", LoggingLevel.VERBOSE, LoggingType.NORMAL, this);
        source.maxDistance = maxDistance;
        logger.Log("Setting 3D audio options of our registered AudioSource entry successfull", LoggingLevel.HIGH, LoggingType.NORMAL, this);
        return error;
    }

    /// <summary>
    /// Sets the possible given 3D parameters in the AudioSource object from the AudioSourceSettings object.
    /// </summary>
    /// <param name="setting">Object containing all settings needed to set the AudioSource options.</param>
    /// <returns>AudioError, showing wheter and how setting the 3D audio source options failed.</returns>
    private AudioError Set3DAudioOptions(AudioSourceSetting setting) {
        logger.Log("Attempting to set 3D audio options from the given AudioSourceSetting", LoggingLevel.INTERMEDIATE, LoggingType.NORMAL, this);
        AudioError error = AudioError.OK;

        logger.Log("Checking if passed AudioSource has an AudioSource component on the GameObject the AudioManager resides on", LoggingLevel.VERBOSE, LoggingType.NORMAL, this);
        // Check if source is null.
        if (!setting.source) {
            logger.Log("Sound does not have an AudioSource component on the GameObject the AudioManager resides on", LoggingLevel.LOW, LoggingType.WARNING, this);
            error = AudioError.MISSING_SOURCE;
            return error;
        }
        // Checks if 3D was even enabled in the spatialBlend.
        else if (setting.spatialBlend <= SPATIAL_BLEND_2D) {
            logger.Log("The sound can not be 3D, because spatialBlend is set to be 2D instead of 3D", LoggingLevel.VERBOSE, LoggingType.NORMAL, this);
            error = AudioError.CAN_NOT_BE_3D;
            return error;
        }

        logger.Log("Setting spatialize property of our given registered AudioSource entry", LoggingLevel.VERBOSE, LoggingType.NORMAL, this);
        setting.source.spatialize = true;
        logger.Log("Setting spatialBlend property of our given registered AudioSource entry", LoggingLevel.VERBOSE, LoggingType.NORMAL, this);
        setting.source.spatialBlend = setting.spatialBlend;
        logger.Log("Setting dopplerLevel property of our given registered AudioSource entry", LoggingLevel.VERBOSE, LoggingType.NORMAL, this);
        setting.source.dopplerLevel = setting.dopplerLevel;
        logger.Log("Setting spread property of our given registered AudioSource entry", LoggingLevel.VERBOSE, LoggingType.NORMAL, this);
        setting.source.spread = setting.spread;
        logger.Log("Setting rolloffMode property of our given registered AudioSource entry", LoggingLevel.VERBOSE, LoggingType.NORMAL, this);
        setting.source.rolloffMode = setting.volumeRolloff;
        logger.Log("Setting minDistance property of our given registered AudioSource entry", LoggingLevel.VERBOSE, LoggingType.NORMAL, this);
        setting.source.minDistance = setting.minDistance;
        logger.Log("Setting maxDistance property of our given registered AudioSource entry", LoggingLevel.VERBOSE, LoggingType.NORMAL, this);
        setting.source.maxDistance = setting.maxDistance;
        logger.Log("Setting 3D audio options of our registered AudioSource entry from AudioSourceSetting successfull", LoggingLevel.HIGH, LoggingType.NORMAL, this);
        return error;
    }

    /// <summary>
    /// Creates a new empty gameObject with a copy of the given registered AudioSource entry component attached to it.
    /// </summary>
    /// <param name="name">Name of the newly created gameObject.</param>
    /// <param name="position">Position the newly created gameObject should be created at.</param>
    /// <param name="parentSource">Parent AudioSource the settings should be copied from.</param>
    /// <param name="newSource">New child AudioSource that should be created and the settings copied into.</param>
    /// <returns>AudioError, showing wheter and how creating a new empty gameObject failed.</returns>
    private AudioError CreateEmptyGameObject(string name, Vector3 position, AudioSource parentSource, out AudioSource newSource) {
        logger.Log("Attempting to create empty GameObject", LoggingLevel.INTERMEDIATE, LoggingType.NORMAL, this);
        // Create new empty gameObject, at the given position.
        var newGameObject = new GameObject(name);
        logger.Log("Parent empty GameObject to the GameObject the AudioManager resides on", LoggingLevel.VERBOSE, LoggingType.NORMAL, this);
        // Set the parent of the newly created gameObject to the AudioManager.
        newGameObject.transform.SetParent(this.transform);
        logger.Log("Setting position of the newly created GameObject to the given position", LoggingLevel.VERBOSE, LoggingType.NORMAL, this);
        // Set the position of the newly created gameObject to the given position.
        newGameObject.transform.position = position;
        return AttachAudioSourceCopy(parentSource, out newSource, newGameObject);
    }

    /// <summary>
    /// Creates a new empty audioSource which is a copy of the given parentSource and attaches it to the given gameObject.
    /// </summary>
    /// <param name="parentSource">Parent AudioSource the settings should be copied from.</param>
    /// <param name="newSource">New child AudioSource that should be created and the settings copied into.</param>
    /// <param name="newSource">GameObject the new audioSource should be attached too..</param>
    /// <returns>AudioError, showing wheter and how attching a audioSource to the given gameobject failed.</returns>
    private AudioError AttachAudioSourceCopy(AudioSource parentSource, out AudioSource newSource, GameObject newGameObject) {
        logger.Log("Attempting to attach a new AudioSource to the given GameObject", LoggingLevel.INTERMEDIATE, LoggingType.NORMAL, this);
        // Add audioSource component to the newly created gameObject.
        newSource = newGameObject.AddComponent<AudioSource>();
        // Copy the values of our respective parent audioSource into the just created child audioSource object.
        return CopyAudioSourceSettings(newSource, parentSource);
    }

    /// <summary>
    /// Plays the sound with the given name once at a 3D position in space.
    /// </summary>
    /// <param name="parentSource">Parent source that the copied childSource was created from or should be created from.</param>
    /// <param name="position">Position we want to create an empty gameObject and play the given sound at.</param>
    /// <param name="oneShot">Wheter the AudioSource.PlayOneShot or AudioSource.PlayOneShot should be called.</param>
    /// <returns>AudioError, showing wheter and how playing the sound at the given position once failed.</returns>
    private AudioError PlayAt3DPosition(AudioSource parentSource, Vector3 position, bool oneShot, [CallerMemberName] string memberName = "") {
        logger.Log("Attempting to play given registered AudioSource entry at a 3D position", LoggingLevel.INTERMEDIATE, LoggingType.NORMAL, this);
        AudioError error = AudioError.OK;
        logger.Log("Checking if a 3D method was called previously with the given registered AudioSource", LoggingLevel.VERBOSE, LoggingType.NORMAL, this);
        // Check if the parentChildDirectory has already created a dictionary with the key being the given parentSource.
        if (parentChildDictionary.TryGetValue(parentSource, out var childDictionary)) {
            logger.Log("Checking if this 3D method was called previously", LoggingLevel.VERBOSE, LoggingType.NORMAL, this);
            // Check if the given childDictionary contains a key value pair that was created in the method with the given name.
            if (childDictionary.TryGetValue(memberName, out var childSource)) {
                // If it was, simply update the AudioSource component parent position, which is the previously created empty gameObject.
                error = CopyAudioSourceSettings(childSource, parentSource);
                logger.Log("Setting position of empty gameObject created previously containing our childSource", LoggingLevel.VERBOSE, LoggingType.NORMAL, this);
                childSource.transform.position = position;
                PlayOrPlayOneShot(childSource, oneShot);
                return error;
            }

            // If it wasn't, create a new empty gameobject and attach a copy of the parentSource object to it.
            error = CreateEmptyGameObject(memberName, position, parentSource, out AudioSource newChildSource);
            logger.Log("Register newly created empty gameObject with the AudioManager", LoggingLevel.VERBOSE, LoggingType.NORMAL, this);
            // Then add that to our already existing dictionary with the key being the name of the method that called this method.
            childDictionary.Add(memberName, newChildSource);
            PlayOrPlayOneShot(newChildSource, oneShot);
            return error;
        }

        error = CreateEmptyGameObject(memberName, position, parentSource, out AudioSource newSource);
        // Check if copying settings was successfull.
        if (error != AudioError.OK) {
            return error;
        }
        logger.Log("Create new registry containing our newly created empty gameObject", LoggingLevel.VERBOSE, LoggingType.NORMAL, this);
        // Create a new childDictionary with the key being the name of the method that called this method and
        // the value being the newSource that contains the copied settings of the parentSource object.
        var newChildDictionary = new Dictionary<string, AudioSource>() { { memberName, newSource } };
        logger.Log("Register newly created registry with the AudioManager", LoggingLevel.VERBOSE, LoggingType.NORMAL, this);
        // Add the newly created audioSource to our parentChildDictionary.
        parentChildDictionary.Add(parentSource, newChildDictionary);
        PlayOrPlayOneShot(newSource, oneShot);
        logger.Log("Playing registered AudioSource entry at 3D position successfull", LoggingLevel.HIGH, LoggingType.NORMAL, this);
        return error;
    }

    /// <summary>
    /// Plays the sound with the given name attached to a GameObject.
    /// </summary>
    /// <param name="parentSource">Parent source that the copied childSource was created from or should be created from.</param>
    /// <param name="gameObject">GameObject we want to attach our sound too.</param>
    /// <param name="oneShot">Wheter the AudioSource.PlayOneShot or AudioSource.PlayOneShot should be called.</param>
    /// <returns>AudioError, showing wheter and how playing the sound attached to the given gameobject failed.</returns>
    private AudioError PlayAttachedToGameObject(AudioSource parentSource, GameObject gameObject, bool oneShot, [CallerMemberName] string memberName = "") {
        logger.Log("Attempting to play given registered AudioSource entry attached to a GameObject", LoggingLevel.INTERMEDIATE, LoggingType.NORMAL, this);
        AudioError error = AudioError.OK;
        logger.Log("Checking if a 3D method was called previously with the given registered AudioSource", LoggingLevel.VERBOSE, LoggingType.NORMAL, this);
        // Check if the parentChildDirectory has already created a dictionary with the key being the given parentSource.
        if (parentChildDictionary.TryGetValue(parentSource, out Dictionary<string, AudioSource> childDictionary)) {
            // Check if the given childDictionary contains a key value pair that was created in the method with the given name and
            // if it was, check if the gameObject is still the same or if we need to copy to another gameObject.
            if (childDictionary.TryGetValue(memberName, out AudioSource childSource) && gameObject == childSource.gameObject) {
                error = CopyAudioSourceSettings(childSource, parentSource);
                PlayOrPlayOneShot(childSource, oneShot);
                return error;
            }

            // If it wasn't, create a new empty gameobject and attach a copy of the parentSource object to it.
            error = AttachAudioSourceCopy(parentSource, out AudioSource newChildSource, gameObject);
            logger.Log("Register newly created AudioSource copy with the AudioManager", LoggingLevel.VERBOSE, LoggingType.NORMAL, this);
            // Then add that to our already existing dictionary with the key being the name of the method that called this method.
            childDictionary.Add(memberName, newChildSource);
            PlayOrPlayOneShot(newChildSource, oneShot);
            return error;
        }

        error = AttachAudioSourceCopy(parentSource, out AudioSource newSource, gameObject);
        // Check if copying settings was successfull.
        if (error != AudioError.OK) {
            return error;
        }
        logger.Log("Create new registry containing our newly created AudioSource copy", LoggingLevel.VERBOSE, LoggingType.NORMAL, this);
        // Create a new childDictionary with the key being the name of the method that called this method and
        // the value being the newSource that contains the copied settings of the parentSource object.
        var newChildDictionary = new Dictionary<string, AudioSource>() { { memberName, newSource } };
        logger.Log("Register newly created registry with the AudioManager", LoggingLevel.VERBOSE, LoggingType.NORMAL, this);
        // Add the newly created audioSource to our parentChildDictionary.
        parentChildDictionary.Add(parentSource, newChildDictionary);
        PlayOrPlayOneShot(newSource, oneShot);
        logger.Log("Playing registered AudioSource entry attached to GameObject successfull", LoggingLevel.HIGH, LoggingType.NORMAL, this);
        return error;
    }

    /// <summary>
    /// Calls either AudioSource.PlayOneShot or AudioSource.Play.
    /// </summary>
    /// <param name="childSource">ÂudioSource that we want to start playing.</param>
    /// <param name="oneShot">Wheter the AudioSource.PlayOneShot or AudioSource.PlayOneShot should be called.</param>
    private void PlayOrPlayOneShot(AudioSource childSource, bool oneShot) {
        if (oneShot) {
            logger.Log("Starting to play the given registered AudioSource entry once", LoggingLevel.VERBOSE, LoggingType.NORMAL, this);
            childSource.PlayOneShot(childSource.clip);
            return;
        }

        logger.Log("Starting to play the given registered AudioSource entry", LoggingLevel.VERBOSE, LoggingType.NORMAL, this);
        childSource.Play();
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

using AudioManager.Service;
using System;
using UnityEngine;
using UnityEngine.Audio;

namespace AudioManager.Locator {
    public interface IAudioManager {
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
        /// <returns>AudioError, showing wheter and how adding a 2D sound from the given path with the given settings failed.</returxns>
        public AudioError AddSoundFromPath(string name, string path, float volume = 1f, float pitch = 1f, bool loop = false, AudioSource source = null, AudioMixerGroup mixerGroup = null);

        /// <summary>
        /// Plays the sound with the given name.
        /// </summary>
        /// <param name="name">Name of the sound.</param>
        /// <returns>AudioError, showing wheter and how playing the sound failed.</returns>
        public AudioError Play(string name);

        /// <summary>
        /// Plays the sound with the given name starting at the given startTime in the sound.
        /// </summary>
        /// <param name="name">Name of the sound.</param>
        /// <param name="startTime">Time we want to start playing the sound at in seconds.</param>
        /// <returns>AudioError, showing wheter and how playing the sound from the given startTime failed.</returns>
        public AudioError PlayAtTimeStamp(string name, float startTime);

        /// <summary>
        /// Gets the current playback position of the given sound in seconds.
        /// </summary>
        /// <param name="name">Name of the sound.</param>
        /// <returns>
        /// ValueDataError, where the value (gettable with Value), is the current playback position of the given sound in seconds
        /// and where the error (gettable with Error) is an integer representing the AudioError Enum, 
        /// showing wheter and how getting the current playback position of the sound failed.
        /// </returns>
        public ValueDataError<float> GetPlaybackPosition(string name);

        /// <summary>
        /// Plays the sound with the given name at a 3D position in space.
        /// </summary>
        /// <param name="name">Name of the sound.</param>
        /// <param name="position">Position we want to create an empty gameObject and play the given sound at.</param>
        /// <returns>AudioError, showing wheter and how playing the sound at the given position failed.</returns>
        public AudioError PlayAt3DPosition(string name, Vector3 position);

        /// <summary>
        /// Plays the sound with the given name once at a 3D position in space.
        /// Multiple instances of the same sound can be run at the same time with this method.
        /// </summary>
        /// <param name="name">Name of the sound.</param>
        /// <param name="position">Position we want to create an empty gameObject and play the given sound at.</param>
        /// <returns>AudioError, showing wheter and how playing the sound at the given position once failed.</returns>
        public AudioError PlayOneShotAt3DPosition(string name, Vector3 position);

        /// <summary>
        /// Plays the sound with the given name attached to a GameObject.
        /// </summary>
        /// <param name="name">Name of the sound.</param>
        /// <param name="gameObject">GameObject we want to attach our sound too.</param>
        /// <returns>AudioError, showing wheter and how playing the sound attached to the given gameobject once failed.</returns>
        public AudioError PlayAttachedToGameObject(string name, GameObject gameObject);

        /// <summary>
        /// Plays the sound with the given name attached to a GameObject.
        /// Multiple instances of the same sound can be run at the same time with this method.
        /// </summary>
        /// <param name="name">Name of the sound.</param>
        /// <param name="gameObject">GameObject we want to attach our sound too.</param>
        /// <returns>AudioError, showing wheter and how playing the sound attached to the given gameobject failed.</returns>
        public AudioError PlayOneShotAttachedToGameObject(string name, GameObject gameObject);

        /// <summary>
        /// Plays the sound with the given name after the given delay time.
        /// </summary>
        /// <param name="name">Name of the sound.</param>
        /// <param name="delay">Delay until sound is played.</param>
        /// <returns>AudioError, showing wheter and how playing the sound after the given amount of time failed.</returns>
        public AudioError PlayDelayed(string name, float delay);

        /// <summary>
        /// Plays the sound with the given name once.
        /// Multiple instances of the same sound can be run at the same time with this method.
        /// </summary>
        /// <param name="name">Name of the sound.</param>
        /// <returns>AudioError, showing wheter and how playing the sound once failed.</returns>
        public AudioError PlayOneShot(string name);

        /// <summary>
        /// Sets the pitch of the given sound to random value between the given min -and max values.
        /// </summary>
        /// <param name="name">Name of the sound.</param>
        /// <param name="minPitch">Minimum amount of pitch the sound can be played at.</param>
        /// <param name="minPitch">Maximum amount of pitch the sound can be played at.</param>
        /// <returns>AudioError, showing wheter and how chaging the pitch failed.</returns>
        public AudioError ChangePitch(string name, float minPitch, float maxPitch);

        /// <summary>
        /// Plays the sound with the given name after the given delay time.
        /// Additionally buffer time is added to the waitTime to prepare the playback and fetch it from media.
        /// </summary>
        /// <param name="name">Name of the sound.</param>
        /// <param name="time">Delay until sound is played.</param>
        /// <returns>AudioError, showing wheter and how playing the sound after the given amount of time failed.</returns>
        public AudioError PlayScheduled(string name, double time);

        /// <summary>
        /// Stops the sound with the given name.
        /// </summary>
        /// <param name="name">Name of the sound.</param>
        /// <returns>AudioError, showing wheter and how stopping the sound failed.</returns>
        public AudioError Stop(string name);

        /// <summary>
        /// Mutes or Unmutes the sound with the given name.
        /// </summary>
        /// <param name="name">Name of the sound.</param>
        /// <returns>AudioError, showing wheter and how muting or unmuting the sound failed.</returns>
        public AudioError ToggleMute(string name);

        /// <summary>
        /// Pauses or Unpauses the sound with the given name.
        /// </summary>
        /// <param name="name">Name of the sound.</param>
        /// <returns>AudioError, showing wheter and how pausing or unpausing the sound failed.</returns>
        public AudioError TogglePause(string name);

        /// <summary>
        /// Subscribes the given callback, so that it will be called with the given name and remaining time as a parameter,
        /// as soon as the sound only has the given remainingTime left to play until it finishes.
        /// </summary>
        /// <param name="name">Name of the sound.</param>
        /// <param name="remainingTime">Amount of remaining playback time in seconds, we want to call the callback at.</param>
        /// <param name="callback">Callback that should be called, once the sound only has the given amount of time left.</param>
        /// <returns>AudioError, showing wheter and how subscribing the callback failed.</returns>
        public AudioError SubscribeAudioFinished(string name, float remainingTime, Action<string, float> callback);

        /// <summary>
        /// Returns the progress of the sound with the given name from 0 to 1 where 1 is fully completed.
        /// </summary>
        /// <param name="name">Name of the sound.</param>
        /// <returns>
        /// ValueDataError, where the value (gettable with Value), is the progress of the given sound, which is a float from 0 to 1
        /// and where the error (gettable with Error) is an integer representing the AudioError Enum, 
        /// showing wheter and how getting the current playback position of the sound failed.
        /// </returns>
        public ValueDataError<float> GetProgress(string name);

        /// <summary>
        /// Gets the corresponding source to the sound with the given name or if we found multiple.
        /// </summary>
        /// <param name="name">Name of the sound.</param>
        /// <param name="source">Variale source should be copied into.</param>
        /// <returns>AudioError, showing wheter and how getting the source of the given sound failed.</returns>
        public AudioError TryGetSource(string name, out AudioSource source);

        /// <summary>
        /// Changes the pitch of the sound with the given name over the given amount of time to the given endValue.
        /// </summary>
        /// <param name="name">Name of the sound.</param>
        /// <param name="endValue">Value we wan't to have at the end.</param>
        /// <param name="waitTime">Total time needed to reach the given endValue.</param>
        /// <param name="granularity">Amount of steps that will be taken to decrease to the endValue (Setting to high is not advised).</param>
        /// <returns>AudioError, showing wheter and how changing the pitch of the given sound failed.</returns>
        public AudioError LerpPitch(string name, float endValue, float waitTime = 1f, float granularity = 5f);

        /// <summary>
        /// Changes the volume of the sound with the given name over the given amount of time to the given endValue.
        /// </summary>
        /// <param name="name">Name of the sound.</param>
        /// <param name="endValue">Value we wan't to have at the end.</param>
        /// <param name="waitTime">Total time needed to reach the given endValue.</param>
        /// <param name="granularity">Amount of steps that will be taken to decrease to the endValue (Setting to high is not advised).</param>
        /// <returns>AudioError, showing wheter and how changing the volume of the given sound failed.</returns>
        public AudioError LerpVolume(string name, float endValue, float waitTime = 1f, float granularity = 5f);

        /// <summary>
        /// Changes the value of the given exposed parameter for the complete AudioMixerGroup of the given sound to the given newValue.
        /// </summary>
        /// <param name="name">Name of the sound.</param>
        /// <param name="exposedParameterName">Name of the exposed parameter we want to change.</param>
        /// <param name="newValue">Value we want to set the exposed parameter to.</param>
        /// <returns>AudioError, showing wheter and how changing the given exposed parameter for the complete AudioMixerGroup of the given sound failed.</returns>
        public AudioError ChangeGroupValue(string name, string exposedParameterName, float newValue);

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
        public ValueDataError<float> GetGroupValue(string name, string exposedParameterName);

        /// <summary>
        /// Resets the value of the given exposed parameter for the complete AudioMixerGroup of the given sound to the default value.
        /// </summary>
        /// <param name="name">Name of the sound.</param>
        /// <param name="exposedParameterName">Name of the exposed parameter we want to reset.</param>
        /// <returns>AudioError, showing wheter and how reseting the given exposed parameter for the complete AudioMixerGroup of the given sound failed.</returns>
        public AudioError ResetGroupValue(string name, string exposedParameterName);

        /// <summary>
        /// Changes the value of the given exposed parameter for the complete AudioMixerGroup of the given sound over the given amount of time to the given endValue.
        /// </summary>
        /// <param name="name">Name of the sound.</param>
        /// <param name="exposedParameterName">Name of the exposed parameter we want to change.</param>
        /// <param name="endValue">Value we wan't to have at the end.</param>
        /// <param name="waitTime">Total time needed to reach the given endValue.</param>
        /// <param name="granularity">Amount of steps that will be taken to decrease to the endValue (Setting to high is not advised).</param>
        /// <returns>AudioError, showing wheter and how changing the given exposed parameter for the complete AudioMixerGroup of the given sound failed.</returns>
        public AudioError LerpGroupValue(string name, string exposedParameterName, float endValue, float waitTime = 1f, float granularity = 5f);

        /// <summary>
        /// Remove the AudioMixerGroup from the sound with the given name,
        /// so that it isn't influenced by the settings in it anymore.
        /// </summary>
        /// <param name="name">Name of the sound.</param>
        /// <returns>AudioError, showing wheter and how removing the AudioMixerGroup failed.</returns>
        public AudioError RemoveGroup(string name);

        /// <summary>
        /// Adding the AudioMixerGroup to the sound with the given name,
        /// so that it is influenced by the settings in it.
        /// </summary>
        /// <param name="name">Name of the sound.</param>
        /// <returns>AudioError, showing wheter and how adding the AudioMixerGroup failed.</returns>
        public AudioError AddGroup(string name, AudioMixerGroup mixerGroup);

        /// <summary>
        /// Remove a sound with the given name from the AudioManager.
        /// </summary>
        /// <param name="name">Name of the sound.</param>
        /// <returns>AudioError, showing wheter and how removing the sound failed.</returns>
        public AudioError RemoveSound(string name);

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
        public AudioError Set3DAudioOptions(string name, float minDistance = 1f, float maxDistance = 500f, float spatialBlend = 1f, float spread = 0f, float dopplerLevel = 1f, AudioRolloffMode rolloffMode = AudioRolloffMode.Logarithmic);

        /// <summary>
        /// Sets the startTime of the given sound.
        /// Does not get reset when playing the sound again use PlayAtTimeStamp for that.
        /// </summary>
        /// <param name="name">Name of the sound.</param>
        /// <param name="startTime">Moment in the sound we want to start playing at in seconds.</param>
        /// <returns>AudioError, showing wheter and how setting the start time failed.</returns>
        public AudioError SetStartTime(string name, float startTime);
    }
}

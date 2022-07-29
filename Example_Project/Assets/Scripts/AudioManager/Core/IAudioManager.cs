using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace AudioManager.Core {
    /// <summary>
    /// Subscribable callback that gets called when the given progress has been achieved or passed in the songs runtime.
    /// </summary>
    /// <param name="name">Name of the registered sound the callback has been called for.</param>
    /// <param name="progress">Point in the songs playtime from 0 to 1 we should call the callback at.</param>
    /// <param name="child">Object <see cref="ChildType"/> that called this method. Will always be <see cref="ChildType.PARENT"/>, besides when using with 3D methods.</param>
    /// <returns><see cref="AudioError"/>, showing wheter and how we should subscribe the ProgressCoroutineCallback again for the same progress.</returns>
    public delegate ProgressResponse ProgressCoroutineCallback(string name, float progress, ChildType child);
    public interface IAudioManager {
        /// <summary>
        /// Adds given 2D sound with the given settings to the possible playable sounds,
        /// creates a new <see cref="AudioSource"/> object if not already done so with the given settings
        /// and appends it to the <see cref="GameObject"/> that was passed to this class in the constructor.
        /// If 3D functionality wants to be added additionaly call the <see cref="Set3DAudioOptions"/> method.
        /// </summary>
        /// <param name="name">Name the new sound should have.</param>
        /// <param name="audioPath">Path to the clip we want to add to the new sound in the Resource folder.</param>
        /// <param name="volume">Volume we want the new sound to have.</param>
        /// <param name="pitch">Pitch we want the new sound to have.</param>
        /// <param name="loop">Defines wheter we want to repeat the new sound after completing it or not.</param>
        /// <param name="source">Source we want to add to the new sound.</param>
        /// <param name="mixerGroup">Mixer group the sound is influenced by.</param>
        /// <returns><see cref="AudioError"/>, showing wheter and how adding a 2D sound from the given path with the given settings failed.</returns>
        public AudioError AddSoundFromPath(string name, string audioPath, float volume = Constants.DEFAULT_VOLUME, float pitch = Constants.DEFAULT_PITCH, bool loop = Constants.DEFAULT_LOOP, AudioSource source = Constants.DEFAULT_SOURCE, AudioMixerGroup mixerGroup = Constants.DEFAULT_GROUP);

        /// <summary>
        /// Returns an enumerable to the underlying list of all registered sound names. Can be used to call methods like <see cref="LerpVolume"/> for each registered sound.
        /// </summary>
        /// <returns>Enumerable of all underlying registered sound names.</returns>
        public IEnumerable<string> GetEnumerator();

        /// <summary>
        /// Plays the sound with the given <see cref="ChildType"/>.
        /// </summary>
        /// <param name="name">Name of the registered sound.</param>
        /// <param name="child">Child that we want to call this method on.</param>
        /// <returns><see cref="AudioError"/>, showing wheter and how playing the sound failed.</returns>
        public AudioError Play(string name, ChildType child = Constants.DEFAULT_CHILD_TYPE);

        /// <summary>
        /// Plays the sound with the given <see cref="ChildType"/>, starting at the given startTime in the sound
        /// </summary>
        /// <param name="name">Name of the registered sound.</param>
        /// <param name="startTime">Time we want to start playing the sound at in seconds.</param>
        /// <param name="child">Child that we want to call this method on.</param>
        /// <returns><see cref="AudioError"/>, showing wheter and how playing the sound from the given startTime failed.</returns>
        public AudioError PlayAtTimeStamp(string name, float startTime, ChildType child = Constants.DEFAULT_CHILD_TYPE);

        /// <summary>
        /// Gets the current playback position of the given sound and the given <see cref="ChildType"/> in seconds.
        /// </summary>
        /// <param name="name">Name of the registered sound.</param>
        /// <param name="time">Variable the playback position in seconds will be copied into (<see cref="float.NaN"/> on failure).</param>
        /// <param name="child">Child that we want to call this method on.</param>
        /// <returns><see cref="AudioError"/>, showing wheter and how getting the current playback position of the sound failed.</returns>
        public AudioError GetPlaybackPosition(string name, out float time, ChildType child = Constants.DEFAULT_CHILD_TYPE);

        /// <summary>
        /// Sets the given direction the song with the given <see cref="ChildType"/> should be played in. A given pitch of 0 or more means it is a normal song and should just be played with the given pitch value from the start.
        /// Less than 0 means that the song will play in reverse from the end of the song tough.
        /// </summary>
        /// <param name="name">Name of the registered sound.</param>
        /// <param name="pitch">Pitch that shows in wich direction and with wich speed to play the song in (Reverse, Normal).</param>
        /// <param name="child">Child that we want to call this method on.</param>
        /// <returns><see cref="AudioError"/>, showing wheter and how setting the playback direction failed.</returns>
        public AudioError SetPlaybackDirection(string name, float pitch = Constants.DEFAULT_REVERSE_PITCH, ChildType child = Constants.DEFAULT_CHILD_TYPE);

        /// <summary>
        /// Registers a new child sound at a 3D position in space, so it can later be referenced via. the corresponding <see cref="ChildType"/> value.
        /// </summary>
        /// <param name="name">Name of the registered sound.</param>
        /// <param name="position">Position we want to create an empty <see cref="GameObject"/> on.</param>
        /// <param name="child">Variable the created <see cref="ChildType"/> will be copied into.</param>
        /// <returns><see cref="AudioError"/>, showing wheter and how registering the child sound at the given position failed.</returns>
        public AudioError RegisterChildAt3DPos(string name, Vector3 position, out ChildType child);

        /// <summary>
        /// Registers a new child sound attached to the given <see cref="GameObject"/>, so it can later be referenced via. the corresponding <see cref="ChildType"/> value.
        /// </summary>
        /// <param name="name">Name of the registered sound.</param>
        /// <param name="attachGameObject"><see cref="GameObject"/> we want to attach our child sound too.</param>
        /// <param name="child">Variable the created <see cref="ChildType"/> will be copied into.</param>
        /// <returns><see cref="AudioError"/>, showing wheter and how registering the child sound attached to the given <see cref="GameObject"/> failed.</returns>
        public AudioError RegisterChildAttachedToGo(string name, GameObject attachGameObject, out ChildType child);

        /// <summary>
        /// Deregisters and deletes the underlying AudioSource component of a previously registered child sound.
        /// </summary>
        /// <param name="name">Name of the registered sound.</param>
        /// <param name="child">Type of the child we want to deregister.</param>
        /// <returns><see cref="AudioError"/>, showing wheter and how registering the child sound attached to the given <see cref="GameObject"/> failed.</returns>
        public AudioError DeregisterChild(string name, ChildType child);

        /// <summary>
        /// Plays the sound with the given <see cref="ChildType"/> after the given delay time.
        /// </summary>
        /// <param name="name">Name of the registered sound.</param>
        /// <param name="delay">Delay until sound is played.</param>
        /// <param name="child">Child that we want to call this method on.</param>
        /// <returns><see cref="AudioError"/>, showing wheter and how playing the sound after the given amount of time failed.</returns>
        public AudioError PlayDelayed(string name, float delay, ChildType child = Constants.DEFAULT_CHILD_TYPE);

        /// <summary>
        /// Plays the sound with the given <see cref="ChildType"/> once.
        /// Multiple instances of the same sound can be run at the same time with this method.
        /// </summary>
        /// <param name="name">Name of the registered sound.</param>
        /// <param name="child">Child that we want to call this method on.</param>
        /// <returns><see cref="AudioError"/>, showing wheter and how playing the sound once failed.</returns>
        public AudioError PlayOneShot(string name, ChildType child = Constants.DEFAULT_CHILD_TYPE);

        /// <summary>
        /// Sets the pitch of the given sound and the given <see cref="ChildType"/> to random value between the given min -and max values.
        /// </summary>
        /// <param name="name">Name of the registered sound.</param>
        /// <param name="minPitch">Minimum amount of pitch the sound can be played at.</param>
        /// <param name="minPitch">Maximum amount of pitch the sound can be played at.</param>
        /// <param name="child">Child that we want to call this method on.</param>
        /// <returns><see cref="AudioError"/>, showing wheter and how changing the pitch failed.</returns>
        public AudioError ChangePitch(string name, float minPitch, float maxPitch, ChildType child = Constants.DEFAULT_CHILD_TYPE);

        /// <summary>
        /// Gets the clip length of the underlying <see cref="AudioClip"/> for the given sound and the given <see cref="ChildType"/>.
        /// </summary>
        /// <param name="name">Name of the registered sound.</param>
        /// <param name="time">Variable the clip length will be copied into (<see cref="float.NaN"/> on failure).</param>
        /// <param name="child">Child that we want to call this method on.</param>
        /// <returns><see cref="AudioError"/>, showing wheter and how getting the clip length of the sound failed.</returns>
        public AudioError GetClipLength(string name, out double length, ChildType child = Constants.DEFAULT_CHILD_TYPE);

        /// <summary>
        /// Plays the sound with the given <see cref="ChildType"/> at the exact given time in the future.
        /// Uses the Audio System’s DSP Time in the background meaning it is much more accurate,
        /// then <see cref="PlayDelayed"/>, <see cref="Time.time"/> and <see cref="WaitForSeconds"/>.
        /// </summary>
        /// <param name="name">Name of the registered sound.</param>
        /// <param name="time">Time until the given sound will be played.</param>
        /// <param name="child">Child that we want to call this method on.</param>
        /// <returns><see cref="AudioError"/>, showing wheter and how playing the sound after the given amount of time failed.</returns>
        public AudioError PlayScheduled(string name, double time, ChildType child = Constants.DEFAULT_CHILD_TYPE);

        /// <summary>
        /// Stops the sound with the given <see cref="ChildType"/>.
        /// </summary>
        /// <param name="name">Name of the registered sound.</param>
        /// <param name="child">Child that we want to call this method on.</param>
        /// <returns><see cref="AudioError"/>, showing wheter and how stopping the sound failed.</returns>
        public AudioError Stop(string name, ChildType child = Constants.DEFAULT_CHILD_TYPE);

        /// <summary>
        /// Mutes or Unmutes the sound with the given <see cref="ChildType"/>.
        /// </summary>
        /// <param name="name">Name of the registered sound.</param>
        /// <param name="child">Child that we want to call this method on.</param>
        /// <returns><see cref="AudioError"/>, showing wheter and how muting or unmuting the sound failed.</returns>
        public AudioError ToggleMute(string name, ChildType child = Constants.DEFAULT_CHILD_TYPE);

        /// <summary>
        /// Pauses or Unpauses the sound with the given <see cref="ChildType"/>.
        /// </summary>
        /// <param name="name">Name of the registered sound.</param>
        /// <param name="child">Child that we want to call this method on.</param>
        /// <returns><see cref="AudioError"/>, showing wheter and how pausing or unpausing the sound failed.</returns>
        public AudioError TogglePause(string name, ChildType child = Constants.DEFAULT_CHILD_TYPE);

        /// <summary>
        /// Subscribes the given <see cref="SourceChangedCallback"/>, so that it will be called when the underlying <see cref="AudioSourceWrapper"/> of the subscribed sound has been changed.
        /// </summary>
        /// <param name="name">Name of the registered sound.</param>
        /// <param name="callback">Callback that should be called, once the sound has changed.</param>
        /// <returns><see cref="AudioError"/>, showing wheter and how subscribing the callback failed.</returns>
        public AudioError SubscribeSourceChanged(string name, SourceChangedCallback callback);

        /// <summary>   
        /// Unsubscribes the previously via. <see cref="SubscribeSourceChanged"/> subscribed <see cref="SourceChangedCallback"/>,
        /// so that it will not be called anymore when the sound with the given underlying <see cref="AudioSourceWrapper"/> has been changed.
        /// </summary>
        /// <param name="name">Name of the registered sound.</param>
        /// <param name="callback">Callback that should not be called aynmore, once the sound has changed.</param>
        /// <returns><see cref="AudioError"/>, showing wheter and how unsubscribing the callback failed.</returns>
        public AudioError UnsubscribeSourceChanged(string name, SourceChangedCallback callback);

        /// <summary>
        /// Subscribes the given <see cref="ProgressCoroutineCallback"/>, so that it will be called as soon as the sound has reached the given progress point in the clips runtime.
        /// Depeding on the return value of the callback, it will be subscribed again for the next time that progress is hit.
        /// </summary>
        /// <param name="name">Name of the registered sound.</param>
        /// <param name="progress">Amount of progress from 0 to 1, we want to call the callback at.</param>
        /// <param name="callback">Callback that should be called, once the sound only has the given amount of time left.</param>
        /// <returns><see cref="AudioError"/>, showing wheter and how subscribing the callback failed.</returns>
        public AudioError SubscribeProgressCoroutine(string name, float progress, ProgressCoroutineCallback callback);

        /// <summary>
        /// Unsubscribes the previously via. <see cref="SubscribeProgressCoroutine"/> subscribed <see cref="ProgressCoroutineCallback"/>,
        /// so that it will not be called anymore when the sound reaches the given progress point in the clips runtime.
        /// </summary>
        /// <param name="name">Name of the registered sound.</param>
        /// <param name="progress">Amount of progress from 0 to 1, we want to call the callback at.</param>
        /// <returns><see cref="AudioError"/>, showing wheter and how unsubscribing the callback failed.</returns>
        public AudioError UnsubscribeProgressCoroutine(string name, float progress);

        /// <summary>
        /// Returns the progress (from 0 to 1 where 1 is fully completed) of the sound with the given <see cref="ChildType"/>.
        /// </summary>
        /// <param name="name">Name of the registered sound.</param>
        /// <param name="progress">Variable the progress will be copied into (<see cref="float.NaN"/> on failure).</param>
        /// <param name="child">Child that we want to call this method on.</param>
        /// <returns><see cref="AudioError"/>, showing wheter and how subscribing the callback failed, showing wheter and how getting the current progess of the sound failed.</returns>
        public AudioError GetProgress(string name, out float progress, ChildType child = Constants.DEFAULT_CHILD_TYPE);

        /// <summary>
        /// Gets the corresponding <see cref="AudioSourceWrapper"/> from the sound.
        /// </summary>
        /// <param name="name">Name of the registered sound.</param>
        /// <param name="source">Variable source should be copied into.</param>
        /// <returns><see cref="AudioError"/>, showing wheter and how getting the source of the given sound failed.</returns>
        public AudioError TryGetSource(string name, out AudioSourceWrapper source);

        /// <summary>
        /// Changes the pitch of the sound with the given <see cref="ChildType"/>, over the given amount of time to the given endValue.
        /// </summary>
        /// <param name="name">Name of the registered sound.</param>
        /// <param name="endValue">Value we want to have at the end.</param>
        /// <param name="duration">Total time needed to reach the given endValue.</param>
        /// <param name="child">Child that we want to call this method on.</param>
        /// <returns><see cref="AudioError"/>, showing wheter and how changing the pitch of the given sound failed.</returns>
        public AudioError LerpPitch(string name, float endValue, float duration = Constants.DEFAULT_DURATION, ChildType child = Constants.DEFAULT_CHILD_TYPE);

        /// <summary>
        /// Changes the volume of the sound with the given <see cref="ChildType"/>, over the given amount of time to the given endValue.
        /// </summary>
        /// <param name="name">Name of the registered sound.</param>
        /// <param name="endValue">Value we want to have at the end.</param>
        /// <param name="duration">Total time needed to reach the given endValue.</param>
        /// <param name="child">Child that we want to call this method on.</param>
        /// <returns><see cref="AudioError"/>, showing wheter and how changing the volume of the given sound failed.</returns>
        public AudioError LerpVolume(string name, float endValue, float duration = Constants.DEFAULT_DURATION, ChildType child = Constants.DEFAULT_CHILD_TYPE);

        /// <summary>
        /// Changes the value of the given exposed parameter for the complete <see cref="AudioMixerGroup"/> of the given sound to the given newValue.
        /// Be aware that some value like the volume of the <see cref="AudioMixerGroup"/> work on a logarithmic scale so to accurately change the value like expected use <see cref="Mathf.Log10()"/> on the value you pass beforehand.
        /// </summary>
        /// <param name="name">Name of the registered sound.</param>
        /// <param name="exposedParameterName">Name of the exposed parameter we want to change.</param>
        /// <param name="newValue">Value we want to set the exposed parameter to.</param>
        /// <returns><see cref="AudioError"/>, showing wheter and how changing the given exposed parameter for the complete <see cref="AudioMixerGroup"/> of the given sound failed.</returns>
        public AudioError ChangeGroupValue(string name, string exposedParameterName, float newValue);

        /// <summary>
        /// Gets the value of the given exposed parameter for the complete <see cref="AudioMixerGroup"/> of the given sound.
        /// </summary>
        /// <param name="name">Name of the registered sound.</param>
        /// <param name="exposedParameterName">Name of the exposed parameter we want to get.</param>
        /// <param name="currentValue">Variable the current exposed parameter will be copied into (<see cref="float.NaN"/> on failure).</param>
        /// <returns><see cref="AudioError"/>, showing wheter and how getting the current exposed parameter value failed.</returns>
        public AudioError GetGroupValue(string name, string exposedParameterName, out float currentValue);

        /// <summary>
        /// Resets the value of the given exposed parameter for the complete <see cref="AudioMixerGroup"/> of the given sound to the default value.
        /// </summary>
        /// <param name="name">Name of the registered sound.</param>
        /// <param name="exposedParameterName">Name of the exposed parameter we want to reset.</param>
        /// <returns><see cref="AudioError"/>, showing wheter and how reseting the given exposed parameter for the complete <see cref="AudioMixerGroup"/> of the given sound failed.</returns>
        public AudioError ResetGroupValue(string name, string exposedParameterName);

        /// <summary>
        /// Changes the value of the given exposed parameter for the complete <see cref="AudioMixerGroup"/> of the given sound over the given amount of time to the given endValue.
        /// Additionaly produces a more smooth result than <see cref="LerpVolume"/> or <see cref="LerpPitch"/> because of additonaly interpolation between frames applied by the AudioMixer.
        /// </summary>
        /// <param name="name">Name of the registered sound.</param>
        /// <param name="exposedParameterName">Name of the exposed parameter we want to change.</param>
        /// <param name="endValue">Value we want to have at the end.</param>
        /// <param name="duration">Total time needed to reach the given endValue.</param>
        /// <returns><see cref="AudioError"/>, showing wheter and how changing the given exposed parameter for the complete <see cref="AudioMixerGroup"/> of the given sound failed.</returns>
        public AudioError LerpGroupValue(string name, string exposedParameterName, float endValue, float duration = Constants.DEFAULT_DURATION);

        /// <summary>
        /// Removes the <see cref="AudioMixerGroup"/> from the sound with the given <see cref="ChildType"/>,
        /// so that it isn't influenced by the settings in it anymore.
        /// </summary>
        /// <param name="name">Name of the registered sound.</param>
        /// <param name="child">Child that we want to call this method on.</param>
        /// <returns><see cref="AudioError"/>, showing wheter and how removing the <see cref="AudioMixerGroup"/> failed.</returns>
        public AudioError RemoveGroup(string name, ChildType child = Constants.DEFAULT_CHILD_TYPE);

        /// <summary>
        /// Adding the <see cref="AudioMixerGroup"/> to the sound with the given <see cref="ChildType"/>,
        /// so that it is influenced by the settings in it.
        /// </summary>
        /// <param name="name">Name of the registered sound.</param>
        /// <param name="mixerGroup"><see cref="AudioMixerGroup"/> settings the sound should be influenced by.</param>
        /// <param name="child">Child that we want to call this method on.</param>
        /// <returns><see cref="AudioError"/>, showing wheter and how adding the <see cref="AudioMixerGroup"/> failed.</returns>
        public AudioError AddGroup(string name, AudioMixerGroup mixerGroup, ChildType child = Constants.DEFAULT_CHILD_TYPE);

        /// <summary>
        /// Deregisters and deletes the underlying AudioSource component of a sound with the AudioManager so it can't be played anymore.
        /// </summary>
        /// <param name="name">Name of the registered sound.</param>
        /// <returns><see cref="AudioError"/>, showing wheter and how removing the sound failed.</returns>
        public AudioError RemoveSound(string name);

        /// <summary>
        /// Enables and sets the possible 3D audio options for the sound with the given <see cref="ChildType"/>.
        /// </summary>
        /// <param name="name">Name of the registered sound.</param>
        /// <param name="minDistance">Distance that sound will not get louder at.</param>
        /// <param name="maxDistance">Distance that sound will still be hearable at.</param>
        /// <param name="child">Child that we want to call this method on.</param>
        /// <param name="spreadAngle">Sets the spread angles of the sound in degrees. (0f - 360f)</param>
        /// <param name="spatialBlend">Defines how much the Audio Source is affected by 3D space. (0f = 2D, 1f = 3D)</param>
        /// <param name="dopplerLevel">Defines Doppler Scale for the Audio Source. (0f - 5f)</param>
        /// <param name="rolloffMode">Sets how the Volume will be lowered over distance.</param>
        /// <returns><see cref="AudioError"/>, showing wheter and how setting the 3D audio options failed.</returns>
        public AudioError Set3DAudioOptions(string name, float minDistance = Constants.DEFAULT_MIN_DISTANCE, float maxDistance = Constants.DEFAULT_MAX_DISTANCE, ChildType child = Constants.DEFAULT_CHILD_TYPE, float spatialBlend = Constants.DEFAULT_BLEND, float spreadAngle = Constants.DEFAULT_ANGLE, float dopplerLevel = Constants.DEFAULT_DOPPLER, AudioRolloffMode rolloffMode = Constants.DEFAULT_MODE);

        /// <summary>
        /// Sets the startTime of the given sound with the given <see cref="ChildType"/>.
        /// Does not get reset when playing the sound again use <see cref="PlayAtTimeStamp(string, float)"/> for that.
        /// </summary>
        /// <param name="name">Name of the registered sound.</param>
        /// <param name="startTime">Moment in the sound we want to start playing at in seconds.</param>
        /// <param name="child">Child that we want to call this method on.</param>
        /// <returns><see cref="AudioError"/>, showing wheter and how setting the start time failed.</returns>
        public AudioError SetStartTime(string name, float startTime, ChildType child = Constants.DEFAULT_CHILD_TYPE);

        /// <summary>
        /// Skips the given sound with the given <see cref="ChildType"/> forwards or backwards for the given amount of time in seconds limited to the end of the song or the start of the song.
        /// </summary>
        /// <param name="name">Name of the registered sound.</param>
        /// <param name="time">Amount of time in seconds we want to advance the given track (Negative number skips backward, positive number skips forward).</param>
        /// <param name="child">Child that we want to call this method on.</param>
        /// <returns><see cref="AudioError"/>, showing wheter and how skipping forward or backward the current time failed.</returns>
        public AudioError SkipTime(string name, float time, ChildType child = Constants.DEFAULT_CHILD_TYPE);
    }
}

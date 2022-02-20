using AudioManager.Locator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Audio;

namespace AudioManager.Service {
    public class AudioManager : IAudioManager {
        // Readonly private member variables.
        private readonly GameObject m_parentGameObject;
        private readonly MonoBehaviour m_parentBehaviour;
        private readonly Transform m_parentTransform;
        private readonly AudioFinishedCallback m_resetStartTimeCallback;

        // Constant private member variables.
        // Max. progress of the sound still detactable in an IEnumerator.
        private const float MAX_PROGRESS = 0.99f;
        // Max. spatial blend value that still counts as 2D.
        private const float SPATIAL_BLEND_2D = 0f;
        // Min. granularity value that is still valid.
        private const float MIN_GRANULARITY = 1f;

        // Private member variables.
        private Dictionary<AudioSource, Dictionary<string, AudioSource>> m_parentChildDictionary = new Dictionary<AudioSource, Dictionary<string, AudioSource>>();
        private Dictionary<string, AudioSource> m_soundDictionary = new Dictionary<string, AudioSource>();

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="sounds">Dictionary of the registered AudioSource entries with the given name.</param>
        /// <param name="parentGameObject">Gameobject we want to parent new empty GameObject or AudioSource components to.</param>
        public AudioManager(Dictionary<string, AudioSource> sounds, GameObject parentGameObject) {
            // Cache the parentGameObject so we can still parent new empty gameObjects and AudioSources to something.
            m_parentGameObject = parentGameObject;
            // Cache the MonoBehaviour of the given parentGameObject,
            // this is done so that we can still play coroutines even tough this class itself isn't a MonoBehaviour.
            m_parentBehaviour = parentGameObject.GetComponent<MonoBehaviour>();
            // Cache the Transform of the given parentGameObject,
            // this is done so that we can attach newly created gameObject to it.
            m_parentTransform = parentGameObject.transform;
            // Cache the given dictionary that contains all registered sounds so we can change their values and use them to play the acutal AudioSource.
            m_soundDictionary = sounds;
            // Initalize the callback called for PlayAtTimeStamp.
            m_resetStartTimeCallback = ResetStartTime;
        }

        public AudioError AddSoundFromPath(string name, string path, float volume, float pitch, bool loop, AudioSource source, AudioMixerGroup mixerGroup) {
            AudioError error = AudioError.OK;
            // Load sound clip from the Resource folder on the given path.
            var clip = Resources.Load<AudioClip>(path);

            // Check if the clip couldn't be loaded correctly.
            if (!clip) {
                error = AudioError.INVALID_PATH;
                return error;
            }
            else if (!source) {
                source = m_parentGameObject.AddComponent<AudioSource>();
            }

            error = AddSound(name, source);

            // Check if the clip could be added correctly.
            if (error != AudioError.OK) {
                return error;
            }

            error = Set2DAudioOptions(source, clip, mixerGroup, loop, volume, pitch);
            return error;
        }

        public AudioError Play(string name) {
            AudioError error = TryGetSource(name, out AudioSource source);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }

            source.Play();
            return error;
        }

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

            // Calls the given callback as soon as the song is finished.
            m_parentBehaviour.StartCoroutine(DetectCurrentProgressCoroutine(name, MAX_PROGRESS, m_resetStartTimeCallback));
            source.Play();
            return error;
        }

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

        public AudioError PlayAt3DPosition(string name, Vector3 position) {
            AudioError error = TryGetSource(name, out AudioSource parentSource);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }
            // Checks if 3D was even enabled in the spatialBlend.
            else if (parentSource.spatialBlend <= SPATIAL_BLEND_2D) {
                error = AudioError.CAN_NOT_BE_3D;
                return error;
            }

            return PlayAt3DPosition(parentSource, position, false);
        }

        public AudioError PlayOneShotAt3DPosition(string name, Vector3 position) {
            AudioError error = TryGetSource(name, out AudioSource parentSource);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }
            // Checks if 3D was even enabled in the spatialBlend.
            else if (parentSource.spatialBlend <= SPATIAL_BLEND_2D) {
                error = AudioError.CAN_NOT_BE_3D;
                return error;
            }

            return PlayAt3DPosition(parentSource, position, true);
        }

        public AudioError PlayAttachedToGameObject(string name, GameObject attachGameObject) {
            AudioError error = TryGetSource(name, out AudioSource parentSource);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }
            // Checks if 3D was even enabled in the spatialBlend.
            else if (parentSource.spatialBlend <= SPATIAL_BLEND_2D) {
                error = AudioError.CAN_NOT_BE_3D;
                return error;
            }

            return PlayAttachedToGameObject(parentSource, attachGameObject, false);
        }

        public AudioError PlayOneShotAttachedToGameObject(string name, GameObject gameObject) {
            AudioError error = TryGetSource(name, out AudioSource parentSource);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }
            // Checks if 3D was even enabled in the spatialBlend.
            else if (parentSource.spatialBlend <= SPATIAL_BLEND_2D) {
                error = AudioError.CAN_NOT_BE_3D;
                return error;
            }

            return PlayAttachedToGameObject(parentSource, gameObject, true);
        }

        public AudioError PlayDelayed(string name, float delay) {
            AudioError error = TryGetSource(name, out AudioSource source);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }

            source.PlayDelayed(delay);
            return error;
        }

        public AudioError PlayOneShot(string name) {
            AudioError error = TryGetSource(name, out AudioSource source);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }

            source.PlayOneShot(source.clip);
            return error;
        }

        public AudioError ChangePitch(string name, float minPitch, float maxPitch) {
            AudioError error = TryGetSource(name, out AudioSource source);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }

            source.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
            return error;
        }

        public AudioError PlayScheduled(string name, double time) {
            AudioError error = TryGetSource(name, out AudioSource source);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }

            source.PlayScheduled(time);
            return error;
        }

        public AudioError Stop(string name) {
            AudioError error = TryGetSource(name, out AudioSource source);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }

            source.Stop();
            return error;
        }

        public AudioError ToggleMute(string name) {
            AudioError error = TryGetSource(name, out AudioSource source);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }

            source.mute = !source.mute;
            return error;
        }

        public AudioError TogglePause(string name) {
            AudioError error = TryGetSource(name, out AudioSource source);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }

            // Check if the sound is playing right now.
            if (source.isPlaying) {
                source.Pause();
                return error;
            }

            source.UnPause();
            return error;
        }

        public AudioError SubscribeAudioFinished(string name, float remainingTime, AudioFinishedCallback callback) {
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
            m_parentBehaviour.StartCoroutine(DetectCurrentProgressCoroutine(name, progress, callback));
            return error;
        }

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

        public AudioError TryGetSource(string name, out AudioSource source) {
            AudioError error = AudioError.OK;

            // Check if the given sound name is in our m_soundDictionary.
            if (!m_soundDictionary.TryGetValue(name, out source)) {
                error = AudioError.DOES_NOT_EXIST;
            }
            // Check if the source is set.
            else if (!source) {
                error = AudioError.MISSING_SOURCE;
            }
            return error;
        }

        public AudioError LerpPitch(string name, float endValue, float waitTime, float granularity) {
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

            m_parentBehaviour.StartCoroutine(LerpPitchCoroutine(source, stepValue, stepTime, granularity));
            return error;
        }

        public AudioError LerpVolume(string name, float endValue, float waitTime, float granularity) {
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

            m_parentBehaviour.StartCoroutine(LerpVolumeCoroutine(source, stepValue, stepTime, granularity));
            return error;
        }

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
                valueDataError.Error = ((int)AudioError.MISSING_MIXER_GROUP);
                return valueDataError;
            }
            // Check if the AudioMixer parameter is exposed.
            else if (!source.outputAudioMixerGroup.audioMixer.GetFloat(exposedParameterName, out currentValue)) {
                valueDataError.Error = ((int)AudioError.MIXER_NOT_EXPOSED);
            }
            valueDataError.Value = currentValue;
            return valueDataError;
        }

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

        public AudioError LerpGroupValue(string name, string exposedParameterName, float endValue, float waitTime, float granularity) {
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

            m_parentBehaviour.StartCoroutine(LerpExposedParameterCoroutine(source.outputAudioMixerGroup.audioMixer, exposedParameterName, stepValue, stepTime, granularity));
            return error;
        }

        public AudioError RemoveGroup(string name) {
            AudioError error = TryGetSource(name, out AudioSource source);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }

            source.outputAudioMixerGroup = null;
            return error;
        }

        public AudioError AddGroup(string name, AudioMixerGroup mixerGroup) {
            AudioError error = TryGetSource(name, out AudioSource source);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }

            source.outputAudioMixerGroup = mixerGroup;
            return error;
        }

        public AudioError RemoveSound(string name) {
            AudioError error = AudioError.OK;

            // Check if the given sound name is in our m_soundDictionary.
            if (!m_soundDictionary.Remove(name)) {
                error = AudioError.DOES_NOT_EXIST;
            }
            return error;
        }

        public AudioError Set3DAudioOptions(string name, float minDistance, float maxDistance, float spatialBlend, float spread, float dopplerLevel, AudioRolloffMode rolloffMode) {
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
        /// Appends the given sounds AudioSource components to the dictionary.
        /// </summary>
        /// <param name="name">Name of the sound we want to register with the AudioManager.</param>
        /// <param name="source">AudioSource that contains the settings we want the sound to have.</param>
        /// <returns>AudioError, showing wheter and how adding a sound failed.</returns>
        private AudioError AddSound(string name, AudioSource source) {
            AudioError error = AudioError.OK;
            // Ensure there is not already a sound with the given name in our dictionary.
            if (m_soundDictionary.ContainsKey(name)) {
                error = AudioError.ALREADY_EXISTS;
                return error;
            }
            else if (!source) {
                error = AudioError.MISSING_SOURCE;
                return error;
            }
            m_soundDictionary.Add(name, source);
            return error;
        }

        /// <summary>
        /// Changes the pitch of the given sound with a certain waitTime after each de -or increase.
        /// </summary>
        /// <param name="source">Source of the AudioFile.</param>
        /// <param name="stepValue">How much we want to de -or increase the value by each step.</param>
        /// <param name="stepTime">Delay we want to have after each de -or increase.</param>
        /// <param name="steps">Amount of steps that will be taken to decrease to the endValue.</param>
        private IEnumerator LerpPitchCoroutine(AudioSource source, float stepValue, float stepTime, float steps) {
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
        private IEnumerator LerpVolumeCoroutine(AudioSource source, float stepValue, float stepTime, float steps) {
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
        private IEnumerator LerpExposedParameterCoroutine(AudioMixer mixer, string exposedParameterName, float stepValue, float stepTime, float steps) {
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
        private IEnumerator DetectCurrentProgressCoroutine(string name, float progress, AudioFinishedCallback callback) {
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
            Debug.Log((float)source.timeSamples / (float)source.clip.samples);
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
        /// Creates a new empty gameObject with a copy of the given registered AudioSource entry component attached to it.
        /// </summary>
        /// <param name="name">Name of the newly created gameObject.</param>
        /// <param name="position">Position the newly created gameObject should be created at.</param>
        /// <param name="parentSource">Parent AudioSource the settings should be copied from.</param>
        /// <param name="newSource">New child AudioSource that should be created and the settings copied into.</param>
        /// <returns>AudioError, showing wheter and how creating a new empty gameObject failed.</returns>
        private AudioError CreateEmptyGameObject(string name, Vector3 position, AudioSource parentSource, out AudioSource newSource) {
            // Create new empty gameObject, at the given position.
            var newGameObject = new GameObject(name);
            var newTransform = newGameObject.transform;
            // Set the parent of the newly created gameObject to the AudioManager.
            newTransform.SetParent(m_parentTransform);
            // Set the position of the newly created gameObject to the given position.
            newTransform.position = position;
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
            AudioError error = AudioError.OK;
            // Check if the parentChildDirectory has already created a dictionary with the key being the given parentSource.
            if (m_parentChildDictionary.TryGetValue(parentSource, out var childDictionary)) {
                // Check if the given childDictionary contains a key value pair that was created in the method with the given name.
                if (childDictionary.TryGetValue(memberName, out var childSource)) {
                    // If it was, simply update the AudioSource component parent position, which is the previously created empty gameObject.
                    error = CopyAudioSourceSettings(childSource, parentSource);
                    childSource.transform.position = position;
                    PlayOrPlayOneShot(childSource, oneShot);
                    return error;
                }

                // If it wasn't, create a new empty gameobject and attach a copy of the parentSource object to it.
                error = CreateEmptyGameObject(memberName, position, parentSource, out AudioSource newChildSource);
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
            // Create a new childDictionary with the key being the name of the method that called this method and
            // the value being the newSource that contains the copied settings of the parentSource object.
            var newChildDictionary = new Dictionary<string, AudioSource>() { { memberName, newSource } };
            // Add the newly created audioSource to our m_parentChildDictionary.
            m_parentChildDictionary.Add(parentSource, newChildDictionary);
            PlayOrPlayOneShot(newSource, oneShot);
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
            AudioError error = AudioError.OK;
            // Check if the parentChildDirectory has already created a dictionary with the key being the given parentSource.
            if (m_parentChildDictionary.TryGetValue(parentSource, out Dictionary<string, AudioSource> childDictionary)) {
                // Check if the given childDictionary contains a key value pair that was created in the method with the given name and
                // if it was, check if the gameObject is still the same or if we need to copy to another gameObject.
                if (childDictionary.TryGetValue(memberName, out AudioSource childSource) && gameObject == childSource.gameObject) {
                    error = CopyAudioSourceSettings(childSource, parentSource);
                    PlayOrPlayOneShot(childSource, oneShot);
                    return error;
                }

                // If it wasn't, create a new empty gameobject and attach a copy of the parentSource object to it.
                error = AttachAudioSourceCopy(parentSource, out AudioSource newChildSource, gameObject);
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
            // Create a new childDictionary with the key being the name of the method that called this method and
            // the value being the newSource that contains the copied settings of the parentSource object.
            var newChildDictionary = new Dictionary<string, AudioSource>() { { memberName, newSource } };
            // Add the newly created audioSource to our m_parentChildDictionary.
            m_parentChildDictionary.Add(parentSource, newChildDictionary);
            PlayOrPlayOneShot(newSource, oneShot);
            return error;
        }

        /// <summary>
        /// Calls either AudioSource.PlayOneShot or AudioSource.Play.
        /// </summary>
        /// <param name="childSource">ÂudioSource that we want to start playing.</param>
        /// <param name="oneShot">Wheter the AudioSource.PlayOneShot or AudioSource.PlayOneShot should be called.</param>
        private void PlayOrPlayOneShot(AudioSource childSource, bool oneShot) {
            if (oneShot) {
                childSource.PlayOneShot(childSource.clip);
                return;
            }

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
        private void ResetStartTime(string name, float remainingTime) {
            TryGetSource(name, out AudioSource source);
            // Stop the sound if it isn't set to looping,
            // this is done to ensure the sound doesn't replay,
            // when it is not set to looping.
            if (!source.loop) {
                Stop(name);
            }
            SetStartTime(name, 0f);
        }
    }
}

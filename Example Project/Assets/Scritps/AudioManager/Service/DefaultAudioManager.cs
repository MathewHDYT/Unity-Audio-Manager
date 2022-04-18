using AudioManager.Core;
using AudioManager.Helper;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Audio;

namespace AudioManager.Service {
    public class DefaultAudioManager : IAudioManager {
        // Private constant member variables.
        private const float nullValue = float.NaN;

        // Readonly private member variables.
        private readonly GameObject m_parentGameObject;
        private readonly MonoBehaviour m_parentBehaviour;
        private readonly Transform m_parentTransform;
        private readonly AudioFinishedCallback m_resetStartTimeCallback;

        // Private member variables.
        private Dictionary<AudioSource, Dictionary<string, AudioSource>> m_parentChildDictionary = new Dictionary<AudioSource, Dictionary<string, AudioSource>>();
        private Dictionary<string, AudioSource> m_soundDictionary = new Dictionary<string, AudioSource>();

        public DefaultAudioManager(Dictionary<string, AudioSource> sounds, GameObject parentGameObject) {
            m_resetStartTimeCallback = ResetStartTime;

            if (sounds is object) {
                m_soundDictionary = sounds;
            }

            if (parentGameObject) {
                m_parentGameObject = parentGameObject;
                m_parentBehaviour = m_parentGameObject.GetComponent<MonoBehaviour>();
                m_parentTransform = m_parentGameObject.transform;
            }
        }

        public AudioError AddSoundFromPath(string name, string path, float volume, float pitch, bool loop, AudioSource source, AudioMixerGroup mixerGroup) {
            AudioError error = AudioHelper.LoadAudioClipFromPath(path, out AudioClip clip);

            if (error != AudioError.OK) {
                return error;
            }
            else if (!m_parentGameObject) {
                return AudioError.MISSING_PARENT;
            }
            else if (!source) {
                AudioHelper.AddAudioSourceComponent(m_parentGameObject, out source);
            }

            error = source.Set2DAudioOptions(clip, mixerGroup, loop, volume, pitch);
            if (error != AudioError.OK) {
                return error;
            }

            error = RegisterAudioSource(name, source);
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
            else if (!m_parentBehaviour) {
                error = AudioError.MISSING_PARENT;
                return error;
            }

            error = SetStartTime(name, startTime);
            if (error != AudioError.OK) {
                return error;
            }

            float remainingTime = source.ConvertProgressIntoTimeStamp(Constants.MAX_PROGRESS);
            m_parentBehaviour.StartCoroutine(DetectCurrentProgressCoroutine(name, Constants.MAX_PROGRESS, remainingTime, m_resetStartTimeCallback));
            source.Play();
            return error;
        }

        public ValueDataError<float> GetPlaybackPosition(string name) {
            AudioError error = TryGetSource(name, out AudioSource source);
            ValueDataError<float> valueDataError = new ValueDataError<float>(nullValue, error);

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
            else if (parentSource.IsSound2D()) {
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
            else if (parentSource.IsSound2D()) {
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
            else if (parentSource.IsSound2D()) {
                error = AudioError.CAN_NOT_BE_3D;
                return error;
            }
            else if (!attachGameObject) {
                error = AudioError.INVALID_PARENT;
                return error;
            }

            return PlayAttachedToGameObject(parentSource, attachGameObject, false);
        }

        public AudioError PlayOneShotAttachedToGameObject(string name, GameObject attachGameObject) {
            AudioError error = TryGetSource(name, out AudioSource parentSource);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }
            else if (parentSource.IsSound2D()) {
                error = AudioError.CAN_NOT_BE_3D;
                return error;
            }
            else if (!attachGameObject) {
                error = AudioError.INVALID_PARENT;
                return error;
            }

            return PlayAttachedToGameObject(parentSource, attachGameObject, true);
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

            source.SetRandomPitch(minPitch, maxPitch);
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
            else if (!m_parentBehaviour) {
                error = AudioError.MISSING_PARENT;
                return error;
            }
            else if (!source.IsLengthValid(remainingTime)) {
                error = AudioError.INVALID_TIME;
                return error;
            }

            float progress = source.ConvertTimeStampIntoProgress(remainingTime);
            if (!AudioHelper.IsProgressValid(progress)) {
                error = AudioError.INVALID_PROGRESS;
                return error;
            }

            m_parentBehaviour.StartCoroutine(DetectCurrentProgressCoroutine(name, progress, remainingTime, callback));
            return error;
        }

        public ValueDataError<float> GetProgress(string name) {
            AudioError error = TryGetSource(name, out AudioSource source);
            ValueDataError<float> valueDataError = new ValueDataError<float>(nullValue, error);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return valueDataError;
            }

            valueDataError.Value = source.GetProgress();
            return valueDataError;
        }

        public AudioError TryGetSource(string name, out AudioSource source) {
            AudioError error = AudioError.OK;

            if (!TryGetRegisteredAudioSource(name, out source)) {
                error = AudioError.DOES_NOT_EXIST;
                return error;
            }

            error = source.IsSoundValid();
            return error;
        }

        public AudioError LerpPitch(string name, float endValue, float waitTime, int granularity) {
            AudioError error = TryGetSource(name, out AudioSource source);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }
            else if (source.IsSamePitch(endValue)) {
                error = AudioError.INVALID_END_VALUE;
                return error;
            }
            else if (!AudioHelper.IsGranularityValid(granularity)) {
                error = AudioError.INVALID_GRANULARITY;
                return error;
            }
            else if (!m_parentBehaviour) {
                error = AudioError.MISSING_PARENT;
                return error;
            }

            AudioHelper.GetStepValueAndTime(source.pitch, endValue, waitTime, granularity, out float stepValue, out float stepTime);
            m_parentBehaviour.StartCoroutine(LerpPitchCoroutine(source, stepValue, stepTime, granularity));
            return error;
        }

        public AudioError LerpVolume(string name, float endValue, float waitTime, int granularity) {
            AudioError error = TryGetSource(name, out AudioSource source);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }
            else if (source.IsSameVolume(endValue)) {
                error = AudioError.INVALID_END_VALUE;
                return error;
            }
            else if (!AudioHelper.IsGranularityValid(granularity)) {
                error = AudioError.INVALID_GRANULARITY;
                return error;
            }
            else if (!m_parentBehaviour) {
                error = AudioError.MISSING_PARENT;
                return error;
            }

            AudioHelper.GetStepValueAndTime(source.volume, endValue, waitTime, granularity, out float stepValue, out float stepTime);
            m_parentBehaviour.StartCoroutine(LerpVolumeCoroutine(source, stepValue, stepTime, granularity));
            return error;
        }

        public AudioError ChangeGroupValue(string name, string exposedParameterName, float newValue) {
            AudioError error = TryGetSource(name, out AudioSource source);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }
            else if (!source.IsAudioMixerGroupValid()) {
                error = AudioError.MISSING_MIXER_GROUP;
                return error;
            }
            error = source.TrySetGroupValue(exposedParameterName, newValue);
            return error;
        }

        public ValueDataError<float> GetGroupValue(string name, string exposedParameterName) {
            AudioError error = TryGetSource(name, out AudioSource source);
            float currentValue = nullValue;
            ValueDataError<float> valueDataError = new ValueDataError<float>(currentValue, error);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return valueDataError;
            }
            else if (!source.IsAudioMixerGroupValid()) {
                valueDataError.Error = AudioError.MISSING_MIXER_GROUP;
                return valueDataError;
            }
            valueDataError.Error = source.TryGetGroupValue(exposedParameterName, out currentValue);
            valueDataError.Value = currentValue;
            return valueDataError;
        }

        public AudioError ResetGroupValue(string name, string exposedParameterName) {
            AudioError error = TryGetSource(name, out AudioSource source);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }
            else if (!source.IsAudioMixerGroupValid()) {
                error = AudioError.MISSING_MIXER_GROUP;
                return error;
            }
            error = source.TryClearGroupValue(exposedParameterName);
            return error;
        }

        public AudioError LerpGroupValue(string name, string exposedParameterName, float endValue, float waitTime, int granularity) {
            AudioError error = TryGetSource(name, out AudioSource source);
            float currentValue = nullValue;

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }
            else if (!source.IsAudioMixerGroupValid()) {
                error = AudioError.MISSING_MIXER_GROUP;
                return error;
            }
            else if (!m_parentBehaviour) {
                error = AudioError.MISSING_PARENT;
                return error;
            }

            error = source.TryGetGroupValue(exposedParameterName, out currentValue);
            if (error != AudioError.OK) {
                return error;
            }
            else if (!AudioHelper.IsEndValueValid(currentValue, endValue)) {
                error = AudioError.INVALID_END_VALUE;
            }
            else if (!AudioHelper.IsGranularityValid(granularity)) {
                error = AudioError.INVALID_GRANULARITY;
                return error;
            }

            AudioHelper.GetStepValueAndTime(currentValue, endValue, waitTime, granularity, out float stepValue, out float stepTime);
            m_parentBehaviour.StartCoroutine(LerpExposedParameterCoroutine(source.outputAudioMixerGroup.audioMixer, exposedParameterName, stepValue, stepTime, granularity));
            return error;
        }

        public AudioError RemoveGroup(string name) {
            AudioError error = TryGetSource(name, out AudioSource source);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }

            source.SetAudioMixerGroup(null);
            return error;
        }

        public AudioError AddGroup(string name, AudioMixerGroup mixerGroup) {
            AudioError error = TryGetSource(name, out AudioSource source);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }

            source.SetAudioMixerGroup(mixerGroup);
            return error;
        }

        public AudioError RemoveSound(string name) {
            AudioError error = AudioError.OK;

            if (!DeregisterSound(name)) {
                error = AudioError.DOES_NOT_EXIST;
            }
            return error;
        }

        public AudioError Set3DAudioOptions(string name, float minDistance, float maxDistance, float spatialBlend, float spreadAngle, float dopplerLevel, AudioRolloffMode rolloffMode) {
            AudioError error = TryGetSource(name, out AudioSource source);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }

            error = source.Set3DAudioOptions(spatialBlend, dopplerLevel, spreadAngle, rolloffMode, minDistance, maxDistance);
            return error;
        }

        public AudioError SetStartTime(string name, float startTime) {
            AudioError error = TryGetSource(name, out AudioSource source);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }
            else if (!source.IsLengthValid(startTime)) {
                error = AudioError.INVALID_TIME;
                return error;
            }

            source.SetTime(startTime);
            return error;
        }

        //************************************************************************************************************************
        // Private Section
        //************************************************************************************************************************

        private AudioError RegisterAudioSource(string name, AudioSource source) {
            AudioError error = AudioError.OK;

            if (IsSoundRegistered(name)) {
                error = AudioError.ALREADY_EXISTS;
                return error;
            }

            error = source.IsSoundValid();
            if (error != AudioError.OK) {
                return error;
            }

            RegisterSound(name, source);
            return error;
        }

        private IEnumerator LerpPitchCoroutine(AudioSource source, float stepValue, float stepTime, int steps) {
            // Cache WaitForSeconds, done to ensure we don't needlessly allocate a new WaitForSeconds with the same value each time.
            var waitForStepTime = new WaitForSeconds(stepTime);
            // De -or increases the given pitch with the given amount of steps.
            for (; steps > 0; steps--) {
                yield return waitForStepTime;
                source.pitch += stepValue;
            }
            // Correct for float rounding errors.
            source.pitch = Mathf.Round(source.pitch * 100f) / 100f;
        }

        private IEnumerator LerpVolumeCoroutine(AudioSource source, float stepValue, float stepTime, int steps) {
            // Cache WaitForSeconds, done to ensure we don't needlessly allocate a new WaitForSeconds with the same value each time.
            var waitForStepTime = new WaitForSeconds(stepTime);
            // De -or increases the given pitch with the given amount of steps.
            for (; steps > 0; steps--) {
                yield return waitForStepTime;
                source.volume += stepValue;
            }
            // Correct for float rounding errors.
            source.volume = Mathf.Round(source.volume * 100f) / 100f;
        }

        private IEnumerator LerpExposedParameterCoroutine(AudioMixer mixer, string exposedParameterName, float stepValue, float stepTime, int steps) {
            // Cache WaitForSeconds, done to ensure we don't needlessly allocate a new WaitForSeconds with the same value each time.
            var waitForStepTime = new WaitForSeconds(stepTime);
            mixer.GetFloat(exposedParameterName, out float currentValue);
            // De -or increases the given pitch with the given amount of steps.
            for (; steps > 0; steps--) {
                yield return waitForStepTime;
                currentValue += stepValue;
                mixer.SetFloat(exposedParameterName, currentValue);
            }
            // Correct for float rounding errors.
            currentValue = Mathf.Round(currentValue * 100f) / 100f;
            mixer.SetFloat(exposedParameterName, currentValue);
        }

        private IEnumerator DetectCurrentProgressCoroutine(string name, float progress, float remainingTime, AudioFinishedCallback callback) {
            TryGetSource(name, out AudioSource source);
            // Cache WaitUntil, done to ensure we don't needlessly allocate a new WaitUntil with the same value each time.
            var waitUntilSoundFinished = new WaitUntil(() => source.SoundFinished(progress));
            yield return waitUntilSoundFinished;
            callback?.Invoke(name, remainingTime);
        }

        private void RegisterSound(string name, AudioSource source) {
            m_soundDictionary.Add(name, source);
        }

        private bool DeregisterSound(string name) {
            return m_soundDictionary.Remove(name);
        }

        private bool TryGetRegisteredAudioSource(string name, out AudioSource source) {
            return m_soundDictionary.TryGetValue(name, out source);
        }

        private bool IsSoundRegistered(string name) {
            return m_soundDictionary.ContainsKey(name);
        }

        private bool TryGetRegisteredChildren(AudioSource parentSource, out Dictionary<string, AudioSource> childDictionary) {
            return m_parentChildDictionary.TryGetValue(parentSource, out childDictionary);
        }

        private static Dictionary<string, AudioSource> CreateNewChild(string keyName, AudioSource newChildSource) {
            return new Dictionary<string, AudioSource>() { { keyName, newChildSource } };
        }

        private void RegisterNewChildren(AudioSource parentSource, Dictionary<string, AudioSource> newChildDictionary) {
            m_parentChildDictionary.Add(parentSource, newChildDictionary);
        }

        private bool TryGetRegisteredChild(Dictionary<string, AudioSource> childDictionary, string keyName, out AudioSource childSource) {
            return childDictionary.TryGetValue(keyName, out childSource);
        }

        private void RegisterNewChild(Dictionary<string, AudioSource> childDictionary, string keyName, AudioSource newChildSource) {
            childDictionary.Add(keyName, newChildSource);
        }

        private AudioError CreateNewChildren(AudioSource parentSource, Vector3 position, string keyName, out AudioSource newChildSource) {
            AudioError error = AudioError.OK;

            if (!m_parentTransform) {
                newChildSource = null;
                error = AudioError.MISSING_PARENT;
                return error;
            }

            error = parentSource.CreateEmptyGameObject(keyName, position, m_parentTransform, out newChildSource);
            if (error != AudioError.OK) {
                return error;
            }

            var newChildDictionary = CreateNewChild(keyName, newChildSource);
            RegisterNewChildren(parentSource, newChildDictionary);
            return error;
        }

        private AudioError UpdateOrCreateChild(AudioSource parentSource, Vector3 position, string keyName, Dictionary<string, AudioSource> childDictionary, out AudioSource childSource) {
            AudioError error = AudioError.OK;

            if (TryGetRegisteredChild(childDictionary, keyName, out childSource)) {
                error = childSource.CopySettingsAndPosition(position, parentSource);
                return error;
            }
            return CreateNewChild(parentSource, position, keyName, childDictionary, out childSource);
        }

        private AudioError CreateNewChild(AudioSource parentSource, Vector3 position, string keyName, Dictionary<string, AudioSource> childDictionary, out AudioSource newChildSource) {
            AudioError error = AudioError.OK;

            if (!m_parentTransform) {
                newChildSource = null;
                error = AudioError.MISSING_PARENT;
                return error;
            }

            error = parentSource.CreateEmptyGameObject(keyName, position, m_parentTransform, out newChildSource);
            if (error != AudioError.OK) {
                return error;
            }

            RegisterNewChild(childDictionary, keyName, newChildSource);
            return error;
        }

        private AudioError CreateNewChildren(AudioSource parentSource, GameObject parent, string keyName, out AudioSource newChildSource) {
            AudioError error = parentSource.AttachAudioSource(out newChildSource, parent);

            if (error != AudioError.OK) {
                return error;
            }

            var newChildDictionary = CreateNewChild(keyName, newChildSource);
            RegisterNewChildren(parentSource, newChildDictionary);
            return error;
        }

        private AudioError UpdateOrCreateChild(AudioSource parentSource, GameObject parent, string keyName, Dictionary<string, AudioSource> childDictionary, out AudioSource childSource) {
            AudioError error = AudioError.OK;

            if (TryGetRegisteredChild(childDictionary, keyName, out childSource) && childSource.IsSameParent(parent)) {
                error = childSource.CopyAudioSourceSettings(parentSource);
                return error;
            }
            return CreateNewChild(parentSource, parent, keyName, childDictionary, out childSource);
        }

        private AudioError CreateNewChild(AudioSource parentSource, GameObject parent, string keyName, Dictionary<string, AudioSource> childDictionary, out AudioSource newChildSource) {
            AudioError error = parentSource.AttachAudioSource(out newChildSource, parent);
            RegisterNewChild(childDictionary, keyName, newChildSource);
            return error;
        }

        private void PlayOrPlayOneShot(AudioSource childSource, bool oneShot) {
            if (oneShot) {
                childSource.PlayOneShot(childSource.clip);
                return;
            }
            childSource.Play();
        }

        private AudioError PlayAt3DPosition(AudioSource parentSource, Vector3 position, bool oneShot, [CallerMemberName] string memberName = "") {
            AudioError error = AudioError.OK;
            AudioSource newSource = null;

            if (TryGetRegisteredChildren(parentSource, out var childDictionary)) {
                error = UpdateOrCreateChild(parentSource, position, memberName, childDictionary, out newSource);
            }
            else {
                error = CreateNewChildren(parentSource, position, memberName, out newSource);
            }

            if (error != AudioError.OK) {
                return error;
            }
            PlayOrPlayOneShot(newSource, oneShot);
            return error;
        }

        private AudioError PlayAttachedToGameObject(AudioSource parentSource, GameObject gameObject, bool oneShot, [CallerMemberName] string memberName = "") {
            AudioError error = AudioError.OK;
            AudioSource newSource = null;

            if (TryGetRegisteredChildren(parentSource, out var childDictionary)) {
                error = UpdateOrCreateChild(parentSource, gameObject, memberName, childDictionary, out newSource);
            }
            else {
                error = CreateNewChildren(parentSource, gameObject, memberName, out newSource);
            }

            if (error != AudioError.OK) {
                return error;
            }
            PlayOrPlayOneShot(newSource, oneShot);
            return error;
        }

        private void ResetStartTime(string name, float remainingTime) {
            TryGetSource(name, out AudioSource source);
            // Stop the sound if it isn't set to looping,
            // this is done to ensure the sound doesn't replay,
            // when it is not set to looping.
            if (!source.loop) {
                source.Stop();
            }
            source.SetTime(0f);
        }
    }
}

using AudioManager.Core;
using AudioManager.Helper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace AudioManager.Service {
    public class DefaultAudioManager : IAudioManager {
        // Readonly private member variables.
        private readonly GameObject m_parentGameObject;
        private readonly MonoBehaviour m_parentBehaviour;
        private readonly Transform m_parentTransform;

        // Private member variables.
        private IDictionary<AudioSource, IDictionary<ChildType, AudioSource>> m_parentChildDictionary;
        private IDictionary<string, IDictionary<float, Coroutine>> m_soundProgressDictionary;
        private IDictionary<string, AudioSourceWrapper> m_soundDictionary;

        public DefaultAudioManager(IDictionary<string, AudioSourceWrapper> sounds, GameObject parentGameObject) {
            m_parentChildDictionary = new Dictionary<AudioSource, IDictionary<ChildType, AudioSource>>();
            m_soundProgressDictionary = new Dictionary<string, IDictionary<float, Coroutine>>();
            m_soundDictionary = new Dictionary<string, AudioSourceWrapper>();

            if (sounds is object) {
                m_soundDictionary = sounds;
            }

            m_parentGameObject = parentGameObject ? parentGameObject : null;
            m_parentBehaviour = parentGameObject ? m_parentGameObject.GetComponent<MonoBehaviour>() : null;
            m_parentTransform = parentGameObject ? m_parentGameObject.transform : null;
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

            source.Set2DAudioOptions(clip, mixerGroup, loop, volume, pitch);
            error = RegisterAudioSource(name, source);
            return error;
        }

        public IEnumerable<string> GetEnumerator() {
            return m_soundDictionary.Keys;
        }

        public AudioError Play(string name) {
            AudioError error = TryGetSource(name, out var source);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }

            source.Source.Play();
            return error;
        }

        public AudioError PlayAtTimeStamp(string name, float startTime) {
            AudioError error = TryGetSource(name, out var source);

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

            float progress = source.Source.IsReversePitch() ? Constants.MAX_PROGRESS : Constants.MIN_PROGRESS;
            error = SubscribeProgressCoroutine(name, progress, ResetStartTime);
            source.Source.Play();
            return error;
        }

        public AudioError GetPlaybackPosition(string name, out float time) {
            AudioError error = TryGetSource(name, out var source);

            // Couldn't find source.
            if (error != AudioError.OK) {
                time = Constants.NULL_VALUE;
                return error;
            }

            time = source.Time;
            return error;
        }

        public AudioError SetPlaybackDirection(string name, float pitch) {
            AudioError error = TryGetSource(name, out var source);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }

            source.SetPitch(pitch);
            source.SetTimeFromCurrentPitch();
            return error;
        }

        public AudioError PlayAt3DPosition(string name, Vector3 position) {
            AudioError error = TryGetSource(name, out var parentSource);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }
            else if (parentSource.Source.IsSound2D()) {
                error = AudioError.CAN_NOT_BE_3D;
                return error;
            }
            else if (!m_parentTransform) {
                error = AudioError.INVALID_PARENT;
                return error;
            }

            PlayAt3DPosition(parentSource, position, false, ChildType.PLAY_AT_3D_POS);
            return error;
        }

        public AudioError PlayOneShotAt3DPosition(string name, Vector3 position) {
            AudioError error = TryGetSource(name, out var parentSource);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }
            else if (parentSource.Source.IsSound2D()) {
                error = AudioError.CAN_NOT_BE_3D;
                return error;
            }
            else if (!m_parentTransform) {
                error = AudioError.INVALID_PARENT;
                return error;
            }

            PlayAt3DPosition(parentSource, position, true, ChildType.PLAY_OS_AT_3D_POS);
            return error;
        }

        public AudioError PlayAttachedToGameObject(string name, GameObject attachGameObject) {
            AudioError error = TryGetSource(name, out var parentSource);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }
            else if (parentSource.Source.IsSound2D()) {
                error = AudioError.CAN_NOT_BE_3D;
                return error;
            }
            else if (!attachGameObject) {
                error = AudioError.INVALID_PARENT;
                return error;
            }

            PlayAttachedToGameObject(parentSource, attachGameObject, false, ChildType.PLAY_ATTCHD_TO_GO);
            return error;
        }

        public AudioError PlayOneShotAttachedToGameObject(string name, GameObject attachGameObject) {
            AudioError error = TryGetSource(name, out var parentSource);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }
            else if (parentSource.Source.IsSound2D()) {
                error = AudioError.CAN_NOT_BE_3D;
                return error;
            }
            else if (!attachGameObject) {
                error = AudioError.INVALID_PARENT;
                return error;
            }

            PlayAttachedToGameObject(parentSource, attachGameObject, true, ChildType.PLAY_OS_ATTCHD_TO_GO);
            return error;
        }

        public AudioError PlayDelayed(string name, float delay) {
            AudioError error = TryGetSource(name, out var source);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }

            source.Source.PlayDelayed(delay);
            return error;
        }

        public AudioError PlayOneShot(string name) {
            AudioError error = TryGetSource(name, out var source);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }

            source.Source.PlayOneShot(source.Source.clip);
            return error;
        }

        public AudioError ChangePitch(string name, float minPitch, float maxPitch) {
            AudioError error = TryGetSource(name, out var source);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }

            float pitch = Random.Range(minPitch, maxPitch);
            error = SetPlaybackDirection(name, pitch);
            return error;
        }

        public AudioError PlayScheduled(string name, double time) {
            AudioError error = TryGetSource(name, out var source);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }

            source.Source.PlayScheduled(time);
            return error;
        }

        public AudioError Stop(string name, ChildType child) {
            AudioError error = TryGetSource(name, out var source);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }

            AudioSource childSource;
            if (child == ChildType.PARENT) {
                childSource = source.Source;
            }
            else if (!TryGetRegisteredChildren(source.Source, out var childDictionary)) {
                error = AudioError.MISSING_CHILDREN;
                return error;
            }
            else if (!TryGetRegisteredChild(childDictionary, child, out childSource)) {
                error = AudioError.INVALID_CHILD;
                return error;
            }
            childSource.Stop();
            return error;
        }

        public AudioError ToggleMute(string name) {
            AudioError error = TryGetSource(name, out var source);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }

            source.Mute = !source.Mute;
            return error;
        }

        public AudioError TogglePause(string name, ChildType child) {
            AudioError error = TryGetSource(name, out var source);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }

            AudioSource childSource;
            if (child == ChildType.PARENT) {
                childSource = source.Source;
            }
            else if (!TryGetRegisteredChildren(source.Source, out var childDictionary)) {
                error = AudioError.MISSING_CHILDREN;
                return error;
            }
            else if (!TryGetRegisteredChild(childDictionary, child, out childSource)) {
                error = AudioError.INVALID_CHILD;
                return error;
            }

            // Check if the sound is playing right now.
            if (childSource.isPlaying) {
                childSource.Pause();
                return error;
            }

            childSource.UnPause();
            return error;
        }

        public AudioError SubscribeSourceChanged(string name, SourceChangedCallback callback) {
            AudioError error = TryGetSource(name, out var source);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }

            source.RegisterCallback(callback);
            return error;
        }

        public AudioError UnsubscribeSourceChanged(string name, SourceChangedCallback callback) {
            AudioError error = TryGetSource(name, out var source);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }

            source.DeregisterCallback(callback);
            return error;
        }

        public AudioError SubscribeProgressCoroutine(string name, float progress, ProgressCoroutineCallback callback) {
            AudioError error = TryGetSource(name, out var source);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }
            else if (!m_parentBehaviour) {
                error = AudioError.MISSING_PARENT;
                return error;
            }
            else if (!source.Source.IsProgressValid(progress)) {
                error = AudioError.INVALID_PROGRESS;
                return error;
            }

            error = RegisterProgressCoroutine(name, source.Source, progress, callback);
            return error;
        }

        public AudioError UnsubscribeProgressCoroutine(string name, float progress) {
            AudioError error = TryGetSource(name, out var source);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }
            else if (!m_parentBehaviour) {
                error = AudioError.MISSING_PARENT;
                return error;
            }
            else if (!source.Source.IsProgressValid(progress)) {
                error = AudioError.INVALID_PROGRESS;
                return error;
            }

            if (!IsProgressRegistered(name, progress, out var coroutineDictionary)) {
                error = AudioError.NOT_SUBSCRIBED;
                return error;
            }

            DeregisterProgressCoroutine(coroutineDictionary, progress);
            return error;
        }

        public AudioError GetProgress(string name, out float progress, ChildType child) {
            AudioError error = TryGetSource(name, out var source);

            // Couldn't find source.
            if (error != AudioError.OK) {
                progress = Constants.NULL_VALUE;
                return error;
            }

            AudioSource childSource;
            if (child == ChildType.PARENT) {
                childSource = source.Source;
            }
            else if (!TryGetRegisteredChildren(source.Source, out var childDictionary)) {
                error = AudioError.MISSING_CHILDREN;
                progress = Constants.NULL_VALUE;
                return error;
            }
            else if (!TryGetRegisteredChild(childDictionary, child, out childSource)) {
                error = AudioError.INVALID_CHILD;
                progress = Constants.NULL_VALUE;
                return error;
            }
            progress = childSource.GetProgress();
            return error;
        }

        public AudioError TryGetSource(string name, out AudioSourceWrapper source) {
            AudioError error = AudioError.OK;

            if (!TryGetRegisteredAudioSource(name, out source)) {
                error = AudioError.DOES_NOT_EXIST;
                return error;
            }

            error = source.IsSoundValid();
            return error;
        }

        public AudioError LerpPitch(string name, float endValue, float waitTime, int granularity) {
            AudioError error = TryGetSource(name, out var source);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }
            else if (source.Source.IsSamePitch(endValue)) {
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

            AudioHelper.GetStepValueAndTime(source.Pitch, endValue, waitTime, granularity, out float stepValue, out float stepTime);
            m_parentBehaviour.StartCoroutine(LerpPitchCoroutine(source, stepValue, stepTime, granularity));
            return error;
        }

        public AudioError LerpVolume(string name, float endValue, float waitTime, int granularity) {
            AudioError error = TryGetSource(name, out var source);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }
            else if (source.Source.IsSameVolume(endValue)) {
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

            AudioHelper.GetStepValueAndTime(source.Volume, endValue, waitTime, granularity, out float stepValue, out float stepTime);
            m_parentBehaviour.StartCoroutine(LerpVolumeCoroutine(source, stepValue, stepTime, granularity));
            return error;
        }

        public AudioError ChangeGroupValue(string name, string exposedParameterName, float newValue) {
            AudioError error = TryGetSource(name, out var source);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }
            else if (!source.Source.IsAudioMixerGroupValid()) {
                error = AudioError.MISSING_MIXER_GROUP;
                return error;
            }
            error = source.Source.TrySetGroupValue(exposedParameterName, newValue);
            return error;
        }

        public AudioError GetGroupValue(string name, string exposedParameterName, out float currentValue) {
            AudioError error = TryGetSource(name, out var source);
            currentValue = Constants.NULL_VALUE;

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }
            else if (!source.Source.IsAudioMixerGroupValid()) {
                error = AudioError.MISSING_MIXER_GROUP;
                return error;
            }
            error = source.Source.TryGetGroupValue(exposedParameterName, out currentValue);
            return error;
        }

        public AudioError ResetGroupValue(string name, string exposedParameterName) {
            AudioError error = TryGetSource(name, out var source);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }
            else if (!source.Source.IsAudioMixerGroupValid()) {
                error = AudioError.MISSING_MIXER_GROUP;
                return error;
            }
            error = source.Source.TryClearGroupValue(exposedParameterName);
            return error;
        }

        public AudioError LerpGroupValue(string name, string exposedParameterName, float endValue, float waitTime, int granularity) {
            AudioError error = TryGetSource(name, out var source);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }
            else if (!source.Source.IsAudioMixerGroupValid()) {
                error = AudioError.MISSING_MIXER_GROUP;
                return error;
            }
            else if (!m_parentBehaviour) {
                error = AudioError.MISSING_PARENT;
                return error;
            }

            error = source.Source.TryGetGroupValue(exposedParameterName, out float currentValue);
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
            m_parentBehaviour.StartCoroutine(LerpExposedParameterCoroutine(source.MixerGroup.audioMixer, exposedParameterName, stepValue, stepTime, granularity));
            return error;
        }

        public AudioError RemoveGroup(string name) {
            AudioError error = TryGetSource(name, out var source);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }

            source.SetAudioMixerGroup(null);
            return error;
        }

        public AudioError AddGroup(string name, AudioMixerGroup mixerGroup) {
            AudioError error = TryGetSource(name, out var source);

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
            AudioError error = TryGetSource(name, out var source);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }
            else if (AudioHelper.IsSound2D(spatialBlend)) {
                error = AudioError.CAN_NOT_BE_3D;
                return error;
            }

            source.Set3DAudioOptions(spatialBlend, dopplerLevel, spreadAngle, rolloffMode, minDistance, maxDistance);
            return error;
        }

        public AudioError SetStartTime(string name, float startTime) {
            AudioError error = TryGetSource(name, out var source);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }
            else if (!source.Source.IsLengthValid(startTime)) {
                error = AudioError.INVALID_TIME;
                return error;
            }

            source.SetTime(startTime);
            return error;
        }

        public AudioError SkipTime(string name, float time) {
            AudioError error = TryGetSource(name, out var source);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }
            else if (time < 0f) {
                source.DecreaseTime(time);
            }
            else {
                source.IncreaseTime(time);
            }

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

            RegisterSound(name, source);
            return error;
        }

        private IEnumerator LerpPitchCoroutine(AudioSourceWrapper source, float stepValue, float stepTime, int steps) {
            // Cache WaitForSeconds, done to ensure we don't needlessly allocate a new WaitForSeconds with the same value each time.
            var waitForStepTime = new WaitForSeconds(stepTime);
            // De -or increases the given pitch with the given amount of steps.
            for (; steps > 0; steps--) {
                yield return waitForStepTime;
                source.Pitch += stepValue;
            }
            // Correct for float rounding errors.
            source.Pitch = Mathf.Round(source.Pitch * 100f) / 100f;
        }

        private IEnumerator LerpVolumeCoroutine(AudioSourceWrapper source, float stepValue, float stepTime, int steps) {
            // Cache WaitForSeconds, done to ensure we don't needlessly allocate a new WaitForSeconds with the same value each time.
            var waitForStepTime = new WaitForSeconds(stepTime);
            // De -or increases the given pitch with the given amount of steps.
            for (; steps > 0; steps--) {
                yield return waitForStepTime;
                source.Volume += stepValue;
            }
            // Correct for float rounding errors.
            source.Volume = Mathf.Round(source.Volume * 100f) / 100f;
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

        private IEnumerator DetectCurrentProgressCoroutine(AudioSource source, float progress, string name, ProgressCoroutineCallback callback) {
            // Cache WaitUntil, done to ensure we don't needlessly allocate a new WaitUntil with the same value each time.
            var waitUntilProgressAchieved = new WaitUntil(() => DetectProgressAchieved(source, progress));
            yield return waitUntilProgressAchieved;
            yield return ResubscribeProgressCoroutine(source, progress, name, callback);
        }

        private bool DetectProgressAchieved(AudioSource source, float progress) {
            bool progressAchieved = source.ProgressAchieved(progress);

            if (!TryGetRegisteredChildren(source, out var childDictionary)) {
                return progressAchieved;
            }

            foreach (var pair in childDictionary) {
                if (!pair.Value.ProgressAchieved(progress)) {
                    continue;
                }

                progressAchieved = true;
                break;
            }
            return progressAchieved;
        }

        private void DetectChildType(AudioSource source, float progress, out ChildType child) {
            child = ChildType.PARENT;

            if (!TryGetRegisteredChildren(source, out var childDictionary)) {
                return;
            }

            foreach (var pair in childDictionary) {
                if (!pair.Value.ProgressAchieved(progress)) {
                    continue;
                }

                child = pair.Key;
                break;
            }
        }

        private IEnumerator ResubscribeProgressCoroutine(AudioSource source, float progress, string name, ProgressCoroutineCallback callback) {
            if (!IsProgressRegistered(name, progress, out var coroutineDictionary)) {
                yield break;
            }

            DetectChildType(source, progress, out ChildType child);
            ProgressResponse response = (callback?.Invoke(name, progress, child)).GetValueOrDefault();
            yield return HandleProgressResponse(coroutineDictionary, source, name, callback, response, progress);
        }

        private IEnumerator HandleProgressResponse(IDictionary<float, Coroutine> coroutineDictionary, AudioSource source, string name, ProgressCoroutineCallback callback, ProgressResponse response, float progress) {
            var waitForClipRemainingTime = new WaitForSeconds(source.GetClipRemainingTime());

            switch (response) {
                case ProgressResponse.UNSUB:
                    DeregisterCoroutine(coroutineDictionary, progress);
                    break;
                case ProgressResponse.RESUB_IN_LOOP:
                    yield return waitForClipRemainingTime;
                    goto case ProgressResponse.RESUB_IMMEDIATE;
                case ProgressResponse.RESUB_IMMEDIATE:
                    yield return DetectCurrentProgressCoroutine(source, progress, name, callback);
                    break;
                default:
                    // Unexpected ProgressResponse argument.
                    goto case ProgressResponse.UNSUB;
            }
        }

        private void RegisterSound(string name, AudioSource source) {
            m_soundDictionary.Add(name, new AudioSourceWrapper(source));
        }

        private bool DeregisterSound(string name) {
            return m_soundDictionary.Remove(name);
        }

        private bool TryGetRegisteredAudioSource(string name, out AudioSourceWrapper sourceWrapper) {
            return m_soundDictionary.TryGetValue(name, out sourceWrapper);
        }

        private bool IsSoundRegistered(string name) {
            return m_soundDictionary.ContainsKey(name);
        }

        private bool TryGetRegisteredChildren(AudioSource parentSource, out IDictionary<ChildType, AudioSource> childDictionary) {
            return m_parentChildDictionary.TryGetValue(parentSource, out childDictionary);
        }

        private void UpdateChildren(AudioSource parentSource, IDictionary<ChildType, AudioSource> childDictionary) {
            foreach (var childSource in childDictionary.Values) {
                UpdateChild(parentSource, childSource);
            }
        }

        private void UpdateChild(AudioSource parentSource, AudioSource childSource) {
            childSource.CopyAudioSourceSettings(parentSource);
        }

        private bool TryGetRegisteredCoroutines(string name, out IDictionary<float, Coroutine> coroutineDictionary) {
            return m_soundProgressDictionary.TryGetValue(name, out coroutineDictionary);
        }

        private IDictionary<ChildType, AudioSource> CreateNewChild(ChildType child, AudioSource newChildSource) {
            return new Dictionary<ChildType, AudioSource>() { { child, newChildSource } };
        }

        private IDictionary<float, Coroutine> CreateNewCoroutine(float progress, Coroutine coroutine) {
            return new Dictionary<float, Coroutine>() { { progress, coroutine } };
        }

        private void RegisterNewChildren(AudioSource parentSource, IDictionary<ChildType, AudioSource> newChildDictionary) {
            m_parentChildDictionary.Add(parentSource, newChildDictionary);
        }

        private void RegisterNewCoroutines(string name, IDictionary<float, Coroutine> coroutineDictionary) {
            m_soundProgressDictionary.Add(name, coroutineDictionary);
        }

        private bool TryGetRegisteredChild(IDictionary<ChildType, AudioSource> childDictionary, ChildType child, out AudioSource childSource) {
            return childDictionary.TryGetValue(child, out childSource);
        }

        private bool TryGetRegisteredCoroutine(IDictionary<float, Coroutine> coroutineDictionary, float progress, out Coroutine coroutine) {
            return coroutineDictionary.TryGetValue(progress, out coroutine);
        }

        private bool IsProgressRegistered(string name, float progress, out IDictionary<float, Coroutine> coroutineDictionary) {
            return TryGetRegisteredCoroutines(name, out coroutineDictionary) && IsCoroutineRegistered(coroutineDictionary, progress);
        }

        private bool IsCoroutineRegistered(IDictionary<float, Coroutine> coroutineDictionary, float progress) {
            return coroutineDictionary.ContainsKey(progress);
        }

        private bool DeregisterCoroutine(IDictionary<float, Coroutine> coroutineDictionary, float progress) {
            return coroutineDictionary.Remove(progress);
        }

        private void RegisterNewChild(IDictionary<ChildType, AudioSource> childDictionary, ChildType child, AudioSource newChildSource) {
            childDictionary.Add(child, newChildSource);
        }

        private void RegisterNewCoroutine(IDictionary<float, Coroutine> coroutineDictionary, float progress, Coroutine coroutine) {
            coroutineDictionary.Add(progress, coroutine);
        }

        private void CreateNewChildren(AudioSourceWrapper parentSource, Vector3 position, ChildType child, out AudioSource newChildSource) {
            parentSource.Source.CreateEmptyGameObject(position, m_parentTransform, out newChildSource);
            var newChildDictionary = CreateNewChild(child, newChildSource);
            RegisterNewChildren(parentSource.Source, newChildDictionary);
            parentSource.RegisterCallback(UpdateChildren);
        }

        private void CreateNewCoroutines(string name, float progress, Coroutine coroutine) {
            var newCoroutineDictionary = CreateNewCoroutine(progress, coroutine);
            RegisterNewCoroutines(name, newCoroutineDictionary);
        }

        private void UpdateOrCreateChild(AudioSource parentSource, Vector3 position, ChildType child, IDictionary<ChildType, AudioSource> childDictionary, out AudioSource childSource) {
            if (TryGetRegisteredChild(childDictionary, child, out childSource)) {
                childSource.CopySettingsAndPosition(position, parentSource);
                return;
            }
            CreateNewChild(parentSource, position, child, childDictionary, out childSource);
        }

        private void CreateNewChild(AudioSource parentSource, Vector3 position, ChildType child, IDictionary<ChildType, AudioSource> childDictionary, out AudioSource newChildSource) {
            parentSource.CreateEmptyGameObject(position, m_parentTransform, out newChildSource);
            RegisterNewChild(childDictionary, child, newChildSource);
        }

        private void CreateNewChildren(AudioSourceWrapper parentSource, GameObject parent, ChildType child, out AudioSource newChildSource) {
            parentSource.Source.AttachAudioSource(out newChildSource, parent);
            var newChildDictionary = CreateNewChild(child, newChildSource);
            RegisterNewChildren(parentSource.Source, newChildDictionary);
            parentSource.RegisterCallback(UpdateChildren);
        }

        private void UpdateOrCreateChild(AudioSource parentSource, GameObject parent, ChildType child, IDictionary<ChildType, AudioSource> childDictionary, out AudioSource childSource) {
            if (TryGetRegisteredChild(childDictionary, child, out childSource)) {
                childSource.CopySettingsAndGameObject(parent, parentSource);
                return;
            }
            CreateNewChild(parentSource, parent, child, childDictionary, out childSource);
        }

        private void CreateNewChild(AudioSource parentSource, GameObject parent, ChildType child, IDictionary<ChildType, AudioSource> childDictionary, out AudioSource newChildSource) {
            parentSource.AttachAudioSource(out newChildSource, parent);
            RegisterNewChild(childDictionary, child, newChildSource);
        }

        private void PlayOrPlayOneShot(AudioSource childSource, bool oneShot) {
            if (oneShot) {
                childSource.PlayOneShot(childSource.clip);
                return;
            }
            childSource.Play();
        }

        private void PlayAt3DPosition(AudioSourceWrapper parentSource, Vector3 position, bool oneShot, ChildType childType) {
            AudioSource newSource = null;

            if (TryGetRegisteredChildren(parentSource.Source, out var childDictionary)) {
                UpdateOrCreateChild(parentSource.Source, position, childType, childDictionary, out newSource);
            }
            else {
                CreateNewChildren(parentSource, position, childType, out newSource);
            }

            PlayOrPlayOneShot(newSource, oneShot);
        }

        private void PlayAttachedToGameObject(AudioSourceWrapper parentSource, GameObject gameObject, bool oneShot, ChildType childType) {
            AudioSource newSource = null;

            if (TryGetRegisteredChildren(parentSource.Source, out var childDictionary)) {
                UpdateOrCreateChild(parentSource.Source, gameObject, childType, childDictionary, out newSource);
            }
            else {
                CreateNewChildren(parentSource, gameObject, childType, out newSource);
            }

            PlayOrPlayOneShot(newSource, oneShot);
        }

        private AudioError RegisterProgressCoroutine(string name, AudioSource source, float progress, ProgressCoroutineCallback callback) {
            AudioError error = AudioError.OK;

            if (IsProgressRegistered(name, progress, out var coroutineDictionary)) {
                error = AudioError.ALREADY_SUBSCRIBED;
                return error;
            }

            Coroutine coroutine = m_parentBehaviour.StartCoroutine(DetectCurrentProgressCoroutine(source, progress, name, callback));
            if (coroutineDictionary is object) {
                RegisterNewCoroutine(coroutineDictionary, progress, coroutine);
            }
            else {
                CreateNewCoroutines(name, progress, coroutine);
            }

            return error;
        }

        private void DeregisterProgressCoroutine(IDictionary<float, Coroutine> coroutineDictionary, float progress) {
            TryGetRegisteredCoroutine(coroutineDictionary, progress, out Coroutine coroutine);
            m_parentBehaviour.StopCoroutine(coroutine);
            DeregisterCoroutine(coroutineDictionary, progress);
        }

        private ProgressResponse ResetStartTime(string name, float progress, ChildType child) {
            TryGetSource(name, out var source);
            // Stop the sound if it isn't set to looping,
            // this is done to ensure the sound doesn't replay,
            // when it is not set to looping.
            if (!source.Loop) {
                Stop(name, child);
            }
            SetStartTime(name, 0f);
            return ProgressResponse.UNSUB;
        }

        private void UpdateChildren(AudioSource changedSource) {
            if (!TryGetRegisteredChildren(changedSource, out var childDictionary)) {
                return;
            }
            UpdateChildren(changedSource, childDictionary);
        }
    }
}

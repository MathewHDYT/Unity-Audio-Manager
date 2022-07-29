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
        private readonly IDictionary<string, IDictionary<float, Coroutine>> m_soundProgressDictionary;
        private readonly IDictionary<string, AudioSourceWrapper> m_soundDictionary;

        public DefaultAudioManager(IDictionary<string, AudioSourceWrapper> sounds, GameObject parentGameObject) {
            m_soundProgressDictionary = new Dictionary<string, IDictionary<float, Coroutine>>();
            m_soundDictionary = new Dictionary<string, AudioSourceWrapper>();

            if (sounds is object) {
                m_soundDictionary = sounds;
            }

            m_parentGameObject = parentGameObject ? parentGameObject : null;
            m_parentBehaviour = parentGameObject ? m_parentGameObject.GetComponent<MonoBehaviour>() : null;
            m_parentTransform = parentGameObject ? m_parentGameObject.transform : null;
            RegisterUpdateChildren();
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

        public AudioError Play(string name, ChildType child) {
            AudioError error = TryGetSource(name, out var source);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }

            error = source.InvokeChild(child, (s) => s.Play());
            return error;
        }

        public AudioError PlayAtTimeStamp(string name, float startTime, ChildType child) {
            AudioError error = TryGetSource(name, out var source);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }
            else if (!m_parentBehaviour) {
                error = AudioError.MISSING_PARENT;
                return error;
            }

            error = SetStartTime(name, startTime, child);
            if (error != AudioError.OK) {
                return error;
            }

            float progress = source.Source.IsReversePitch() ? Constants.MAX_PROGRESS : Constants.MIN_PROGRESS;
            error = SubscribeProgressCoroutine(name, progress, ResetStartTime);
            error = Play(name, child);
            return error;
        }

        public AudioError GetPlaybackPosition(string name, out float time, ChildType child) {
            AudioError error = TryGetSource(name, out var source);

            // Couldn't find source.
            if (error != AudioError.OK) {
                time = Constants.F_NULL_VALUE;
                return error;
            }

            error = source.InvokeChild(child, (s) => s.time, out time);
            return error;
        }

        public AudioError SetPlaybackDirection(string name, float pitch, ChildType child) {
            AudioError error = TryGetSource(name, out var source);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }

            error = source.InvokeChild(child, (s) => {
                s.SetPitch(pitch);
                s.SetTimeFromCurrentPitch();
            });
            return error;
        }

        public AudioError RegisterChildAt3DPos(string name, Vector3 position, out ChildType child) {
            AudioError error = TryGetSource(name, out var parentSource);
            child = ChildType.AT_3D_POS;

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

            RegisterAt3DPosition(parentSource, position, child);
            return error;
        }

        public AudioError RegisterChildAttachedToGo(string name, GameObject attachGameObject, out ChildType child) {
            AudioError error = TryGetSource(name, out var parentSource);
            child = ChildType.ATTCHD_TO_GO;

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

            RegisterAttachedToGameObject(parentSource, attachGameObject, child);
            return error;
        }

        public AudioError DeregisterChild(string name, ChildType child) {
            AudioError error = TryGetSource(name, out var parentSource);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }

            // Check if all children should be deregistered instead.
            if (child == ChildType.ALL) {
                parentSource.DeregisterChildren();
                return error;
            }

            error = parentSource.DeregisterChild(child);
            return error;
        }

        public AudioError PlayDelayed(string name, float delay, ChildType child) {
            AudioError error = TryGetSource(name, out var source);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }

            error = source.InvokeChild(child, (s) => s.PlayDelayed(delay));
            return error;
        }

        public AudioError PlayOneShot(string name, ChildType child) {
            AudioError error = TryGetSource(name, out var source);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }

            error = source.InvokeChild(child, (s) => s.PlayOneShot(s.clip));
            return error;
        }

        public AudioError ChangePitch(string name, float minPitch, float maxPitch, ChildType child) {
            float pitch = Random.Range(minPitch, maxPitch);
            return SetPlaybackDirection(name, pitch, child);
        }

        public AudioError GetClipLength(string name, out double length, ChildType child) {
            AudioError error = TryGetSource(name, out var source);
            length = Constants.D_NULL_VALUE;

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }

            error = source.InvokeChild(child, (s) => s.GetClipLength(), out length);
            return error;
        }

        public AudioError PlayScheduled(string name, double time, ChildType child) {
            AudioError error = TryGetSource(name, out var source);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }

            time += AudioSettings.dspTime;
            error = source.InvokeChild(child, (s) => s.PlayScheduled(time));
            return error;
        }

        public AudioError Stop(string name, ChildType child) {
            AudioError error = TryGetSource(name, out var source);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }

            error = source.InvokeChild(child, (s) => s.Stop());
            return error;
        }

        public AudioError ToggleMute(string name, ChildType child) {
            AudioError error = TryGetSource(name, out var source);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }

            error = source.InvokeChild(child, (s) => s.ToggleMute());
            return error;
        }

        public AudioError TogglePause(string name, ChildType child) {
            AudioError error = TryGetSource(name, out var source);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }

            error = source.InvokeChild(child, (s) => s.TogglePause());
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

            error = RegisterProgressCoroutine(name, source, progress, callback);
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
            progress = Constants.F_NULL_VALUE;

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }

            error = source.InvokeChild(child, (s) => s.GetProgress(), out progress);
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

        public AudioError LerpPitch(string name, float endValue, float duration, ChildType child) {
            AudioError error = TryGetSource(name, out var source);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }
            else if (source.Source.IsSamePitch(endValue)) {
                error = AudioError.INVALID_END_VALUE;
                return error;
            }
            else if (!m_parentBehaviour) {
                error = AudioError.MISSING_PARENT;
                return error;
            }

            error = source.GetChildSource(child, out var childSource);
            if (error != AudioError.OK) {
                return error;
            }

            SetCallback<float> cb = (x, s) => {
                if (childSource is null) {
                    s.Pitch = x;
                    return;
                }
                childSource.pitch = x;
            };

            m_parentBehaviour.StartCoroutine(source.LerpValueCoroutine(source.Pitch, endValue, duration, cb));
            return error;
        }

        public AudioError LerpVolume(string name, float endValue, float duration, ChildType child) {
            AudioError error = TryGetSource(name, out var source);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }
            else if (source.Source.IsSameVolume(endValue)) {
                error = AudioError.INVALID_END_VALUE;
                return error;
            }
            else if (!m_parentBehaviour) {
                error = AudioError.MISSING_PARENT;
                return error;
            }
             
            error = source.GetChildSource(child, out var childSource);
            if (error != AudioError.OK) {
                return error;
            }

            SetCallback<float> cb = (x, s) => {
                if (childSource is null) {
                    s.Volume = x;
                    return;
                }
                childSource.volume = x;
            };

            m_parentBehaviour.StartCoroutine(source.LerpValueCoroutine(source.Volume, endValue, duration, cb));
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

            error = source.TrySetGroupValue(exposedParameterName, newValue);
            return error;
        }

        public AudioError GetGroupValue(string name, string exposedParameterName, out float currentValue) {
            AudioError error = TryGetSource(name, out var source);
            currentValue = Constants.F_NULL_VALUE;

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }
            else if (!source.Source.IsAudioMixerGroupValid()) {
                error = AudioError.MISSING_MIXER_GROUP;
                return error;
            }

            error = source.TryGetGroupValue(exposedParameterName, out currentValue);
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

            error = source.TryClearGroupValue(exposedParameterName);
            return error;
        }

        public AudioError LerpGroupValue(string name, string exposedParameterName, float endValue, float duration) {
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

            error = source.TryGetGroupValue(exposedParameterName, out float currentValue);
            if (error != AudioError.OK) {
                return error;
            }
            else if (!AudioHelper.IsEndValueValid(currentValue, endValue)) {
                error = AudioError.INVALID_END_VALUE;
            }

            m_parentBehaviour.StartCoroutine(source.LerpGroupValueCoroutine(exposedParameterName, endValue, duration));
            return error;
        }

        public AudioError RemoveGroup(string name, ChildType child) {
            AudioError error = TryGetSource(name, out var source);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }

            error = source.InvokeChild(child, (s) => s.SetAudioMixerGroup(null));
            return error;
        }

        public AudioError AddGroup(string name, AudioMixerGroup mixerGroup, ChildType child) {
            AudioError error = TryGetSource(name, out var source);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }

            error = source.InvokeChild(child, (s) => s.SetAudioMixerGroup(mixerGroup));
            return error;
        }

        public AudioError RemoveSound(string name) {
            return DeregisterSound(name);
        }

        public AudioError Set3DAudioOptions(string name, float minDistance, float maxDistance, ChildType child, float spatialBlend, float spreadAngle, float dopplerLevel, AudioRolloffMode rolloffMode) {
            AudioError error = TryGetSource(name, out var source);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }
            else if (AudioHelper.IsSound2D(spatialBlend)) {
                error = AudioError.CAN_NOT_BE_3D;
                return error;
            }

            error = source.InvokeChild(child, (s) => s.Set3DAudioOptions(spatialBlend, dopplerLevel, spreadAngle, rolloffMode, minDistance, maxDistance));
            return error;
        }

        public AudioError SetStartTime(string name, float startTime, ChildType child) {
            AudioError error = TryGetSource(name, out var source);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }
            else if (!source.Source.IsLengthValid(startTime)) {
                error = AudioError.INVALID_TIME;
                return error;
            }

            error = source.InvokeChild(child, (s) => s.SetTime(startTime));
            return error;
        }

        public AudioError SkipTime(string name, float time, ChildType child) {
            AudioError error = TryGetSource(name, out var source);

            // Couldn't find source.
            if (error != AudioError.OK) {
                return error;
            }

            error = source.InvokeChild(child, (s) => s.SkipTime(time));
            return error;
        }

        //************************************************************************************************************************
        // Private Section
        //************************************************************************************************************************

        private void RegisterUpdateChildren() {
            foreach (var name in m_soundDictionary.Keys) {
                SubscribeSourceChanged(name, UpdateChildren);
            }
        }

        private AudioError RegisterAudioSource(string name, AudioSource source) {
            AudioError error = AudioError.OK;

            if (IsSoundRegistered(name)) {
                error = AudioError.ALREADY_EXISTS;
                return error;
            }

            RegisterSound(name, source);
            return error;
        }

        private IEnumerator DetectCurrentProgressCoroutine(AudioSourceWrapper source, float progress, string name, ProgressCoroutineCallback callback) {
            // Cache WaitUntil, done to ensure we don't needlessly allocate a new WaitUntil with the same value each time.
            var waitUntilProgressAchieved = new WaitUntil(() => DetectProgressAchieved(source, progress));
            yield return waitUntilProgressAchieved;
            yield return ResubscribeProgressCoroutine(source, progress, name, callback);
        }

        private bool DetectProgressAchieved(AudioSourceWrapper source, float progress) {
            bool progressAchieved = source.Source.ProgressAchieved(progress);

            foreach (var pair in source) {
                if (!pair.Value.ProgressAchieved(progress)) {
                    continue;
                }

                progressAchieved = true;
                break;
            }
            return progressAchieved;
        }

        private void DetectChildType(AudioSourceWrapper source, float progress, out ChildType child) {
            child = ChildType.PARENT;

            foreach (var pair in source) {
                if (!pair.Value.ProgressAchieved(progress)) {
                    continue;
                }

                child = pair.Key;
                break;
            }
        }

        private IEnumerator ResubscribeProgressCoroutine(AudioSourceWrapper source, float progress, string name, ProgressCoroutineCallback callback) {
            if (!IsProgressRegistered(name, progress, out var coroutineDictionary)) {
                yield break;
            }

            DetectChildType(source, progress, out ChildType child);
            ProgressResponse response = (callback?.Invoke(name, progress, child)).GetValueOrDefault();
            yield return HandleProgressResponse(coroutineDictionary, source, name, callback, response, progress);
        }

        private IEnumerator HandleProgressResponse(IDictionary<float, Coroutine> coroutineDictionary, AudioSourceWrapper source, string name, ProgressCoroutineCallback callback, ProgressResponse response, float progress) {
            var waitForClipRemainingTime = new WaitForSeconds(source.Source.GetClipRemainingTime());

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

        private AudioError DeregisterSound(string name) {
            AudioError error = AudioError.OK;
            if (!m_soundDictionary.TryGetValue(name, out var source)) {
                error = AudioError.DOES_NOT_EXIST;
                return error;
            }

            DeregisterChildren(source);
            DestroySound(name, source.Source);
            return error;
        }

        private void DeregisterChildren(AudioSourceWrapper source) {
            source.DeregisterChildren();
        }

        private void DestroySound(string name, AudioSource source) {
            Object.Destroy(source);
            m_soundDictionary.Remove(name);
        }

        private bool TryGetRegisteredAudioSource(string name, out AudioSourceWrapper sourceWrapper) {
            return m_soundDictionary.TryGetValue(name, out sourceWrapper);
        }

        private bool IsSoundRegistered(string name) {
            return m_soundDictionary.ContainsKey(name);
        }

        private bool TryGetRegisteredCoroutines(string name, out IDictionary<float, Coroutine> coroutineDictionary) {
            return m_soundProgressDictionary.TryGetValue(name, out coroutineDictionary);
        }

        private IDictionary<float, Coroutine> CreateNewCoroutine(float progress, Coroutine coroutine) {
            return new Dictionary<float, Coroutine>() { { progress, coroutine } };
        }

        private void RegisterNewCoroutines(string name, IDictionary<float, Coroutine> coroutineDictionary) {
            m_soundProgressDictionary.Add(name, coroutineDictionary);
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

        private void RegisterNewCoroutine(IDictionary<float, Coroutine> coroutineDictionary, float progress, Coroutine coroutine) {
            coroutineDictionary.Add(progress, coroutine);
        }

        private void CreateNewCoroutines(string name, float progress, Coroutine coroutine) {
            var newCoroutineDictionary = CreateNewCoroutine(progress, coroutine);
            RegisterNewCoroutines(name, newCoroutineDictionary);
        }

        private void UpdateChildPosition(AudioSource parentSource, Vector3 position, AudioSource childSource) {
            childSource.CopySettingsAndPosition(position, parentSource);
        }

        private void CreateNewChild(AudioSourceWrapper parentSource, Vector3 position, ChildType child) {
            parentSource.Source.CreateEmptyGameObject(position, m_parentTransform, out AudioSource newChildSource);
            parentSource.RegisterNewChild(child, newChildSource);
        }

        private void UpdateChildGameObject(AudioSource parentSource, GameObject parent, AudioSource childSource) {
            childSource.CopySettingsAndGameObject(parent, parentSource);
        }

        private void CreateNewChild(AudioSourceWrapper parentSource, GameObject parent, ChildType child) {
            parentSource.Source.AttachAudioSource(out AudioSource newChildSource, parent);
            parentSource.RegisterNewChild(child, newChildSource);
        }

        private void RegisterAt3DPosition(AudioSourceWrapper parentSource, Vector3 position, ChildType child) {
            if (parentSource.TryGetRegisteredChild(child, out var childSource)) {
                UpdateChildPosition(parentSource.Source, position, childSource);
                return;
            }
            CreateNewChild(parentSource, position, child);
        }

        private void RegisterAttachedToGameObject(AudioSourceWrapper parentSource, GameObject gameObject, ChildType child) {
            if (parentSource.TryGetRegisteredChild(child, out var childSource)) {
                UpdateChildGameObject(parentSource.Source, gameObject, childSource);
                return;
            }
            CreateNewChild(parentSource, gameObject, child);
        }

        private AudioError RegisterProgressCoroutine(string name, AudioSourceWrapper source, float progress, ProgressCoroutineCallback callback) {
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
            AudioError error = source.InvokeChild(child, (s) => s.loop, out bool looping);
            if (error == AudioError.OK && !looping) {
                Stop(name, child); 
            }
            SetStartTime(name, 0f, child);
            return ProgressResponse.UNSUB;
        }

        private void UpdateChildren(AudioSourceWrapper changedSource) {
            foreach (var childSource in changedSource.GetChildren()) {
                childSource.CopyAudioSourceSettings(changedSource.Source);
            }
        }
    }
}

using AudioManager.Core;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace AudioManager.Locator {
    /// <summary>
    /// Null instances of the IAudioManager interface, simply returns the AudioError.NOT_INITALIZED error for all API methods.
    /// </summary>
    public class NullAudioManager : IAudioManager {
        public AudioError AddSoundFromPath(string name, string path, float volume, float pitch, bool loop, AudioSource source, AudioMixerGroup mixerGroup) {
            return AudioError.NOT_INITIALIZED;
        }

        public IEnumerable<string> GetEnumerator() {
            return null;
        }

        public AudioError Play(string name, ChildType child) {
            return AudioError.NOT_INITIALIZED;
        }

        public AudioError PlayAtTimeStamp(string name, float startTime, ChildType child) {
            return AudioError.NOT_INITIALIZED;
        }

        public AudioError GetPlaybackPosition(string name, out float time, ChildType child) {
            time = Constants.F_NULL_VALUE;
            return AudioError.NOT_INITIALIZED;
        }

        public AudioError SetPlaybackDirection(string name, float pitch, ChildType child) {
            return AudioError.NOT_INITIALIZED;
        }

        public AudioError RegisterChildAt3DPos(string name, Vector3 position, out ChildType child) {
            child = ChildType.AT_3D_POS;
            return AudioError.NOT_INITIALIZED;
        }

        public AudioError RegisterChildAttachedToGo(string name, GameObject gameObject, out ChildType child) {
            child = ChildType.ATTCHD_TO_GO;
            return AudioError.NOT_INITIALIZED;
        }

        public AudioError DeregisterChild(string name, ChildType child) {
            return AudioError.NOT_INITIALIZED;
        }

        public AudioError PlayDelayed(string name, float delay, ChildType child) {
            return AudioError.NOT_INITIALIZED;
        }

        public AudioError PlayOneShot(string name, ChildType child) {
            return AudioError.NOT_INITIALIZED;
        }

        public AudioError ChangePitch(string name, float minPitch, float maxPitch, ChildType child) {
            return AudioError.NOT_INITIALIZED;
        }

        public AudioError GetClipLength(string name, out double length, ChildType child) {
            length = Constants.D_NULL_VALUE;
            return AudioError.NOT_INITIALIZED;
        }

        public AudioError PlayScheduled(string name, double time, ChildType child) {
            return AudioError.NOT_INITIALIZED;
        }

        public AudioError Stop(string name, ChildType child) {
            return AudioError.NOT_INITIALIZED;
        }

        public AudioError ToggleMute(string name, ChildType child) {
            return AudioError.NOT_INITIALIZED;
        }

        public AudioError TogglePause(string name, ChildType child) {
            return AudioError.NOT_INITIALIZED;
        }

        public AudioError SubscribeSourceChanged(string name, SourceChangedCallback callback) {
            return AudioError.NOT_INITIALIZED;
        }

        public AudioError UnsubscribeSourceChanged(string name, SourceChangedCallback callback) {
            return AudioError.NOT_INITIALIZED;
        }

        public AudioError SubscribeProgressCoroutine(string name, float progress, ProgressCoroutineCallback callback) {
            return AudioError.NOT_INITIALIZED;
        }

        public AudioError UnsubscribeProgressCoroutine(string name, float progress) {
            return AudioError.NOT_INITIALIZED;
        }

        public AudioError GetProgress(string name, out float progress, ChildType child) {
            progress = Constants.F_NULL_VALUE;
            return AudioError.NOT_INITIALIZED;
        }

        public AudioError TryGetSource(string name, out AudioSourceWrapper source) {
            source = null;
            return AudioError.NOT_INITIALIZED;
        }

        public AudioError LerpPitch(string name, float endValue, float duration, ChildType child) {
            return AudioError.NOT_INITIALIZED;
        }

        public AudioError LerpVolume(string name, float endValue, float duration, ChildType child) {
            return AudioError.NOT_INITIALIZED;
        }

        public AudioError ChangeGroupValue(string name, string exposedParameterName, float newValue) {
            return AudioError.NOT_INITIALIZED;
        }

        public AudioError GetGroupValue(string name, string exposedParameterName, out float currentValue) {
            currentValue = Constants.F_NULL_VALUE;
            return AudioError.NOT_INITIALIZED;
        }

        public AudioError ResetGroupValue(string name, string exposedParameterName) {
            return AudioError.NOT_INITIALIZED;
        }

        public AudioError LerpGroupValue(string name, string exposedParameterName, float endValue, float duration) {
            return AudioError.NOT_INITIALIZED;
        }

        public AudioError RemoveGroup(string name, ChildType child) {
            return AudioError.NOT_INITIALIZED;
        }

        public AudioError AddGroup(string name, AudioMixerGroup mixerGroup, ChildType child) {
            return AudioError.NOT_INITIALIZED;
        }

        public AudioError RemoveSound(string name) {
            return AudioError.NOT_INITIALIZED;
        }

        public AudioError Set3DAudioOptions(string name, float minDistance, float maxDistance, ChildType child, float spatialBlend, float spread, float dopplerLevel, AudioRolloffMode rolloffMode) {
            return AudioError.NOT_INITIALIZED;
        }

        public AudioError SetStartTime(string name, float startTime, ChildType child) {
            return AudioError.NOT_INITIALIZED;
        }

        public AudioError SkipTime(string name, float time, ChildType child) {
            return AudioError.NOT_INITIALIZED;
        }
    }
}

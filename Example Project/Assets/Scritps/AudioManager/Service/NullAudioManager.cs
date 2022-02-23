using AudioManager.Locator;
using UnityEngine;
using UnityEngine.Audio;

namespace AudioManager.Service {
    /// <summary>
    /// Null instances of the IAudioManager interface, simply returns the AudioError.NOT_INITALIZED error for all API methods.
    /// </summary>
    public class NullAudioManager : IAudioManager {
        /// <summary>
        /// Empty Constructor.
        /// </summary>
        public NullAudioManager() {
            // Nothing to do.
        }

        public AudioError AddSoundFromPath(string name, string path, float volume, float pitch, bool loop, AudioSource source, AudioMixerGroup mixerGroup) {
            return AudioError.NOT_INITIALIZED;
        }

        public AudioError Play(string name) {
            return AudioError.NOT_INITIALIZED;
        }

        public AudioError PlayAtTimeStamp(string name, float startTime) {
            return AudioError.NOT_INITIALIZED;
        }

        public ValueDataError<float> GetPlaybackPosition(string name) {
            return new ValueDataError<float>(float.NaN, ((int)AudioError.NOT_INITIALIZED));
        }

        public AudioError PlayAt3DPosition(string name, Vector3 position) {
            return AudioError.NOT_INITIALIZED;
        }

        public AudioError PlayOneShotAt3DPosition(string name, Vector3 position) {
            return AudioError.NOT_INITIALIZED;
        }

        public AudioError PlayAttachedToGameObject(string name, GameObject gameObject) {
            return AudioError.NOT_INITIALIZED;
        }

        public AudioError PlayOneShotAttachedToGameObject(string name, GameObject gameObject) {
            return AudioError.NOT_INITIALIZED;
        }

        public AudioError PlayDelayed(string name, float delay) {
            return AudioError.NOT_INITIALIZED;
        }

        public AudioError PlayOneShot(string name) {
            return AudioError.NOT_INITIALIZED;
        }

        public AudioError ChangePitch(string name, float minPitch, float maxPitch) {
            return AudioError.NOT_INITIALIZED;
        }

        public AudioError PlayScheduled(string name, double time) {
            return AudioError.NOT_INITIALIZED;
        }

        public AudioError Stop(string name) {
            return AudioError.NOT_INITIALIZED;
        }

        public AudioError ToggleMute(string name) {
            return AudioError.NOT_INITIALIZED;
        }

        public AudioError TogglePause(string name) {
            return AudioError.NOT_INITIALIZED;
        }

        public AudioError SubscribeAudioFinished(string name, float remainingTime, AudioFinishedCallback callback) {
            return AudioError.NOT_INITIALIZED;
        }

        public ValueDataError<float> GetProgress(string name) {
            return new ValueDataError<float>(float.NaN, ((int)AudioError.NOT_INITIALIZED));
        }

        public AudioError TryGetSource(string name, out AudioSource source) {
            source = null;
            return AudioError.NOT_INITIALIZED;
        }

        public AudioError LerpPitch(string name, float endValue, float waitTime, float granularity) {
            return AudioError.NOT_INITIALIZED;
        }

        public AudioError LerpVolume(string name, float endValue, float waitTime, float granularity) {
            return AudioError.NOT_INITIALIZED;
        }

        public AudioError ChangeGroupValue(string name, string exposedParameterName, float newValue) {
            return AudioError.NOT_INITIALIZED;
        }

        public ValueDataError<float> GetGroupValue(string name, string exposedParameterName) {
            return new ValueDataError<float>(float.NaN, ((int)AudioError.NOT_INITIALIZED));
        }

        public AudioError ResetGroupValue(string name, string exposedParameterName) {
            return AudioError.NOT_INITIALIZED;
        }

        public AudioError LerpGroupValue(string name, string exposedParameterName, float endValue, float waitTime, float granularity) {
            return AudioError.NOT_INITIALIZED;
        }

        public AudioError RemoveGroup(string name) {
            return AudioError.NOT_INITIALIZED;
        }

        public AudioError AddGroup(string name, AudioMixerGroup mixerGroup) {
            return AudioError.NOT_INITIALIZED;
        }

        public AudioError RemoveSound(string name) {
            return AudioError.NOT_INITIALIZED;
        }

        public AudioError Set3DAudioOptions(string name, float minDistance, float maxDistance, float spatialBlend, float spread, float dopplerLevel, AudioRolloffMode rolloffMode) {
            return AudioError.NOT_INITIALIZED;
        }

        public AudioError SetStartTime(string name, float startTime) {
            return AudioError.NOT_INITIALIZED;
        }
    }
}

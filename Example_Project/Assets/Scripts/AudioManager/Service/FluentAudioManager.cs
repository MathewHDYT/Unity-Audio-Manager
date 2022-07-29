using AudioManager.Core;
using AudioManager.Helper;
using UnityEngine;
using UnityEngine.Audio;

namespace AudioManager.Service {
    /// <summary>
    /// Fluent instance of the IFluentAudioManager interface, simply wraps the given IAudioManager instance, to seperate fluent interface behaviour from the actual method implementation and to easily not use that behaviour if not needed.
    /// </summary>
    public class FluentAudioManager : IFluentAudioManager {
        // Private member variables.
        private IAudioManager m_wrappedInstance;
        private string m_name;
        private ChildType m_child;
        private AudioError m_error;

        /// <summary>
        /// Constructs a decorartor that wraps the given IAudioManager instance and allows the usage as a fluent interface.
        /// </summary>
        /// <param name="audioManager">IAudioManager instance that should be wrapped and allow method chaining.</param>
        /// <param name="name">Name of the registered sound.</param>
        /// <param name="child">Child that we want to chain methods on.</param>
        /// <param name="error">Inital error the class should be set too, everything besides <see cref="AudioError.OK"/> causes the methods to not be called anymore.</param>
        public FluentAudioManager(IAudioManager audioManager, string name, ChildType child, AudioError error) {
            m_wrappedInstance = audioManager;
            m_name = name;
            m_child = child;
            m_error = error;
        }

        /// <summary>
        /// Adjust this decorator to wrap another IAudioManager instance with other settings instead. Done to make it possible to reuse the same class instance.
        /// </summary>
        /// <param name="audioManager">IAudioManager instance that should be wrapped and allow method chaining.</param>
        /// <param name="name">Name of the registered sound.</param>
        /// <param name="child">Child that we want to chain methods on.</param>
        /// <param name="error">Inital error the class should be set too, everything besides <see cref="AudioError.OK"/> causes the methods to not be called anymore.</param>
        public void ReuseInstance(IAudioManager audioManager, string name, ChildType child, AudioError error) {
            m_wrappedInstance = audioManager;
            m_name = name;
            m_child = child;
            m_error = error;
        }

        public AudioError Execute() {
            return m_error;
        }

        public AudioError Play() {
            if (m_error == AudioError.OK) {
                m_error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.Play(m_name, m_child));
            }
            return m_error;
        }

        public AudioError PlayAtTimeStamp(float startTime) {
            if (m_error == AudioError.OK) {
                m_error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.PlayAtTimeStamp(m_name, startTime, m_child));
            }
            return m_error;
        }

        public IFluentAudioManager GetPlaybackPosition(out float time) {
            time = Constants.F_NULL_VALUE;
            if (m_error == AudioError.OK) {
                m_error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.GetPlaybackPosition(m_name, out time, m_child));
            }
            return this;
        }

        public IFluentAudioManager SetPlaybackDirection(float pitch) {
            if (m_error == AudioError.OK) {
                m_error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.SetPlaybackDirection(m_name, pitch, m_child));
            }
            return this;
        }

        public AudioError PlayDelayed(float delay) {
            if (m_error == AudioError.OK) {
                m_error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.PlayDelayed(m_name, delay, m_child));
            }
            return m_error;
        }

        public AudioError PlayOneShot() {
            if (m_error == AudioError.OK) {
                m_error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.PlayOneShot(m_name, m_child));
            }
            return m_error;
        }

        public IFluentAudioManager ChangePitch(float minPitch, float maxPitch) {
            if (m_error == AudioError.OK) {
                m_error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.ChangePitch(m_name, minPitch, maxPitch, m_child));
            }
            return this;
        }

        public IFluentAudioManager GetClipLength(out double length) {
            length = Constants.D_NULL_VALUE;
            if (m_error == AudioError.OK) {
                m_error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.GetClipLength(m_name, out length, m_child));
            }
            return this;
        }

        public AudioError PlayScheduled(double time) {
            if (m_error == AudioError.OK) {
                m_error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.PlayScheduled(m_name, time, m_child));
            }
            return m_error;
        }

        public IFluentAudioManager DeregisterChild() {
            if (m_error == AudioError.OK) {
                m_error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.DeregisterChild(m_name, m_child));
            }
            return this;
        }

        public AudioError Stop() {
            if (m_error == AudioError.OK) {
                m_error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.Stop(m_name, m_child));
            }
            return m_error;
        }

        public IFluentAudioManager ToggleMute() {
            if (m_error == AudioError.OK) {
                m_error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.ToggleMute(m_name, m_child));
            }
            return this;
        }

        public IFluentAudioManager TogglePause() {
            if (m_error == AudioError.OK) {
                m_error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.TogglePause(m_name, m_child));
            }
            return this;
        }

        public IFluentAudioManager SubscribeSourceChanged(SourceChangedCallback callback) {
            if (m_error == AudioError.OK) {
                m_error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.SubscribeSourceChanged(m_name, callback));
            }
            return this;
        }

        public IFluentAudioManager UnsubscribeSourceChanged(SourceChangedCallback callback) {
            if (m_error == AudioError.OK) {
                m_error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.UnsubscribeSourceChanged(m_name, callback));
            }
            return this;
        }

        public IFluentAudioManager SubscribeProgressCoroutine(float progress, ProgressCoroutineCallback callback) {
            if (m_error == AudioError.OK) {
                m_error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.SubscribeProgressCoroutine(m_name, progress, callback));
            }
            return this;
        }

        public IFluentAudioManager UnsubscribeProgressCoroutine(float progress) {
            if (m_error == AudioError.OK) {
                m_error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.UnsubscribeProgressCoroutine(m_name, progress));
            }
            return this;
        }

        public IFluentAudioManager GetProgress(out float progress) {
            progress = Constants.F_NULL_VALUE;
            if (m_error == AudioError.OK) {
                m_error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.GetProgress(m_name, out progress, m_child));
            }
            return this;
        }

        public IFluentAudioManager TryGetSource(out AudioSourceWrapper source) {
            source = null;
            if (m_error == AudioError.OK) {
                m_error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.TryGetSource(m_name, out source));
            }
            return this;
        }

        public IFluentAudioManager LerpPitch(float endValue, float duration) {
            if (m_error == AudioError.OK) {
                m_error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.LerpPitch(m_name, endValue, duration, m_child));
            }
            return this;
        }

        public IFluentAudioManager LerpVolume(float endValue, float duration) {
            if (m_error == AudioError.OK) {
                m_error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.LerpVolume(m_name, endValue, duration, m_child));
            }
            return this;
        }

        public IFluentAudioManager ChangeGroupValue(string exposedParameterName, float newValue) {
            if (m_error == AudioError.OK) {
                m_error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.ChangeGroupValue(m_name, exposedParameterName, newValue));
            }
            return this;
        }

        public IFluentAudioManager GetGroupValue(string exposedParameterName, out float currentValue) {
            currentValue = Constants.F_NULL_VALUE;
            if (m_error == AudioError.OK) {
                m_error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.GetGroupValue(m_name, exposedParameterName, out currentValue));
            }
            return this;
        }

        public IFluentAudioManager ResetGroupValue(string exposedParameterName) {
            if (m_error == AudioError.OK) {
                m_error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.ResetGroupValue(m_name, exposedParameterName));
            }
            return this;
        }

        public IFluentAudioManager LerpGroupValue(string exposedParameterName, float endValue, float duration) {
            if (m_error == AudioError.OK) {
                m_error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.LerpGroupValue(m_name, exposedParameterName, endValue, duration));
            }
            return this;
        }

        public IFluentAudioManager RemoveGroup() {
            if (m_error == AudioError.OK) {
                m_error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.RemoveGroup(m_name, m_child));
            }
            return this;
        }

        public IFluentAudioManager AddGroup(AudioMixerGroup mixerGroup) {
            if (m_error == AudioError.OK) {
                m_error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.AddGroup(m_name, mixerGroup, m_child));
            }
            return this;
        }

        public AudioError RemoveSound() {
            if (m_error == AudioError.OK) {
                m_error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.RemoveSound(m_name));
            }
            return m_error;
        }

        public IFluentAudioManager Set3DAudioOptions(float minDistance, float maxDistance, float spatialBlend, float spreadAngle, float dopplerLevel, AudioRolloffMode rolloffMode) {
            if (m_error == AudioError.OK) {
                m_error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.Set3DAudioOptions(m_name, minDistance, maxDistance, m_child, spatialBlend, spreadAngle, dopplerLevel, rolloffMode));
            }
            return this;
        }

        public IFluentAudioManager SetStartTime(float startTime) {
            if (m_error == AudioError.OK) {
                m_error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.SetStartTime(m_name, startTime, m_child));
            }
            return this;
        }

        public IFluentAudioManager SkipTime(float time) {
            if (m_error == AudioError.OK) {
                m_error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.SkipTime(m_name, time, m_child));
            }
            return this;
        }
    }
}

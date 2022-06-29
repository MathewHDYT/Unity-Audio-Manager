using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace AudioManager.Core {
    /// <summary>
    /// Subscribable callback that gets called whenever a value of the underlying AudioSource changes.
    /// <param name="changedSource">Source that was accessed and some of its values changed.</param>
    /// </summary>
    public delegate void SourceChangedCallback(AudioSource changedSource);
    public class AudioSourceWrapper {
        // Private member variables.
        private SourceChangedCallback m_cb;
        private AudioSource m_wrappedSource;

        // Private delegate helpers.
        private delegate void SetValueCallback<T>(T value, AudioSource source);
        private delegate T GetValueCallback<T>(AudioSource source);

        public AudioSourceWrapper(AudioSource source) {
            m_cb = null;
            m_wrappedSource = source;
        }

        public void RegisterCallback(SourceChangedCallback callback) {
            m_cb += callback;
        }

        public void DeregisterCallback(SourceChangedCallback callback) {
            m_cb -= callback;
        }

        public AudioSource Source {
            get { return GetC((s) => s); }
        }

        public AudioMixerGroup MixerGroup {
            get { return GetC((s) => s.outputAudioMixerGroup); }
            set { Set(value, MixerGroup, (x, s) => s.outputAudioMixerGroup = x); }
        }

        public float Volume {
            get { return GetS((s) => s.volume); }
            set { Set(value, Volume, (x, s) => s.volume = x); }
        }

        public float Pitch {
            get { return GetS((s) => s.pitch); }
            set { Set(value, Pitch, (x, s) => s.pitch = x); }
        }

        public float Time {
            get { return GetS((s) => s.time); }
            set { Set(value, Time, (x, s) => s.time = x); }
        }

        public float SpatialBlend {
            get { return GetS((s) => s.spatialBlend); }
            set { Set(value, SpatialBlend, (x, s) => s.spatialBlend = x); }
        }

        public float DopplerLevel {
            get { return GetS((s) => s.dopplerLevel); }
            set { Set(value, DopplerLevel, (x, s) => s.dopplerLevel = x); }
        }

        public float Spread {
            get { return GetS((s) => s.spread); }
            set { Set(value, Spread, (x, s) => s.spread = x); }
        }

        public AudioRolloffMode RolloffMode {
            get { return GetS((s) => s.rolloffMode); }
            set { Set(value, RolloffMode, (x, s) => s.rolloffMode = x); }
        }

        public float MinDistance {
            get { return GetS((s) => s.minDistance); }
            set { Set(value, MinDistance, (x, s) => s.minDistance = x); }
        }

        public float MaxDistance {
            get { return GetS((s) => s.maxDistance); }
            set { Set(value, MaxDistance, (x, s) => s.maxDistance = x); }
        }

        public bool Loop {
            get { return GetS((s) => s.loop); }
            set { Set(value, Loop, (x, s) => s.loop = x); }
        }

        public bool Spatialize {
            get { return GetS((s) => s.spatialize); }
            set { Set(value, Spatialize, (x, s) => s.spatialize = x); }
        }

        public bool Mute {
            get { return GetS((s) => s.mute); }
            set { Set(value, Mute, (x, s) => s.mute = x); }
        }

        //************************************************************************************************************************
        // Private Section
        //************************************************************************************************************************

        private void Set<T>(T newValue, T currValue, SetValueCallback<T> set) {
            if (m_wrappedSource is null) {
                return;
            }
            else if (HasSameValue(newValue, currValue)) {
                return;
            }

            set?.Invoke(newValue, m_wrappedSource);
            m_cb?.Invoke(m_wrappedSource);
        }

        // Restrain to classes needed. Because the compiler expects default(RetrunTypeOfRHS) for classes.
        private T GetC<T>(GetValueCallback<T> get) where T : class {
            if (m_wrappedSource is null) {
                return default;
            }

            return get?.Invoke(m_wrappedSource);
        }

        // Restrain to struct needed. Because the compiler expects default(Nullable<RetrunTypeOfRHS>) for structs.
        private T GetS<T>(GetValueCallback<T> get) where T : struct {
            if (m_wrappedSource is null) {
                return default;
            }

            return get?.Invoke(m_wrappedSource) ?? default;
        }

        private bool HasSameValue<T>(T newValue, T currValue) {
            bool? result = currValue?.Equals(newValue);
            return result.HasValue ? result.Value : (newValue is null);
        }
    }
}

using UnityEngine;
using UnityEngine.Audio;

namespace AudioManager.Core {
    /// <summary>
    /// Subscribable callback that gets called whenever a value of the underlying AudioSource changes.
    /// </summary>
    public delegate void SourceChangedCallback(AudioSource source);
    public class AudioSourceWrapper {
        // Readonly private member variables.
        private readonly AudioSource m_wrappedSource;

        // Private member variables.
        private SourceChangedCallback m_cb;

        // Private delegate helpers.
        private delegate void SetValueCallback<T>(T value, AudioSource source);
        private delegate T GetValueCallback<T>(AudioSource source);

        public AudioSourceWrapper(AudioSource source, SourceChangedCallback callback) {
            m_wrappedSource = source;
            m_cb = callback;
        }

        public void SetCallback(SourceChangedCallback callback) {
            m_cb = callback;
        }

        public AudioSource Source => m_wrappedSource;

        public AudioClip Clip {
            get { return GetC((s) => s.clip); }
            set { Set(value, (x, s) => s.clip = x); }
        }

        public AudioMixerGroup MixerGroup {
            get { return GetC((s) => s.outputAudioMixerGroup); }
            set { Set(value, (x, s) => s.outputAudioMixerGroup = x); }
        }

        public float Volume {
            get { return GetS((s) => s.volume); }
            set { Set(value, (x, s) => s.volume = x); }
        }

        public float Pitch {
            get { return GetS((s) => s.pitch); }
            set { Set(value, (x, s) => s.pitch = x); }
        }

        public float SpatialBlend {
            get { return GetS((s) => s.spatialBlend); }
            set { Set(value, (x, s) => s.spatialBlend = x); }
        }

        public float DopplerLevel {
            get { return GetS((s) => s.dopplerLevel); }
            set { Set(value, (x, s) => s.dopplerLevel = x); }
        }

        public float Spread {
            get { return GetS((s) => s.spread); }
            set { Set(value, (x, s) => s.spread = x); }
        }

        public AudioRolloffMode RolloffMode {
            get { return GetS((s) => s.rolloffMode); }
            set { Set(value, (x, s) => s.rolloffMode = x); }
        }

        public float MinDistance {
            get { return GetS((s) => s.minDistance); }
            set { Set(value, (x, s) => s.minDistance = x); }
        }

        public float MaxDistance {
            get { return GetS((s) => s.maxDistance); }
            set { Set(value, (x, s) => s.maxDistance = x); }
        }

        //************************************************************************************************************************
        // Private Section
        //************************************************************************************************************************

        private void Set<T>(T value, SetValueCallback<T> set) {
            if (m_wrappedSource is null) {
                return;
            }

            set?.Invoke(value, m_wrappedSource);
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
    }
}

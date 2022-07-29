using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace AudioManager.Core {
    /// <summary>
    /// Subscribable callback that gets called whenever a value of the underlying <see cref="AudioSource"/> changes.
    /// </summary>
    /// <param name="changedSource">Source that was accessed and some of its values changed.</param>
    public delegate void SourceChangedCallback(AudioSourceWrapper changedSource);
    /// <summary>
    /// Subscribable callback that gets called for all children that fit the given <see cref="ChildType"/>, whenever <see cref="AudioSourceWrapper.InvokeChild"/> get's called.
    /// </summary>
    /// <param name="childSource">Source that we want to invoke a method on.</param>
    public delegate void InvokeCallback(AudioSource childSource);
    /// <summary>
    /// Subscribable callback that gets called for all children that fit the given <see cref="ChildType"/>, whenever <see cref="AudioSourceWrapper.InvokeChild"/> get's called
    /// and expects a return value of the given type.
    /// </summary>
    /// <param name="childSource">Source that we want to invoke a method on and return the given value with.</param>
    /// <returs>The value with the given type <see cref="T"/>.</returs>
    public delegate T InvokeCallback<T>(AudioSource childSource);
    /// <summary>
    /// Subscribable callback that gets called, when we want to change the given <see cref="AudioSourceWrapper"/> to the given value.
    /// </summary>
    /// <param name="value">Value we want to set on the given source.</param>
    /// <param name="source">Source that we want to change the given value on.</param>
    public delegate void SetCallback<T>(T value, AudioSourceWrapper source);
    public class AudioSourceWrapper {
        // Private readonly member variables.
        private readonly AudioSource m_wrappedSource;
        private readonly IDictionary<ChildType, AudioSource> m_childrenDictionary;

        // Private member variables.
        private SourceChangedCallback m_cb;

        // Private delegate helpers.
        private delegate void SetValueCallback<T>(T value, AudioSource source);
        private delegate T GetValueCallback<T>(AudioSource source);

        public AudioSourceWrapper(AudioSource source) {
            m_cb = null;
            m_wrappedSource = source;
            m_childrenDictionary = new Dictionary<ChildType, AudioSource>();
        }

        public ICollection<AudioSource> GetChildren() {
            return m_childrenDictionary.Values;
        }

        public IEnumerator<KeyValuePair<ChildType, AudioSource>> GetEnumerator() {
            return m_childrenDictionary.GetEnumerator();
        }

        public bool TryGetRegisteredChild(ChildType child, out AudioSource childSource) {
            return m_childrenDictionary.TryGetValue(child, out childSource);
        }

        public void DeregisterChildren() {
            foreach (var childSource in GetChildren()) {
                Object.Destroy(childSource);
            }
            m_childrenDictionary.Clear();
        }

        public AudioError DeregisterChild(ChildType child) {
            AudioError error = AudioError.OK;
            if (!TryGetRegisteredChild(child, out var childSource)) {
                error = AudioError.INVALID_CHILD;
                return error;
            }
            Object.Destroy(childSource);
            m_childrenDictionary.Remove(child);
            return error;
        }

        public void RegisterNewChild(ChildType child, AudioSource childSource) {
            m_childrenDictionary.Add(child, childSource);
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

        public AudioMixer Mixer {
            get { return MixerGroup.audioMixer; }
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
            m_cb?.Invoke(this);
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
            return result ?? newValue is null;
        }
    }
}

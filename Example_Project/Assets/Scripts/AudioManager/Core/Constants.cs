using UnityEngine;
using UnityEngine.Audio;

namespace AudioManager.Core {
    public struct Constants {
        // Max. progress of the sound still detactable in an IEnumerator,
        // when playing the sound with a positive pitch.
        public const float MAX_PROGRESS = 0.99f;
        // Min. progress of the sound still detactable in an IEnumerator,
        // when playing the sound with a negative pitch.
        public const float MIN_PROGRESS = 1f - MAX_PROGRESS;
        // Max. spatial blend value that still counts as 2D.
        public const float SPATIAL_BLEND_2D = 0f;
        // AudioError if the given IAudioManager is not initalized yet.
        public const AudioError NULL_AUDIO_ERROR = AudioError.NOT_INITIALIZED;

        // Default values for method parameters.
        public const float F_NULL_VALUE = float.NaN;
        public const double D_NULL_VALUE = double.NaN;
        public const float DEFAULT_VOLUME = 1f;
        public const float DEFAULT_PITCH = 1f;
        public const float DEFAULT_REVERSE_PITCH = -1f;
        public const bool DEFAULT_LOOP = false;
        public const float DEFAULT_DURATION = 1f;
        public const AudioSource DEFAULT_SOURCE = null;
        public const AudioMixerGroup DEFAULT_GROUP = null;
        public const float DEFAULT_MIN_DISTANCE = 1f;
        public const float DEFAULT_MAX_DISTANCE = 500f;
        public const float DEFAULT_BLEND = 1f;
        public const float DEFAULT_ANGLE = 0f;
        public const float DEFAULT_DOPPLER = 1f;
        public const ChildType DEFAULT_CHILD_TYPE = ChildType.PARENT;
        public const AudioRolloffMode DEFAULT_MODE = AudioRolloffMode.Logarithmic;
    }
}

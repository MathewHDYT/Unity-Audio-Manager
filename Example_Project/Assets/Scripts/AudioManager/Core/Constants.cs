using UnityEngine;
using UnityEngine.Audio;

namespace AudioManager.Core {
    public struct Constants {
        // Max. progress of the sound still detactable in an IEnumerator.
        public const float MAX_PROGRESS = 0.99f;
        // Max. spatial blend value that still counts as 2D.
        public const float SPATIAL_BLEND_2D = 0f;
        // Min. granularity value that is still valid.
        public const int MIN_GRANULARITY = 1;

        // Default values for method parameters.
        public const float DEFAULT_VOLUME = 1f;
        public const float DEFAULT_PITCH = 1f;
        public const float DEFAULT_REVERSE_PITCH = -1f;
        public const bool DEFAULT_LOOP = false;
        public const float DEFAULT_WAIT_TIME = 1f;
        public const int DEFAULT_GRANULARITY = 5;
        public const AudioSource DEFAULT_SOURCE = null;
        public const AudioMixerGroup DEFAULT_GROUP = null;
        public const float DEFAULT_MIN_DISTANCE = 1f;
        public const float DEFAULT_MAX_DISTANCE = 500f;
        public const float DEFAULT_BLEND = 1f;
        public const float DEFAULT_ANGLE = 0f;
        public const float DEFAULT_DOPPLER = 1f;
        public const AudioRolloffMode DEFAULT_MODE = AudioRolloffMode.Logarithmic;
    }
}

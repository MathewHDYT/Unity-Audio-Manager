using AudioManager.Core;
using UnityEngine;
using UnityEngine.Audio;

namespace AudioManager.Helper {
    public static class AudioSourceWrapperExtension {
        public static void SetTime(this AudioSourceWrapper source, float timeStamp) {
            source.Time = timeStamp;
        }

        public static void SetPitch(this AudioSourceWrapper source, float pitch) {
            source.Pitch = pitch;
        }

        public static void SetTimeFromCurrentPitch(this AudioSourceWrapper source) {
            float startTime = source.Source.IsReversePitch() ? source.Source.GetEndOfClip() : source.Source.GetStartOfClip();
            source.SetTime(startTime);
        }

        public static void IncreaseTime(this AudioSourceWrapper source, float time) {
            float currentTime = source.Source.ExceedsClipEnd(time) ? source.Source.GetEndOfClip() : source.Time + time;
            source.SetTime(currentTime);
        }

        public static void DecreaseTime(this AudioSourceWrapper source, float time) {
            float currentTime = source.Source.ExceedsClipStart(time) ? 0f : source.Time + time;
            source.SetTime(currentTime);
        }

        public static void SetAudioMixerGroup(this AudioSourceWrapper source, AudioMixerGroup mixerGroup) {
            source.MixerGroup = mixerGroup;
        }

        public static void Set3DAudioOptions(this AudioSourceWrapper source, float spatialBlend, float dopplerLevel, float spreadAngle, AudioRolloffMode rolloffMode, float minDistance, float maxDistance) {
            source.Spatialize = true;
            source.SpatialBlend = spatialBlend;
            source.DopplerLevel = dopplerLevel;
            source.Spread = spreadAngle;
            source.RolloffMode = rolloffMode;
            source.MinDistance = minDistance;
            source.MaxDistance = maxDistance;
        }
    }
}

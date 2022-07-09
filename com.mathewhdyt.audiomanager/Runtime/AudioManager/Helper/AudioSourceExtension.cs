using AudioManager.Core;
using UnityEngine;
using UnityEngine.Audio;

namespace AudioManager.Helper {
    public static class AudioSourceExtension {
        public static void ToggleMute(this AudioSource source) {
            source.mute = !source.mute;
        }

        public static void SetTime(this AudioSource source, float timeStamp) {
            source.time = timeStamp;
        }

        public static void SetPitch(this AudioSource source, float pitch) {
            source.pitch = pitch;
        }

        public static void SetTimeFromCurrentPitch(this AudioSource source) {
            float startTime = source.IsReversePitch() ? source.GetEndOfClip() : 0f;
            source.SetTime(startTime);
        }

        public static void SkipTime(this AudioSource source, float time) {
            if (float.IsNegative(time)) {
                source.DecreaseTime(time);
                return;
            }
            source.IncreaseTime(time);
        }

        public static void IncreaseTime(this AudioSource source, float time) {
            float currentTime = source.ExceedsClipEnd(time) ? source.GetEndOfClip() : source.time + time;
            source.SetTime(currentTime);
        }

        public static void DecreaseTime(this AudioSource source, float time) {
            float currentTime = source.ExceedsClipStart(time) ? 0f : source.time + time;
            source.SetTime(currentTime);
        }

        public static bool IsReversePitch(this AudioSource source) {
            return float.IsNegative(source.pitch);
        }

        public static bool IsProgressValid(this AudioSource source, float progress) {
            return source.IsReversePitch() ? progress >= Constants.MIN_PROGRESS : progress <= Constants.MAX_PROGRESS;
        }

        public static float GetEndOfClip(this AudioSource source) {
            return (source.clip.length * Constants.MAX_PROGRESS);
        }

        public static void TogglePause(this AudioSource source) {
            if (source.isPlaying) {
                source.Pause();
                return;
            }
            source.UnPause();
        }

        public static bool IsSameVolume(this AudioSource source, float volume) {
            return !AudioHelper.IsEndValueValid(volume, source.volume);
        }

        public static bool IsSamePitch(this AudioSource source, float pitch) {
            return !AudioHelper.IsEndValueValid(pitch, source.pitch);
        }

        public static bool ExceedsClipEnd(this AudioSource source, float time) {
            return (source.time + time) - source.clip.length > float.Epsilon;
        }

        public static bool ExceedsClipStart(this AudioSource source, float time) {
            return source.time + time < float.Epsilon;
        }

        public static void SetAudioMixerGroup(this AudioSource source, AudioMixerGroup mixerGroup) {
            source.outputAudioMixerGroup = mixerGroup;
        }

        public static bool IsAudioMixerGroupValid(this AudioSource source) {
            return source.outputAudioMixerGroup;
        }

        public static bool ProgressAchieved(this AudioSource source, float progress) {
            float currentProgress = source.GetProgress();
            bool progressAchieved = source.IsReversePitch() ? (currentProgress <= progress && currentProgress >= Constants.MIN_PROGRESS) : (currentProgress >= progress && currentProgress <= Constants.MAX_PROGRESS);
            return source.isPlaying && progressAchieved;
        }

        public static float GetClipRemainingTime(this AudioSource source) {
            float remainingTime = (source.clip.length - source.time) / source.pitch;
            return source.IsReversePitch() ? (source.clip.length + remainingTime) : remainingTime;
        }

        public static double GetClipLength(this AudioSource source) {
            return (double)source.clip.samples / source.clip.frequency;
        }

        public static bool IsSound2D(this AudioSource source) {
            return AudioHelper.IsSound2D(source.spatialBlend);
        }

        public static bool IsLengthValid(this AudioSource source, float length) {
            return length <= source.clip.length && length >= 0f;
        }

        public static float GetProgress(this AudioSource source) {
            return (float)source.timeSamples / source.clip.samples;
        }

        public static void CopyAudioSourceSettings(this AudioSource copyTo, AudioSource copyFrom) {
            Set2DAudioOptions(copyTo, copyFrom.clip, copyFrom.outputAudioMixerGroup, copyFrom.loop, copyFrom.volume, copyFrom.pitch);
            Set3DAudioOptions(copyTo, copyFrom.spatialBlend, copyFrom.dopplerLevel, copyFrom.spread, copyFrom.rolloffMode, copyFrom.minDistance, copyFrom.maxDistance);
        }

        public static void CopyAudioSourceSettings(this AudioSource copyTo, AudioClip clip, AudioMixerGroup mixerGroup, bool loop, float volume, float pitch, float spatialBlend, float dopplerLevel, float spreadAngle, AudioRolloffMode rolloffMode, float minDistance, float maxDistance) {
            Set2DAudioOptions(copyTo, clip, mixerGroup, loop, volume, pitch);
            Set3DAudioOptions(copyTo, spatialBlend, dopplerLevel, spreadAngle, rolloffMode, minDistance, maxDistance);
        }

        public static void CreateEmptyGameObject(this AudioSource parentSource, Vector3 position, Transform parent, out AudioSource newSource) {
            GameObject newGameObject = AudioHelper.CreateNewGameObject();
            Transform newTransform = AudioHelper.GetTransform(newGameObject);
            AudioHelper.SetTransformParent(newTransform, parent);
            AudioHelper.SetTransformPosition(newTransform, position);
            parentSource.AttachAudioSource(out newSource, newGameObject);
        }

        public static void CopySettingsAndGameObject(this AudioSource childSource, GameObject parent, AudioSource parentSource) {
           childSource.CopyAudioSourceSettings(parentSource);
            AudioHelper.SetTransformParent(childSource.transform, parent.transform);
        }

        public static void CopySettingsAndPosition(this AudioSource childSource, Vector3 position, AudioSource parentSource) {
            childSource.CopyAudioSourceSettings(parentSource);
            AudioHelper.SetTransformPosition(childSource.transform, position);
        }

        public static void AttachAudioSource(this AudioSource parentSource, out AudioSource newSource, GameObject newGameObject) {
            AudioHelper.AddAudioSourceComponent(newGameObject, out newSource);
            newSource.CopyAudioSourceSettings(parentSource);
        }

        public static void Set2DAudioOptions(this AudioSource source, AudioClip clip, AudioMixerGroup mixerGroup, bool loop, float volume, float pitch) {
            source.clip = clip;
            source.outputAudioMixerGroup = mixerGroup;
            source.loop = loop;
            source.volume = volume;
            source.pitch = pitch;
        }

        public static void Set3DAudioOptions(this AudioSource source, float spatialBlend, float dopplerLevel, float spreadAngle, AudioRolloffMode rolloffMode, float minDistance, float maxDistance) {
            source.spatialize = true;
            source.spatialBlend = spatialBlend;
            source.dopplerLevel = dopplerLevel;
            source.spread = spreadAngle;
            source.rolloffMode = rolloffMode;
            source.minDistance = minDistance;
            source.maxDistance = maxDistance;
        }
    }
}

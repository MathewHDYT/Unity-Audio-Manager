using AudioManager.Core;
using UnityEngine;
using UnityEngine.Audio;

namespace AudioManager.Helper {
    public static class AudioSourceExtension {
        public static void SetTime(this AudioSource source, float timeStamp) {
            source.time = timeStamp;
        }

        public static void SetPitch(this AudioSource source, float pitch) {
            source.pitch = pitch;
        }

        public static bool IsReversePitch(this AudioSource source, float pitch) {
            return pitch < 0f;
        }

        public static void SetTimeFromPitch(this AudioSource source, float pitch) {
            float startTime = source.IsReversePitch(pitch) ? source.GetEndOfClip() : 0f;
            source.SetTime(startTime);
        }

        public static float GetEndOfClip(this AudioSource source) {
            return (source.clip.length * Constants.MAX_PROGRESS);
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
            return source.time - time < float.Epsilon;
        }

        public static void IncreaseTime(this AudioSource source, float time) {
            float currentTime = source.ExceedsClipEnd(time) ? source.GetEndOfClip() : source.time + time;
            source.SetTime(currentTime);
        }

        public static void DecreaseTime(this AudioSource source, float time) {
            float currentTime = source.ExceedsClipStart(time) ? 0f : source.time - time;
            source.SetTime(currentTime);
        }

        public static AudioError TryGetGroupValue(this AudioSource source, string exposedParameterName, out float currentValue) {
            AudioError error = AudioError.OK;
            if (!source.outputAudioMixerGroup.audioMixer.GetFloat(exposedParameterName, out currentValue)) {
                error = AudioError.MIXER_NOT_EXPOSED;
            }
            return error;
        }

        public static AudioError TrySetGroupValue(this AudioSource source, string exposedParameterName, float newValue) {
            AudioError error = AudioError.OK;
            if (!source.outputAudioMixerGroup.audioMixer.SetFloat(exposedParameterName, newValue)) {
                error = AudioError.MIXER_NOT_EXPOSED;
            }
            return error;
        }

        public static AudioError TryClearGroupValue(this AudioSource source, string exposedParameterName) {
            AudioError error = AudioError.OK;
            if (!source.outputAudioMixerGroup.audioMixer.ClearFloat(exposedParameterName)) {
                error = AudioError.MIXER_NOT_EXPOSED;
            }
            return error;
        }

        public static void SetRandomPitch(this AudioSource source, float minPitch, float maxPitch) {
            source.pitch = Random.Range(minPitch, maxPitch);
        }

        public static AudioError IsSoundValid(this AudioSource source) {
            AudioError error = AudioError.OK;

            if (!source) {
                error = AudioError.MISSING_SOURCE;
            }
            else if (!source.clip) {
                error = AudioError.MISSING_CLIP;
            }
            return error;
        }

        public static bool IsAudioMixerGroupValid(this AudioSource source) {
            return source.outputAudioMixerGroup;
        }

        public static float ConvertTimeStampIntoProgress(this AudioSource source, float remainingTime) {
            // Divide the given timeStamp in the sound by its length to get a value ranging from 0 to 1,
            // then reverse the value, because the remainingTime starts from the end of the clip and not the start.
            return -(remainingTime / source.clip.length) + 1f;
        }

        public static float ConvertProgressIntoTimeStamp(this AudioSource source, float progress) {
            // Mulitply the given timeStamp by its length to get a time in the clip ranging from the start to the end,
            // then subtract the value, becuase the progress starts from the end of the clip and not the start. 
            return source.clip.length - (progress * source.clip.length);
        }

        public static bool SoundFinished(this AudioSource source, float progress) {
            return source.isPlaying && (source.GetProgress() >= progress);
        }

        public static bool IsSound2D(this AudioSource source) {
            return AudioHelper.IsSound2D(source.spatialBlend);
        }

        public static bool IsLengthValid(this AudioSource source, float length) {
            return length <= source.clip.length;
        }

        public static float GetProgress(this AudioSource source) {
            return (float)source.timeSamples / (float)source.clip.samples;
        }

        public static void SetAudioMixerGroup(this AudioSource source, AudioMixerGroup mixerGroup) {
            source.outputAudioMixerGroup = mixerGroup;
        }

        public static void CopyAudioSourceSettings(this AudioSource copyTo, AudioSource copyFrom) {
            Set2DAudioOptions(copyTo, copyFrom.clip, copyFrom.outputAudioMixerGroup, copyFrom.loop, copyFrom.volume, copyFrom.pitch);
            Set3DAudioOptions(copyTo, copyFrom.spatialBlend, copyFrom.dopplerLevel, copyFrom.spread, copyFrom.rolloffMode, copyFrom.minDistance, copyFrom.maxDistance);
        }

        public static void CopyAudioSourceSettings(this AudioSource copyTo, AudioClip clip, AudioMixerGroup mixerGroup, bool loop, float volume, float pitch, float spatialBlend, float dopplerLevel, float spreadAngle, AudioRolloffMode rolloffMode, float minDistance, float maxDistance) {
            Set2DAudioOptions(copyTo, clip, mixerGroup, loop, volume, pitch);
            Set3DAudioOptions(copyTo, spatialBlend, dopplerLevel, spreadAngle, rolloffMode, minDistance, maxDistance);
        }

        public static void CreateEmptyGameObject(this AudioSource parentSource, string name, Vector3 position, Transform parent, out AudioSource newSource) {
            GameObject newGameObject = AudioHelper.CreateNewGameObject(name);
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

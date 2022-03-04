using AudioManager.Locator;
using UnityEngine;
using UnityEngine.Audio;

namespace AudioManager.Helper {
    public static class AudioSourceExtension {
        public static void SetTime(this AudioSource source, float timeStamp) {
            source.time = timeStamp;
        }

        public static bool IsSameVolume(this AudioSource source, float volume) {
            return !AudioHelper.IsEndValueValid(volume, source.volume);
        }

        public static bool IsSamePitch(this AudioSource source, float pitch) {
            return !AudioHelper.IsEndValueValid(pitch, source.pitch);
        }

        public static bool IsSameParent(this AudioSource source, GameObject gameObject) {
            return source.gameObject == gameObject;
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

        public static bool SoundFinished(this AudioSource source) {
            return source.isPlaying && source.GetProgress() >= Constants.MAX_PROGRESS;
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

        public static AudioError CopyAudioSourceSettings(this AudioSource copyTo, AudioSource copyFrom) {
            AudioError error = Set2DAudioOptions(copyTo, copyFrom.clip, copyFrom.outputAudioMixerGroup, copyFrom.loop, copyFrom.volume, copyFrom.pitch);
            if (error != AudioError.OK) {
                return error;
            }
            return Set3DAudioOptions(copyTo, copyFrom.spatialBlend, copyFrom.dopplerLevel, copyFrom.spread, copyFrom.rolloffMode, copyFrom.minDistance, copyFrom.maxDistance);
        }

        public static AudioError CopyAudioSourceSettings(this AudioSource copyTo, AudioClip clip, AudioMixerGroup mixerGroup, bool loop, float volume, float pitch, float spatialBlend, float dopplerLevel, float spreadAngle, AudioRolloffMode rolloffMode, float minDistance, float maxDistance) {
            AudioError error = Set2DAudioOptions(copyTo, clip, mixerGroup, loop, volume, pitch);
            if (error != AudioError.OK) {
                return error;
            }
            return Set3DAudioOptions(copyTo, spatialBlend, dopplerLevel, spreadAngle, rolloffMode, minDistance, maxDistance);
        }

        public static AudioError CreateEmptyGameObject(this AudioSource parentSource, string name, Vector3 position, Transform parent, out AudioSource newSource) {
            GameObject newGameObject = AudioHelper.CreateNewGameObject(name);
            Transform newTransform = AudioHelper.GetTransform(newGameObject);
            AudioHelper.SetTransformParent(newTransform, parent);
            AudioHelper.SetTransformPosition(newTransform, position);
            return parentSource.AttachAudioSource(out newSource, newGameObject);
        }

        public static AudioError CopySettingsAndPosition(this AudioSource childSource, Vector3 position, AudioSource parentSource) {
            AudioError error = childSource.CopyAudioSourceSettings(parentSource);
            AudioHelper.SetTransformPosition(childSource.transform, position);
            return error;
        }

        public static AudioError AttachAudioSource(this AudioSource parentSource, out AudioSource newSource, GameObject newGameObject) {
            AudioHelper.AddAudioSourceComponent(newGameObject, out newSource);
            return newSource.CopyAudioSourceSettings(parentSource);
        }

        public static AudioError Set2DAudioOptions(this AudioSource source, AudioClip clip, AudioMixerGroup mixerGroup, bool loop, float volume, float pitch) {
            AudioError error = AudioError.OK;

            if (!source) {
                error = AudioError.MISSING_SOURCE;
                return error;
            }

            source.clip = clip;
            source.outputAudioMixerGroup = mixerGroup;
            source.loop = loop;
            source.volume = volume;
            source.pitch = pitch;
            return error;
        }

        public static AudioError Set3DAudioOptions(this AudioSource source, float spatialBlend, float dopplerLevel, float spreadAngle, AudioRolloffMode rolloffMode, float minDistance, float maxDistance) {
            AudioError error = AudioError.OK;

            // Check if source is null.
            if (!source) {
                error = AudioError.MISSING_SOURCE;
                return error;
            }
            else if (AudioHelper.IsSound2D(spatialBlend)) {
                error = AudioError.CAN_NOT_BE_3D;
                return error;
            }

            source.spatialize = true;
            source.spatialBlend = spatialBlend;
            source.dopplerLevel = dopplerLevel;
            source.spread = spreadAngle;
            source.rolloffMode = rolloffMode;
            source.minDistance = minDistance;
            source.maxDistance = maxDistance;
            return error;
        }
    }
}

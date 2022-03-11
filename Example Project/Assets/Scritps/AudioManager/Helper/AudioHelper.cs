using AudioManager.Locator;
using UnityEngine;
using UnityEngine.Audio;

namespace AudioManager.Helper {
    public class AudioHelper {
        public static void GetStepValueAndTime(float startValue, float endValue, float waitTime, float granularity, out float stepValue, out float stepTime) {
            float difference = endValue - startValue;
            stepValue = difference / granularity;
            stepTime = waitTime / granularity;
        }

        public static AudioError LoadAudioClipFromPath(string path, out AudioClip clip) {
            clip = Resources.Load<AudioClip>(path);
            return clip ? AudioError.OK : AudioError.INVALID_PATH;
        }

        public static bool IsGranularityValid(float granularity) {
            return granularity >= Constants.MIN_GRANULARITY;
        }

        public static AudioError AttachAudioSource(out AudioSource newSource, GameObject newGameObject, AudioClip clip, AudioMixerGroup mixerGroup, bool loop, float volume, float pitch, float spatialBlend, float dopplerLevel, float spreadAngle, AudioRolloffMode rolloffMode, float minDistance, float maxDistance) {
            AddAudioSourceComponent(newGameObject, out newSource);
            return newSource.CopyAudioSourceSettings(clip, mixerGroup, loop, volume, pitch, spatialBlend, dopplerLevel, spreadAngle, rolloffMode, minDistance, maxDistance);
        }

        public static void AddAudioSourceComponent(GameObject parent, out AudioSource source) {
            source = parent.AddComponent<AudioSource>();
        }

        public static bool IsSound2D(float spatialBlend) {
            return spatialBlend <= Constants.SPATIAL_BLEND_2D;
        }

        public static bool IsProgressValid(float progress) {
            return progress <= Constants.MAX_PROGRESS;
        }

        public static bool IsEndValueValid(float startValue, float endValue) {
            return startValue - endValue >= float.Epsilon || endValue - startValue >= float.Epsilon;
        }

        public static GameObject CreateNewGameObject(string name) {
            return new GameObject(name);
        }

        public static Transform GetTransform(GameObject gameObject) {
            return gameObject.transform;
        }

        public static void SetTransformParent(Transform child, Transform parent) {
            child.SetParent(parent);
        }

        public static void SetTransformPosition(Transform transform, Vector3 position) {
            transform.position = position;
        }
    }
}

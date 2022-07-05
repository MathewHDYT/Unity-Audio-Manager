using AudioManager.Core;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace AudioManager.Helper {
    public static class AudioHelper {

        public static AudioError LoadAudioClipFromPath(string path, out AudioClip clip) {
            clip = Resources.Load<AudioClip>(path);
            return clip ? AudioError.OK : AudioError.INVALID_PATH;
        }

        public static void AttachAudioSource(out AudioSource newSource, GameObject newGameObject, AudioClip clip, AudioMixerGroup mixerGroup, bool loop, float volume, float pitch, float spatialBlend, float dopplerLevel, float spreadAngle, AudioRolloffMode rolloffMode, float minDistance, float maxDistance) {
            AddAudioSourceComponent(newGameObject, out newSource);
            newSource.CopyAudioSourceSettings(clip, mixerGroup, loop, volume, pitch, spatialBlend, dopplerLevel, spreadAngle, rolloffMode, minDistance, maxDistance);
        }

        public static void AddAudioSourceComponent(GameObject parent, out AudioSource source) {
            source = parent.AddComponent<AudioSource>();
        }

        public static bool IsSound2D(float spatialBlend) {
            return spatialBlend <= Constants.SPATIAL_BLEND_2D;
        }

        public static bool IsEndValueValid(float startValue, float endValue) {
            return startValue - endValue >= float.Epsilon || endValue - startValue >= float.Epsilon;
        }

        public static GameObject CreateNewGameObject() {
            return new GameObject();
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

        // Originally from John Leonard French's blog with an article about different methods to fade audio in and out.
        // See https://johnleonardfrench.com/how-to-fade-audio-in-unity-i-tested-every-method-this-ones-the-best/.
        public static IEnumerator LerpGroupValueCoroutine(AudioMixer mixer, string exposedParameterName, float endValue, float duration) {
            float time = 0;

            mixer.GetFloat(exposedParameterName, out float currValue);

            while (time <= duration) {
                time += Time.deltaTime;
                float newValue = Mathf.Lerp(currValue, endValue, time / duration);
                mixer.SetFloat(exposedParameterName, newValue);
                yield return null;
            }
        }
    }
}

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace AudioManager.Settings {
    [CustomEditor(typeof(AudioSourceSetting), true)]
    public class AudioSourceSettingEditor : Editor {
        [SerializeField]
        private AudioSource preview;

        private void OnEnable() {
            preview = EditorUtility.CreateGameObjectWithHideFlags("Audio preview", HideFlags.HideAndDontSave, typeof(AudioSource)).GetComponent<AudioSource>();
        }

        private void OnDisable() {
            DestroyImmediate(preview.gameObject);
        }

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            EditorGUI.BeginDisabledGroup(serializedObject.isEditingMultipleObjects);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(new GUIContent("Play", "Will begin playback of an AudioSource with the given settings."))) {
                CopySettingsAndPlay((AudioSourceSetting)target);
            }
            else if (GUILayout.Button(new GUIContent("Stop", "Will stop the playback of the created AudioSource."))) {
                Stop();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUI.EndDisabledGroup();
        }

        private void CopySettingsAndPlay(AudioSourceSetting setting) {
            if (setting == null) {
                return;
            }

            preview.clip = setting.audioClip;
            preview.outputAudioMixerGroup = setting.mixerGroup;
            preview.loop = setting.loop;
            preview.volume = setting.volume;
            preview.pitch = setting.pitch;
            preview.spatialBlend = setting.spatialBlend;
            preview.dopplerLevel = setting.dopplerLevel;
            preview.spread = setting.spreadAngle;
            preview.rolloffMode = setting.volumeRolloff;
            preview.minDistance = setting.minDistance;
            preview.maxDistance = setting.maxDistance;
            preview.Play();
        }

        private void Stop() {
            preview.Stop();
        }
    }
}
#endif // UNITY_EDITOR

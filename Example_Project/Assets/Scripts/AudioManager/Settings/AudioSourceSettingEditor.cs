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
            //DrawDefaultInspector();

            EditorGUI.BeginDisabledGroup(serializedObject.isEditingMultipleObjects);
            if (GUILayout.Button("Preview AudioSource")) {
                CopySettingsAndPlay((AudioSourceSetting)target);
            }
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
    }
}

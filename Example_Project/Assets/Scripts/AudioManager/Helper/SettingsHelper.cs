using AudioManager.Settings;
using System.Collections.Generic;
using UnityEngine;

namespace AudioManager.Helper {
    public static class SettingsHelper {
        public static void SetupSounds(out Dictionary<string, AudioSource> sounds, AudioSourceSetting[] settings, GameObject gameObject) {
            sounds = new Dictionary<string, AudioSource>();

            if (settings is null) {
                return;
            }
            CreateAndRegisterSound(sounds, settings, gameObject);
        }

        public static void CreateAndRegisterSound(Dictionary<string, AudioSource> sounds, AudioSourceSetting[] settings, GameObject gameObject) {
            foreach (var setting in settings) {
                if (setting is null) {
                    continue;
                }
                CreateAndRegisterSound(sounds, setting, gameObject);
            }
        }

        public static void CreateAndRegisterSound(Dictionary<string, AudioSource> sounds, AudioSourceSetting setting, GameObject gameObject) {
            AudioHelper.AttachAudioSource(out setting.source, gameObject, setting.audioClip, setting.mixerGroup, setting.loop, setting.volume, setting.pitch, setting.spatialBlend, setting.dopplerLevel, setting.spreadAngle, setting.volumeRolloff, setting.minDistance, setting.maxDistance);
            if (IsSoundRegistered(sounds, setting.soundName)) {
                return;
            }
            RegisterSound(sounds, (setting.soundName, setting.source));
        }

        public static bool IsSoundRegistered(Dictionary<string, AudioSource> sounds, string soundName) {
            return sounds.ContainsKey(soundName);
        }

        public static void RegisterSound(Dictionary<string, AudioSource> sounds, (string soundName, AudioSource soundSource) keyValuePair) {
            sounds.Add(keyValuePair.soundName, keyValuePair.soundSource);
        }
    }
}

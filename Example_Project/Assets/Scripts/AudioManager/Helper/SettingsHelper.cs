using AudioManager.Core;
using AudioManager.Settings;
using System.Collections.Generic;
using UnityEngine;

namespace AudioManager.Helper {
    public static class SettingsHelper {
        public static void SetupSounds(out IDictionary<string, AudioSourceWrapper> sounds, AudioSourceSetting[] settings, GameObject gameObject) {
            sounds = new Dictionary<string, AudioSourceWrapper>();

            if (settings is null) {
                return;
            }
            CreateAndRegisterSound(sounds, settings, gameObject);
        }

        public static void CreateAndRegisterSound(IDictionary<string, AudioSourceWrapper> sounds, AudioSourceSetting[] settings, GameObject gameObject) {
            foreach (var setting in settings) {
                if (setting is null) {
                    continue;
                }
                CreateAndRegisterSound(sounds, setting, gameObject);
            }
        }

        public static void CreateAndRegisterSound(IDictionary<string, AudioSourceWrapper> sounds, AudioSourceSetting setting, GameObject gameObject) {
            AudioHelper.AttachAudioSource(out setting.source, gameObject, setting.audioClip, setting.mixerGroup, setting.loop, setting.volume, setting.pitch, setting.spatialBlend, setting.dopplerLevel, setting.spreadAngle, setting.volumeRolloff, setting.minDistance, setting.maxDistance);
            if (IsSoundRegistered(sounds, setting.soundName)) {
                return;
            }
            RegisterSound(sounds, (setting.soundName, new AudioSourceWrapper(setting.source)));
        }

        public static bool IsSoundRegistered(IDictionary<string, AudioSourceWrapper> sounds, string soundName) {
            return sounds.ContainsKey(soundName);
        }

        public static void RegisterSound(IDictionary<string, AudioSourceWrapper> sounds, (string soundName, AudioSourceWrapper soundSource) keyValuePair) {
            sounds.Add(keyValuePair.soundName, keyValuePair.soundSource);
        }
    }
}

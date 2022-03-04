using AudioManager.Helper;
using AudioManager.Locator;
using AudioManager.Logger;
using AudioManager.Settings;
using System.Collections.Generic;
using UnityEngine;

namespace AudioManager.Service {
    public class AudioManagerSettings : MonoBehaviour {
        [SerializeField]
        [Header("Hide Settings:")]
        [Tooltip("Defines how much and if the gameObject this script is attached too, should be hidden in the scene hierarchy.")]
        private HideFlags customHideFlags;

        [SerializeField]
        [Header("Logger Settings:")]
        [Tooltip("Maximum level the logger should output messages at, any priority lower and the logger will not print that message.")]
        private LoggingLevel loggingLevel;

        [SerializeField]
        [Header("Sound Settings:")]
        [Tooltip("Inital sounds that should be registered on Awake with the AudioManager and the given settings.")]
        private AudioSourceSetting[] settings;

        private void OnEnable() {
            // When the gameObject first gets enabled we set the given hideFlags.
            gameObject.hideFlags = customHideFlags;
        }

        private void Awake() {
            // Make gameObject persistent so that audio keeps playing over scene changes,
            // as all audioSources and emtpy gameObjects get attached or parented to the passed gameObject in the AudioManager constructor.
            DontDestroyOnLoad(gameObject);
            IAudioManager service = new AudioManager(SetupSounds(settings), gameObject);

            // Check if the loggingLevel is higher than none, if it is use the loggedAudioManager instead of the audioManager which adds log calls,
            // with a predefined priority before and after calling the actually impemented method in the given IAudioManager instance.
            if (loggingLevel != LoggingLevel.NONE) {
                Logger.ILogger defaultLogger = new Logger.Logger(loggingLevel);
                service = new LoggedAudioManager(defaultLogger, service, this);
            }

            // Register the service.
            ServiceLocator.RegisterService(service);
        }

        private Dictionary<string, AudioSource> SetupSounds(AudioSourceSetting[] settings) {
            var dictionary = new Dictionary<string, AudioSource>();
            foreach (var setting in settings) {
                AudioHelper.AttachAudioSource(out setting.source, gameObject, setting.audioClip, setting.mixerGroup, setting.loop, setting.volume, setting.pitch, setting.spatialBlend, setting.dopplerLevel, setting.spreadAngle, setting.volumeRolloff, setting.minDistance, setting.maxDistance);
                if(!IsSoundRegistered(dictionary, setting.soundName)) {
                    RegisterSound(dictionary, setting.soundName, setting.source);
                }
            }
            return dictionary;
        }

        private bool IsSoundRegistered(Dictionary<string, AudioSource> sounds, string soundName) {
            return sounds.ContainsKey(soundName);
        }

        private void RegisterSound(Dictionary<string, AudioSource> sounds, string soundName, AudioSource soundSource) {
            sounds.Add(soundName, soundSource);
        }
    }
}

using AudioManager.Locator;
using AudioManager.Logger;
using AudioManager.Settings;
using System.Collections.Generic;
using UnityEngine;

namespace AudioManager.Service {
    /// <summary>
    /// Holds the settings needed to register an IAudioManager with the ServiceLocator class.
    /// </summary>
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
            // When the gameObject first get's enabled we set the given hideFlags.
            gameObject.hideFlags = customHideFlags;
        }

        private void Awake() {
            // Make gameObject persistent so that audio keeps playing over scene changes,
            // as all audioSources and emtpy gameObjects get attached or parented to the passed gameObject in the AudioManager constructor.
            DontDestroyOnLoad(gameObject);
            IAudioManager service = new AudioManager(SetupSounds(settings), gameObject);

            // Check if the loggingLevel is higher than none, f it is use the loggedAudioManager instead of the audioManager which adds log calls,
            // with a predefined priority before and after calling the actually impemented method in the given IAudioManager instance.
            if (loggingLevel != LoggingLevel.NONE) {
                Logger.ILogger defaultLogger = new Logger.Logger(loggingLevel);
                service = new LoggedAudioManager(defaultLogger, service, this);
            }

            // Register the service.
            ServiceLocator.RegisterService(service);
        }

        /// <summary>
        /// Setup sounds so that the user input in the AudioSourceSetting scriptable objects,
        /// can be used to play the given sounds with the given values.
        /// </summary>
        /// <param name="settings">Array of settings we want to register with the Dictionary.</param>
        /// <returns>Dictionary that contains the AudioSource with the copied settings as well as the given name.</returns>
        private Dictionary<string, AudioSource> SetupSounds(AudioSourceSetting[] settings) {
            var dictionary = new Dictionary<string, AudioSource>();
            foreach (var setting in settings) {
                // Create AudioSource object for the AudioSourceSetting scriptable object.
                setting.source = gameObject.AddComponent<AudioSource>();
                if(!dictionary.ContainsKey(setting.soundName)) {
                    // Add the name and it's AudioSource to our dictionary.
                    dictionary.Add(setting.soundName, setting.source);
                }
                // Set the 2D values of the AudioSourceSetting attached AudioSource object.
                Set2DAudioOptions(setting);
                // Set 3D options if given of the AudioSourceSetting attached AudioSource object.
                Set3DAudioOptions(setting);
            }
            return dictionary;
        }

        /// <summary>
        /// Sets the possible given 2D parameters in the AudioSource object from the AudioSourceSettings object.
        /// </summary>
        /// <param name="setting">Object containing all settings needed to set the AudioSource options.</param>
        /// <returns>AudioError, showing wheter and how setting the 2D audio source options failed.</returns>
        private AudioError Set2DAudioOptions(AudioSourceSetting setting) {
            AudioError error = AudioError.OK;

            if (!setting.source) {
                error = AudioError.MISSING_SOURCE;
                return error;
            }

            setting.source.clip = setting.audioClip;
            setting.source.outputAudioMixerGroup = setting.mixerGroup;
            setting.source.loop = setting.loop;
            setting.source.volume = setting.volume;
            setting.source.pitch = setting.pitch;
            return error;
        }

        /// <summary>
        /// Sets the possible given 3D parameters in the AudioSource object from the AudioSourceSettings object.
        /// </summary>
        /// <param name="setting">Object containing all settings needed to set the AudioSource options.</param>
        /// <returns>AudioError, showing wheter and how setting the 3D audio source options failed.</returns>
        private AudioError Set3DAudioOptions(AudioSourceSetting setting) {
            AudioError error = AudioError.OK;

            // Check if source is null.
            if (!setting.source) {
                error = AudioError.MISSING_SOURCE;
                return error;
            }
            // Checks if 3D was even enabled in the spatialBlend.
            else if (setting.spatialBlend <= 0f) {
                error = AudioError.CAN_NOT_BE_3D;
                return error;
            }

            setting.source.spatialize = true;
            setting.source.spatialBlend = setting.spatialBlend;
            setting.source.dopplerLevel = setting.dopplerLevel;
            setting.source.spread = setting.spread;
            setting.source.rolloffMode = setting.volumeRolloff;
            setting.source.minDistance = setting.minDistance;
            setting.source.maxDistance = setting.maxDistance;
            return error;
        }
    }
}

using AudioManager.Logger;
using AudioManager.Settings;
using UnityEngine;

namespace AudioManager.Audio {
    /// <summary>
    /// Holds the settings needed to register an IAudioManager with the ServiceLocator class.
    /// </summary>
    public class AudioManagerSettings : MonoBehaviour {
        [SerializeField]
        [Header("Hide Settings:")]
        private HideFlags customHideFlags;

        [SerializeField]
        [Header("Logger Settings:")]
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
            IAudioManager service = new AudioManager(settings, gameObject);

            // Check if the loggingLevel is higher than none, f it is use the loggedAudioManager instead of the audioManager which adds log calls,
            // with a predefined priority before and after calling the actually impemented method in the given IAudioManager instance.
            if (loggingLevel != LoggingLevel.NONE) {
                service = new LoggedAudioManager(loggingLevel, this, service);
            }

            // Register the service.
            ServiceLocator.RegisterService(service);
        }
    }
}

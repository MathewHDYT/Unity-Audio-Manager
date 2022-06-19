using AudioManager.Helper;
using AudioManager.Locator;
using AudioManager.Logger;
using AudioManager.Service;
using AudioManager.Settings;
using UnityEngine;

namespace AudioManager.Provider {
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

            SettingsHelper.SetupSounds(out var sounds, settings, this.gameObject);
            ServiceLocator.RegisterService(new DefaultAudioManager(sounds, this.gameObject));
            // Only register a logger if we are in the Editor and the logging level is higher than LoggingLevel.NONE.
            // This is done to ensure no needless Debug.Log calls get made when the game is built and needs no debug output.
#if UNITY_EDITOR
            if (IsLoggingEnabled(loggingLevel)) {
                ServiceLocator.RegisterLogger(new AudioLogger(loggingLevel), this);
            }
#endif // UNITY_EDITOR
        }

        // Test methods used only for UnitTesting.
#if UNITY_EDITOR
        public void SetCustomHideFlags(HideFlags testHideFlags) {
            customHideFlags = testHideFlags;
        }

        public void SetLoggingLevel(LoggingLevel testLoggingLevel) {
            loggingLevel = testLoggingLevel;
        }

        public void SetSettings(AudioSourceSetting[] testSettings) {
            settings = testSettings;
        }

        public void TestOnEnable() {
            OnEnable();
        }

        public void TestAwake() {
            Awake();
        }
#endif // UNITY_EDITOR

        private bool IsLoggingEnabled(LoggingLevel loggingLevel) {
            return loggingLevel != LoggingLevel.NONE;
        }
    }
}

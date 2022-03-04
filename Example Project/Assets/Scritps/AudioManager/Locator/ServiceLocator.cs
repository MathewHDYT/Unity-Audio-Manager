using AudioManager.Service;

namespace AudioManager.Locator {
    public class ServiceLocator {
        // Default audio manager if nothing or null is registered.
        private static readonly NullAudioManager s_nullAudioManagerService = new NullAudioManager();
        // Audio manager instance, that implements our public API.
        private static IAudioManager s_audioManagerService = s_nullAudioManagerService;

        /// <summary>
        /// Gets the registered audio manager service instance.
        /// </summary>
        /// <returns>Registered IAudioManager implementation.</returns>
        public static IAudioManager GetService() {
            return s_audioManagerService;
        }

        /// <summary>
        /// Registers a audio manager service instance with our service provider. If it is null the default NullAudioManager service will be registered instead.
        /// </summary>
        /// <param name="service">IAudioManager implementation we want to register.</param>
        public static void RegisterService(IAudioManager service) {
            if (!IsServiceValid(service)) {
                SetDefaultService();
                return;
            }
            SetService(service);
        }

        private static bool IsServiceValid(IAudioManager service) {
            return service != null;
        }

        private static void SetDefaultService() {
            s_audioManagerService = s_nullAudioManagerService;
        }

        private static void SetService(IAudioManager service) {
            s_audioManagerService = service;
        }
    }
}

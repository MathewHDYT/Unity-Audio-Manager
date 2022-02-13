namespace AudioManager.Audio {
    /// <summary>
    /// ServiceLocator that makes the currently active IAudioManager instance globally publicly accesible.
    /// </summary>
    public class ServiceLocator {
        // Default audio manager if nothing or null is registered.
        private static NullAudioManager nullAudioManagerService = new NullAudioManager();
        // Audio manager instance, that implements our public API.
        private static IAudioManager audioManagerService = nullAudioManagerService;

        /// <summary>
        /// Gets the registered audio manager service instance.
        /// </summary>
        /// <returns>Registered IAudioManager implementation.</returns>
        public static IAudioManager GetAudioManager() {
            return audioManagerService;
        }

        /// <summary>
        /// Registers a audio manager service instance with our service provider. If it is null the default NullAudioManager service will be registered instead.
        /// </summary>
        /// <param name="service">IAudioManager implementation we want to register.</param>
        public static void RegisterService(IAudioManager service) {
            if (service == null) {
                // Revert to null service.
                audioManagerService = nullAudioManagerService;
                return;
            }
            audioManagerService = service;
        }
    }
}

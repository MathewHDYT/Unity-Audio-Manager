namespace AudioManager.Logger {
    /// <summary>
    /// Defines the given underlying Debug.Log call that should be executed by the given Log method.
    /// </summary>
    public enum LoggingType {
        NORMAL, // Debug.Log.
        WARNING, // Debug.LogWarning.
        ERROR, // Debug.LogError.
        ASSERTION // Debug.LogAssertion.
    }
}

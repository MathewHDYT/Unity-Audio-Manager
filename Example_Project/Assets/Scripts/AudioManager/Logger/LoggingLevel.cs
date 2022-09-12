namespace AudioManager.Logger {
    /// <summary>
    /// Defines the logging level of the given log call. The higher the level the more information will be printed.
    /// </summary>
    public enum LoggingLevel {
        NONE, // No logging of any messages. Improved performance because the Logger is never initiated nor called.
        LOW, // Only warnings of method executions that failed will be logged.
        INTERMEDIATE, // All above levels and a message when a method is being executed.
        HIGH, // All above levels and a message when a method has successfully executed.
        STOPWATCH // All above levels and a message with the time needed to execute the method.
    }
}

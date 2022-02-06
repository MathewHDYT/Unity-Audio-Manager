public enum LoggingLevel {
    NONE, // No logging of any messages.
    LOW, // Only warnings and method executions that failed will be logged.
    INTERMEDIATE, // All above levels and a message when a method is being executed.
    HIGH, // All above levels and a message when a method has successfully executed.
    VERBOSE // Everything.
}

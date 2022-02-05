using UnityEngine;

[System.Serializable]
public class Logger {

    // Holds the minmum log level that must be passed to the Log method,
    // so that the message actually get's printed to the console.
    private LoggingLevel logLevel = LoggingLevel.NONE;

    /// <summary>
    /// Constructor for the Logger, pass the minimal log level needed to be printed into the console.
    /// </summary>
    /// <param name="minLogLevel">Minmum log level that must be passed to the Log methods so that the message actually get's printed to the console.</param>
    public Logger(LoggingLevel minLogLevel) {
        logLevel = minLogLevel;
    }

    /// <summary>
    /// Simply logs the given message with the given type if it surpasses the previously entered minmum log level.
    /// </summary>
    /// <param name="message">Message we want to print in the console.</param>
    /// <param name="level">Level the current message should be printed in the console at.</param>
    /// <param name="type">Type of the message. (Error, Warning, etc.)</param>
    /// <param name="context">Optional object to which the message applies.</param>
    public void Log(object message, LoggingLevel level, LoggingType type, Object context = null) {
        // Check if the given level is higher than to the minumum needed logLevel,
        // if it is don't print the given message.
        if (level > logLevel) {
            return;
        }

        switch (type) {
            case LoggingType.NORMAL:
                if (context == null) {
                    Debug.Log(message);
                    break;
                }
                Debug.Log(message, context);
                break;
            case LoggingType.WARNING:
                if (context == null) {
                    Debug.LogWarning(message);
                    break;
                }
                Debug.LogWarning(message, context);
                break;
            case LoggingType.ERROR:
                if (context == null) {
                    Debug.LogError(message);
                    break;
                }
                Debug.LogError(message, context);
                break;
            case LoggingType.ASSERTION:
                if (context == null) {
                    Debug.LogAssertion(message);
                    break;
                }
                Debug.LogAssertion(message, context);
                break;
            default:
                // Unexpected LoggingType argument.
                break;
        }
    }

    /// <summary>
    /// Logs the given arguments with the given format and with the given type if it surpasses the previously entered minmum log level.
    /// </summary>
    /// <param name="format">Format we want to print the messaged in.</param>
    /// <param name="level">Level the current message should be printed in the console at.</param>
    /// <param name="type">Type of the message. (Error, Warning, etc.)</param>
    /// <param name="context">Optional object to which the message applies.</param>   
    /// <param name="args">Formatted arguements that should be printed.</param>
    public void LogFormat(string format, LoggingLevel level, LoggingType type, Object context = null, params object[] args) {
        // Check if the given level is smaller than the minumum needed logLevel,
        // if it is don't print the given message.
        if (level < logLevel) {
            return;
        }

        switch (type) {
            case LoggingType.NORMAL:
                if (context == null) {
                    Debug.LogFormat(format, args);
                    break;
                }
                Debug.LogFormat(format, args, context);
                break;
            case LoggingType.WARNING:
                if (context == null) {
                    Debug.LogWarningFormat(format, args, context);
                    break;
                }
                Debug.LogWarningFormat(format, args, context);
                break;
            case LoggingType.ERROR:
                if (context == null) {
                    Debug.LogErrorFormat(format, args, context);
                    break;
                }
                Debug.LogErrorFormat(format, args, context);
                break;
            case LoggingType.ASSERTION:
                if (context == null) {
                    Debug.LogAssertionFormat(format, args, context);
                    break;
                }
                Debug.LogAssertionFormat(format, args, context);
                break;
            default:
                // Unexpected LoggingType argument.
                break;
        }
    }

    /// <summary>
    /// Logs the given expection if it surpasses the previously entered minmum log level.
    /// </summary>
    /// <param name="exception">Exception we want to print into the console.</param>
    /// <param name="level">Level the current message should be printed in the console at.</param>
    /// <param name="context">Optional object to which the message applies.</param>
    public void LogExpection(System.Exception exception, LoggingLevel level, Object context = null) {
        // Check if the given level is smaller than the minumum needed logLevel,
        // if it is don't print the given message.
        if (level < logLevel) {
            return;
        }

        if (context == null) {
            Debug.LogException(exception);
            return;
        }

        Debug.LogException(exception, context);
    }

    /// <summary>
    /// Logs the given assertion with the given message if it surpasses the previously entered minmum log level.
    /// </summary>
    /// <param name="condition">Condition we want to print in the console.</param>
    /// <param name="message">Message we want to print in the console.</param>
    /// <param name="level">Level the current message should be printed in the console at.</param>
    /// <param name="context">Optional object to which the message applies.</param>
    public void LogAssert(bool condition, string message, LoggingLevel level, Object context = null) {
        // Check if the given level is smaller than the minumum needed logLevel,
        // if it is don't print the given message.
        if (level < logLevel) {
            return;
        }

        if (context == null) {
            Debug.Assert(condition, message);
            return;
        }

        Debug.Assert(condition, message, context);
    }

    /// <summary>
    /// Logs the given assertion arguments with the given format and with the given type if it surpasses the previously entered minmum log level.
    /// </summary>
    /// <param name="condition">Condition we want to print in the console.</param>
    /// <param name="format">Format we want to print the messaged in.</param>
    /// <param name="level">Level the current message should be printed in the console at.</param>
    /// <param name="context">Optional object to which the message applies.</param>
    /// <param name="args">Formatted arguements that should be printed.</param>
    public void LogAssertFormat(bool condition, string format, LoggingLevel level, Object context = null, params object[] args) {
        // Check if the given level is smaller than the minumum needed logLevel,
        // if it is don't print the given message.
        if (level < logLevel) {
            return;
        }

        if (context == null) {
            Debug.AssertFormat(condition, format, args);
            return;
        }

        Debug.AssertFormat(condition, format, context, args);
    }
}

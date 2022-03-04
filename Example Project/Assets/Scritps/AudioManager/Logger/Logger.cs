using UnityEngine;

namespace AudioManager.Logger {
    public class Logger : ILogger {
        // Holds the minmum log level that must be passed to the Log method,
        // so that the message actually gets printed to the console.
        private readonly LoggingLevel m_logLevel = LoggingLevel.NONE;

        /// <summary>
        /// Constructor for the Logger, pass the minimal log level needed to be printed into the console.
        /// </summary>
        /// <param name="minLogLevel">Minmum log level that must be passed to the Log methods so that the message actually get's printed to the console.</param>
        public Logger(LoggingLevel minLogLevel) {
            m_logLevel = minLogLevel;
        }

        public void Log(object message, LoggingLevel level, LoggingType type, Object context = null) {
            if (level > m_logLevel) {
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

        public void LogFormat(string format, LoggingLevel level, LoggingType type, Object context = null, params object[] args) {
            if (level < m_logLevel) {
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

        public void LogExpection(System.Exception exception, LoggingLevel level, Object context = null) {
            if (level < m_logLevel) {
                return;
            }

            if (context == null) {
                Debug.LogException(exception);
                return;
            }

            Debug.LogException(exception, context);
        }

        public void LogAssert(bool condition, string message, LoggingLevel level, Object context = null) {
            if (level < m_logLevel) {
                return;
            }

            if (context == null) {
                Debug.Assert(condition, message);
                return;
            }

            Debug.Assert(condition, message, context);
        }

        public void LogAssertFormat(bool condition, string format, LoggingLevel level, Object context = null, params object[] args) {
            if (level < m_logLevel) {
                return;
            }

            if (context == null) {
                Debug.AssertFormat(condition, format, args);
                return;
            }

            Debug.AssertFormat(condition, format, context, args);
        }
    }
}

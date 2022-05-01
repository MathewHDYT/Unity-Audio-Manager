using UnityEngine;
using UnityEngine.UI;
using AudioManager.Logger;

public class UIAudioLogger : IAudioLogger {
    // Private readonly member variables.
    // Holds the text we want to log into.
    private readonly Text m_logOutput = null;
    // Holds the minmum log level that must be passed to the Log method,
    // so that the message actually gets printed to the console.
    private readonly LoggingLevel m_logLevel = LoggingLevel.NONE;

    /// <summary>
    /// Constructor for the Logger, pass the minimal log level needed to be printed into the given Text.
    /// </summary>
    /// <param name="minLogLevel">Minmum log level that must be passed to the Log methods so that the message actually get's printed to the given text.</param>
    /// <param name="text">Textfield we want to print our logs into.</param>
    public UIAudioLogger(LoggingLevel minLogLevel, Text text) {
        m_logOutput = text;
        m_logLevel = minLogLevel;
    }

    public void Log(object message, LoggingLevel level, LoggingType type, Object context) {
        if (!CanLog(level)) {
            return;
        }

        LogIntoTextBox(message);
    }

    public void LogFormat(string format, LoggingLevel level, LoggingType type, Object context, params object[] args) {
        if (!CanLog(level)) {
            return;
        }

        LogIntoTextBox(string.Format(format, args));
    }

    public void LogException(System.Exception exception, LoggingLevel level, Object context) {
        if (!CanLog(level)) {
            return;
        }

        LogIntoTextBox(exception.Message);
    }

    public void LogAssert(bool condition, string message, LoggingLevel level, Object context) {
        if (!CanLog(level)) {
            return;
        }

        LogIntoTextBox(condition ? "Failed" : "Succeded");
        LogIntoTextBox(message);
    }

    public void LogAssertFormat(bool condition, string format, LoggingLevel level, Object context, params object[] args) {
        if (!CanLog(level)) {
            return;
        }

        LogIntoTextBox(condition ? "Failed" : "Succeded");
        LogIntoTextBox(string.Format(format, args));
    }

    private bool CanLog(LoggingLevel level) {
        return level <= m_logLevel;
    }

    private void LogIntoTextBox(object message) {
        m_logOutput.text += message;
        m_logOutput.text += System.Environment.NewLine;
    }
}

using AudioManager.Logger;
using NUnit.Framework;
using UnityEngine;

public class TestAudioLogger {
    string m_text;
    LoggingLevel m_loggingLevel;
    LoggingType m_loggingType;
    Object m_object;
    AudioLogger m_audioLogger;

    [SetUp]
    public void TestSetUp() {
        m_text = string.Empty;
        m_loggingLevel = LoggingLevel.NONE;
        m_loggingType = LoggingType.NORMAL;
        m_object = new GameObject();
        m_audioLogger = new AudioLogger(m_loggingLevel);
    }

    [Test]
    public void TestLog() {
        // Context is null.
        m_audioLogger.Log(m_text, m_loggingLevel, m_loggingType, null);
        // Context is not null.
        m_audioLogger.Log(m_text, m_loggingLevel, m_loggingType, m_object);
    }

    [Test]
    public void TestLogFormat() {
        // Context is null, arguments are null.
        m_audioLogger.LogFormat(m_text, m_loggingLevel, m_loggingType, null, null);
        // Arguments are null.
        m_audioLogger.LogFormat(m_text, m_loggingLevel, m_loggingType, m_object, null);
        // Context is null.
        m_audioLogger.LogFormat(m_text, m_loggingLevel, m_loggingType, null, m_text);
        m_audioLogger.LogFormat(m_text, m_loggingLevel, m_loggingType, m_object, m_text);
    }

    [Test]
    public void TestLogException() {
        // Context is null, exception is null.
        m_audioLogger.LogException(null, m_loggingLevel, null);
        // Exception is null.
        m_audioLogger.LogException(null, m_loggingLevel, m_object);
    }

    [Test]
    public void TestLogAssert() {
        // Context is null, assert is true.
        m_audioLogger.LogAssert(true, m_text, m_loggingLevel, null);
        // Assert is true.
        m_audioLogger.LogAssert(true, m_text, m_loggingLevel, m_object);

        // Context is null, assert is false.
        m_audioLogger.LogAssert(false, m_text, m_loggingLevel, null);
        // Assert is false.
        m_audioLogger.LogAssert(false, m_text, m_loggingLevel, m_object);
    }

    [Test]
    public void TestLogAssertFormat() {
        // Context is null, arguments are null, assert is true.
        m_audioLogger.LogAssertFormat(true, m_text, m_loggingLevel, null, null);
        // Arguments are null, assert is true.
        m_audioLogger.LogAssertFormat(true, m_text, m_loggingLevel, m_object, null);
        // Context is null, assert is true.
        m_audioLogger.LogAssertFormat(true, m_text, m_loggingLevel, null, m_text);
        // Assert is true.
        m_audioLogger.LogAssertFormat(true, m_text, m_loggingLevel, m_object, m_text);

        // Context is null, arguments are null, assert is false.
        m_audioLogger.LogAssertFormat(false, m_text, m_loggingLevel, null, null);
        // Arguments are null, assert is false.
        m_audioLogger.LogAssertFormat(false, m_text, m_loggingLevel, m_object, null);
        // Context is null, assert is false.
        m_audioLogger.LogAssertFormat(false, m_text, m_loggingLevel, null, m_text);
        // Assert is false.
        m_audioLogger.LogAssertFormat(false, m_text, m_loggingLevel, m_object, m_text);
    }
}

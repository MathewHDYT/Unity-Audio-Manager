using AudioManager.Core;
using AudioManager.Locator;
using AudioManager.Logger;
using AudioManager.Provider;
using AudioManager.Settings;
using NUnit.Framework;
using UnityEngine;

public sealed class TestAudioManagerSettings {
    string m_soundName;
    HideFlags m_hideFlags;
    GameObject m_gameObject;
    AudioManagerSettings m_settings;

    [SetUp]
    public void TestSetUp() {
        m_soundName = "Test";
        AudioSourceSetting scriptableObject = ScriptableObject.CreateInstance<AudioSourceSetting>();
        scriptableObject.soundName = m_soundName;
        AudioSourceSetting[] scriptableObjects = { scriptableObject, scriptableObject, null };
        m_hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
        m_gameObject = new GameObject();
        m_settings = m_gameObject.AddComponent<AudioManagerSettings>();
        m_settings.SetCustomHideFlags(m_hideFlags);
        m_settings.SetLoggingLevel(LoggingLevel.LOW);
        m_settings.SetSettings(scriptableObjects);
    }

    [Test]
    public void TestOnEnable() {
        Assert.AreNotEqual(m_hideFlags, m_gameObject.hideFlags);
        m_settings.TestOnEnable();
        Assert.AreEqual(m_hideFlags, m_gameObject.hideFlags);
    }

    [Test]
    public void TestAwake() {
        m_settings.TestAwake();
        IAudioManager audioManager = ServiceLocator.GetService();
#if UNITY_EDITOR
        Assert.IsTrue(audioManager is LoggedAudioManager);
#else
        Assert.IsTrue(audioManager is DefaultAudioManager);
#endif // UNITY_EDITOR
        audioManager.TryGetSource(m_soundName, out var source);
        Assert.IsNotNull(source);
    }
}

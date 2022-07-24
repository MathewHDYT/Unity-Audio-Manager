using AudioManager.Core;
using AudioManager.Service;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Audio;

public class TestAudioChainer {
    DummyAudioManager m_audioManager;

    [SetUp]
    public void TestSetUp() {
        m_audioManager = null;
    }

    [Test]
    public void TestAddSoundFromPath() {
        string name = string.Empty;
        string audioPath = string.Empty;
        float volume = float.NaN;
        float pitch = float.NaN;
        bool loop = false;
        AudioSource source = null;
        AudioMixerGroup mixerGroup = null;

        var fluentAudioManager = AudioChainer.AddSoundFromPath(m_audioManager, name, audioPath, volume, pitch, loop, source, mixerGroup);
        Assert.IsNull(fluentAudioManager);
        m_audioManager = new DummyAudioManager();

        fluentAudioManager = AudioChainer.AddSoundFromPath(m_audioManager, name, audioPath, volume, pitch, loop, source, mixerGroup);
        Assert.IsNotNull(fluentAudioManager);
        AudioError error = fluentAudioManager.Execute();
        Assert.AreEqual(AudioError.OK, error);
    }

    [Test]
    public void TestRegisterChildAt3DPos() {
        string name = string.Empty;
        Vector3 position = Vector3.zero;

        var fluentAudioManager = AudioChainer.RegisterChildAt3DPos(m_audioManager, name, position);
        Assert.IsNull(fluentAudioManager);
        m_audioManager = new DummyAudioManager();

        fluentAudioManager = AudioChainer.RegisterChildAt3DPos(m_audioManager, name, position);
        Assert.IsNotNull(fluentAudioManager);
        AudioError error = fluentAudioManager.Execute();
        Assert.AreEqual(AudioError.OK, error);
    }

    [Test]
    public void TestRegisterChildAttachedToGo() {
        string name = string.Empty;
        GameObject gameObject = null;

        var fluentAudioManager = AudioChainer.RegisterChildAttachedToGo(m_audioManager, name, gameObject);
        Assert.IsNull(fluentAudioManager);
        m_audioManager = new DummyAudioManager();

        fluentAudioManager = AudioChainer.RegisterChildAttachedToGo(m_audioManager, name, gameObject);
        Assert.IsNotNull(fluentAudioManager);
        AudioError error = fluentAudioManager.Execute();
        Assert.AreEqual(AudioError.OK, error);
    }

    [Test]
    public void TestSelectSound() {
        string name = string.Empty;
        ChildType child = ChildType.PARENT;

        var fluentAudioManager = AudioChainer.SelectSound(m_audioManager, name, child);
        Assert.IsNull(fluentAudioManager);
        m_audioManager = new DummyAudioManager();

        fluentAudioManager = AudioChainer.SelectSound(m_audioManager, name, child);
        Assert.IsNotNull(fluentAudioManager);
        AudioError error = fluentAudioManager.Execute();
        Assert.AreEqual(AudioError.OK, error);
    }
}

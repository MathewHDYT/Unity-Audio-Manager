using AudioManager.Core;
using AudioManager.Logger;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class TestLoggedAudioManager {
    string m_name;
    Object m_context;
    DummyAudioManager m_audioManager;
    DummyAudioLogger m_audioLogger;
    LoggedAudioManager m_loggedAudioManager;

    [SetUp]
    public void TestSetUp() {
        m_name = string.Empty;
        m_context = new GameObject();
        m_audioManager = new DummyAudioManager();
        m_audioLogger = new DummyAudioLogger();
        m_loggedAudioManager = new LoggedAudioManager(null, null, null);
        m_audioLogger.Logged = false;
        m_audioLogger.Context = null;
    }

    [Test]
    public void TestAddSoundFromPath() {
        const float volume = float.NaN;
        const float pitch = float.NaN;
        const bool loop = true;
        const AudioMixerGroup mixerGroup = null;
        string path = string.Empty;

        /// ---------------------------------------------
        /// Invalid case (AudioError.NOT_INITIALIZED) / Missing IAudioManager, IAudioLogger and Context
        /// ---------------------------------------------
        AudioError error = m_loggedAudioManager.AddSoundFromPath(m_name, path, volume, pitch, loop, null, mixerGroup);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing IAudioLogger and Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(null, m_audioManager, null);
        error = m_loggedAudioManager.AddSoundFromPath(m_name, path, volume, pitch, loop, null, mixerGroup);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsFalse(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, null);
        error = m_loggedAudioManager.AddSoundFromPath(m_name, path, volume, pitch, loop, null, mixerGroup);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, m_context);
        error = m_loggedAudioManager.AddSoundFromPath(m_name, path, volume, pitch, loop, null, mixerGroup);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNotNull(m_audioLogger.Context);
    }

    [Test]
    public void TestGetEnumerator() {
        /// ---------------------------------------------
        /// Invalid case (AudioError.NOT_INITIALIZED) / Missing IAudioManager, IAudioLogger and Context
        /// ---------------------------------------------
        IEnumerable<string> sounds = m_loggedAudioManager.GetEnumerator();
        Assert.IsNull(sounds);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing IAudioLogger and Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(null, m_audioManager, null);
        sounds = m_loggedAudioManager.GetEnumerator();
        Assert.IsNull(sounds);
        Assert.IsFalse(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, null);
        sounds = m_loggedAudioManager.GetEnumerator();
        Assert.IsNull(sounds);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, m_context);
        sounds = m_loggedAudioManager.GetEnumerator();
        Assert.IsNull(sounds);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNotNull(m_audioLogger.Context);
    }

    [Test]
    public void TestPlay() {
        ChildType child = ChildType.PARENT;

        /// ---------------------------------------------
        /// Invalid case (AudioError.NOT_INITIALIZED) / Missing IAudioManager, IAudioLogger and Context
        /// ---------------------------------------------
        AudioError error = m_loggedAudioManager.Play(m_name, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing IAudioLogger and Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(null, m_audioManager, null);
        error = m_loggedAudioManager.Play(m_name, child);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsFalse(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, null);
        error = m_loggedAudioManager.Play(m_name, child);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, m_context);
        error = m_loggedAudioManager.Play(m_name, child);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNotNull(m_audioLogger.Context);
    }

    [Test]
    public void TestPlayAtTimeStamp() {
        const float startTime = float.NaN;

        /// ---------------------------------------------
        /// Invalid case (AudioError.NOT_INITIALIZED) / Missing IAudioManager, IAudioLogger and Context
        /// ---------------------------------------------
        AudioError error = m_loggedAudioManager.PlayAtTimeStamp(m_name, startTime);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing IAudioLogger and Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(null, m_audioManager, null);
        error = m_loggedAudioManager.PlayAtTimeStamp(m_name, startTime);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsFalse(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, null);
        error = m_loggedAudioManager.PlayAtTimeStamp(m_name, startTime);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, m_context);
        error = m_loggedAudioManager.PlayAtTimeStamp(m_name, startTime);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNotNull(m_audioLogger.Context);
    }

    [Test]
    public void TestGetPlaybackPosition() {
        /// ---------------------------------------------
        /// Invalid case (AudioError.NOT_INITIALIZED) / Missing IAudioManager, IAudioLogger and Context
        /// ---------------------------------------------
        AudioError error = m_loggedAudioManager.GetPlaybackPosition(m_name, out float time);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.IsNaN(time);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing IAudioLogger and Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(null, m_audioManager, null);
        error = m_loggedAudioManager.GetPlaybackPosition(m_name, out time);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsNaN(time);
        Assert.IsFalse(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, null);
        error = m_loggedAudioManager.GetPlaybackPosition(m_name, out time);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsNaN(time);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, m_context);
        error = m_loggedAudioManager.GetPlaybackPosition(m_name, out time);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsNaN(time);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNotNull(m_audioLogger.Context);
    }

    [Test]
    public void TestSetPlaypbackDirection() {
        const float pitch = float.NaN;

        /// ---------------------------------------------
        /// Invalid case (AudioError.NOT_INITIALIZED) / Missing IAudioManager, IAudioLogger and Context
        /// ---------------------------------------------
        AudioError error = m_loggedAudioManager.SetPlaybackDirection(m_name, pitch);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing IAudioLogger and Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(null, m_audioManager, null);
        error = m_loggedAudioManager.SetPlaybackDirection(m_name, pitch);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsFalse(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, null);
        error = m_loggedAudioManager.SetPlaybackDirection(m_name, pitch);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, m_context);
        error = m_loggedAudioManager.SetPlaybackDirection(m_name, pitch);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNotNull(m_audioLogger.Context);
    }

    [Test]
    public void TestRegisterChildAt3DPos() {
        Vector3 position = Vector3.zero;

        /// ---------------------------------------------
        /// Invalid case (AudioError.NOT_INITIALIZED) / Missing IAudioManager, IAudioLogger and Context
        /// ---------------------------------------------
        AudioError error = m_loggedAudioManager.RegisterChildAt3DPos(m_name, position);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing IAudioLogger and Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(null, m_audioManager, null);
        error = m_loggedAudioManager.RegisterChildAt3DPos(m_name, position);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsFalse(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, null);
        error = m_loggedAudioManager.RegisterChildAt3DPos(m_name, position);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, m_context);
        error = m_loggedAudioManager.RegisterChildAt3DPos(m_name, position);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNotNull(m_audioLogger.Context);
    }

    [Test]
    public void TestRegisterChildAttachedToGo() {
        const GameObject gameObject = null;

        /// ---------------------------------------------
        /// Invalid case (AudioError.NOT_INITIALIZED) / Missing IAudioManager, IAudioLogger and Context
        /// ---------------------------------------------
        AudioError error = m_loggedAudioManager.RegisterChildAttachedToGo(m_name, gameObject);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing IAudioLogger and Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(null, m_audioManager, null);
        error = m_loggedAudioManager.RegisterChildAttachedToGo(m_name, gameObject);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsFalse(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, null);
        error = m_loggedAudioManager.RegisterChildAttachedToGo(m_name, gameObject);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, m_context);
        error = m_loggedAudioManager.RegisterChildAttachedToGo(m_name, gameObject);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNotNull(m_audioLogger.Context);
    }

    [Test]
    public void TestPlayDelayed() {
        const float startTime = float.NaN;

        /// ---------------------------------------------
        /// Invalid case (AudioError.NOT_INITIALIZED) / Missing IAudioManager, IAudioLogger and Context
        /// ---------------------------------------------
        AudioError error = m_loggedAudioManager.PlayDelayed(m_name, startTime);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing IAudioLogger and Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(null, m_audioManager, null);
        error = m_loggedAudioManager.PlayDelayed(m_name, startTime);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsFalse(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, null);
        error = m_loggedAudioManager.PlayDelayed(m_name, startTime);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, m_context);
        error = m_loggedAudioManager.PlayDelayed(m_name, startTime);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNotNull(m_audioLogger.Context);
    }

    [Test]
    public void TestPlayOneShot() {
        /// ---------------------------------------------
        /// Invalid case (AudioError.NOT_INITIALIZED) / Missing IAudioManager, IAudioLogger and Context
        /// ---------------------------------------------
        AudioError error = m_loggedAudioManager.PlayOneShot(m_name);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing IAudioLogger and Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(null, m_audioManager, null);
        error = m_loggedAudioManager.PlayOneShot(m_name);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsFalse(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, null);
        error = m_loggedAudioManager.PlayOneShot(m_name);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, m_context);
        error = m_loggedAudioManager.PlayOneShot(m_name);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNotNull(m_audioLogger.Context);
    }

    [Test]
    public void TestChangePitch() {
        const float minPitch = float.NaN;
        const float maxPitch = float.NaN;

        /// ---------------------------------------------
        /// Invalid case (AudioError.NOT_INITIALIZED) / Missing IAudioManager, IAudioLogger and Context
        /// ---------------------------------------------
        AudioError error = m_loggedAudioManager.ChangePitch(m_name, minPitch, maxPitch);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing IAudioLogger and Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(null, m_audioManager, null);
        error = m_loggedAudioManager.ChangePitch(m_name, minPitch, maxPitch);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsFalse(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, null);
        error = m_loggedAudioManager.ChangePitch(m_name, minPitch, maxPitch);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, m_context);
        error = m_loggedAudioManager.ChangePitch(m_name, minPitch, maxPitch);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNotNull(m_audioLogger.Context);
    }

    [Test]
    public void TestPlayScheduled() {
        const float delay = float.NaN;

        /// ---------------------------------------------
        /// Invalid case (AudioError.NOT_INITIALIZED) / Missing IAudioManager, IAudioLogger and Context
        /// ---------------------------------------------
        AudioError error = m_loggedAudioManager.PlayScheduled(m_name, delay);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing IAudioLogger and Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(null, m_audioManager, null);
        error = m_loggedAudioManager.PlayScheduled(m_name, delay);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsFalse(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, null);
        error = m_loggedAudioManager.PlayScheduled(m_name, delay);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, m_context);
        error = m_loggedAudioManager.PlayScheduled(m_name, delay);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNotNull(m_audioLogger.Context);
    }

    [Test]
    public void TestStop() {
        ChildType child = ChildType.PARENT;

        /// ---------------------------------------------
        /// Invalid case (AudioError.NOT_INITIALIZED) / Missing IAudioManager, IAudioLogger and Context
        /// ---------------------------------------------
        AudioError error = m_loggedAudioManager.Stop(m_name, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing IAudioLogger and Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(null, m_audioManager, null);
        error = m_loggedAudioManager.Stop(m_name, child);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsFalse(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, null);
        error = m_loggedAudioManager.Stop(m_name, child);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, m_context);
        error = m_loggedAudioManager.Stop(m_name, child);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNotNull(m_audioLogger.Context);
    }

    [Test]
    public void TestToggleMute() {
        /// ---------------------------------------------
        /// Invalid case (AudioError.NOT_INITIALIZED) / Missing IAudioManager, IAudioLogger and Context
        /// ---------------------------------------------
        AudioError error = m_loggedAudioManager.ToggleMute(m_name);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing IAudioLogger and Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(null, m_audioManager, null);
        error = m_loggedAudioManager.ToggleMute(m_name);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsFalse(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, null);
        error = m_loggedAudioManager.ToggleMute(m_name);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, m_context);
        error = m_loggedAudioManager.ToggleMute(m_name);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNotNull(m_audioLogger.Context);
    }

    [Test]
    public void TestTogglePause() {
        ChildType child = ChildType.PARENT;

        /// ---------------------------------------------
        /// Invalid case (AudioError.NOT_INITIALIZED) / Missing IAudioManager, IAudioLogger and Context
        /// ---------------------------------------------
        AudioError error = m_loggedAudioManager.TogglePause(m_name, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing IAudioLogger and Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(null, m_audioManager, null);
        error = m_loggedAudioManager.TogglePause(m_name, child);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsFalse(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, null);
        error = m_loggedAudioManager.TogglePause(m_name, child);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, m_context);
        error = m_loggedAudioManager.TogglePause(m_name, child);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNotNull(m_audioLogger.Context);
    }

    [Test]
    public void TestSubscribeSourceChanged() {
        const SourceChangedCallback callback = null;

        /// ---------------------------------------------
        /// Invalid case (AudioError.NOT_INITIALIZED) / Missing IAudioManager, IAudioLogger and Context
        /// ---------------------------------------------
        AudioError error = m_loggedAudioManager.SubscribeSourceChanged(m_name, callback);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing IAudioLogger and Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(null, m_audioManager, null);
        error = m_loggedAudioManager.SubscribeSourceChanged(m_name, callback);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsFalse(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, null);
        error = m_loggedAudioManager.SubscribeSourceChanged(m_name, callback);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, m_context);
        error = m_loggedAudioManager.SubscribeSourceChanged(m_name, callback);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNotNull(m_audioLogger.Context);
    }

    [Test]
    public void TestUnsubscribeSourceChanged() {
        const SourceChangedCallback callback = null;

        /// ---------------------------------------------
        /// Invalid case (AudioError.NOT_INITIALIZED) / Missing IAudioManager, IAudioLogger and Context
        /// ---------------------------------------------
        AudioError error = m_loggedAudioManager.UnsubscribeSourceChanged(m_name, callback);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing IAudioLogger and Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(null, m_audioManager, null);
        error = m_loggedAudioManager.UnsubscribeSourceChanged(m_name, callback);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsFalse(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, null);
        error = m_loggedAudioManager.UnsubscribeSourceChanged(m_name, callback);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, m_context);
        error = m_loggedAudioManager.UnsubscribeSourceChanged(m_name, callback);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNotNull(m_audioLogger.Context);
    }

    [Test]
    public void TestSubscribeProgressCoroutine() {
        const float progress = float.NaN;
        const ProgressCoroutineCallback callback = null;

        /// ---------------------------------------------
        /// Invalid case (AudioError.NOT_INITIALIZED) / Missing IAudioManager, IAudioLogger and Context
        /// ---------------------------------------------
        AudioError error = m_loggedAudioManager.SubscribeProgressCoroutine(m_name, progress, callback);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing IAudioLogger and Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(null, m_audioManager, null);
        error = m_loggedAudioManager.SubscribeProgressCoroutine(m_name, progress, callback);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsFalse(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, null);
        error = m_loggedAudioManager.SubscribeProgressCoroutine(m_name, progress, callback);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, m_context);
        error = m_loggedAudioManager.SubscribeProgressCoroutine(m_name, progress, callback);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNotNull(m_audioLogger.Context);
    }

    [Test]
    public void TestUnsubscribeProgressCoroutine() {
        const float progress = float.NaN;

        /// ---------------------------------------------
        /// Invalid case (AudioError.NOT_INITIALIZED) / Missing IAudioManager, IAudioLogger and Context
        /// ---------------------------------------------
        AudioError error = m_loggedAudioManager.UnsubscribeProgressCoroutine(m_name, progress);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing IAudioLogger and Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(null, m_audioManager, null);
        error = m_loggedAudioManager.UnsubscribeProgressCoroutine(m_name, progress);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsFalse(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------- -----
        /// Valid case (AudioError.OK) / Missing Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, null);
        error = m_loggedAudioManager.UnsubscribeProgressCoroutine(m_name, progress);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, m_context);
        error = m_loggedAudioManager.UnsubscribeProgressCoroutine(m_name, progress);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNotNull(m_audioLogger.Context);
    }

    [Test]
    public void TestGetProgress() {
        ChildType child = ChildType.PARENT;

        /// ---------------------------------------------
        /// Invalid case (AudioError.NOT_INITIALIZED) / Missing IAudioManager, IAudioLogger and Context
        /// ---------------------------------------------
        AudioError error = m_loggedAudioManager.GetProgress(m_name, out float progress, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.IsNaN(progress);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing IAudioLogger and Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(null, m_audioManager, null);
        error = m_loggedAudioManager.GetProgress(m_name, out progress, child);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsNaN(progress);
        Assert.IsFalse(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, null);
        error = m_loggedAudioManager.GetProgress(m_name, out progress, child);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsNaN(progress);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, m_context);
        error = m_loggedAudioManager.GetProgress(m_name, out progress, child);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsNaN(progress);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNotNull(m_audioLogger.Context);
    }

    [Test]
    public void TestTryGetSource() {
        /// ---------------------------------------------
        /// Invalid case (AudioError.NOT_INITIALIZED) / Missing IAudioManager, IAudioLogger and Context
        /// ---------------------------------------------
        AudioError error = m_loggedAudioManager.TryGetSource(m_name, out var source);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);
        Assert.IsNull(source);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing IAudioLogger and Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(null, m_audioManager, null);
        error = m_loggedAudioManager.TryGetSource(m_name, out source);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsNull(source);
        Assert.IsFalse(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, null);
        error = m_loggedAudioManager.TryGetSource(m_name, out source);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsNull(source);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, m_context);
        error = m_loggedAudioManager.TryGetSource(m_name, out source);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsNull(source);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNotNull(m_audioLogger.Context);
    }

    [Test]
    public void TestLerpPitch() {
        const float endValue = float.NaN;
        const float duration = float.NaN;
        ChildType child = ChildType.PARENT;

        /// ---------------------------------------------
        /// Invalid case (AudioError.NOT_INITIALIZED) / Missing IAudioManager, IAudioLogger and Context
        /// ---------------------------------------------
        AudioError error = m_loggedAudioManager.LerpPitch(m_name, endValue, duration, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing IAudioLogger and Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(null, m_audioManager, null);
        error = m_loggedAudioManager.LerpPitch(m_name, endValue, duration, child);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsFalse(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, null);
        error = m_loggedAudioManager.LerpPitch(m_name, endValue, duration, child);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, m_context);
        error = m_loggedAudioManager.LerpPitch(m_name, endValue, duration, child);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNotNull(m_audioLogger.Context);
    }

    [Test]
    public void TestLerpVolume() {
        const float endValue = float.NaN;
        const float duration = float.NaN;
        ChildType child = ChildType.PARENT;

        /// ---------------------------------------------
        /// Invalid case (AudioError.NOT_INITIALIZED) / Missing IAudioManager, IAudioLogger and Context
        /// ---------------------------------------------
        AudioError error = m_loggedAudioManager.LerpVolume(m_name, endValue, duration, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing IAudioLogger and Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(null, m_audioManager, null);
        error = m_loggedAudioManager.LerpVolume(m_name, endValue, duration, child);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsFalse(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, null);
        error = m_loggedAudioManager.LerpVolume(m_name, endValue, duration, child);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, m_context);
        error = m_loggedAudioManager.LerpVolume(m_name, endValue, duration, child);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNotNull(m_audioLogger.Context);
    }

    [Test]
    public void TestChangeGroupValue() {
        const float newValue = float.NaN;
        string exposedParameterName = string.Empty;

        /// ---------------------------------------------
        /// Invalid case (AudioError.NOT_INITIALIZED) / Missing IAudioManager, IAudioLogger and Context
        /// ---------------------------------------------
        AudioError error = m_loggedAudioManager.ChangeGroupValue(m_name, exposedParameterName, newValue);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing IAudioLogger and Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(null, m_audioManager, null);
        error = m_loggedAudioManager.ChangeGroupValue(m_name, exposedParameterName, newValue);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsFalse(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, null);
        error = m_loggedAudioManager.ChangeGroupValue(m_name, exposedParameterName, newValue);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, m_context);
        error = m_loggedAudioManager.ChangeGroupValue(m_name, exposedParameterName, newValue);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNotNull(m_audioLogger.Context);
    }

    [Test]
    public void TestGetGroupValue() {
        string exposedParameterName = string.Empty;

        /// ---------------------------------------------
        /// Invalid case (AudioError.NOT_INITIALIZED) / Missing IAudioManager, IAudioLogger and Context
        /// ---------------------------------------------
        AudioError error = m_loggedAudioManager.GetGroupValue(m_name, exposedParameterName, out float currentValue);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.IsNaN(currentValue);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing IAudioLogger and Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(null, m_audioManager, null);
        error = m_loggedAudioManager.GetGroupValue(m_name, exposedParameterName, out currentValue);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsNaN(currentValue);
        Assert.IsFalse(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, null);
        error = m_loggedAudioManager.GetGroupValue(m_name, exposedParameterName, out currentValue);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsNaN(currentValue);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, m_context);
        error = m_loggedAudioManager.GetGroupValue(m_name, exposedParameterName, out currentValue);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsNaN(currentValue);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNotNull(m_audioLogger.Context);
    }

    [Test]
    public void TestResetGroupValue() {
        string exposedParameterName = string.Empty;

        /// ---------------------------------------------
        /// Invalid case (AudioError.NOT_INITIALIZED) / Missing IAudioManager, IAudioLogger and Context
        /// ---------------------------------------------
        AudioError error = m_loggedAudioManager.ResetGroupValue(m_name, exposedParameterName);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing IAudioLogger and Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(null, m_audioManager, null);
        error = m_loggedAudioManager.ResetGroupValue(m_name, exposedParameterName);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsFalse(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, null);
        error = m_loggedAudioManager.ResetGroupValue(m_name, exposedParameterName);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, m_context);
        error = m_loggedAudioManager.ResetGroupValue(m_name, exposedParameterName);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNotNull(m_audioLogger.Context);
    }

    [Test]
    public void TestLerpGroupValue() {
        const float endValue = float.NaN;
        const float duration = float.NaN;
        string exposedParameterName = string.Empty;

        /// ---------------------------------------------
        /// Invalid case (AudioError.NOT_INITIALIZED) / Missing IAudioManager, IAudioLogger and Context
        /// ---------------------------------------------
        AudioError error = m_loggedAudioManager.LerpGroupValue(m_name, exposedParameterName, endValue, duration);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing IAudioLogger and Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(null, m_audioManager, null);
        error = m_loggedAudioManager.LerpGroupValue(m_name, exposedParameterName, endValue, duration);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsFalse(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, null);
        error = m_loggedAudioManager.LerpGroupValue(m_name, exposedParameterName, endValue, duration);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, m_context);
        error = m_loggedAudioManager.LerpGroupValue(m_name, exposedParameterName, endValue, duration);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNotNull(m_audioLogger.Context);
    }

    [Test]
    public void TestRemoveGroup() {
        /// ---------------------------------------------
        /// Invalid case (AudioError.NOT_INITIALIZED) / Missing IAudioManager, IAudioLogger and Context
        /// ---------------------------------------------
        AudioError error = m_loggedAudioManager.RemoveGroup(m_name);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing IAudioLogger and Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(null, m_audioManager, null);
        error = m_loggedAudioManager.RemoveGroup(m_name);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsFalse(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, null);
        error = m_loggedAudioManager.RemoveGroup(m_name);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, m_context);
        error = m_loggedAudioManager.RemoveGroup(m_name);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNotNull(m_audioLogger.Context);
    }

    [Test]
    public void TestAddGroup() {
        const AudioMixerGroup mixerGroup = null;

        /// ---------------------------------------------
        /// Invalid case (AudioError.NOT_INITIALIZED) / Missing IAudioManager, IAudioLogger and Context
        /// ---------------------------------------------
        AudioError error = m_loggedAudioManager.AddGroup(m_name, mixerGroup);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing IAudioLogger and Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(null, m_audioManager, null);
        error = m_loggedAudioManager.AddGroup(m_name, mixerGroup);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsFalse(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, null);
        error = m_loggedAudioManager.AddGroup(m_name, mixerGroup);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, m_context);
        error = m_loggedAudioManager.AddGroup(m_name, mixerGroup);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNotNull(m_audioLogger.Context);
    }

    [Test]
    public void TestRemoveSound() {
        /// ---------------------------------------------
        /// Invalid case (AudioError.NOT_INITIALIZED) / Missing IAudioManager, IAudioLogger and Context
        /// ---------------------------------------------
        AudioError error = m_loggedAudioManager.RemoveSound(m_name);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing IAudioLogger and Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(null, m_audioManager, null);
        error = m_loggedAudioManager.RemoveSound(m_name);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsFalse(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, null);
        error = m_loggedAudioManager.RemoveSound(m_name);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, m_context);
        error = m_loggedAudioManager.RemoveSound(m_name);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNotNull(m_audioLogger.Context);
    }

    [Test]
    public void TestSet3DAudioOptions() {
        const float minDistance = float.NaN;
        const float maxDistanc = float.NaN;
        const float spatialBlend = float.NaN;
        const float spreadAngle = float.NaN;
        const float dopplerLevel = float.NaN;
        const AudioRolloffMode rolloffMode = AudioRolloffMode.Logarithmic;

        /// ---------------------------------------------
        /// Invalid case (AudioError.NOT_INITIALIZED) / Missing IAudioManager, IAudioLogger and Context
        /// ---------------------------------------------
        AudioError error = m_loggedAudioManager.Set3DAudioOptions(m_name, minDistance, maxDistanc, spatialBlend, spreadAngle, dopplerLevel, rolloffMode);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing IAudioLogger and Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(null, m_audioManager, null);
        error = m_loggedAudioManager.Set3DAudioOptions(m_name, minDistance, maxDistanc, spatialBlend, spreadAngle, dopplerLevel, rolloffMode);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsFalse(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, null);
        error = m_loggedAudioManager.Set3DAudioOptions(m_name, minDistance, maxDistanc, spatialBlend, spreadAngle, dopplerLevel, rolloffMode);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, m_context);
        error = m_loggedAudioManager.Set3DAudioOptions(m_name, minDistance, maxDistanc, spatialBlend, spreadAngle, dopplerLevel, rolloffMode);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNotNull(m_audioLogger.Context);
    }

    [Test]
    public void TestSetStartTime() {
        const float startTime = float.NaN;

        /// ---------------------------------------------
        /// Invalid case (AudioError.NOT_INITIALIZED) / Missing IAudioManager, IAudioLogger and Context
        /// ---------------------------------------------
        AudioError error = m_loggedAudioManager.SetStartTime(m_name, startTime);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing IAudioLogger and Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(null, m_audioManager, null);
        error = m_loggedAudioManager.SetStartTime(m_name, startTime);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsFalse(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, null);
        error = m_loggedAudioManager.SetStartTime(m_name, startTime);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, m_context);
        error = m_loggedAudioManager.SetStartTime(m_name, startTime);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNotNull(m_audioLogger.Context);
    }

    [Test]
    public void TestSkipTime() {
        const float time = float.NaN;

        /// ---------------------------------------------
        /// Invalid case (AudioError.NOT_INITIALIZED) / Missing IAudioManager, IAudioLogger and Context
        /// ---------------------------------------------
        AudioError error = m_loggedAudioManager.SkipTime(m_name, time);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing IAudioLogger and Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(null, m_audioManager, null);
        error = m_loggedAudioManager.SkipTime(m_name, time);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsFalse(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK) / Missing Context
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, null);
        error = m_loggedAudioManager.SkipTime(m_name, time);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNull(m_audioLogger.Context);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        m_loggedAudioManager = new LoggedAudioManager(m_audioLogger, m_audioManager, m_context);
        error = m_loggedAudioManager.SkipTime(m_name, time);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_audioLogger.Logged);
        Assert.IsNotNull(m_audioLogger.Context);
    }
}

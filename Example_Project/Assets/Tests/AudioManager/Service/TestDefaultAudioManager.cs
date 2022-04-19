using AudioManager.Core;
using AudioManager.Service;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.TestTools;

public class TestDefaultAudioManager {
    string m_unregisteredAudioSourceName;
    string m_nullAudioSourceName;
    string m_audioSourceName;
    string m_clipPath;
    string m_parameterName;
    AudioClip m_clip;
    float m_clipStartTime;
    float m_clipEndTime;
    Dictionary<string, AudioSource> m_sounds;
    GameObject m_gameObject;
    AudioSource m_source;
    AudioMixerGroup m_mixerGroup;
    DefaultAudioManager m_audioManager;

    [SetUp]
    public void TestSetUp() {
        m_unregisteredAudioSourceName = "Test1";
        m_nullAudioSourceName = "Test2";
        m_audioSourceName = "Test3";
        m_clipPath = "TestClip";
        m_parameterName = "Volume";
        m_clip = Resources.Load<AudioClip>(m_clipPath);
        m_clipStartTime = m_clip.length / 100f;
        m_clipEndTime = m_clip.length * 0.99f;
        m_sounds = new Dictionary<string, AudioSource>();
        m_gameObject = new GameObject();
        m_gameObject.AddComponent<DummyMonoBehvaiour>();
        m_source = m_gameObject.AddComponent<AudioSource>();
        AudioMixer mixer = Resources.Load<AudioMixer>("Mixer");
        m_mixerGroup = mixer ? mixer.FindMatchingGroups("Master")[0] : null;
        m_sounds.Add(m_nullAudioSourceName, null);
        m_sounds.Add(m_audioSourceName, m_source);
        m_audioManager = new DefaultAudioManager(m_sounds, null);
        // Ensure AudioSource is stopped before attempting to play it.
        m_source.Stop();
    }

    [TearDown]
    public void TestTearDown() {
        Object.Destroy(m_gameObject);
    }

    [Test]
    public void TestAddSoundFromPath() {
        const string name = "Test4";
        const float volume = 0.5f;
        const float pitch = 0.5f;
        const bool loop = true;
        string path = string.Empty;

        /// ---------------------------------------------
        /// Invalid case (AudioError.INVALID_PATH)
        /// ---------------------------------------------
        AudioError error = m_audioManager.AddSoundFromPath(name, path, volume, pitch, loop, null, m_mixerGroup);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.INVALID_PATH, error);
        m_audioManager.TryGetSource(name, out AudioSource source);
        Assert.IsNull(source);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_PARENT)
        /// ---------------------------------------------
        path = m_clipPath;
        error = m_audioManager.AddSoundFromPath(m_nullAudioSourceName, path, volume, pitch, loop, null, m_mixerGroup);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_PARENT, error);
        m_audioManager.TryGetSource(name, out source);
        Assert.IsNull(source);

        /// ---------------------------------------------
        /// Invalid case (AudioError.ALREADY_EXISTS)
        /// ---------------------------------------------
        m_audioManager = new DefaultAudioManager(m_sounds, m_gameObject);
        error = m_audioManager.AddSoundFromPath(m_nullAudioSourceName, path, volume, pitch, loop, null, m_mixerGroup);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.ALREADY_EXISTS, error);
        m_audioManager.TryGetSource(name, out source);
        Assert.IsNull(source);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        error = m_audioManager.AddSoundFromPath(name, path, volume, pitch, loop, null, m_mixerGroup);
        Assert.AreEqual(AudioError.OK, error);
        m_audioManager.TryGetSource(name, out source);
        Assert.IsNotNull(source);
        Assert.AreEqual(volume, source.volume);
        Assert.AreEqual(pitch, source.volume);
        Assert.AreEqual(loop, source.loop);
        Assert.IsNotNull(source.clip);
        Assert.IsNotNull(source.outputAudioMixerGroup);
    }

    [Test]
    public void TestPlay() {
        /// ---------------------------------------------
        /// Invalid case (AudioError.DOES_NOT_EXIST)
        /// ---------------------------------------------
        AudioError error = m_audioManager.Play(m_unregisteredAudioSourceName);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.DOES_NOT_EXIST, error);
        Assert.IsFalse(m_source.isPlaying);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_SOURCE)
        /// ---------------------------------------------
        error = m_audioManager.Play(m_nullAudioSourceName);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_SOURCE, error);
        Assert.IsFalse(m_source.isPlaying);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_CLIP)
        /// ---------------------------------------------
        error = m_audioManager.Play(m_audioSourceName);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_CLIP, error);
        Assert.IsFalse(m_source.isPlaying);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        m_source.clip = m_clip;
        error = m_audioManager.Play(m_audioSourceName);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_source.isPlaying);
    }

    [UnityTest]
    public IEnumerator TestPlayAtTimeStamp() {
        const float maxDifferenceStartTime = 0.00002f;
        float startTime = m_clip.length * 2f;

        /// ---------------------------------------------
        /// Invalid case (AudioError.DOES_NOT_EXIST)
        /// ---------------------------------------------
        AudioError error = m_audioManager.PlayAtTimeStamp(m_unregisteredAudioSourceName, startTime);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.DOES_NOT_EXIST, error);
        Assert.IsFalse(m_source.isPlaying);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_SOURCE)
        /// ---------------------------------------------
        error = m_audioManager.PlayAtTimeStamp(m_nullAudioSourceName, startTime);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_SOURCE, error);
        Assert.IsFalse(m_source.isPlaying);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_CLIP)
        /// ---------------------------------------------
        error = m_audioManager.PlayAtTimeStamp(m_audioSourceName, startTime);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_CLIP, error);
        Assert.IsFalse(m_source.isPlaying);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_PARENT)
        /// ---------------------------------------------
        m_source.clip = m_clip;
        error = m_audioManager.PlayAtTimeStamp(m_audioSourceName, startTime);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_PARENT, error);
        Assert.IsFalse(m_source.isPlaying);

        /// ---------------------------------------------
        /// Invalid case (AudioError.INVALID_TIME)
        /// ---------------------------------------------
        m_audioManager = new DefaultAudioManager(m_sounds, m_gameObject);
        error = m_audioManager.PlayAtTimeStamp(m_audioSourceName, startTime);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.INVALID_TIME, error);
        Assert.IsFalse(m_source.isPlaying);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        startTime = m_clipEndTime;
        error = m_audioManager.PlayAtTimeStamp(m_audioSourceName, startTime);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_source.isPlaying);
        Assert.IsTrue(startTime - m_source.time <= maxDifferenceStartTime);
        // The startTime is only reset at the approximate end of the song, because a higher resolution isn't possible.
        // Therefore we wait a little bit more than the actual time, to ensure the startTime is actually reset.
        yield return new WaitForSeconds(m_clip.length - (startTime * 0.90f));
        Assert.IsFalse(m_source.isPlaying);
        Assert.AreEqual(0f, m_source.time);
    }

    [Test]
    public void TestGetPlaybackPosition() {
        float expectedTime = m_source.time;

        /// ---------------------------------------------
        /// Invalid case (AudioError.DOES_NOT_EXIST)
        /// ---------------------------------------------
        ValueDataError<float> valueDataError = m_audioManager.GetPlaybackPosition(m_unregisteredAudioSourceName);
        Assert.AreNotEqual(AudioError.OK, valueDataError.Error);
        Assert.AreEqual(AudioError.DOES_NOT_EXIST, valueDataError.Error);
        Assert.AreNotEqual(expectedTime, valueDataError.Value);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_SOURCE)
        /// ---------------------------------------------
        valueDataError = m_audioManager.GetPlaybackPosition(m_nullAudioSourceName);
        Assert.AreNotEqual(AudioError.OK, valueDataError.Error);
        Assert.AreEqual(AudioError.MISSING_SOURCE, valueDataError.Error);
        Assert.AreNotEqual(expectedTime, valueDataError.Value);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_CLIP)
        /// ---------------------------------------------
        valueDataError = m_audioManager.GetPlaybackPosition(m_audioSourceName);
        Assert.AreNotEqual(AudioError.OK, valueDataError.Error);
        Assert.AreEqual(AudioError.MISSING_CLIP, valueDataError.Error);
        Assert.AreNotEqual(expectedTime, valueDataError.Value);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        m_source.clip = m_clip;
        valueDataError = m_audioManager.GetPlaybackPosition(m_audioSourceName);
        Assert.AreEqual(AudioError.OK, valueDataError.Error);
        Assert.AreEqual(expectedTime, valueDataError.Value);
    }

    [Test]
    public void TestPlayAt3DPosition() {
        // We do not test if m_source is playing and the position of the attached gameObject m_source resides on,
        // because 3D methods don't use the original AudioSource but make a copy of it and attach that to a new gameObject instead.
        const float expectedSpatialBlend = 1f;
        Vector3 expectedPosition = Vector3.one;

        /// ---------------------------------------------
        /// Invalid case (AudioError.DOES_NOT_EXIST)
        /// ---------------------------------------------
        AudioError error = m_audioManager.PlayAt3DPosition(m_unregisteredAudioSourceName, expectedPosition);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.DOES_NOT_EXIST, error);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_SOURCE)
        /// ---------------------------------------------
        error = m_audioManager.PlayAt3DPosition(m_nullAudioSourceName, expectedPosition);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_SOURCE, error);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_CLIP)
        /// ---------------------------------------------
        error = m_audioManager.PlayAt3DPosition(m_audioSourceName, expectedPosition);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_CLIP, error);

        /// ---------------------------------------------
        /// Valid case (AudioError.CAN_NOT_BE_3D)
        /// ---------------------------------------------
        m_source.clip = m_clip;
        error = m_audioManager.PlayAt3DPosition(m_audioSourceName, expectedPosition);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.CAN_NOT_BE_3D, error);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_PARENT)
        /// ---------------------------------------------
        m_source.spatialBlend = expectedSpatialBlend;
        error = m_audioManager.PlayAt3DPosition(m_audioSourceName, expectedPosition);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_PARENT, error);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        m_audioManager = new DefaultAudioManager(m_sounds, m_gameObject);
        error = m_audioManager.PlayAt3DPosition(m_audioSourceName, expectedPosition);
        Assert.AreEqual(AudioError.OK, error);
    }

    [Test]
    public void TestPlayOneShotAt3DPosition() {
        // We do not test if m_source is playing and the position of the attached gameObject m_source resides on,
        // because 3D methods don't use the original AudioSource but make a copy of it and attach that to a new gameObject instead.
        const float expectedSpatialBlend = 1f;
        Vector3 expectedPosition = Vector3.one;

        /// ---------------------------------------------
        /// Invalid case (AudioError.DOES_NOT_EXIST)
        /// ---------------------------------------------
        AudioError error = m_audioManager.PlayOneShotAt3DPosition(m_unregisteredAudioSourceName, expectedPosition);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.DOES_NOT_EXIST, error);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_SOURCE)
        /// ---------------------------------------------
        error = m_audioManager.PlayOneShotAt3DPosition(m_nullAudioSourceName, expectedPosition);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_SOURCE, error);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_CLIP)
        /// ---------------------------------------------
        error = m_audioManager.PlayOneShotAt3DPosition(m_audioSourceName, expectedPosition);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_CLIP, error);

        /// ---------------------------------------------
        /// Valid case (AudioError.CAN_NOT_BE_3D)
        /// ---------------------------------------------
        m_source.clip = m_clip;
        error = m_audioManager.PlayOneShotAt3DPosition(m_audioSourceName, expectedPosition);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.CAN_NOT_BE_3D, error);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_PARENT)
        /// ---------------------------------------------
        m_source.spatialBlend = expectedSpatialBlend;
        error = m_audioManager.PlayOneShotAt3DPosition(m_audioSourceName, expectedPosition);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_PARENT, error);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        m_audioManager = new DefaultAudioManager(m_sounds, m_gameObject);
        error = m_audioManager.PlayOneShotAt3DPosition(m_audioSourceName, expectedPosition);
        Assert.AreEqual(AudioError.OK, error);
    }

    [Test]
    public void TestPlayAttachedToGameObject() {
        // We do not test if m_source is playing and is attached to the given gameObject,
        // because 3D methods don't use the original AudioSource but make a copy of it and attach that to a new gameObject instead.
        const float expectedSpatialBlend = 1f;
        GameObject expectedGameObject = null;

        /// ---------------------------------------------
        /// Invalid case (AudioError.DOES_NOT_EXIST)
        /// ---------------------------------------------
        AudioError error = m_audioManager.PlayAttachedToGameObject(m_unregisteredAudioSourceName, expectedGameObject);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.DOES_NOT_EXIST, error);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_SOURCE)
        /// ---------------------------------------------
        error = m_audioManager.PlayAttachedToGameObject(m_nullAudioSourceName, expectedGameObject);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_SOURCE, error);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_CLIP)
        /// ---------------------------------------------
        error = m_audioManager.PlayAttachedToGameObject(m_audioSourceName, expectedGameObject);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_CLIP, error);

        /// ---------------------------------------------
        /// Valid case (AudioError.CAN_NOT_BE_3D)
        /// ---------------------------------------------
        m_source.clip = m_clip;
        error = m_audioManager.PlayAttachedToGameObject(m_audioSourceName, expectedGameObject);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.CAN_NOT_BE_3D, error);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_PARENT)
        /// ---------------------------------------------
        m_source.spatialBlend = expectedSpatialBlend;
        error = m_audioManager.PlayAttachedToGameObject(m_audioSourceName, expectedGameObject);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.INVALID_PARENT, error);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        expectedGameObject = m_gameObject;
        m_audioManager = new DefaultAudioManager(m_sounds, m_gameObject);
        error = m_audioManager.PlayAttachedToGameObject(m_audioSourceName, expectedGameObject);
        Assert.AreEqual(AudioError.OK, error);
    }

    [Test]
    public void TestPlayOneShotAttachedToGameObject() {
        // We do not test if m_source is playing and is attached to the given gameObject,
        // because 3D methods don't use the original AudioSource but make a copy of it and attach that to a new gameObject instead.
        const float expectedSpatialBlend = 1f;
        GameObject expectedGameObject = null;

        /// ---------------------------------------------
        /// Invalid case (AudioError.DOES_NOT_EXIST)
        /// ---------------------------------------------
        AudioError error = m_audioManager.PlayOneShotAttachedToGameObject(m_unregisteredAudioSourceName, expectedGameObject);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.DOES_NOT_EXIST, error);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_SOURCE)
        /// ---------------------------------------------
        error = m_audioManager.PlayOneShotAttachedToGameObject(m_nullAudioSourceName, expectedGameObject);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_SOURCE, error);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_CLIP)
        /// ---------------------------------------------
        error = m_audioManager.PlayOneShotAttachedToGameObject(m_audioSourceName, expectedGameObject);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_CLIP, error);

        /// ---------------------------------------------
        /// Valid case (AudioError.CAN_NOT_BE_3D)
        /// ---------------------------------------------
        m_source.clip = m_clip;
        error = m_audioManager.PlayOneShotAttachedToGameObject(m_audioSourceName, expectedGameObject);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.CAN_NOT_BE_3D, error);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_PARENT)
        /// ---------------------------------------------
        m_source.spatialBlend = expectedSpatialBlend;
        error = m_audioManager.PlayOneShotAttachedToGameObject(m_audioSourceName, expectedGameObject);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.INVALID_PARENT, error);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        expectedGameObject = m_gameObject;
        m_audioManager = new DefaultAudioManager(m_sounds, m_gameObject);
        error = m_audioManager.PlayOneShotAttachedToGameObject(m_audioSourceName, expectedGameObject);
        Assert.AreEqual(AudioError.OK, error);
    }

    [Test]
    public void TestPlayDelayed() {
        const float delay = 1f;

        /// ---------------------------------------------
        /// Invalid case (AudioError.DOES_NOT_EXIST)
        /// ---------------------------------------------
        AudioError error = m_audioManager.PlayDelayed(m_unregisteredAudioSourceName, delay);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.DOES_NOT_EXIST, error);
        Assert.IsFalse(m_source.isPlaying);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_SOURCE)
        /// ---------------------------------------------
        error = m_audioManager.PlayDelayed(m_nullAudioSourceName, delay);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_SOURCE, error);
        Assert.IsFalse(m_source.isPlaying);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_CLIP)
        /// ---------------------------------------------
        error = m_audioManager.PlayDelayed(m_audioSourceName, delay);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_CLIP, error);
        Assert.IsFalse(m_source.isPlaying);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// --------------------------------------------- 
        m_source.clip = m_clip;
        error = m_audioManager.PlayDelayed(m_audioSourceName, delay);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_source.isPlaying);
    }

    [Test]
    public void TestPlayOneShot() {
        /// ---------------------------------------------
        /// Invalid case (AudioError.DOES_NOT_EXIST)
        /// ---------------------------------------------
        AudioError error = m_audioManager.PlayOneShot(m_unregisteredAudioSourceName);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.DOES_NOT_EXIST, error);
        Assert.IsFalse(m_source.isPlaying);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_SOURCE)
        /// ---------------------------------------------
        error = m_audioManager.PlayOneShot(m_nullAudioSourceName);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_SOURCE, error);
        Assert.IsFalse(m_source.isPlaying);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_CLIP)
        /// ---------------------------------------------
        error = m_audioManager.PlayOneShot(m_audioSourceName);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_CLIP, error);
        Assert.IsFalse(m_source.isPlaying);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// --------------------------------------------- 
        m_source.clip = m_clip;
        error = m_audioManager.PlayOneShot(m_audioSourceName);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_source.isPlaying);
    }

    [Test]
    public void TestChangePitch() {
        const float minPitch = 0.1f;
        const float maxPitch = 0.9f;

        /// ---------------------------------------------
        /// Invalid case (AudioError.DOES_NOT_EXIST)
        /// ---------------------------------------------
        AudioError error = m_audioManager.ChangePitch(m_unregisteredAudioSourceName, minPitch, maxPitch);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.DOES_NOT_EXIST, error);
        Assert.IsFalse(m_source.pitch >= minPitch && m_source.pitch <= maxPitch);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_SOURCE)
        /// ---------------------------------------------
        error = m_audioManager.ChangePitch(m_nullAudioSourceName, minPitch, maxPitch);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_SOURCE, error);
        Assert.IsFalse(m_source.pitch >= minPitch && m_source.pitch <= maxPitch);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_CLIP)
        /// ---------------------------------------------
        error = m_audioManager.ChangePitch(m_audioSourceName, minPitch, maxPitch);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_CLIP, error);
        Assert.IsFalse(m_source.pitch >= minPitch && m_source.pitch <= maxPitch);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// --------------------------------------------- 
        m_source.clip = m_clip;
        error = m_audioManager.ChangePitch(m_audioSourceName, minPitch, maxPitch);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_source.pitch >= minPitch && m_source.pitch <= maxPitch);
    }

    [Test]
    public void TestPlayScheduled() {
        const float delay = 1f;

        /// ---------------------------------------------
        /// Invalid case (AudioError.DOES_NOT_EXIST)
        /// ---------------------------------------------
        AudioError error = m_audioManager.PlayScheduled(m_unregisteredAudioSourceName, delay);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.DOES_NOT_EXIST, error);
        Assert.IsFalse(m_source.isPlaying);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_SOURCE)
        /// ---------------------------------------------
        error = m_audioManager.PlayScheduled(m_nullAudioSourceName, delay);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_SOURCE, error);
        Assert.IsFalse(m_source.isPlaying);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_CLIP)
        /// ---------------------------------------------
        error = m_audioManager.PlayScheduled(m_audioSourceName, delay);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_CLIP, error);
        Assert.IsFalse(m_source.isPlaying);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// --------------------------------------------- 
        m_source.clip = m_clip;
        error = m_audioManager.PlayScheduled(m_audioSourceName, delay);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_source.isPlaying);
    }

    [Test]
    public void TestStop() {
        // Start playing the given clip. So we can test if it was actually stop.
        m_source.clip = m_clip;
        AudioError error = m_audioManager.Play(m_audioSourceName);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_source.isPlaying);

        /// ---------------------------------------------
        /// Invalid case (AudioError.DOES_NOT_EXIST)
        /// ---------------------------------------------
        error = m_audioManager.Stop(m_unregisteredAudioSourceName);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.DOES_NOT_EXIST, error);
        Assert.IsTrue(m_source.isPlaying);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_SOURCE)
        /// ---------------------------------------------
        error = m_audioManager.Stop(m_nullAudioSourceName);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_SOURCE, error);
        Assert.IsTrue(m_source.isPlaying);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_CLIP)
        /// ---------------------------------------------
        // Can not be tested because the clip needs to be set when the AudioSource is still playing,
        // if we set it to null AudioSource.isPlaying() will become false and
        // we therefore can not test if the actual Stop() method stopped the AudioSource or not.

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        error = m_audioManager.Stop(m_audioSourceName);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsFalse(m_source.isPlaying);
    }

    [Test]
    public void TestToggleMute() {
        /// ---------------------------------------------
        /// Invalid case (AudioError.DOES_NOT_EXIST)
        /// ---------------------------------------------
        AudioError error = m_audioManager.ToggleMute(m_unregisteredAudioSourceName);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.DOES_NOT_EXIST, error);
        Assert.IsFalse(m_source.mute);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_SOURCE)
        /// ---------------------------------------------
        error = m_audioManager.ToggleMute(m_nullAudioSourceName);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_SOURCE, error);
        Assert.IsFalse(m_source.mute);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_CLIP)
        /// ---------------------------------------------
        error = m_audioManager.ToggleMute(m_audioSourceName);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_CLIP, error);
        Assert.IsFalse(m_source.mute);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        m_source.clip = m_clip;
        error = m_audioManager.ToggleMute(m_audioSourceName);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_source.mute);
        error = m_audioManager.ToggleMute(m_audioSourceName);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsFalse(m_source.mute);
    }

    [Test]
    public void TestTogglePause() {
        // Start playing the given clip. So we can test if it was actually paused.
        m_source.clip = m_clip;
        AudioError error = m_audioManager.Play(m_audioSourceName);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_source.isPlaying);

        /// ---------------------------------------------
        /// Invalid case (AudioError.DOES_NOT_EXIST)
        /// ---------------------------------------------
        error = m_audioManager.TogglePause(m_unregisteredAudioSourceName);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.DOES_NOT_EXIST, error);
        Assert.IsTrue(m_source.isPlaying);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_SOURCE)
        /// ---------------------------------------------
        error = m_audioManager.TogglePause(m_nullAudioSourceName);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_SOURCE, error);
        Assert.IsTrue(m_source.isPlaying);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_CLIP)
        /// ---------------------------------------------
        // Can not be tested because the clip needs to be set when the AudioSource is still playing,
        // if we set it to null AudioSource.isPlaying() will become false and
        // we therefore can not test if the actual TogglePause() method paused the AudioSource or not.

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        error = m_audioManager.TogglePause(m_audioSourceName);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsFalse(m_source.isPlaying);
        error = m_audioManager.TogglePause(m_audioSourceName);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_source.isPlaying);
    }

    [UnityTest]
    public IEnumerator TestSubscribeAudioFinished() {
        float remainingTime = m_clip.length * 2f;
        bool calledCallback = false;
        AudioFinishedCallback callback = (string n, float t) => {
            calledCallback = true;
            Assert.AreEqual(remainingTime, t);
        };

        /// ---------------------------------------------
        /// Invalid case (AudioError.DOES_NOT_EXIST)
        /// ---------------------------------------------
        AudioError error = m_audioManager.SubscribeAudioFinished(m_unregisteredAudioSourceName, remainingTime, callback);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.DOES_NOT_EXIST, error);
        Assert.IsFalse(calledCallback);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_SOURCE)
        /// ---------------------------------------------
        error = m_audioManager.SubscribeAudioFinished(m_nullAudioSourceName, remainingTime, callback);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_SOURCE, error);
        Assert.IsFalse(calledCallback);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_CLIP)
        /// ---------------------------------------------
        error = m_audioManager.SubscribeAudioFinished(m_audioSourceName, remainingTime, callback);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_CLIP, error);
        Assert.IsFalse(calledCallback);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_PARENT)
        /// ---------------------------------------------
        m_source.clip = m_clip;
        error = m_audioManager.SubscribeAudioFinished(m_audioSourceName, remainingTime, callback);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_PARENT, error);
        Assert.IsFalse(calledCallback);

        /// ---------------------------------------------
        /// Invalid case (AudioError.INVALID_TIME)
        /// ---------------------------------------------
        m_audioManager = new DefaultAudioManager(m_sounds, m_gameObject);
        error = m_audioManager.SubscribeAudioFinished(m_audioSourceName, remainingTime, callback);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.INVALID_TIME, error);
        Assert.IsFalse(calledCallback);

        // Start playing the given clip. So we can test if subscribing was successfull.
        error = m_audioManager.Play(m_audioSourceName);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_source.isPlaying);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        remainingTime = m_clipEndTime;
        error = m_audioManager.SubscribeAudioFinished(m_audioSourceName, remainingTime, callback);
        Assert.AreEqual(AudioError.OK, error);
        // Callback is only called at the approximate time passed, because a higher resolution isn't possible.
        // Therefore we wait a little bit more than the actual time, to ensure the callback is actually called.
        yield return new WaitForSeconds(m_clip.length - (remainingTime * 0.90f));
        Assert.IsTrue(calledCallback);
    }

    [UnityTest]
    public IEnumerator TestGetProgress() {
        /// ---------------------------------------------
        /// Invalid case (AudioError.DOES_NOT_EXIST)
        /// ---------------------------------------------
        ValueDataError<float> valueDataError = m_audioManager.GetProgress(m_unregisteredAudioSourceName);
        Assert.AreNotEqual(AudioError.OK, valueDataError.Error);
        Assert.AreEqual(AudioError.DOES_NOT_EXIST, valueDataError.Error);
        Assert.AreEqual(float.NaN, valueDataError.Value);
        Assert.AreNotEqual(m_source.time / m_clip.length, valueDataError.Value);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_SOURCE)
        /// ---------------------------------------------
        valueDataError = m_audioManager.GetProgress(m_nullAudioSourceName);
        Assert.AreNotEqual(AudioError.OK, valueDataError.Error);
        Assert.AreEqual(AudioError.MISSING_SOURCE, valueDataError.Error);
        Assert.AreEqual(float.NaN, valueDataError.Value);
        Assert.AreNotEqual(m_source.time / m_clip.length, valueDataError.Value);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_CLIP)
        /// ---------------------------------------------
        valueDataError = m_audioManager.GetProgress(m_nullAudioSourceName);
        Assert.AreNotEqual(AudioError.OK, valueDataError.Error);
        Assert.AreEqual(AudioError.MISSING_SOURCE, valueDataError.Error);
        Assert.AreEqual(float.NaN, valueDataError.Value);
        Assert.AreNotEqual(m_source.time / m_clip.length, valueDataError.Value);

        // Start playing the given clip. So we can test if getting the progress was successfull.
        m_source.clip = m_clip;
        AudioError error = m_audioManager.Play(m_audioSourceName);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_source.isPlaying);
        yield return new WaitForSeconds(m_clipStartTime);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        valueDataError = m_audioManager.GetProgress(m_audioSourceName);
        Assert.AreEqual(AudioError.OK, error);
        Assert.AreEqual(m_source.time / m_clip.length, valueDataError.Value);
    }

    [Test]
    public void TestTryGetSource() {
        /// ---------------------------------------------
        /// Invalid case (AudioError.DOES_NOT_EXIST)
        /// ---------------------------------------------
        AudioError error = m_audioManager.TryGetSource(m_unregisteredAudioSourceName, out AudioSource source);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.DOES_NOT_EXIST, error);
        Assert.IsNull(source);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_SOURCE)
        /// ---------------------------------------------
        error = m_audioManager.TryGetSource(m_nullAudioSourceName, out source);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_SOURCE, error);
        Assert.IsNull(source);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_CLIP)
        /// ---------------------------------------------
        error = m_audioManager.TryGetSource(m_audioSourceName, out source);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_CLIP, error);
        Assert.IsNotNull(source);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        m_source.clip = m_clip;
        error = m_audioManager.TryGetSource(m_audioSourceName, out source);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsNotNull(source);
    }

    [UnityTest]
    public IEnumerator TestLerpPitch() {
        const int validGranularity = 5;
        const float validEndValue = 0f;
        const float waitTime = 0f;
        float endValue = 1f;
        int granularity = 0;

        /// ---------------------------------------------
        /// Invalid case (AudioError.DOES_NOT_EXIST)
        /// ---------------------------------------------
        AudioError error = m_audioManager.LerpPitch(m_unregisteredAudioSourceName, endValue, waitTime, granularity);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.DOES_NOT_EXIST, error);
        Assert.AreEqual(endValue, m_source.pitch);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_SOURCE)
        /// ---------------------------------------------
        error = m_audioManager.LerpPitch(m_nullAudioSourceName, endValue, waitTime, granularity);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_SOURCE, error);
        Assert.AreEqual(endValue, m_source.pitch);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_CLIP)
        /// ---------------------------------------------
        error = m_audioManager.LerpPitch(m_audioSourceName, endValue, waitTime, granularity);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_CLIP, error);
        Assert.AreEqual(endValue, m_source.pitch);

        /// ---------------------------------------------
        /// Invalid case (AudioError.INVALID_END_VALUE)
        /// ---------------------------------------------
        m_source.clip = m_clip;
        error = m_audioManager.LerpPitch(m_audioSourceName, endValue, waitTime, granularity);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.INVALID_END_VALUE, error);
        Assert.AreEqual(endValue, m_source.pitch);

        /// ---------------------------------------------
        /// Invalid case (AudioError.INVALID_GRANULARITY)
        /// ---------------------------------------------
        endValue = validEndValue;
        error = m_audioManager.LerpPitch(m_audioSourceName, endValue, waitTime, granularity);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.INVALID_GRANULARITY, error);
        Assert.AreNotEqual(endValue, m_source.pitch);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_PARENT)
        /// ---------------------------------------------
        granularity = validGranularity;
        error = m_audioManager.LerpPitch(m_audioSourceName, endValue, waitTime, granularity);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_PARENT, error);
        Assert.AreNotEqual(endValue, m_source.pitch);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        m_audioManager = new DefaultAudioManager(m_sounds, m_gameObject);
        error = m_audioManager.LerpPitch(m_audioSourceName, endValue, waitTime, granularity);
        Assert.AreEqual(AudioError.OK, error);
        // Wait a little bit more than the actual time,
        // to ensure the endValue has enough time to achieve it's value and get rounded as well.
        yield return new WaitForSeconds(waitTime + 0.05f);
        Assert.AreEqual(endValue, m_source.pitch);
    }


    [UnityTest]
    public IEnumerator TestLerpVolume() {
        const int validGranularity = 5;
        const float validEndValue = 0f;
        const float waitTime = 0f;
        float endValue = 1f;
        int granularity = 0;

        /// ---------------------------------------------
        /// Invalid case (AudioError.DOES_NOT_EXIST)
        /// ---------------------------------------------
        AudioError error = m_audioManager.LerpVolume(m_unregisteredAudioSourceName, endValue, waitTime, granularity);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.DOES_NOT_EXIST, error);
        Assert.AreEqual(endValue, m_source.volume);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_SOURCE)
        /// ---------------------------------------------
        error = m_audioManager.LerpVolume(m_nullAudioSourceName, endValue, waitTime, granularity);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_SOURCE, error);
        Assert.AreEqual(endValue, m_source.volume);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_CLIP)
        /// ---------------------------------------------
        error = m_audioManager.LerpVolume(m_audioSourceName, endValue, waitTime, granularity);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_CLIP, error);
        Assert.AreEqual(endValue, m_source.volume);

        /// ---------------------------------------------
        /// Invalid case (AudioError.INVALID_END_VALUE)
        /// ---------------------------------------------
        m_source.clip = m_clip;
        error = m_audioManager.LerpVolume(m_audioSourceName, endValue, waitTime, granularity);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.INVALID_END_VALUE, error);
        Assert.AreEqual(endValue, m_source.volume);

        /// ---------------------------------------------
        /// Invalid case (AudioError.INVALID_GRANULARITY)
        /// ---------------------------------------------
        endValue = validEndValue;
        error = m_audioManager.LerpVolume(m_audioSourceName, endValue, waitTime, granularity);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.INVALID_GRANULARITY, error);
        Assert.AreNotEqual(endValue, m_source.volume);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_PARENT)
        /// ---------------------------------------------
        granularity = validGranularity;
        error = m_audioManager.LerpVolume(m_audioSourceName, endValue, waitTime, granularity);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_PARENT, error);
        Assert.AreNotEqual(endValue, m_source.volume);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        m_audioManager = new DefaultAudioManager(m_sounds, m_gameObject);
        error = m_audioManager.LerpVolume(m_audioSourceName, endValue, waitTime, granularity);
        Assert.AreEqual(AudioError.OK, error);
        // Wait a little bit more than the actual time,
        // to ensure the endValue has enough time to achieve it's value and get rounded as well.
        yield return new WaitForSeconds(waitTime + 0.05f);
        Assert.AreEqual(endValue, m_source.volume);
    }

    [Test]
    public void TestChangeGroupValue() {
        const float newValue = 10f;
        string parameterName = string.Empty;

        /// ---------------------------------------------
        /// Invalid case (AudioError.DOES_NOT_EXIST)
        /// ---------------------------------------------
        AudioError error = m_audioManager.ChangeGroupValue(m_unregisteredAudioSourceName, parameterName, newValue);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.DOES_NOT_EXIST, error);
        m_mixerGroup.audioMixer.GetFloat(parameterName, out float currentValue);
        Assert.AreNotEqual(newValue, currentValue);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_SOURCE)
        /// ---------------------------------------------
        error = m_audioManager.ChangeGroupValue(m_nullAudioSourceName, parameterName, newValue);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_SOURCE, error);
        m_mixerGroup.audioMixer.GetFloat(parameterName, out currentValue);
        Assert.AreNotEqual(newValue, currentValue);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_CLIP)
        /// ---------------------------------------------
        error = m_audioManager.ChangeGroupValue(m_audioSourceName, parameterName, newValue);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_CLIP, error);
        m_mixerGroup.audioMixer.GetFloat(parameterName, out currentValue);
        Assert.AreNotEqual(newValue, currentValue);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_MIXER_GROUP)
        /// ---------------------------------------------
        m_source.clip = m_clip;
        error = m_audioManager.ChangeGroupValue(m_audioSourceName, parameterName, newValue);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_MIXER_GROUP, error);
        m_mixerGroup.audioMixer.GetFloat(parameterName, out currentValue);
        Assert.AreNotEqual(newValue, currentValue);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MIXER_NOT_EXPOSED)
        /// ---------------------------------------------
        m_source.outputAudioMixerGroup = m_mixerGroup;
        error = m_audioManager.ChangeGroupValue(m_audioSourceName, parameterName, newValue);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MIXER_NOT_EXPOSED, error);
        m_mixerGroup.audioMixer.GetFloat(parameterName, out currentValue);
        Assert.AreNotEqual(newValue, currentValue);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        parameterName = m_parameterName;
        error = m_audioManager.ChangeGroupValue(m_audioSourceName, parameterName, newValue);
        Assert.AreEqual(AudioError.OK, error);
        m_mixerGroup.audioMixer.GetFloat(parameterName, out currentValue);
        Assert.AreEqual(newValue, currentValue);
    }

    [Test]
    public void TestGetGroupValue() {
        string parameterName = string.Empty;

        /// ---------------------------------------------
        /// Invalid case (AudioError.DOES_NOT_EXIST)
        /// ---------------------------------------------
        ValueDataError<float> valueDataError = m_audioManager.GetGroupValue(m_unregisteredAudioSourceName, parameterName);
        Assert.AreNotEqual(AudioError.OK, valueDataError.Error);
        Assert.AreEqual(AudioError.DOES_NOT_EXIST, valueDataError.Error);
        m_mixerGroup.audioMixer.GetFloat(parameterName, out float currentValue);
        Assert.AreNotEqual(currentValue, valueDataError.Value);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_SOURCE)
        /// ---------------------------------------------
        valueDataError = m_audioManager.GetGroupValue(m_nullAudioSourceName, parameterName);
        Assert.AreNotEqual(AudioError.OK, valueDataError.Error);
        Assert.AreEqual(AudioError.MISSING_SOURCE, valueDataError.Error);
        m_mixerGroup.audioMixer.GetFloat(parameterName, out currentValue);
        Assert.AreNotEqual(currentValue, valueDataError.Value);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_CLIP)
        /// ---------------------------------------------
        valueDataError = m_audioManager.GetGroupValue(m_audioSourceName, parameterName);
        Assert.AreNotEqual(AudioError.OK, valueDataError.Error);
        Assert.AreEqual(AudioError.MISSING_CLIP, valueDataError.Error);
        m_mixerGroup.audioMixer.GetFloat(parameterName, out currentValue);
        Assert.AreNotEqual(currentValue, valueDataError.Value);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_MIXER_GROUP)
        /// ---------------------------------------------
        m_source.clip = m_clip;
        valueDataError = m_audioManager.GetGroupValue(m_audioSourceName, parameterName);
        Assert.AreNotEqual(AudioError.OK, valueDataError.Error);
        Assert.AreEqual(AudioError.MISSING_MIXER_GROUP, valueDataError.Error);
        m_mixerGroup.audioMixer.GetFloat(parameterName, out currentValue);
        Assert.AreNotEqual(currentValue, valueDataError.Value);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MIXER_NOT_EXPOSED)
        /// ---------------------------------------------
        m_source.outputAudioMixerGroup = m_mixerGroup;
        valueDataError = m_audioManager.GetGroupValue(m_audioSourceName, parameterName);
        Assert.AreNotEqual(AudioError.OK, valueDataError.Error);
        Assert.AreEqual(AudioError.MIXER_NOT_EXPOSED, valueDataError.Error);
        m_mixerGroup.audioMixer.GetFloat(parameterName, out currentValue);
        Assert.AreNotEqual(currentValue, valueDataError.Value);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        parameterName = m_parameterName;
        valueDataError = m_audioManager.GetGroupValue(m_audioSourceName, parameterName);
        Assert.AreEqual(AudioError.OK, valueDataError.Error);
        m_mixerGroup.audioMixer.GetFloat(parameterName, out currentValue);
        Assert.AreEqual(currentValue, valueDataError.Value);
    }

    [Test]
    public void TestResetGroupValue() {
        const float newValue = 20f;

        // Get group value, to ensure we get the default value before setting it.
        string parameterName = m_parameterName;
        m_mixerGroup.audioMixer.GetFloat(parameterName, out float defaultParameterValue);
        // Set group value, so we can check if it resetting was actually successfull.
        m_source.clip = m_clip;
        m_source.outputAudioMixerGroup = m_mixerGroup;
        AudioError error = m_audioManager.ChangeGroupValue(m_audioSourceName, parameterName, newValue);
        Assert.AreEqual(AudioError.OK, error);
        m_mixerGroup.audioMixer.GetFloat(parameterName, out float currentValue);
        Assert.AreEqual(newValue, currentValue);
        m_source.clip = null;
        m_source.outputAudioMixerGroup = null;
        parameterName = string.Empty;

        /// ---------------------------------------------
        /// Invalid case (AudioError.DOES_NOT_EXIST)
        /// ---------------------------------------------
        error = m_audioManager.ResetGroupValue(m_unregisteredAudioSourceName, parameterName);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.DOES_NOT_EXIST, error);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_SOURCE)
        /// ---------------------------------------------
        error = m_audioManager.ResetGroupValue(m_nullAudioSourceName, parameterName);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_SOURCE, error);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_CLIP)
        /// ---------------------------------------------
        error = m_audioManager.ResetGroupValue(m_audioSourceName, parameterName);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_CLIP, error);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_MIXER_GROUP)
        /// ---------------------------------------------
        m_source.clip = m_clip;
        error = m_audioManager.ResetGroupValue(m_audioSourceName, parameterName);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_MIXER_GROUP, error);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MIXER_NOT_EXPOSED)
        /// ---------------------------------------------
        m_source.outputAudioMixerGroup = m_mixerGroup;
        error = m_audioManager.ResetGroupValue(m_audioSourceName, parameterName);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MIXER_NOT_EXPOSED, error);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        parameterName = m_parameterName;
        error = m_audioManager.ResetGroupValue(m_audioSourceName, parameterName);
        Assert.AreEqual(AudioError.OK, error);
        m_mixerGroup.audioMixer.GetFloat(parameterName, out currentValue);
        Assert.AreEqual(defaultParameterValue, currentValue);
    }

    [UnityTest]
    public IEnumerator TestLerpGroupValue() {
        const int validGranularity = 5;
        const float validEndValue = 0.5f;
        const float waitTime = 0f;
        int granularity = 0;

        // Get group value, to ensure we get an invalid end value before changing it.
        string parameterName = m_parameterName;
        m_mixerGroup.audioMixer.GetFloat(parameterName, out float endValue);
        parameterName = string.Empty;

        /// ---------------------------------------------
        /// Invalid case (AudioError.DOES_NOT_EXIST)
        /// ---------------------------------------------
        AudioError error = m_audioManager.LerpGroupValue(m_unregisteredAudioSourceName, parameterName, endValue, waitTime, granularity);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.DOES_NOT_EXIST, error);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_SOURCE)
        /// ---------------------------------------------
        error = m_audioManager.LerpGroupValue(m_nullAudioSourceName, parameterName, endValue, waitTime, granularity);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_SOURCE, error);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_CLIP)
        /// ---------------------------------------------
        error = m_audioManager.LerpGroupValue(m_audioSourceName, parameterName, endValue, waitTime, granularity);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_CLIP, error);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_MIXER_GROUP)
        /// ---------------------------------------------
        m_source.clip = m_clip;
        error = m_audioManager.LerpGroupValue(m_audioSourceName, parameterName, endValue, waitTime, granularity);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_MIXER_GROUP, error);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_PARENT)
        /// ---------------------------------------------
        m_source.outputAudioMixerGroup = m_mixerGroup;
        error = m_audioManager.LerpGroupValue(m_audioSourceName, parameterName, endValue, waitTime, granularity);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_PARENT, error);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MIXER_NOT_EXPOSED)
        /// ---------------------------------------------
        m_audioManager = new DefaultAudioManager(m_sounds, m_gameObject);
        error = m_audioManager.LerpGroupValue(m_audioSourceName, parameterName, endValue, waitTime, granularity);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MIXER_NOT_EXPOSED, error);

        /// ---------------------------------------------
        /// Invalid case (AudioError.INVALID_END_VALUE)
        /// ---------------------------------------------
        parameterName = m_parameterName;
        error = m_audioManager.LerpGroupValue(m_audioSourceName, parameterName, endValue, waitTime, granularity);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.INVALID_END_VALUE, error);

        /// ---------------------------------------------
        /// Invalid case (AudioError.INVALID_GRANULARITY)
        /// ---------------------------------------------
        endValue = validEndValue;
        error = m_audioManager.LerpGroupValue(m_audioSourceName, parameterName, endValue, waitTime, granularity);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.INVALID_GRANULARITY, error);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        granularity = validGranularity;
        error = m_audioManager.LerpGroupValue(m_audioSourceName, parameterName, endValue, waitTime, granularity);
        Assert.AreEqual(AudioError.OK, error);
        // Wait a little bit more than the actual time,
        // to ensure the endValue has enough time to achieve it's value and get rounded as well.
        yield return new WaitForSeconds(waitTime + 0.05f);
        m_mixerGroup.audioMixer.GetFloat(parameterName, out float currentValue);
        Assert.AreEqual(endValue, currentValue);
    }

    [Test]
    public void TestRemoveGroup() {
        /// ---------------------------------------------
        /// Invalid case (AudioError.DOES_NOT_EXIST)
        /// ---------------------------------------------
        m_source.outputAudioMixerGroup = m_mixerGroup;
        AudioError error = m_audioManager.RemoveGroup(m_unregisteredAudioSourceName);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.DOES_NOT_EXIST, error);
        Assert.IsNotNull(m_source.outputAudioMixerGroup);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_SOURCE)
        /// ---------------------------------------------
        error = m_audioManager.RemoveGroup(m_nullAudioSourceName);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_SOURCE, error);
        Assert.IsNotNull(m_source.outputAudioMixerGroup);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_CLIP)
        /// ---------------------------------------------
        error = m_audioManager.RemoveGroup(m_audioSourceName);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_CLIP, error);
        Assert.IsNotNull(m_source.outputAudioMixerGroup);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        m_source.clip = m_clip;
        error = m_audioManager.RemoveGroup(m_audioSourceName);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsNull(m_source.outputAudioMixerGroup);
    }

    [Test]
    public void TestAddGroup() {
        /// ---------------------------------------------
        /// Invalid case (AudioError.DOES_NOT_EXIST)
        /// ---------------------------------------------
        AudioError error = m_audioManager.AddGroup(m_unregisteredAudioSourceName, m_mixerGroup);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.DOES_NOT_EXIST, error);
        Assert.IsNull(m_source.outputAudioMixerGroup);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_SOURCE)
        /// ---------------------------------------------
        error = m_audioManager.AddGroup(m_nullAudioSourceName, m_mixerGroup);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_SOURCE, error);
        Assert.IsNull(m_source.outputAudioMixerGroup);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_CLIP)
        /// ---------------------------------------------
        error = m_audioManager.AddGroup(m_audioSourceName, m_mixerGroup);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_CLIP, error);
        Assert.IsNull(m_source.outputAudioMixerGroup);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        m_source.clip = m_clip;
        error = m_audioManager.AddGroup(m_audioSourceName, m_mixerGroup);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsNotNull(m_source.outputAudioMixerGroup);
    }

    [Test]
    public void TestRemoveSound() {
        /// ---------------------------------------------
        /// Invalid case (AudioError.DOES_NOT_EXIST)
        /// ---------------------------------------------
        AudioError error = m_audioManager.RemoveSound(m_unregisteredAudioSourceName);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.DOES_NOT_EXIST, error);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        error = m_audioManager.RemoveSound(m_audioSourceName);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsFalse(m_sounds.ContainsKey(m_audioSourceName));
    }

    [Test]
    public void TestSet3DAudioOptions() {
        const float minDistance = 10f;
        const float maxDistance = 25f;
        const float spatialBlend = 0.5f;
        const float spreadAngle = 20f;
        const float dopplerLevel = 0.5f;
        const AudioRolloffMode rolloffMode = AudioRolloffMode.Linear;

        /// ---------------------------------------------
        /// Invalid case (AudioError.DOES_NOT_EXIST)
        /// ---------------------------------------------
        AudioError error = m_audioManager.Set3DAudioOptions(m_unregisteredAudioSourceName, minDistance, maxDistance, spatialBlend, spreadAngle, dopplerLevel, rolloffMode);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.DOES_NOT_EXIST, error);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_SOURCE)
        /// ---------------------------------------------
        error = m_audioManager.Set3DAudioOptions(m_nullAudioSourceName, minDistance, maxDistance, spatialBlend, spreadAngle, dopplerLevel, rolloffMode);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_SOURCE, error);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_CLIP)
        /// ---------------------------------------------
        error = m_audioManager.Set3DAudioOptions(m_audioSourceName, minDistance, maxDistance, spatialBlend, spreadAngle, dopplerLevel, rolloffMode);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_CLIP, error);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        m_source.clip = m_clip;
        error = m_audioManager.Set3DAudioOptions(m_audioSourceName, minDistance, maxDistance, spatialBlend, spreadAngle, dopplerLevel, rolloffMode);
        Assert.AreEqual(AudioError.OK, error);
        Assert.AreEqual(minDistance, m_source.minDistance);
        Assert.AreEqual(maxDistance, m_source.maxDistance);
        Assert.AreEqual(spatialBlend, m_source.spatialBlend);
        Assert.AreEqual(spreadAngle, m_source.spread);
        Assert.AreEqual(dopplerLevel, m_source.dopplerLevel);
        Assert.AreEqual(rolloffMode, m_source.rolloffMode);
    }

    [Test]
    public void TestSetStartTime() {
        const float maxDifferenceStartTime = 0.00002f;
        float startTime = m_clip.length * 2f;

        /// ---------------------------------------------
        /// Invalid case (AudioError.DOES_NOT_EXIST)
        /// ---------------------------------------------
        AudioError error = m_audioManager.SetStartTime(m_unregisteredAudioSourceName, startTime);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.DOES_NOT_EXIST, error);
        Assert.AreNotEqual(startTime, m_source.time);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_SOURCE)
        /// ---------------------------------------------
        error = m_audioManager.SetStartTime(m_nullAudioSourceName, startTime);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_SOURCE, error);
        Assert.AreNotEqual(startTime, m_source.time);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_CLIP)
        /// ---------------------------------------------
        error = m_audioManager.SetStartTime(m_audioSourceName, startTime);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_CLIP, error);
        Assert.AreNotEqual(startTime, m_source.time);

        /// ---------------------------------------------
        /// Invalid case (AudioError.INVALID_TIME)
        /// ---------------------------------------------
        m_source.clip = m_clip;
        error = m_audioManager.SetStartTime(m_audioSourceName, startTime);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.INVALID_TIME, error);
        Assert.AreNotEqual(startTime, m_source.time);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        startTime = m_clipEndTime;
        error = m_audioManager.SetStartTime(m_audioSourceName, startTime);
        Assert.AreEqual(AudioError.OK, error);
        // The clip needs to be played to have it's time actually assigned.
        error = m_audioManager.Play(m_audioSourceName);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(startTime - m_source.time <= maxDifferenceStartTime);
    }
}

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
    string m_nullAudioSourceWrapperName;
    string m_nullAudioSourceName;
    string m_audioSourceName;
    string m_initalizedAudioSourceName;
    string m_clipPath;
    string m_parameterName;
    AudioClip m_clip;
    float m_clipStartTime;
    float m_clipEndTime;
    float m_maxDifference;
    Dictionary<string, AudioSourceWrapper> m_sounds;
    GameObject m_gameObject;
    AudioSource m_source;
    AudioSource m_initalizedSource;
    AudioSourceWrapper m_wrapper;
    AudioMixerGroup m_mixerGroup;
    DefaultAudioManager m_audioManager;
    AudioSourceWrapper m_initalizedWrapper;

    [SetUp]
    public void TestSetUp() {
        Application.targetFrameRate = 1;
        m_unregisteredAudioSourceName = "Test1";
        m_nullAudioSourceWrapperName = "Test2";
        m_nullAudioSourceName = "Test3";
        m_audioSourceName = "Test4";
        m_initalizedAudioSourceName = "Test5";
        m_clipPath = "TestClip";
        m_parameterName = "Volume";
        m_clip = Resources.Load<AudioClip>(m_clipPath);
        m_clipStartTime = m_clip.length / 100f;
        m_clipEndTime = m_clip.length * 0.95f;
        m_maxDifference = 0.00002f;
        m_sounds = new Dictionary<string, AudioSourceWrapper>();
        m_gameObject = new GameObject();
        m_gameObject.AddComponent<DummyMonoBehvaiour>();
        m_source = m_gameObject.AddComponent<AudioSource>();
        m_wrapper = new AudioSourceWrapper(m_source);
        m_initalizedSource = m_gameObject.AddComponent<AudioSource>();
        m_initalizedSource.spatialBlend = 1f;
        m_initalizedSource.clip = m_clip;
        AudioMixer mixer = Resources.Load<AudioMixer>("Mixer");
        m_mixerGroup = mixer ? mixer.FindMatchingGroups("Master")[0] : null;
        m_sounds.Add(m_nullAudioSourceWrapperName, null);
        m_sounds.Add(m_nullAudioSourceName, new AudioSourceWrapper(null));
        m_sounds.Add(m_audioSourceName, m_wrapper);
        m_initalizedWrapper = new AudioSourceWrapper(m_initalizedSource);
        m_sounds.Add(m_initalizedAudioSourceName, m_initalizedWrapper);
        m_audioManager = new DefaultAudioManager(m_sounds, null);
        // Ensure AudioSource is stopped before attempting to play it.
        m_source.Stop();
        m_initalizedSource.Stop();
    }

    [TearDown]
    public void TestTearDown() {
        Object.Destroy(m_gameObject);
    }

    [Test]
    public void TestAddSoundFromPath() {
        const string name = "Test6";
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
        m_audioManager.TryGetSource(name, out var source);
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
        Assert.AreEqual(volume, source.Volume);
        Assert.AreEqual(pitch, source.Pitch);
        Assert.AreEqual(loop, source.Loop);
        Assert.IsNotNull(source.Source.clip);
        Assert.IsNotNull(source.MixerGroup);
    }

    [Test]
    public void TestGetEnumerator() {
        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        IEnumerable<string> sounds = m_audioManager.GetEnumerator();
        Assert.IsNotNull(sounds);
    }

    [Test]
    public void TestPlay() {
        ChildType child = ChildType.AT_3D_POS;
        /// ---------------------------------------------
        /// Invalid case (AudioError.DOES_NOT_EXIST)
        /// ---------------------------------------------
        AudioError error = m_audioManager.Play(m_unregisteredAudioSourceName, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.DOES_NOT_EXIST, error);
        Assert.IsFalse(m_source.isPlaying);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_WRAPPER)
        /// ---------------------------------------------
        error = m_audioManager.Play(m_nullAudioSourceWrapperName, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_WRAPPER, error);
        Assert.IsFalse(m_source.isPlaying);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_SOURCE)
        /// ---------------------------------------------
        error = m_audioManager.Play(m_nullAudioSourceName, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_SOURCE, error);
        Assert.IsFalse(m_source.isPlaying);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_CLIP)
        /// ---------------------------------------------
        error = m_audioManager.Play(m_audioSourceName, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_CLIP, error);
        Assert.IsFalse(m_source.isPlaying);

        /// ---------------------------------------------
        /// Invalid case (AudioError.INVALID_CHILD)
        /// ---------------------------------------------
        error = m_audioManager.Play(m_initalizedAudioSourceName, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.INVALID_CHILD, error);
        Assert.IsFalse(m_initalizedSource.isPlaying);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        child = ChildType.PARENT;
        error = m_audioManager.Play(m_initalizedAudioSourceName, child);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_initalizedSource.isPlaying);
    }

    [UnityTest]
    public IEnumerator TestPlayAtTimeStamp() {
        ChildType child = ChildType.AT_3D_POS;
        float startTime = m_clip.length * 2f;

        /// ---------------------------------------------
        /// Invalid case (AudioError.DOES_NOT_EXIST)
        /// ---------------------------------------------
        AudioError error = m_audioManager.PlayAtTimeStamp(m_unregisteredAudioSourceName, startTime, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.DOES_NOT_EXIST, error);
        Assert.IsFalse(m_source.isPlaying);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_WRAPPER)
        /// ---------------------------------------------
        error = m_audioManager.PlayAtTimeStamp(m_nullAudioSourceWrapperName, startTime, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_WRAPPER, error);
        Assert.IsFalse(m_source.isPlaying);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_SOURCE)
        /// ---------------------------------------------
        error = m_audioManager.PlayAtTimeStamp(m_nullAudioSourceName, startTime, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_SOURCE, error);
        Assert.IsFalse(m_source.isPlaying);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_CLIP)
        /// ---------------------------------------------
        error = m_audioManager.PlayAtTimeStamp(m_audioSourceName, startTime, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_CLIP, error);
        Assert.IsFalse(m_source.isPlaying);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_PARENT)
        /// ---------------------------------------------
        m_source.clip = m_clip;
        error = m_audioManager.PlayAtTimeStamp(m_audioSourceName, startTime, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_PARENT, error);
        Assert.IsFalse(m_source.isPlaying);

        /// ---------------------------------------------
        /// Invalid case (AudioError.INVALID_TIME)
        /// ---------------------------------------------
        m_audioManager = new DefaultAudioManager(m_sounds, m_gameObject);
        error = m_audioManager.PlayAtTimeStamp(m_audioSourceName, startTime, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.INVALID_TIME, error);
        Assert.IsFalse(m_source.isPlaying);

        /// ---------------------------------------------
        /// Invalid case (AudioError.INVALID_CHILD)
        /// ---------------------------------------------
        startTime = 0f;
        error = m_audioManager.PlayAtTimeStamp(m_audioSourceName, startTime, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.INVALID_CHILD, error);
        Assert.IsFalse(m_source.isPlaying);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        child = ChildType.PARENT;
        error = m_audioManager.PlayAtTimeStamp(m_audioSourceName, startTime, child);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_source.isPlaying);
        Assert.IsTrue(startTime - m_source.time <= m_maxDifference);
        // The startTime is only reset at the approximate end of the song, because a higher resolution isn't possible.
        // Therefore we wait a little bit more than the actual time, to ensure the startTime is actually reset.
        yield return new WaitForSeconds(m_clip.length);
        Assert.IsFalse(m_source.isPlaying);
        Assert.AreEqual(0f, m_source.time);
    }

    [Test]
    public void TestGetPlaybackPosition() {
        ChildType child = ChildType.AT_3D_POS;
        float expectedTime = m_source.time;

        /// ---------------------------------------------
        /// Invalid case (AudioError.DOES_NOT_EXIST)
        /// ---------------------------------------------
        AudioError error = m_audioManager.GetPlaybackPosition(m_unregisteredAudioSourceName, out float time, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.DOES_NOT_EXIST, error);
        Assert.IsNaN(time);
        Assert.AreNotEqual(expectedTime, time);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_WRAPPER)
        /// ---------------------------------------------
        error = m_audioManager.GetPlaybackPosition(m_nullAudioSourceWrapperName, out time, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_WRAPPER, error);
        Assert.IsNaN(time);
        Assert.AreNotEqual(expectedTime, time);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_SOURCE)
        /// ---------------------------------------------
        error = m_audioManager.GetPlaybackPosition(m_nullAudioSourceName, out time, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_SOURCE, error);
        Assert.IsNaN(time);
        Assert.AreNotEqual(expectedTime, time);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_CLIP)
        /// ---------------------------------------------
        error = m_audioManager.GetPlaybackPosition(m_audioSourceName, out time, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_CLIP, error);
        Assert.IsNaN(time);
        Assert.AreNotEqual(expectedTime, time);

        /// ---------------------------------------------
        /// Invalid case (AudioError.INVALID_CHILD)
        /// ---------------------------------------------
        m_source.clip = m_clip;
        error = m_audioManager.GetPlaybackPosition(m_audioSourceName, out time, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.INVALID_CHILD, error);
        Assert.IsNaN(time);
        Assert.AreNotEqual(expectedTime, time);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        child = ChildType.PARENT;
        error = m_audioManager.GetPlaybackPosition(m_audioSourceName, out time, child);
        Assert.AreEqual(AudioError.OK, error);
        Assert.AreEqual(expectedTime, time);
    }

    [Test]
    public void TestSetPlaybackDirection() {
        ChildType child = ChildType.AT_3D_POS;
        float pitch = 1f;

        /// ---------------------------------------------
        /// Invalid case (AudioError.DOES_NOT_EXIST)
        /// ---------------------------------------------
        AudioError error = m_audioManager.SetPlaybackDirection(m_unregisteredAudioSourceName, pitch, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.DOES_NOT_EXIST, error);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_WRAPPER)
        /// ---------------------------------------------
        error = m_audioManager.SetPlaybackDirection(m_nullAudioSourceWrapperName, pitch, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_WRAPPER, error);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_SOURCE)
        /// ---------------------------------------------
        error = m_audioManager.SetPlaybackDirection(m_nullAudioSourceName, pitch, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_SOURCE, error);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_CLIP)
        /// ---------------------------------------------
        error = m_audioManager.SetPlaybackDirection(m_audioSourceName, pitch, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_CLIP, error);

        /// ---------------------------------------------
        /// Invalid case (AudioError.INVALID_CHILD)
        /// ---------------------------------------------
        m_source.clip = m_clip;
        error = m_audioManager.SetPlaybackDirection(m_audioSourceName, pitch, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.INVALID_CHILD, error);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        child = ChildType.PARENT;
        float endOfClip = (m_clip.length * Constants.MAX_PROGRESS);
        error = m_audioManager.SetPlaybackDirection(m_audioSourceName, pitch, child);
        Assert.AreEqual(AudioError.OK, error);
        Assert.AreEqual(pitch, m_source.pitch);
        Assert.AreEqual(0f, m_source.time);

        pitch = -1f;
        error = m_audioManager.SetPlaybackDirection(m_audioSourceName, pitch, child);
        Assert.AreEqual(AudioError.OK, error);
        Assert.AreEqual(pitch, m_source.pitch);
        // The clip needs to be played to have it's time actually assigned.
        error = m_audioManager.Play(m_audioSourceName, ChildType.PARENT);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(endOfClip - m_source.time <= m_maxDifference);
    }

    [Test]
    public void TestRegisterChildAt3DPos() {
        ChildType expectedChild = ChildType.AT_3D_POS;
        const float expectedSpatialBlend = 1f;
        Vector3 expectedPosition = Vector3.zero;

        /// ---------------------------------------------
        /// Invalid case (AudioError.DOES_NOT_EXIST)
        /// ---------------------------------------------
        AudioError error = m_audioManager.RegisterChildAt3DPos(m_unregisteredAudioSourceName, expectedPosition, out ChildType child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.DOES_NOT_EXIST, error);
        Assert.AreEqual(expectedChild, child);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_WRAPPER)
        /// ---------------------------------------------
        error = m_audioManager.RegisterChildAt3DPos(m_nullAudioSourceWrapperName, expectedPosition, out child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_WRAPPER, error);
        Assert.AreEqual(expectedChild, child);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_SOURCE)
        /// ---------------------------------------------
        error = m_audioManager.RegisterChildAt3DPos(m_nullAudioSourceName, expectedPosition, out child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_SOURCE, error);
        Assert.AreEqual(expectedChild, child);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_CLIP)
        /// ---------------------------------------------
        error = m_audioManager.RegisterChildAt3DPos(m_audioSourceName, expectedPosition, out child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_CLIP, error);
        Assert.AreEqual(expectedChild, child);

        /// ---------------------------------------------
        /// Valid case (AudioError.CAN_NOT_BE_3D)
        /// ---------------------------------------------
        m_source.clip = m_clip;
        error = m_audioManager.RegisterChildAt3DPos(m_audioSourceName, expectedPosition, out child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.CAN_NOT_BE_3D, error);
        Assert.AreEqual(expectedChild, child);

        /// ---------------------------------------------
        /// Invalid case (AudioError.INVALID_PARENT)
        /// ---------------------------------------------
        m_source.spatialBlend = expectedSpatialBlend;
        error = m_audioManager.RegisterChildAt3DPos(m_audioSourceName, expectedPosition, out child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.INVALID_PARENT, error);
        Assert.AreEqual(expectedChild, child);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        m_audioManager = new DefaultAudioManager(m_sounds, m_gameObject);
        error = m_audioManager.RegisterChildAt3DPos(m_audioSourceName, expectedPosition, out child);
        Assert.AreEqual(AudioError.OK, error);
        Assert.AreEqual(expectedChild, child);
        Assert.IsTrue(m_wrapper.TryGetRegisteredChild(child, out AudioSource childSource));
        Assert.IsNotNull(childSource);
    }

    [Test]
    public void TestRegisterChildAttachedToGo() {
        ChildType expectedChild = ChildType.ATTCHD_TO_GO;
        const float expectedSpatialBlend = 1f;
        GameObject expectedGameObject = null;

        /// ---------------------------------------------
        /// Invalid case (AudioError.DOES_NOT_EXIST)
        /// ---------------------------------------------
        AudioError error = m_audioManager.RegisterChildAttachedToGo(m_unregisteredAudioSourceName, expectedGameObject, out ChildType child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.DOES_NOT_EXIST, error);
        Assert.AreEqual(expectedChild, child);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_WRAPPER)
        /// ---------------------------------------------
        error = m_audioManager.RegisterChildAttachedToGo(m_nullAudioSourceWrapperName, expectedGameObject, out child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_WRAPPER, error);
        Assert.AreEqual(expectedChild, child);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_SOURCE)
        /// ---------------------------------------------
        error = m_audioManager.RegisterChildAttachedToGo(m_nullAudioSourceName, expectedGameObject, out child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_SOURCE, error);
        Assert.AreEqual(expectedChild, child);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_CLIP)
        /// ---------------------------------------------
        error = m_audioManager.RegisterChildAttachedToGo(m_audioSourceName, expectedGameObject, out child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_CLIP, error);
        Assert.AreEqual(expectedChild, child);

        /// ---------------------------------------------
        /// Valid case (AudioError.CAN_NOT_BE_3D)
        /// ---------------------------------------------
        m_source.clip = m_clip;
        error = m_audioManager.RegisterChildAttachedToGo(m_audioSourceName, expectedGameObject, out child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.CAN_NOT_BE_3D, error);
        Assert.AreEqual(expectedChild, child);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_PARENT)
        /// ---------------------------------------------
        m_source.spatialBlend = expectedSpatialBlend;
        error = m_audioManager.RegisterChildAttachedToGo(m_audioSourceName, expectedGameObject, out child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.INVALID_PARENT, error);
        Assert.AreEqual(expectedChild, child);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        expectedGameObject = m_gameObject;
        m_audioManager = new DefaultAudioManager(m_sounds, m_gameObject);
        error = m_audioManager.RegisterChildAttachedToGo(m_audioSourceName, expectedGameObject, out child);
        Assert.AreEqual(AudioError.OK, error);
        Assert.AreEqual(expectedChild, child);
        Assert.IsTrue(m_wrapper.TryGetRegisteredChild(child, out AudioSource childSource));
        Assert.IsNotNull(childSource);
    }

    [Test]
    public void TestPlayDelayed() {
        ChildType child = ChildType.AT_3D_POS;
        const float delay = 1f;

        /// ---------------------------------------------
        /// Invalid case (AudioError.DOES_NOT_EXIST)
        /// ---------------------------------------------
        AudioError error = m_audioManager.PlayDelayed(m_unregisteredAudioSourceName, delay, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.DOES_NOT_EXIST, error);
        Assert.IsFalse(m_source.isPlaying);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_WRAPPER)
        /// ---------------------------------------------
        error = m_audioManager.PlayDelayed(m_nullAudioSourceWrapperName, delay, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_WRAPPER, error);
        Assert.IsFalse(m_source.isPlaying);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_SOURCE)
        /// ---------------------------------------------
        error = m_audioManager.PlayDelayed(m_nullAudioSourceName, delay, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_SOURCE, error);
        Assert.IsFalse(m_source.isPlaying);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_CLIP)
        /// ---------------------------------------------
        error = m_audioManager.PlayDelayed(m_audioSourceName, delay, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_CLIP, error);
        Assert.IsFalse(m_source.isPlaying);

        /// ---------------------------------------------
        /// Invalid case (AudioError.INVALID_CHILD)
        /// ---------------------------------------------
        m_source.clip = m_clip;
        error = m_audioManager.PlayDelayed(m_audioSourceName, delay, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.INVALID_CHILD, error);
        Assert.IsFalse(m_source.isPlaying);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        child = ChildType.PARENT;
        error = m_audioManager.PlayDelayed(m_audioSourceName, delay, child);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_source.isPlaying);
    }

    [Test]
    public void TestPlayOneShot() {
        ChildType child = ChildType.AT_3D_POS;
        /// ---------------------------------------------
        /// Invalid case (AudioError.DOES_NOT_EXIST)
        /// ---------------------------------------------
        AudioError error = m_audioManager.PlayOneShot(m_unregisteredAudioSourceName, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.DOES_NOT_EXIST, error);
        Assert.IsFalse(m_source.isPlaying);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_WRAPPER)
        /// ---------------------------------------------
        error = m_audioManager.PlayOneShot(m_nullAudioSourceWrapperName, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_WRAPPER, error);
        Assert.IsFalse(m_source.isPlaying);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_SOURCE)
        /// ---------------------------------------------
        error = m_audioManager.PlayOneShot(m_nullAudioSourceName, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_SOURCE, error);
        Assert.IsFalse(m_source.isPlaying);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_CLIP)
        /// ---------------------------------------------
        error = m_audioManager.PlayOneShot(m_audioSourceName, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_CLIP, error);
        Assert.IsFalse(m_source.isPlaying);

        /// ---------------------------------------------
        /// Invalid case (AudioError.INVALID_CHILD)
        /// ---------------------------------------------
        m_source.clip = m_clip;
        error = m_audioManager.PlayOneShot(m_audioSourceName, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.INVALID_CHILD, error);
        Assert.IsFalse(m_source.isPlaying);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        child = ChildType.PARENT;
        error = m_audioManager.PlayOneShot(m_audioSourceName, child);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_source.isPlaying);
    }

    [Test]
    public void TestChangePitch() {
        ChildType child = ChildType.AT_3D_POS;
        const float minPitch = 0.1f;
        const float maxPitch = 0.9f;

        /// ---------------------------------------------
        /// Invalid case (AudioError.DOES_NOT_EXIST)
        /// ---------------------------------------------
        AudioError error = m_audioManager.ChangePitch(m_unregisteredAudioSourceName, minPitch, maxPitch, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.DOES_NOT_EXIST, error);
        Assert.IsFalse(m_source.pitch >= minPitch && m_source.pitch <= maxPitch);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_WRAPPER)
        /// ---------------------------------------------
        error = m_audioManager.ChangePitch(m_nullAudioSourceWrapperName, minPitch, maxPitch, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_WRAPPER, error);
        Assert.IsFalse(m_source.pitch >= minPitch && m_source.pitch <= maxPitch);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_SOURCE)
        /// ---------------------------------------------
        error = m_audioManager.ChangePitch(m_nullAudioSourceName, minPitch, maxPitch, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_SOURCE, error);
        Assert.IsFalse(m_source.pitch >= minPitch && m_source.pitch <= maxPitch);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_CLIP)
        /// ---------------------------------------------
        error = m_audioManager.ChangePitch(m_audioSourceName, minPitch, maxPitch, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_CLIP, error);
        Assert.IsFalse(m_source.pitch >= minPitch && m_source.pitch <= maxPitch);

        /// ---------------------------------------------
        /// Invalid case (AudioError.INVALID_CHILD)
        /// ---------------------------------------------
        m_source.clip = m_clip;
        error = m_audioManager.ChangePitch(m_audioSourceName, minPitch, maxPitch, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.INVALID_CHILD, error);
        Assert.IsFalse(m_source.pitch >= minPitch && m_source.pitch <= maxPitch);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        child = ChildType.PARENT;
        error = m_audioManager.ChangePitch(m_audioSourceName, minPitch, maxPitch, child);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_source.pitch >= minPitch && m_source.pitch <= maxPitch);
    }

    [Test]
    public void TestPlayScheduled() {
        ChildType child = ChildType.AT_3D_POS;
        const float delay = 1f;

        /// ---------------------------------------------
        /// Invalid case (AudioError.DOES_NOT_EXIST)
        /// ---------------------------------------------
        AudioError error = m_audioManager.PlayScheduled(m_unregisteredAudioSourceName, delay, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.DOES_NOT_EXIST, error);
        Assert.IsFalse(m_source.isPlaying);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_WRAPPER)
        /// ---------------------------------------------
        error = m_audioManager.PlayScheduled(m_nullAudioSourceWrapperName, delay, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_WRAPPER, error);
        Assert.IsFalse(m_source.isPlaying);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_SOURCE)
        /// ---------------------------------------------
        error = m_audioManager.PlayScheduled(m_nullAudioSourceName, delay, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_SOURCE, error);
        Assert.IsFalse(m_source.isPlaying);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_CLIP)
        /// ---------------------------------------------
        error = m_audioManager.PlayScheduled(m_audioSourceName, delay, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_CLIP, error);
        Assert.IsFalse(m_source.isPlaying);

        /// ---------------------------------------------
        /// Invalid case (AudioError.INVALID_CHILD)
        /// ---------------------------------------------
        m_source.clip = m_clip;
        error = m_audioManager.PlayScheduled(m_audioSourceName, delay, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.INVALID_CHILD, error);
        Assert.IsFalse(m_source.isPlaying);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        child = ChildType.PARENT;
        error = m_audioManager.PlayScheduled(m_audioSourceName, delay, child);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_source.isPlaying);
    }

    [Test]
    public void TestStop() {
        ChildType child = ChildType.AT_3D_POS;
        m_source.clip = m_clip;
        // Start playing the given clip. So we can test if it was actually stop.
        AudioError error = m_audioManager.Play(m_initalizedAudioSourceName, ChildType.PARENT);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_initalizedSource.isPlaying);

        /// ---------------------------------------------
        /// Invalid case (AudioError.DOES_NOT_EXIST)
        /// ---------------------------------------------
        error = m_audioManager.Stop(m_unregisteredAudioSourceName, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.DOES_NOT_EXIST, error);
        Assert.IsTrue(m_initalizedSource.isPlaying);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_WRAPPER)
        /// ---------------------------------------------
        error = m_audioManager.Stop(m_nullAudioSourceWrapperName, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_WRAPPER, error);
        Assert.IsTrue(m_initalizedSource.isPlaying);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_SOURCE)
        /// ---------------------------------------------
        error = m_audioManager.Stop(m_nullAudioSourceName, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_SOURCE, error);
        Assert.IsTrue(m_initalizedSource.isPlaying);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_CLIP)
        /// ---------------------------------------------
        // Can not be tested because the clip needs to be set when the AudioSource is still playing,
        // if we set it to null AudioSource.isPlaying() will become false and
        // we therefore can not test if the actual Stop() method stopped the AudioSource or not.

        /// ---------------------------------------------
        /// Invalid case (AudioError.INVALID_CHILD)
        /// ---------------------------------------------
        error = m_audioManager.Stop(m_initalizedAudioSourceName, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.INVALID_CHILD, error);
        Assert.IsTrue(m_initalizedSource.isPlaying);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        child = ChildType.PARENT;
        error = m_audioManager.Stop(m_initalizedAudioSourceName, child);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsFalse(m_initalizedSource.isPlaying);
    }

    [Test]
    public void TestToggleMute() {
        ChildType child = ChildType.AT_3D_POS;
        /// ---------------------------------------------
        /// Invalid case (AudioError.DOES_NOT_EXIST)
        /// ---------------------------------------------
        AudioError error = m_audioManager.ToggleMute(m_unregisteredAudioSourceName, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.DOES_NOT_EXIST, error);
        Assert.IsFalse(m_source.mute);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_WRAPPER)
        /// ---------------------------------------------
        error = m_audioManager.ToggleMute(m_nullAudioSourceWrapperName, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_WRAPPER, error);
        Assert.IsFalse(m_source.mute);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_SOURCE)
        /// ---------------------------------------------
        error = m_audioManager.ToggleMute(m_nullAudioSourceName, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_SOURCE, error);
        Assert.IsFalse(m_source.mute);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_CLIP)
        /// ---------------------------------------------
        error = m_audioManager.ToggleMute(m_audioSourceName, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_CLIP, error);
        Assert.IsFalse(m_source.mute);

        /// ---------------------------------------------
        /// Invalid case (AudioError.INVALID_CHILD)
        /// ---------------------------------------------
        m_source.clip = m_clip;
        error = m_audioManager.ToggleMute(m_audioSourceName, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.INVALID_CHILD, error);
        Assert.IsFalse(m_source.mute);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        child = ChildType.PARENT;
        error = m_audioManager.ToggleMute(m_audioSourceName, child);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_source.mute);
        error = m_audioManager.ToggleMute(m_audioSourceName, child);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsFalse(m_source.mute);
    }

    [Test]
    public void TestTogglePause() {
        ChildType child = ChildType.AT_3D_POS;

        // Start playing the given clip. So we can test if it was actually paused.
        m_source.clip = m_clip;
        AudioError error = m_audioManager.Play(m_initalizedAudioSourceName, ChildType.PARENT);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_initalizedSource.isPlaying);

        /// ---------------------------------------------
        /// Invalid case (AudioError.DOES_NOT_EXIST)
        /// ---------------------------------------------
        error = m_audioManager.TogglePause(m_unregisteredAudioSourceName, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.DOES_NOT_EXIST, error);
        Assert.IsTrue(m_initalizedSource.isPlaying);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_WRAPPER)
        /// ---------------------------------------------
        error = m_audioManager.TogglePause(m_nullAudioSourceWrapperName, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_WRAPPER, error);
        Assert.IsTrue(m_initalizedSource.isPlaying);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_SOURCE)
        /// ---------------------------------------------
        error = m_audioManager.TogglePause(m_nullAudioSourceName, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_SOURCE, error);
        Assert.IsTrue(m_initalizedSource.isPlaying);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_CLIP)
        /// ---------------------------------------------
        // Can not be tested because the clip needs to be set when the AudioSource is still playing,
        // if we set it to null AudioSource.isPlaying() will become false and
        // we therefore can not test if the actual TogglePause() method paused the AudioSource or not.

        /// ---------------------------------------------
        /// Invalid case (AudioError.INVALID_CHILD)
        /// ---------------------------------------------
        error = m_audioManager.TogglePause(m_initalizedAudioSourceName, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.INVALID_CHILD, error);
        Assert.IsTrue(m_initalizedSource.isPlaying);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        child = ChildType.PARENT;
        error = m_audioManager.TogglePause(m_initalizedAudioSourceName, child);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsFalse(m_initalizedSource.isPlaying);
        error = m_audioManager.TogglePause(m_initalizedAudioSourceName, child);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_initalizedSource.isPlaying);
    }

    [Test]
    public void TestSubscribeSourceChanged() {
        int calledCount = 0;
        int calledCallback = 0;
        SourceChangedCallback changedCallback = (s) => {
            calledCallback++;
            Assert.IsNotNull(s);
        };

        /// ---------------------------------------------
        /// Invalid case (AudioError.DOES_NOT_EXIST)
        /// ---------------------------------------------
        AudioError error = m_audioManager.SubscribeSourceChanged(m_unregisteredAudioSourceName, changedCallback);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.DOES_NOT_EXIST, error);
        Assert.AreEqual(calledCount, calledCallback);
        m_wrapper.Mute = !m_wrapper.Mute;
        Assert.AreEqual(calledCount, calledCallback);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_WRAPPER)
        /// ---------------------------------------------
        error = m_audioManager.SubscribeSourceChanged(m_nullAudioSourceWrapperName, changedCallback);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_WRAPPER, error);
        Assert.AreEqual(calledCount, calledCallback);
        m_wrapper.Mute = !m_wrapper.Mute;
        Assert.AreEqual(calledCount, calledCallback);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_SOURCE)
        /// ---------------------------------------------
        error = m_audioManager.SubscribeSourceChanged(m_nullAudioSourceName, changedCallback);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_SOURCE, error);
        Assert.AreEqual(calledCount, calledCallback);
        m_wrapper.Mute = !m_wrapper.Mute;
        Assert.AreEqual(calledCount, calledCallback);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_CLIP)
        /// ---------------------------------------------
        error = m_audioManager.SubscribeSourceChanged(m_audioSourceName, changedCallback);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_CLIP, error);
        Assert.AreEqual(calledCount, calledCallback);
        m_wrapper.Mute = !m_wrapper.Mute;
        Assert.AreEqual(calledCount, calledCallback);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        error = m_audioManager.SubscribeSourceChanged(m_initalizedAudioSourceName, changedCallback);
        Assert.AreEqual(AudioError.OK, error);
        Assert.AreEqual(calledCount++, calledCallback);
        m_initalizedWrapper.Mute = !m_initalizedWrapper.Mute;
        Assert.AreEqual(calledCount, calledCallback);

        Assert.AreEqual(calledCount, calledCallback);
        m_initalizedWrapper.Mute = m_initalizedWrapper.Mute;
        Assert.AreEqual(calledCount, calledCallback);

        m_source.spatialBlend = 1f;
        error = m_audioManager.RegisterChildAttachedToGo(m_initalizedAudioSourceName, m_gameObject, out _);
        Assert.AreEqual(AudioError.OK, error);
        Assert.AreEqual(calledCount++, calledCallback);
        m_initalizedWrapper.Mute = !m_initalizedWrapper.Mute;
        Assert.AreEqual(calledCount, calledCallback);

        m_audioManager.UnsubscribeSourceChanged(m_initalizedAudioSourceName, changedCallback);
    }

    [Test]
    public void TestUnsubscribeSourceChanged() {
        int calledCount = 0;
        int calledCallbackCount = 0;
        SourceChangedCallback changedCallback = (s) => {
            calledCallbackCount++;
            Assert.IsNotNull(s);
        };
        m_source.clip = m_clip;
        AudioError error = m_audioManager.SubscribeSourceChanged(m_audioSourceName, changedCallback);
        Assert.AreEqual(AudioError.OK, error);
        m_source.clip = null;

        /// ---------------------------------------------
        /// Invalid case (AudioError.DOES_NOT_EXIST)
        /// ---------------------------------------------
        error = m_audioManager.UnsubscribeSourceChanged(m_unregisteredAudioSourceName, changedCallback);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.DOES_NOT_EXIST, error);
        Assert.AreEqual(calledCount++, calledCallbackCount);
        m_wrapper.Mute = !m_wrapper.Mute;
        Assert.AreEqual(calledCount, calledCallbackCount);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_WRAPPER)
        /// ---------------------------------------------
        error = m_audioManager.UnsubscribeSourceChanged(m_nullAudioSourceWrapperName, changedCallback);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_WRAPPER, error);
        Assert.AreEqual(calledCount++, calledCallbackCount);
        m_wrapper.Mute = !m_wrapper.Mute;
        Assert.AreEqual(calledCount, calledCallbackCount);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_SOURCE)
        /// ---------------------------------------------
        error = m_audioManager.UnsubscribeSourceChanged(m_nullAudioSourceName, changedCallback);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_SOURCE, error);
        Assert.AreEqual(calledCount++, calledCallbackCount);
        m_wrapper.Mute = !m_wrapper.Mute;
        Assert.AreEqual(calledCount, calledCallbackCount);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_CLIP)
        /// ---------------------------------------------
        error = m_audioManager.UnsubscribeSourceChanged(m_audioSourceName, changedCallback);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_CLIP, error);
        Assert.AreEqual(calledCount++, calledCallbackCount);
        m_wrapper.Mute = !m_wrapper.Mute;
        Assert.AreEqual(calledCount, calledCallbackCount);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        m_source.clip = m_clip;
        error = m_audioManager.UnsubscribeSourceChanged(m_audioSourceName, changedCallback);
        Assert.AreEqual(AudioError.OK, error);
        Assert.AreEqual(calledCount, calledCallbackCount);
        m_wrapper.Mute = !m_wrapper.Mute;
        Assert.AreEqual(calledCount, calledCallbackCount);
    }

    [UnityTest]
    public IEnumerator TestSubscribeProgressCoroutine() {
        float progress = 1f;
        ChildType child = ChildType.PARENT;
        int calledCount = 0;
        int calledUnsubCallbackCount = 0;
        int calledLoopCallbackCount = 0;
        int calledImdtCallbackCount = 0;
        int calledInvalidCallback = 0;
        ChildType calledChild = ChildType.PARENT;
        ProgressCoroutineCallback unsubCallback = (n, p, c) => {
            calledUnsubCallbackCount++;
            calledChild = c;
            Assert.AreEqual(progress, p);
            return ProgressResponse.UNSUB;
        };
        ProgressCoroutineCallback resubLoopCallback = (n, p, c) => {
            calledLoopCallbackCount++;
            calledChild = c;
            Assert.AreEqual(progress, p);
            return ProgressResponse.RESUB_IN_LOOP;
        };
        ProgressCoroutineCallback resubImdtCallback = (n, p, c) => {
            calledImdtCallbackCount++;
            calledChild = c;
            Assert.AreEqual(progress, p);
            return ProgressResponse.RESUB_IMMEDIATE;
        };
        ProgressCoroutineCallback resubInvalidCallback = (n, p, c) => {
            calledInvalidCallback++;
            calledChild = c;
            Assert.AreEqual(progress, p);
            return (ProgressResponse)(-1);
        };

        /// ---------------------------------------------
        /// Invalid case (AudioError.DOES_NOT_EXIST)
        /// ---------------------------------------------
        AudioError error = m_audioManager.SubscribeProgressCoroutine(m_unregisteredAudioSourceName, progress, unsubCallback);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.DOES_NOT_EXIST, error);
        Assert.AreEqual(calledCount, calledUnsubCallbackCount);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_WRAPPER)
        /// ---------------------------------------------
        error = m_audioManager.SubscribeProgressCoroutine(m_nullAudioSourceWrapperName, progress, unsubCallback);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_WRAPPER, error);
        Assert.AreEqual(calledCount, calledUnsubCallbackCount);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_SOURCE)
        /// ---------------------------------------------
        error = m_audioManager.SubscribeProgressCoroutine(m_nullAudioSourceName, progress, unsubCallback);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_SOURCE, error);
        Assert.AreEqual(calledCount, calledUnsubCallbackCount);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_CLIP)
        /// ---------------------------------------------
        error = m_audioManager.SubscribeProgressCoroutine(m_audioSourceName, progress, unsubCallback);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_CLIP, error);
        Assert.AreEqual(calledCount, calledUnsubCallbackCount);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_PARENT)
        /// ---------------------------------------------
        m_source.clip = m_clip;
        error = m_audioManager.SubscribeProgressCoroutine(m_audioSourceName, progress, unsubCallback);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_PARENT, error);
        Assert.AreEqual(calledCount, calledUnsubCallbackCount);

        /// ---------------------------------------------
        /// Invalid case (AudioError.INVALID_PROGRESS)
        /// ---------------------------------------------
        m_audioManager = new DefaultAudioManager(m_sounds, m_gameObject);
        error = m_audioManager.SubscribeProgressCoroutine(m_audioSourceName, progress, unsubCallback);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.INVALID_PROGRESS, error);
        Assert.AreEqual(calledCount, calledUnsubCallbackCount);

        /// ---------------------------------------------
        /// Invalid case (AudioError.ALREADY_SUBSCRIBED)
        /// ---------------------------------------------
        progress = 0f;
        error = m_audioManager.SubscribeProgressCoroutine(m_audioSourceName, progress, unsubCallback);
        Assert.AreEqual(AudioError.OK, error);
        error = m_audioManager.SubscribeProgressCoroutine(m_audioSourceName, progress, unsubCallback);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.ALREADY_SUBSCRIBED, error);
        Assert.AreEqual(calledCount, calledUnsubCallbackCount);

        // Unsubscribe callback to ensure the other callback can be subscribed successfully.
        m_audioManager.UnsubscribeProgressCoroutine(m_audioSourceName, progress);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        error = m_audioManager.SubscribeProgressCoroutine(m_audioSourceName, progress, resubInvalidCallback);
        Assert.AreEqual(AudioError.OK, error);
        // Start playing the given clip. So we can test if subscribing was successfull.
        error = m_audioManager.Play(m_audioSourceName, child);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_source.isPlaying);
        yield return new WaitForSeconds(m_clip.length);
        Assert.AreEqual(1, calledInvalidCallback);

        error = m_audioManager.SubscribeProgressCoroutine(m_audioSourceName, progress, resubImdtCallback);
        Assert.AreEqual(AudioError.OK, error);
        // Start playing the given clip. So we can test if subscribing was successfull.
        error = m_audioManager.Play(m_audioSourceName, child);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_source.isPlaying);
        yield return new WaitForSeconds(m_clip.length);
        Assert.IsTrue(calledImdtCallbackCount >= 1);

        // Unsubscribe callback to ensure the other callback can be subscribed successfully.
        m_audioManager.UnsubscribeProgressCoroutine(m_audioSourceName, progress);
        // Stop playing the given clip. So we can actually detect the next clip.
        error = m_audioManager.Stop(m_audioSourceName, child);
        Assert.AreEqual(AudioError.OK, error);

        // Ensure song loops to test if it recalls callback next loop iteration.
        m_source.loop = true;
        error = m_audioManager.SubscribeProgressCoroutine(m_audioSourceName, progress, resubLoopCallback);
        Assert.AreEqual(AudioError.OK, error);
        // Start playing the given clip. So we can test if subscribing was successfull.
        error = m_audioManager.Play(m_audioSourceName, child);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_source.isPlaying);
        Assert.AreEqual(0, calledLoopCallbackCount);
        yield return new WaitForSeconds(m_clip.length / 2);
        Assert.AreEqual(1, calledLoopCallbackCount);
        yield return new WaitForSeconds(m_clip.length / 2);
        Assert.AreEqual(2, calledLoopCallbackCount);

        // Unsubscribe callback to ensure the other callback can be subscribed successfully.
        m_audioManager.UnsubscribeProgressCoroutine(m_audioSourceName, progress);
        // Stop playing the given clip. So we can actually detect the next clip.
        error = m_audioManager.Stop(m_audioSourceName, child);
        Assert.AreEqual(AudioError.OK, error);

        error = m_audioManager.SubscribeProgressCoroutine(m_initalizedAudioSourceName, progress, unsubCallback);
        Assert.AreEqual(AudioError.OK, error);
        error = m_audioManager.RegisterChildAt3DPos(m_initalizedAudioSourceName, Vector3.zero, out child);
        Assert.AreEqual(AudioError.OK, error);
        // Start playing the given clip. So we can test if subscribing was successfull.
        error = m_audioManager.Play(m_initalizedAudioSourceName, child);
        Assert.AreEqual(AudioError.OK, error);
        Assert.AreEqual(calledCount++, calledUnsubCallbackCount);
        yield return new WaitForSeconds(m_clip.length);
        Assert.AreEqual(calledCount, calledUnsubCallbackCount);
        Assert.AreEqual(child, calledChild);

        // Unsubscribe callback to ensure the other callback can be subscribed successfully.
        m_audioManager.UnsubscribeProgressCoroutine(m_initalizedAudioSourceName, progress);
        // Stop playing the given clip. So we can actually detect the next clip.
        error = m_audioManager.Stop(m_initalizedAudioSourceName, child);
        Assert.AreEqual(AudioError.OK, error);

        error = m_audioManager.SubscribeProgressCoroutine(m_initalizedAudioSourceName, progress, unsubCallback);
        Assert.AreEqual(AudioError.OK, error);
        error = m_audioManager.RegisterChildAttachedToGo(m_initalizedAudioSourceName, m_gameObject, out child);
        Assert.AreEqual(AudioError.OK, error);
        // Start playing the given clip. So we can test if subscribing was successfull.
        error = m_audioManager.Play(m_initalizedAudioSourceName, child);
        Assert.AreEqual(AudioError.OK, error);
        Assert.AreEqual(calledCount++, calledUnsubCallbackCount);
        yield return new WaitForSeconds(m_clip.length);
        Assert.AreEqual(calledCount, calledUnsubCallbackCount);
        Assert.AreEqual(child, calledChild);

        // Unsubscribe callback to ensure the other callback can be subscribed successfully.
        m_audioManager.UnsubscribeProgressCoroutine(m_initalizedAudioSourceName, progress);
        // Stop playing the given clip. So we can actually detect the next clip.
        error = m_audioManager.Stop(m_initalizedAudioSourceName, child);
        Assert.AreEqual(AudioError.OK, error);

        error = m_audioManager.SubscribeProgressCoroutine(m_initalizedAudioSourceName, progress, unsubCallback);
        Assert.AreEqual(AudioError.OK, error);
        error = m_audioManager.RegisterChildAttachedToGo(m_initalizedAudioSourceName, m_gameObject, out child);
        Assert.AreEqual(AudioError.OK, error);
        // Start playing the given clip. So we can test if subscribing was successfull.
        error = m_audioManager.Play(m_initalizedAudioSourceName, child);
        Assert.AreEqual(AudioError.OK, error);
        error = m_audioManager.RegisterChildAt3DPos(m_initalizedAudioSourceName, Vector3.zero, out _);
        Assert.AreEqual(AudioError.OK, error);
        // Start playing the given clip. So we can test if subscribing was successfull.
        error = m_audioManager.Play(m_initalizedAudioSourceName, child);
        Assert.AreEqual(AudioError.OK, error);
        Assert.AreEqual(calledCount++, calledUnsubCallbackCount);
        yield return new WaitForSeconds(m_clip.length);
        Assert.AreEqual(calledCount, calledUnsubCallbackCount);
        Assert.AreEqual(child, calledChild);

        // Unsubscribe callback to ensure the other callback can be subscribed successfully.
        m_audioManager.UnsubscribeProgressCoroutine(m_initalizedAudioSourceName, progress);
        // Stop playing the given clip. So we can actually detect the next clip.
        error = m_audioManager.Stop(m_initalizedAudioSourceName, child);
        Assert.AreEqual(AudioError.OK, error);
    }

    [UnityTest]
    public IEnumerator TestUnsubscribeProgressCoroutine() {
        float progress = 1f;
        bool calledCallback = false;
        ProgressCoroutineCallback unsubCallback = (n, p, c) => {
            calledCallback = true;
            Assert.AreEqual(progress, p);
            return ProgressResponse.UNSUB;
        };

        /// ---------------------------------------------
        /// Invalid case (AudioError.DOES_NOT_EXIST)
        /// ---------------------------------------------
        AudioError error = m_audioManager.UnsubscribeProgressCoroutine(m_unregisteredAudioSourceName, progress);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.DOES_NOT_EXIST, error);
        Assert.IsFalse(calledCallback);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_WRAPPER)
        /// ---------------------------------------------
        error = m_audioManager.UnsubscribeProgressCoroutine(m_nullAudioSourceWrapperName, progress);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_WRAPPER, error);
        Assert.IsFalse(calledCallback);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_SOURCE)
        /// ---------------------------------------------
        error = m_audioManager.UnsubscribeProgressCoroutine(m_nullAudioSourceName, progress);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_SOURCE, error);
        Assert.IsFalse(calledCallback);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_CLIP)
        /// ---------------------------------------------
        error = m_audioManager.UnsubscribeProgressCoroutine(m_audioSourceName, progress);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_CLIP, error);
        Assert.IsFalse(calledCallback);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_PARENT)
        /// ---------------------------------------------
        m_source.clip = m_clip;
        error = m_audioManager.UnsubscribeProgressCoroutine(m_audioSourceName, progress);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_PARENT, error);
        Assert.IsFalse(calledCallback);

        /// ---------------------------------------------
        /// Invalid case (AudioError.INVALID_PROGRESS)
        /// ---------------------------------------------
        m_audioManager = new DefaultAudioManager(m_sounds, m_gameObject);
        error = m_audioManager.UnsubscribeProgressCoroutine(m_audioSourceName, progress);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.INVALID_PROGRESS, error);
        Assert.IsFalse(calledCallback);

        /// ---------------------------------------------
        /// Invalid case (AudioError.NOT_SUBSCRIBEd)
        /// ---------------------------------------------
        progress = 0f;
        error = m_audioManager.UnsubscribeProgressCoroutine(m_audioSourceName, progress);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.NOT_SUBSCRIBED, error);
        Assert.IsFalse(calledCallback);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        error = m_audioManager.SubscribeProgressCoroutine(m_audioSourceName, progress, unsubCallback);
        Assert.AreEqual(AudioError.OK, error);

        error = m_audioManager.UnsubscribeProgressCoroutine(m_audioSourceName, progress);
        Assert.AreEqual(AudioError.OK, error);

        // Start playing the given clip. So we can test if subscribing then unsubscribing was successfull.
        error = m_audioManager.Play(m_audioSourceName, ChildType.PARENT);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_source.isPlaying);

        yield return new WaitForSeconds(m_clip.length * Constants.MIN_PROGRESS);
        Assert.IsFalse(calledCallback);
    }

    [UnityTest]
    public IEnumerator TestGetProgress() {
        ChildType child = ChildType.AT_3D_POS;

        /// ---------------------------------------------
        /// Invalid case (AudioError.DOES_NOT_EXIST)
        /// ---------------------------------------------
        AudioError error = m_audioManager.GetProgress(m_unregisteredAudioSourceName, out float progress, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.DOES_NOT_EXIST, error);
        Assert.IsNaN(progress);
        Assert.AreNotEqual(m_source.time / m_clip.length, progress);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_WRAPPER)
        /// ---------------------------------------------
        error = m_audioManager.GetProgress(m_nullAudioSourceWrapperName, out progress, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_WRAPPER, error);
        Assert.IsNaN(progress);
        Assert.AreNotEqual(m_source.time / m_clip.length, progress);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_SOURCE)
        /// ---------------------------------------------
        error = m_audioManager.GetProgress(m_nullAudioSourceName, out progress, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_SOURCE, error);
        Assert.IsNaN(progress);
        Assert.AreNotEqual(m_source.time / m_clip.length, progress);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_CLIP)
        /// ---------------------------------------------
        error = m_audioManager.GetProgress(m_nullAudioSourceName, out progress, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_SOURCE, error);
        Assert.IsNaN(progress);
        Assert.AreNotEqual(m_source.time / m_clip.length, progress);

        // Start playing the given clip. So we can test if getting the progress was successfull.
        error = m_audioManager.Play(m_initalizedAudioSourceName, ChildType.PARENT);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_initalizedSource.isPlaying);
        yield return new WaitForSeconds(m_clipStartTime);

        /// ---------------------------------------------
        /// Invalid case (AudioError.INVALID_CHILD)
        /// ---------------------------------------------
        error = m_audioManager.GetProgress(m_initalizedAudioSourceName, out progress, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.INVALID_CHILD, error);
        Assert.IsNaN(progress);
        Assert.AreNotEqual(m_initalizedSource.time / m_clip.length, progress);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        child = ChildType.PARENT;
        error = m_audioManager.GetProgress(m_initalizedAudioSourceName, out progress, child);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_initalizedSource.time / m_clip.length - progress <= m_maxDifference);
    }

    [Test]
    public void TestTryGetSource() {
        /// ---------------------------------------------
        /// Invalid case (AudioError.DOES_NOT_EXIST)
        /// ---------------------------------------------
        AudioError error = m_audioManager.TryGetSource(m_unregisteredAudioSourceName, out var source);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.DOES_NOT_EXIST, error);
        Assert.IsNull(source);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_WRAPPER)
        /// ---------------------------------------------
        error = m_audioManager.TryGetSource(m_nullAudioSourceWrapperName, out source);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_WRAPPER, error);
        Assert.IsNull(source);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_SOURCE)
        /// ---------------------------------------------
        error = m_audioManager.TryGetSource(m_nullAudioSourceName, out source);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_SOURCE, error);
        Assert.IsNull(source.Source);

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
        ChildType child = ChildType.AT_3D_POS;
        const float validEndValue = 0f;
        const float duration = 0f;
        float endValue = 1f;

        /// ---------------------------------------------
        /// Invalid case (AudioError.DOES_NOT_EXIST)
        /// ---------------------------------------------
        AudioError error = m_audioManager.LerpPitch(m_unregisteredAudioSourceName, endValue, duration, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.DOES_NOT_EXIST, error);
        Assert.AreEqual(endValue, m_source.pitch);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_WRAPPER)
        /// ---------------------------------------------
        error = m_audioManager.LerpPitch(m_nullAudioSourceWrapperName, endValue, duration, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_WRAPPER, error);
        Assert.AreEqual(endValue, m_source.pitch);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_SOURCE)
        /// ---------------------------------------------
        error = m_audioManager.LerpPitch(m_nullAudioSourceName, endValue, duration, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_SOURCE, error);
        Assert.AreEqual(endValue, m_source.pitch);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_CLIP)
        /// ---------------------------------------------
        error = m_audioManager.LerpPitch(m_audioSourceName, endValue, duration, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_CLIP, error);
        Assert.AreEqual(endValue, m_source.pitch);

        /// ---------------------------------------------
        /// Invalid case (AudioError.INVALID_END_VALUE)
        /// ---------------------------------------------
        m_source.clip = m_clip;
        error = m_audioManager.LerpPitch(m_audioSourceName, endValue, duration, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.INVALID_END_VALUE, error);
        Assert.AreEqual(endValue, m_source.pitch);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_PARENT)
        /// ---------------------------------------------
        endValue = validEndValue;
        error = m_audioManager.LerpPitch(m_audioSourceName, endValue, duration, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_PARENT, error);
        Assert.AreNotEqual(endValue, m_source.pitch);

        /// ---------------------------------------------
        /// Invalid case (AudioError.INVALID_CHILD)
        /// ---------------------------------------------
        m_audioManager = new DefaultAudioManager(m_sounds, m_gameObject);
        error = m_audioManager.LerpPitch(m_audioSourceName, endValue, duration, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.INVALID_CHILD, error);
        Assert.AreNotEqual(endValue, m_source.pitch);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        child = ChildType.PARENT;
        error = m_audioManager.LerpPitch(m_audioSourceName, endValue, duration, child);
        Assert.AreEqual(AudioError.OK, error);
        // Wait a little bit more than the actual time,
        // to ensure the endValue has enough time to achieve its value.
        yield return new WaitForSeconds(duration + 0.05f);
        Assert.AreEqual(endValue, m_source.pitch);

        child = ChildType.ALL;
        error = m_audioManager.RegisterChildAt3DPos(m_initalizedAudioSourceName, Vector3.zero, out ChildType createdChild);
        Assert.AreEqual(AudioError.OK, error);
        error = m_audioManager.LerpPitch(m_initalizedAudioSourceName, endValue, duration, child);
        Assert.AreEqual(AudioError.OK, error);
        m_initalizedWrapper.TryGetRegisteredChild(createdChild, out AudioSource source);
        // Wait a little bit more than the actual time,
        // to ensure the endValue has enough time to achieve its value.
        yield return new WaitForSeconds(duration + 0.05f);
        Assert.AreEqual(endValue, m_initalizedSource.pitch);
        Assert.AreEqual(endValue, source.pitch);
    }

    [UnityTest]
    public IEnumerator TestLerpVolume() {
        ChildType child = ChildType.AT_3D_POS;
        const float validEndValue = 0f;
        const float duration = 0f;
        float endValue = 1f;

        /// ---------------------------------------------
        /// Invalid case (AudioError.DOES_NOT_EXIST)
        /// ---------------------------------------------
        AudioError error = m_audioManager.LerpVolume(m_unregisteredAudioSourceName, endValue, duration, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.DOES_NOT_EXIST, error);
        Assert.AreEqual(endValue, m_source.volume);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_WRAPPER)
        /// ---------------------------------------------
        error = m_audioManager.LerpVolume(m_nullAudioSourceWrapperName, endValue, duration, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_WRAPPER, error);
        Assert.AreEqual(endValue, m_source.volume);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_SOURCE)
        /// ---------------------------------------------
        error = m_audioManager.LerpVolume(m_nullAudioSourceName, endValue, duration, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_SOURCE, error);
        Assert.AreEqual(endValue, m_source.volume);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_CLIP)
        /// ---------------------------------------------
        error = m_audioManager.LerpVolume(m_audioSourceName, endValue, duration, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_CLIP, error);
        Assert.AreEqual(endValue, m_source.volume);

        /// ---------------------------------------------
        /// Invalid case (AudioError.INVALID_END_VALUE)
        /// ---------------------------------------------
        m_source.clip = m_clip;
        error = m_audioManager.LerpVolume(m_audioSourceName, endValue, duration, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.INVALID_END_VALUE, error);
        Assert.AreEqual(endValue, m_source.volume);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_PARENT)
        /// ---------------------------------------------
        endValue = validEndValue;
        error = m_audioManager.LerpVolume(m_audioSourceName, endValue, duration, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_PARENT, error);
        Assert.AreNotEqual(endValue, m_source.volume);

        /// ---------------------------------------------
        /// Invalid case (AudioError.INVALID_CHILD)
        /// ---------------------------------------------
        m_audioManager = new DefaultAudioManager(m_sounds, m_gameObject);
        error = m_audioManager.LerpVolume(m_audioSourceName, endValue, duration, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.INVALID_CHILD, error);
        Assert.AreNotEqual(endValue, m_source.volume);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        child = ChildType.PARENT;
        error = m_audioManager.LerpVolume(m_audioSourceName, endValue, duration, child);
        Assert.AreEqual(AudioError.OK, error);
        // Wait a little bit more than the actual time,
        // to ensure the endValue has enough time to achieve its value.
        yield return new WaitForSeconds(duration + 0.05f);
        Assert.AreEqual(endValue, m_source.volume);

        child = ChildType.ALL;
        error = m_audioManager.RegisterChildAt3DPos(m_initalizedAudioSourceName, Vector3.zero, out ChildType createdChild);
        Assert.AreEqual(AudioError.OK, error);
        error = m_audioManager.LerpVolume(m_initalizedAudioSourceName, endValue, duration, child);
        Assert.AreEqual(AudioError.OK, error);
        m_initalizedWrapper.TryGetRegisteredChild(createdChild, out AudioSource source);
        // Wait a little bit more than the actual time,
        // to ensure the endValue has enough time to achieve its value.
        yield return new WaitForSeconds(duration + 0.05f);
        Assert.AreEqual(endValue, m_initalizedSource.volume);
        Assert.AreEqual(endValue, source.volume);
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
        /// Invalid case (AudioError.MISSING_WRAPPER)
        /// ---------------------------------------------
        error = m_audioManager.ChangeGroupValue(m_nullAudioSourceWrapperName, parameterName, newValue);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_WRAPPER, error);
        m_mixerGroup.audioMixer.GetFloat(parameterName, out currentValue);
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
        AudioError error = m_audioManager.GetGroupValue(m_unregisteredAudioSourceName, parameterName, out float currentValue);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.DOES_NOT_EXIST, error);
        m_mixerGroup.audioMixer.GetFloat(parameterName, out float expectedValue);
        Assert.AreNotEqual(expectedValue, currentValue);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_WRAPPER)
        /// ---------------------------------------------
        error = m_audioManager.GetGroupValue(m_nullAudioSourceWrapperName, parameterName, out currentValue);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_WRAPPER, error);
        m_mixerGroup.audioMixer.GetFloat(parameterName, out expectedValue);
        Assert.AreNotEqual(expectedValue, currentValue);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_SOURCE)
        /// ---------------------------------------------
        error = m_audioManager.GetGroupValue(m_nullAudioSourceName, parameterName, out currentValue);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_SOURCE, error);
        m_mixerGroup.audioMixer.GetFloat(parameterName, out expectedValue);
        Assert.AreNotEqual(expectedValue, currentValue);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_CLIP)
        /// ---------------------------------------------
        error = m_audioManager.GetGroupValue(m_audioSourceName, parameterName, out currentValue);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_CLIP, error);
        m_mixerGroup.audioMixer.GetFloat(parameterName, out expectedValue);
        Assert.AreNotEqual(expectedValue, currentValue);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_MIXER_GROUP)
        /// ---------------------------------------------
        m_source.clip = m_clip;
        error = m_audioManager.GetGroupValue(m_audioSourceName, parameterName, out currentValue);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_MIXER_GROUP, error);
        m_mixerGroup.audioMixer.GetFloat(parameterName, out expectedValue);
        Assert.AreNotEqual(expectedValue, currentValue);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MIXER_NOT_EXPOSED)
        /// ---------------------------------------------
        m_source.outputAudioMixerGroup = m_mixerGroup;
        error = m_audioManager.GetGroupValue(m_audioSourceName, parameterName, out currentValue);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MIXER_NOT_EXPOSED, error);
        m_mixerGroup.audioMixer.GetFloat(parameterName, out expectedValue);
        Assert.AreNotEqual(expectedValue, currentValue);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        parameterName = m_parameterName;
        error = m_audioManager.GetGroupValue(m_audioSourceName, parameterName, out currentValue);
        Assert.AreEqual(AudioError.OK, error);
        m_mixerGroup.audioMixer.GetFloat(parameterName, out expectedValue);
        Assert.AreEqual(expectedValue, currentValue);
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
        /// Invalid case (AudioError.MISSING_WRAPPER)
        /// ---------------------------------------------
        error = m_audioManager.ResetGroupValue(m_nullAudioSourceWrapperName, parameterName);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_WRAPPER, error);

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
        const float validEndValue = 0.5f;
        const float duration = 0f;

        // Get group value, to ensure we get an invalid end value before changing it.
        string parameterName = m_parameterName;
        m_mixerGroup.audioMixer.GetFloat(parameterName, out float endValue);
        parameterName = string.Empty;

        /// ---------------------------------------------
        /// Invalid case (AudioError.DOES_NOT_EXIST)
        /// ---------------------------------------------
        AudioError error = m_audioManager.LerpGroupValue(m_unregisteredAudioSourceName, parameterName, endValue, duration);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.DOES_NOT_EXIST, error);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_WRAPPER)
        /// ---------------------------------------------
        error = m_audioManager.LerpGroupValue(m_nullAudioSourceWrapperName, parameterName, endValue, duration);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_WRAPPER, error);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_SOURCE)
        /// ---------------------------------------------
        error = m_audioManager.LerpGroupValue(m_nullAudioSourceName, parameterName, endValue, duration);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_SOURCE, error);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_CLIP)
        /// ---------------------------------------------
        error = m_audioManager.LerpGroupValue(m_audioSourceName, parameterName, endValue, duration);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_CLIP, error);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_MIXER_GROUP)
        /// ---------------------------------------------
        m_source.clip = m_clip;
        error = m_audioManager.LerpGroupValue(m_audioSourceName, parameterName, endValue, duration);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_MIXER_GROUP, error);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_PARENT)
        /// ---------------------------------------------
        m_source.outputAudioMixerGroup = m_mixerGroup;
        error = m_audioManager.LerpGroupValue(m_audioSourceName, parameterName, endValue, duration);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_PARENT, error);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MIXER_NOT_EXPOSED)
        /// ---------------------------------------------
        m_audioManager = new DefaultAudioManager(m_sounds, m_gameObject);
        error = m_audioManager.LerpGroupValue(m_audioSourceName, parameterName, endValue, duration);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MIXER_NOT_EXPOSED, error);

        /// ---------------------------------------------
        /// Invalid case (AudioError.INVALID_END_VALUE)
        /// ---------------------------------------------
        parameterName = m_parameterName;
        error = m_audioManager.LerpGroupValue(m_audioSourceName, parameterName, endValue, duration);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.INVALID_END_VALUE, error);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        endValue = validEndValue;
        error = m_audioManager.LerpGroupValue(m_audioSourceName, parameterName, endValue, duration);
        Assert.AreEqual(AudioError.OK, error);
        // Wait a little bit more than the actual time,
        // to ensure the endValue has enough time to achieve its value.
        yield return new WaitForSeconds(duration + 0.05f);
        m_mixerGroup.audioMixer.GetFloat(parameterName, out float currentValue);
        Assert.AreEqual(endValue, currentValue);
    }

    [Test]
    public void TestRemoveGroup() {
        ChildType child = ChildType.AT_3D_POS;
        /// ---------------------------------------------
        /// Invalid case (AudioError.DOES_NOT_EXIST)
        /// ---------------------------------------------
        m_source.outputAudioMixerGroup = m_mixerGroup;
        AudioError error = m_audioManager.RemoveGroup(m_unregisteredAudioSourceName, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.DOES_NOT_EXIST, error);
        Assert.IsNotNull(m_source.outputAudioMixerGroup);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_WRAPPER)
        /// ---------------------------------------------
        error = m_audioManager.RemoveGroup(m_nullAudioSourceWrapperName, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_WRAPPER, error);
        Assert.IsNotNull(m_source.outputAudioMixerGroup);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_SOURCE)
        /// ---------------------------------------------
        error = m_audioManager.RemoveGroup(m_nullAudioSourceName, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_SOURCE, error);
        Assert.IsNotNull(m_source.outputAudioMixerGroup);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_CLIP)
        /// ---------------------------------------------
        error = m_audioManager.RemoveGroup(m_audioSourceName, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_CLIP, error);
        Assert.IsNotNull(m_source.outputAudioMixerGroup);

        /// ---------------------------------------------
        /// Invalid case (AudioError.INVALID_CHILD)
        /// ---------------------------------------------
        m_source.clip = m_clip;
        error = m_audioManager.RemoveGroup(m_audioSourceName, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.INVALID_CHILD, error);
        Assert.IsFalse(m_source.isPlaying);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        child = ChildType.PARENT;
        error = m_audioManager.RemoveGroup(m_audioSourceName, child);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsNull(m_source.outputAudioMixerGroup);
    }

    [Test]
    public void TestAddGroup() {
        ChildType child = ChildType.AT_3D_POS;
        /// ---------------------------------------------
        /// Invalid case (AudioError.DOES_NOT_EXIST)
        /// ---------------------------------------------
        AudioError error = m_audioManager.AddGroup(m_unregisteredAudioSourceName, m_mixerGroup, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.DOES_NOT_EXIST, error);
        Assert.IsNull(m_source.outputAudioMixerGroup);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_WRAPPER)
        /// ---------------------------------------------
        error = m_audioManager.AddGroup(m_nullAudioSourceWrapperName, m_mixerGroup, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_WRAPPER, error);
        Assert.IsNull(m_source.outputAudioMixerGroup);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_SOURCE)
        /// ---------------------------------------------
        error = m_audioManager.AddGroup(m_nullAudioSourceName, m_mixerGroup, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_SOURCE, error);
        Assert.IsNull(m_source.outputAudioMixerGroup);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_CLIP)
        /// ---------------------------------------------
        error = m_audioManager.AddGroup(m_audioSourceName, m_mixerGroup, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_CLIP, error);
        Assert.IsNull(m_source.outputAudioMixerGroup);

        /// ---------------------------------------------
        /// Invalid case (AudioError.INVALID_CHILD)
        /// ---------------------------------------------
        m_source.clip = m_clip;
        error = m_audioManager.AddGroup(m_audioSourceName, m_mixerGroup, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.INVALID_CHILD, error);
        Assert.IsNull(m_source.outputAudioMixerGroup);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        child = ChildType.PARENT;
        error = m_audioManager.AddGroup(m_audioSourceName, m_mixerGroup, child);
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
        ChildType child = ChildType.AT_3D_POS;
        const float minDistance = 10f;
        const float maxDistance = 25f;
        const float spreadAngle = 20f;
        const float dopplerLevel = 0.5f;
        const AudioRolloffMode rolloffMode = AudioRolloffMode.Linear;
        float spatialBlend = 0f;

        /// ---------------------------------------------
        /// Invalid case (AudioError.DOES_NOT_EXIST)
        /// ---------------------------------------------
        AudioError error = m_audioManager.Set3DAudioOptions(m_unregisteredAudioSourceName, minDistance, maxDistance, child, spatialBlend, spreadAngle, dopplerLevel, rolloffMode);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.DOES_NOT_EXIST, error);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_WRAPPER)
        /// ---------------------------------------------
        error = m_audioManager.Set3DAudioOptions(m_nullAudioSourceWrapperName, minDistance, maxDistance, child, spatialBlend, spreadAngle, dopplerLevel, rolloffMode);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_WRAPPER, error);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_SOURCE)
        /// ---------------------------------------------
        error = m_audioManager.Set3DAudioOptions(m_nullAudioSourceName, minDistance, maxDistance, child, spatialBlend, spreadAngle, dopplerLevel, rolloffMode);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_SOURCE, error);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_CLIP)
        /// ---------------------------------------------
        error = m_audioManager.Set3DAudioOptions(m_audioSourceName, minDistance, maxDistance, child, spatialBlend, spreadAngle, dopplerLevel, rolloffMode);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_CLIP, error);

        /// ---------------------------------------------
        /// Invalid case (AudioError.CAN_NOT_BE_3D)
        /// ---------------------------------------------
        m_source.clip = m_clip;
        error = m_audioManager.Set3DAudioOptions(m_audioSourceName, minDistance, maxDistance, child, spatialBlend, spreadAngle, dopplerLevel, rolloffMode);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.CAN_NOT_BE_3D, error);

        /// ---------------------------------------------
        /// Invalid case (AudioError.INVALID_CHILD)
        /// ---------------------------------------------
        spatialBlend = 0.5f;
        error = m_audioManager.Set3DAudioOptions(m_audioSourceName, minDistance, maxDistance, child, spatialBlend, spreadAngle, dopplerLevel, rolloffMode);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.INVALID_CHILD, error);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        child = ChildType.PARENT;
        error = m_audioManager.Set3DAudioOptions(m_audioSourceName, minDistance, maxDistance, child, spatialBlend, spreadAngle, dopplerLevel, rolloffMode);
        Assert.AreEqual(AudioError.OK, error);
        Assert.AreEqual(minDistance, m_source.minDistance);
        Assert.AreEqual(maxDistance, m_source.maxDistance);
        Assert.AreEqual(spatialBlend, m_source.spatialBlend);
        Assert.AreEqual(spreadAngle, m_source.spread);
        Assert.AreEqual(dopplerLevel, m_source.dopplerLevel);
        Assert.AreEqual(rolloffMode, m_source.rolloffMode);
    }

    [Test]
    public void TestGetClipLength() {
        ChildType child = ChildType.AT_3D_POS;

        /// ---------------------------------------------
        /// Invalid case (AudioError.DOES_NOT_EXIST)
        /// ---------------------------------------------
        AudioError error = m_audioManager.GetClipLength(m_unregisteredAudioSourceName, out double length, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.DOES_NOT_EXIST, error);
        Assert.IsNaN(length);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_WRAPPER)
        /// ---------------------------------------------
        error = m_audioManager.GetClipLength(m_nullAudioSourceWrapperName, out length, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_WRAPPER, error);
        Assert.IsNaN(length);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_SOURCE)
        /// ---------------------------------------------
        error = m_audioManager.GetClipLength(m_nullAudioSourceName, out length, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_SOURCE, error);
        Assert.IsNaN(length);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_CLIP)
        /// ---------------------------------------------
        error = m_audioManager.GetClipLength(m_audioSourceName, out length, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_CLIP, error);
        Assert.IsNaN(length);

        /// ---------------------------------------------
        /// Invalid case (AudioError.INVALID_CHILD)
        /// ---------------------------------------------
        m_source.clip = m_clip;
        error = m_audioManager.GetClipLength(m_audioSourceName, out length, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.INVALID_CHILD, error);
        Assert.IsNaN(length);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        child = ChildType.PARENT;
        double clipLength = (double)m_source.clip.samples / m_source.clip.frequency;
        error = m_audioManager.GetClipLength(m_audioSourceName, out length, child);
        Assert.AreEqual(AudioError.OK, error);
        Assert.AreEqual(clipLength, length);
    }

    [Test]
    public void TestSetStartTime() {
        ChildType child = ChildType.AT_3D_POS;
        float startTime = m_clip.length * 2f;

        /// ---------------------------------------------
        /// Invalid case (AudioError.DOES_NOT_EXIST)
        /// ---------------------------------------------
        AudioError error = m_audioManager.SetStartTime(m_unregisteredAudioSourceName, startTime, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.DOES_NOT_EXIST, error);
        Assert.AreNotEqual(startTime, m_source.time);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_WRAPPER)
        /// ---------------------------------------------
        error = m_audioManager.SetStartTime(m_nullAudioSourceWrapperName, startTime, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_WRAPPER, error);
        Assert.AreNotEqual(startTime, m_source.time);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_SOURCE)
        /// ---------------------------------------------
        error = m_audioManager.SetStartTime(m_nullAudioSourceName, startTime, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_SOURCE, error);
        Assert.AreNotEqual(startTime, m_source.time);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_CLIP)
        /// ---------------------------------------------
        error = m_audioManager.SetStartTime(m_audioSourceName, startTime, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_CLIP, error);
        Assert.AreNotEqual(startTime, m_source.time);

        /// ---------------------------------------------
        /// Invalid case (AudioError.INVALID_TIME)
        /// ---------------------------------------------
        m_source.clip = m_clip;
        error = m_audioManager.SetStartTime(m_audioSourceName, startTime, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.INVALID_TIME, error);
        Assert.AreNotEqual(startTime, m_source.time);

        /// ---------------------------------------------
        /// Invalid case (AudioError.INVALID_CHILD)
        /// ---------------------------------------------
        startTime = m_clipEndTime;
        error = m_audioManager.SetStartTime(m_audioSourceName, startTime, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.INVALID_CHILD, error);
        Assert.AreNotEqual(startTime, m_source.time);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        child = ChildType.PARENT;
        error = m_audioManager.SetStartTime(m_audioSourceName, startTime, child);
        Assert.AreEqual(AudioError.OK, error);
        // The clip needs to be played to have it's time actually assigned.
        error = m_audioManager.Play(m_audioSourceName, ChildType.PARENT);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(startTime - m_source.time <= m_maxDifference);
    }

    [Test]
    public void TestSkipTime() {
        ChildType child = ChildType.AT_3D_POS;
        float backwardStartTime = m_clip.length / 2f;
        float forwardStartTime = (m_clip.length * Constants.MAX_PROGRESS);
        float time = -1f;

        /// ---------------------------------------------
        /// Invalid case (AudioError.DOES_NOT_EXIST)
        /// ---------------------------------------------
        AudioError error = m_audioManager.SkipTime(m_unregisteredAudioSourceName, time, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.DOES_NOT_EXIST, error);
        Assert.AreNotEqual(time, m_source.time);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_WRAPPER)
        /// ---------------------------------------------
        error = m_audioManager.SkipTime(m_nullAudioSourceWrapperName, time, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_WRAPPER, error);
        Assert.AreNotEqual(time, m_source.time);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_SOURCE)
        /// ---------------------------------------------
        error = m_audioManager.SkipTime(m_nullAudioSourceName, time, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_SOURCE, error);
        Assert.AreNotEqual(time, m_source.time);

        /// ---------------------------------------------
        /// Invalid case (AudioError.MISSING_CLIP)
        /// ---------------------------------------------
        error = m_audioManager.SkipTime(m_audioSourceName, time, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.MISSING_CLIP, error);
        Assert.AreNotEqual(time, m_source.time);

        /// ---------------------------------------------
        /// Invalid case (AudioError.INVALID_CHILD)
        /// ---------------------------------------------
        m_source.clip = m_clip;
        error = m_audioManager.SkipTime(m_audioSourceName, time, child);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(AudioError.INVALID_CHILD, error);
        Assert.AreNotEqual(time, m_source.time);

        /// ---------------------------------------------
        /// Valid case (AudioError.OK)
        /// ---------------------------------------------
        child = ChildType.PARENT;
        error = m_audioManager.SkipTime(m_audioSourceName, time, child);
        Assert.AreEqual(AudioError.OK, error);
        // The clip needs to be played to have it's time actually assigned.
        error = m_audioManager.Play(m_audioSourceName, ChildType.PARENT);
        Assert.AreEqual(AudioError.OK, error);
        Assert.AreEqual(0f, m_source.time);

        m_source.time = backwardStartTime;
        error = m_audioManager.SkipTime(m_audioSourceName, time, child);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_source.time - (backwardStartTime + time) <= m_maxDifference);

        m_source.time = forwardStartTime;
        time *= -1;
        error = m_audioManager.SkipTime(m_audioSourceName, time, child);
        Assert.AreEqual(AudioError.OK, error);
        Assert.IsTrue(m_source.time - (forwardStartTime + time) <= m_maxDifference);
    }
}

using AudioManager.Core;
using AudioManager.Service;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Audio;

public sealed class TestFluentAudioManager {
    string m_audioSourceName;
    ChildType m_childType;
    AudioError m_audioError;
    DummyAudioManager m_audioManager;
    FluentAudioManager m_fluentAudioManager;

    [SetUp]
    public void TestSetUp() {
        m_audioSourceName = string.Empty;
        m_childType = ChildType.PARENT;
        m_audioError = AudioError.DOES_NOT_EXIST;
        m_audioManager = new DummyAudioManager();
        m_fluentAudioManager = new FluentAudioManager(m_audioManager, m_audioSourceName, m_childType, m_audioError);
    }

    [Test]
    public void TestPlay() {
        /// ---------------------------------------------
        /// Invalid case (Method skipped)
        /// ---------------------------------------------
        AudioError error = m_fluentAudioManager.Play();
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(m_audioError, error);

        /// ---------------------------------------------
        /// Valid case (Method executed)
        /// ---------------------------------------------
        m_audioError = AudioError.OK;
        m_fluentAudioManager.ReuseInstance(m_audioManager, m_audioSourceName, m_childType, m_audioError);
        error = m_fluentAudioManager.Play();
        Assert.AreEqual(m_audioError, error);
    }

    [Test]
    public void TestPlayAtTimeStamp() {
        float startTime = float.NaN;

        /// ---------------------------------------------
        /// Invalid case (Method skipped)
        /// ---------------------------------------------
        AudioError error = m_fluentAudioManager.PlayAtTimeStamp(startTime);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(m_audioError, error);

        /// ---------------------------------------------
        /// Valid case (Method executed)
        /// ---------------------------------------------
        m_audioError = AudioError.OK;
        m_fluentAudioManager.ReuseInstance(m_audioManager, m_audioSourceName, m_childType, m_audioError);
        error = m_fluentAudioManager.PlayAtTimeStamp(startTime);
        Assert.AreEqual(m_audioError, error);
    }

    [Test]
    public void TestGetPlaybackPosition() {
        /// ---------------------------------------------
        /// Invalid case (Method skipped)
        /// ---------------------------------------------
        AudioError error = m_fluentAudioManager.GetPlaybackPosition(out float time).Execute();
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.IsNaN(time);
        Assert.AreEqual(m_audioError, error);

        /// ---------------------------------------------
        /// Valid case (Method executed)
        /// ---------------------------------------------
        m_audioError = AudioError.OK;
        m_fluentAudioManager.ReuseInstance(m_audioManager, m_audioSourceName, m_childType, m_audioError);
        error = m_fluentAudioManager.GetPlaybackPosition(out time).Execute();
        Assert.AreEqual(m_audioError, error);
        Assert.IsNaN(time);
    }

    [Test]
    public void TestSetPlaybackDirection() {
        float pitch = float.NaN;

        /// ---------------------------------------------
        /// Invalid case (Method skipped)
        /// ---------------------------------------------
        AudioError error = m_fluentAudioManager.SetPlaybackDirection(pitch).Execute();
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(m_audioError, error);

        /// ---------------------------------------------
        /// Valid case (Method executed)
        /// ---------------------------------------------
        m_audioError = AudioError.OK;
        m_fluentAudioManager.ReuseInstance(m_audioManager, m_audioSourceName, m_childType, m_audioError);
        error = m_fluentAudioManager.SetPlaybackDirection(pitch).Execute();
        Assert.AreEqual(m_audioError, error);
    }

    [Test]
    public void TestPlayDelayed() {
        float delay = float.NaN;

        /// ---------------------------------------------
        /// Invalid case (Method skipped)
        /// ---------------------------------------------
        AudioError error = m_fluentAudioManager.PlayDelayed(delay);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(m_audioError, error);

        /// ---------------------------------------------
        /// Valid case (Method executed)
        /// ---------------------------------------------
        m_audioError = AudioError.OK;
        m_fluentAudioManager.ReuseInstance(m_audioManager, m_audioSourceName, m_childType, m_audioError);
        error = m_fluentAudioManager.PlayDelayed(delay);
        Assert.AreEqual(m_audioError, error);
    }

    [Test]
    public void TestPlayOneShot() {
        /// ---------------------------------------------
        /// Invalid case (Method skipped)
        /// ---------------------------------------------
        AudioError error = m_fluentAudioManager.PlayOneShot();
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(m_audioError, error);

        /// ---------------------------------------------
        /// Valid case (Method executed)
        /// ---------------------------------------------
        m_audioError = AudioError.OK;
        m_fluentAudioManager.ReuseInstance(m_audioManager, m_audioSourceName, m_childType, m_audioError);
        error = m_fluentAudioManager.PlayOneShot();
        Assert.AreEqual(m_audioError, error);
    }

    [Test]
    public void TestChangePitch() {
        float minPitch = float.NaN;
        float maxPitch = float.NaN;

        /// ---------------------------------------------
        /// Invalid case (Method skipped)
        /// ---------------------------------------------
        AudioError error = m_fluentAudioManager.ChangePitch(minPitch, maxPitch).Execute();
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(m_audioError, error);

        /// ---------------------------------------------
        /// Valid case (Method executed)
        /// ---------------------------------------------
        m_audioError = AudioError.OK;
        m_fluentAudioManager.ReuseInstance(m_audioManager, m_audioSourceName, m_childType, m_audioError);
        error = m_fluentAudioManager.ChangePitch(minPitch, maxPitch).Execute();
        Assert.AreEqual(m_audioError, error);
    }

    [Test]
    public void TestPlayScheduled() {
        double time = double.NaN;

        /// ---------------------------------------------
        /// Invalid case (Method skipped)
        /// ---------------------------------------------
        AudioError error = m_fluentAudioManager.PlayScheduled(time);
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(m_audioError, error);

        /// ---------------------------------------------
        /// Valid case (Method executed)
        /// ---------------------------------------------
        m_audioError = AudioError.OK;
        m_fluentAudioManager.ReuseInstance(m_audioManager, m_audioSourceName, m_childType, m_audioError);
        error = m_fluentAudioManager.PlayScheduled(time);
        Assert.AreEqual(m_audioError, error);
    }

    [Test]
    public void TestDeregisterChild() {
        /// ---------------------------------------------
        /// Invalid case (Method skipped)
        /// ---------------------------------------------
        AudioError error = m_fluentAudioManager.DeregisterChild().Execute();
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(m_audioError, error);

        /// ---------------------------------------------
        /// Valid case (Method executed)
        /// ---------------------------------------------
        m_audioError = AudioError.OK;
        m_fluentAudioManager.ReuseInstance(m_audioManager, m_audioSourceName, m_childType, m_audioError);
        error = m_fluentAudioManager.DeregisterChild().Execute();
        Assert.AreEqual(m_audioError, error);
    }

    [Test]
    public void TestStop() {
        /// ---------------------------------------------
        /// Invalid case (Method skipped)
        /// ---------------------------------------------
        AudioError error = m_fluentAudioManager.Stop();
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(m_audioError, error);

        /// ---------------------------------------------
        /// Valid case (Method executed)
        /// ---------------------------------------------
        m_audioError = AudioError.OK;
        m_fluentAudioManager.ReuseInstance(m_audioManager, m_audioSourceName, m_childType, m_audioError);
        error = m_fluentAudioManager.Stop();
        Assert.AreEqual(m_audioError, error);
    }

    [Test]
    public void TestToggleMute() {
        /// ---------------------------------------------
        /// Invalid case (Method skipped)
        /// ---------------------------------------------
        AudioError error = m_fluentAudioManager.ToggleMute().Execute();
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(m_audioError, error);

        /// ---------------------------------------------
        /// Valid case (Method executed)
        /// ---------------------------------------------
        m_audioError = AudioError.OK;
        m_fluentAudioManager.ReuseInstance(m_audioManager, m_audioSourceName, m_childType, m_audioError);
        error = m_fluentAudioManager.ToggleMute().Execute();
        Assert.AreEqual(m_audioError, error);
    }

    [Test]
    public void TestTogglePause() {
        /// ---------------------------------------------
        /// Invalid case (Method skipped)
        /// ---------------------------------------------
        AudioError error = m_fluentAudioManager.TogglePause().Execute();
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(m_audioError, error);

        /// ---------------------------------------------
        /// Valid case (Method executed)
        /// ---------------------------------------------
        m_audioError = AudioError.OK;
        m_fluentAudioManager.ReuseInstance(m_audioManager, m_audioSourceName, m_childType, m_audioError);
        error = m_fluentAudioManager.TogglePause().Execute();
        Assert.AreEqual(m_audioError, error);
    }

    [Test]
    public void TestSubscribeSourceChanged() {
        SourceChangedCallback cb = null;

        /// ---------------------------------------------
        /// Invalid case (Method skipped)
        /// ---------------------------------------------
        AudioError error = m_fluentAudioManager.SubscribeSourceChanged(cb).Execute();
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(m_audioError, error);

        /// ---------------------------------------------
        /// Valid case (Method executed)
        /// ---------------------------------------------
        m_audioError = AudioError.OK;
        m_fluentAudioManager.ReuseInstance(m_audioManager, m_audioSourceName, m_childType, m_audioError);
        error = m_fluentAudioManager.SubscribeSourceChanged(cb).Execute();
        Assert.AreEqual(m_audioError, error);
    }

    [Test]
    public void TestUnsubscribeSourceChanged() {
        SourceChangedCallback cb = null;

        /// ---------------------------------------------
        /// Invalid case (Method skipped)
        /// ---------------------------------------------
        AudioError error = m_fluentAudioManager.UnsubscribeSourceChanged(cb).Execute();
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(m_audioError, error);

        /// ---------------------------------------------
        /// Valid case (Method executed)
        /// ---------------------------------------------
        m_audioError = AudioError.OK;
        m_fluentAudioManager.ReuseInstance(m_audioManager, m_audioSourceName, m_childType, m_audioError);
        error = m_fluentAudioManager.UnsubscribeSourceChanged(cb).Execute();
        Assert.AreEqual(m_audioError, error);
    }

    [Test]
    public void TestSubscribeProgressCoroutine() {
        float progress = float.NaN;
        ProgressCoroutineCallback cb = null;

        /// ---------------------------------------------
        /// Invalid case (Method skipped)
        /// ---------------------------------------------
        AudioError error = m_fluentAudioManager.SubscribeProgressCoroutine(progress, cb).Execute();
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(m_audioError, error);

        /// ---------------------------------------------
        /// Valid case (Method executed)
        /// ---------------------------------------------
        m_audioError = AudioError.OK;
        m_fluentAudioManager.ReuseInstance(m_audioManager, m_audioSourceName, m_childType, m_audioError);
        error = m_fluentAudioManager.SubscribeProgressCoroutine(progress, cb).Execute();
        Assert.AreEqual(m_audioError, error);
    }

    [Test]
    public void TestUnsubscribeProgressCoroutine() {
        float progress = float.NaN;

        /// ---------------------------------------------
        /// Invalid case (Method skipped)
        /// ---------------------------------------------
        AudioError error = m_fluentAudioManager.UnsubscribeProgressCoroutine(progress).Execute();
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(m_audioError, error);

        /// ---------------------------------------------
        /// Valid case (Method executed)
        /// ---------------------------------------------
        m_audioError = AudioError.OK;
        m_fluentAudioManager.ReuseInstance(m_audioManager, m_audioSourceName, m_childType, m_audioError);
        error = m_fluentAudioManager.UnsubscribeProgressCoroutine(progress).Execute();
        Assert.AreEqual(m_audioError, error);
    }

    [Test]
    public void TestGetProgress() {
        /// ---------------------------------------------
        /// Invalid case (Method skipped)
        /// ---------------------------------------------
        AudioError error = m_fluentAudioManager.GetProgress(out float progress).Execute();
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.IsNaN(progress);
        Assert.AreEqual(m_audioError, error);

        /// ---------------------------------------------
        /// Valid case (Method executed)
        /// ---------------------------------------------
        m_audioError = AudioError.OK;
        m_fluentAudioManager.ReuseInstance(m_audioManager, m_audioSourceName, m_childType, m_audioError);
        error = m_fluentAudioManager.GetProgress(out progress).Execute();
        Assert.AreEqual(m_audioError, error);
        Assert.IsNaN(progress);
    }

    [Test]
    public void TestTryGetSource() {
        /// ---------------------------------------------
        /// Invalid case (Method skipped)
        /// ---------------------------------------------
        AudioError error = m_fluentAudioManager.TryGetSource(out var source).Execute();
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.IsNull(source);
        Assert.AreEqual(m_audioError, error);

        /// ---------------------------------------------
        /// Valid case (Method executed)
        /// ---------------------------------------------
        m_audioError = AudioError.OK;
        m_fluentAudioManager.ReuseInstance(m_audioManager, m_audioSourceName, m_childType, m_audioError);
        error = m_fluentAudioManager.TryGetSource(out source).Execute();
        Assert.AreEqual(m_audioError, error);
        Assert.IsNull(source);
    }

    [Test]
    public void TestLerpPitch() {
        float endValue = float.NaN;
        float duration = float.NaN;

        /// ---------------------------------------------
        /// Invalid case (Method skipped)
        /// ---------------------------------------------
        AudioError error = m_fluentAudioManager.LerpPitch(endValue, duration).Execute();
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(m_audioError, error);

        /// ---------------------------------------------
        /// Valid case (Method executed)
        /// ---------------------------------------------
        m_audioError = AudioError.OK;
        m_fluentAudioManager.ReuseInstance(m_audioManager, m_audioSourceName, m_childType, m_audioError);
        error = m_fluentAudioManager.LerpPitch(endValue, duration).Execute();
        Assert.AreEqual(m_audioError, error);
    }

    [Test]
    public void TestLerpVolume() {
        float endValue = float.NaN;
        float duration = float.NaN;

        /// ---------------------------------------------
        /// Invalid case (Method skipped)
        /// ---------------------------------------------
        AudioError error = m_fluentAudioManager.LerpVolume(endValue, duration).Execute();
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(m_audioError, error);

        /// ---------------------------------------------
        /// Valid case (Method executed)
        /// ---------------------------------------------
        m_audioError = AudioError.OK;
        m_fluentAudioManager.ReuseInstance(m_audioManager, m_audioSourceName, m_childType, m_audioError);
        error = m_fluentAudioManager.LerpVolume(endValue, duration).Execute();
        Assert.AreEqual(m_audioError, error);
    }

    [Test]
    public void TestChangeGroupValue() {
        string exposedParameterName = string.Empty;
        float newValue = float.NaN;

        /// ---------------------------------------------
        /// Invalid case (Method skipped)
        /// ---------------------------------------------
        AudioError error = m_fluentAudioManager.ChangeGroupValue(exposedParameterName, newValue).Execute();
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(m_audioError, error);

        /// ---------------------------------------------
        /// Valid case (Method executed)
        /// ---------------------------------------------
        m_audioError = AudioError.OK;
        m_fluentAudioManager.ReuseInstance(m_audioManager, m_audioSourceName, m_childType, m_audioError);
        error = m_fluentAudioManager.ChangeGroupValue(exposedParameterName, newValue).Execute();
        Assert.AreEqual(m_audioError, error);
    }

    [Test]
    public void TestGetGroupValue() {
        string exposedParameterName = string.Empty;

        /// ---------------------------------------------
        /// Invalid case (Method skipped)
        /// ---------------------------------------------
        AudioError error = m_fluentAudioManager.GetGroupValue(exposedParameterName, out float currentValue).Execute();
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.IsNaN(currentValue);
        Assert.AreEqual(m_audioError, error);

        /// ---------------------------------------------
        /// Valid case (Method executed)
        /// ---------------------------------------------
        m_audioError = AudioError.OK;
        m_fluentAudioManager.ReuseInstance(m_audioManager, m_audioSourceName, m_childType, m_audioError);
        error = m_fluentAudioManager.GetGroupValue(exposedParameterName, out currentValue).Execute();
        Assert.AreEqual(m_audioError, error);
        Assert.IsNaN(currentValue);
    }

    [Test]
    public void TestResetGroupValue() {
        string exposedParameterName = string.Empty;

        /// ---------------------------------------------
        /// Invalid case (Method skipped)
        /// ---------------------------------------------
        AudioError error = m_fluentAudioManager.ResetGroupValue(exposedParameterName).Execute();
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(m_audioError, error);

        /// ---------------------------------------------
        /// Valid case (Method executed)
        /// ---------------------------------------------
        m_audioError = AudioError.OK;
        m_fluentAudioManager.ReuseInstance(m_audioManager, m_audioSourceName, m_childType, m_audioError);
        error = m_fluentAudioManager.ResetGroupValue(exposedParameterName).Execute();
        Assert.AreEqual(m_audioError, error);
    }

    [Test]
    public void TestLerpGroupValue() {
        string exposedParameterName = string.Empty;
        float endValue = float.NaN;
        float duration = float.NaN;

        /// ---------------------------------------------
        /// Invalid case (Method skipped)
        /// ---------------------------------------------
        AudioError error = m_fluentAudioManager.LerpGroupValue(exposedParameterName, endValue, duration).Execute();
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(m_audioError, error);

        /// ---------------------------------------------
        /// Valid case (Method executed)
        /// ---------------------------------------------
        m_audioError = AudioError.OK;
        m_fluentAudioManager.ReuseInstance(m_audioManager, m_audioSourceName, m_childType, m_audioError);
        error = m_fluentAudioManager.LerpGroupValue(exposedParameterName, endValue, duration).Execute();
        Assert.AreEqual(m_audioError, error);
    }

    [Test]
    public void TestRemoveGroup() {
        /// ---------------------------------------------
        /// Invalid case (Method skipped)
        /// ---------------------------------------------
        AudioError error = m_fluentAudioManager.RemoveGroup().Execute();
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(m_audioError, error);

        /// ---------------------------------------------
        /// Valid case (Method executed)
        /// ---------------------------------------------
        m_audioError = AudioError.OK;
        m_fluentAudioManager.ReuseInstance(m_audioManager, m_audioSourceName, m_childType, m_audioError);
        error = m_fluentAudioManager.RemoveGroup().Execute();
        Assert.AreEqual(m_audioError, error);
    }

    [Test]
    public void TestAddGroup() {
        AudioMixerGroup mixerGroup = null;

        /// ---------------------------------------------
        /// Invalid case (Method skipped)
        /// ---------------------------------------------
        AudioError error = m_fluentAudioManager.AddGroup(mixerGroup).Execute();
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(m_audioError, error);

        /// ---------------------------------------------
        /// Valid case (Method executed)
        /// ---------------------------------------------
        m_audioError = AudioError.OK;
        m_fluentAudioManager.ReuseInstance(m_audioManager, m_audioSourceName, m_childType, m_audioError);
        error = m_fluentAudioManager.AddGroup(mixerGroup).Execute();
        Assert.AreEqual(m_audioError, error);
    }

    [Test]
    public void TestRemoveSound() {
        /// ---------------------------------------------
        /// Invalid case (Method skipped)
        /// ---------------------------------------------
        AudioError error = m_fluentAudioManager.RemoveSound();
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(m_audioError, error);

        /// ---------------------------------------------
        /// Valid case (Method executed)
        /// ---------------------------------------------
        m_audioError = AudioError.OK;
        m_fluentAudioManager.ReuseInstance(m_audioManager, m_audioSourceName, m_childType, m_audioError);
        error = m_fluentAudioManager.RemoveSound();
        Assert.AreEqual(m_audioError, error);
    }

    [Test]
    public void TestSet3DAudioOptions() {
        float minDistance = float.NaN;
        float maxDistance = float.NaN;
        float spatialBlend = float.NaN;
        float spreadAngle = float.NaN;
        float dopplerLevel = float.NaN;
        AudioRolloffMode rolloffMode = AudioRolloffMode.Logarithmic;

        /// ---------------------------------------------
        /// Invalid case (Method skipped)
        /// ---------------------------------------------
        AudioError error = m_fluentAudioManager.Set3DAudioOptions(minDistance, maxDistance, spatialBlend, spreadAngle, dopplerLevel, rolloffMode).Execute();
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(m_audioError, error);

        /// ---------------------------------------------
        /// Valid case (Method executed)
        /// ---------------------------------------------
        m_audioError = AudioError.OK;
        m_fluentAudioManager.ReuseInstance(m_audioManager, m_audioSourceName, m_childType, m_audioError);
        error = m_fluentAudioManager.Set3DAudioOptions(minDistance, maxDistance, spatialBlend, spreadAngle, dopplerLevel, rolloffMode).Execute();
        Assert.AreEqual(m_audioError, error);
    }

    [Test]
    public void TestGetClipLength() {
        /// ---------------------------------------------
        /// Invalid case (Method skipped)
        /// ---------------------------------------------
        AudioError error = m_fluentAudioManager.GetClipLength(out double length).Execute();
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.IsNaN(length);
        Assert.AreEqual(m_audioError, error);

        /// ---------------------------------------------
        /// Valid case (Method executed)
        /// ---------------------------------------------
        m_audioError = AudioError.OK;
        m_fluentAudioManager.ReuseInstance(m_audioManager, m_audioSourceName, m_childType, m_audioError);
        error = m_fluentAudioManager.GetClipLength(out length).Execute();
        Assert.AreEqual(m_audioError, error);
        Assert.IsNaN(length);
    }

    [Test]
    public void TestSetStartTime() {
        float startTime = float.NaN;

        /// ---------------------------------------------
        /// Invalid case (Method skipped)
        /// ---------------------------------------------
        AudioError error = m_fluentAudioManager.SetStartTime(startTime).Execute();
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(m_audioError, error);

        /// ---------------------------------------------
        /// Valid case (Method executed)
        /// ---------------------------------------------
        m_audioError = AudioError.OK;
        m_fluentAudioManager.ReuseInstance(m_audioManager, m_audioSourceName, m_childType, m_audioError);
        error = m_fluentAudioManager.SetStartTime(startTime).Execute();
        Assert.AreEqual(m_audioError, error);
    }

    [Test]
    public void TestSkipTime() {
        float time = float.NaN;

        /// ---------------------------------------------
        /// Invalid case (Method skipped)
        /// ---------------------------------------------
        AudioError error = m_fluentAudioManager.SkipTime(time).Execute();
        Assert.AreNotEqual(AudioError.OK, error);
        Assert.AreEqual(m_audioError, error);

        /// ---------------------------------------------
        /// Valid case (Method executed)
        /// ---------------------------------------------
        m_audioError = AudioError.OK;
        m_fluentAudioManager.ReuseInstance(m_audioManager, m_audioSourceName, m_childType, m_audioError);
        error = m_fluentAudioManager.SkipTime(time).Execute();
        Assert.AreEqual(m_audioError, error);
    }
}

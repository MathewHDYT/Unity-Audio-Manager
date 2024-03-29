using AudioManager.Core;
using AudioManager.Locator;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class TestNullAudioManager {
    private IAudioManager m_am;
    private string m_text;
    private float m_val;
    private ChildType m_child;
    private Vector3 m_pos;
    private GameObject m_go;
    private ProgressCoroutineCallback m_pCb;
    private SourceChangedCallback m_sCb;
    private AudioMixerGroup m_mixer;

    [SetUp]
    public void TestSetUp() {
        m_text = string.Empty;
        m_val = float.NaN;
        m_child = ChildType.ALL;
        m_pos = Vector3.zero;
        m_go = null;
        m_pCb = null;
        m_sCb = null;
        m_mixer = null;
        m_am = new NullAudioManager();
    }

    [Test]
    public void TestAddSoundFromPath() {
        AudioError error = m_am.AddSoundFromPath(m_text, m_text);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);
    }

    [Test]
    public void TestGetEnumerator() {
        IEnumerable<string> sounds = m_am.GetEnumerator();
        Assert.IsNull(sounds);
    }

    [Test]
    public void TestPlay() {
        AudioError error = m_am.Play(m_text);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);
    }

    [Test]
    public void TestPlayAtTimeStamp() {
        AudioError error = m_am.PlayAtTimeStamp(m_text, m_val);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);
    }

    [Test]
    public void TestGetPlaybackPosition() {
        AudioError error = m_am.GetPlaybackPosition(m_text, out float time);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);
        Assert.IsNaN(time);
    }

    [Test]
    public void TestSetPlaybackDirection() {
        AudioError error = m_am.SetPlaybackDirection(m_text, m_val);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);
    }

    [Test]
    public void TestRegisterChildAt3DPos() {
        AudioError error = m_am.RegisterChildAt3DPos(m_text, m_pos, out ChildType child);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);
        Assert.AreEqual(ChildType.AT_3D_POS, child);
    }

    [Test]
    public void TestRegisterAttachedToGo() {
        AudioError error = m_am.RegisterChildAttachedToGo(m_text, m_go, out ChildType child);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);
        Assert.AreEqual(ChildType.ATTCHD_TO_GO, child);
    }

    [Test]
    public void TestPlayDelayed() {
        AudioError error = m_am.PlayDelayed(m_text, m_val);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);
    }

    [Test]
    public void TestPlayOneShot() {
        AudioError error = m_am.PlayOneShot(m_text);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);
    }

    [Test]
    public void TestChangePitch() {
        AudioError error = m_am.ChangePitch(m_text, m_val, m_val);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);
    }

    [Test]
    public void TestDeregisterChild() {
        AudioError error = m_am.DeregisterChild(m_text, m_child);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);
    }

    [Test]
    public void TestPlayScheduled() {
        AudioError error = m_am.PlayScheduled(m_text, m_val);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);
    }

    [Test]
    public void TestStop() {
        AudioError error = m_am.Stop(m_text);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);
    }

    [Test]
    public void TestToggleMute() {
        AudioError error = m_am.ToggleMute(m_text);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);
    }

    [Test]
    public void TestTogglePause() {
        AudioError error = m_am.TogglePause(m_text);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);
    }

    [Test]
    public void TestSubscribeSourceChanged() {
        AudioError error = m_am.SubscribeSourceChanged(m_text, m_sCb);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);
    }

    [Test]
    public void TestUnsubscribeSourceChanged() {
        AudioError error = m_am.UnsubscribeSourceChanged(m_text, m_sCb);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);
    }

    [Test]
    public void TestSubscribeProgressCoroutine() {
        AudioError error = m_am.SubscribeProgressCoroutine(m_text, m_val, m_pCb);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);
    }

    [Test]
    public void TestUnsubscribeProgressCoroutine() {
        AudioError error = m_am.UnsubscribeProgressCoroutine(m_text, m_val);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);
    }

    [Test]
    public void TestGetProgress() {
        AudioError error = m_am.GetProgress(m_text, out float progress);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);
        Assert.IsNaN(progress);
    }

    [Test]
    public void TestTryGetSource() {
        AudioError error = m_am.TryGetSource(m_text, out var source);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);
        Assert.IsNull(source);
    }

    [Test]
    public void TestLerpPitch() {
        AudioError error = m_am.LerpPitch(m_text, m_val);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);
    }

    [Test]
    public void TestLerpVolume() {
        AudioError error = m_am.LerpVolume(m_text, m_val);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);
    }

    [Test]
    public void TestChangeGroupValue() {
        AudioError error = m_am.ChangeGroupValue(m_text, m_text, m_val);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);
    }

    [Test]
    public void TestGetGroupValue() {
        AudioError error = m_am.GetGroupValue(m_text, m_text, out float currentValue);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);
        Assert.IsNaN(currentValue);
    }

    [Test]
    public void TestResetGroupValue() {
        AudioError error = m_am.ResetGroupValue(m_text, m_text);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);
    }

    [Test]
    public void TestLerpGroupValue() {
        AudioError error = m_am.LerpGroupValue(m_text, m_text, m_val);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);
    }

    [Test]
    public void TestRemoveGroup() {
        AudioError error = m_am.RemoveGroup(m_text);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);
    }

    [Test]
    public void TestAddGroup() {
        AudioError error = m_am.AddGroup(m_text, m_mixer);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);
    }

    [Test]
    public void TestRemoveSound() {
        AudioError error = m_am.RemoveSound(m_text);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);
    }

    [Test]
    public void TestSet3DAudioOptions() {
        AudioError error = m_am.Set3DAudioOptions(m_text);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);
    }

    [Test]
    public void TestGetClipLength() {
        AudioError error = m_am.GetClipLength(m_text, out _);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);
    }

    [Test]
    public void TestSetStartTime() {
        AudioError error = m_am.SetStartTime(m_text, m_val);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);
    }

    [Test]
    public void TestSkipTime() {
        AudioError error = m_am.SkipTime(m_text, m_val);
        Assert.AreEqual(AudioError.NOT_INITIALIZED, error);
    }
}

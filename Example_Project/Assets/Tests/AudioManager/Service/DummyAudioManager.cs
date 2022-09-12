using AudioManager.Core;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public sealed class DummyAudioManager : IAudioManager {
    public AudioError AddSoundFromPath(string name, string path, float volume, float pitch, bool loop, AudioSource source, AudioMixerGroup mixerGroup) {
        return AudioError.OK;
    }

    public IEnumerable<string> GetEnumerator() {
        return null;
    }

    public AudioError Play(string name, ChildType child) {
        return AudioError.OK;
    }

    public AudioError PlayAtTimeStamp(string name, float startTime, ChildType child) {
        return AudioError.OK;
    }

    public AudioError GetPlaybackPosition(string name, out float time, ChildType child) {
        time = Constants.F_NULL_VALUE;
        return AudioError.OK;
    }

    public AudioError SetPlaybackDirection(string name, float pitch, ChildType child) {
        return AudioError.OK;
    }

    public AudioError RegisterChildAt3DPos(string name, Vector3 position, out ChildType child) {
        child = ChildType.AT_3D_POS;
        return AudioError.OK;
    }

    public AudioError RegisterChildAttachedToGo(string name, GameObject gameObject, out ChildType child) {
        child = ChildType.ATTCHD_TO_GO;
        return AudioError.OK;
    }

    public AudioError DeregisterChild(string name, ChildType child) {
        return AudioError.OK;
    }

    public AudioError PlayDelayed(string name, float delay, ChildType child) {
        return AudioError.OK;
    }

    public AudioError PlayOneShot(string name, ChildType child) {
        return AudioError.OK;
    }

    public AudioError ChangePitch(string name, float minPitch, float maxPitch, ChildType child) {
        return AudioError.OK;
    }

    public AudioError GetClipLength(string name, out double length, ChildType child) {
        length = Constants.D_NULL_VALUE;
        return AudioError.OK;
    }

    public AudioError PlayScheduled(string name, double time, ChildType child) {
        return AudioError.OK;
    }

    public AudioError Stop(string name, ChildType child) {
        return AudioError.OK;
    }

    public AudioError ToggleMute(string name, ChildType child) {
        return AudioError.OK;
    }

    public AudioError TogglePause(string name, ChildType child) {
        return AudioError.OK;
    }

    public AudioError SubscribeSourceChanged(string name, SourceChangedCallback callback) {
        return AudioError.OK;
    }

    public AudioError UnsubscribeSourceChanged(string name, SourceChangedCallback callback) {
        return AudioError.OK;
    }

    public AudioError SubscribeProgressCoroutine(string name, float progress, ProgressCoroutineCallback callback) {
        return AudioError.OK;
    }

    public AudioError UnsubscribeProgressCoroutine(string name, float progress) {
        return AudioError.OK;
    }

    public AudioError GetProgress(string name, out float progress, ChildType child) {
        progress = Constants.F_NULL_VALUE;
        return AudioError.OK;
    }

    public AudioError TryGetSource(string name, out AudioSourceWrapper source) {
        source = null;
        return AudioError.OK;
    }

    public AudioError LerpPitch(string name, float endValue, float duration, ChildType child) {
        return AudioError.OK;
    }

    public AudioError LerpVolume(string name, float endValue, float duration, ChildType child) {
        return AudioError.OK;
    }

    public AudioError ChangeGroupValue(string name, string exposedParameterName, float newValue) {
        return AudioError.OK;
    }

    public AudioError GetGroupValue(string name, string exposedParameterName, out float currentValue) {
        currentValue = Constants.F_NULL_VALUE;
        return AudioError.OK;
    }

    public AudioError ResetGroupValue(string name, string exposedParameterName) {
        return AudioError.OK;
    }

    public AudioError LerpGroupValue(string name, string exposedParameterName, float endValue, float duration) {
        return AudioError.OK;
    }

    public AudioError RemoveGroup(string name, ChildType child) {
        return AudioError.OK;
    }

    public AudioError AddGroup(string name, AudioMixerGroup mixerGroup, ChildType child) {
        return AudioError.OK;
    }

    public AudioError RemoveSound(string name) {
        return AudioError.OK;
    }

    public AudioError Set3DAudioOptions(string name, float minDistance, float maxDistance, ChildType child, float spatialBlend, float spread, float dopplerLevel, AudioRolloffMode rolloffMode) {
        return AudioError.OK;
    }

    public AudioError SetStartTime(string name, float startTime, ChildType child) {
        return AudioError.OK;
    }

    public AudioError SkipTime(string name, float time, ChildType child) {
        return AudioError.OK;
    }
}

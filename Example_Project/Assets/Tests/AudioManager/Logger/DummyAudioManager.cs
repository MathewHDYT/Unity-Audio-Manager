using AudioManager.Core;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class DummyAudioManager : IAudioManager {
    public AudioError AddSoundFromPath(string name, string path, float volume, float pitch, bool loop, AudioSource source, AudioMixerGroup mixerGroup) {
        return AudioError.OK;
    }

    public IEnumerable<string> GetEnumerator() {
        return null;
    }

    public AudioError Play(string name) {
        return AudioError.OK;
    }

    public AudioError PlayAtTimeStamp(string name, float startTime) {
        return AudioError.OK;
    }

    public AudioError GetPlaybackPosition(string name, out float time) {
        time = Constants.NULL_VALUE;
        return AudioError.OK;
    }

    public AudioError SetPlaybackDirection(string name, float pitch) {
        return AudioError.OK;
    }

    public AudioError PlayAt3DPosition(string name, Vector3 position) {
        return AudioError.OK;
    }

    public AudioError PlayOneShotAt3DPosition(string name, Vector3 position) {
        return AudioError.OK;
    }

    public AudioError PlayAttachedToGameObject(string name, GameObject gameObject) {
        return AudioError.OK;
    }

    public AudioError PlayOneShotAttachedToGameObject(string name, GameObject gameObject) {
        return AudioError.OK;
    }

    public AudioError PlayDelayed(string name, float delay) {
        return AudioError.OK;
    }

    public AudioError PlayOneShot(string name) {
        return AudioError.OK;
    }

    public AudioError ChangePitch(string name, float minPitch, float maxPitch) {
        return AudioError.OK;
    }

    public AudioError PlayScheduled(string name, double time) {
        return AudioError.OK;
    }

    public AudioError Stop(string name) {
        return AudioError.OK;
    }

    public AudioError ToggleMute(string name) {
        return AudioError.OK;
    }

    public AudioError TogglePause(string name) {
        return AudioError.OK;
    }

    public AudioError SubscribeSourceChanged(string name, SourceChangedCallback callback) {
        return AudioError.OK;
    }

    public AudioError UnsubscribeSourceChanged(string name, SourceChangedCallback callback) {
        return AudioError.OK;
    }

    public AudioError SubscribeProgressCoroutine(string name, float progress, AudioFinishedCallback callback) {
        return AudioError.OK;
    }

    public AudioError UnsubscribeProgressCoroutine(string name, float progress) {
        return AudioError.OK;
    }

    public AudioError GetProgress(string name, out float progress) {
        progress = Constants.NULL_VALUE;
        return AudioError.OK;
    }

    public AudioError TryGetSource(string name, out AudioSourceWrapper source) {
        source = null;
        return AudioError.OK;
    }

    public AudioError LerpPitch(string name, float endValue, float waitTime, int granularity) {
        return AudioError.OK;
    }

    public AudioError LerpVolume(string name, float endValue, float waitTime, int granularity) {
        return AudioError.OK;
    }

    public AudioError ChangeGroupValue(string name, string exposedParameterName, float newValue) {
        return AudioError.OK;
    }

    public AudioError GetGroupValue(string name, string exposedParameterName, out float currentValue) {
        currentValue = Constants.NULL_VALUE;
        return AudioError.OK;
    }

    public AudioError ResetGroupValue(string name, string exposedParameterName) {
        return AudioError.OK;
    }

    public AudioError LerpGroupValue(string name, string exposedParameterName, float endValue, float waitTime, int granularity) {
        return AudioError.OK;
    }

    public AudioError RemoveGroup(string name) {
        return AudioError.OK;
    }

    public AudioError AddGroup(string name, AudioMixerGroup mixerGroup) {
        return AudioError.OK;
    }

    public AudioError RemoveSound(string name) {
        return AudioError.OK;
    }

    public AudioError Set3DAudioOptions(string name, float minDistance, float maxDistance, float spatialBlend, float spread, float dopplerLevel, AudioRolloffMode rolloffMode) {
        return AudioError.OK;
    }

    public AudioError SetStartTime(string name, float startTime) {
        return AudioError.OK;
    }

    public AudioError SkipTime(string name, float time) {
        return AudioError.OK;
    }
}

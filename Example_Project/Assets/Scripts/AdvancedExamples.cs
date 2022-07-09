using AudioManager.Core;
using AudioManager.Helper;
using AudioManager.Locator;
using AudioManager.Service;
using AudioManager.Settings;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AdvancedExamples : MonoBehaviour {
    [SerializeField]
    private AdvancedExample advancedExample;

    [Header("Lerp in 3d sound at position section:")]
    [SerializeField]
    [Tooltip("Song that should be looped between the given start and end position.")]
    private AudioSourceSetting sound3D;
    [SerializeField]
    [Tooltip("Position we should play the song at.")]
    private Vector3 position3D;
    [SerializeField]
    [Tooltip("Amount of time until the song is completly faded in.")]
    private float fadeInTime3D;

    [Header("Loop song sub-section:")]
    [SerializeField]
    [Tooltip("Song that should be looped between the given start and end position.")]
    private AudioSourceSetting loopSound;
    [SerializeField]
    [Tooltip("Moment in the clip that the loop should start at.")]
    private float loopStart;
    [SerializeField]
    [Tooltip("Moment in the clip that the loop should end at.")]
    private float loopEnd;

    [Header("Reverse Loop song sub-section:")]
    [SerializeField]
    [Tooltip("Song that should be looped between the given start and end position.")]
    private AudioSourceSetting reverseLoopSound;
    [SerializeField]
    [Tooltip("How many seconds into the song the loop should start.")]
    private float reverseLoopStart;
    [SerializeField]
    [Tooltip("After how many seconds in the loop the song should reset to the reverseLoopStart.")]
    private float reverseLoopEnd;

    [Header("Play song after another example:")]
    [SerializeField]
    [Tooltip("Song that should be first and faded out.")]
    private AudioSourceSetting firstSound;
    [SerializeField]
    [Tooltip("Song that should be second and faded in.")]
    private AudioSourceSetting secondSound;

    [Header("Play song in reverse:")]
    [SerializeField]
    [Tooltip("Song that should be played in reverse or normal direction.")]
    private AudioSourceSetting reverseSound;
    [SerializeField]
    [Tooltip("Pitch that will be set and decides , which direction to play in (normal > 0, reverse < 0).")]
    private float directionPitch;

    [Header("Fade in song:")]
    [SerializeField]
    [Tooltip("Song that should be slowly faded in.")]
    private AudioSourceSetting fadeInSong;
    [SerializeField]
    [Tooltip("Time until the clip has completly faded in.")]
    private float clipFadeInTime;
    [SerializeField]
    [Tooltip("End volume of the clip when it has completly faded in.")]
    private float fadeInEndVolume;

    [Header("Fade out song:")]
    [SerializeField]
    [Tooltip("Song that should be played normally then slowly faded out.")]
    private AudioSourceSetting fadeOutSong;
    [SerializeField]
    [Tooltip("Time until the clip has completly faded out.")]
    private float clipFadeOutTime;

    [Header("Change volume via. UI:")]
    [SerializeField]
    [Tooltip("Song that should be played normally and adjusted to the group volume.")]
    private AudioSourceSetting groupSong;
    [SerializeField]
    [Tooltip("Slider that lets us edit the volume from 0.0001 to 1.")]
    private Slider uiSlider;

    private const string EXPOSED_VOLUME_NAME = "Volume";

    private IAudioManager am;

    private void Start() {
        SettingsHelper.SetupSounds(out var sounds, new AudioSourceSetting[] { sound3D, loopSound, reverseLoopSound, firstSound, secondSound, reverseSound, fadeInSong, fadeOutSong, groupSong }, this.gameObject);
        ServiceLocator.RegisterService(new DefaultAudioManager(sounds, this.gameObject));
        am = ServiceLocator.GetService();

        switch (advancedExample) {
            case AdvancedExample.NONE:
                // Nothing to do.
                break;
            case AdvancedExample.LERP_IN_3D_SOUND_AT_POS:
                InitLerpIn3DSoundAtPosExample();
                break;
            case AdvancedExample.LOOP_SONG_SUB_SECTION:
                InitLoopSubSectionExample();
                break;
            case AdvancedExample.REVERSE_LOOP_SONG_SUB_SECTION:
                InitReverseLoopSubSectionExample();
                break;
            case AdvancedExample.PLAY_SONG_AFTER_ANOTHER:
                InitPlaySongAfterAnotherExample();
                break;
            case AdvancedExample.PLAY_SONG_IN_REVERSE:
                InitPlaySongInReverseExample();
                break;
            case AdvancedExample.FADE_IN_SONG:
                InitFadeInSongExample();
                break;
            case AdvancedExample.FADE_OUT_SONG:
                InitFadeOutSongExample();
                break;
            case AdvancedExample.CHANGE_VOLUME_VIA_UI:
                InitChangeVolumeViaUIExample();
                break;
            default:
                // Unexpected AdvancedExample argument.
                break;
        }
    }

    public ProgressResponse SongProgressCallback(string name, float progress, ChildType child) {
        am.TryGetSource(name, out var source);
        Debug.Log("ChildType: " + Enum.GetName(typeof(ChildType), child) + ", Actual: " + source.Time + ", Expected: " + (progress * source.Source.clip.length));

        ProgressResponse response = ProgressResponse.UNSUB;
        switch (advancedExample) {
            case AdvancedExample.NONE:
                // Nothing to do.
                break;
            case AdvancedExample.LERP_IN_3D_SOUND_AT_POS:
                response = HandleLerpIn3DSoundAtPosExample(name);
                break;
            case AdvancedExample.LOOP_SONG_SUB_SECTION:
                response = HandleLoopSubSectionExample(name);
                break;
            case AdvancedExample.REVERSE_LOOP_SONG_SUB_SECTION:
                response = HandleReverseLoopSubSectionExample(name);
                break;
            case AdvancedExample.PLAY_SONG_AFTER_ANOTHER:
                // Nothing to do.
                break;
            case AdvancedExample.PLAY_SONG_IN_REVERSE:
                // Nothing to do.
                break;
            case AdvancedExample.FADE_IN_SONG:
                response = HandleFadeInSongExample(name);
                break;
            case AdvancedExample.FADE_OUT_SONG:
                response = HandleFadeOutSongExample(name, progress);
                break;
            case AdvancedExample.CHANGE_VOLUME_VIA_UI:
                // Nothing to do.
                break;
            default:
                // Unexpected AdvancedExample argument.
                break;
        }
        return response;
    }

    private void InitLerpIn3DSoundAtPosExample() {
        am.SubscribeProgressCoroutine(sound3D.soundName, 0f, SongProgressCallback);
        am.RegisterChildAt3DPos(sound3D.soundName, position3D, out ChildType child);
        am.Play(sound3D.soundName, child);
    }

    private ProgressResponse HandleLerpIn3DSoundAtPosExample(string name) {
        am.TryGetSource(name, out var source);
        float endValue = source.Volume;
        source.Volume = 0f;
        am.LerpVolume(name, endValue, fadeInTime3D);
        return ProgressResponse.RESUB_IN_LOOP;
    }

    private void InitLoopSubSectionExample() {
        am.TryGetSource(loopSound.soundName, out var source);
        source.Time = loopStart;
        float progress = (loopEnd / source.Source.clip.length);
        am.SubscribeProgressCoroutine(loopSound.soundName, progress, SongProgressCallback);
        am.Play(loopSound.soundName);
    }

    private ProgressResponse HandleLoopSubSectionExample(string name) {
        float skipTime = loopStart - loopEnd;
        am.SkipTime(name, skipTime);
        return ProgressResponse.RESUB_IMMEDIATE;
    }

    private void InitReverseLoopSubSectionExample() {
        am.SetPlaybackDirection(reverseLoopSound.soundName);
        am.TryGetSource(reverseLoopSound.soundName, out var source);
        source.Time = source.Source.clip.length - reverseLoopStart;
        float progress = (source.Source.clip.length - reverseLoopEnd - reverseLoopStart) / source.Source.clip.length;
        am.SubscribeProgressCoroutine(reverseLoopSound.soundName, progress, SongProgressCallback);
        am.Play(reverseLoopSound.soundName);
    }

    private ProgressResponse HandleReverseLoopSubSectionExample(string name) {
        float skipTime = reverseLoopEnd;
        am.SkipTime(name, skipTime);
        return ProgressResponse.RESUB_IMMEDIATE;
    }

    private void InitPlaySongAfterAnotherExample() {
        am.PlayScheduled(firstSound.soundName, 0d);
        am.GetClipLength(firstSound.soundName, out double time);
        am.PlayScheduled(secondSound.soundName, time);
    }

    private void InitPlaySongInReverseExample() {
        am.SetPlaybackDirection(reverseSound.soundName, directionPitch);
        am.Play(reverseSound.soundName);
    }

    private void InitFadeInSongExample() {
        am.SubscribeProgressCoroutine(fadeInSong.soundName, 0f, SongProgressCallback);
        am.Play(fadeInSong.soundName);
    }

    private ProgressResponse HandleFadeInSongExample(string name) {
        am.TryGetSource(name, out var source);
        source.Volume = 0f;
        am.LerpVolume(name, fadeInEndVolume, clipFadeInTime);
        return ProgressResponse.RESUB_IN_LOOP;
    }

    private void InitFadeOutSongExample() {
        am.TryGetSource(fadeOutSong.soundName, out var source);
        float progress = ((source.Source.clip.length - clipFadeOutTime) / source.Source.clip.length);
        am.SubscribeProgressCoroutine(fadeOutSong.soundName, progress, SongProgressCallback);
        am.Play(fadeOutSong.soundName);
    }

    private ProgressResponse HandleFadeOutSongExample(string name, float progress) {
        am.LerpVolume(name, 0f, progress);
        return ProgressResponse.UNSUB;
    }

    private void InitChangeVolumeViaUIExample() {
        // Turn value from 0.0001 to 1 into a logarithmic scale from 0 to -80db.
        float newVolume = Mathf.Log10(uiSlider.value) * 20;
        am.ChangeGroupValue(groupSong.soundName, EXPOSED_VOLUME_NAME, newVolume);
        am.Play(groupSong.soundName);
    }

    public void HandleChangeVolumeViaUIExample(float sliderValue) {
        // Turn value from 0.0001 to 1 into a logarithmic scale from 0 to -80db.
        float newVolume = Mathf.Log10(sliderValue) * 20;
        am.ChangeGroupValue(groupSong.soundName, EXPOSED_VOLUME_NAME, newVolume);
    }
}

using AudioManager.Core;
using AudioManager.Helper;
using AudioManager.Locator;
using AudioManager.Service;
using AudioManager.Settings;
using System.Collections;
using UnityEngine;

public class AdvancedExamples : MonoBehaviour {

    [SerializeField]
    private AdvancedExample advancedExample;

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

    [Header("Transition between songs:")]
    [SerializeField]
    [Tooltip("Song that should be first and faded out.")]
    private AudioSourceSetting firstSound;
    [SerializeField]
    [Tooltip("Song that should be second and faded in.")]
    private AudioSourceSetting secondSound;
    [SerializeField]
    [Tooltip("Amount of time until the first song is completly faded out and the second completly faded in minus the " + nameof(intermissionDelay) + ".")]
    private float transitionTime;
    [SerializeField]
    [Tooltip("Amount of time until the second song fades in when the first has started fading out.")]
    private float intermissionDelay;
    [SerializeField]
    [Tooltip("How many steps we take to decrease the volume more means a smoother decrease, normally around 5-10 is more than enough tough.")]
    private int transitionGranularity;

    [Header("Play song in reverse:")]
    [SerializeField]
    [Tooltip("Song that should be played in reverse or normal direction.")]
    private AudioSourceSetting reverseSound;
    [SerializeField]
    [Tooltip("Pitch that will be set and decides , which direction to play in (normal > 0, reverse < 0).")]
    private float directionPitch;

    private IAudioManager am;

    private void Start() {
        SettingsHelper.SetupSounds(out var sounds, new AudioSourceSetting[] { loopSound, firstSound, secondSound, reverseSound }, this.gameObject);
        ServiceLocator.RegisterService(new DefaultAudioManager(sounds, this.gameObject));
        am = ServiceLocator.GetService();

        switch (advancedExample) {
            case AdvancedExample.NONE:
                // Nothing to do.
                break;
            case AdvancedExample.LOOP_SONG_SUB_SECTION:
                InitLoopSubSectionExample();
                break;
            case AdvancedExample.TRANSITION_BETWEEN_SONGS:
                InitTransitionBetweenSongsExample();
                break;
            case AdvancedExample.PLAY_SONG_IN_REVERSE:
                InitPlayInReverseExample();
                break;
            default:
                // Unexpected AdvancedExample argument.
                break;
        }
    }

    private void SongProgressCallback(string name, float remainingTime) {
        switch (advancedExample) {
            case AdvancedExample.NONE:
                // Nothing to do.
                break;
            case AdvancedExample.LOOP_SONG_SUB_SECTION:
                HandleLoopSubSectionExample(name, remainingTime);
                break;
            case AdvancedExample.TRANSITION_BETWEEN_SONGS:
                HandleTransitionBetweenSongsExample(name, remainingTime);
                break;
            case AdvancedExample.PLAY_SONG_IN_REVERSE:
                // Nothing to do.
                break;
            default:
                // Unexpected AdvancedExample argument.
                break;
        }
    }

    private void InitLoopSubSectionExample() {
        am.TryGetSource(loopSound.soundName, out AudioSource source);
        source.loop = true;
        source.time = loopStart;
        float remainingTime = source.clip.length - loopEnd;
        am.SubscribeAudioFinished(loopSound.soundName, remainingTime, SongProgressCallback);
        am.Play(loopSound.soundName);
    }

    private void HandleLoopSubSectionExample(string name, float remainingTime) {
        float skipTime = loopEnd - loopStart;
        am.SkipBackward(name, skipTime);
        am.SubscribeAudioFinished(name, remainingTime, SongProgressCallback);
    }

    private void InitTransitionBetweenSongsExample() {
        am.SubscribeAudioFinished(firstSound.soundName, transitionTime, SongProgressCallback);
        am.Play(firstSound.soundName);
    }

    private void HandleTransitionBetweenSongsExample(string name, float remainingTime) {
        am.LerpVolume(name, 0f, remainingTime, transitionGranularity);
        am.TryGetSource(secondSound.soundName, out AudioSource source);
        float endValue = source.volume;
        source.volume = 0f;
        StartCoroutine(DelayedPlay(endValue, remainingTime));
    }

    private void InitPlayInReverseExample() {
        am.SetPlaypbackDirection(reverseSound.soundName, directionPitch);
        am.Play(reverseSound.soundName);
    }

    private IEnumerator DelayedPlay(float endValue, float remainingTime) {
        yield return new WaitForSeconds(intermissionDelay);
        am.Play(secondSound.soundName);
        am.LerpVolume(secondSound.soundName, endValue, remainingTime - intermissionDelay, transitionGranularity);
    }
}

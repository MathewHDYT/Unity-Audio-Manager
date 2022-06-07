using AudioManager.Core;
using AudioManager.Locator;
using AudioManager.Logger;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class MethodCalls : MonoBehaviour {
    [Header("Input:")]
    [SerializeField]
    private Dropdown soundNameDropDown;
    [SerializeField]
    private InputField timeInput;
    [SerializeField]
    private InputField endValueInput;
    [SerializeField]
    private InputField granularityInput;
    [SerializeField]
    private InputField minPitchInput;
    [SerializeField]
    private InputField maxPitchInput;

    [Header("Output:")]
    [SerializeField]
    private Text outputText;
    [SerializeField]
    private LoggingLevel loggingLevel;

    [Header("Background:")]
    [SerializeField]
    private Image panel;
    [SerializeField]
    private GameObject fallbackImage;
    [SerializeField]
    private GameObject[] uiPanels;
    [SerializeField]
    private string[] videoClips;
    [SerializeField]
    private VideoPlayer videoPlayer;

    [Header("Objects:")]
    [SerializeField]
    private GameObject radio;

    private IAudioManager am;
    private int lastIndex = int.MaxValue;

    private const string NOT_A_NUMBER = "Input is not a valid number in the textfield:";
    private const string EXPOSED_VOLUME_NAME = "Volume";

    private void Start() {
        ServiceLocator.RegisterLogger(new UIAudioLogger(loggingLevel, outputText), this);
        am = ServiceLocator.GetService();
#if UNITY_WEBGL
        panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, 0f);
        fallbackImage.SetActive(true);
#endif // UNITY_WEBGL
        // Initally enable first tab.
        SwitchTab(0);
    }

    public void SwitchTab(int index) {
        // Don't switch tabs, if the index didn't change.
        if (index == lastIndex) {
            return;
        }
        lastIndex = index;

        foreach (var panel in uiPanels) {
            panel.SetActive(false);
        }

        if (index < uiPanels.Length) {
            uiPanels[index].SetActive(true);
        }
#if !UNITY_WEBGL
        if (index < videoClips.Length) {
            videoPlayer.url = Path.Join(Application.streamingAssetsPath, videoClips[index]);
            videoPlayer.Play();
        }
#endif // UNITY_WEBGL
    }

    public void PlayClicked() {
        ClearText();
        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        am.Play(selectedSoundName);
    }

    public void GetEnumeratorClicked() {
        ClearText();
        if (!float.TryParse(endValueInput.text, out float endValue)) {
            SetText(string.Join(" ", NOT_A_NUMBER, "End Value"));
            return;
        }
        if (!float.TryParse(timeInput.text, out float time)) {
            SetText(string.Join(" ", NOT_A_NUMBER, "Time"));
            return;
        }
        if (!int.TryParse(granularityInput.text, out int granularity)) {
            SetText(string.Join(" ", NOT_A_NUMBER, "Granularity"));
            return;
        }

        foreach (var name in am.GetEnumerator()) {
            am.LerpVolume(name, endValue, time, granularity);
        }
    }

    public void SkipForwardClicked() {
        ClearText();
        if (!float.TryParse(timeInput.text, out float timeStamp)) {
            SetText(string.Join(" ", NOT_A_NUMBER, "Time"));
            return;
        }

        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        am.SkipForward(selectedSoundName, timeStamp);
    }

    public void SkipBackwardClicked() {
        ClearText();
        if (!float.TryParse(timeInput.text, out float timeStamp)) {
            SetText(string.Join(" ", NOT_A_NUMBER, "Time"));
            return;
        }

        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        am.SkipBackward(selectedSoundName, timeStamp);
    }

    public void SetPlaybackDirection() {
        ClearText();
        if (!float.TryParse(minPitchInput.text, out float minPitch)) {
            SetText(string.Join(" ", NOT_A_NUMBER, "Min Pitch"));
            return;
        }

        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        am.SetPlaypbackDirection(selectedSoundName, minPitch);
    }

    public void PlayAtTimeStampClicked() {
        ClearText();
        if (!float.TryParse(timeInput.text, out float timeStamp)) {
            SetText(string.Join(" ", NOT_A_NUMBER, "Time"));
            return;
        }

        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        am.PlayAtTimeStamp(selectedSoundName, timeStamp);
    }

    public void GetPlayBackPositionClicked() {
        ClearText();
        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        AudioError error = am.GetPlaybackPosition(selectedSoundName, out float time);

        if (CheckSuccess(error)) {
            AppendText(string.Join(" ", "Current playback position being:", time.ToString("0.00"), "seconds"));
        }
    }

    public void PlayAt3DPositionClicked() {
        ClearText();
        float randomXPos = Random.Range(-15f, 15f);
        float randomYPos = Random.Range(-7.5f, 10f);
        var worldPosition = new Vector3(randomXPos, randomYPos, 5f);

        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        am.PlayAt3DPosition(selectedSoundName, worldPosition);
    }

    public void PlayAttachedToGameObjectClicked() {
        ClearText();
        float randomXPos = Random.Range(-15f, 15f);
        float randomYPos = Random.Range(-7.5f, 10f);
        Vector3 worldPosition = new Vector3(randomXPos, randomYPos, 5f);

        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        am.PlayAttachedToGameObject(selectedSoundName, radio);
        radio.transform.position = worldPosition;
    }

    public void ChangePitchClicked() {
        ClearText();
        if (!float.TryParse(minPitchInput.text, out float minPitch)) {
            SetText(string.Join(" ", NOT_A_NUMBER, "Min Pitch"));
            return;
        }
        if (!float.TryParse(maxPitchInput.text, out float maxPitch)) {
            SetText(string.Join(" ", NOT_A_NUMBER, "Max Pitch"));
            return;
        }

        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        am.ChangePitch(selectedSoundName, minPitch, maxPitch);
    }

    public void PlayOneShotAt3DPositionClicked() {
        ClearText();
        float randomXPos = Random.Range(-15f, 15f);
        float randomYPos = Random.Range(-7.5f, 10f);
        var worldPosition = new Vector3(randomXPos, randomYPos, 5f);

        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        am.PlayOneShotAt3DPosition(selectedSoundName, worldPosition);
    }

    public void PlayOneShotAttachedToGameObjectClicked() {
        ClearText();
        float randomXPos = Random.Range(-15f, 15f);
        float randomYPos = Random.Range(-7.5f, 10f);
        Vector3 worldPosition = new Vector3(randomXPos, randomYPos, 5f);

        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        am.PlayOneShotAttachedToGameObject(selectedSoundName, radio);
        radio.transform.position = worldPosition;
    }

    public void PlayDelayedClicked() {
        ClearText();
        if (!float.TryParse(timeInput.text, out float delay)) {
            SetText(string.Join(" ", NOT_A_NUMBER, "Time"));
            return;
        }

        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        am.PlayDelayed(selectedSoundName, delay);
    }

    public void PlayOneShotClicked() {
        ClearText();
        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        am.PlayOneShot(selectedSoundName);
    }

    public void PlayScheduledClicked() {
        ClearText();
        if (!double.TryParse(timeInput.text, out double delay)) {
            SetText(string.Join(" ", NOT_A_NUMBER, "Time"));
            return;
        }

        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        am.PlayScheduled(selectedSoundName, delay);
    }

    public void StopClicked() {
        ClearText();
        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        am.Stop(selectedSoundName);
    }

    public void ToggleMuteClicked() {
        ClearText();
        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        am.ToggleMute(selectedSoundName);
    }

    public void TogglePauseClicked() {
        ClearText();
        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        am.TogglePause(selectedSoundName);
    }

    public void GetProgressClicked() {
        ClearText();
        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        AudioError error = am.GetProgress(selectedSoundName, out float progress);

        if (CheckSuccess(error)) {
            AppendText(string.Join(" ", "Current progress being:", (progress * 100).ToString("0.00"), "%"));
        }
    }

    public void GetSourceClicked() {
        ClearText();
        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        am.TryGetSource(selectedSoundName, out _);
    }

    public void LerpPitchClicked() {
        ClearText();
        if (!float.TryParse(endValueInput.text, out float endValue)) {
            SetText(string.Join(" ", NOT_A_NUMBER, "End Value"));
            return;
        }
        if (!float.TryParse(timeInput.text, out float time)) {
            SetText(string.Join(" ", NOT_A_NUMBER, "Time"));
            return;
        }
        if (!int.TryParse(granularityInput.text, out int granularity)) {
            SetText(string.Join(" ", NOT_A_NUMBER, "Granularity"));
            return;
        }

        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        am.LerpPitch(selectedSoundName, endValue, time, granularity);
    }

    public void LerpVolumeClicked() {
        ClearText();
        if (!float.TryParse(endValueInput.text, out float endValue)) {
            SetText(string.Join(" ", NOT_A_NUMBER, "End Value"));
            return;
        }
        if (!float.TryParse(timeInput.text, out float time)) {
            SetText(string.Join(" ", NOT_A_NUMBER, "Time"));
            return;
        }
        if (!int.TryParse(granularityInput.text, out int granularity)) {
            SetText(string.Join(" ", NOT_A_NUMBER, "Granularity"));
            return;
        }

        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        am.LerpVolume(selectedSoundName, endValue, time, granularity);
    }

    public void ChangeGroupValueClicked() {
        ClearText();
        if (!float.TryParse(endValueInput.text, out float endValue)) {
            SetText(string.Join(" ", NOT_A_NUMBER, "End Value"));
            return;
        }

        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        am.ChangeGroupValue(selectedSoundName, EXPOSED_VOLUME_NAME, endValue);
    }

    public void GetGroupValueClicked() {
        ClearText();
        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        AudioError error = am.GetGroupValue(selectedSoundName, EXPOSED_VOLUME_NAME, out float currentValue);

        if (CheckSuccess(error)) {
            AppendText(string.Join(" ", "Current group value being:", currentValue.ToString("0.00")));
        }
    }

    public void ResetGroupValueClicked() {
        ClearText();
        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        am.ResetGroupValue(selectedSoundName, EXPOSED_VOLUME_NAME);
    }

    public void LerpGroupValueClicked() {
        ClearText();
        if (!float.TryParse(endValueInput.text, out float endValue)) {
            SetText(string.Join(" ", NOT_A_NUMBER, "End Value"));
            return;
        }
        if (!float.TryParse(timeInput.text, out float time)) {
            SetText(string.Join(" ", NOT_A_NUMBER, "Time"));
            return;
        }
        if (!int.TryParse(granularityInput.text, out int granularity)) {
            SetText(string.Join(" ", NOT_A_NUMBER, "Granularity"));
            return;
        }

        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        am.LerpGroupValue(selectedSoundName, EXPOSED_VOLUME_NAME, endValue, time, granularity);
    }

    public void SetStartTimeClicked() {
        ClearText();
        if (!float.TryParse(timeInput.text, out float time)) {
            SetText(string.Join(" ", NOT_A_NUMBER, "Time"));
            return;
        }

        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        am.SetStartTime(selectedSoundName, time);
    }

    private bool CheckSuccess(AudioError error) {
        return error == AudioError.OK;
    }

    private void ClearText() {
        outputText.text = "";
    }

    private void SetText(string text) {
        AppendText(text);
    }

    private void AppendText(string text) {
        outputText.text += text;
    }
}

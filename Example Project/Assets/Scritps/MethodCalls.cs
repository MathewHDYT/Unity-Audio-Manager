using AudioManager.Locator;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("Output:")]
    [SerializeField]
    private Text outputText;
    [SerializeField]
    private Image panel;
    [SerializeField]
    private Color32 successColor;
    [SerializeField]
    private Color32 failureColor;
    [SerializeField]
    private Color32 successBackgroundColor;
    [SerializeField]
    private Color32 failureBackgroundColor;
    [SerializeField]
    private AudioManager.Logger.LoggingLevel loggingLevel;

    [Header("Objects:")]
    [SerializeField]
    private GameObject radio;

    private IAudioManager am;

    private const string NOT_A_NUMBER = "Input is not a valid number in the textfield:";
    private const string EXPOSED_VOLUME_NAME = "Volume";

    private void Start() {
        ServiceLocator.RegisterLogger(new UILogger(loggingLevel, outputText), this);
        am = ServiceLocator.GetService();
    }

    public void PlayClicked() {
        ClearText();
        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        AudioError error = am.Play(selectedSoundName);
        ChangeColorOnError(error);
    }

    public void PlayAtTimeStampClicked() {
        ClearText();
        if (!float.TryParse(timeInput.text, out float timeStamp)) {
            SetTextAndColor(string.Join(" ", NOT_A_NUMBER, "Time"));
            return;
        }

        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        AudioError error = am.PlayAtTimeStamp(selectedSoundName, timeStamp);
        ChangeColorOnError(error);
    }

    public void GetPlayBackPositionClicked() {
        ClearText();
        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        ValueDataError<float> valueDataError = am.GetPlaybackPosition(selectedSoundName);
        ChangeColorOnError(valueDataError.Error);

        if (CheckSuccess(valueDataError.Error)) {
            AppendText(string.Join(" ", "Current playback position being:", valueDataError.Value.ToString("0.00"), "seconds"));
        }
    }

    public void PlayAt3DPositionClicked() {
        ClearText();
        float randomXPos = Random.Range(-15f, 15f);
        float randomYPos = Random.Range(-7.5f, 10f);
        var worldPosition = new Vector3(randomXPos, randomYPos, 5f);

        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        AudioError error = am.PlayAt3DPosition(selectedSoundName, worldPosition);
        ChangeColorOnError(error);
    }

    public void PlayAttachedToGameObjectClicked() {
        ClearText();
        float randomXPos = Random.Range(-15f, 15f);
        float randomYPos = Random.Range(-7.5f, 10f);
        Vector3 worldPosition = new Vector3(randomXPos, randomYPos, 5f);

        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        AudioError error = am.PlayAttachedToGameObject(selectedSoundName, radio);
        radio.transform.position = worldPosition;
        ChangeColorOnError(error);
    }

    public void PlayOneShotAt3DPositionClicked() {
        ClearText();
        float randomXPos = Random.Range(-15f, 15f);
        float randomYPos = Random.Range(-7.5f, 10f);
        var worldPosition = new Vector3(randomXPos, randomYPos, 5f);

        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        AudioError error = am.PlayOneShotAt3DPosition(selectedSoundName, worldPosition);
        ChangeColorOnError(error);
    }

    public void PlayOneShotAttachedToGameObjectClicked() {
        ClearText();
        float randomXPos = Random.Range(-15f, 15f);
        float randomYPos = Random.Range(-7.5f, 10f);
        Vector3 worldPosition = new Vector3(randomXPos, randomYPos, 5f);

        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        AudioError error = am.PlayOneShotAttachedToGameObject(selectedSoundName, radio);
        radio.transform.position = worldPosition;
        ChangeColorOnError(error);
    }

    public void PlayDelayedClicked() {
        ClearText();
        if (!float.TryParse(timeInput.text, out float delay)) {
            SetTextAndColor(string.Join(" ", NOT_A_NUMBER, "Time"));
            return;
        }

        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        AudioError error = am.PlayDelayed(selectedSoundName, delay);
        ChangeColorOnError(error);
    }

    public void PlayOneShotClicked() {
        ClearText();
        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        AudioError error = am.PlayOneShot(selectedSoundName);
        ChangeColorOnError(error);
    }

    public void PlayScheduledClicked() {
        ClearText();
        if (!double.TryParse(timeInput.text, out double delay)) {
            SetTextAndColor(string.Join(" ", NOT_A_NUMBER, "Time"));
            return;
        }

        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        AudioError error = am.PlayScheduled(selectedSoundName, delay);
        ChangeColorOnError(error);
    }

    public void StopClicked() {
        ClearText();
        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        AudioError error = am.Stop(selectedSoundName);
        ChangeColorOnError(error);
    }

    public void ToggleMuteClicked() {
        ClearText();
        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        AudioError error = am.ToggleMute(selectedSoundName);
        ChangeColorOnError(error);
    }

    public void TogglePauseClicked() {
        ClearText();
        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        AudioError error = am.TogglePause(selectedSoundName);
        ChangeColorOnError(error);
    }

    public void GetProgressClicked() {
        ClearText();
        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        ValueDataError<float> valueDataError = am.GetProgress(selectedSoundName);
        ChangeColorOnError(valueDataError.Error);

        if (CheckSuccess(valueDataError.Error)) {
            AppendText(string.Join(" ", "Current progress being:", (valueDataError.Value * 100).ToString("0.00"), "%"));
        }
    }

    public void GetSourceClicked() {
        ClearText();
        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        AudioError error = am.TryGetSource(selectedSoundName, out _);
        ChangeColorOnError(error);
    }

    public void LerpPitchClicked() {
        ClearText();
        if (!float.TryParse(endValueInput.text, out float endValue)) {
            SetTextAndColor(string.Join(" ", NOT_A_NUMBER, "End Value"));
            return;
        }
        if (!float.TryParse(timeInput.text, out float time)) {
            SetTextAndColor(string.Join(" ", NOT_A_NUMBER, "Time"));
            return;
        }
        if (!float.TryParse(granularityInput.text, out float granularity)) {
            SetTextAndColor(string.Join(" ", NOT_A_NUMBER, "Granularity"));
            return;
        }

        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        AudioError error = am.LerpPitch(selectedSoundName, endValue, time, granularity);
        ChangeColorOnError(error);
    }

    public void LerpVolumeClicked() {
        ClearText();
        if (!float.TryParse(endValueInput.text, out float endValue)) {
            SetTextAndColor(string.Join(" ", NOT_A_NUMBER, "End Value"));
            return;
        }
        if (!float.TryParse(timeInput.text, out float time)) {
            SetTextAndColor(string.Join(" ", NOT_A_NUMBER, "Time"));
            return;
        }
        if (!float.TryParse(granularityInput.text, out float granularity)) {
            SetTextAndColor(string.Join(" ", NOT_A_NUMBER, "Granularity"));
            return;
        }

        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        AudioError error = am.LerpVolume(selectedSoundName, endValue, time, granularity);
        ChangeColorOnError(error);
    }

    public void ChangeGroupValueClicked() {
        ClearText();
        if (!float.TryParse(endValueInput.text, out float endValue)) {
            SetTextAndColor(string.Join(" ", NOT_A_NUMBER, "End Value"));
            return;
        }

        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        AudioError error = am.ChangeGroupValue(selectedSoundName, EXPOSED_VOLUME_NAME, endValue);
        ChangeColorOnError(error);
    }

    public void GetGroupValueClicked() {
        ClearText();
        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        ValueDataError<float> valueDataError = am.GetGroupValue(selectedSoundName, EXPOSED_VOLUME_NAME);
        ChangeColorOnError(valueDataError.Error);

        if (CheckSuccess(valueDataError.Error)) {
            AppendText(string.Join(" ", "Current group value being:", valueDataError.Value.ToString("0.00")));
        }
    }

    public void ResetGroupValueClicked() {
        ClearText();
        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        AudioError error = am.ResetGroupValue(selectedSoundName, EXPOSED_VOLUME_NAME);
        ChangeColorOnError(error);
    }

    public void LerpGroupValueClicked() {
        ClearText();
        if (!float.TryParse(endValueInput.text, out float endValue)) {
            SetTextAndColor(string.Join(" ", NOT_A_NUMBER, "End Value"));
            return;
        }
        if (!float.TryParse(timeInput.text, out float time)) {
            SetTextAndColor(string.Join(" ", NOT_A_NUMBER, "Time"));
            return;
        }
        if (!float.TryParse(granularityInput.text, out float granularity)) {
            SetTextAndColor(string.Join(" ", NOT_A_NUMBER, "Granularity"));
            return;
        }

        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        AudioError error = am.LerpGroupValue(selectedSoundName, EXPOSED_VOLUME_NAME, endValue, time, granularity);
        ChangeColorOnError(error);
    }

    public void SetStartTimeClicked() {
        ClearText();
        if (!float.TryParse(timeInput.text, out float time)) {
            SetTextAndColor(string.Join(" ", NOT_A_NUMBER, "Time"));
            return;
        }

        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        AudioError error = am.SetStartTime(selectedSoundName, time);
        ChangeColorOnError(error);
    }

    private bool CheckSuccess(AudioError error) {
        return error == AudioError.OK;
    }

    private void ChangeColorOnError(AudioError error) {
        SetColor(CheckSuccess(error));
    }

    private void ClearText() {
        outputText.text = "";
    }

    private void SetTextAndColor(string text) {
        AppendText(text);
        SetColor(false);
    }

    private void AppendText(string text) {
        outputText.text += text;
    }

    private void SetColor(bool success) {
        outputText.color = success ? successColor : failureColor;
        panel.color = success ? successBackgroundColor : failureBackgroundColor;
    }
}

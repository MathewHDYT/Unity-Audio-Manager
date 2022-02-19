using AudioManager.Locator;
using AudioManager.Helper;
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

    [Header("Objects:")]
    [SerializeField]
    private GameObject radio;

    private IAudioManager am;
    private Color32 successColor = new Color32(75, 181, 67, 255);
    private Color32 failureColor = new Color32(237, 67, 55, 255);

    private const string NOT_A_NUMBER = "Input is not a valid number in the textfield: ";
    private const string exposedVolumeName = "Volume";

    private void Start() {
        am = ServiceLocator.GetAudioManager();
    }

    public void PlayClicked() {
        AudioError error = am.Play(soundNameDropDown.options[soundNameDropDown.value].text);
        if (error != AudioError.OK) {
            SetTextAndColor("Playing sound called: " + soundNameDropDown.options[soundNameDropDown.value].text + " failed with error message: " + ErrorToStringConvertor.ErrorToMessage(error), failureColor);
        }
        else {
            SetTextAndColor("Playing sound called: " + soundNameDropDown.options[soundNameDropDown.value].text + " succesfull", successColor);
        }
    }

    public void PlayAtTimeStampClicked() {
        if (!float.TryParse(timeInput.text, out float timeStamp)) {
            SetTextAndColor(NOT_A_NUMBER + "Time", failureColor);
            return;
        }

        AudioError error = am.PlayAtTimeStamp(soundNameDropDown.options[soundNameDropDown.value].text, timeStamp);
        if (error != AudioError.OK) {
            SetTextAndColor("Playing sound called: " + soundNameDropDown.options[soundNameDropDown.value].text + " at timestamp: " + timeStamp.ToString("0.00") + " failed with error message: " + ErrorToStringConvertor.ErrorToMessage(error), failureColor);
        }
        else {
            SetTextAndColor("Playing sound called: " + soundNameDropDown.options[soundNameDropDown.value].text + " at timestamp: " + timeStamp.ToString("0.00") + " succesfull", successColor);
        }
    }

    public void GetPlayBackPositionClicked() {
        ValueDataError<float> valueDataError = am.GetPlaybackPosition(soundNameDropDown.options[soundNameDropDown.value].text);
        if (valueDataError.Error != (int)AudioError.OK) {
            SetTextAndColor("Getting playBackPosition of the sound called: " + soundNameDropDown.options[soundNameDropDown.value].text + " failed with error message: " + ErrorToStringConvertor.ErrorToMessage((AudioError)valueDataError.Error), failureColor);
        }
        else {
            SetTextAndColor("Getting playBackPosition of the sound called: " + soundNameDropDown.options[soundNameDropDown.value].text + " with the position being: " + valueDataError.Value.ToString("0.00") + " succesfull", successColor);
        }
    }

    public void PlayAt3DPositionClicked() {
        float randomXPos = Random.Range(-15f, 15f);
        float randomYPos = Random.Range(-7.5f, 10f);
        var worldPosition = new Vector3(randomXPos, randomYPos, 5f);

        AudioError error = am.PlayAt3DPosition(soundNameDropDown.options[soundNameDropDown.value].text, worldPosition);
        if (error != AudioError.OK) {
            SetTextAndColor("Playing sound called: " + soundNameDropDown.options[soundNameDropDown.value].text + " at the position x " + worldPosition.x.ToString("0.00") + " and y " + worldPosition.y.ToString("0.00") + " failed with error message: " + ErrorToStringConvertor.ErrorToMessage(error), failureColor);
        }
        else {
            SetTextAndColor("Playing sound called: " + soundNameDropDown.options[soundNameDropDown.value].text + " at the position x " + worldPosition.x.ToString("0.00") + " and y " + worldPosition.y.ToString("0.00") + " succesfull", successColor);
        }
    }

    public void PlayAttachedToGameObjectClicked() {
        float randomXPos = Random.Range(-15f, 15f);
        float randomYPos = Random.Range(-7.5f, 10f);
        Vector3 worldPosition = new Vector3(randomXPos, randomYPos, 5f);

        AudioError error = am.PlayAttachedToGameObject(soundNameDropDown.options[soundNameDropDown.value].text, radio);
        if (error != AudioError.OK) {
            SetTextAndColor("Playing sound called: " + soundNameDropDown.options[soundNameDropDown.value].text + " attached to: " + radio.name + " failed with error message: " + ErrorToStringConvertor.ErrorToMessage(error), failureColor);
        }
        else {
            radio.transform.position = worldPosition;
            SetTextAndColor("Playing sound called: " + soundNameDropDown.options[soundNameDropDown.value].text + " attached to: " + radio.name + " succesfull", successColor);
        }
    }

    public void PlayOneShotAt3DPositionClicked() {
        float randomXPos = Random.Range(-15f, 15f);
        float randomYPos = Random.Range(-7.5f, 10f);
        var worldPosition = new Vector3(randomXPos, randomYPos, 5f);

        AudioError error = am.PlayOneShotAt3DPosition(soundNameDropDown.options[soundNameDropDown.value].text, worldPosition);
        if (error != AudioError.OK) {
            SetTextAndColor("Playing sound called: " + soundNameDropDown.options[soundNameDropDown.value].text + " once at the position x " + worldPosition.x.ToString("0.00") + " and y " + worldPosition.y.ToString("0.00") + " failed with error message: " + ErrorToStringConvertor.ErrorToMessage(error), failureColor);
        }
        else {
            SetTextAndColor("Playing sound called: " + soundNameDropDown.options[soundNameDropDown.value].text + " once at the position x " + worldPosition.x.ToString("0.00") + " and y " + worldPosition.y.ToString("0.00") + " succesfull", successColor);
        }
    }

    public void PlayOneShotAttachedToGameObjectClicked() {
        float randomXPos = Random.Range(-15f, 15f);
        float randomYPos = Random.Range(-7.5f, 10f);
        Vector3 worldPosition = new Vector3(randomXPos, randomYPos, 5f);

        AudioError error = am.PlayOneShotAttachedToGameObject(soundNameDropDown.options[soundNameDropDown.value].text, radio);
        if (error != AudioError.OK) {
            SetTextAndColor("Playing sound called: " + soundNameDropDown.options[soundNameDropDown.value].text + " once attached to: " + radio.name + " failed with error message: " + ErrorToStringConvertor.ErrorToMessage(error), failureColor);
        }
        else {
            radio.transform.position = worldPosition;
            SetTextAndColor("Playing sound called: " + soundNameDropDown.options[soundNameDropDown.value].text + " once attached to: " + radio.name + " succesfull", successColor);
        }
    }

    public void PlayDelayedClicked() {
        if (!float.TryParse(timeInput.text, out float delay)) {
            SetTextAndColor(NOT_A_NUMBER + "Time", failureColor);
            return;
        }

        AudioError error = am.PlayDelayed(soundNameDropDown.options[soundNameDropDown.value].text, delay);
        if (error != AudioError.OK) {
            SetTextAndColor("Playing sound called: " + soundNameDropDown.options[soundNameDropDown.value].text + " after " + delay.ToString("0.00") + " seconds failed with error message: " + ErrorToStringConvertor.ErrorToMessage(error), failureColor);
        }
        else {
            SetTextAndColor("Playing sound called: " + soundNameDropDown.options[soundNameDropDown.value].text + " after " + delay.ToString("0.00") + " seconds succesfull", successColor);
        }
    }

    public void PlayOneShotClicked() {
        AudioError error = am.PlayOneShot(soundNameDropDown.options[soundNameDropDown.value].text);
        if (error != AudioError.OK) {
            SetTextAndColor("Playing sound called: " + soundNameDropDown.options[soundNameDropDown.value].text + " once failed with error message: " + ErrorToStringConvertor.ErrorToMessage(error), failureColor);
        }
        else {
            SetTextAndColor("Playing sound called: " + soundNameDropDown.options[soundNameDropDown.value].text + " once succesfull", successColor);
        }
    }

    public void PlayScheduledClicked() {
        if (!double.TryParse(timeInput.text, out double delay)) {
            SetTextAndColor(NOT_A_NUMBER + "Time", failureColor);
            return;
        }

        AudioError error = am.PlayScheduled(soundNameDropDown.options[soundNameDropDown.value].text, delay);
        if (error != AudioError.OK) {
            SetTextAndColor("Playing sound called: " + soundNameDropDown.options[soundNameDropDown.value].text + " after " + delay.ToString("0.00") + " failed with error message: " + ErrorToStringConvertor.ErrorToMessage(error), failureColor);
        }
        else {
            SetTextAndColor("Playing sound called: " + soundNameDropDown.options[soundNameDropDown.value].text + " after " + delay.ToString("0.00") + " seconds succesfull", successColor);
        }
    }

    public void StopClicked() {
        AudioError error = am.Stop(soundNameDropDown.options[soundNameDropDown.value].text);
        if (error != AudioError.OK) {
            SetTextAndColor("Stopping sound called: " + soundNameDropDown.options[soundNameDropDown.value].text + " failed with error message: " + ErrorToStringConvertor.ErrorToMessage(error), failureColor);
        }
        else {
            SetTextAndColor("Stopping sound called: " + soundNameDropDown.options[soundNameDropDown.value].text + " succesfull", successColor);
        }
    }

    public void ToggleMuteClicked() {
        AudioError error = am.ToggleMute(soundNameDropDown.options[soundNameDropDown.value].text);
        if (error != AudioError.OK) {
            SetTextAndColor("Muting or unmuting sound called: " + soundNameDropDown.options[soundNameDropDown.value].text + " failed with error message: " + ErrorToStringConvertor.ErrorToMessage(error), failureColor);
        }
        else {
            SetTextAndColor("Muting or unmuting sound called: " + soundNameDropDown.options[soundNameDropDown.value].text + " succesfull", successColor);
        }
    }

    public void TogglePauseClicked() {
        AudioError error = am.TogglePause(soundNameDropDown.options[soundNameDropDown.value].text);
        if (error != AudioError.OK) {
            SetTextAndColor("Pausing or unpausing sound called: " + soundNameDropDown.options[soundNameDropDown.value].text + " failed with error message: " + ErrorToStringConvertor.ErrorToMessage(error), failureColor);
        }
        else {
            SetTextAndColor("Pausing or unpausing sound called: " + soundNameDropDown.options[soundNameDropDown.value].text + " succesfull", successColor);
        }
    }

    public void GetProgressClicked() {
        ValueDataError<float> valueDataError = am.GetProgress(soundNameDropDown.options[soundNameDropDown.value].text);
        if (valueDataError.Error != (int)AudioError.OK) {
            SetTextAndColor("Getting progress of the sound called: " + soundNameDropDown.options[soundNameDropDown.value].text + " failed with error message: " + ErrorToStringConvertor.ErrorToMessage((AudioError)valueDataError.Error), failureColor);
        }
        else {
            SetTextAndColor("Getting progress of the sound called: " + soundNameDropDown.options[soundNameDropDown.value].text + " with the progress being: " + (valueDataError.Value * 100).ToString("0.00") + "% succesfull", successColor);
        }
    }

    public void GetSourceClicked() {
        AudioError error = am.TryGetSource(soundNameDropDown.options[soundNameDropDown.value].text, out _);
        if (error != AudioError.OK) {
            SetTextAndColor("Getting source of the sound called: " + soundNameDropDown.options[soundNameDropDown.value].text + " failed with error message: " + ErrorToStringConvertor.ErrorToMessage(error), failureColor);
        }
        else {
            SetTextAndColor("Getting source of the sound called: " + soundNameDropDown.options[soundNameDropDown.value].text + " succesfull", successColor);
        }
    }

    public void LerpPitchClicked() {
        if (!float.TryParse(endValueInput.text, out float endValue)) {
            SetTextAndColor(NOT_A_NUMBER + "End Value", failureColor);
            return;
        }
        if (!float.TryParse(timeInput.text, out float time)) {
            SetTextAndColor(NOT_A_NUMBER + "Time", failureColor);
            return;
        }
        if (!float.TryParse(granularityInput.text, out float granularity)) {
            SetTextAndColor(NOT_A_NUMBER + "Granularity", failureColor);
            return;
        }

        AudioError error = am.LerpPitch(soundNameDropDown.options[soundNameDropDown.value].text, endValue, time, granularity);
        if (error != AudioError.OK) {
            SetTextAndColor("Lerping pitch of the sound called: " + soundNameDropDown.options[soundNameDropDown.value].text + " failed with error message: " + ErrorToStringConvertor.ErrorToMessage(error), failureColor);
        }
        else {
            SetTextAndColor("Lerping pitch of the sound called: " + soundNameDropDown.options[soundNameDropDown.value].text + " in the time: " + time.ToString("0.00") + " seconds with the endValue: " + endValue.ToString("0.00") + " and the granularity: " + granularity.ToString("0.00") + " succesfull", successColor);
        }
    }

    public void LerpVolumeClicked() {
        if (!float.TryParse(endValueInput.text, out float endValue)) {
            SetTextAndColor(NOT_A_NUMBER + "End Value", failureColor);
            return;
        }
        if (!float.TryParse(timeInput.text, out float time)) {
            SetTextAndColor(NOT_A_NUMBER + "Time", failureColor);
            return;
        }
        if (!float.TryParse(granularityInput.text, out float granularity)) {
            SetTextAndColor(NOT_A_NUMBER + "Granularity", failureColor);
            return;
        }

        AudioError error = am.LerpVolume(soundNameDropDown.options[soundNameDropDown.value].text, endValue, time, granularity);
        if (error != AudioError.OK) {
            SetTextAndColor("Lerping volume of the sound called: " + soundNameDropDown.options[soundNameDropDown.value].text + " failed with error message: " + ErrorToStringConvertor.ErrorToMessage(error), failureColor);
        }
        else {
            SetTextAndColor("Lerping volume of the sound called: " + soundNameDropDown.options[soundNameDropDown.value].text + " in the time: " + time.ToString("0.00") + " seconds with the endValue: " + endValue.ToString("0.00") + " and the granularity: " + granularity.ToString("0.00") + " succesfull", successColor);
        }
    }

    public void ChangeGroupValueClicked() {
        if (!float.TryParse(endValueInput.text, out float endValue)) {
            SetTextAndColor(NOT_A_NUMBER + "End Value", failureColor);
            return;
        }

        AudioError error = am.ChangeGroupValue(soundNameDropDown.options[soundNameDropDown.value].text, exposedVolumeName, endValue);
        if (error != AudioError.OK) {
            SetTextAndColor("Changing AudioMixerGroup volume of the sound called: " + soundNameDropDown.options[soundNameDropDown.value].text + " failed with error message: " + ErrorToStringConvertor.ErrorToMessage(error), failureColor);
        }
        else {
            SetTextAndColor("Changing AudioMixerGroup volume of the sound called: " + soundNameDropDown.options[soundNameDropDown.value].text + " with the endValue: " + endValue.ToString("0.00") + " succesfull", successColor);
        }
    }

    public void GetGroupValueClicked() {
        ValueDataError<float> valueDataError = am.GetGroupValue(soundNameDropDown.options[soundNameDropDown.value].text, exposedVolumeName);
        if (valueDataError.Error != (int)AudioError.OK) {
            SetTextAndColor("Getting AudioMixerGroup volume of the sound called: " + soundNameDropDown.options[soundNameDropDown.value].text + " failed with error message: " + ErrorToStringConvertor.ErrorToMessage((AudioError)valueDataError.Error), failureColor);
        }
        else {
            SetTextAndColor("Getting AudioMixerGroup volume of the sound called: " + soundNameDropDown.options[soundNameDropDown.value].text + " with the current value being: " + valueDataError.Value.ToString("0.00") + " succesfull", successColor);
        }
    }

    public void ResetGroupValueClicked() {
        AudioError error = am.ResetGroupValue(soundNameDropDown.options[soundNameDropDown.value].text, exposedVolumeName);
        if (error != AudioError.OK) {
            SetTextAndColor("Reseting AudioMixerGroup volume to its default value of the sound called: " + soundNameDropDown.options[soundNameDropDown.value].text + " failed with error message: " + ErrorToStringConvertor.ErrorToMessage(error), failureColor);
        }
        else {
            SetTextAndColor("Reseting AudioMixerGroup volume to its default value of the sound called: " + soundNameDropDown.options[soundNameDropDown.value].text + " succesfull", successColor);
        }
    }

    public void LerpGroupValueClicked() {
        if (!float.TryParse(endValueInput.text, out float endValue)) {
            SetTextAndColor(NOT_A_NUMBER + "End Value", failureColor);
            return;
        }
        if (!float.TryParse(timeInput.text, out float time)) {
            SetTextAndColor(NOT_A_NUMBER + "Time", failureColor);
            return;
        }
        if (!float.TryParse(granularityInput.text, out float granularity)) {
            SetTextAndColor(NOT_A_NUMBER + "Granularity", failureColor);
            return;
        }

        AudioError error = am.LerpGroupValue(soundNameDropDown.options[soundNameDropDown.value].text, exposedVolumeName, endValue, time, granularity);
        if (error != AudioError.OK) {
            SetTextAndColor("Lerping AudioMixerGroup volume of the sound called: " + soundNameDropDown.options[soundNameDropDown.value].text + " failed with error message: " + ErrorToStringConvertor.ErrorToMessage(error), failureColor);
        }
        else {
            SetTextAndColor("Lerping AudioMixerGroup volume of the sound called: " + soundNameDropDown.options[soundNameDropDown.value].text + " in the time: " + time.ToString("0.00") + " seconds with the endValue: " + endValue.ToString("0.00") + " and the granularity: " + granularity.ToString("0.00") + " succesfull", successColor);
        }
    }

    public void SetStartTimeClicked() {
        if (!float.TryParse(timeInput.text, out float time)) {
            SetTextAndColor(NOT_A_NUMBER + "Time", failureColor);
            return;
        }

        AudioError error = am.SetStartTime(soundNameDropDown.options[soundNameDropDown.value].text, time);
        if (error != AudioError.OK) {
            SetTextAndColor("Setting start time of the sound called: " + soundNameDropDown.options[soundNameDropDown.value].text + " failed with error message: " + ErrorToStringConvertor.ErrorToMessage(error), failureColor);
        }
        else {
            SetTextAndColor("Setting start time of the sound called: " + soundNameDropDown.options[soundNameDropDown.value].text + " with the time: " + time.ToString("0.00") + " seconds succesfull", successColor);
        }
    }

    private void SetTextAndColor(string text, Color32 color) {
        outputText.color = color;
        outputText.text = text;
    }
}

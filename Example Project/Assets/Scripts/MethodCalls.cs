using UnityEngine;
using UnityEngine.UI;

public class MethodCalls : MonoBehaviour {

    [Header("Input:")]
    [SerializeField]
    private InputField soundNameInput;
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

    private AudioManager am;
    private Color32 successColor = new Color32(75, 181, 67, 255);
    private Color32 failureColor = new Color32(237, 67, 55, 255);

    private const string NOT_A_NUMBER = "Input is not a valid number in the textfield: ";
    private const string exposedVolumeName = "Volume";

    private void Start() {
        am = AudioManager.instance;
    }

    public void PlayClicked() {
        AudioError error = am.Play(soundNameInput.text);
        if (error != AudioError.OK) {
            SetTextAndColor("Playing sound called: " + soundNameInput.text + " failed with error message: " + ErrorToMessage(error), failureColor);
        }
        else {
            SetTextAndColor("Playing sound called: " + soundNameInput.text + " succesfull", successColor);
        }
    }

    public void PlayAtTimeStampClicked() {
        if (!float.TryParse(timeInput.text, out float timeStamp)) {
            SetTextAndColor(NOT_A_NUMBER + "Time", failureColor);
            return;
        }

        AudioError error = am.PlayAtTimeStamp(soundNameInput.text, timeStamp);
        if (error != AudioError.OK) {
            SetTextAndColor("Playing sound called: " + soundNameInput.text + " at timestamp: " + timeStamp.ToString("0.00") + " failed with error message: " + ErrorToMessage(error), failureColor);
        }
        else {
            SetTextAndColor("Playing sound called: " + soundNameInput.text + " at timestamp: " + timeStamp.ToString("0.00") + " succesfull", successColor);
        }
    }

    public void GetPlayBackPositionClicked() {
        ValueDataError<float> valueDataError = am.GetPlaybackPosition(soundNameInput.text);
        if (valueDataError.Error != (int)AudioError.OK) {
            SetTextAndColor("Getting playBackPosition of the sound called: " + soundNameInput.text + " failed with error message: " + ErrorToMessage((AudioError)valueDataError.Error), failureColor);
        }
        else {
            SetTextAndColor("Getting playBackPosition of the sound called: " + soundNameInput.text + " with the position being: " + valueDataError.Value.ToString("0.00") + " succesfull", successColor);
        }
    }

    public void PlayAt3DPositionClicked() {
        float randomXPos = Random.Range(-15f, 15f);
        float randomYPos = Random.Range(-7.5f, 10f);
        var worldPosition = new Vector3(randomXPos, randomYPos, 5f);

        AudioError error = am.PlayAt3DPosition(soundNameInput.text, worldPosition);
        if (error != AudioError.OK) {
            SetTextAndColor("Playing sound called: " + soundNameInput.text + " at the position x " + worldPosition.x.ToString("0.00") + " and y " + worldPosition.y.ToString("0.00") + " failed with error message: " + ErrorToMessage(error), failureColor);
        }
        else {
            SetTextAndColor("Playing sound called: " + soundNameInput.text + " at the position x " + worldPosition.x.ToString("0.00") + " and y " + worldPosition.y.ToString("0.00") + " succesfull", successColor);
        }
    }

    public void PlayAttachedToGameObjectClicked() {
        float randomXPos = Random.Range(-15f, 15f);
        float randomYPos = Random.Range(-7.5f, 10f);
        Vector3 worldPosition = new Vector3(randomXPos, randomYPos, 5f);

        AudioError error = am.PlayAttachedToGameObject(soundNameInput.text, radio);
        if (error != AudioError.OK) {
            SetTextAndColor("Playing sound called: " + soundNameInput.text + " attached to: " + radio.name + " failed with error message: " + ErrorToMessage(error), failureColor);
        }
        else {
            radio.transform.position = worldPosition;
            SetTextAndColor("Playing sound called: " + soundNameInput.text + " attached to: " + radio.name + " succesfull", successColor);
        }
    }

    public void PlayDelayedClicked() {
        if (!float.TryParse(timeInput.text, out float delay)) {
            SetTextAndColor(NOT_A_NUMBER + "Time", failureColor);
            return;
        }

        AudioError error = am.PlayDelayed(soundNameInput.text, delay);
        if (error != AudioError.OK) {
            SetTextAndColor("Playing sound called: " + soundNameInput.text + " after " + delay.ToString("0.00") + " seconds failed with error message: " + ErrorToMessage(error), failureColor);
        }
        else {
            SetTextAndColor("Playing sound called: " + soundNameInput.text + " after " + delay.ToString("0.00") + " seconds succesfull", successColor);
        }
    }

    public void PlayOneShotClicked() {
        AudioError error = am.PlayOneShot(soundNameInput.text);
        if (error != AudioError.OK) {
            SetTextAndColor("Playing sound called: " + soundNameInput.text + " once failed with error message: " + ErrorToMessage(error), failureColor);
        }
        else {
            SetTextAndColor("Playing sound called: " + soundNameInput.text + " once succesfull", successColor);
        }
    }

    public void PlayScheduledClicked() {
        if (!double.TryParse(timeInput.text, out double delay)) {
            SetTextAndColor(NOT_A_NUMBER + "Time", failureColor);
            return;
        }

        AudioError error = am.PlayScheduled(soundNameInput.text, delay);
        if (error != AudioError.OK) {
            SetTextAndColor("Playing sound called: " + soundNameInput.text + " after " + delay.ToString("0.00") + " failed with error message: " + ErrorToMessage(error), failureColor);
        }
        else {
            SetTextAndColor("Playing sound called: " + soundNameInput.text + " after " + delay.ToString("0.00") + " seconds succesfull", successColor);
        }
    }

    public void StopClicked() {
        AudioError error = am.Stop(soundNameInput.text);
        if (error != AudioError.OK) {
            SetTextAndColor("Stopping sound called: " + soundNameInput.text + " failed with error message: " + ErrorToMessage(error), failureColor);
        }
        else {
            SetTextAndColor("Stopping sound called: " + soundNameInput.text + " succesfull", successColor);
        }
    }

    public void ToggleMuteClicked() {
        AudioError error = am.ToggleMute(soundNameInput.text);
        if (error != AudioError.OK) {
            SetTextAndColor("Muting or unmuting sound called: " + soundNameInput.text + " failed with error message: " + ErrorToMessage(error), failureColor);
        }
        else {
            SetTextAndColor("Muting or unmuting sound called: " + soundNameInput.text + " succesfull", successColor);
        }
    }

    public void TogglePauseClicked() {
        AudioError error = am.TogglePause(soundNameInput.text);
        if (error != AudioError.OK) {
            SetTextAndColor("Pausing or unpausing sound called: " + soundNameInput.text + " failed with error message: " + ErrorToMessage(error), failureColor);
        }
        else {
            SetTextAndColor("Pausing or unpausing sound called: " + soundNameInput.text + " succesfull", successColor);
        }
    }

    public void GetProgressClicked() {
        ValueDataError<float> valueDataError = am.GetProgress(soundNameInput.text);
        if (valueDataError.Error != (int)AudioError.OK) {
            SetTextAndColor("Getting progress of the sound called: " + soundNameInput.text + " failed with error message: " + ErrorToMessage((AudioError)valueDataError.Error), failureColor);
        }
        else {
            SetTextAndColor("Getting progress of the sound called: " + soundNameInput.text + " with the progress being: " + (valueDataError.Value * 100).ToString("0.00") + "% succesfull", successColor);
        }
    }

    public void GetSourceClicked() {
        AudioError error = am.TryGetSource(soundNameInput.text, out _);
        if (error != AudioError.OK) {
            SetTextAndColor("Getting source of the sound called: " + soundNameInput.text + " failed with error message: " + ErrorToMessage(error), failureColor);
        }
        else {
            SetTextAndColor("Getting source of the sound called: " + soundNameInput.text + " succesfull", successColor);
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

        AudioError error = am.LerpPitch(soundNameInput.text, endValue, time, granularity);
        if (error != AudioError.OK) {
            SetTextAndColor("Lerping pitch of the sound called: " + soundNameInput.text + " failed with error message: " + ErrorToMessage(error), failureColor);
        }
        else {
            SetTextAndColor("Lerping pitch of the sound called: " + soundNameInput.text + " in the time: " + time.ToString("0.00") + " seconds with the endValue: " + endValue.ToString("0.00") + " and the granularity: " + granularity.ToString("0.00") + " succesfull", successColor);
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

        AudioError error = am.LerpVolume(soundNameInput.text, endValue, time, granularity);
        if (error != AudioError.OK) {
            SetTextAndColor("Lerping volume of the sound called: " + soundNameInput.text + " failed with error message: " + ErrorToMessage(error), failureColor);
        }
        else {
            SetTextAndColor("Lerping volume of the sound called: " + soundNameInput.text + " in the time: " + time.ToString("0.00") + " seconds with the endValue: " + endValue.ToString("0.00") + " and the granularity: " + granularity.ToString("0.00") + " succesfull", successColor);
        }
    }

    public void ChangeGroupValueClicked() {
        if (!float.TryParse(endValueInput.text, out float endValue)) {
            SetTextAndColor(NOT_A_NUMBER + "End Value", failureColor);
            return;
        }

        AudioError error = am.ChangeGroupValue(soundNameInput.text, exposedVolumeName, endValue);
        if (error != AudioError.OK) {
            SetTextAndColor("Changing AudioMixerGroup volume of the sound called: " + soundNameInput.text + " failed with error message: " + ErrorToMessage(error), failureColor);
        }
        else {
            SetTextAndColor("Changing AudioMixerGroup volume of the sound called: " + soundNameInput.text + " with the endValue: " + endValue.ToString("0.00") + " succesfull", successColor);
        }
    }

    public void GetGroupValueClicked() {
        ValueDataError<float> valueDataError = am.GetGroupValue(soundNameInput.text, exposedVolumeName);
        if (valueDataError.Error != (int)AudioError.OK) {
            SetTextAndColor("Getting AudioMixerGroup volume of the sound called: " + soundNameInput.text + " failed with error message: " + ErrorToMessage((AudioError)valueDataError.Error), failureColor);
        }
        else {
            SetTextAndColor("Getting AudioMixerGroup volume of the sound called: " + soundNameInput.text + " with the current value being: " + valueDataError.Value.ToString("0.00") + " succesfull", successColor);
        }
    }

    public void ResetGroupValueClicked() {
        AudioError error = am.ResetGroupValue(soundNameInput.text, exposedVolumeName);
        if (error != AudioError.OK) {
            SetTextAndColor("Reseting AudioMixerGroup volume to its default value of the sound called: " + soundNameInput.text + " failed with error message: " + ErrorToMessage(error), failureColor);
        }
        else {
            SetTextAndColor("Reseting AudioMixerGroup volume to its default value of the sound called: " + soundNameInput.text + " succesfull", successColor);
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

        AudioError error = am.LerpGroupValue(soundNameInput.text, exposedVolumeName, endValue, time, granularity);
        if (error != AudioError.OK) {
            SetTextAndColor("Lerping AudioMixerGroup volume of the sound called: " + soundNameInput.text + " failed with error message: " + ErrorToMessage(error), failureColor);
        }
        else {
            SetTextAndColor("Lerping AudioMixerGroup volume of the sound called: " + soundNameInput.text + " in the time: " + time.ToString("0.00") + " seconds with the endValue: " + endValue.ToString("0.00") + " and the granularity: " + granularity.ToString("0.00") + " succesfull", successColor);
        }
    }

    public void SetStartTimeClicked() {
        if (!float.TryParse(timeInput.text, out float time)) {
            SetTextAndColor(NOT_A_NUMBER + "Time", failureColor);
            return;
        }

        AudioError error = am.SetStartTime(soundNameInput.text, time);
        if (error != AudioError.OK) {
            SetTextAndColor("Setting start time of the sound called: " + soundNameInput.text + " failed with error message: " + ErrorToMessage(error), failureColor);
        }
        else {
            SetTextAndColor("Setting start time of the sound called: " + soundNameInput.text + " with the time: " + time.ToString("0.00") + " seconds succesfull", successColor);
        }
    }

    private string ErrorToMessage(AudioError error) {
        string message = "";

        switch (error) {
            case AudioError.OK:
                message = "Method succesfully executed";
                break;
            case AudioError.DOES_NOT_EXIST:
                message = "Sound has not been registered with the AudioManager";
                break;
            case AudioError.ALREADY_EXISTS:
                message = "Can't add sound as there already exists a sound with that name";
                break;
            case AudioError.INVALID_PATH:
                message = "Can't add sound because the path does not lead to a valid audio clip";
                break;
            case AudioError.INVALID_END_VALUE:
                message = "The given endValue is already the same as the current value";
                break;
            case AudioError.INVALID_GRANULARITY:
                message = "The given granularity is too small, has to be higher than or equal to 1";
                break;
            case AudioError.INVALID_TIME:
                message = "The given time exceeds the actual length of the clip";
                break;
            case AudioError.INVALID_PROGRESS:
                message = "The given value is to close to the end of the actual clip length, therefore the given value can not be detected, because playing audio is frame rate independent.";
                break;
            case AudioError.MIXER_NOT_EXPOSED:
                message = "The given parameter in the AudioMixer is not exposed or does not exist";
                break;
            case AudioError.MISSING_SOURCE:
                message = "Sound does not have an AudioSource component on the GameObject the AudioManager resides on";
                break;
            case AudioError.MISSING_MIXER_GROUP:
                message = "Group methods may only be called with a sound that has a set AudioMixerGroup";
                break;
            case AudioError.CAN_NOT_BE_3D:
                message = "The sound can not be 3D, because spatialBlend is set to be 2D instead of 3D";
                break;
            default:
                // Invalid AudioError argument.
                break;
        }

        return message;
    }

    private void SetTextAndColor(string text, Color32 color) {
        outputText.color = color;
        outputText.text = text;
    }
}

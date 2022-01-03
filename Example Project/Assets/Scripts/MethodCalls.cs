using UnityEngine;
using TMPro;

public class MethodCalls : MonoBehaviour {

    [Header("Input:")]
    [SerializeField]
    private TMP_InputField soundNameInput;
    [SerializeField]
    private TMP_InputField timeInput;
    [SerializeField]
    private TMP_InputField endValueInput;
    [SerializeField]
    private TMP_InputField granularityInput;

    [Header("Output:")]
    [SerializeField]
    private TMP_Text outputText;

    [Header("Objects:")]
    [SerializeField]
    private GameObject radio;

    private AudioManager am;

    private const string NOT_A_NUMBER = "Input is not a valid number in the textfield: ";
    private const string exposedVolumeName = "Volume";
    private Color32 successColor = new Color32(75, 181, 67, 255);
    private Color32 failureColor = new Color32(237, 67, 55, 255);

    private void Start() {
        am = AudioManager.instance;
    }

    public void PlayClicked() {
        AudioManager.AudioError error = am.Play(soundNameInput.text);
        if (error != AudioManager.AudioError.OK) {
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

        AudioManager.AudioError error = am.PlayAtTimeStamp(soundNameInput.text, timeStamp);
        if (error != AudioManager.AudioError.OK) {
            SetTextAndColor("Playing sound called: " + soundNameInput.text + " at timestamp: " + timeStamp.ToString("0.00") + " failed with error message: " + ErrorToMessage(error), failureColor);
        }
        else {
            SetTextAndColor("Playing sound called: " + soundNameInput.text + " at timestamp: " + timeStamp.ToString("0.00") + " succesfull", successColor);
        }
    }

    public void GetPlayBackPositionClicked() {
        ValueDataError<float> valueDataError = am.GetPlaybackPosition(soundNameInput.text);
        if (valueDataError.Error != (int)AudioManager.AudioError.OK) {
            SetTextAndColor("Getting playBackPosition of the sound called: " + soundNameInput.text + " failed with error message: " + ErrorToMessage((AudioManager.AudioError)valueDataError.Error), failureColor);
        }
        else {
            SetTextAndColor("Getting playBackPosition of the sound called: " + soundNameInput.text + " with the position being: " + valueDataError.Value.ToString("0.00") + " succesfull", successColor);
        }
    }

    public void PlayAt3DPositionClicked() {
        float randomXPos = Random.Range(-15f, 15f);
        float randomYPos = Random.Range(-7.5f, 10f);
        var worldPosition = new Vector3(randomXPos, randomYPos, 5f);

        AudioManager.AudioError error = am.PlayAt3DPosition(soundNameInput.text, worldPosition, 10f, 20f);
        if (error != AudioManager.AudioError.OK) {
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

        AudioManager.AudioError error = am.PlayAttachedToGameObject(soundNameInput.text, radio, 5f, 15f);
        if (error != AudioManager.AudioError.OK) {
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

        AudioManager.AudioError error = am.PlayDelayed(soundNameInput.text, delay);
        if (error != AudioManager.AudioError.OK) {
            SetTextAndColor("Playing sound called: " + soundNameInput.text + " after " + delay.ToString("0.00") + " seconds failed with error message: " + ErrorToMessage(error), failureColor);
        }
        else {
            SetTextAndColor("Playing sound called: " + soundNameInput.text + " after " + delay.ToString("0.00") + " seconds succesfull", successColor);
        }
    }

    public void PlayOneShotClicked() {
        AudioManager.AudioError error = am.PlayOneShot(soundNameInput.text);
        if (error != AudioManager.AudioError.OK) {
            SetTextAndColor("Playing sound called: " + soundNameInput.text + " once failed with error message: " + ErrorToMessage(error), failureColor);
        }
        else {
            SetTextAndColor("Playing sound called: " + soundNameInput.text + " once succesfull", failureColor);
        }
    }

    public void PlayScheduledClicked() {
        if (!double.TryParse(timeInput.text, out double delay)) {
            SetTextAndColor(NOT_A_NUMBER + "Time", failureColor);
            return;
        }

        AudioManager.AudioError error = am.PlayScheduled(soundNameInput.text, delay);
        if (error != AudioManager.AudioError.OK) {
            SetTextAndColor("Playing sound called: " + soundNameInput.text + " after " + delay.ToString("0.00") + " failed with error message: " + ErrorToMessage(error), failureColor);
        }
        else {
            SetTextAndColor("Playing sound called: " + soundNameInput.text + " after " + delay.ToString("0.00") + " seconds succesfull", successColor);
        }
    }

    public void StopClicked() {
        AudioManager.AudioError error = am.Stop(soundNameInput.text);
        if (error != AudioManager.AudioError.OK) {
            SetTextAndColor("Stopping sound called: " + soundNameInput.text + " failed with error message: " + ErrorToMessage(error), failureColor);
        }
        else {
            SetTextAndColor("Stopping sound called: " + soundNameInput.text + " succesfull", successColor);
        }
    }

    public void ToggleMuteClicked() {
        AudioManager.AudioError error = am.ToggleMute(soundNameInput.text);
        if (error != AudioManager.AudioError.OK) {
            SetTextAndColor("Muting or unmuting sound called: " + soundNameInput.text + " failed with error message: " + ErrorToMessage(error), failureColor);
        }
        else {
            SetTextAndColor("Muting or unmuting sound called: " + soundNameInput.text + " succesfull", successColor);
        }
    }

    public void TogglePauseClicked() {
        AudioManager.AudioError error = am.TogglePause(soundNameInput.text);
        if (error != AudioManager.AudioError.OK) {
            SetTextAndColor("Pausing or unpausing sound called: " + soundNameInput.text + " failed with error message: " + ErrorToMessage(error), failureColor);
        }
        else {
            SetTextAndColor("Pausing or unpausing sound called: " + soundNameInput.text + " succesfull", successColor);
        }
    }

    public void GetProgressClicked() {
        ValueDataError<float> valueDataError = am.GetProgress(soundNameInput.text);
        if (valueDataError.Error != (int)AudioManager.AudioError.OK) {
            SetTextAndColor("Getting progress of the sound called: " + soundNameInput.text + " failed with error message: " + ErrorToMessage((AudioManager.AudioError)valueDataError.Error), failureColor);
        }
        else {
            SetTextAndColor("Getting progress of the sound called: " + soundNameInput.text + " with the progress being: " + (valueDataError.Value * 100).ToString("0.00") + "% succesfull", successColor);
        }
    }

    public void GetSourceClicked() {
        AudioManager.AudioError error = am.TryGetSource(soundNameInput.text, out _);
        if (error != AudioManager.AudioError.OK) {
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

        AudioManager.AudioError error = am.LerpPitch(soundNameInput.text, endValue, time, granularity);
        if (error != AudioManager.AudioError.OK) {
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

        AudioManager.AudioError error = am.LerpVolume(soundNameInput.text, endValue, time, granularity);
        if (error != AudioManager.AudioError.OK) {
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

        AudioManager.AudioError error = am.ChangeGroupValue(soundNameInput.text, exposedVolumeName, endValue);
        if (error != AudioManager.AudioError.OK) {
            SetTextAndColor("Changing AudioMixerGroup volume of the sound called: " + soundNameInput.text + " failed with error message: " + ErrorToMessage(error), failureColor);
        }
        else {
            SetTextAndColor("Changing AudioMixerGroup volume of the sound called: " + soundNameInput.text + " with the endValue: " + endValue.ToString("0.00") + " succesfull", successColor);
        }
    }

    public void GetGroupValueClicked() {
        ValueDataError<float> valueDataError = am.GetGroupValue(soundNameInput.text, exposedVolumeName);
        if (valueDataError.Error != (int)AudioManager.AudioError.OK) {
            SetTextAndColor("Getting AudioMixerGroup volume of the sound called: " + soundNameInput.text + " failed with error message: " + ErrorToMessage((AudioManager.AudioError)valueDataError.Error), failureColor);
        }
        else {
            SetTextAndColor("Getting AudioMixerGroup volume of the sound called: " + soundNameInput.text + " with the current value being: " + valueDataError.Value.ToString("0.00") + " succesfull", successColor);
        }
    }

    public void ResetGroupValueClicked() {
        AudioManager.AudioError error = am.ResetGroupValue(soundNameInput.text, exposedVolumeName);
        if (error != AudioManager.AudioError.OK) {
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

        AudioManager.AudioError error = am.LerpGroupValue(soundNameInput.text, exposedVolumeName, endValue, time, granularity);
        if (error != AudioManager.AudioError.OK) {
            SetTextAndColor("Lerping AudioMixerGroup volume of the sound called: " + soundNameInput.text + " failed with error message: " + ErrorToMessage(error), failureColor);
        }
        else {
            SetTextAndColor("Lerping AudioMixerGroup volume of the sound called: " + soundNameInput.text + " in the time: " + time.ToString("0.00") + " seconds with the endValue: " + endValue.ToString("0.00") + " and the granularity: " + granularity.ToString("0.00") + " succesfull", successColor);
        }
    }

    private string ErrorToMessage(AudioManager.AudioError error) {
        string message = "";

        switch (error) {
            case AudioManager.AudioError.OK:
                message = "Method succesfully executed";
                break;
            case AudioManager.AudioError.DOES_NOT_EXIST:
                message = "Sound has not been registered with the AudioManager";
                break;
            case AudioManager.AudioError.FOUND_MULTIPLE:
                message = "Multiple instances with the same name found. First will be played";
                break;
            case AudioManager.AudioError.ALREADY_EXISTS:
                message = "Can't add sound as there already exists a sound with that name";
                break;
            case AudioManager.AudioError.INVALID_PATH:
                message = "Can't add sound because the path does not lead to a valid audio clip";
                break;
            case AudioManager.AudioError.SAME_AS_CURRENT:
                message = "The given endValue is already the same as the current value";
                break;
            case AudioManager.AudioError.TOO_SMALL:
                message = "The given granularity is too small, has to be higher than or equal to 1";
                break;
            case AudioManager.AudioError.NOT_EXPOSED:
                message = "The given parameter in the AudioMixer is not exposed or does not exist.";
                break;
            case AudioManager.AudioError.MISSING_SOURCE:
                message = "Sound does not have an AudioSource component on the GameObject the AudioManager resides on.";
                break;
            case AudioManager.AudioError.MISSING_MIXER_GROUP:
                message = "Group methods may only be called with a sound that has a set AudioMixerGroup.";
                break;
            default:
                // Invalid AudioManager.AudioError argument.
                break;
        }

        return message;
    }

    private void SetTextAndColor(string text, Color32 color) {
        outputText.color = color;
        outputText.text = text;
    }
}

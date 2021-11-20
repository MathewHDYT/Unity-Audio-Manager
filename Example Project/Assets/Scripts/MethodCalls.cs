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

    private const string NOT_A_NUMBER = "Input is not a valid number in the textfield: ";

    private void Start() {
        am = AudioManager.instance;
    }

    public void PlayClicked() {
        AudioManager.AudioError err = am.Play(soundNameInput.text);
        if (err != AudioManager.AudioError.OK) {
            SetTextAndColor("Playing sound called: " + soundNameInput.text + " failed with error message: " + ErrorToMessage(err), Color.red);
        }
        else {
            SetTextAndColor("Playing sound called: " + soundNameInput.text + " succesfull", Color.green);
        }
    }

    public void PlayAtTimeStampClicked() {
        if (!float.TryParse(timeInput.text, out float timeStamp)) {
            SetTextAndColor(NOT_A_NUMBER + "Time", Color.red);
            return;
        }

        AudioManager.AudioError err = am.PlayAtTimeStamp(soundNameInput.text, timeStamp);
        if (err != AudioManager.AudioError.OK) {
            SetTextAndColor("Playing sound called: " + soundNameInput.text + " at timestamp: " + timeStamp.ToString("0.00") + " failed with error message: " + ErrorToMessage(err), Color.red);
        }
        else {
            SetTextAndColor("Playing sound called: " + soundNameInput.text + " at timestamp: " + timeStamp.ToString("0.00") + " succesfull", Color.green);
        }
    }

    public void GetPlayBackPositionClicked() {
        ValueDataError<float> valueDataError = am.GetPlaybackPosition(soundNameInput.text);
        if (valueDataError.Error != (int)AudioManager.AudioError.OK) {
            SetTextAndColor("Getting playBackPosition of the sound called: " + soundNameInput.text + " failed with error message: " + ErrorToMessage((AudioManager.AudioError)valueDataError.Error), Color.red);
        }
        else {
            SetTextAndColor("Getting playBackPosition of the sound called: " + soundNameInput.text + " with the position being: " + valueDataError.Value.ToString("0.00") + " succesfull", Color.green);
        }
    }

    public void PlayAt3DPositionClicked() {
        float randomXPos = Random.Range(-15f, 15f);
        float randomYPos = Random.Range(-7.5f, 10f);
        var worldPosition = new Vector3(randomXPos, randomYPos, 5f);

        AudioManager.AudioError err = am.PlayAt3DPosition(soundNameInput.text, worldPosition, 10f, 20f);
        if (err != AudioManager.AudioError.OK) {
            SetTextAndColor("Playing sound called: " + soundNameInput.text + " at the position x " + worldPosition.x.ToString("0.00") + " and y " + worldPosition.y.ToString("0.00") + " failed with error message: " + ErrorToMessage(err), Color.red);
        }
        else {
            SetTextAndColor("Playing sound called: " + soundNameInput.text + " at the position x " + worldPosition.x.ToString("0.00") + " and y " + worldPosition.y.ToString("0.00") + " succesfull", Color.green);
        }
    }

    public void PlayAttachedToGameObjectClicked() {
        float randomXPos = Random.Range(-15f, 15f);
        float randomYPos = Random.Range(-7.5f, 10f);
        Vector3 worldPosition = new Vector3(randomXPos, randomYPos, 5f);

        AudioManager.AudioError err = am.PlayAttachedToGameObject(soundNameInput.text, radio, 5f, 15f);
        if (err != AudioManager.AudioError.OK) {
            SetTextAndColor("Playing sound called: " + soundNameInput.text + " attached to: " + radio.name + " failed with error message: " + ErrorToMessage(err), Color.red);
        }
        else {
            radio.transform.position = worldPosition;
            SetTextAndColor("Playing sound called: " + soundNameInput.text + " attached to: " + radio.name + " succesfull", Color.green);
        }
    }

    public void PlayDelayedClicked() {
        if (!float.TryParse(timeInput.text, out float delay)) {
            SetTextAndColor(NOT_A_NUMBER + "Time", Color.red);
            return;
        }

        AudioManager.AudioError err = am.PlayDelayed(soundNameInput.text, delay);
        if (err != AudioManager.AudioError.OK) {
            SetTextAndColor("Playing sound called: " + soundNameInput.text + " after " + delay.ToString("0.00") + " seconds failed with error message: " + ErrorToMessage(err), Color.red);
        }
        else {
            SetTextAndColor("Playing sound called: " + soundNameInput.text + " after " + delay.ToString("0.00") + " seconds succesfull", Color.green);
        }
    }

    public void PlayOneShotClicked() {
        AudioManager.AudioError err = am.PlayOneShot(soundNameInput.text);
        if (err != AudioManager.AudioError.OK) {
            SetTextAndColor("Playing sound called: " + soundNameInput.text + " once failed with error message: " + ErrorToMessage(err), Color.red);
        }
        else {
            SetTextAndColor("Playing sound called: " + soundNameInput.text + " once succesfull", Color.red);
        }
    }

    public void PlayScheduledClicked() {
        if (!double.TryParse(timeInput.text, out double delay)) {
            SetTextAndColor(NOT_A_NUMBER + "Time", Color.red);
            return;
        }

        AudioManager.AudioError err = am.PlayScheduled(soundNameInput.text, delay);
        if (err != AudioManager.AudioError.OK) {
            SetTextAndColor("Playing sound called: " + soundNameInput.text + " after " + delay.ToString("0.00") + " failed with error message: " + ErrorToMessage(err), Color.red);
        }
        else {
            SetTextAndColor("Playing sound called: " + soundNameInput.text + " after " + delay.ToString("0.00") + " seconds succesfull", Color.green);
        }
    }

    public void StopClicked() {
        AudioManager.AudioError err = am.Stop(soundNameInput.text);
        if (err != AudioManager.AudioError.OK) {
            SetTextAndColor("Stopping sound called: " + soundNameInput.text + " failed with error message: " + ErrorToMessage(err), Color.red);
        }
        else {
            SetTextAndColor("Stopping sound called: " + soundNameInput.text + " succesfull", Color.green);
        }
    }

    public void ToggleMuteClicked() {
        AudioManager.AudioError err = am.ToggleMute(soundNameInput.text);
        if (err != AudioManager.AudioError.OK) {
            SetTextAndColor("Muting or unmuting sound called: " + soundNameInput.text + " failed with error message: " + ErrorToMessage(err), Color.red);
        }
        else {
            SetTextAndColor("Muting or unmuting sound called: " + soundNameInput.text + " succesfull", Color.green);
        }
    }

    public void TogglePauseClicked() {
        AudioManager.AudioError err = am.TogglePause(soundNameInput.text);
        if (err != AudioManager.AudioError.OK) {
            SetTextAndColor("Pausing or unpausing sound called: " + soundNameInput.text + " failed with error message: " + ErrorToMessage(err), Color.red);
        }
        else {
            SetTextAndColor("Pausing or unpausing sound called: " + soundNameInput.text + " succesfull", Color.green);
        }
    }

    public void GetProgressClicked() {
        ValueDataError<float> valueDataError = am.GetProgress(soundNameInput.text);
        if (valueDataError.Error != (int)AudioManager.AudioError.OK) {
            SetTextAndColor("Getting progress of the sound called: " + soundNameInput.text + " failed with error message: " + ErrorToMessage((AudioManager.AudioError)valueDataError.Error), Color.red);
        }
        else {
            SetTextAndColor("Getting progress of the sound called: " + soundNameInput.text + " with the progress being: " + (valueDataError.Value * 100).ToString("0.00") + "% succesfull", Color.green);
        }
    }

    public void GetSourceClicked() {
        AudioManager.AudioError err = am.TryGetSource(soundNameInput.text, out _);
        if (err != AudioManager.AudioError.OK) {
            SetTextAndColor("Getting source of the sound called: " + soundNameInput.text + " failed with error message: " + ErrorToMessage(err), Color.red);
        }
        else {
            SetTextAndColor("Getting source of the sound called: " + soundNameInput.text + " succesfull", Color.green);
        }
    }

    public void ChangePitchClicked() {
        if (!float.TryParse(endValueInput.text, out float endValue)) {
            SetTextAndColor(NOT_A_NUMBER + "End Value", Color.red);
            return;
        }
        if (!float.TryParse(timeInput.text, out float time)) {
            SetTextAndColor(NOT_A_NUMBER + "Time", Color.red);
            return;
        }
        if (!float.TryParse(granularityInput.text, out float granularity)) {
            SetTextAndColor(NOT_A_NUMBER + "Granularity", Color.red);
            return;
        }

        AudioManager.AudioError err = am.ChangePitch(soundNameInput.text, endValue, time, granularity);
        if (err != AudioManager.AudioError.OK) {
            SetTextAndColor("Changing pitch of the sound called: " + soundNameInput.text + " failed with error message: " + ErrorToMessage(err), Color.red);
        }
        else {
            SetTextAndColor("Changing pitch of the sound called: " + soundNameInput.text + " in the time: " + time.ToString("0.00") + "seconds with the endValue: " + endValue.ToString("0.00") + " and the granularity: " + granularity.ToString("0.00") + " succesfull", Color.green);
        }
    }

    public void ChangeVolumeClicked() {
        if (!float.TryParse(endValueInput.text, out float endValue)) {
            SetTextAndColor(NOT_A_NUMBER + "End Value", Color.red);
            return;
        }
        if (!float.TryParse(timeInput.text, out float time)) {
            SetTextAndColor(NOT_A_NUMBER + "Time", Color.red);
            return;
        }
        if (!float.TryParse(granularityInput.text, out float granularity)) {
            SetTextAndColor(NOT_A_NUMBER + "Granularity", Color.red);
            return;
        }

        AudioManager.AudioError err = am.ChangeVolume(soundNameInput.text, endValue, time, granularity);
        if (err != AudioManager.AudioError.OK) {
            SetTextAndColor("Changing volume of the sound called: " + soundNameInput.text + " failed with error message: " + ErrorToMessage(err), Color.red);
        }
        else {
            SetTextAndColor("Changing volume of the sound called: " + soundNameInput.text + "in the time: " + time.ToString("0.00") + "seconds with the endValue: " + endValue.ToString("0.00") + " and the granularity: " + granularity.ToString("0.00") + " succesfull", Color.green);
        }
    }

    private string ErrorToMessage(AudioManager.AudioError err) {
        string message = "";

        switch (err) {
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
            default:
                // Invalid AudioManager.AudioError argument.
                break;
        }

        return message;
    }

    private void SetTextAndColor(string text, Color color) {
        outputText.text = text;
        outputText.color = color;
    }
}

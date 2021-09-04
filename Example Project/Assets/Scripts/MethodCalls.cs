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
    private Text errorField;

    [Header("Objects:")]
    [SerializeField]
    private GameObject radio;

    private AudioManager am;

    private const string MISSING_INPUT = "Missing input in the textfield: ";
    private const string WRONG_INPUT = "Wrong or nonexistent input given the in texfield: ";
    private const string NOT_A_NUMBER = "Input is not a valid number in the textfield: ";
    private const string SUCCESS = "Succesfully executed the function with the given values for the method: ";

    private void Start() {
        am = AudioManager.instance;
    }

    public void ResetAllInputFieldColors(string name) {
        switch (name) {
            case "SoundName":
                ResetInputFieldColor(soundNameInput);
                break;
            case "Time":
                ResetInputFieldColor(timeInput);
                break;
            case "EndValue":
                ResetInputFieldColor(endValueInput);
                break;
            case "Granularity":
                ResetInputFieldColor(granularityInput);
                break;
            default:
                break;
        }
    }

    public void PlayAtTimeStampClicked() {
        string methodName = "PlayAtTimeStamp";
        string tempSoundName = "";
        float tempTime = 0f;

        if (GetSoundName(ref tempSoundName) && GetTime(ref tempTime)) {
            am.PlayAtTimeStamp(tempSoundName, tempTime);
            SetTextColor(Color.green);
            errorField.text = SUCCESS + methodName;
        }
    }

    public void GetPlayBackPositionClicked() {
        string methodName = "GetPlayBackPosition";
        string tempSoundName = "";

        if (GetSoundName(ref tempSoundName)) {
            // Get number of seconds in float and round it to 2 decimal places.
            string timeStamp = am.GetPlaybackPosition(tempSoundName).ToString("0.00");
            SetTextColor(Color.green);
            errorField.text = SUCCESS + methodName + " with the output " + timeStamp + " seconds";
        }
    }

    public void PlayAt3DPositionClicked() {
        string methodName = "PlayAt3DPosition";
        string tempSoundName = "";
        float randomXPos = Random.Range(-15f, 15f);
        float randomYPos = Random.Range(-7.5f, 10f);

        if (GetSoundName(ref tempSoundName)) {
            am.PlayAt3DPosition(tempSoundName, new Vector3(randomXPos, randomYPos, 5f), 10f, 20f);
            SetTextColor(Color.green);
            errorField.text = SUCCESS + methodName + " at the position x " + randomXPos.ToString("0.00") + " and y " + randomYPos.ToString("0.00");
        }
    }

    public void PlayAttachedToGameObjectClicked() {
        string methodName = "PlayAttachedToGameObject";
        string tempSoundName = "";
        float randomXPos = Random.Range(-15f, 15f);
        float randomYPos = Random.Range(-7.5f, 10f);

        if (GetSoundName(ref tempSoundName)) {
            radio.transform.position = new Vector3(randomXPos, randomYPos, 5f);
            am.PlayAttachedToGameObject(tempSoundName, radio, 10f, 20f);
            SetTextColor(Color.green);
            errorField.text = SUCCESS + methodName + " attached to the cube";
        }
    }

    public void PlayDelayedClicked() {
        string methodName = "PlayDelayed";
        string tempSoundName = "";
        float tempTime = 0f;

        if (GetSoundName(ref tempSoundName) && GetTime(ref tempTime)) {
            am.PlayDelayed(tempSoundName, tempTime);
            SetTextColor(Color.green);
            errorField.text = SUCCESS + methodName + " playing in " + tempTime.ToString("0.00") + " seconds";
        }
    }

    public void PlayOneShotClicked() {
        string methodName = "PlayOneShot";
        string tempSoundName = "";

        if (GetSoundName(ref tempSoundName)) {
            am.PlayOneShot(tempSoundName);
            SetTextColor(Color.green);
            errorField.text = SUCCESS + methodName;
        }
    }

    public void PlayScheduledClicked() {
        string methodName = "PlayScheduled";
        string tempSoundName = "";
        float tempTime = 0f;

        if (GetSoundName(ref tempSoundName) && GetTime(ref tempTime)) {
            am.PlayDelayed(tempSoundName, tempTime);
            SetTextColor(Color.green);
            errorField.text = SUCCESS + methodName + " playing at " + tempTime.ToString("0.00") + " seconds in our time line";
        }
    }

    public void StopClicked() {
        string methodName = "Stop";
        string tempSoundName = "";

        if (GetSoundName(ref tempSoundName)) {
            am.Stop(tempSoundName);
            SetTextColor(Color.green);
            errorField.text = SUCCESS + methodName;
        }
    }

    public void ToggleMuteClicked() {
        string methodName = "ToggleMute";
        string tempSoundName = "";

        if (GetSoundName(ref tempSoundName)) {
            am.ToggleMute(tempSoundName);
            SetTextColor(Color.green);
            errorField.text = SUCCESS + methodName;
        }
    }

    public void GetSourceClicked() {
        string methodName = "GetSource";
        string tempSoundName = "";

        if (GetSoundName(ref tempSoundName)) {
            SetTextColor(Color.green);
            errorField.text = SUCCESS + methodName;
        }
    }

    public void ChangePitchClicked() {
        string methodName = "ChangePitch";
        string tempSoundName = "";
        float tempEndValue = 0f;
        float tempTime = 1f;
        float tempGranularity = 5f;

        if (GetSoundName(ref tempSoundName) && GetTime(ref tempTime) && GetEndValue(ref tempEndValue) && GetGranularity(ref tempGranularity)) {
            am.ChangePitch(tempSoundName, tempEndValue, tempTime, tempGranularity);
            SetTextColor(Color.green);
            errorField.text = SUCCESS + methodName + " in the time " + tempTime.ToString("0.00") + " with the endValue " + tempEndValue.ToString("0.00") + " and the stepValue " + tempGranularity.ToString("0.00");
        }
    }

    public void ChangeVolumeClicked() {
        string methodName = "ChangeVolume";
        string tempSoundName = "";
        float tempEndValue = 0f;
        float tempTime = 1f;
        float tempGranularity = 5f;

        if (GetSoundName(ref tempSoundName) && GetTime(ref tempTime) && GetEndValue(ref tempEndValue) && GetGranularity(ref tempGranularity)) {
            am.ChangeVolume(tempSoundName, tempEndValue, tempTime, tempGranularity);
            SetTextColor(Color.green);
            errorField.text = SUCCESS + methodName + " in the time " + tempTime.ToString("0.00") + " with the endValue " + tempEndValue.ToString("0.00") + " and the granularity " + tempGranularity.ToString("0.00");
        }
    }

    public void PlayClicked() {
        string methodName = "Play";
        string tempSoundName = "";

        if (GetSoundName(ref tempSoundName)) {
            am.Play(tempSoundName);
            SetTextColor(Color.green);
            errorField.text = SUCCESS + methodName;
        }
    }

    public void GetProgressClicked() {
        string methodName = "Progress";
        string tempSoundName = "";

        if (GetSoundName(ref tempSoundName)) {
            // Get current progress as float from 0 to 1.
            float progress = am.GetProgress(tempSoundName);
            SetTextColor(Color.green);
            errorField.text = SUCCESS + methodName + " with the output " + (progress * 100).ToString("0.00") + " % of the sound completed";
        }
    }

    private bool GetSoundName(ref string soundName) {
        string inputField = "Sound Name";

        // Check if we received any input.
        if (string.IsNullOrWhiteSpace(soundNameInput.textComponent.text)) {
            SetTextColor(Color.red);
            SetInputFieldColor(soundNameInput, Color.red);
            errorField.text = MISSING_INPUT + inputField;
            return false;
        }

        // Check if the input is equal to an actual sound.
        AudioSource source = am.GetSource(soundNameInput.textComponent.text);
        if (source == default) {
            SetTextColor(Color.red);
            SetInputFieldColor(soundNameInput, Color.red);
            errorField.text = WRONG_INPUT + inputField;
            return false;
        }

        soundName = soundNameInput.text;
        return true;
    }

    private bool GetTime(ref float timeStamp) {
        string inputField = "Time";

        // Check if we received any input.
        if (string.IsNullOrWhiteSpace(timeInput.textComponent.text)) {
            SetTextColor(Color.red);
            SetInputFieldColor(timeInput, Color.red);
            errorField.text = MISSING_INPUT + inputField;
            return false;
        }

        // Check if input is a valid number.
        bool success = float.TryParse(timeInput.textComponent.text, out timeStamp);
        if (!success) {
            SetTextColor(Color.red);
            SetInputFieldColor(timeInput, Color.red);
            errorField.text = NOT_A_NUMBER + inputField;
            return false;
        }

        return true;
    }

    private bool GetEndValue(ref float endValue) {
        string inputField = "End Value";

        // Check if we received any input.
        if (string.IsNullOrWhiteSpace(endValueInput.textComponent.text)) {
            SetTextColor(Color.red);
            SetInputFieldColor(endValueInput, Color.red);
            errorField.text = MISSING_INPUT + inputField;
            return false;
        }

        // Check if input is a valid number.
        if (!float.TryParse(endValueInput.textComponent.text, out endValue)) {
            SetTextColor(Color.red);
            SetInputFieldColor(endValueInput, Color.red);
            errorField.text = NOT_A_NUMBER + inputField;
            return false;
        }

        return true;
    }

    private bool GetGranularity(ref float granularity) {
        string inputField = "Granularity";

        // Check if we received any input.
        if (string.IsNullOrWhiteSpace(granularityInput.textComponent.text)) {
            SetTextColor(Color.red);
            SetInputFieldColor(granularityInput, Color.red);
            errorField.text = MISSING_INPUT + inputField;
            return false;
        }

        // Check if input is a valid number.
        if (!float.TryParse(granularityInput.textComponent.text, out granularity)) {
            SetTextColor(Color.red);
            SetInputFieldColor(granularityInput, Color.red);
            errorField.text = NOT_A_NUMBER + inputField;
            return false;
        }

        return true;
    }

    private void SetTextColor(Color color) {
        errorField.color = color;
    }

    private void SetInputFieldColor(InputField inputField, Color color) {
        ColorBlock colorVar = inputField.colors;

        // Set RGB values of the inputFields color normally.
        colorVar.normalColor = color;
        inputField.colors = colorVar;

        // Set RGB values of the inputFields color when it's hovered over.
        colorVar.highlightedColor = color;
        inputField.colors = colorVar;
    }

    private void ResetInputFieldColor(InputField inputField) {
        ColorBlock colorVar = inputField.colors;

        // Reset RGB values of the inputFields color normally.
        colorVar.normalColor = new Color(255f, 255f, 255f);
        inputField.colors = colorVar;

        // Reset RGB values of the inputFields color when it's hovered over.
        colorVar.highlightedColor = new Color(245f, 245f, 245f);
        inputField.colors = colorVar;
    }
}

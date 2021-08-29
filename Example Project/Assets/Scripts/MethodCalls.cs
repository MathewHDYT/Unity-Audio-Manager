using UnityEngine;
using UnityEngine.UI;

public class MethodCalls : MonoBehaviour {

    [Header("Input:")]
    [SerializeField]
    private Text soundNameInput;
    [SerializeField]
    private Text timeInput;
    [SerializeField]
    private Text endValueInput;
    [SerializeField]
    private Text stepValueInput;

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
        float tempStepValue = 0f;
        float tempTime = 0f;

        if (GetSoundName(ref tempSoundName) && GetTime(ref tempTime) && GetEndAndStepValue(ref tempEndValue, ref tempStepValue)) {
            am.ChangePitch(tempSoundName, tempEndValue, tempStepValue, tempTime);
            SetTextColor(Color.green);
            errorField.text = SUCCESS + methodName + " in the time " + tempTime.ToString("0.00") + " with the endValue " + tempEndValue.ToString("0.00") + " and the stepValue " + tempStepValue.ToString("0.00");
        }
    }

    public void ChangeVolumeClicked() {
        string methodName = "ChangeVolume";
        string tempSoundName = "";
        float endValue = 0f;
        float stepValue = 0f;
        float tempTime = 0f;

        if (GetSoundName(ref tempSoundName) && GetTime(ref tempTime) && GetEndAndStepValue(ref endValue, ref stepValue)) {
            am.ChangeVolume(tempSoundName, endValue, stepValue, tempTime);
            SetTextColor(Color.green);
            errorField.text = SUCCESS + methodName + " in the time " + tempTime.ToString("0.00") + " with the endValue " + endValue.ToString("0.00") + " and the stepValue " + stepValue.ToString("0.00");
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

    private bool GetSoundName(ref string soundName) {
        string inputField = "Sound Name";

        // Check if we received any input.
        if (string.IsNullOrWhiteSpace(soundNameInput.text)) {
            SetTextColor(Color.red);
            errorField.text = MISSING_INPUT + inputField;
            return false;
        }

        // Check if the input is equal to an actual sound.
        AudioSource source = am.GetSource(soundNameInput.text);
        if (source == default) {
            SetTextColor(Color.red);
            errorField.text = WRONG_INPUT + inputField;
            return false;
        }

        soundName = soundNameInput.text;
        return true;
    }

    private bool GetTime(ref float timeStamp) {
        string inputField = "Time";

        // Check if we received any input.
        if (string.IsNullOrWhiteSpace(timeInput.text)) {
            SetTextColor(Color.red);
            errorField.text = MISSING_INPUT + inputField;
            return false;
        }

        // Check if input is a valid number.
        bool success = float.TryParse(timeInput.text, out timeStamp);
        if (!success) {
            SetTextColor(Color.red);
            errorField.text = NOT_A_NUMBER + inputField;
            return false;
        }

        return true;
    }

    private bool GetEndAndStepValue(ref float endValue, ref float stepValue) {
        string inputField = "End Value";
        string inputField2 = "Step Value";

        // Check if we received any input.
        if (string.IsNullOrWhiteSpace(endValueInput.text)) {
            SetTextColor(Color.red);
            errorField.text = MISSING_INPUT + inputField;
            return false;
        }
        else if (string.IsNullOrWhiteSpace(stepValueInput.text)) {
            SetTextColor(Color.red);
            errorField.text = MISSING_INPUT + inputField2;
            return false;
        }

        // Check if input is a valid number.
        bool success = float.TryParse(endValueInput.text, out endValue);
        if (!success) {
            SetTextColor(Color.red);
            errorField.text = NOT_A_NUMBER + inputField;
            return false;
        }
        success = float.TryParse(stepValueInput.text, out stepValue);
        if (!success) {
            SetTextColor(Color.red);
            errorField.text = NOT_A_NUMBER + inputField2;
            return false;
        }

        return true;
    }

    private void SetTextColor(Color color) {
        errorField.color = color;
    }
}

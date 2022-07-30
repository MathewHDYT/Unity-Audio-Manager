using AudioManager.Core;
using AudioManager.Locator;
using AudioManager.Logger;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
#if !UNITY_WEBGL
using System.IO;
#endif // UNITY_WEBGL

public class MethodCalls : MonoBehaviour {
    [Header("Input:")]
    [SerializeField]
    private Dropdown soundNameDropDown;
    [SerializeField]
    private Dropdown childDropDown;
    [SerializeField]
    private InputField timeInput;
    [SerializeField]
    private InputField endValueInput;
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
        SetDropDownOptions();
        // Initally enable first tab.
        SwitchTab(0);
    }

    private void SetDropDownOptions() {
        var children = System.Enum.GetNames(typeof(ChildType)).ToList();
        childDropDown.AddOptions(children);
        childDropDown.value = (int)ChildType.PARENT;
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
#endif // !UNITY_WEBGL
    }

    public void PlayClicked() {
        ClearText();
        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        var selectedChild = (ChildType)childDropDown.value;
        am.Play(selectedSoundName, selectedChild);
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

        foreach (var name in am.GetEnumerator()) {
            am.LerpVolume(name, endValue, time);
        }
    }

    public void SkipTimeClicked() {
        ClearText();
        if (!float.TryParse(timeInput.text, out float timeStamp)) {
            SetText(string.Join(" ", NOT_A_NUMBER, "Time"));
            return;
        }

        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        var selectedChild = (ChildType)childDropDown.value;
        am.SkipTime(selectedSoundName, timeStamp, selectedChild);
    }

    public void SetPlaybackDirection() {
        ClearText();
        if (!float.TryParse(minPitchInput.text, out float minPitch)) {
            SetText(string.Join(" ", NOT_A_NUMBER, "Min Pitch"));
            return;
        }

        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        var selectedChild = (ChildType)childDropDown.value;
        am.SetPlaybackDirection(selectedSoundName, minPitch, selectedChild);
    }

    public void PlayAtTimeStampClicked() {
        ClearText();
        if (!float.TryParse(timeInput.text, out float timeStamp)) {
            SetText(string.Join(" ", NOT_A_NUMBER, "Time"));
            return;
        }

        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        var selectedChild = (ChildType)childDropDown.value;
        am.PlayAtTimeStamp(selectedSoundName, timeStamp, selectedChild);
    }

    public void GetPlayBackPositionClicked() {
        ClearText();
        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        var selectedChild = (ChildType)childDropDown.value;
        AudioError error = am.GetPlaybackPosition(selectedSoundName, out float time, selectedChild);

        if (CheckSuccess(error)) {
            AppendText(string.Join(" ", "Current playback position being:", time.ToString("0.00"), "seconds"));
        }
    }

    public void RegisterChildAt3DPosClicked() {
        ClearText();
        float randomXPos = Random.Range(-15f, 15f);
        float randomYPos = Random.Range(-7.5f, 10f);
        var worldPosition = new Vector3(randomXPos, randomYPos, 5f);

        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        am.RegisterChildAt3DPos(selectedSoundName, worldPosition, out _);
    }

    public void RegisterAttachedToGoClicked() {
        ClearText();
        float randomXPos = Random.Range(-15f, 15f);
        float randomYPos = Random.Range(-7.5f, 10f);
        var worldPosition = new Vector3(randomXPos, randomYPos, 5f);

        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        am.RegisterChildAttachedToGo(selectedSoundName, radio, out _);
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
        var selectedChild = (ChildType)childDropDown.value;
        am.ChangePitch(selectedSoundName, minPitch, maxPitch, selectedChild);
    }

    public void PlayDelayedClicked() {
        ClearText();
        if (!float.TryParse(timeInput.text, out float delay)) {
            SetText(string.Join(" ", NOT_A_NUMBER, "Time"));
            return;
        }

        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        var selectedChild = (ChildType)childDropDown.value;
        am.PlayDelayed(selectedSoundName, delay, selectedChild);
    }

    public void PlayOneShotClicked() {
        ClearText();
        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        var selectedChild = (ChildType)childDropDown.value;
        am.PlayOneShot(selectedSoundName, selectedChild);
    }

    public void PlayScheduledClicked() {
        ClearText();
        if (!double.TryParse(timeInput.text, out double delay)) {
            SetText(string.Join(" ", NOT_A_NUMBER, "Time"));
            return;
        }

        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        var selectedChild = (ChildType)childDropDown.value;
        am.PlayScheduled(selectedSoundName, delay, selectedChild);
    }

    public void StopClicked() {
        ClearText();
        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        var selectedChild = (ChildType)childDropDown.value;
        am.Stop(selectedSoundName, selectedChild);
    }

    public void ToggleMuteClicked() {
        ClearText();
        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        var selectedChild = (ChildType)childDropDown.value;
        am.ToggleMute(selectedSoundName, selectedChild);
    }

    public void TogglePauseClicked() {
        ClearText();
        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        var selectedChild = (ChildType)childDropDown.value;
        am.TogglePause(selectedSoundName, selectedChild);
    }

    public void GetProgressClicked() {
        ClearText();
        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        var selectedChild = (ChildType)childDropDown.value;
        AudioError error = am.GetProgress(selectedSoundName, out float progress, selectedChild);

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

        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        var selectedChild = (ChildType)childDropDown.value;
        am.LerpPitch(selectedSoundName, endValue, time, selectedChild);
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

        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        var selectedChild = (ChildType)childDropDown.value;
        am.LerpVolume(selectedSoundName, endValue, time, selectedChild);
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

        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        am.LerpGroupValue(selectedSoundName, EXPOSED_VOLUME_NAME, endValue, time);
    }

    public void SetStartTimeClicked() {
        ClearText();
        if (!float.TryParse(timeInput.text, out float time)) {
            SetText(string.Join(" ", NOT_A_NUMBER, "Time"));
            return;
        }

        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        var selectedChild = (ChildType)childDropDown.value;
        am.SetStartTime(selectedSoundName, time, selectedChild);
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

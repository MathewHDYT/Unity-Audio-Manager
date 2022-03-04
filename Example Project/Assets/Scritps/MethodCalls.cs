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
    private Color32 m_successColor = new Color32(75, 181, 67, 255);
    private Color32 m_failureColor = new Color32(237, 67, 55, 255);

    private const string NOT_A_NUMBER = "Input is not a valid number in the textfield:";
    private const string EXPOSED_VOLUME_NAME = "Volume";

    private void Start() {
        am = ServiceLocator.GetService();
    }

    public void PlayClicked() {
        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        AudioError error = am.Play(selectedSoundName);
        if (error != AudioError.OK) {
            SetTextAndColor(string.Join(" ", "Playing sound called:", selectedSoundName, "failed with error message:", ErrorToStringConvertor.ErrorToMessage(error)), m_failureColor);
        }
        else {
            SetTextAndColor(string.Join(" ", "Playing sound called:", selectedSoundName, "succesfull"), m_successColor);
        }
    }

    public void PlayAtTimeStampClicked() {
        if (!float.TryParse(timeInput.text, out float timeStamp)) {
            SetTextAndColor(string.Join(" ", NOT_A_NUMBER, "Time"), m_failureColor);
            return;
        }

        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        AudioError error = am.PlayAtTimeStamp(selectedSoundName, timeStamp);
        if (error != AudioError.OK) {
            SetTextAndColor(string.Join(" ", "Playing sound called:", selectedSoundName, "at timestamp:", timeStamp.ToString("0.00"), "failed with error message:", ErrorToStringConvertor.ErrorToMessage(error)), m_failureColor);
        }
        else {
            SetTextAndColor(string.Join(" ", "Playing sound called:", selectedSoundName, "at timestamp:", timeStamp.ToString("0.00"), "succesfull"), m_successColor);
        }
    }

    public void GetPlayBackPositionClicked() {
        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        ValueDataError<float> valueDataError = am.GetPlaybackPosition(selectedSoundName);
        if (valueDataError.Error != (int)AudioError.OK) {
            SetTextAndColor(string.Join(" ", "Getting playBackPosition of the sound called:", selectedSoundName, "failed with error message:", ErrorToStringConvertor.ErrorToMessage((AudioError)valueDataError.Error)), m_failureColor);
        }
        else {
            SetTextAndColor(string.Join(" ", "Getting playBackPosition of the sound called:", selectedSoundName, "with the position being:", valueDataError.Value.ToString("0.00"), "succesfull"), m_successColor);
        }
    }

    public void PlayAt3DPositionClicked() {
        float randomXPos = Random.Range(-15f, 15f);
        float randomYPos = Random.Range(-7.5f, 10f);
        var worldPosition = new Vector3(randomXPos, randomYPos, 5f);

        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        AudioError error = am.PlayAt3DPosition(selectedSoundName, worldPosition);
        if (error != AudioError.OK) {
            SetTextAndColor(string.Join(" ", "Playing sound called:", selectedSoundName, "at the position x", worldPosition.x.ToString("0.00"), "and y", worldPosition.y.ToString("0.00"), "failed with error message:", ErrorToStringConvertor.ErrorToMessage(error)), m_failureColor);
        }
        else {
            SetTextAndColor(string.Join(" ", "Playing sound called:", selectedSoundName, "at the position x", worldPosition.x.ToString("0.00"), "and y", worldPosition.y.ToString("0.00"), "succesfull"), m_successColor);
        }
    }

    public void PlayAttachedToGameObjectClicked() {
        float randomXPos = Random.Range(-15f, 15f);
        float randomYPos = Random.Range(-7.5f, 10f);
        Vector3 worldPosition = new Vector3(randomXPos, randomYPos, 5f);

        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        AudioError error = am.PlayAttachedToGameObject(selectedSoundName, radio);
        if (error != AudioError.OK) {
            SetTextAndColor(string.Join(" ", "Playing sound called:", selectedSoundName, "attached to:", radio.name, "failed with error message:", ErrorToStringConvertor.ErrorToMessage(error)), m_failureColor);
        }
        else {
            radio.transform.position = worldPosition;
            SetTextAndColor(string.Join(" ", "Playing sound called:", selectedSoundName, "attached to:", radio.name, "succesfull"), m_successColor);
        }
    }

    public void PlayOneShotAt3DPositionClicked() {
        float randomXPos = Random.Range(-15f, 15f);
        float randomYPos = Random.Range(-7.5f, 10f);
        var worldPosition = new Vector3(randomXPos, randomYPos, 5f);

        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        AudioError error = am.PlayOneShotAt3DPosition(selectedSoundName, worldPosition);
        if (error != AudioError.OK) {
            SetTextAndColor(string.Join(" ", "Playing sound called:", selectedSoundName, "once at the position x", worldPosition.x.ToString("0.00"), "and y", worldPosition.y.ToString("0.00"), "failed with error message:", ErrorToStringConvertor.ErrorToMessage(error)), m_failureColor);
        }
        else {
            SetTextAndColor(string.Join(" ", "Playing sound called:", selectedSoundName, "once at the position x", worldPosition.x.ToString("0.00"), "and y", worldPosition.y.ToString("0.00"), " succesfull"), m_successColor);
        }
    }
    
    public void PlayOneShotAttachedToGameObjectClicked() {
        float randomXPos = Random.Range(-15f, 15f);
        float randomYPos = Random.Range(-7.5f, 10f);
        Vector3 worldPosition = new Vector3(randomXPos, randomYPos, 5f);

        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        AudioError error = am.PlayOneShotAttachedToGameObject(selectedSoundName, radio);
        if (error != AudioError.OK) {
            SetTextAndColor(string.Join(" ", "Playing sound called:", selectedSoundName, "once attached to:", radio.name, "failed with error message:", ErrorToStringConvertor.ErrorToMessage(error)), m_failureColor);
        }
        else {
            radio.transform.position = worldPosition;
            SetTextAndColor(string.Join(" ", "Playing sound called:", selectedSoundName, "once attached to:", radio.name, "succesfull"), m_successColor);
        }
    }

    public void PlayDelayedClicked() {
        if (!float.TryParse(timeInput.text, out float delay)) {
            SetTextAndColor(string.Join(" ", NOT_A_NUMBER, "Time"), m_failureColor);
            return;
        }

        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        AudioError error = am.PlayDelayed(selectedSoundName, delay);
        if (error != AudioError.OK) {
            SetTextAndColor(string.Join(" ", "Playing sound called:", selectedSoundName, "after", delay.ToString("0.00"), "seconds failed with error message:", ErrorToStringConvertor.ErrorToMessage(error)), m_failureColor);
        }
        else {
            SetTextAndColor(string.Join(" ", "Playing sound called:", selectedSoundName, "after", delay.ToString("0.00"), "seconds succesfull"), m_successColor);
        }
    }

    public void PlayOneShotClicked() {
        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        AudioError error = am.PlayOneShot(selectedSoundName);
        if (error != AudioError.OK) {
            SetTextAndColor(string.Join(" ", "Playing sound called:", selectedSoundName, "once failed with error message:", ErrorToStringConvertor.ErrorToMessage(error)), m_failureColor);
        }
        else {
            SetTextAndColor(string.Join(" ", "Playing sound called:", selectedSoundName, "once succesfull"), m_successColor);
        }
    }

    public void PlayScheduledClicked() {
        if (!double.TryParse(timeInput.text, out double delay)) {
            SetTextAndColor(string.Join(" ", NOT_A_NUMBER, "Time"), m_failureColor);
            return;
        }

        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        AudioError error = am.PlayScheduled(selectedSoundName, delay);
        if (error != AudioError.OK) {
            SetTextAndColor(string.Join(" ", "Playing sound called:", selectedSoundName, "after", delay.ToString("0.00"), "failed with error message:", ErrorToStringConvertor.ErrorToMessage(error)), m_failureColor);
        }
        else {
            SetTextAndColor(string.Join(" ", "Playing sound called:", selectedSoundName, "after", delay.ToString("0.00"), "seconds succesfull"), m_successColor);
        }
    }

    public void StopClicked() {
        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        AudioError error = am.Stop(soundNameDropDown.options[soundNameDropDown.value].text);
        if (error != AudioError.OK) {
            SetTextAndColor(string.Join(" ", "Stopping sound called:", selectedSoundName, "failed with error message:", ErrorToStringConvertor.ErrorToMessage(error)), m_failureColor);
        }
        else {
            SetTextAndColor(string.Join(" ", "Stopping sound called:", selectedSoundName, "succesfull"), m_successColor);
        }
    }

    public void ToggleMuteClicked() {
        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        AudioError error = am.ToggleMute(soundNameDropDown.options[soundNameDropDown.value].text);
        if (error != AudioError.OK) {
            SetTextAndColor(string.Join(" ", "Muting or unmuting sound called:", selectedSoundName, "failed with error message:", ErrorToStringConvertor.ErrorToMessage(error)), m_failureColor);
        }
        else {
            SetTextAndColor(string.Join(" ", "Muting or unmuting sound called:", selectedSoundName, "succesfull"), m_successColor);
        }
    }

    public void TogglePauseClicked() {
        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        AudioError error = am.TogglePause(soundNameDropDown.options[soundNameDropDown.value].text);
        if (error != AudioError.OK) {
            SetTextAndColor(string.Join(" ", "Pausing or unpausing sound called:", selectedSoundName, "failed with error message:", ErrorToStringConvertor.ErrorToMessage(error)), m_failureColor);
        }
        else {
            SetTextAndColor(string.Join(" ", "Pausing or unpausing sound called:", selectedSoundName, "succesfull"), m_successColor);
        }
    }

    public void GetProgressClicked() {
        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        ValueDataError<float> valueDataError = am.GetProgress(soundNameDropDown.options[soundNameDropDown.value].text);
        if (valueDataError.Error != (int)AudioError.OK) {
            SetTextAndColor(string.Join(" ", "Getting progress of the sound called:", selectedSoundName, "failed with error message:", ErrorToStringConvertor.ErrorToMessage((AudioError)valueDataError.Error)), m_failureColor);
        }
        else {
            SetTextAndColor(string.Join(" ", "Getting progress of the sound called:", selectedSoundName, "with the progress being:", (valueDataError.Value * 100).ToString("0.00"), "% succesfull"), m_successColor);
        }
    }

    public void GetSourceClicked() {
        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        AudioError error = am.TryGetSource(selectedSoundName, out _);
        if (error != AudioError.OK) {
            SetTextAndColor(string.Join(" ", "Getting source of the sound called:", selectedSoundName, "failed with error message:", ErrorToStringConvertor.ErrorToMessage(error)), m_failureColor);
        }
        else {
            SetTextAndColor(string.Join(" ", "Getting source of the sound called:", selectedSoundName, "succesfull"), m_successColor);
        }
    }

    public void LerpPitchClicked() {
        if (!float.TryParse(endValueInput.text, out float endValue)) {
            SetTextAndColor(string.Join(" ", NOT_A_NUMBER, "End Value"), m_failureColor);
            return;
        }
        if (!float.TryParse(timeInput.text, out float time)) {
            SetTextAndColor(string.Join(" ", NOT_A_NUMBER, "Time"), m_failureColor);
            return;
        }
        if (!float.TryParse(granularityInput.text, out float granularity)) {
            SetTextAndColor(string.Join(" ", NOT_A_NUMBER, "Granularity"), m_failureColor);
            return;
        }

        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        AudioError error = am.LerpPitch(selectedSoundName, endValue, time, granularity);
        if (error != AudioError.OK) {
            SetTextAndColor(string.Join(" ", "Lerping pitch of the sound called:", selectedSoundName, "failed with error message:", ErrorToStringConvertor.ErrorToMessage(error)), m_failureColor);
        }
        else {
            SetTextAndColor(string.Join(" ", "Lerping pitch of the sound called:", selectedSoundName, "in the time:", time.ToString("0.00"), "seconds with the endValue:", endValue.ToString("0.00"), "and the granularity:", granularity.ToString("0.00"), "succesfull"), m_successColor);
        }
    }

    public void LerpVolumeClicked() {
        if (!float.TryParse(endValueInput.text, out float endValue)) {
            SetTextAndColor(string.Join(" ", NOT_A_NUMBER, "End Value"), m_failureColor);
            return;
        }
        if (!float.TryParse(timeInput.text, out float time)) {
            SetTextAndColor(string.Join(" ", NOT_A_NUMBER, "Time"), m_failureColor);
            return;
        }
        if (!float.TryParse(granularityInput.text, out float granularity)) {
            SetTextAndColor(string.Join(" ", NOT_A_NUMBER, "Granularity"), m_failureColor);
            return;
        }

        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        AudioError error = am.LerpVolume(selectedSoundName, endValue, time, granularity);
        if (error != AudioError.OK) {
            SetTextAndColor(string.Join(" ", "Lerping volume of the sound called:", selectedSoundName, "failed with error message:", ErrorToStringConvertor.ErrorToMessage(error)), m_failureColor);
        }
        else {
            SetTextAndColor(string.Join(" ", "Lerping volume of the sound called:", selectedSoundName, "in the time:", time.ToString("0.00"), "seconds with the endValue:", endValue.ToString("0.00"), "and the granularity:", granularity.ToString("0.00"), "succesfull"), m_successColor);
        }
    }

    public void ChangeGroupValueClicked() {
        if (!float.TryParse(endValueInput.text, out float endValue)) {
            SetTextAndColor(string.Join(" ", NOT_A_NUMBER, "End Value"), m_failureColor);
            return;
        }

        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        AudioError error = am.ChangeGroupValue(selectedSoundName, EXPOSED_VOLUME_NAME, endValue);
        if (error != AudioError.OK) {
            SetTextAndColor(string.Join(" ", "Changing AudioMixerGroup volume of the sound called:", selectedSoundName, "failed with error message:", ErrorToStringConvertor.ErrorToMessage(error)), m_failureColor);
        }
        else {
            SetTextAndColor(string.Join(" ", "Changing AudioMixerGroup volume of the sound called:", selectedSoundName, "with the endValue:", endValue.ToString("0.00"), "succesfull"), m_successColor);
        }
    }

    public void GetGroupValueClicked() {
        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        ValueDataError<float> valueDataError = am.GetGroupValue(selectedSoundName, EXPOSED_VOLUME_NAME);
        if (valueDataError.Error != (int)AudioError.OK) {
            SetTextAndColor(string.Join(" ", "Getting AudioMixerGroup volume of the sound called:", selectedSoundName, "failed with error message:", ErrorToStringConvertor.ErrorToMessage((AudioError)valueDataError.Error)), m_failureColor);
        }
        else {
            SetTextAndColor(string.Join(" ", "Getting AudioMixerGroup volume of the sound called:", selectedSoundName, "with the current value being:", valueDataError.Value.ToString("0.00"), "succesfull"), m_successColor);
        }
    }

    public void ResetGroupValueClicked() {
        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        AudioError error = am.ResetGroupValue(selectedSoundName, EXPOSED_VOLUME_NAME);
        if (error != AudioError.OK) {
            SetTextAndColor(string.Join(" ", "Reseting AudioMixerGroup volume to its default value of the sound called:", selectedSoundName, "failed with error message:", ErrorToStringConvertor.ErrorToMessage(error)), m_failureColor);
        }
        else {
            SetTextAndColor(string.Join(" ", "Reseting AudioMixerGroup volume to its default value of the sound called:", selectedSoundName, "succesfull"), m_successColor);
        }
    }

    public void LerpGroupValueClicked() {
        if (!float.TryParse(endValueInput.text, out float endValue)) {
            SetTextAndColor(string.Join(" ", NOT_A_NUMBER, "End Value"), m_failureColor);
            return;
        }
        if (!float.TryParse(timeInput.text, out float time)) {
            SetTextAndColor(string.Join(" ", NOT_A_NUMBER, "Time"), m_failureColor);
            return;
        }
        if (!float.TryParse(granularityInput.text, out float granularity)) {
            SetTextAndColor(string.Join(" ", NOT_A_NUMBER, "Granularity"), m_failureColor);
            return;
        }

        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        AudioError error = am.LerpGroupValue(selectedSoundName, EXPOSED_VOLUME_NAME, endValue, time, granularity);
        if (error != AudioError.OK) {
            SetTextAndColor(string.Join(" ", "Lerping AudioMixerGroup volume of the sound called:", selectedSoundName, "failed with error message:", ErrorToStringConvertor.ErrorToMessage(error)), m_failureColor);
        }
        else {
            SetTextAndColor(string.Join(" ", "Lerping AudioMixerGroup volume of the sound called:", selectedSoundName, "in the time:", time.ToString("0.00"), "seconds with the endValue:", endValue.ToString("0.00"), "and the granularity:", granularity.ToString("0.00"), "succesfull"), m_successColor);
        }
    }

    public void SetStartTimeClicked() {
        if (!float.TryParse(timeInput.text, out float time)) {
            SetTextAndColor(string.Join(" ", NOT_A_NUMBER, "Time"), m_failureColor);
            return;
        }

        var selectedSoundName = soundNameDropDown.options[soundNameDropDown.value].text;
        AudioError error = am.SetStartTime(selectedSoundName, time);
        if (error != AudioError.OK) {
            SetTextAndColor(string.Join(" ", "Setting start time of the sound called:", selectedSoundName, "failed with error message:", ErrorToStringConvertor.ErrorToMessage(error)), m_failureColor);
        }
        else {
            SetTextAndColor(string.Join(" ", "Setting start time of the sound called:", selectedSoundName, "with the time:", time.ToString("0.00"), "seconds succesfull"), m_successColor);
        }
    }

    private void SetTextAndColor(string text, Color32 color) {
        outputText.color = color;
        outputText.text = text;
    }
}

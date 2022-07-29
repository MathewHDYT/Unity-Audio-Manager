using AudioManager.Core;
using AudioManager.Helper;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace AudioManager.Logger {
    /// <summary>
    /// Logger instances of the IAudioManager interface, simply wraps the given IAudioManager instance, to seperate logging from the actual method implementation and to easily disable logging if needed.
    /// </summary>
    public class LoggedAudioManager : IAudioManager {
        // Readonly private member variables.
        // Class used for logging.
        private readonly IAudioLogger m_logger;
        // Class used for logging.
        private readonly Object m_logContext;
        // Instance we wrap for logs.
        private readonly IAudioManager m_wrappedInstance;

        // Private member variables.
        // Float that holds the time, when a method is executed and exited.
        // Needed to calculate the time needed for a method execution.
        private float m_enterMethodTime;

        /// <summary>
        /// Constructs a decorartor that wraps the given IAudioManager instance and calls the given ILogger.
        /// </summary>
        /// <param name="logger">Logger that implements the given log methods in the ILogger interface.</param>
        /// <param name="audioManager">IAudioManager instance that should be wrapped with logging.</param>
        /// <param name="context">Context MonoBehaviour that has instantiated this class.</param>
        public LoggedAudioManager(IAudioLogger logger, IAudioManager audioManager, Object context) {
            m_logContext = context;
            m_wrappedInstance = audioManager;
            m_logger = logger;
            m_enterMethodTime = 0f;
        }

        public AudioError AddSoundFromPath(string name, string path, float volume, float pitch, bool loop, AudioSource source, AudioMixerGroup mixerGroup) {
            const string enterLogBase = "Attempting to register new AudioSource entry";
            const string exitLogBase = "Registering new AudioSource entry with the AudioManager";

            OnMethodEnter(enterLogBase, name);
            AudioError error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.AddSoundFromPath(name, path, volume, pitch, loop, source, mixerGroup));
            OnReceivedError(exitLogBase, error);
            OnMethodExit(exitLogBase, error);
            return error;
        }

        public IEnumerable<string> GetEnumerator() {
            const string enterLogBase = "Attempting to get all registered AudioSource entries";
            const string exitLogBase = "Getting all registered AudioSource entries";

            OnMethodEnter(enterLogBase);
            IEnumerable<string> value = m_wrappedInstance?.GetEnumerator();
            OnReceivedError(exitLogBase, AudioError.OK);
            OnMethodExit(exitLogBase, AudioError.OK);
            return value;
        }

        public AudioError Play(string name, ChildType child) {
            const string enterLogBase = "Attempting to play the registered AudioSource entry";
            const string exitLogBase = "Playing registered AudioSource entry";

            OnMethodEnter(enterLogBase, name);
            AudioError error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.Play(name, child));
            OnReceivedError(exitLogBase, error);
            OnMethodExit(exitLogBase, error);
            return error;
        }

        public AudioError PlayAtTimeStamp(string name, float startTime, ChildType child) {
            const string enterLogBase = "Attempting to play the registered AudioSource entry at the given timeStamp";
            const string exitLogBase = "Playing registered AudioSource entry at the given timeStamp";

            OnMethodEnter(enterLogBase, name);
            AudioError error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.PlayAtTimeStamp(name, startTime, child));
            OnReceivedError(exitLogBase, error);
            OnMethodExit(exitLogBase, error);
            return error;
        }

        public AudioError GetPlaybackPosition(string name, out float time, ChildType child) {
            const string enterLogBase = "Attempting to read the playbackPosition of the registered AudioSource entry";
            const string exitLogBase = "Reading the playbackPosition of the given registered AudioSource entry";

            time = Constants.F_NULL_VALUE;
            OnMethodEnter(enterLogBase, name);
            AudioError error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.GetPlaybackPosition(name, out time, child));
            OnReceivedError(exitLogBase, error);
            OnMethodExit(exitLogBase, error);
            return error;
        }

        public AudioError SetPlaybackDirection(string name, float pitch, ChildType child) {
            const string enterLogBase = "Attempting to set the playback direction of the registered AudioSource entry";
            const string exitLogBase = "Setting playback direction of the given registered AudioSource entry";

            OnMethodEnter(enterLogBase, name);
            AudioError error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.SetPlaybackDirection(name, pitch, child));
            OnReceivedError(exitLogBase, error);
            OnMethodExit(exitLogBase, error);
            return error;
        }

        public AudioError RegisterChildAt3DPos(string name, Vector3 position, out ChildType child) {
            const string enterLogBase = "Attempting to register new chid sound at the given 3D position in space";
            const string exitLogBase = "Registering the new chid sound at the given 3D position in space";

            child = ChildType.AT_3D_POS;
            OnMethodEnter(enterLogBase, name);
            AudioError error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.RegisterChildAt3DPos(name, position, out child));
            OnReceivedError(exitLogBase, error);
            OnMethodExit(exitLogBase, error);
            return error;
        }

        public AudioError RegisterChildAttachedToGo(string name, GameObject gameObject, out ChildType child) {
            const string enterLogBase = "Attempting to register new chid sound attached to the given gameObject";
            const string exitLogBase = "Registering the new chid sound attached to the given gameObject";

            child = ChildType.ATTCHD_TO_GO;
            OnMethodEnter(enterLogBase, name);
            AudioError error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.RegisterChildAttachedToGo(name, gameObject, out child));
            OnReceivedError(exitLogBase, error);
            OnMethodExit(exitLogBase, error);
            return error;
        }

        public AudioError DeregisterChild(string name, ChildType child) {
            const string enterLogBase = "Attempting to deregister previously registered child";
            const string exitLogBase = "Deregistering the previously registered child";

            OnMethodEnter(enterLogBase, name);
            AudioError error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.DeregisterChild(name, child));
            OnReceivedError(exitLogBase, error);
            OnMethodExit(exitLogBase, error);
            return error;

        }

        public AudioError PlayDelayed(string name, float delay, ChildType child) {
            const string enterLogBase = "Attempting to play the registered AudioSource entry delayed";
            const string exitLogBase = "Starting to play the given registered AudioSource entry delayed";

            OnMethodEnter(enterLogBase, name);
            AudioError error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.PlayDelayed(name, delay, child));
            OnReceivedError(exitLogBase, error);
            OnMethodExit(exitLogBase, error);
            return error;
        }

        public AudioError PlayOneShot(string name, ChildType child) {
            const string enterLogBase = "Attempting to play the registered AudioSource entry once";
            const string exitLogBase = "Starting to play the given registered AudioSource entry once";

            OnMethodEnter(enterLogBase, name);
            AudioError error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.PlayOneShot(name, child));
            OnReceivedError(exitLogBase, error);
            OnMethodExit(exitLogBase, error);
            return error;
        }

        public AudioError ChangePitch(string name, float minPitch, float maxPitch, ChildType child) {
            const string enterLogBase = "Attempting to randomly change pitch of the registered AudioSource entry";
            const string exitLogBase = "Randomly changing pitch of the given registered AudioSource entry to a random value between the given min and max values";

            OnMethodEnter(enterLogBase, name);
            AudioError error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.ChangePitch(name, minPitch, maxPitch, child));
            OnReceivedError(exitLogBase, error);
            OnMethodExit(exitLogBase, error);
            return error;
        }

        public AudioError GetClipLength(string name, out double length, ChildType child) {
            const string enterLogBase = "Attempting to randomly change pitch of the registered AudioSource entry";
            const string exitLogBase = "Randomly changing pitch of the given registered AudioSource entry to a random value between the given min and max values";

            length = Constants.D_NULL_VALUE;
            OnMethodEnter(enterLogBase, name);
            AudioError error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.GetClipLength(name, out length, child));
            OnReceivedError(exitLogBase, error);
            OnMethodExit(exitLogBase, error);
            return error;
        }

        public AudioError PlayScheduled(string name, double time, ChildType child) {
            const string enterLogBase = "Attempting to play the registered AudioSource entry scheduled";
            const string exitLogBase = "Starting to play the given registered AudioSource entry scheduled";

            OnMethodEnter(enterLogBase, name);
            AudioError error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.PlayScheduled(name, time, child));
            OnReceivedError(exitLogBase, error);
            OnMethodExit(exitLogBase, error);
            return error;
        }

        public AudioError Stop(string name, ChildType child) {
            const string enterLogBase = "Attempting to stop the registered AudioSource entry";
            const string exitLogBase = "Stopping the given registered AudioSource entry";

            OnMethodEnter(enterLogBase, name);
            AudioError error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.Stop(name, child));
            OnReceivedError(exitLogBase, error);
            OnMethodExit(exitLogBase, error);
            return error;
        }

        public AudioError ToggleMute(string name, ChildType child) {
            const string enterLogBase = "Attempting to toggle mute the registered AudioSource entry";
            const string exitLogBase = "Toggling mute for the given registered AudioSource entry";

            OnMethodEnter(enterLogBase, name);
            AudioError error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.ToggleMute(name, child));
            OnReceivedError(exitLogBase, error);
            OnMethodExit(exitLogBase, error);
            return error;
        }

        public AudioError TogglePause(string name, ChildType child) {
            const string enterLogBase = "Attempting to toggle pause the registered AudioSource entry";
            const string exitLogBase = "Toggling pause for the given registered AudioSource entry";

            OnMethodEnter(enterLogBase, name);
            AudioError error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.TogglePause(name, child));
            OnReceivedError(exitLogBase, error);
            OnMethodExit(exitLogBase, error);
            return error;
        }

        public AudioError SubscribeSourceChanged(string name, SourceChangedCallback callback) {
            const string enterLogBase = "Attempting to subscribe to the registered AudioSource entry being changed";
            const string exitLogBase = "Subscribing to the registered AudioSource entry being changed";

            OnMethodEnter(enterLogBase, name);
            AudioError error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.SubscribeSourceChanged(name, callback));
            OnReceivedError(exitLogBase, error);
            OnMethodExit(exitLogBase, error);
            return error;
        }

        public AudioError UnsubscribeSourceChanged(string name, SourceChangedCallback callback) {
            const string enterLogBase = "Attempting to unsubscribe to the registered AudioSource entry being changed";
            const string exitLogBase = "Unsubscribing to the registered AudioSource entry being changed";

            OnMethodEnter(enterLogBase, name);
            AudioError error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.UnsubscribeSourceChanged(name, callback));
            OnReceivedError(exitLogBase, error);
            OnMethodExit(exitLogBase, error);
            return error;
        }

        public AudioError SubscribeProgressCoroutine(string name, float progress, ProgressCoroutineCallback callback) {
            const string enterLogBase = "Attempting to subscribe to the registered AudioSource entry finishing to the given progress";
            const string exitLogBase = "Subscribing to the registered AudioSource entry finishing to the given progress";

            OnMethodEnter(enterLogBase, name);
            AudioError error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.SubscribeProgressCoroutine(name, progress, callback));
            OnReceivedError(exitLogBase, error);
            OnMethodExit(exitLogBase, error);
            return error;
        }

        public AudioError UnsubscribeProgressCoroutine(string name, float progress) {
            const string enterLogBase = "Attempting to unsubscribe to the registered AudioSource entry finishing to the given progress";
            const string exitLogBase = "Unsubscribing to the registered AudioSource entry finishing to the given progress";

            OnMethodEnter(enterLogBase, name);
            AudioError error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.UnsubscribeProgressCoroutine(name, progress));
            OnReceivedError(exitLogBase, error);
            OnMethodExit(exitLogBase, error);
            return error;
        }

        public AudioError GetProgress(string name, out float progress, ChildType child) {
            const string enterLogBase = "Attempting to read the progress of the registered AudioSource entry";
            const string exitLogBase = "Reading the progress of the registered AudioSource entry";

            progress = Constants.F_NULL_VALUE;
            OnMethodEnter(enterLogBase, name);
            AudioError error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.GetProgress(name, out progress, child));
            OnReceivedError(exitLogBase, error);
            OnMethodExit(exitLogBase, error);
            return error;
        }

        public AudioError TryGetSource(string name, out AudioSourceWrapper source) {
            const string enterLogBase = "Attempting to get registered AudioSource entry";
            const string exitLogBase = "Getting registered AudioSource entry";
            source = null;

            OnMethodEnter(enterLogBase, name);
            AudioError error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.TryGetSource(name, out source));
            OnReceivedError(exitLogBase, error);
            OnMethodExit(exitLogBase, error);
            return error;
        }

        public AudioError LerpPitch(string name, float endValue, float duration, ChildType child) {
            const string enterLogBase = "Attempting to lerp pitch of the registered AudioSource entry";
            const string exitLogBase = "Lerping pitch of the registered AudioSource entry";

            OnMethodEnter(enterLogBase, name);
            AudioError error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.LerpPitch(name, endValue, duration, child));
            OnReceivedError(exitLogBase, error);
            OnMethodExit(exitLogBase, error);
            return error;
        }

        public AudioError LerpVolume(string name, float endValue, float duration, ChildType child) {
            const string enterLogBase = "Attempting to lerp volume of the registered AudioSource entry";
            const string exitLogBase = "Lerping volume of the registered AudioSource entry";

            OnMethodEnter(enterLogBase, name);
            AudioError error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.LerpVolume(name, endValue, duration, child));
            OnReceivedError(exitLogBase, error);
            OnMethodExit(exitLogBase, error);
            return error;
        }

        public AudioError ChangeGroupValue(string name, string exposedParameterName, float newValue) {
            string enterLogBase = string.Join(" ", "Attempting to change group value with the name:", exposedParameterName, "of the registered AudioSource entry");
            const string exitLogBase = "Changing group value of the registered AudioSource entry";

            OnMethodEnter(enterLogBase, name);
            AudioError error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.ChangeGroupValue(name, exposedParameterName, newValue));
            OnReceivedError(exitLogBase, error);
            OnMethodExit(exitLogBase, error);
            return error;
        }

        public AudioError GetGroupValue(string name, string exposedParameterName, out float currentValue) {
            string enterLogBase = string.Join(" ", "Attempting to get group value with the name:", exposedParameterName, "of the registered AudioSource entry");
            const string exitLogBase = "Getting group value of the registered AudioSource entry";

            currentValue = Constants.F_NULL_VALUE;
            OnMethodEnter(enterLogBase, name);
            AudioError error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.GetGroupValue(name, exposedParameterName, out currentValue));
            OnReceivedError(exitLogBase, error);
            OnMethodExit(exitLogBase, error);
            return error;
        }

        public AudioError ResetGroupValue(string name, string exposedParameterName) {
            string enterLogBase = string.Join(" ", "Attempting to reset group value with the name:", exposedParameterName, "of the registered AudioSource entry");
            const string exitLogBase = "Resetting group value of the registered AudioSource entry";

            OnMethodEnter(enterLogBase, name);
            AudioError error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.ResetGroupValue(name, exposedParameterName));
            OnReceivedError(exitLogBase, error);
            OnMethodExit(exitLogBase, error);
            return error;
        }

        public AudioError LerpGroupValue(string name, string exposedParameterName, float endValue, float duration) {
            string enterLogBase = string.Join(" ", "Attempting to lerp group value with the name:", exposedParameterName, "of the registered AudioSource entry");
            const string exitLogBase = "Lerping group value of the registered AudioSource entry";

            OnMethodEnter(enterLogBase, name);
            AudioError error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.LerpGroupValue(name, exposedParameterName, endValue, duration));
            OnReceivedError(exitLogBase, error);
            OnMethodExit(exitLogBase, error);
            return error;
        }

        public AudioError RemoveGroup(string name, ChildType child) {
            const string enterLogBase = "Attempting to remove group from the registered AudioSource entry";
            const string exitLogBase = "Removing group from the registered AudioSource entry";

            OnMethodEnter(enterLogBase, name);
            AudioError error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.RemoveGroup(name, child));
            OnReceivedError(exitLogBase, error);
            OnMethodExit(exitLogBase, error);
            return error;
        }

        public AudioError AddGroup(string name, AudioMixerGroup mixerGroup, ChildType child) {
            const string enterLogBase = "Attempting to add group to the registered AudioSource entry";
            const string exitLogBase = "Adding group to the registered AudioSource entry";

            OnMethodEnter(enterLogBase, name);
            AudioError error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.AddGroup(name, mixerGroup, child));
            OnReceivedError(exitLogBase, error);
            OnMethodExit(exitLogBase, error);
            return error;
        }

        public AudioError RemoveSound(string name) {
            const string enterLogBase = "Attempting to remove registered AudioSource entry";
            const string exitLogBase = "Removing registered AudioSource entry";

            OnMethodEnter(enterLogBase, name);
            AudioError error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.RemoveSound(name));
            OnReceivedError(exitLogBase, error);
            OnMethodExit(exitLogBase, error);
            return error;
        }

        public AudioError Set3DAudioOptions(string name, float minDistance, float maxDistance, ChildType child, float spatialBlend, float spreadAngle, float dopplerLevel, AudioRolloffMode rolloffMode) {
            const string enterLogBase = "Attempting to set 3D audio options of the registered AudioSource entry";
            const string exitLogBase = "Setting 3D audio options of the registered AudioSource entry";

            OnMethodEnter(enterLogBase, name);
            AudioError error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.Set3DAudioOptions(name, minDistance, maxDistance, child, spatialBlend, spreadAngle, dopplerLevel, rolloffMode));
            OnReceivedError(exitLogBase, error);
            OnMethodExit(exitLogBase, error);
            return error;
        }

        public AudioError SetStartTime(string name, float startTime, ChildType child) {
            const string enterLogBase = "Attempting to set start time of the registered AudioSource entry";
            const string exitLogBase = "Setting start time of the given registered AudioSource entry";

            OnMethodEnter(enterLogBase, name);
            AudioError error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.SetStartTime(name, startTime, child));
            OnReceivedError(exitLogBase, error);
            OnMethodExit(exitLogBase, error);
            return error;
        }

        public AudioError SkipTime(string name, float time, ChildType child) {
            const string enterLogBase = "Attempting to skip the registered AudioSource entries forward or backward";
            const string exitLogBase = "Skipping the given registered AudioSource entry forward or backward";

            OnMethodEnter(enterLogBase);
            AudioError error = AudioHelper.ConvertToAudioError(m_wrappedInstance?.SkipTime(name, time, child));
            OnReceivedError(exitLogBase, error);
            OnMethodExit(exitLogBase, error);
            return error;
        }

        //************************************************************************************************************************
        // Private Section
        //************************************************************************************************************************

        private void OnMethodEnter(string baselogMessage) {
            m_logger?.Log(string.Join(" ", baselogMessage), LoggingLevel.HIGH, LoggingType.NORMAL, m_logContext);
            // Cache the current time, before the method will be executed.
            m_enterMethodTime = Time.realtimeSinceStartup;
        }

        private void OnMethodEnter(string baselogMessage, string name) {
            m_logger?.Log(string.Join(" ", baselogMessage, "with the name:", name), LoggingLevel.HIGH, LoggingType.NORMAL, m_logContext);
            // Cache the current time, before the method will be executed.
            m_enterMethodTime = Time.realtimeSinceStartup;
        }

        private void OnReceivedError(string baselogMessage, AudioError error) {
            // Don't log when no error has been received.
            if (error == AudioError.OK) {
                return;
            }
            m_logger?.LogFormat("{0} failed.\r\n<color=yellow>{1}</color>", LoggingLevel.LOW, LoggingType.WARNING, m_logContext, baselogMessage, ErrorToStringConvertor.ErrorToMessage(error));
        }

        private void OnMethodExit(string baselogMessage, AudioError error) {
            // Get the current time, after the method was executed.
            float exitMethodTime = Time.realtimeSinceStartup;
            // Log method execution time, calculated fromt the cached enter time and the exit time.
            m_logger?.Log(string.Join(" ", baselogMessage, "executed in:", ((exitMethodTime - m_enterMethodTime) * 1000000f), "microseconds"), LoggingLevel.STOPWATCH, LoggingType.NORMAL, m_logContext);

            // Check if any errors have occured while calling the method.
            if (error != AudioError.OK) {
                m_logger?.Log(string.Join(" ", baselogMessage, "failed"), LoggingLevel.INTERMEDIATE, LoggingType.NORMAL, m_logContext);
                return;
            }

            m_logger?.Log(string.Join(" ", baselogMessage, "successfull"), LoggingLevel.INTERMEDIATE, LoggingType.NORMAL, m_logContext);
        }
    }
}

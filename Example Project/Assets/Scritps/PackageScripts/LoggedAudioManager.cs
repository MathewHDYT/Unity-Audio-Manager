using AudioManager.Helper;
using AudioManager.Locator;
using AudioManager.Logger;
using UnityEngine;
using UnityEngine.Audio;

namespace AudioManager.Service {
    /// <summary>
    /// Logger instances of the IAudioManager interface, simply wraps the given IAudioManager instance, to seperate logging from the actual method implementations and to easily disable logging if needed.
    /// </summary>
    public class LoggedAudioManager : IAudioManager {
        // Class used for logging.
        private Logger.Logger logger;
        // Class used for logging.
        private Object logContext;
        // Instance we wrap for logs.
        private IAudioManager wrappedInstance;
        // Float that holds the time, when a method is executed and exited.
        // Needed to calculate the time needed for a method execution.
        private float enterMethodTime;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="minloggingLevel">Minimum logging level to still print to the console for the logger.</param>
        /// <param name="context">Context MonoBehaviour that has instantiated this class.</param>
        /// <param name="audioManager">IAudioManager instance that should be wrapped with logging.</param>
        public LoggedAudioManager(LoggingLevel minloggingLevel, Object context, IAudioManager audioManager) {
            logContext = context;
            wrappedInstance = audioManager;
            logger = new Logger.Logger(minloggingLevel);
        }

        public AudioError AddSoundFromPath(string name, string path, float volume, float pitch, bool loop, AudioSource source, AudioMixerGroup mixerGroup) {
            const string enterLogBase = "Attempting to register new AudioSource entry";
            const string exitLogBase = "Registering new AudioSource entry with the AudioManager";

            OnMethodEnter(enterLogBase, name);
            AudioError error = wrappedInstance.AddSoundFromPath(name, path, volume, pitch, loop, source, mixerGroup);
            OnReceivedError(exitLogBase, error);
            OnMethodExit(exitLogBase, error);
            return error;
        }

        public AudioError Play(string name) {
            const string enterLogBase = "Attempting to play the registered AudioSource entry";
            const string exitLogBase = "Playing registered AudioSource entry";

            OnMethodEnter(enterLogBase, name);
            AudioError error = wrappedInstance.Play(name);
            OnReceivedError(exitLogBase, error);
            OnMethodExit(exitLogBase, error);
            return error;
        }

        public AudioError PlayAtTimeStamp(string name, float startTime) {
            const string enterLogBase = "Attempting to play the registered AudioSource entry at the given timeStamp";
            const string exitLogBase = "Playing registered AudioSource entry at the given timeStamp";

            OnMethodEnter(enterLogBase, name);
            AudioError error = wrappedInstance.PlayAtTimeStamp(name, startTime);
            OnReceivedError(exitLogBase, error);
            OnMethodExit(exitLogBase, error);
            return error;
        }

        public ValueDataError<float> GetPlaybackPosition(string name) {
            const string enterLogBase = "Attempting to read the playBackPosition of the registered AudioSource entry";
            const string exitLogBase = "Reading the playBackPosition of the given registered AudioSource entry";

            OnMethodEnter(enterLogBase, name);
            ValueDataError<float> valueDataError = wrappedInstance.GetPlaybackPosition(name);
            OnReceivedError(exitLogBase, ((AudioError)valueDataError.Error));
            OnMethodExit(exitLogBase, ((AudioError)valueDataError.Error));
            return valueDataError;
        }

        public AudioError PlayAt3DPosition(string name, Vector3 position) {
            const string enterLogBase = "Attempting to play the registered AudioSource entry at the given 3D position in space";
            const string exitLogBase = "Playing the registered AudioSource entry at the given 3D position in space";

            OnMethodEnter(enterLogBase, name);
            AudioError error = wrappedInstance.PlayAt3DPosition(name, position);
            OnReceivedError(exitLogBase, error);
            OnMethodExit(exitLogBase, error);
            return error;
        }

        public AudioError PlayOneShotAt3DPosition(string name, Vector3 position) {
            const string enterLogBase = "Attempting to play the registered AudioSource entry once at the given 3D position in space";
            const string exitLogBase = "Playing the registered AudioSource entry once at the given 3D position in space";

            OnMethodEnter(enterLogBase, name);
            AudioError error = wrappedInstance.PlayOneShotAt3DPosition(name, position);
            OnReceivedError(exitLogBase, error);
            OnMethodExit(exitLogBase, error);
            return error;
        }

        public AudioError PlayAttachedToGameObject(string name, GameObject gameObject) {
            const string enterLogBase = "Attempting to play the registered AudioSource entry attached to the given gameObject";
            const string exitLogBase = "Playing the registered AudioSource entry attached to the given gameObject";

            OnMethodEnter(enterLogBase, name);
            AudioError error = wrappedInstance.PlayAttachedToGameObject(name, gameObject);
            OnReceivedError(exitLogBase, error);
            OnMethodExit(exitLogBase, error);
            return error;
        }

        public AudioError PlayOneShotAttachedToGameObject(string name, GameObject gameObject) {
            const string enterLogBase = "Attempting to play the registered AudioSource entry once attached to the given gameObject";
            const string exitLogBase = "Playing the registered AudioSource entry once attached to the given gameObject";

            OnMethodEnter(enterLogBase, name);
            AudioError error = wrappedInstance.PlayOneShotAttachedToGameObject(name, gameObject);
            OnReceivedError(exitLogBase, error);
            OnMethodExit(exitLogBase, error);
            return error;
        }

        public AudioError PlayDelayed(string name, float delay) {
            const string enterLogBase = "Attempting to play the registered AudioSource entry delayed";
            const string exitLogBase = "Starting to play the given registered AudioSource entry delayed";

            OnMethodEnter(enterLogBase, name);
            AudioError error = wrappedInstance.PlayDelayed(name, delay);
            OnReceivedError(exitLogBase, error);
            OnMethodExit(exitLogBase, error);
            return error;
        }

        public AudioError PlayOneShot(string name) {
            const string enterLogBase = "Attempting to play the registered AudioSource entry once";
            const string exitLogBase = "Starting to play the given registered AudioSource entry once";

            OnMethodEnter(enterLogBase, name);
            AudioError error = wrappedInstance.PlayOneShot(name);
            OnReceivedError(exitLogBase, error);
            OnMethodExit(exitLogBase, error);
            return error;
        }

        public AudioError ChangePitch(string name, float minPitch, float maxPitch) {
            const string enterLogBase = "Attempting to randomly change pitch of the registered AudioSource entry";
            const string exitLogBase = "Randomly changing pitch of the given registered AudioSource entry to a random value between the given min and max values";

            OnMethodEnter(enterLogBase, name);
            AudioError error = wrappedInstance.ChangePitch(name, minPitch, maxPitch);
            OnReceivedError(exitLogBase, error);
            OnMethodExit(exitLogBase, error);
            return error;
        }

        public AudioError PlayScheduled(string name, double time) {
            const string enterLogBase = "Attempting to play the registered AudioSource entry scheduled";
            const string exitLogBase = "Starting to play the given registered AudioSource entry scheduled";

            OnMethodEnter(enterLogBase, name);
            AudioError error = wrappedInstance.PlayScheduled(name, time);
            OnReceivedError(exitLogBase, error);
            OnMethodExit(exitLogBase, error);
            return error;
        }

        public AudioError Stop(string name) {
            const string enterLogBase = "Attempting to stop the registered AudioSource entry";
            const string exitLogBase = "Stopping the given registered AudioSource entry";

            OnMethodEnter(enterLogBase, name);
            AudioError error = wrappedInstance.Stop(name);
            OnReceivedError(exitLogBase, error);
            OnMethodExit(exitLogBase, error);
            return error;
        }

        public AudioError ToggleMute(string name) {
            const string enterLogBase = "Attempting to toggle mute the registered AudioSource entry";
            const string exitLogBase = "Toggling mute for the given registered AudioSource entry";

            OnMethodEnter(enterLogBase, name);
            AudioError error = wrappedInstance.ToggleMute(name);
            OnReceivedError(exitLogBase, error);
            OnMethodExit(exitLogBase, error);
            return error;
        }

        public AudioError TogglePause(string name) {
            const string enterLogBase = "Attempting to toggle pause the registered AudioSource entry";
            const string exitLogBase = "Toggling pause for the given registered AudioSource entry";

            OnMethodEnter(enterLogBase, name);
            AudioError error = wrappedInstance.TogglePause(name);
            OnReceivedError(exitLogBase, error);
            OnMethodExit(exitLogBase, error);
            return error;
        }

        public AudioError SubscribeAudioFinished(string name, float remainingTime, System.Action<string, float> callback) {
            const string enterLogBase = "Attempting to subscribe to the registered AudioSource entry finishing to the given remainingTime";
            const string exitLogBase = "Subscribing to the registered AudioSource entry finishing to the given remainingTime";

            OnMethodEnter(enterLogBase, name);
            AudioError error = wrappedInstance.SubscribeAudioFinished(name, remainingTime, callback);
            OnReceivedError(exitLogBase, error);
            OnMethodExit(exitLogBase, error);
            return error;
        }

        public ValueDataError<float> GetProgress(string name) {
            const string enterLogBase = "Attempting to read the progress of the registered AudioSource entry";
            const string exitLogBase = "Reading the progress of the registered AudioSource entry";

            OnMethodEnter(enterLogBase, name);
            ValueDataError<float> valueDataError = wrappedInstance.GetProgress(name);
            OnReceivedError(exitLogBase, ((AudioError)valueDataError.Error));
            OnMethodExit(exitLogBase, ((AudioError)valueDataError.Error));
            return valueDataError;
        }

        public AudioError TryGetSource(string name, out AudioSource source) {
            const string enterLogBase = "Attempting to get registered AudioSource entry";
            const string exitLogBase = "Getting registered AudioSource entry";

            OnMethodEnter(enterLogBase, name);
            AudioError error = wrappedInstance.TryGetSource(name, out source);
            OnReceivedError(exitLogBase, error);
            OnMethodExit(exitLogBase, error);
            return error;
        }

        public AudioError LerpPitch(string name, float endValue, float waitTime, float granularity) {
            const string enterLogBase = "Attempting to lerp pitch of the registered AudioSource entry";
            const string exitLogBase = "Lerping pitch of the registered AudioSource entry";

            OnMethodEnter(enterLogBase, name);
            AudioError error = wrappedInstance.LerpPitch(name, endValue, waitTime, granularity);
            OnReceivedError(exitLogBase, error);
            OnMethodExit(exitLogBase, error);
            return error;
        }

        public AudioError LerpVolume(string name, float endValue, float waitTime, float granularity) {
            const string enterLogBase = "Attempting to lerp volume of the registered AudioSource entry";
            const string exitLogBase = "Lerping volume of the registered AudioSource entry";

            OnMethodEnter(enterLogBase, name);
            AudioError error = wrappedInstance.LerpVolume(name, endValue, waitTime, granularity);
            OnReceivedError(exitLogBase, error);
            OnMethodExit(exitLogBase, error);
            return error;
        }

        public AudioError ChangeGroupValue(string name, string exposedParameterName, float newValue) {
            string enterLogBase = string.Join(" ", "Attempting to change group value with the name:", exposedParameterName, "of the registered AudioSource entry");
            const string exitLogBase = "Changing group value of the registered AudioSource entry";

            OnMethodEnter(enterLogBase, name);
            AudioError error = wrappedInstance.ChangeGroupValue(name, exposedParameterName, newValue);
            OnReceivedError(exitLogBase, error);
            OnMethodExit(exitLogBase, error);
            return error;
        }

        public ValueDataError<float> GetGroupValue(string name, string exposedParameterName) {
            string enterLogBase = string.Join(" ", "Attempting to get group value with the name:", exposedParameterName, "of the registered AudioSource entry");
            const string exitLogBase = "Getting group value of the registered AudioSource entry";

            OnMethodEnter(enterLogBase, name);
            ValueDataError<float> valueDataError = wrappedInstance.GetGroupValue(name, exposedParameterName);
            OnReceivedError(exitLogBase, ((AudioError)valueDataError.Error));
            OnMethodExit(exitLogBase, ((AudioError)valueDataError.Error));
            return valueDataError;
        }

        public AudioError ResetGroupValue(string name, string exposedParameterName) {
            string enterLogBase = string.Join(" ", "Attempting to reset group value with the name:", exposedParameterName, "of the registered AudioSource entry");
            const string exitLogBase = "Resetting group value of the registered AudioSource entry";

            OnMethodEnter(enterLogBase, name);
            AudioError error = wrappedInstance.ResetGroupValue(name, exposedParameterName);
            OnReceivedError(exitLogBase, error);
            OnMethodExit(exitLogBase, error);
            return error;
        }

        public AudioError LerpGroupValue(string name, string exposedParameterName, float endValue, float waitTime, float granularity) {
            string enterLogBase = string.Join(" ", "Attempting to lerp group value with the name:", exposedParameterName, "of the registered AudioSource entry");
            const string exitLogBase = "Lerping group value of the registered AudioSource entry";

            OnMethodEnter(enterLogBase, name);
            AudioError error = wrappedInstance.LerpGroupValue(name, exposedParameterName, endValue, waitTime, granularity);
            OnReceivedError(exitLogBase, error);
            OnMethodExit(exitLogBase, error);
            return error;
        }

        public AudioError RemoveGroup(string name) {
            const string enterLogBase = "Attempting to remove group from the registered AudioSource entry";
            const string exitLogBase = "Removing group from the registered AudioSource entry";

            OnMethodEnter(enterLogBase, name);
            AudioError error = wrappedInstance.RemoveGroup(name);
            OnReceivedError(exitLogBase, error);
            OnMethodExit(exitLogBase, error);
            return error;
        }

        public AudioError AddGroup(string name, AudioMixerGroup mixerGroup) {
            const string enterLogBase = "Attempting to add group to the registered AudioSource entry";
            const string exitLogBase = "Adding group to the registered AudioSource entry";

            OnMethodEnter(enterLogBase, name);
            AudioError error = wrappedInstance.AddGroup(name, mixerGroup);
            OnReceivedError(exitLogBase, error);
            OnMethodExit(exitLogBase, error);
            return error;
        }

        public AudioError RemoveSound(string name) {
            const string enterLogBase = "Attempting to remove registered AudioSource entry";
            const string exitLogBase = "Removing registered AudioSource entry";

            OnMethodEnter(enterLogBase, name);
            AudioError error = wrappedInstance.RemoveSound(name);
            OnReceivedError(exitLogBase, error);
            OnMethodExit(exitLogBase, error);
            return error;
        }

        public AudioError Set3DAudioOptions(string name, float minDistance, float maxDistance, float spatialBlend, float spread, float dopplerLevel, AudioRolloffMode rolloffMode) {
            const string enterLogBase = "Attempting to set 3D audio options of the registered AudioSource entry";
            const string exitLogBase = "Setting 3D audio options of the registered AudioSource entry";

            OnMethodEnter(enterLogBase, name);
            AudioError error = wrappedInstance.Set3DAudioOptions(name, minDistance, maxDistance, spatialBlend, spread, dopplerLevel, rolloffMode);
            OnReceivedError(exitLogBase, error);
            OnMethodExit(exitLogBase, error);
            return error;
        }

        public AudioError SetStartTime(string name, float startTime) {
            const string enterLogBase = "Attempting to set start time of the registered AudioSource entry";
            const string exitLogBase = "Setting start time of the given registered AudioSource entry";

            OnMethodEnter(enterLogBase, name);
            AudioError error = wrappedInstance.SetStartTime(name, startTime);
            OnReceivedError(exitLogBase, error);
            OnMethodExit(exitLogBase, error);
            return error;
        }

        /// <summary>
        /// Prints a log statement for when a method has been entering.
        /// </summary>
        /// <param name="baselogMessage">Base message that shows which method was entered.</param>
        /// <param name="name">Name of the registered AudioSource entry that was passed as an argument to the method.</param>
        private void OnMethodEnter(string baselogMessage, string name) {
            logger.Log(string.Join(" ", baselogMessage, "with the name:", name), LoggingLevel.INTERMEDIATE, LoggingType.NORMAL, logContext);
            // Cache the current time, before the method will be executed.
            enterMethodTime = Time.realtimeSinceStartup;
        }

        /// <summary>
        /// Prints a log statement for when a method has produced an error.
        /// </summary>
        /// <param name="baselogMessage">Base message that shows which method was entered.</param>
        /// <param name="error">Error that was returned by the method.</param>
        private void OnReceivedError(string baselogMessage, AudioError error) {
            // Don't log when no error has been received.
            if (error == AudioError.OK) {
                return;
            }
            logger.Log(string.Join(" ", baselogMessage, "failed.", ErrorToStringConvertor.ErrorToMessage(error)), LoggingLevel.LOW, LoggingType.WARNING, logContext);
        }

        /// <summary>
        /// Prints a log statement for when a method has been exited. Either successfully or unsuccessfully.
        /// </summary>
        /// <param name="baselogMessage">Base message that shows which method was entered.</param>
        /// <param name="error">Error that was returned by the method.</param>
        private void OnMethodExit(string baselogMessage, AudioError error) {
            // Get the current time, after the method was executed.
            float exitMethodTime = Time.realtimeSinceStartup;
            // Log method execution time, calculated fromt the cached enter time and the exit time.
            logger.Log(string.Join(" ", baselogMessage, "executed in:", ((exitMethodTime - enterMethodTime) * 1000000f), "microseconds"), LoggingLevel.STOPWATCH, LoggingType.NORMAL, logContext);

            // Check if any errors have occured while calling the method.
            if (error != AudioError.OK) {
                logger.Log(string.Join(" ", baselogMessage, "failed"), LoggingLevel.HIGH, LoggingType.NORMAL, logContext);
                return;
            }

            logger.Log(string.Join(" ", baselogMessage, "successfull"), LoggingLevel.HIGH, LoggingType.NORMAL, logContext);
        }
    }
}

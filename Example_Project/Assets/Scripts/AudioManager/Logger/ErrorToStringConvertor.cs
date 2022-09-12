using AudioManager.Core;

namespace AudioManager.Logger {
    public static class ErrorToStringConvertor {
        /// <summary>
        /// Convert the given error to a string message.
        /// </summary>
        /// <param name="error">Error we want to convert into a readable string.</param>
        /// <returns>Message that represents the given AudioError</returns>
        public static string ErrorToMessage(AudioError error) {
            return error switch {
                AudioError.OK => "Method succesfully executed",
                AudioError.DOES_NOT_EXIST => "Sound has not been registered with the AudioManager",
                AudioError.ALREADY_EXISTS => "There already exists a sound with that name",
                AudioError.INVALID_PATH => "Path does not lead to a valid audio clip",
                AudioError.INVALID_END_VALUE => "The given endValue is already the same as the current value",
                AudioError.INVALID_TIME => "The given time exceeds the actual length of the clip",
                AudioError.INVALID_PROGRESS => "The given value is to close to the end or the start of the actual clip length, because playing audio is frame rate independent",
                AudioError.MIXER_NOT_EXPOSED => "The given parameter in the AudioMixer is not exposed or does not exist",
                AudioError.MISSING_SOURCE => "Sound does not have an AudioSource component on the GameObject the AudioManager resides on",
                AudioError.MISSING_MIXER_GROUP => "Group methods may only be called with a sound that has a set AudioMixerGroup",
                AudioError.CAN_NOT_BE_3D => "The sound can not be 3D, because spatialBlend is set to be 2D instead of 3D",
                AudioError.NOT_INITIALIZED => "No IAudioManager has been registered with the ServiceLocator ensure a GameObject with the AudioManagerSettings script is in your scene",
                AudioError.MISSING_CLIP => "Sound does not have an AudioClip component that can be played",
                AudioError.MISSING_PARENT => "AudioManager did not get passed a valid parent gameObject with the needed components. (MonoBehaviour, Transform)",
                AudioError.INVALID_PARENT => "The given gameObject passed to the method was null and therefore no AudioSource component can be attached and played on it",
                AudioError.ALREADY_SUBSCRIBED => "Callback with the exact same progress was already subscribed for this sound",
                AudioError.NOT_SUBSCRIBED => "Callback with the progress was not yet subscribed for this sound",
                AudioError.MISSING_WRAPPER => "The given AudioSourceWrapper is null",
                AudioError.INVALID_CHILD => "The given sound does not have a registered child of the given type.",
                _ => "", // Unexpected AudioError argument.
            };
        }
    }
}

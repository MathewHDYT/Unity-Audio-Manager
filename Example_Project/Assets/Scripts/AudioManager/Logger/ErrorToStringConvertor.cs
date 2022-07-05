using AudioManager.Core;

namespace AudioManager.Logger {
    public class ErrorToStringConvertor {
        /// <summary>
        /// Convert the given error to a string message.
        /// </summary>
        /// <param name="error">Error we want to convert into a readable string.</param>
        /// <returns>Message that represents the given AudioError</returns>
        public static string ErrorToMessage(AudioError error) {
            switch (error) {
                case AudioError.OK:
                    return "Method succesfully executed";
                case AudioError.DOES_NOT_EXIST:
                    return "Sound has not been registered with the AudioManager";
                case AudioError.ALREADY_EXISTS:
                    return "There already exists a sound with that name";
                case AudioError.INVALID_PATH:
                    return "Path does not lead to a valid audio clip";
                case AudioError.INVALID_END_VALUE:
                    return "The given endValue is already the same as the current value";
                case AudioError.INVALID_TIME:
                    return "The given time exceeds the actual length of the clip";
                case AudioError.INVALID_PROGRESS:
                    return "The given value is to close to the end or the start of the actual clip length, because playing audio is frame rate independent";
                case AudioError.MIXER_NOT_EXPOSED:
                    return "The given parameter in the AudioMixer is not exposed or does not exist";
                case AudioError.MISSING_SOURCE:
                    return "Sound does not have an AudioSource component on the GameObject the AudioManager resides on";
                case AudioError.MISSING_MIXER_GROUP:
                    return "Group methods may only be called with a sound that has a set AudioMixerGroup";
                case AudioError.CAN_NOT_BE_3D:
                    return "The sound can not be 3D, because spatialBlend is set to be 2D instead of 3D";
                case AudioError.NOT_INITIALIZED:
                    return "No IAudioManager has been registered with the ServiceLocator ensure a GameObject with the AudioManagerSettings script is in your scene";
                case AudioError.MISSING_CLIP:
                    return "Sound does not have an AudioClip component that can be played";
                case AudioError.MISSING_PARENT:
                    return "AudioManager did not get passed a valid parent gameObject with the needed components. (MonoBehaviour, Transform)";
                case AudioError.INVALID_PARENT:
                    return "The given gameObject passed to the method was null and therefore no AudioSource component can be attached and played on it";
                case AudioError.ALREADY_SUBSCRIBED:
                    return "Callback with the exact same progress was already subscribed for this sound";
                case AudioError.NOT_SUBSCRIBED:
                    return "Callback with the progress was not yet subscribed for this sound";
                case AudioError.MISSING_WRAPPER:
                    return "The given AudioSourceWrapper is null";
                case AudioError.INVALID_CHILD:
                    return "The given sound does not have a registered child of the given type.";
                default:
                    // Unexpected AudioError argument.
                    return "";
            }
        }
    }
}

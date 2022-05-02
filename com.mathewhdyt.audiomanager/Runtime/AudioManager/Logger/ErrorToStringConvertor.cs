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
                    return "Can't add sound as there already exists a sound with that name";
                case AudioError.INVALID_PATH:
                    return "Can't add sound because the path does not lead to a valid audio clip";
                case AudioError.INVALID_END_VALUE:
                    return "The given endValue is already the same as the current value";
                case AudioError.INVALID_GRANULARITY:
                    return "The given granularity is too small, has to be higher than or equal to 1";
                case AudioError.INVALID_TIME:
                    return "The given time exceeds the actual length of the clip";
                case AudioError.INVALID_PROGRESS:
                    return "The given value is to close to the end of the actual clip length, therefore the given value can not be detected, because playing audio is frame rate independent";
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
                default:
                    // Unexpected AudioError argument.
                    return "";
            }
        }
    }
}

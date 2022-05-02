using UnityEngine;
using UnityEngine.Audio;

namespace AudioManager.Settings {
    [CreateAssetMenu(fileName = "AudioSourceSettings", menuName = "AudioManager/AudioSourceSettings", order = 1)]
    public class AudioSourceSetting : ScriptableObject {
        [Tooltip("Sets the name we can later access this objects AudioSource in the AudioManager with.")]
        public string soundName;
        [Tooltip("The AudioClip asset played by the AudioSource.")]
        public AudioClip audioClip;

        [Space(15)]

        [Tooltip("Sets wheter the sound should play through an AudioMixer first or directly to the AudioListener.")]
        public AudioMixerGroup mixerGroup;
        [Tooltip("Set the source to loop once it is finished.")]
        public bool loop = false;

        [Space(15)]

        [Tooltip("Sets the overall volume of the sound.")]
        [Range(0f, 1f)]
        public float volume = 1f;
        [Tooltip("Sets the frequency of the sound. Use this to slow down or speed up the sounds.")]
        [Range(-3f, 3f)]
        public float pitch = 1f;

        [Space(15)]

        [Range(0f, 1f)]
        [Tooltip("Sets how much the AudioSource is treated as a 3D source. 3D sources are effected by spatial position and spreadAngle. If 3D Pan Level is 0, all spatial attenuation is ignored.")]
        public float spatialBlend = 0f;

        [Space(15)]

#if UNITY_EDITOR
        [ShowIfAttribute(ActionOnConditionFail.DO_NOT_DRAW, ConditionOperator.AND, nameof(spatialBlend))]
#endif // UNITY_EDITOR
        [Tooltip("Sets how much the pitch is changed based on the relative velocity between AudioListener and AudioSource.")]
        [Range(0f, 5f)]
        public float dopplerLevel = 1f;
#if UNITY_EDITOR
        [ShowIfAttribute(ActionOnConditionFail.DO_NOT_DRAW, ConditionOperator.AND, nameof(spatialBlend))]
#endif // UNITY_EDITOR
        [Tooltip("Sets the angle of the spread of a 3D sound relative to the speaker position.")]
        [Range(0f, 360f)]
        public float spreadAngle = 0f;
#if UNITY_EDITOR
        [ShowIfAttribute(ActionOnConditionFail.DO_NOT_DRAW, ConditionOperator.AND, nameof(spatialBlend))]
#endif // UNITY_EDITOR
        [Tooltip("Sets the type of rollof curve to use.")]
        public AudioRolloffMode volumeRolloff = AudioRolloffMode.Logarithmic;
#if UNITY_EDITOR
        [ShowIfAttribute(ActionOnConditionFail.DO_NOT_DRAW, ConditionOperator.AND, nameof(spatialBlend))]
#endif // UNITY_EDITOR
        [Tooltip("Sets the minDistance, where the volume will stay at the loudest possible. Outside of this minDistance it will begin to attenuate.")]
        public float minDistance = 1f;
#if UNITY_EDITOR
        [ShowIfAttribute(ActionOnConditionFail.DO_NOT_DRAW, ConditionOperator.AND, nameof(spatialBlend))]
#endif // UNITY_EDITOR
        [Tooltip("Sets the maxDistance, where a sound stops attenuating at.")]
        public float maxDistance = 500f;

        [HideInInspector]
        public AudioSource source;
    }
}

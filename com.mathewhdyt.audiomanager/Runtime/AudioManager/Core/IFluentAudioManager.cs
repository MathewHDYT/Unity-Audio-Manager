using UnityEngine;
using UnityEngine.Audio;

namespace AudioManager.Core {
    public interface IFluentAudioManager {
        /// <summary>
        /// Stops the chaining of methods and returns if any the error we received that stopped the other methods from being called.
        /// </summary>
        /// <returns><see cref="AudioError"/>, showing wheter and how calling the previously called method failed.</returns>
        public AudioError Execute();

        /// <summary>
        /// <see cref="IAudioManager.Play"/>
        /// </summary>
        /// <returns><see cref="AudioError"/>, showing wheter and how playing the sound failed.</returns>
        public AudioError Play();

        /// <summary>
        /// <see cref="IAudioManager.PlayAtTimeStamp"/>
        /// </summary>
        /// <returns><see cref="AudioError"/>, showing wheter and how playing the sound from the given startTime failed.</returns>
        public AudioError PlayAtTimeStamp(float startTime);

        /// <summary>
        /// <see cref="IAudioManager.PlayDelayed"/>
        /// </summary>
        /// <returns><see cref="AudioError"/>, showing wheter and how playing the sound after the given amount of time failed.</returns>
        public AudioError PlayDelayed(float delay);

        /// <summary>
        /// <see cref="IAudioManager.PlayOneShot"/>
        /// </summary>
        /// <returns><see cref="AudioError"/>, showing wheter and how playing the sound once failed.</returns>
        public AudioError PlayOneShot();

        /// <summary>
        /// <see cref="IAudioManager.PlayScheduled"/>
        /// </summary>
        /// <returns><see cref="AudioError"/>, showing wheter and how playing the sound after the given amount of time failed.</returns>
        public AudioError PlayScheduled(double time);

        /// <summary>
        /// <see cref="IAudioManager.DeregisterChild"/>
        /// </summary>
        /// <returns><see cref="IFluentAudioManager"/>, class that allows chaining.</returns>
        public IFluentAudioManager DeregisterChild();

        /// <summary>
        /// <see cref="IAudioManager.Stop"/>
        /// </summary>
        /// <returns><see cref="AudioError"/>, showing wheter and how stopping the sound failed.</returns>
        public AudioError Stop();

        /// <summary>
        /// <see cref="IAudioManager.RemoveSound"/>
        /// </summary>
        /// <returns><see cref="AudioError"/>, showing wheter and how removing the sound failed.</returns>
        public AudioError RemoveSound();

        /// <summary>
        /// <see cref="IAudioManager.GetPlaybackPosition"/>
        /// </summary>
        /// <returns><see cref="IFluentAudioManager"/>, class that allows chaining.</returns>
        public IFluentAudioManager GetPlaybackPosition(out float time);

        /// <summary>
        /// <see cref="IAudioManager.SetPlaybackDirection"/>
        /// </summary>
        /// <returns><see cref="IFluentAudioManager"/>, class that allows chaining.</returns>
        public IFluentAudioManager SetPlaybackDirection(float pitch = Constants.DEFAULT_REVERSE_PITCH);

        /// <summary>
        /// <see cref="IAudioManager.ChangePitch"/>
        /// </summary>
        /// <returns><see cref="IFluentAudioManager"/>, class that allows chaining.</returns>
        public IFluentAudioManager ChangePitch(float minPitch, float maxPitch);

        /// <summary>
        /// <see cref="IAudioManager.GetClipLength"/>
        /// </summary>
        /// <returns><see cref="IFluentAudioManager"/>, class that allows chaining.</returns>
        public IFluentAudioManager GetClipLength(out double length);

        /// <summary>
        /// <see cref="IAudioManager.ToggleMute"/>
        /// </summary>
        /// <returns><see cref="IFluentAudioManager"/>, class that allows chaining.</returns>
        public IFluentAudioManager ToggleMute();

        /// <summary>
        /// <see cref="IAudioManager.TogglePause"/>
        /// </summary>
        /// <returns><see cref="IFluentAudioManager"/>, class that allows chaining.</returns>
        public IFluentAudioManager TogglePause();

        /// <summary>
        /// <see cref="IAudioManager.SubscribeSourceChanged"/>
        /// </summary>
        /// <returns><see cref="IFluentAudioManager"/>, class that allows chaining.</returns>
        public IFluentAudioManager SubscribeSourceChanged(SourceChangedCallback callback);

        /// <summary>
        /// <see cref="IAudioManager.UnsubscribeSourceChanged"/>
        /// </summary>
        /// <returns><see cref="IFluentAudioManager"/>, class that allows chaining.</returns>
        public IFluentAudioManager UnsubscribeSourceChanged(SourceChangedCallback callback);

        /// <summary>
        /// <see cref="IAudioManager.SubscribeProgressCoroutine"/>
        /// </summary>
        /// <returns><see cref="IFluentAudioManager"/>, class that allows chaining.</returns>
        public IFluentAudioManager SubscribeProgressCoroutine(float progress, ProgressCoroutineCallback callback);

        /// <summary>
        /// <see cref="IAudioManager.UnsubscribeProgressCoroutine"/>
        /// </summary>
        /// <returns><see cref="IFluentAudioManager"/>, class that allows chaining.</returns>
        public IFluentAudioManager UnsubscribeProgressCoroutine(float progress);

        /// <summary>
        /// <see cref="IAudioManager.GetProgress"/>
        /// </summary>
        /// <returns><see cref="IFluentAudioManager"/>, class that allows chaining.</returns>
        public IFluentAudioManager GetProgress(out float progress);

        /// <summary>
        /// <see cref="IAudioManager.TryGetSource"/>
        /// </summary>
        /// <returns><see cref="IFluentAudioManager"/>, class that allows chaining.</returns>
        public IFluentAudioManager TryGetSource(out AudioSourceWrapper source);

        /// <summary>
        /// <see cref="IAudioManager.LerpPitch"/>
        /// </summary>
        /// <returns><see cref="IFluentAudioManager"/>, class that allows chaining.</returns>
        public IFluentAudioManager LerpPitch(float endValue, float duration = Constants.DEFAULT_DURATION);

        /// <summary>
        /// <see cref="IAudioManager.LerpVolume"/>
        /// </summary>
        /// <returns><see cref="IFluentAudioManager"/>, class that allows chaining.</returns>
        public IFluentAudioManager LerpVolume(float endValue, float duration = Constants.DEFAULT_DURATION);

        /// <summary>
        /// <see cref="IAudioManager.ChangeGroupValue"/>
        /// </summary>
        /// <returns><see cref="IFluentAudioManager"/>, class that allows chaining.</returns>
        public IFluentAudioManager ChangeGroupValue(string exposedParameterName, float newValue);

        /// <summary>
        /// <see cref="IAudioManager.GetGroupValue"/>
        /// </summary>
        /// <returns><see cref="IFluentAudioManager"/>, class that allows chaining.</returns>
        public IFluentAudioManager GetGroupValue(string exposedParameterName, out float currentValue);

        /// <summary>
        /// <see cref="IAudioManager.ResetGroupValue"/>
        /// </summary>
        /// <returns><see cref="IFluentAudioManager"/>, class that allows chaining.</returns>
        public IFluentAudioManager ResetGroupValue(string exposedParameterName);

        /// <summary>
        /// <see cref="IAudioManager.LerpGroupValue"/>
        /// </summary>
        /// <returns><see cref="IFluentAudioManager"/>, class that allows chaining.</returns>
        public IFluentAudioManager LerpGroupValue(string exposedParameterName, float endValue, float duration = Constants.DEFAULT_DURATION);

        /// <summary>
        /// <see cref="IAudioManager.RemoveGroup"/>
        /// </summary>
        /// <returns><see cref="IFluentAudioManager"/>, class that allows chaining.</returns>
        public IFluentAudioManager RemoveGroup();

        /// <summary>
        /// <see cref="IAudioManager.AddGroup"/>
        /// </summary>
        /// <returns><see cref="IFluentAudioManager"/>, class that allows chaining.</returns>
        public IFluentAudioManager AddGroup(AudioMixerGroup mixerGroup);

        /// <summary>
        /// <see cref="IAudioManager.Set3DAudioOptions"/>
        /// </summary>
        /// <returns><see cref="IFluentAudioManager"/>, class that allows chaining.</returns>
        public IFluentAudioManager Set3DAudioOptions(float minDistance = Constants.DEFAULT_MIN_DISTANCE, float maxDistance = Constants.DEFAULT_MAX_DISTANCE, float spatialBlend = Constants.DEFAULT_BLEND, float spreadAngle = Constants.DEFAULT_ANGLE, float dopplerLevel = Constants.DEFAULT_DOPPLER, AudioRolloffMode rolloffMode = Constants.DEFAULT_MODE);

        /// <summary>
        /// <see cref="IAudioManager.SetStartTime"/>
        /// </summary>
        /// <returns><see cref="IFluentAudioManager"/>, class that allows chaining.</returns>
        public IFluentAudioManager SetStartTime(float startTime);

        /// <summary>
        /// <see cref="IAudioManager.SkipTime"/>
        /// </summary>
        /// <returns><see cref="IFluentAudioManager"/>, class that allows chaining.</returns>
        public IFluentAudioManager SkipTime(float time);
    }
}
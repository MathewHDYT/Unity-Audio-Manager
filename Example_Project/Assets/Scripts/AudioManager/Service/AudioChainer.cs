using AudioManager.Core;
using AudioManager.Helper;
using UnityEngine;
using UnityEngine.Audio;

namespace AudioManager.Service {
    public static class AudioChainer {
        private static FluentAudioManager m_instance;

        public static IFluentAudioManager AddSoundFromPath(IAudioManager am, string name, string audioPath, float volume = Constants.DEFAULT_VOLUME, float pitch = Constants.DEFAULT_PITCH, bool loop = Constants.DEFAULT_LOOP, AudioSource source = Constants.DEFAULT_SOURCE, AudioMixerGroup mixerGroup = Constants.DEFAULT_GROUP) {
            AudioError error = AudioHelper.ConvertToAudioError(am?.AddSoundFromPath(name, audioPath, volume, pitch, loop, source, mixerGroup));
            return am?.GetInstance(name, ChildType.PARENT, error);
        }

        public static IFluentAudioManager SelectSound(IAudioManager am, string name, ChildType child = Constants.DEFAULT_CHILD_TYPE) {
            return am?.GetInstance(name, child);
        }

        public static IFluentAudioManager RegisterChildAt3DPos(IAudioManager am, string name, Vector3 position) {
            ChildType child = ChildType.AT_3D_POS;
            AudioError error = AudioHelper.ConvertToAudioError(am?.RegisterChildAt3DPos(name, position, out child));
            return am?.GetInstance(name, child, error);
        }

        public static IFluentAudioManager RegisterChildAttachedToGo(IAudioManager am, string name, GameObject gameObject) {
            ChildType child = ChildType.ATTCHD_TO_GO;
            AudioError error = AudioHelper.ConvertToAudioError(am?.RegisterChildAttachedToGo(name, gameObject, out child));
            return am?.GetInstance(name, child, error);
        }

        //************************************************************************************************************************
        // Private Section
        //************************************************************************************************************************

        private static FluentAudioManager GetInstance(this IAudioManager audioManager, string name, ChildType child = Constants.DEFAULT_CHILD_TYPE, AudioError error = AudioError.OK) {
            if (m_instance is null) {
                m_instance = new FluentAudioManager(audioManager, name, child, error);
            }
            else {
                m_instance.ReuseInstance(audioManager, name, child, error);
            }
            return m_instance;
        }
    }
}

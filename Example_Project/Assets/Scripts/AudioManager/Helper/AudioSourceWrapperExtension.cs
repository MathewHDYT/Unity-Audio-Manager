using AudioManager.Core;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

namespace AudioManager.Helper {
    public static class AudioSourceWrapperExtension {
        public static AudioError InvokeChild(this AudioSourceWrapper source, ChildType child, InvokeCallback cb) {
            AudioError error = AudioError.OK;

            switch (child) {
                case ChildType.ALL:
                    foreach (var s in source.GetChildren()) {
                        cb?.Invoke(s);
                    }
                    goto case ChildType.PARENT;
                case ChildType.PARENT:
                    cb?.Invoke(source.Source);
                    break;
                case ChildType.AT_3D_POS:
                    goto case ChildType.ATTCHD_TO_GO;
                case ChildType.ATTCHD_TO_GO:
                    if (!source.TryGetRegisteredChild(child, out var childSource)) {
                        error = AudioError.INVALID_CHILD;
                        return error;
                    }
                    cb?.Invoke(childSource);
                    break;
                default:
                    // Unexpected ChildType argument.
                    break;
            }
            return error;
        }

        public static AudioError InvokeChild(this AudioSourceWrapper source, ChildType child, InvokeCallback<float> cb, out float value) {
            AudioError error = AudioError.OK;
            value = Constants.F_NULL_VALUE;

            if (cb is null) {
                return error;
            }

            IList<float> values = new List<float>();
            switch (child) {
                case ChildType.ALL:
                    foreach (var s in source.GetChildren()) {
                        values.Add(cb.Invoke(s));
                    }
                    goto case ChildType.PARENT;
                case ChildType.PARENT:
                    values.Add(cb.Invoke(source.Source));
                    break;
                case ChildType.AT_3D_POS:
                    goto case ChildType.ATTCHD_TO_GO;
                case ChildType.ATTCHD_TO_GO:
                    if (!source.TryGetRegisteredChild(child, out var childSource)) {
                        error = AudioError.INVALID_CHILD;
                        return error;
                    }
                    values.Add(cb.Invoke(childSource));
                    break;
                default:
                    // Unexpected ChildType argument.
                    break;
            }
            value = values.Count > 0 ? values.Average() : Constants.F_NULL_VALUE;
            return error;
        }

        public static AudioError InvokeChild(this AudioSourceWrapper source, ChildType child, InvokeCallback<double> cb, out double value) {
            AudioError error = AudioError.OK;
            value = Constants.D_NULL_VALUE;

            if (cb is null) {
                return error;
            }

            IList<double> values = new List<double>();
            switch (child) {
                case ChildType.ALL:
                    foreach (var s in source.GetChildren()) {
                        values.Add(cb.Invoke(s));
                    }
                    goto case ChildType.PARENT;
                case ChildType.PARENT:
                    values.Add(cb.Invoke(source.Source));
                    break;
                case ChildType.AT_3D_POS:
                    goto case ChildType.ATTCHD_TO_GO;
                case ChildType.ATTCHD_TO_GO:
                    if (!source.TryGetRegisteredChild(child, out var childSource)) {
                        error = AudioError.INVALID_CHILD;
                        return error;
                    }
                    values.Add(cb.Invoke(childSource));
                    break;
                default:
                    // Unexpected ChildType argument.
                    break;
            }
            value = values.Count > 0 ? values.Average() : Constants.D_NULL_VALUE;
            return error;
        }

        public static AudioError InvokeChild(this AudioSourceWrapper source, ChildType child, InvokeCallback<bool> cb, out bool value) {
            AudioError error = AudioError.OK;
            value = false;

            if (cb is null) {
                return error;
            }

            switch (child) {
                case ChildType.ALL:
                    foreach (var s in source.GetChildren()) {
                        value = value || cb.Invoke(s);
                    }
                    goto case ChildType.PARENT;
                case ChildType.PARENT:
                    value = value || cb.Invoke(source.Source);
                    break;
                case ChildType.AT_3D_POS:
                    goto case ChildType.ATTCHD_TO_GO;
                case ChildType.ATTCHD_TO_GO:
                    if (!source.TryGetRegisteredChild(child, out var childSource)) {
                        error = AudioError.INVALID_CHILD;
                        return error;
                    }
                    value = value || cb.Invoke(childSource);
                    break;
                default:
                    // Unexpected ChildType argument.
                    break;
            }
            return error;
        }

        public static AudioError GetChildSource(this AudioSourceWrapper source, ChildType child, out AudioSource childSource) {
            AudioError error = AudioError.OK;
            childSource = null;

            switch (child) {
                case ChildType.ALL:
                    break;
                case ChildType.PARENT:
                    childSource = source.Source;
                    break;
                case ChildType.AT_3D_POS:
                    goto case ChildType.ATTCHD_TO_GO;
                case ChildType.ATTCHD_TO_GO:
                    if (!source.TryGetRegisteredChild(child, out childSource)) {
                        error = AudioError.INVALID_CHILD;
                        return error;
                    }
                    break;
                default:
                    // Unexpected ChildType argument.
                    break;
            }
            return error;
        }

        public static AudioError TryGetGroupValue(this AudioSourceWrapper source, string exposedParameterName, out float currentValue) {
            AudioError error = AudioError.OK;
            if (!source.Mixer.GetFloat(exposedParameterName, out currentValue)) {
                error = AudioError.MIXER_NOT_EXPOSED;
            }
            return error;
        }

        public static AudioError TrySetGroupValue(this AudioSourceWrapper source, string exposedParameterName, float newValue) {
            AudioError error = AudioError.OK;
            if (!source.Mixer.SetFloat(exposedParameterName, newValue)) {
                error = AudioError.MIXER_NOT_EXPOSED;
            }
            return error;
        }

        public static AudioError TryClearGroupValue(this AudioSourceWrapper source, string exposedParameterName) {
            AudioError error = AudioError.OK;
            if (!source.Mixer.ClearFloat(exposedParameterName)) {
                error = AudioError.MIXER_NOT_EXPOSED;
            }
            return error;
        }

        public static AudioError IsSoundValid(this AudioSourceWrapper source) {
            AudioError error = AudioError.OK;
            
            if (source is null) {
                error = AudioError.MISSING_WRAPPER;
            }
            else if (source.Source is null) {
                error = AudioError.MISSING_SOURCE;
            }
            else if (source.Source.clip is null) {
                error = AudioError.MISSING_CLIP;
            }
            return error;
        }

        // Originally from John Leonard French's blog with an article about different methods to fade audio in and out.
        // See https://johnleonardfrench.com/how-to-fade-audio-in-unity-i-tested-every-method-this-ones-the-best/.
        public static IEnumerator LerpValueCoroutine(this AudioSourceWrapper source, float currValue, float endValue, float duration, SetCallback<float> cb) {
            float time = 0;

            while (time <= duration) {
                time += Time.deltaTime;
                cb?.Invoke(Mathf.Lerp(currValue, endValue, time / duration), source);
                yield return null;
            }
        }

        // Originally from John Leonard French's blog with an article about different methods to fade audio in and out.
        // See https://johnleonardfrench.com/how-to-fade-audio-in-unity-i-tested-every-method-this-ones-the-best/.
        public static IEnumerator LerpGroupValueCoroutine(this AudioSourceWrapper source, string exposedParameterName, float endValue, float duration) {
            float time = 0;

            source.Mixer.GetFloat(exposedParameterName, out float currValue);

            while (time <= duration) {
                time += Time.deltaTime;
                float newValue = Mathf.Lerp(currValue, endValue, time / duration);
                source.Mixer.SetFloat(exposedParameterName, newValue);
                yield return null;
            }
        }
    }
}

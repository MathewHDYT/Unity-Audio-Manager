---
layout: default
title: Change Group Value
parent: AudioMixer
grand_parent: Documentation
---

## Change Group Value
**What it does:**
Changes the value of the given exposed parameter for the complete [```AudioMixerGroup```](https://docs.unity3d.com/ScriptReference/Audio.AudioMixerGroup.html) of the given sound to the given newValue
and returns an AudioError (see [Possible Errors](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)), showing wheter and how changing the value of the given exposed parameter failed.

[**Possible Errors:**](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)
- DOES_NOT_EXIST
- MISSING_WRAPPER
- MISSING_SOURCE
- MISSING_CLIP
- MISSING_MIXER_GROUP
- MIXER_NOT_EXPOSED

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to change the [```AudioMixerGroup```](https://docs.unity3d.com/ScriptReference/Audio.AudioMixerGroup.html) parameter on
- ```ExposedParameterName``` is the name we have given the exposed parameter on the [```AudioMixer```](https://docs.unity3d.com/ScriptReference/Audio.AudioMixer.html)
- ```NewValue``` is the value we want to set the exposed parameter to

```csharp
string soundName = "SoundName";
string exposedParameterName = "Volume";
float newValue = 0.0001f;
newValue = Mathf.Log10(newValue) * 20; // Transforms a range from 0.0001 to 1 in linear scale into -80 to 0 in logarithmic scale.

AudioError error = am.ChangeGroupValue(soundName, exposedParameterName, newValue);
if (error != AudioError.OK) {
    Debug.Log("Changing AudioMixerGroup exposed parameter with the name " + exposedParameterName + " on the sound called: " + soundName + " failed with error id: " + err);
}
else {
    Debug.Log("Changing AudioMixerGroup exposed parameter with the name " + exposedParameterName + " on the sound called: " + soundName + " with the endValue: " + endValue.ToString("0.00") + " succesfull");
}
```

**When to use it:**
When you want to change an exposed parameter (for example the volume or pitch) for the complete [```AudioMixerGroup```](https://docs.unity3d.com/ScriptReference/Audio.AudioMixerGroup.html) the sound is connected to.

**Remarks:**
See [```AudioMixer.SetFloat```](https://docs.unity3d.com/ScriptReference/Audio.AudioMixer.SetFloat.html) for more details on what change group value does.
Be aware that all values of an [```AudioMixerGroup```](https://docs.unity3d.com/ScriptReference/Audio.AudioMixerGroup.html) work on a logarithmic scale so to accurately change the value like expected use ```Mathf.Log10()``` on the value you pass beforehand.
For example if we pass a value of 0.5 in a scale from 0 to 1, we expect the value to only have 50% of its maximum value. This is only the case with a linear scale if we have a logarithmic scale instead a value of 0.5 in a scale from 0 to 1 will result in the value still having around 92.5% of its maximum value.

---
layout: default
title: Lerp Group Value
parent: AudioMixer
grand_parent: Documentation
---

## Lerp Group Value
**What it does:**
Lerps the value of the given exposed parameter for the complete [```AudioMixerGroup```](https://docs.unity3d.com/ScriptReference/Audio.AudioMixerGroup.html) of the given sound over the given amount of time and returns an AudioError (see [Possible Errors](https://mathewhdyt.github.io/Unity-Audio-Manager/#possible-errors)), showing wheter and how lerping the value of the given exposed parameter failed.

[**Possible Errors:**](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)
- DOES_NOT_EXIST
- MISSING_WRAPPER
- MISSING_SOURCE
- MISSING_CLIP
- MISSING_MIXER_GROUP
- MISSING_PARENT
- MIXER_NOT_EXPOSED
- INVALID_END_VALUE

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to reset the [```AudioMixerGroup```](https://docs.unity3d.com/ScriptReference/Audio.AudioMixerGroup.html) parameter on
- ```ExposedParameterName``` is the name we have given the exposed parameter on the [```AudioMixer```](https://docs.unity3d.com/ScriptReference/Audio.AudioMixer.html)
- ```EndValue``` is the value the exposed parameter should have at the end
- ```Duration``` defines the total amount of time needed to achieve the given ```endValue```

```csharp
string soundName = "SoundName";
string exposedParameterName = "Volume";
float endValue = 0.0001f;
endValue = Mathf.Log10(endValue) * 20; // Transforms a range from 0.0001 to 1 in linear scale into -80 to 0 in logarithmic scale.
float duration = 1f;

AudioError err = am.LerpGroupValue(soundName, exposedParameterName, endValue, duration);
if (error != AudioError.OK) {
    Debug.Log("Lerping AudioMixerGroup exposed parameter with the name " + exposedParameterName + " on the sound called: " + soundName + " failed with error id: " + err);
}
else {
    Debug.Log("Lerping AudioMixerGroup exposed parameter with the name " + exposedParameterName + " on the sound called: " + soundName + " in the time: " + duration.ToString("0.00") + " seconds with the endValue: " + endValue.ToString("0.00") + " and the granularity: " + granularity.ToString("0.00") + " succesfull");
}
```

Alternatively you can call the methods with less paramters as some of them have default arguments.

```csharp
string soundName = "SoundName";
string exposedParameterName = "Volume";
float endValue = 0.0001f;
endValue = Mathf.Log10(endValue) * 20; // Transforms a range from 0.0001 to 1 in linear scale into -80 to 0 in logarithmic scale.

AudioError err = am.LerpGroupValue(soundName, exposedParameterName, endValue);
if (err != AudioError.OK) {
    Debug.Log("Lerping AudioMixerGroup exposed parameter with the name " + exposedParameterName + " on the sound called: " + soundName + " failed with error id: " + err);
}
else {
    Debug.Log("Lerping AudioMixerGroup exposed parameter with the name " + exposedParameterName + " on the sound called: " + soundName + " to the endValue: " + endValue.ToString("0.00") + " succesfull");
}
```

**When to use it:**
When you want to lerp an exposed parameter (for example the volume or pitch) for the complete [```AudioMixerGroup```](https://docs.unity3d.com/ScriptReference/Audio.AudioMixerGroup.html) the sound is connected to.

**Remarks:**
Produces a more smooth result than [```LerpVolume```](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/audiosource/lerp_volume/) or [```LerpPitch```](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/audiosource/lerp_pitch/) because of additonaly interpolation between frames applied by the [```AudioMixer```](https://docs.unity3d.com/ScriptReference/Audio.AudioMixer.html).
Be aware that all values of an [```AudioMixerGroup```](https://docs.unity3d.com/ScriptReference/Audio.AudioMixerGroup.html) work on a logarithmic scale so to accurately change the value like expected use ```Mathf.Log10()``` on the value you pass beforehand.
For example if we pass a value of 0.5 in a scale from 0 to 1, we expect the value to only have 50% of its maximum value. This is only the case with a linear scale if we have a logarithmic scale instead a value of 0.5 in a scale from 0 to 1 will result in the value still being around 92.5% of its maximum value.

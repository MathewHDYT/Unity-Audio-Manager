---
layout: default
title: Get Group Value
parent: AudioMixer
grand_parent: Documentation
---

## Get Group Value
**What it does:**
Returns the current value of the given exposed parameter and returns an AudioError (see [Possible Errors](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)), showing wheter and how getting the value of the given exposed parameter failed.

[**Possible Errors:**](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)
- DOES_NOT_EXIST
- MISSING_SOURCE
- MISSING_CLIP
- MISSING_MIXER_GROUP
- MIXER_NOT_EXPOSED

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to get the [```AudioMixerGroup```](https://docs.unity3d.com/ScriptReference/Audio.AudioMixerGroup.html) parameter on
- ```ExposedParameterName``` is the name we have given the exposed parameter on the [```AudioMixer```](https://docs.unity3d.com/ScriptReference/Audio.AudioMixer.html)

```csharp
string soundName = "SoundName";
string exposedParameterName = "Volume";

AudioError error = am.GetGroupValue(soundName, exposedParameterName, out float currentValue);
if (error != AudioError.OK) {
    Debug.Log("Getting AudioMixerGroup volume of the sound called: " + soundName + " failed with error id: " + error);
}
else {
    Debug.Log("Getting AudioMixerGroup exposed parameter with the name " + exposedParameterName + " on the sound called: " + soundName + " with the current value being: " + currentValue.ToString("0.00") + " succesfull");
}
```

**When to use it:**
When you want to get an exposed parameter (for example the volume or pitch) for the complete [```AudioMixerGroup```](https://docs.unity3d.com/ScriptReference/Audio.AudioMixerGroup.html) the sound is connected to.

**Remarks:**
See [```AudioMixer.GetFloat```](https://docs.unity3d.com/ScriptReference/Audio.AudioMixer.GetFloat.html) for more details on what get group value does.

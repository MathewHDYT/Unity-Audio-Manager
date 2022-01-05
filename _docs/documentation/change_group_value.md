---
layout: default
title: Change Group Value
parent: Documentation
---

## Change Group Value
**What it does:**
Changes the value of the given exposed parameter for the complete [```AudioMixerGroup```](https://docs.unity3d.com/2021.2/Documentation/ScriptReference/Audio.AudioMixerGroup.html) of a given sound to the given newValue and returns an AudioError (see [Possible Errors](https://mathewhdyt.github.io/Unity-Audio-Manager/#possible-errors)), showing wheter and how changing the value of the given exposed parameter failed.

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to change the [```AudioMixerGroup```](https://docs.unity3d.com/2021.2/Documentation/ScriptReference/Audio.AudioMixerGroup.html) parameter on
- ```ExposedParameterName``` is the name we have given the exposed parameter on the [```AudioMixer```](https://docs.unity3d.com/2021.2/Documentation/ScriptReference/Audio.AudioMixer.html)
- ```NewValue``` is the value we want to set the exposed parameter to

```csharp
string soundName = "SoundName";
string exposedParameterName = "Volume";
float newValue = -80f;

AudioManager.AudioError error = am.ChangeGroupValue(soundName, exposedParameterName, newValue);
if (error != AudioManager.AudioError.OK) {
    Debug.Log("Changing AudioMixerGroup exposed parameter with the name " + exposedParameterName + " on the sound called: " + soundName + " failed with error id: " + err);
}
else {
    Debug.Log("Changing AudioMixerGroup exposed parameter with the name " + exposedParameterName + " on the sound called: " + soundName + " with the endValue: " + endValue.ToString("0.00") + " succesfull");
}
```

**When to use it:**
When you want to change an exposed parameter (for example the volume or pitch) for the complete [```AudioMixerGroup```](https://docs.unity3d.com/2021.2/Documentation/ScriptReference/Audio.AudioMixerGroup.html) the sound is connected to.
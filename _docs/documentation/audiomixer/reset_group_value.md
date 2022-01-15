---
layout: default
title: Reset Group Value
parent: AudioMixer
grand_parent: Documentation
---

## Reset Group Value
**What it does:**
Reset the value of the given exposed parameter for the complete [```AudioMixerGroup```](https://docs.unity3d.com/2021.2/Documentation/ScriptReference/Audio.AudioMixerGroup.html) of a given sound to the default value and returns an AudioError (see [Possible Errors](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)), showing wheter and how resetting the value of the given exposed parameter failed.

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to reset the [```AudioMixerGroup```](https://docs.unity3d.com/2021.2/Documentation/ScriptReference/Audio.AudioMixerGroup.html) parameter on
- ```ExposedParameterName``` is the name we have given the exposed parameter on the [```AudioMixer```](https://docs.unity3d.com/2021.2/Documentation/ScriptReference/Audio.AudioMixer.html)

```csharp
string soundName = "SoundName";
string exposedParameterName = "Volume";

AudioError error = am.ResetGroupValue(soundName, exposedParameterName);
if (error != AudioManager.AudioError.OK) {
    Debug.Log("Resetting AudioMixerGroup exposed parameter with the name " + exposedParameterName + " on the sound called: " + soundName + " failed with error id: " + err);
}
else {
    Debug.Log("Resetting AudioMixerGroup exposed parameter with the name " + exposedParameterName + " on the sound called: " + soundName + " succesfull");
}
```

**When to use it:**
When you want to reset an exposed parameter (for example the volume or pitch) for the complete [```AudioMixerGroup```](https://docs.unity3d.com/2021.2/Documentation/ScriptReference/Audio.AudioMixerGroup.html) the sound is connected to.

**Remarks:**
See [```AudioMixer.ClearFloat```](https://docs.unity3d.com/2019.1/Documentation/ScriptReference/Audio.AudioMixer.ClearFloat.html) for more details on what reset group value does.

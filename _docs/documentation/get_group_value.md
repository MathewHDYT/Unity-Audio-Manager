---
layout: default
title: Get Group Value
parent: Documentation
---

## Get Group Value
**What it does:**
Returns an instance of the ValueDataError class, where the value (gettable with ```Value```), is the current value of the given exposed parameter and where the error (gettable with ```Error```) is an integer representing the AudioError Enum (see [Possible Errors](https://mathewhdyt.github.io/Unity-Audio-Manager/#possible-errors)), showing wheter and how getting the value of the given exposed parameter failed.

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to get the [```AudioMixerGroup```](https://docs.unity3d.com/2021.2/Documentation/ScriptReference/Audio.AudioMixerGroup.html) parameter on
- ```ExposedParameterName``` is the name we have given the exposed parameter on the [```AudioMixer```](https://docs.unity3d.com/2021.2/Documentation/ScriptReference/Audio.AudioMixer.html)

```csharp
string soundName = "SoundName";
string exposedParameterName = "Volume";

ValueDataError<float> valueDataError = am.GetGroupValue(soundName, exposedParameterName);
if (valueDataError.Error != (int)AudioManager.AudioError.OK) {
    Debug.Log("Getting AudioMixerGroup volume of the sound called: " + soundName + " failed with error id: " + valueDataError.Error);
}
else {
    Debug.Log("Getting AudioMixerGroup exposed parameter with the name " + exposedParameterName + " on the sound called: " + soundName + " with the current value being: " + valueDataError.Value.ToString("0.00") + " succesfull");
}
```

**When to use it:**
When you want to get an exposed parameter (for example the volume or pitch) for the complete [```AudioMixerGroup```](https://docs.unity3d.com/2021.2/Documentation/ScriptReference/Audio.AudioMixerGroup.html) the sound is connected to.
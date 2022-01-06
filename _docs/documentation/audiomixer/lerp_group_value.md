---
layout: default
title: Lerp Group Value
parent: AudioSource
grand_parent: Documentation
---

## Lerp Group Value
**What it does:**
Lerps the value of the given exposed parameter for the complete [```AudioMixerGroup```](https://docs.unity3d.com/2021.2/Documentation/ScriptReference/Audio.AudioMixerGroup.html) of a given sound over a given amount of time and returns an AudioError (see [Possible Errors](https://mathewhdyt.github.io/Unity-Audio-Manager/#possible-errors)), showing wheter and how lerping the value of the given exposed parameter failed.

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to reset the [```AudioMixerGroup```](https://docs.unity3d.com/2021.2/Documentation/ScriptReference/Audio.AudioMixerGroup.html) parameter on
- ```ExposedParameterName``` is the name we have given the exposed parameter on the [```AudioMixer```](https://docs.unity3d.com/2021.2/Documentation/ScriptReference/Audio.AudioMixer.html)
- ```EndValue``` is the value the exposed parameter should have at the end
- ```WaitTime``` defines the total amount of time needed to achieve the given ```endValue```
- ```Granularity``` is the amount of steps in which we decrease the volume to the ```endValue```

```csharp
string soundName = "SoundName";
string exposedParameterName = "Volume";
float endValue = -80f;
float waitTime = 1f;
float granularity = 2f;

AudioManager.AudioError err = am.LerpGroupValue(soundName, exposedParameterName, endValue, waitTime, granularity);
if (error != AudioManager.AudioError.OK) {
    Debug.Log("Lerping AudioMixerGroup exposed parameter with the name " + exposedParameterName + " on the sound called: " + soundName + " failed with error id: " + err);
}
else {
    Debug.Log("Lerping AudioMixerGroup exposed parameter with the name " + exposedParameterName + " on the sound called: " + soundName + " in the time: " + waitTime.ToString("0.00") + " seconds with the endValue: " + endValue.ToString("0.00") + " and the granularity: " + granularity.ToString("0.00") + " succesfull");
}
```

Alternatively you can call the methods with less paramters as some of them have default arguments.

```csharp
string soundName = "SoundName";
string exposedParameterName = "Volume";
float endValue = -80f;

AudioManager.AudioError err = am.LerpGroupValue(soundName, exposedParameterName, endValue);
if (err != AudioManager.AudioError.OK) {
    Debug.Log("Lerping AudioMixerGroup exposed parameter with the name " + exposedParameterName + " on the sound called: " + soundName + " failed with error id: " + err);
}
else {
    Debug.Log("Lerping AudioMixerGroup exposed parameter with the name " + exposedParameterName + " on the sound called: " + soundName + " to the endValue: " + endValue.ToString("0.00") + " succesfull");
}
```

**When to use it:**
When you want to lerp an exposed parameter (for example the volume or pitch) for the complete [```AudioMixerGroup```](https://docs.unity3d.com/2021.2/Documentation/ScriptReference/Audio.AudioMixerGroup.html) the sound is connected to.

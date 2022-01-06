---
layout: default
title: Lerp Volume
parent: AudioSource
---

## Lerp Volume
**What it does:**
Lerps the ```volume``` of a given sound over a given amount of time and returns an AudioError (see [Possible Errors](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)), showing wheter and how lerping the volume of the given sound failed.

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to change the volume from
- ```EndValue``` (0 - 1) is the value the ```volume``` should have at the end
- ```WaitTime``` defines the total amount of time needed to achieve the given ```endValue```
- ```Granularity``` is the amount of steps in which we decrease the volume to the ```endValue```

```csharp
string soundName = "SoundName";
float endValue = 0.8f;
float waitTime = 1f;
float granularity = 2f;

AudioManager.AudioError err = am.LerpVolume(soundName, endValue, waitTime, granularity);
if (err != AudioManager.AudioError.OK) {
    Debug.Log("Lerping volume of the sound called: " + soundName + " failed with error id: " + err);
}
else {
    Debug.Log("Lerping volume of the sound called: " + soundName + " in the time: " + waitTime.ToString("0.00") + " seconds with the endValue: " + endValue.ToString("0.00") + " and the granularity: " + granularity.ToString("0.00") + " succesfull");
}
```

Alternatively you can call the methods with less paramters as some of them have default arguments.

```csharp
string soundName = "SoundName";
float endValue = 0.8f;

AudioManager.AudioError err = am.LerpVolume(soundName, endValue);
if (err != AudioManager.AudioError.OK) {
    Debug.Log("Lerping volume of the sound called: " + soundName + " failed with error id: " + err);
}
else {
    Debug.Log("Lerping volume of the sound called: " + soundName + " to the endValue: " + endValue.ToString("0.00") + " succesfull");
}
```

**When to use it:**
When you want to decrease -or increase the ```volume``` over a given amount of time.
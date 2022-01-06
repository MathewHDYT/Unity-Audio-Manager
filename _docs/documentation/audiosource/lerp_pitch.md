---
layout: default
title: Lerp Pitch
parent: AudioSource
grand_parent: Documentation
---

## Lerp Pitch
**What it does:**
Lerps the ```pitch``` of a sound over a given amount of time and returns an AudioError (see [Possible Errors](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)), showing wheter and how lerping the pitch of the given sound failed.

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to change the pitch from
- ```EndValue``` (0.1 - 3) is the value the ```pitch``` should have at the end
- ```WaitTime``` defines the total amount of time needed to achieve the given ```endValue```
- ```Granularity``` is the amount of steps in which we decrease the volume to the ```endValue```

```csharp
string soundName = "SoundName";
float endValue = 0.8f;
float waitTime = 1f;
float granularity = 2f;

AudioManager.AudioError err = am.LerpPitch(soundName, endValue, waitTime, granularity);
if (err != AudioManager.AudioError.OK) {
    Debug.Log("Lerping pitch of the sound called: " + soundName + " failed with error id: " + err);
}
else {
    Debug.Log("Lerping pitch of the sound called: " + soundName + " in the time: " + waitTime.ToString("0.00") + " seconds with the endValue: " + endValue.ToString("0.00") + " and the granularity: " + granularity.ToString("0.00") + " succesfull");
}
```

Alternatively you can call the methods with less paramters as some of them have default arguments.

```csharp
string soundName = "SoundName";
float endValue = 0.8f;

AudioManager.AudioError err = am.LerpPitch(soundName, endValue);
if (err != AudioManager.AudioError.OK) {
    Debug.Log("Lerping pitch of the sound called: " + soundName + " failed with error id: " + err);
}
else {
    Debug.Log("Lerping pitch of the sound called: " + soundName + " to the endValue: " + endValue.ToString("0.00") + " succesfull");
}
```

**When to use it:**
When you want to decrease -or increase the ```pitch``` over a given amount of time.

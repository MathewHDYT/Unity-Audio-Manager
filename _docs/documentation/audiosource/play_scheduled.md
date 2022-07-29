---
layout: default
title: Play Scheduled
parent: AudioSource
grand_parent: Documentation
---

## Play Scheduled
**What it does:**
Starts playing the sound after the given amount of time with additional buffer time to fetch the data from media
and returns an AudioError (see [Possible Errors](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)), showing wheter and how playing the sound after the given amount of time failed.

[**Possible Errors:**](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)
- DOES_NOT_EXIST
- MISSING_WRAPPER
- MISSING_SOURCE
- MISSING_CLIP

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to play after the given amount of time which would be the 10 seconds we've defined
- ```Delay``` is the time after which we want to start playing the sound
- ```Child``` is the [```ChildType```](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-children) that we want to call this method on

```csharp
string soundName = "SoundName";
double delay = 10d;
ChildType child = ChildType.PARENT;

AudioError err = am.PlayScheduled(soundName, delay, child);
if (err != AudioError.OK) {
    Debug.Log("Playing sound called: " + soundName + " after " + delay.ToString("0.00") + " failed with error id: " + err);
}
else {
    Debug.Log("Playing sound called: " + soundName + " after " + delay.ToString("0.00") + " seconds succesfull");
}
```

Alternatively you can call the methods with less paramters as some of them have default arguments.

```csharp
string soundName = "SoundName";
double delay = 10d;

AudioError err = am.PlayScheduled(soundName, delay);
if (err != AudioError.OK) {
    Debug.Log("Playing sound called: " + soundName + " after " + delay.ToString("0.00") + " failed with error id: " + err);
}
else {
    Debug.Log("Playing sound called: " + soundName + " after " + delay.ToString("0.00") + " seconds succesfull");
}
```

**When to use it:**
When you want to switch smoothly between sounds, because the method it is independent of the frame rate and gives the audio system enough time to prepare the playback of the sound to fetch it from media, where the opening and buffering takes a lot of time (streams) without causing sudden CPU spikes.

**Remarks:**
See [```AudioSource.PlayScheduled```](https://docs.unity3d.com/ScriptReference/AudioSource.PlayScheduled.html) for more details on what play scheduled does.

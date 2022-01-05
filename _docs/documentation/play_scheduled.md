---
layout: default
title: Play Scheduled
parent: Documentation
---

## Play Scheduled
**What it does:**
Starts playing the sound after the given amount of time with additional buffer time to fetch the data from media and returns an AudioError (see [Possible Errors](https://mathewhdyt.github.io/Unity-Audio-Manager/#possible-errors)), showing wheter and how playing the sound after the given amount of time failed.

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to play after the given amount of time which would be the 10 seconds we've defined
- ```Delay``` is the time after which we want to start playing the sound

```csharp
string soundName = "SoundName";
double delay = 10d;

AudioManager.AudioError err = am.PlayScheduled(soundName, delay);
if (err != AudioManager.AudioError.OK) {
    Debug.Log("Playing sound called: " + soundName + " after " + delay.ToString("0.00") + " failed with error id: " + err);
}
else {
    Debug.Log("Playing sound called: " + soundName + " after " + delay.ToString("0.00") + " seconds succesfull");
}
```

**When to use it:**
When you want to switch smoothly between sounds, because the method it is independent of the frame rate and gives the audio system enough time to prepare the playback of the sound to fetch it from media, where the opening and buffering takes a lot of time (streams) without causing sudden CPU spikes.

See [```AudioSource.PlayScheduled```](https://docs.unity3d.com/2021.2/Documentation/ScriptReference/AudioSource.PlayScheduled.html) for more details on what play scheduled does.

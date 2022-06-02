---
layout: default
title: Skip Forward
parent: AudioSource
grand_parent: Documentation
---

## Skip Forward
**What it does:**
Skips the given sound forward for the given amount of ```time``` in seconds in the clip timeline to maximum the end of the song and returns an AudioError (see [Possible Errors](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)), showing wheter and how skipping backwards the current time failed.

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to play
- ```Time``` is the amount of time in seconds we want to skip forward

```csharp
string soundName = "SoundName";
float time = 1f;

AudioError err = am.SkipForward(soundName, time);
if (err != AudioError.OK) {
    Debug.Log("Skipping forward time for the sound called: " + soundName + " by the value: " + time.ToString("0.00") + " failed with error id: " + err);
}
else {
    Debug.Log("Skipping forward time for the sound called: " + soundName + " by the value: " + time.ToString("0.00") + " succesfull");
}
```

**When to use it:**
When you want to skip forward the clip timeline by a given amount of time. Can be used to create sub-looping sections if we ```SkipForward``` 10 seconds every 10 seconds in a Coroutine, when the song is played backward (see [SetPlaybackDirection](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/audiosource/set_playback_direction/)).

---
layout: default
title: Play At Time Stamp
parent: AudioSource
grand_parent: Documentation
---

## Play At Time Stamp
**What it does:**
Start playing the choosen sound at the given ```startTime``` and returns an AudioError (see [Possible Errors](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)), showing wheter and how playing the sound from the given ```startTime``` failed.

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to play
- ```StartTime``` is the moment we want to play the sound at so instead of starting at 0 seconds we start at 10 seconds

```csharp
string soundName = "SoundName";
float startTime = 10f;

AudioManager.AudioError err = am.PlayAtTimeStamp(soundName, startTime);
if (err != AudioManager.AudioError.OK) {
    Debug.Log("Playing sound called: " + soundName + " at startTime: " + startTime.ToString("0.00") + " failed with error id: " + err);
}
else {
    Debug.Log("Playing sound called: " + soundName + " at startTime: " + startTime.ToString("0.00") + " succesfull");
}
```

**When to use it:**
When you want to play a sound but skip a portion at the start. Could be used if only the second part of your sound is high intesity and normally you want to build up the intensity, but not when the game is in a special state.

**Remarks:**
Be aware [```PlayAtTimeStamp```](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/audiosource/play_at_time_stamp/) is incompatible with [```TogglePause```](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/audiosource/toggle_pause/), because the given startTime for the song wil reset once [```AudioSource.isPlaying```](https://docs.unity3d.com/2021.2/Documentation/ScriptReference/AudioSource-isPlaying.html) is false.

This is the case if the song is stopped or paused, meaning pausing and then unpausing the song with [```TogglePause```](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/audiosource/toggle_pause/), will start the song anew instead of continuing at the time we paused.

To circumvent this use the [```SetStartTime```](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/audiosource/set_start_time/) method, which does not automatically reset the startTime and then reset it yourself via. the [```AudioSource```](https://docs.unity3d.com/2021.2/Documentation/ScriptReference/AudioSource.html) component which you can get with [```TryGetSource```](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/audiosource/try_get_source/) method and the ```time``` variable on that [```AudioSource```](https://docs.unity3d.com/2021.2/Documentation/ScriptReference/AudioSource-time.html).

---
layout: default
title: Play At 3D Position
parent: AudioSource
grand_parent: Documentation
---

## Play At 3D Position
**What it does:**
Starts playing the choosen sound at a given 3D position and returns an AudioError (see [Possible Errors](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)), showing wheter and how playing the sound at the given position failed.

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to play
- ```WorldPosition``` is the 3D position in world space we want the sound to be emitting from

```csharp
string soundName = "SoundName";
Vector3 worldPosition = new Vector3(10f, 10f, 0f);

AudioError err = am.PlayAt3DPosition(soundName, worldPosition);
if (err != AudioManager.AudioError.OK) {
    Debug.Log("Playing sound called: " + soundName + " at the position x " + worldPosition.x.ToString("0.00") + " and y " + worldPosition.y.ToString("0.00") + " failed with error id: " + err);
}
else {
    Debug.Log("Playing sound called: " + soundName + " at the position x " + worldPosition.x.ToString("0.00") + " and y " + worldPosition.y.ToString("0.00") + " succesfull");
}
```

**When to use it:**
When you want to play a sound directly from a 3D position and make the ```volume``` be influenced by the distance the player has from that position.

**Remarks:**
To use either [```PlayAt3DPosition```](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/audiosource/play_at_3d_position/) or [```PlayAttachedToGameObject```](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/audiosource/play_attached_to_gameobject/) the sound we want to play with it has to have 3D enabled. Either use [```Set3DAudioOptions```](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/audiosource/set_3d_audio_options/) to set the needed settings via. code or make the sound 3D capable with the ```Spatial Blend``` parameter on the ```AudioSourceSetting```.


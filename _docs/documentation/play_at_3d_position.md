---
layout: default
title: Play At 3D Position
parent: Documentation
---

## Play At 3D Position
**What it does:**
Starts playing the choosen sound at a given 3D position and returns an AudioError (see [Possible Errors](https://mathewhdyt.github.io/Unity-Audio-Manager/#possible-errors)), showing wheter and how playing the sound at the given position failed.

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to play
- ```WorldPosition``` is the 3D position in world space we want the sound to be emitting from
- ```MinDistance``` is the distance the sound will not get louder at
- ```MaxDistance``` is the distance the sound will still be hearable at
- ```Spread``` is the angle the sound will be emitted at in degrees
- ```SpatialBlend``` defines how much the sound is affected by 3D (0f = 2D, 1f = 3D)
- ```DopplerLevel``` defines the doppler scale for our sound
- ```RolloffMode``` defines how the sound should decline in ```volume``` between the min and max distance

```csharp
string soundName = "SoundName";
Vector3 worldPosition = new Vector3(10f, 10f, 0f);
float minDistance = 5f;
float maxDistance = 15f;
float spread = 0f;
float spatialBlend = 1f;
float dopplerLevel = 1f;
AudioRolloffMode rolloffMode = AudioRolloffMode.Linear;

AudioManager.AudioError err = am.PlayAt3DPosition(soundName, worldPosition, minDistance, maxDistance, spread, spatialBlend, dopplerLevel, rolloffMode);
if (err != AudioManager.AudioError.OK) {
    Debug.Log("Playing sound called: " + soundName + " at the position x " + worldPosition.x.ToString("0.00") + " and y " + worldPosition.y.ToString("0.00") + " failed with error id: " + err);
}
else {
    Debug.Log("Playing sound called: " + soundName + " at the position x " + worldPosition.x.ToString("0.00") + " and y " + worldPosition.y.ToString("0.00") + " succesfull");
}
```

Alternatively you can call the methods with less paramters as some of them have default arguments.

```csharp
string soundName = "SoundName";
Vector3 worldPosition = new Vector3(10f, 10f, 0f);
float minDistance = 5f;
float maxDistance = 15f;

AudioManager.AudioError err = am.PlayAt3DPosition(soundName, worldPosition, minDistance, maxDistance);
if (err != AudioManager.AudioError.OK) {
    Debug.Log("Playing sound called: " + soundName + " at the position x " + worldPosition.x.ToString("0.00") + " and y " + worldPosition.y.ToString("0.00") + " failed with error id: " + err);
}
else {
    Debug.Log("Playing sound called: " + soundName + " at the position x " + worldPosition.x.ToString("0.00") + " and y " + worldPosition.y.ToString("0.00") + " succesfull");
}
```

**When to use it:**
When you want to play a sound directly from a 3D position and make the ```volume``` be influenced by the distance the player has from that position.

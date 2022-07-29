---
layout: default
title: Register Child At 3D Position
parent: AudioSource
grand_parent: Documentation
---

## Register Child At 3D Position
**What it does:**
Registers a new child sound at the given 3D position, so it can later be referenced via. the corresponding ```ChildType``` value and returns an AudioError (see [Possible Errors](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)), showing wheter and how registering the child sound at the given 3D position failed.

[**Possible Errors:**](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)
- DOES_NOT_EXIST
- MISSING_WRAPPER
- MISSING_SOURCE
- MISSING_CLIP
- CAN_NOT_BE_3D
- INVALID_PARENT

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to play
- ```Position``` is the 3D position in world space we want the sound to be emitting from
- ```Child``` is the variable the [```ChildType```](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-children) we created will be copied into

```csharp
string soundName = "SoundName";
Vector3 position = new Vector3(10f, 10f, 0f);
ChildType child = ChildType.AT_3D_POS;

AudioError err = am.RegisterChildAt3DPos(soundName, position, out child);
if (err != AudioError.OK) {
    Debug.Log("Playing sound called: " + soundName + " at the position x " + position.x.ToString("0.00") + ", y " + position.y.ToString("0.00") + "and z " + position.z.ToString("0.00") + " once failed with error id: " + err);
}
else {
    Debug.Log("Playing sound called: " + soundName + " at the position x " + position.x.ToString("0.00") + ", y " + position.y.ToString("0.00") + "and z " + position.z.ToString("0.00") + " once succesfull");
}
```

**When to use it:**
When you want to create a new child copy of the sound directly at a 3D position and make the ```volume``` be influenced by the distance the player has to that 3D position.

**Remarks:**
To use either [```RegisterChildAttachedToGameObject```](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/audiosource/register_child_attached_to_go/) or [```RegisterChildAt3DPosition```](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/audiosource/register_child_at_3d_pos/) the sound we want to create a copy from, has to have 3D enabled. Either use [```Set3DAudioOptions```](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/audiosource/set_3d_audio_options/) to set the needed settings via. code or make the sound 3D capable with the ```Spatial Blend``` parameter on the ```AudioSourceSetting```.
If the method is called with the same ```SoundName```, that has already registered a child, this method copies all settings of that sound to the registered child instead and changes the position to the newly passed 3D position instead.

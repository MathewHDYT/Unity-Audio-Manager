---
layout: default
title: Register Child Attached To GameObject
parent: AudioSource
grand_parent: Documentation
---

## Register Child Attached To GameObject
**What it does:**
Registers a new child sound attached to the given ```GameObject```, so it can later be referenced via. the corresponding ```ChildType``` value and returns an AudioError (see [Possible Errors](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)), showing wheter and how registering the child sound attached to the given ```GameObject``` failed.

[**Possible Errors:**](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)
- DOES_NOT_EXIST
- MISSING_WRAPPER
- MISSING_SOURCE
- MISSING_CLIP
- CAN_NOT_BE_3D
- INVALID_PARENT

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to play
- ```GameObject``` is the object the sound is emitting from
- ```Child``` is the variable the [```ChildType```](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-children) we created will be copied into

```csharp
string soundName = "SoundName";
GameObject gameObject = this.gameObject;
ChildType child = ChildType.ATTCHD_TO_GO;

AudioError err = am.RegisterChildAttachedToGo(soundName, gameObject, out child);
if (err != AudioError.OK) {
    Debug.Log("Registering new child for: " + soundName + " attached to: " + gameObject.name + " failed with error id: " + err);
}
else {
    Debug.Log("Registering new child for: " + soundName + " attached to: " + gameObject.name + " succesfull");
}
```

**When to use it:**
When you want to create a new child copy of the sound directly attached to the ```GameObject``` and make the ```volume``` be influenced by the distance the player has from that ```GameObject```.

**Remarks:**
To use either [```RegisterChildAttachedToGameObject```](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/audiosource/register_child_attached_to_go/) or [```RegisterChildAt3DPosition```](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/audiosource/register_child_at_3d_pos/) the sound we want to create a copy from, has to have 3D enabled. Either use [```Set3DAudioOptions```](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/audiosource/set_3d_audio_options/) to set the needed settings via. code or make the sound 3D capable with the ```Spatial Blend``` parameter on the ```AudioSourceSetting```.
If the method is called with the same ```SoundName```, that has already registered a child, this method copies all settings of that sound to the registered child instead and changes the parent to the newly passed ```GameObject``` instead.

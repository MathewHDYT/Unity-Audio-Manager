---
layout: default
title: Fluent Interface
nav_order: 9
permalink: /fluent_interface
---

## Fluent Interface

Starting from ```v2.1.0``` see ([GitHub release](https://github.com/MathewHDYT/Unity-Audio-Manager-UAM/releases/)) there is an additional class that allows using a lot of the AudioManager methods as a fluent interface.

### Setup

Ensure the steps in Installation have been completed, especially [Reference to Audio Manager script](https://mathewhdyt.github.io/Unity-Audio-Manager/installation#reference_to_audio_manager_script),
so the gotten ```IAudioManager``` can be passed into the ```AudioChainer``` call.

This is done to either create and register a new sound or use an already existing sound for all following calls to the ```IFluentAudioManager```.

### Example code:

```csharp
using AudioManager.Service; // Additional include to use AudioChainer static helper class.

private void ExampleChainedCall() {
    string soundName = "SoundName";
    ChildType child = ChildType.PARENT;
	float minPitch = 0.8f;
	float maxPitch = 1.2f;

    AudioError err = AudioChainer.SelectSound(am, soundName, child).ChangePitch(minPitch, maxPitch).Play();
    if (err != AudioError.OK) {
        Debug.Log("Chaining calls for the sound called: " + soundName + " failed with error id: " + err);
    }
    else {
        Debug.Log("Chaining calls for the sound called: " + soundName + " succesfull");
    }
}
```

### Remarks:

If a method fails it will cause all following methods in the chain to not be called anymore.
Additionally calling ```Stop``` or any ```Play``` method on the ```IFluentAudioManager```, will stop the chaining and return the current AudioError.
This will be if any the first error we received in the chain and that caused the other methods to not be called anymore.

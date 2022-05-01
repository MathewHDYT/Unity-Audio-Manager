---
layout: default
title: Documentation
nav_order: 2
has_children: true
---

## Public accessible methods
This section explains all public accessible methods, especially what they do, how to call them and when using them might be advantageous instead of other methods. We always assume AudioManager instance has been already referenced in the script. If you haven't done that already see [Reference to Audio Manager Script](https://mathewhdyt.github.io/Unity-Audio-Manager/installation#reference-to-audio-manager-script).

## Possible Errors
Shows all the possible ```AudioError``` enum values that can be returned by the AudioManager methods. Where the ```ID``` is the value that is actually returned, the ```CONSTANT``` is the name that value was given in the ```AudioError``` enum and where the ```MEANING``` is how and why the error was produced in the first place.

| **ID** | **CONSTANT**                  | **MEANING**                                                                                    |
| -------| ------------------------------| -----------------------------------------------------------------------------------------------|
| 0      | OK                            | Method succesfully executed                                                                    |
| 1      | DOES_NOT_EXIST                | Sound has not been registered with the AudioManager                                            |
| 2      | ALREADY_EXISTS                | Can't add sound as there already exists a sound with that name                                 |
| 3      | INVALID_PATH                  | Can't add sound because the path does not lead to a valid audio clip                           |
| 4      | INVALID_END_VALUE             | The given endValue is already the same as the current value                                    |
| 5      | INVALID_GRANULARITY           | The given granularity is too small, has to be higher than or equal to 1                        |
| 6      | INVALID_TIME                  | The given time exceeds the actual length of the clip                                           |
| 7      | INVALID_PROGRESS              | The given value is to close to the end of the actual clip length, therefore the given value can not be detected, because playing audio is frame rate independent |
| 8      | MIXER_NOT_EXPOSED             | The given parameter in the AudioMixer is not exposed or does not exist                         |
| 9      | MISSING_SOURCE                | Sound does not have an AudioSource component on the GameObject the AudioManager resides on     |
| 10     | MISSING_MIXER_GROUP           | Group methods may only be called with a sound that has a set AudioMixerGroup                   |
| 11     | CAN_NOT_BE_3D                 | The sound can not be 3D, because spatialBlend is set to be 2D instead of 3D                    |
| 12     | NOT_INITIALIZED               | No IAudioManager has been registered with the ServiceLocator ensure a GameObject with the AudioManagerSettings script is in your scene |
| 13     | MISSING_CLIP                  | Sound does not have an AudioClip component that can be played                                  |
| 14     | MISSING_PARENT                | AudioManager did not get passed a valid parent gameObject with the needed components. (MonoBehaviour, Transform) |
| 15     | INVALID_PARENT                | The given gameObject passed to the method was null and therefore no AudioSource component can be attached and played on it |

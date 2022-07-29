---
layout: default
title: Documentation
nav_order: 2
has_children: true
---

## Public accessible methods
This section explains all public accessible methods, especially what they do, how to call them and when using them might be advantageous instead of other methods. We always assume AudioManager instance has been already referenced in the script. If you haven't done that already see [Reference to Audio Manager Script](https://mathewhdyt.github.io/Unity-Audio-Manager/installation#reference-to-audio-manager-script).

## Possible Errors
Shows all the possible ```AudioError``` enum values that can be returned by the AudioManager methods.
Where the ```ID``` is the value that is actually returned, the ```CONSTANT``` is the name that value was given in the ```AudioError``` enum and where the ```MEANING``` is how and why the error was produced in the first place.

| **ID** | **CONSTANT**                  | **MEANING**                                                                                    |
| -------| ------------------------------| -----------------------------------------------------------------------------------------------|
| 0      | OK                            | Method succesfully executed                                                                    |
| 1      | DOES_NOT_EXIST                | Sound has not been registered with the AudioManager                                            |
| 2      | ALREADY_EXISTS                | There already exists a sound with that name                                 					  |
| 3      | INVALID_PATH                  | Path does not lead to a valid audio clip                           							  |
| 4      | INVALID_END_VALUE             | The given endValue is already the same as the current value                                    |
| 5      | INVALID_TIME                  | The given time exceeds the actual length of the clip                                           |
| 6      | INVALID_PROGRESS              | The given value is to close to the end or the start of the actual clip length, because playing audio is frame rate independent |
| 7      | MIXER_NOT_EXPOSED             | The given parameter in the AudioMixer is not exposed or does not exist                         |
| 8      | MISSING_SOURCE                | Sound does not have an AudioSource component on the GameObject the AudioManager resides on     |
| 9      | MISSING_MIXER_GROUP           | Group methods may only be called with a sound that has a set AudioMixerGroup                   |
| 10     | CAN_NOT_BE_3D                 | The sound can not be 3D, because spatialBlend is set to be 2D instead of 3D                    |
| 11     | NOT_INITIALIZED               | No IAudioManager has been registered with the ServiceLocator ensure a GameObject with the AudioManagerSettings script is in your scene |
| 12     | MISSING_CLIP                  | Sound does not have an AudioClip component that can be played                                  |
| 13     | MISSING_PARENT                | AudioManager did not get passed a valid parent gameObject with the needed components. (MonoBehaviour, Transform) |
| 14     | INVALID_PARENT                | The given gameObject passed to the method was null and therefore no AudioSource component can be attached and played on it |
| 15     | ALREADY_SUBSCRIBED            | Callback with the exact same progress was already subscribed for this sound					  |
| 16     | NOT_SUBSCRIBED                | Callback with the progress was not yet subscribed for this sound								  |
| 17     | MISSING_WRAPPER               | The given AudioSourceWrapper is null								  							  |
| 18     | INVALID_CHILD                 | The given sound does not have a registered child of the given type.							  |

## Possible Children
Shows all the possible ```ChildType``` enum values that can be passed into AudioManager methods, that allow executing the given action for different generated ```AudioSources```.
Where the ```ID``` is the value that is actually returned, the ```CONSTANT``` is the name that value was given in the ```ChildType``` enum and where the ```MEANING``` is how and what it will do.
This is an option because [```RegisterChildAt3DPos```](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/audiosource/register_child_at_3d_pos/) and [```RegisterChildAttachedToGo```](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/audiosource/register_child_attached_to_go/) can't use the original ```AudioSource``` because the position or the partet ```GameObject``` have to be changed,
meaning we have to create a copy of the ```AudioSource``` instead and use that.

| **ID** | **CONSTANT**                  | **MEANING**                                                                                    |
| -------| ------------------------------| -----------------------------------------------------------------------------------------------|
| 0      | ALL                           | All children as well as the parent                                                             |
| 1      | DOES_NOT_EXIST                | Parent object children were copied from                                                        |
| 2      | ALREADY_EXISTS                | Child for RegisterChildAt3DPosition                                 					          |
| 3      | INVALID_PATH                  | Child for RegisterChildAttachedToGameObject                          					      |

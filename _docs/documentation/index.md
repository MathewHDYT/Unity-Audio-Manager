---
layout: default
title: Documentation
nav_order: 2
has_children: true
---

## Public accessible methods
This section explains all public accessible methods, especially what they do, how to call them and when using them might be advantageous instead of other methods. We always assume AudioManager instance has been already referenced in the script. If you haven't done that already see [Reference to Audio Manager Script](https://mathewhdyt.github.io/Unity-Audio-Manager/installation#reference-to-audio-manager-script).

## Possible Errors
Shows all the possible AudioError Enum values that can be returned by the AudioManager methods. Where the ```ID``` is the value that is actually returned, the ```CONSTANT``` is the name that value was given in the AudioError enum and where the ```MEANING``` is how and why the error was produced in the first place.

| **ID** | **CONSTANT**                  | **MEANING**                                                                                    |
| -------| ------------------------------| -----------------------------------------------------------------------------------------------|
| 0      | OK                            | Method succesfully executed                                                                    |
| 1      | DOES_NOT_EXIST                | Sound has not been registered with the AudioManager                                            |
| 2      | FOUND_MULTIPLE                | Multiple instances with the same name found. First will be played                              |
| 3      | ALREADY_EXISTS                | Can't add sound as there already exists a sound with that name                                 |
| 4      | INVALID_PATH                  | Can't add sound because the path does not lead to a valid audio clip                           |
| 5      | SAME_AS_CURRENT               | The given endValue is already the same as the current value                                    |
| 6      | TOO_SMALL                     | The given granularity is too small, has to be higher than or equal to 1                        |
| 7      | TOO_BIG                       | The given startTime exceeds the actual length of the clip                                      |
| 8      | NOT_EXPOSED                   | The given parameter in the AudioMixer is not exposed or does not exist                         |
| 9      | MISSING_SOURCE                | Sound does not have an AudioSource component on the GameObject the AudioManager resides on     |
| 10     | MISSING_MIXER_GROUP           | Group methods may only be called with a sound that has a set AudioMixerGroup                   |
| 11     | CAN_NOT_BE_3D                 | The sound can not be 3D, because spatialBlend is set to be 2D instead of 3D                    |

---
layout: default
title: Logging
nav_order: 6
permalink: /logging
---

## Logging
The AudioManager uses a seperate Logger class, that creates messaged in the Unity console if the given log level on the AudioManager on startup is higher or equal to the needed log levels.

#### Log Levels
- ```NONE``` (No logging of any message)
- ```LOW``` (Only warnings and method executions that failed will be logged)
- ```INTERMEDIATE``` (All above levels and a message when a method is being executed)
- ```HIGH``` (All above levels and a message when a method has successfully executed)
- ```VERBOSE``` (Everything, generally not recommended)

### Setting minmum log level

To set the minimum log level simply choose one of the above mentioned log levels in the AudioManager ```Logging Level``` dropdown under the ```Logger Settings``` section.

![Image of AudioManager script](https://raw.githubusercontent.com/MathewHDYT/Unity-Audio-Manager/gh-pages/_images/AudioManager.png)

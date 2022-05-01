---
layout: default
title: Logging
nav_order: 6
permalink: /logging
---

## Logging

Starting from ```v1.7.0``` see ([GitHub release](https://github.com/MathewHDYT/Unity-Audio-Manager-UAM/releases/)) the project structure has been completly reworked. Therefore a custom ```IAudioLogger``` implementation can now be implemented.

#### Log Levels

- ```NONE``` (No logging of any messages. Improved performance because the Logger is never initiated nor called.)
- ```LOW``` (Only warnings of method executions that failed will be logged.)
- ```INTERMEDIATE``` (All above levels and a message when a method is being executed.)
- ```HIGH``` (All above levels and a message when a method has successfully executed.)
- ```STOPWATCH``` (All above levels and a message with the time needed to execute the method.)

### Setting minmum log level

To set the minimum log level simply choose one of the above mentioned log levels in the ```Logging Level``` dropdown under the ```Logger Settings``` section.

![Image of AudioManager script](https://raw.githubusercontent.com/MathewHDYT/Unity-Audio-Manager/gh-pages/_images/AudioManager.png)

## Custom logger

To create a custom ```IAudioLogger``` implementation, we first have to create a new C# script. An example template can be found below:

```csharp
using AudioManager.Logger;
using UnityEngine;

public class ExampleAudioLogger : IAudioLogger {

    public ExampleAudioLogger() {
        // Nothing to do.
    }

    public void Log(object message, LoggingLevel level, LoggingType type, Object context) {
        // Nothing to do.
    }

    public void LogFormat(string format, LoggingLevel level, LoggingType type, Object context, params object[] args) {
        // Nothing to do.
    }

    public void LogException(System.Exception exception, LoggingLevel level, Object context) {
        // Nothing to do.
    }

    public void LogAssert(bool condition, string message, LoggingLevel level, Object context) {
        // Nothing to do.
    }

    public void LogAssertFormat(bool condition, string format, LoggingLevel level, Object context, params object[] args) {
        // Nothing to do.
    }
}
```

When the code we actually want to call has been implemented we can use another script that is attached to a ```MonoBehaviour``` and use ```ServiceLocator.RegisterLogger``` to wrap the currently registered ```IAudioManager``` with the given custom ```IAudioLogger``` implementation.


```csharp
private void Start() {
	ExampleAudioLogger example = new ExampleAudioLogger();
	ServiceLocator.RegisterLogger(example);
}
```

Be aware calling ```RegisterLogger``` will wrap the current ```IAudioManager```, meaning if we already enabled logging the new Logger will log the exact same messages. This might make sense in cases were we log to UI or a file instead.
If this is not wanted tough, simply disable logging on the ```AudioManagerSettings``` this can be done through simply selecting ```NONE``` in the ```Logging Level``` dropdown.

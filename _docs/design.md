---
layout: default
title: Design
nav_order: 7
permalink: /design
---

## Design

Sarting from ```v1.7.0``` see ([GitHub release](https://github.com/MathewHDYT/Unity-Audio-Manager-UAM/releases/)) the project structure has been completly reworked and completly abandoned the [```Singelton```](https://gameprogrammingpatterns.com/singleton.html) pattern to improve code readability and maintanability. Instead the [```Service Locator```](https://gameprogrammingpatterns.com/service-locator.html) pattern is now used instead.

Additonally seperate assemblies and namespaces for the different parts of the AudioManager have been added to improve compile speed and overall project structure.

### Service Locator pattern

A more detailed description on the [```Service Locator```](https://gameprogrammingpatterns.com/service-locator.html) pattern and other patterns invaluable for game design can be found on [Game Programming Pattern](https://gameprogrammingpatterns.com/). As well as examples on all the patterns can be found in this github repository [Unity Design Patterns](https://github.com/QianMo/Unity-Design-Pattern).

The short explantion tough is that this pattern makes it possible to make a service globally accessible, which isn't always a good idea but very helpful for an ```AudioManager``` which might be entangled with a lot of your code. Without directly coupling to the class that actually implements the behaviour.
Meaning you use the static ```ServiceLocator``` to get an instance of ```IAudioManager```, which could be a custom implementation the ```NullAudioManager``` if an error occured more simply the ```DefaultAudioManager``` or even the ```LoggedAudioManager```. As you can hopefully see this makes it much easier to write your own implementation see ([Custom AudioManager](https://mathewhdyt.github.io/Unity-Audio-Manager/custom_audio_manager)) if you want to know more.

Additionaly this made it possible to completly seperate the Logging of the ```AudioManager``` from the actual implementation, meaning if Logging is disabled the ```IAudioLogger``` implementation will never even be called. Additonally we can easily register a new custom Logger that might log to a UI text field instead of the console see ([Logging](https://mathewhdyt.github.io/Unity-Audio-Manager/logging)) if you want to know more.

## Namespaces

### AudioManager.Core

#### What it includes
Core scripts needed in all other assemblies and namespaces for example the ```AudioError``` enum.

#### When you need it
In your own script as soon as you either stash the ```IAudioManager``` in a private variable, or save the return statement from an AudioManager method.

### AudioManager.Helper

#### What it includes
Static methods used for handling audio and extensions to the AudioSource.

#### When you need it
Generally not, but can be used when trying to implement a custom AudioManager as it contains many useful Helper methods, be aware tough the Helper methods do not do any null or other checks.
Meaning this has to be done before calling the corresponding methods.

### AudioManager.Locator

#### What it includes
```ServiceLocator``` that gets the currently registered ```IAudioManager``` implementation.

#### When you need it
In your own script as soon as you want to get and use the ```IAudioManager```.

### AudioManager.Logger

#### What it includes
Scripts needed for logging of the AudioManager.

#### When you need it
Is needed in your own script as soon as you either implement your own custom logger or attempt to use the ```ServiceLocator.RegisterLogger()``` method.

### AudioManager.Provider

#### What it includes
Script that needs to be set on one of your local ```GameObjects``` in the scene, so that an ```IAudioManager``` actually gets registered.

#### When you need it
Not needed as it only includes code to provide a default implementation with the given settings to the ```ServiceLocator```.

### AudioManager.Service

#### What it includes
```DefaultAudioManager``` that gets used for a normal AudioManager project.

#### When you need it
Not needed as it only includes code a default implementation of the ```IAudioManager```.

### AudioManager.Settings

#### What it includes
Scripts needed for the ```ScriptableObject``` that contains our ```AudioSourceSettings```.

#### When you need it
Not needed as it only includes code for the handling of the ```ScriptableObject```.

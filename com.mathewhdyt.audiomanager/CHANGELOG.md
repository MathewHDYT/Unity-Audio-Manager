# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Released]

## [2.1.0](https://github.com/MathewHDYT/Unity-Audio-Manager/releases/tag/v2.1.0) - 2022-07-30

### Added
- Added new decorator ```FluentAudioManager```, that similarly to the ```LoggedAudioManager``` wraps an ```IAudioManager```, in this case instead of returning an AudioError directly, we get the instance of the class on which we can then call more functions. Allowing usage as a fluent interface
- Added new static class ```AudioChainer```, that allows starting a chain of methods and reuses the same class instance between different chains to decrease garbage allocation
- Added new method ```DeregisterChild```

### Changed
- Adjusted ```AudioManagerSettings``` GameObject to have a safety check for when we load a scene and there is already another ```AudioManagerSettings``` GameObject the scene
- Adjusted ```RemoveSound``` to actually delete the underlying ```AudioSource``` component and deregister and delete all registered children


## [2.0.0](https://github.com/MathewHDYT/Unity-Audio-Manager/releases/tag/v2.0.0) - 2022-07-09

### Added
- Added new method ```SubscribeSourceChanged```
- Added new method ```UnsubscribeSourceChanged```
- Added new method ```RegisterChildAt3DPos```
- Added new method ```RegisterChildAttachedToGo```
- Added ```AudioSourceWrapper``` that makes to possible to detect changes on the ```AudioSource``` itself

### Changed
- Removed ```PlayAt3DPosition```, ```PlayOneShotAt3DPosition```, ```PlayAttachedToGameObject```, ```PlayOneShotAttachedToGameObject```
- Overhauled nearly all methods of the AudioManager with a additonal parameter ```ChildType``` that allows using that method for an ```AudioSource``` created by either ```RegisterChildAttachedToGo``` (```ChildType.ATTCHD_TO_GO```) or ```RegisterChildAt3DPos``` (```ChildType.AT_3D_POS```), the main original ```AudioSource``` (```ChildType.PARENT```) or all at once (```ChildType.ALL```)
- Overhauled the Example scene to reflect the new changes


## [1.9.0](https://github.com/MathewHDYT/Unity-Audio-Manager/releases/tag/v1.9.0) - 2022-06-19

### Added
- Added new method ```UnsubscribeProgressCoroutine```

### Changed
- Overhauled and renamed ```SubscribeAudioFinished``` to ```SubscribeProgressCoroutine```, now works with reverse playing songs and the callback has a return type, which decides wheter we unsubscribe the callback, resubscribe for the next iteration of the song or resubscribe it immediatly
- Replaced ```SkipForward``` and ```SkipBackward``` with new method ```SkipTime```, which skips forward if the value is positive or backwards if the value is negative
- Replaced ```ValueDataError``` and used ```out``` keyword for returning additional values from methods instead


## [1.8.0](https://github.com/MathewHDYT/Unity-Audio-Manager/releases/tag/v1.8.0) - 2022-06-02

### Added
- Play and Stop buttons on the ScriptableObject that allow to preview the clip with the given settings
- Added new methods ```GetEnumerator```, ```SetPlaypbackDirection```, ```SkipForward``` and ```SkipBackward```

### Changed
- Overhauled the UI and usability of the Example scene


## [1.7.0](https://github.com/MathewHDYT/Unity-Audio-Manager/releases/tag/v1.7.0) - 2022-05-02

### Added
- UnitTesting of the DefaultAudioManager, AudioLogger, LoggedAudioManager, ValueDataError, NullAudioManager and ServiceLocator class
- UILogger for the Example project to log the text shown in the console directly into a Textbox as well

### Changed
- Split source code into different [namespaces](https://docs.unity3d.com/Manual/Namespaces.html), as well as [assemblies](https://docs.unity3d.com/Manual/ScriptCompilationAssemblyDefinitionFiles.html) and folders corresponding to their features
- Fixed incorrect handling of null objects with the ```AudioLogger```, ```LoggedAudioManager``` and ```DefaultAudioManager```
- Renamed ```AudioManager``` to ```DefaultAudioManager``` to ensure no conflicts with the main namespace
- Cached ```new WaitForSecond()``` calls to decrease memory usage in the ```LerpVolume```, ```LerpPitch``` and ```LerpGroupValue``` method


## [1.6.0](https://github.com/MathewHDYT/Unity-Audio-Manager/releases/tag/v1.6.0) - 2022-02-06

### Added
- New logger class that allows logging with a given priority level and different underlying ```Debug.Log``` calls
- Added two new 3D methods which are derivatives of both the ```PlayAt3DPosition``` and ```PlayAttachedToGameObject``` method and support multiple instances of the same sound running at once

### Changed
- Updated AudioManager to have a dropdown toggle, that chooses the current log priority level, which then shows different amount of messages via. the newly added logger class
- Updated the functionality of all 3D methods to work with copies of the original AudioSource resisding on the AudioManager, this is done to ensure the AudioSource can be moved and deleted without effecting the AudioManager GameObject itself


## [1.5.0](https://github.com/MathewHDYT/Unity-Audio-Manager/releases/tag/v1.5.0) - 2022-01-21

### Changed
- AudioManager is now included via. the package manager as a package instead of as single scripts


## [1.4.0](https://github.com/MathewHDYT/Unity-Audio-Manager/releases/tag/v1.4.0) - 2022-01-09

### Added
- AudioSourceSetting scriptable objects, which have a seperate new section wich is hidden per default for 3D audio settings

### Changed
- Replaced old way of adding sounds via. scriptable objects instead
- Replaced old way to searching for a sound instead of a List into a Dictionary to improve performance, when there are many sounds registered
- Adjusted methods that use 3D sound to now use the given settings and not have to take the settings every time from the method call


## [1.3.0](https://github.com/MathewHDYT/Unity-Audio-Manager/releases/tag/v1.3.0) - 2022-01-03

### Added
- Multiple methods to support the AudioMixer and AudioMixerGroups


## [1.2.0](https://github.com/MathewHDYT/Unity-Audio-Manager/releases/tag/v1.2.0) - 2021-11-20

### Changed
- AudioError integration (all methods now return an AudioError enum value instead of a boolean, showing more explicitly how the method failed)


## [1.1.0](https://github.com/MathewHDYT/Unity-Audio-Manager/releases/tag/v1.1.0) - 2021-09-10

### Added
- Pausing sound
- Unpausing sound
- Adding a sound at runtime

### Changed
- Improved documentation
- Added documentation for new methods

## [1.0.0](https://github.com/MathewHDYT/Unity-Audio-Manager/releases/tag/v1.0.0) - 2021-09-04

### Added
- Inital release (Playing sound and changing the volume/pitch over time)

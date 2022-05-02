---
layout: default
title: Changelog
nav_order: 4
permalink: /changelog
---

# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Released]

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

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

## [1.5.0](https://github.com/MathewHDYT/Unity-Audio-Manager/releases/tag/v1.5.0) - 2021-01-21

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

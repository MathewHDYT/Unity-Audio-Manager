---
layout: default
title: Documentation
nav_order: 2
has_children: true
has_toc: false
permalink: /documentation-index
---

## Public accesible methods
This section explains all public accesible methods, especially what they do, how to call them and when using them might be advantageous instead of other methods. We always assume AudioManager instance has been already referenced in the script. If you haven't done that already see [Reference to Audio Manager Script](#reference-to-audio-manager-script).

**Unity Audio Manager implements the following methods consisting of a way to:**
- Add a new possible sound to play at runtime (see [Add Sound From Path method](#add-sound-from-path-method))
- Simply play a sound (see [Play method](#play-method))
- Start playing a sound at a given time in the sound (see [Play At Time Stamp method](#play-at-time-stamp-method))
- Get the amount of time a sound has been played (see [Get Playback Position method](#get-playback-position-method))
- Play a sound at a 3D position (see [Play At 3D Position method](#play-at-3d-position-method))
- Play a sound attached to a gameobject (see [Play Attached To GameObject method](#play-attached-to-gameobject-method))
- Play a sound after a certain delay time (see [Play Delayed method](#play-delayed-method))
- Play a sound once (see [Play OneShot method](#play-oneshot-method))
- Play a sound at a given time in the time line (see [Play Scheduled method](#play-scheduled-method))
- Stop a sound (see [Stop method](#stop-method))
- Mute or unmute a sound (see [Toggle Mute method](#toggle-mute-method))
- Pause or unpause a sound (see [Toggle Pause method](#toggle-pause-method)
- Get the progress of a sound (see [Get Progress method](#get-progress-method))
- Try to get the source of a sound (see [Try Get Source method](#try-get-source-method))
- Lerp the pitch of a sound over a given time (see [Lerp Pitch method](#lerp-pitch-method))
- Lerp the volume of a sound over a given time (see [Lerp Volume method](#lerp-volume-method))
- Change the value of the given exposed parameter of a given sound to the given newValue (see [Change Group Value method](#change-group-value-method))
- Get the value of the given exposed parameter of a given sound (see [Get Group Value method](#get-group-value-method))
- Reset the value of the given exposed parameter of a given sound (see [Reset Group Value method](#reset-group-value-method))
- Lerp the value of the given exposed parameter of a given sound over a given time (see [Lerp Group Value method](#lerp-group-value-method))

namespace AudioManager.Core {
    public enum AudioError {
        OK,
        DOES_NOT_EXIST,
        ALREADY_EXISTS,
        INVALID_PATH,
        INVALID_END_VALUE,
        INVALID_TIME,
        INVALID_PROGRESS,
        MIXER_NOT_EXPOSED,
        MISSING_SOURCE,
        MISSING_MIXER_GROUP,
        CAN_NOT_BE_3D,
        NOT_INITIALIZED,
        MISSING_CLIP,
        MISSING_PARENT,
        INVALID_PARENT,
        ALREADY_SUBSCRIBED,
        NOT_SUBSCRIBED,
        MISSING_WRAPPER,
        INVALID_CHILD
    }
}

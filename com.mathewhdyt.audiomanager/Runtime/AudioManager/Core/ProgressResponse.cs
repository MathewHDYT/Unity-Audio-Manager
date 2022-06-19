namespace AudioManager.Core {
    public enum ProgressResponse {
        UNSUB, // Does not call the given AudioCallback anymore.
        RESUB_IN_LOOP, // Calls the given AudioCallback for the next loop iteration of the song at the same progress point.
        RESUB_IMMEDIATE, // Calls the given AudioCallback immediatly as soon as the subscribed time is reached again, only recommended if we skip back time in the callback.
    }
}

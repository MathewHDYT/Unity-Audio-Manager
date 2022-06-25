namespace AudioManager.Core {
    public enum ChildType {
        PARENT, // Parent object children were copied from.
        PLAY_AT_3D_POS, // Child for PlayAt3DPosition.
        PLAY_OS_AT_3D_POS, // Child for PlayOneShotAt3DPosition.
        PLAY_ATTCHD_TO_GO, // Child for PlayAttachedToGameObject.
        PLAY_OS_ATTCHD_TO_GO // Child for PlayOneShotAttachedToGameObject.
    }
}

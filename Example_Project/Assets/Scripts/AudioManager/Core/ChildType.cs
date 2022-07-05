namespace AudioManager.Core {
    public enum ChildType {
        ALL, // All children as well as the parent.
        PARENT, // Parent object children were copied from.
        AT_3D_POS, // Child for RegisterChildAt3DPosition.
        ATTCHD_TO_GO, // Child for RegisterChildAttachedToGameObject.
    }
}

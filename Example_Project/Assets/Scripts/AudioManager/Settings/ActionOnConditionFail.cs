namespace AudioManager.Settings {
    public enum ActionOnConditionFail {
        DO_NOT_DRAW, // If condition(s) are false, don't draw the field at all.
        JUST_DISABLE // If condition(s) are false, just set the field as disabled.
    }
}

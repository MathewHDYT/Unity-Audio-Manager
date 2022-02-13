namespace AudioManager.Settings {
    public enum ConditionOperator {
        // A field is visible/enabled only if all conditions are true.
        AND,
        // A field is visible/enabled if at least ONE condition is true.
        OR
    }
}

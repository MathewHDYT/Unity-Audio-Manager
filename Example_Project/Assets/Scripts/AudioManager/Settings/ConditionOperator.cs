namespace AudioManager.Settings {
    /// <summary>
    /// Defines when exactly the given condition evaluates to true and when it doesn't.
    /// </summary>
    public enum ConditionOperator {
        AND, // A field is visible/enabled only if all conditions are true.
        OR // A field is visible/enabled if at least ONE condition is true.
    }
}

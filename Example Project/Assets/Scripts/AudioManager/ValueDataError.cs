[System.Serializable]
public class ValueDataError<T> {
    public T Value { get; set; }
    public int Error { get; set; }

    public ValueDataError(T value, int error) {
        Value = value;
        Error = error;
    }
}
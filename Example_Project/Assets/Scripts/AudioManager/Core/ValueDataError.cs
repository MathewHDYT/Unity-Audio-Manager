namespace AudioManager.Core {
    public struct ValueDataError<T> {
        public T Value { get; set; }
        public AudioError Error { get; set; }

        public ValueDataError(T value, AudioError error) {
            Value = value;
            Error = error;
        }
    }
}

namespace ExNovo
{
    public class ExNovoException : System.Exception
    {
        public ExNovoException() { }
        public ExNovoException(string message) : base(message) { }
        public ExNovoException(string message, System.Exception inner) : base(message, inner) { }
    }
}

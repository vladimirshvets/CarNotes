namespace CarNotes.Domain.Common.Exceptions
{
    public class MatchNotFoundException : Exception
    {
        public MatchNotFoundException()
            : this("No node matches the specified parameters.")
        {
        }

        public MatchNotFoundException(string? message)
            : base(message)
        {
        }

        public MatchNotFoundException(string? message, Exception? innerException)
            : base(message, innerException)
        {
        }
    }
}

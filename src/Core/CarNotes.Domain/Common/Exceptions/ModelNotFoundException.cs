namespace CarNotes.Domain.Common.Exceptions
{
    public class ModelNotFoundException : Exception
    {
        public ModelNotFoundException()
            : this("No model matches the specified parameters.")
        {
        }

        public ModelNotFoundException(string? message)
            : base(message)
        {
        }

        public ModelNotFoundException(string? message, Exception? innerException)
            : base(message, innerException)
        {
        }
    }
}

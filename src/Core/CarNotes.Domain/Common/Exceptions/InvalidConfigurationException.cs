using System;
namespace CarNotes.Domain.Common.Exceptions
{
    public class InvalidConfigurationException : Exception
    {
        public InvalidConfigurationException()
            : this("Invalid configuration.")
        {
        }

        public InvalidConfigurationException(string? message)
            : base(message)
        {
        }

        public InvalidConfigurationException(
            string? message, Exception? innerException)
            : base(message, innerException)
        {
        }
    }
}


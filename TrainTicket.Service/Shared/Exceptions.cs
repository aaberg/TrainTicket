namespace TrainTicket.Service.Shared;

public static class Exceptions
{
    public class EntityDoesNotExistException : Exception
    {
        public EntityDoesNotExistException(string message) : base(message)
        {
        }

        public EntityDoesNotExistException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
namespace TrainTicket.Domain;

public static class DomainExceptions
{
    public class InvalidStationName : Exception
    {
        public InvalidStationName(string message) : base(message)
        {
        }
    }
    
    public class InvalidEntityState : Exception
    {
        public InvalidEntityState(object entity, string message) : 
            base($"Entity {entity.GetType().Name} is in invalid state. {message}")
        {
        }
    }
}
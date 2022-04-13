namespace TrainTicket.Central.Station;

public static class Contracts
{
    public static class V1
    {
        public record Add(string Name);
    
        public record ChangeName(string Name);

        public record StationResponse(Guid Id, string Name);
    }
    
}
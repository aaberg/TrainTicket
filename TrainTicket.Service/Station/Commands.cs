namespace TrainTicket.Service.Station;

public static class Commands
{
    public record Add(Guid Id, string Name);

    public record ChangeName(Guid Id, string NewName);
}
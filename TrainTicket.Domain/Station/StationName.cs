namespace TrainTicket.Domain.Station;

public record StationName
{
    private string Value { get; }
    
    internal StationName(string name)
    {
        Value = name;
    }
    
    public static StationName FromString(string name)
    {
        if (name.Length == 0)
            throw new DomainExceptions.InvalidStationName("Station name cannot be empty");
        return new StationName(name);
    }
    
    public static implicit operator string(StationName name)
    {
        return name.Value;
    }

    public static implicit operator StationName(string value)
    {
        return FromString(value);
    }

    public override string ToString()
    {
        return Value;
    }
}
namespace TrainTicket.Domain.Station;

public class StationId
{
    public Guid Value{get;}
    
    public StationId(Guid id)
    {
        Value = id;
    }

    public override string ToString()
    {
        return Value.ToString();
    }

    public static implicit operator Guid(StationId id)
    {
        return id.Value;
    }

    public static implicit operator StationId(Guid value)
    {
        return new StationId(value);
    }
}
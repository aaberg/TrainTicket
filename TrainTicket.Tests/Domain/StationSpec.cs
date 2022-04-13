using System;
using FluentAssertions;
using TrainTicket.Domain;
using TrainTicket.Domain.Station;
using Xunit;

namespace TrainTicket.Tests.Domain;

public class StationSpec
{
    private readonly Station _station;
    
    public StationSpec()
    {
        _station = new Station( Guid.NewGuid(), "Test Station");
    }

    [Fact]
    public void Can_Change_Name()
    {
        const string newStationName = "New Station Name";
        _station.ChangeName(newStationName);
        
        _station.Name.Should().Be(StationName.FromString(newStationName));
    }
    
    [Fact]
    public void Cannot_Change_Name_To_Empty_String()
    {
        Assert.Throws<DomainExceptions.InvalidStationName>(() => 
            _station.ChangeName(string.Empty)
        );
    }
}
namespace TrainTicket.ClientApp;

public class KafkaConfiguration
{
    public string BootstrapServers { get; set; }
    public string GroupId { get; set; }
    
    public string StationTopic = "station-events";
}
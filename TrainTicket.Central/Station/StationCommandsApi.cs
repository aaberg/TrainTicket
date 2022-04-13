using Microsoft.AspNetCore.Mvc;
using TrainTicket.Service.Station;

namespace TrainTicket.Central.Station;

[Route("/station")]
[Produces("application/json")]
[Consumes("application/json")]
public class StationCommandsApi : ControllerBase
{
    private readonly StationService _stationService;

    public StationCommandsApi(StationService stationService)
    {
        _stationService = stationService;
    }

    [HttpPost]
    public async Task<IActionResult> NewStation([FromBody] Contracts.V1.Add request)
    {
        var station = await _stationService.Handle(new Commands.Add(Guid.NewGuid(), request.Name));
        return Ok(new Contracts.V1.StationResponse(station.Id, station.Name));
    }

    [Route("{id:guid}/name")]
    [HttpPut]
    public async Task<IActionResult> ChangeName([FromRoute] Guid id, [FromBody] Contracts.V1.ChangeName request)
    {
        var station = await _stationService.Handle(new Commands.ChangeName(id, request.Name));
        return Ok(new Contracts.V1.StationResponse(station.Id, station.Name));
    }
}
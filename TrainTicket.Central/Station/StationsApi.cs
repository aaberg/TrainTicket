using Microsoft.AspNetCore.Mvc;
using TrainTicket.Service.Infrastructure;
using TrainTicket.Service.Projections;
using TrainTicket.Service.Station;

namespace TrainTicket.Central.Station;

[Route("/station")]
[Produces("application/json")]
[Consumes("application/json")]
[ApiController]
public class StationsApi : ControllerBase
{
    private readonly StationService _stationService;
    private readonly StationQueryService _queryService;

    public StationsApi(StationService stationService, StationQueryService queryService)
    {
        _stationService = stationService;
        _queryService = queryService;
    }

    /// <summary>
    /// Creates a new station
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> NewStation([FromBody] Contracts.V1.Add request)
    {
        var station = await _stationService.Handle(new Commands.Add(Guid.NewGuid(), request.Name));
        return Ok(new Contracts.V1.StationResponse(station.Id, station.Name));
    }

    /// <summary>
    /// Updates station name
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [Route("{id:guid}/name")]
    [HttpPut]
    public async Task<IActionResult> ChangeName([FromRoute] Guid id, [FromBody] Contracts.V1.ChangeName request)
    {
        var station = await _stationService.Handle(new Commands.ChangeName(id, request.Name));
        return Ok(new Contracts.V1.StationResponse(station.Id, station.Name));
    }
    
    /// <summary>
    /// Return all stations
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IEnumerable<StationDetails> GetAll()
    {
        return _queryService.Query(details => true);
    }

    /// <summary>
    /// Return station with the given {id}
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("{id}")]
    public ActionResult<StationDetails> GetById(string id)
    {
        var station = _queryService.Query(details => details.Id == id).FirstOrDefault();
        if (station == null)
            return NotFound();

        return station;
    }
}
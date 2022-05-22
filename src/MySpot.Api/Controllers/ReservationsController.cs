using Microsoft.AspNetCore.Mvc;
using MySpot.Api.Commands;
using MySpot.Api.DTO;
using MySpot.Api.Entities;
using MySpot.Api.Services;

namespace MySpot.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ReservationsController : ControllerBase
{
    private readonly ReservationsService _reservationsService = new ();
    
    [HttpGet]
    public ActionResult<IEnumerable<Reservation>> GetAll()
    {
        return Ok(_reservationsService.GetAllWeekly());
    }


    [HttpGet("{id:guid}")]
    public ActionResult<Reservation> Get(Guid id)
    {
        var reservation = _reservationsService.Get(id);

        if (reservation is null)
        {
            return NotFound();
        }

        return Ok(reservation);
    }

    [HttpPost]
    public ActionResult Post(CreateReservation createReservationCommand)
    {
        var id = _reservationsService.Create(createReservationCommand with { ReservationId = Guid.NewGuid() });
        if (id is null)
        {
            return BadRequest();
        }

        return CreatedAtAction(nameof(Get), new {Id = id}, default);
    }

    [HttpPut("{id:guid}")]
    public ActionResult Put(Guid id, ChangeReservationLicencePlate changeReservationLicencePlateCommand)
    {
        if (id != changeReservationLicencePlateCommand.ReservationId)
        {
            return BadRequest();
        }
        
        var succeeded = _reservationsService.Update(changeReservationLicencePlateCommand);

        if (!succeeded)
        {
            return BadRequest();
        }

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public ActionResult Delete(Guid id)
    {
        var succeeded = _reservationsService.Delete(new DeleteReservation(id));

        if (!succeeded)
        {
            return BadRequest();
        }

        return NoContent();
    }
}
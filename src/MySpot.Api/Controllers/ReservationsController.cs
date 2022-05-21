using System.Resources;
using Microsoft.AspNetCore.Mvc;
using MySpot.Api.Models;

namespace MySpot.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ReservationsController : ControllerBase
{
    private static readonly string[] ParkingSpotNames = {"P1", "P2", "P3", "P4", "P5"};
    private static readonly List<Reservation> Reservations = new();

    private static int Id = 1;

    [HttpGet]
    public ActionResult<IEnumerable<Reservation>> GetAll()
    {
        return Ok(Reservations);
    }


    [HttpGet("{id:int}")]
    public ActionResult<Reservation> Get(int id)
    {
        var reservation = Reservations.SingleOrDefault(x => x.Id == id);

        if (reservation is null)
        {
            HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            return NotFound();
        }

        return Ok(reservation);
    }

    [HttpPost]
    public ActionResult Post(Reservation reservation)
    {
        reservation.Id = Id;
        
        reservation.Date = DateTime.Now.AddDays(1).Date;

        if (ParkingSpotNames.All(x => x != reservation.ParkingSpotName))
        {
            return BadRequest();
        }

        var reservationAlreadyExists = Reservations.Any(
            x => x.Date.Date == reservation.Date.Date && x.ParkingSpotName == reservation.ParkingSpotName);
        if (reservationAlreadyExists)
        {
            return BadRequest();
        }
        
        Id++;
        Reservations.Add(reservation);
        return CreatedAtAction(nameof(Get), new {reservation.Id}, reservation);
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Reservation reservation)
    {
        var existingReservation = Reservations.SingleOrDefault(x => x.Id == id);

        if (existingReservation is null)
        {
            return BadRequest();
        }

        existingReservation.LicencePlate = reservation.LicencePlate;
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var existingReservations = Reservations.SingleOrDefault(x => x.Id == id);

        if (existingReservations is null)
        {
            return BadRequest();
        }

        Reservations.Remove(existingReservations);
        return NoContent();
    }
}
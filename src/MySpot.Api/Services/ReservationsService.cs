using MySpot.Api.Commands;
using MySpot.Api.DTO;
using MySpot.Api.Entities;

namespace MySpot.Api.Services;

public sealed class ReservationsService
{
    private static readonly WeeklyParkingSpot[] WeeklyParkingSpots =
    {
        new(Guid.Parse("00000000-0000-0000-0000-000000000001"), DateTime.UtcNow.AddDays(-7), DateTime.UtcNow, "P1"),
        new(Guid.Parse("00000000-0000-0000-0000-000000000002"), DateTime.UtcNow.AddDays(-7), DateTime.UtcNow, "P2"),
        new(Guid.Parse("00000000-0000-0000-0000-000000000003"), DateTime.UtcNow.AddDays(-7), DateTime.UtcNow, "P3"),
        new(Guid.Parse("00000000-0000-0000-0000-000000000004"), DateTime.UtcNow.AddDays(-7), DateTime.UtcNow, "P4"),
        new(Guid.Parse("00000000-0000-0000-0000-000000000005"), DateTime.UtcNow.AddDays(-7), DateTime.UtcNow, "P5"),
    };

    public IEnumerable<ReservationDto> GetAllWeekly() =>
        WeeklyParkingSpots
            .SelectMany(x => x.Reservations)
            .Select(x => new ReservationDto
                {
                    Id = x.Id,
                    EmployeeName =x.EmployeeName,
                    Date = x.Date.Date
                }
            );

    public ReservationDto Get(Guid id)
    {
        return GetAllWeekly().SingleOrDefault(x => x.Id == id);
    }

    public Guid? Create(CreateReservation createReservationCommand)
    {
        var (parkingSpotId, reservationId, employeeName, licencePlate, date) = createReservationCommand;
        
        var weeklyParkingSpot = WeeklyParkingSpots.SingleOrDefault(
            x => x.Id == parkingSpotId);

        if (weeklyParkingSpot is null)
        {
            return default;
        }

        var reservation = new Reservation(reservationId, employeeName, licencePlate, date);
        weeklyParkingSpot.AddReservation(reservation);
        return reservation.Id;
    }

    public bool Update(ChangeReservationLicencePlate changeReservationLicencePlateCommand)
    {
        var weeklyParkingSpot = GetWeeklyParkingSpotByReservation(changeReservationLicencePlateCommand.ReservationId);

        if (weeklyParkingSpot is null)
        {
            return false;
        }

        var reservation = weeklyParkingSpot.Reservations
            .SingleOrDefault(x => x.Id == changeReservationLicencePlateCommand.ReservationId);

        if (reservation is null)
        {
            return false;
        }
        
        reservation.ChangeLicencePlate(changeReservationLicencePlateCommand.LicencePlate);
        return true;
    }

    public bool Delete(DeleteReservation deleteReservationCommand)
    {
        var weeklyParkingSpot = GetWeeklyParkingSpotByReservation(deleteReservationCommand.ReservationId);

        if (weeklyParkingSpot is null)
        {
            return false;
        }
        
        weeklyParkingSpot.DeleteReservation(deleteReservationCommand.ReservationId);
        return true;
    }

    private WeeklyParkingSpot GetWeeklyParkingSpotByReservation(Guid id)
        => WeeklyParkingSpots.SingleOrDefault(x => x.Reservations.Any(r => r.Id == id));

}
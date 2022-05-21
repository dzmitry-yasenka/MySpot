namespace MySpot.Api.Models;

public class Reservation
{
    public int Id { get; set; }
    
    public string EmployeeName { get; set; } = default!;

    public string ParkingSpotName { get; set; } = default!;

    public string LicencePlate { get; set; } = default!;
    
    public DateTime Date { get; set; }
}
namespace MySpot.Api.DTO;

public sealed class ReservationDto
{
    public Guid Id { get; set; }
    public string EmployeeName { get; set; }
    public string LicencePlate { get; set; }
    public DateTime Date { get; set; }
}
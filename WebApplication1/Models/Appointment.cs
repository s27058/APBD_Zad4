namespace WebApplication1.Models;

public class Appointment
{
    public DateTime Date { get; set; }
    public int AnimalId { get; set; }
    public string Description { get; set; }
    public int Price { get; set; }
}
namespace WebApplication1.Models;

public class Animal
{
    public int Id { get; set; }
    public string Name { get; set; }
    public AnimalCategory Category { get; set; }
    public double Mass { get; set; }
    public string Color { get; set; }

    public Animal(int id, string name, AnimalCategory category, double mass, string color)
    {
        Id = id;
        Name = name;
        Category = category;
        Mass = mass;
        Color = color;
    }
}
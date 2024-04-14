using System.Reflection;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using WebApplication1;
using WebApplication1.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

Db.Animals.Add(new Animal(1, "Szarik", AnimalCategory.Dog, 10, "szary"));

app.MapGet("/api/animals", () => Db.Animals);
app.MapGet("/api/animals/{id}", (int id) =>
{
    var animal = Db.Animals.FirstOrDefault(animal => animal.Id == id);
    return animal is null ? Results.NotFound($"Zwierzę o id {id} nie istnieje.") : Results.Ok(animal);
});
app.MapGet("/api/appointments", (int animalId) =>
{
    if (!Db.Animals.Exists(animal => animal.Id == animalId)) return Results.NotFound($"Zwierzę o id {animalId} nie istnieje.");
    var appointments = Db.Appointments.FindAll(appointment => appointment.AnimalId == animalId);
    return appointments.Count == 0 ? Results.NoContent() : Results.Ok(appointments);
});

app.MapPost("/api/animals/", (Animal animal) =>
{
    if (Db.Animals.Exists(animal1 => animal1.Id == animal.Id)) return Results.Conflict();
    Db.Animals.Add(animal);
    return Results.Created();
});
app.MapPost("/api/appointments", (Appointment appointment) =>
{
    if (!Db.Animals.Exists(animal1 => animal1.Id == appointment.AnimalId)) return Results.NotFound($"Zwierzę o id {appointment.AnimalId} nie istnieje.");
    Db.Appointments.Add(appointment);
    return Results.Created();
});

app.MapPut("/api/animals/", (Animal animal) =>
{
    int index = Db.Animals.FindIndex(animal1 => animal1.Id == animal.Id);
    if (index == -1)
    {
        Db.Animals.Add(animal);
        return Results.Created();
    }
    Db.Animals[index] = animal;
    return Results.NoContent();
});

app.MapDelete("/api/animals/{id}", (int id) =>
{
    return Db.Animals.RemoveAll(animal1 => animal1.Id == id) == 0 ? Results.NotFound($"Zwierzę o id {id} nie istnieje.") : Results.NoContent();
});

app.UseHttpsRedirection();

app.Run("http://localhost:8000");

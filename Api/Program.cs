using Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors();

builder.Services.AddDbContext<HouseDbContext>(options =>
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
builder.Services.AddScoped<IHouseRepository, HouseRepository>();
builder.Services.AddScoped<IBidRepository, BidRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(p => p.WithOrigins("http://localhost:3000")
    .AllowAnyHeader().AllowAnyMethod());

app.UseHttpsRedirection();

/*
 *  Endpoints
 */
app.MapGet("/houses", (IHouseRepository repository) => repository.GetAll())
    .Produces<HouseDto>(statusCode: 200);

app.MapGet("/house/{houseId:int}", async (int houseId, IHouseRepository repository) =>
{
    var house = await repository.Get(houseId);

    if (house == null)
    {
        return Results.Problem($"House with ID {houseId} not found.", statusCode: 404);
    }

    return Results.Ok(house);
}).ProducesProblem(404).Produces<HouseDetailDto>(statusCode: 200);

app.MapPost("/houses", async ([FromBody] HouseDetailDto dto, IHouseRepository repository) =>
{
    if (!MiniValidator.TryValidate(dto, out var errors))
    {
        return Results.ValidationProblem(errors);
    }

    var newHouse = await repository.Add(dto);

    return Results.Created($"/house/{newHouse.HouseId}", newHouse);
}).Produces<HouseDetailDto>(StatusCodes.Status201Created).ProducesValidationProblem();

app.MapPut("/houses", async ([FromBody] HouseDetailDto dto, IHouseRepository repository) =>
{
    if (!MiniValidator.TryValidate(dto, out var errors))
    {
        return Results.ValidationProblem(errors);
    }
    
    if (await repository.Get(dto.HouseId) == null)
    {
        return Results.Problem($"House {dto.HouseId} not found.", statusCode: 404);
    }

    var updatedHouse = await repository.Update(dto);

    return Results.Ok(updatedHouse);
}).ProducesProblem(404).Produces<HouseDetailDto>(statusCode: 200).ProducesValidationProblem();

app.MapDelete("/houses/{houseId:int}", async (int houseId, IHouseRepository repository) =>
{
    if (await repository.Get(houseId) == null)
    {
        return Results.Problem($"House {houseId} not found.", statusCode: 404);
    }

    await repository.Delete(houseId);

    return Results.Ok();
}).ProducesProblem(404).Produces(statusCode: 200);

app.Run();

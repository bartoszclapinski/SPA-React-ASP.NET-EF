using Microsoft.AspNetCore.Mvc;
using MiniValidation;

namespace Api.Data;

public static class WebAppHouseExtensions
{
    public static void MapHouseEndpoints(this WebApplication app)
    {
        app.MapGet("/houses", 
                (IHouseRepository repository) => repository.GetAll())
            .Produces<HouseDto>(statusCode: 200);
        
        app.MapGet("/house/{houseId:int}", 
            async (int houseId, IHouseRepository repository) =>
        {
            var house = await repository.Get(houseId);
        
            if (house == null)
            {
                return Results.Problem($"House with ID {houseId} not found.", statusCode: 404);
            }
        
            return Results.Ok(house);
        }).ProducesProblem(404).Produces<HouseDetailDto>(statusCode: 200);
        
        app.MapPost("/houses", 
            async ([FromBody] HouseDetailDto dto, IHouseRepository repository) =>
        {
            if (!MiniValidator.TryValidate(dto, out var errors))
            {
                return Results.ValidationProblem(errors);
            }
        
            var newHouse = await repository.Add(dto);
        
            return Results.Created($"/house/{newHouse.HouseId}", newHouse);
        }).Produces<HouseDetailDto>(StatusCodes.Status201Created).ProducesValidationProblem();
        
        app.MapPut("/houses", 
            async ([FromBody] HouseDetailDto dto, IHouseRepository repository) =>
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
        
        app.MapDelete("/houses/{houseId:int}",
            async (int houseId, IHouseRepository repository) =>
        {
            if (await repository.Get(houseId) == null)
            {
                return Results.Problem($"House {houseId} not found.", statusCode: 404);
            }
        
            await repository.Delete(houseId);
        
            return Results.Ok();
        }).ProducesProblem(404).Produces(statusCode: 200);
    }
}
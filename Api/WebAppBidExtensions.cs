using Microsoft.AspNetCore.Mvc;
using MiniValidation;

namespace Api.Data;

public static class WebAppBidExtensions
{
    public static void MapBidEndpoints(this WebApplication app)
    {
        app.MapGet("house/{houseId:int}/bids", async (int houseId, IHouseRepository houseRepo, IBidRepository bidRepo) => 
        {
            if (await houseRepo.Get(houseId) == null)
            {
                return Results.Problem($"House with id {houseId} not found.");
            }

            var bids = await bidRepo.Get(houseId);
            return Results.Ok(bids);

        }).ProducesProblem(404).Produces(statusCode: 200);

        app.MapPost("house/{houseId:int}/bids",
            async (int houseId, [FromBody] BidDto dto, IBidRepository repository) => 
            {
                if (dto.HouseId != houseId)
                { 
                    return Results.Problem("No match", statusCode: StatusCodes.Status400BadRequest);
                }

                if (!MiniValidator.TryValidate(dto, out var errors))
                { 
                    return Results.ValidationProblem(errors);
                }

                var newBid = await repository.Add(dto);

                return Results.Created($"/houses/{newBid.HouseId}/bids", newBid);
            }).ProducesProblem(statusCode: 400).Produces<BidDto>(statusCode: 201).ProducesValidationProblem();
    }
}
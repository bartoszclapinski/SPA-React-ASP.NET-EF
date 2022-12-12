using System.ComponentModel.DataAnnotations;

namespace Api.Data;

public record BidDto(
    int BidId,
    int HouseId,
    [property: Required] string Bidder,
    int Amount);

    

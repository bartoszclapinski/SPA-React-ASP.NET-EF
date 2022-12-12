using System.ComponentModel.DataAnnotations;

namespace Api.Data;

public record HouseDetailDto(
    int HouseId, 
    [property: Required] string? Address, 
    [property: Required] string? Country, 
    int Price, 
    string? Description, 
    string? Photo);

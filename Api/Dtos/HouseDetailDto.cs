namespace Api.Data;

public record HouseDetailDto(int HouseId, string? Address, string? Country, 
    int Price, string? Description, string? Photo);

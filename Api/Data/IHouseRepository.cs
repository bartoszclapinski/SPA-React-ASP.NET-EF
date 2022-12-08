namespace Api.Data;

public interface IHouseRepository
{
    Task<List<HouseDto>> GetAll();
}
namespace Api.Data;

public interface IHouseRepository
{
    Task<List<HouseDto>> GetAll();
    Task<HouseDetailDto?> Get(int id);
    Task<HouseDetailDto> Add(HouseDetailDto dto);
    Task<HouseDetailDto> Update(HouseDetailDto dto);
    Task Delete(int id);
}
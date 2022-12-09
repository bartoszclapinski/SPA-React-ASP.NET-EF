using Microsoft.EntityFrameworkCore;

namespace Api.Data;

public class HouseRepository : IHouseRepository
{
    private readonly HouseDbContext _context;

    public HouseRepository(HouseDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<List<HouseDto>> GetAll()
    {
        return await _context.Houses
            .Select(h => new HouseDto(h.HouseId, h.Address, h.Country, h.Price))
            .ToListAsync();
    }

    public async Task<HouseDetailDto?> Get(int id)
    {
        var houseToReturn = await _context.Houses.SingleOrDefaultAsync(h => h.HouseId == id);

        if (houseToReturn == null)
        {
            return null;
        }

        return new HouseDetailDto(
            houseToReturn.HouseId,
            houseToReturn.Address,
            houseToReturn.Country,
            houseToReturn.Price,
            houseToReturn.Description,
            houseToReturn.Photo);
    }
}
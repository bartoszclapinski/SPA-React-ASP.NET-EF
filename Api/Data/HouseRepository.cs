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
}
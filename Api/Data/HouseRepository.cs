using Microsoft.EntityFrameworkCore;

namespace Api.Data;

public class HouseRepository : IHouseRepository
{
    private readonly HouseDbContext _context;

    public HouseRepository (HouseDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    private static HouseDetailDto EntityToDetailDto (HouseEntity houseToReturn)
    {
        return new HouseDetailDto(
            houseToReturn.HouseId,
            houseToReturn.Address,
            houseToReturn.Country,
            houseToReturn.Price,
            houseToReturn.Description,
            houseToReturn.Photo);
    }
    
    private static void DtoToEntity (HouseDetailDto dto, HouseEntity houseEntity)
    {
        houseEntity.Address = dto.Address;
        houseEntity.Country = dto.Country;
        houseEntity.Description = dto.Description;
        houseEntity.Price = dto.Price;
        houseEntity.Photo = dto.Photo;
    }

    public async Task<List<HouseDto>> GetAll ()
    {
        return await _context.Houses
            .Select(h => new HouseDto(h.HouseId, h.Address, h.Country, h.Price))
            .ToListAsync();
    }

    public async Task<HouseDetailDto?> Get (int id)
    {
        var houseToReturn = await _context.Houses.SingleOrDefaultAsync(h => h.HouseId == id);

        if (houseToReturn == null)
        {
            return null;
        }

        return EntityToDetailDto(houseToReturn);
    }

    public async Task<HouseDetailDto> Add (HouseDetailDto dto)
    {
        var houseEntity = new HouseEntity();
        DtoToEntity(dto, houseEntity);
        _context.Houses.Add(houseEntity);
        await _context.SaveChangesAsync();

        return EntityToDetailDto(houseEntity);
    }

    public async Task<HouseDetailDto> Update (HouseDetailDto dto)
    {
        var houseEntity = await _context.Houses.FindAsync(dto.HouseId);
        
        if (houseEntity == null)
        {
            throw new ArgumentException($"Error updating house with id {dto.HouseId}.");
        }
        
        DtoToEntity(dto, houseEntity);
        _context.Entry(houseEntity).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return EntityToDetailDto(houseEntity);
    }

    public async Task Delete(int id)
    {
        var houseEntity = await _context.Houses.FindAsync(id);
        if (houseEntity == null)
        {
            throw new ArgumentException($"Error deleting house with id {id}.");
        }

        _context.Houses.Remove(houseEntity);
        await _context.SaveChangesAsync();
    }
}
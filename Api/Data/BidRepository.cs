using Microsoft.EntityFrameworkCore;

namespace Api.Data;

public class BidRepository : IBidRepository
{
    private readonly HouseDbContext _context;

    public BidRepository(HouseDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<List<BidDto>> Get(int houseId)
    {
        return await _context.Bids.Where(b => b.HouseId == houseId)
            .Select(b => new BidDto(b.BidId, b.HouseId, b.Bidder, b.Amount))
            .ToListAsync();
    }

    public async Task<BidDto> Add(BidDto dto)
    {
        var bidEntity = new BidEntity();
        bidEntity.HouseId = dto.HouseId;
        bidEntity.Bidder = dto.Bidder;
        bidEntity.Amount = dto.Amount;
        _context.Bids.Add(bidEntity);
        await _context.SaveChangesAsync();
        return new BidDto(bidEntity.BidId, bidEntity.HouseId, bidEntity.Bidder, bidEntity.Amount);
    }
}
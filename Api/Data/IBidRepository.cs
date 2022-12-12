namespace Api.Data;

public interface IBidRepository
{
    public Task<List<BidDto>> Get(int houseId);
    public Task<BidDto> Add(BidDto dto);
}

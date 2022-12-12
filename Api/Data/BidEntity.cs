using System.ComponentModel.DataAnnotations;

namespace Api.Data;

public class BidEntity
{
    [Key] public int BidId { get; set; }
    public int HouseId { get; set; }
    public HouseEntity? House { get; set; }
    public string Bidder { get; set; } = string.Empty;
    public int Amount { get; set; }
}
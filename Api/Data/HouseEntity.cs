﻿using System.ComponentModel.DataAnnotations;

namespace Api.Data;

public class HouseEntity
{
    [Key] public int HouseId { get; set; }
    public string? Address { get; set; }
    public string? Country { get; set; }
    public string? Description { get; set; }
    public int Price { get; set; }
    public string? Photo { get; set; }
}
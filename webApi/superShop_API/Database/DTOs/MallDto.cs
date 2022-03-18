using System.ComponentModel.DataAnnotations;
using superShop_API.Database.DTOs.Base;
using superShop_API.Database.Entities;

namespace superShop_API.Database.DTOs;

public class MallDto : BaseDto<Mall, Guid>
{
    public string Name { get; set; }
    public Coordinates Coordinates { get; set; }

    [DataType(DataType.ImageUrl)]
    public string ImageUrl { get; set; }

    public List<BranchDto>? Branches { get; set; }

    public MallDto()
    {
    }

    public MallDto(Mall entity) : base(entity)
    {
        Name = entity.Name;
        ImageUrl = entity.ImageUrl;
        Coordinates = entity.Coordinates;
        Branches = entity.Branches != null ? entity.Branches.ToList().ConvertAll(b => new BranchDto(b)) : new List<BranchDto>();
    }

    protected override Mall MakeEntity()
    {
        return new Mall
        {
            Name = Name,
            ImageUrl = ImageUrl,
            Coordinates = Coordinates
        };
    }
}
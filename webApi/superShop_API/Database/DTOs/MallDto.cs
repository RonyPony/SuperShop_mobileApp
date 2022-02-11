using superShop_API.Database.DTOs.Base;
using superShop_API.Database.Entities;

namespace superShop_API.Database.DTOs;

public class MallDto : BaseDto<Mall>
{
    public string Name { get; set; }
    public Coordinates Coordinates { get; set; }

    public List<Branch> Branches { get; set; }

    public MallDto()
    {
    }

    public MallDto(Mall entity) : base(entity)
    {
        Name = entity.Name;
        Coordinates = entity.Coordinates;
        Branches = entity.Branches.ToList();
    }

    protected override Mall MakeEntity()
    {
        return new Mall
        {
            Name = Name,
            Coordinates = Coordinates
        };
    }
}
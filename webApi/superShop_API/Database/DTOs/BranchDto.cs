using superShop_API.Database.DTOs.Base;
using superShop_API.Database.Entities;

namespace superShop_API.Database.DTOs;

public class BranchDto : BaseDto<Branch>
{
    public string Name { get; set; }
    public string LocalCode { get; set; }
    public Guid MallId { get; set; }

    public BranchDto()
    {
    }

    public BranchDto(Branch entity) : base(entity)
    {
        Name = entity.Name;
        LocalCode = entity.LocalCode;
        MallId = entity.MallId;
    }

    protected override Branch MakeEntity()
    {
        return new Branch
        {
            Name = Name,
            LocalCode = LocalCode,
            MallId = MallId,
        };
    }
}
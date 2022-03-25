using Newtonsoft.Json;
using superShop_API.Database.DTOs.Base;
using superShop_API.Database.Entities;
using superShop_API.Shared;

namespace superShop_API.Database.DTOs;

public class OrderDto : BaseDto<Order, Guid, OrderSeedParams>
{
    public OrderDto()
    {
    }

    public OrderDto(Order entity) : base(entity)
    {
        entity.Branch.Products = null;
        BranchId = entity.BranchId;
        UserId = entity.UserId;
        Address = entity.Address;
        Total = entity.Total;
        Completed = entity.Completed;
        Branch = entity.Branch != null ? new BranchDto(entity.Branch) : new BranchDto();
        _TranssactionDetails = entity.TranssactionDetails;
        Products = entity.Products != null ? entity.Products.ToList().ConvertAll(po => new ProductDto(po)) : new List<ProductDto>();
    }

    public Guid BranchId { get; set; }

    public Guid UserId { get; set; }

    public string Address { get; set; }

    public double TotalTax { get; set; }

    public double TotalWhitoutTaxes { get; set; }

    public double Total { get; set; }

    public bool Completed { get; set; }

    public BranchDto Branch { get; set; }

    public string TranssactionDetails { get { return _TranssactionDetails.ToString(); } set { _TranssactionDetails = new DataObject(value); } }

    [JsonIgnore]
    private DataObject _TranssactionDetails { get; set; }

    public IList<ProductDto>? Products { get; set; }
    protected override Order MakeEntity()
    {
        return new Order
        {
            BranchId = BranchId,
            UserId = UserId,
            Address = Address,
            TotalTax = TotalTax,
            TotalWhitoutTaxes = TotalWhitoutTaxes,
            Total = Total,
            Completed = Completed,
            TranssactionDetails = _TranssactionDetails
        };
    }
}

public class NewOrderDto : BaseDto<Order, Guid, OrderSeedParams>
{
    public Guid BranchId { get; set; }
    public Guid UserId { get; set; }
    public string Address { get; set; }
    public bool Completed { get; set; }
    public Guid[] ProductIds { get; set; }

    public string TranssactionDetails { get { return _TranssactionDetails.ToString(); } set { _TranssactionDetails = new DataObject(value); } }

    [JsonIgnore]
    private DataObject _TranssactionDetails { get; set; }

    public NewOrderDto()
    {

    }

    protected override Order MakeEntity()
    {
        return new Order
        {
            BranchId = BranchId,
            UserId = UserId,
            Address = Address,
            Completed = Completed,
            TranssactionDetails = _TranssactionDetails
        };
    }
}
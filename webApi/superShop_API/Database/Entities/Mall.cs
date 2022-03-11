using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Bogus;
using superShop_API.Database.Entities.Base;
using superShop_API.Database.Seeders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Linq.Expressions;
using Newtonsoft.Json;

namespace superShop_API.Database.Entities;

[Table("malls")]
public class Mall : BaseEntity, ISeeder<Mall>
{
    [Required]
    [Column("name", TypeName = "varchar(80)")]
    public string Name { get; set; }

    [Required]
    [Column("coordinates")]
    public Coordinates Coordinates { get; set; }

    [Required]
    [Column("imageUrl", TypeName = "text")]
    public string ImageUrl { get; set; }

    public virtual ICollection<Branch>? Branches { get; set; }

    public Faker<Mall> SeederDefinition(object? referenceId)
    {
        return new Faker<Mall>()
        .RuleFor(m => m.Name, f => f.Company.CompanyName())
        .RuleFor(m => m.Coordinates, f => new Coordinates(f.Address.Latitude(), f.Address.Longitude()));
    }
}

public struct Coordinates
{
    public Coordinates(double lat, double @long)
    {
        Lat = lat;
        Long = @long;
    }

    public double Lat { get; set; }
    public double Long { get; set; }
}

public class CoordinatesCorverter : ValueConverter<Coordinates, string>
{
    public CoordinatesCorverter() : base(coords => CoordinatesToJson(coords), str => JsonToCoordinates(str))
    {
    }

    public static string CoordinatesToJson(Coordinates value)
    {
        return JsonConvert.SerializeObject(value);
    }

    public static Coordinates JsonToCoordinates(string value)
    {
        return JsonConvert.DeserializeObject<Coordinates>(value);
    }
}
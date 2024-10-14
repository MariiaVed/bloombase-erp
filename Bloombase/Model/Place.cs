
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bloombase;

[Table("Place")]

public class Place
{
    [Key]
    [Required]
    [MaxLength(100)]
    public string PlaceId { get; set; }

    [MaxLength(100)]
    public string PlaceName { get; set; }

    [MaxLength(100)]
    public string PlaceType { get; set; }

    public int FlowerbedCapacity { get; set; }

    // Define foreign key property
    [ForeignKey("BotanicalGarden")]
    [MaxLength(100)]
    public string BotanicalGardenName { get; set; }

    public Place()
    {
        BotanicalGardenName = "Enchanted Greenhouse";
    }
    public Place(string placeName, string placeType)
    {
        PlaceName = placeName;
        PlaceType = placeType;
    }
}

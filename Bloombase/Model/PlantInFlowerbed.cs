using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bloombase;

[Table("PlantInFlowerbed")]
public class PlantInFlowerbed
{
    [ForeignKey("Plant")] 
    [MaxLength(100)]
    public string PlantId { get; set; }
	[NotMapped]
    public Plant Plant { get; set; }

    [ForeignKey("Flowerbed")]
    [Column(Order = 1)]
    [MaxLength(100)]
    public string FlowerbedId { get; set; }

    [NotMapped]
    public Flowerbed Flowerbed { get; set; }

    [ForeignKey("Place")]
    [Column(Order = 2)]
    [MaxLength(100)]
    public string PlaceId { get; set; }

    [NotMapped]
    public Place Place { get; set; }

    public int Quantity { get; set; }

    public PlantInFlowerbed() { }
    public PlantInFlowerbed(int quantity, Plant plant, Flowerbed flowerbed, Place place)
    {
        Quantity = quantity;
        Plant = plant;
        Flowerbed = flowerbed;
        Place = place;
    }
}

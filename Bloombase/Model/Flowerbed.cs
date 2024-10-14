using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bloombase;

[Table("Flowerbed")]

public class Flowerbed
{
    [Key]
    [Required]
    [MaxLength(100)]
    public string FlowerbedId { get; set; }
    public int Size { get; set; }

    // Define foreign key property
    [ForeignKey("Place")]
    [MaxLength(100)]
    public string PlaceId { get; set; }

    [NotMapped]
    public Place Place { get; set; }

    // Define foreign key property
    [ForeignKey("Climate")]
    [MaxLength(100)]
    public string ClimateId { get; set; }

    [NotMapped]
    public Climate Climate { get; set; }

    // Define foreign key property
    [ForeignKey("Employee")]
    [MaxLength(100)]
    public string ResponsibleEmployeeId { get; set; }
   
    [NotMapped]
    public Employee ResponsibleEmployee { get; set; }

    public List<FlowerbedCare> FlowerbedCares { get; set; }

    public Flowerbed() 
    {
        ResponsibleEmployeeId = "";
        FlowerbedCares = new List<FlowerbedCare>();
    }

    public Flowerbed(string flowerbedId, int size, Place place, Climate climate, Employee responsibleEmployee, string responsibleEmployeeId = "")
    {
        FlowerbedId = flowerbedId;
        Size = size;
        Place = place;
        Climate = climate;
        ResponsibleEmployeeId = responsibleEmployeeId;
        ResponsibleEmployee = responsibleEmployee;
        FlowerbedCares = new List<FlowerbedCare>();
    }
}

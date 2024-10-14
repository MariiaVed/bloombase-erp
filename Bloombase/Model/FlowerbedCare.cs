using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bloombase;

[Table("FlowerbedCare")]

public class FlowerbedCare
{
    [Key]
    [Required]
    [MaxLength(100)]
    public string FlowerbedCareId { get; set; }

    [MaxLength(100)]
    public string FlowerbedCareType { get; set; }

    [Column(TypeName = "Date")]
    public DateTime Date { get; set; }

    [Column(TypeName = "Text")]
    public string Description { get; set; }

    // Define foreign key property
    [ForeignKey("Flowerbed")]
    [MaxLength(100)]
    public string FlowerbedId { get; set; }

    [NotMapped]
    public Flowerbed Flowerbed { get; set; }

    // Define foreign key property
    [ForeignKey("Employee")]
    [MaxLength(100)]
    public string EmployeeId { get; set; }

    [NotMapped]
    public Employee Employee { get; set; }

    // Define foreign key property
    [ForeignKey("Place")]
    [MaxLength(100)]
    public string PlaceId { get; set; }

    [NotMapped]
    public Place Place { get; set; }

    public FlowerbedCare() {
     }
    public FlowerbedCare(string flowerbedCareId, string flowerbedCareType, DateTime date, string description, Flowerbed flowerbed, Employee employee, Place place)
    {
        FlowerbedCareId = flowerbedCareId;
        FlowerbedCareType = flowerbedCareType;
        Date = date;
        Description = description;
        Flowerbed = flowerbed;
        Employee = employee;
        Place = place;
    }
}

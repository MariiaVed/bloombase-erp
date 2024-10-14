using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bloombase;

[Table("Maintenance")]
public class Maintenance
{
    [Key]
    [Required]
    [MaxLength(100)]
    public string MaintenanceId { get; set; }

    [MaxLength(100)]
    public string PruningSeason { get; set; }

    [MaxLength(100)]
    public string FloweringSeason { get; set; }

    public int WateringCycle { get; set; }
    
    [MaxLength(100)]
    public string Propagation { get; set; }

    public Maintenance() { }
    public Maintenance(string maintenanceId, string pruningSeason, string floweringSeason, int wateringCycle, string propagation)
    {
        MaintenanceId = maintenanceId;
        PruningSeason = pruningSeason;
        FloweringSeason = floweringSeason;
        WateringCycle = wateringCycle;
        Propagation = propagation;
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bloombase;

[Table("Plant")]
public class Plant
{
    [Key]
    [Required]
    [MaxLength(100)]
    public string PlantId { get; set; }

    [MaxLength(100)]
    public string Name { get; set; }

    // Workaround for image storage
    [NotMapped]
    public string Image
    {
        get
        {
            string[] dirSearch = AppDomain.CurrentDomain.BaseDirectory.Split("Bloombase");
            string resourceDir = dirSearch[0] + "Bloombase\\Resources\\Images";
            string returnedImage = "";

            if (Name == null)
            {
                return "placeholder.png";
            }

            foreach (var file in System.IO.Directory.GetFiles(resourceDir))
            {
                if (file.Replace(resourceDir + "\\", "").StartsWith(Name, StringComparison.CurrentCultureIgnoreCase))
                {
                    returnedImage = file;
                }
            }
            if (Name != "" && returnedImage != "")
            {
                return returnedImage;
            }
            else
            {
                return "placeholder.png";
            }
        }
    }

    [MaxLength(100)]
    public string BotanicalName { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Price { get; set; }

    [MaxLength(100)]
    public string Origin { get; set; }

    // Define foreign key property
    [ForeignKey("Climate")]
    [MaxLength(100)]
    public string ClimateId { get; set; }

    // Define foreign key property
    [ForeignKey("Maintenance")]
    [MaxLength(100)]
    public string MaintenanceId { get; set; }

    [NotMapped]
    public Climate Climate { get; set; }

    [NotMapped]
    public Maintenance Maintenance { get; set; }

    public Plant(){}
    public Plant(string plantId, string name, string botanicalName, decimal price, string origin, byte[] image)
    {
        PlantId = plantId;
        Name = name;
        BotanicalName = botanicalName;
        Price = price;
        Origin = origin;
    }
}
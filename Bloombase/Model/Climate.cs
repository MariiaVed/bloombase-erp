using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bloombase;

[Table("Climate")]

public class Climate
{
    [Key]
    [Required]
    [MaxLength(100)]
    public string ClimateId { get; set; }
   
    [MaxLength(100)]
    public string Light { get; set; }

    [NotMapped]
    private int _humidity;
    //Humidity is a percentage from 0 to 100
    
    public int Humidity
    {
        get { return _humidity; }
        set
        {
            //If above 100, set to 100
            if (value > 100)
            {
                _humidity = 100;
            }
            //If below 0, set to 0
            else if (value < 0)
            {
                _humidity = 0;
            }

            _humidity = value;
        }
    }

    public int Temperature { get; set; }

    [MaxLength(100)]
    public string SoilType { get; set; }

    [NotMapped]
    public List<Plant> Plants { get; set; }

    [NotMapped]
    public List<Flowerbed> Flowerbeds { get; set; }

    public Climate() { }
    public Climate(string climateId, int humidity, int temperature, string soilType, string light, List<Plant> plants, List<Flowerbed> flowerbeds)
    {
        ClimateId = climateId;
        Humidity = humidity;
        Temperature = temperature;
        SoilType = soilType;
        Light = light;
        Plants = plants;
        Flowerbeds = flowerbeds;
    }
}

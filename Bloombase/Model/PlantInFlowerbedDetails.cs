namespace Bloombase;

//NOTE THIS IS A SPECIAL CLASS USED TO DISPLAY PLANT IN FLOWERBED DETAILS BECAUSE LIST IN THE XAML PAGE CANNOT BIND TO MULTIPLE OBJECTS
public class PlantInFlowerbedDetails
{
	public string Name { get; set; }
	public string BotanicalName { get; set; }
	public int Quantity { get; set; }
	public string PlantId { get; set; }
	public string FlowerbedId { get; set; }
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
	public PlantInFlowerbedDetails() { }
	public PlantInFlowerbedDetails(Plant plant, PlantInFlowerbed plantInFlowerbed)
	{
		Name = plant.Name;
		BotanicalName = plant.BotanicalName;
		Quantity = plantInFlowerbed.Quantity;
		PlantId = plant.PlantId;
		FlowerbedId = plantInFlowerbed.FlowerbedId;
	}
}

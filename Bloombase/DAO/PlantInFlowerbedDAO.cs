namespace Bloombase;

public class PlantInFlowerbedDAO
{
    private readonly BloombaseContext _context;

    public PlantInFlowerbedDAO(BloombaseContext context)
    {
        _context = context;
    }

    // Add a new plant in flowerbed
    public void AddPlantInFlowerbed(PlantInFlowerbed plantInFlowerbed)
    {
        _context.PlantsInFlowerbeds.Add(plantInFlowerbed);
        _context.SaveChanges();
    }

    // Get a plant in flowerbed by ID
    public PlantInFlowerbed? GetPlantInFlowerbedById(string plantId, string flowerbedId)
    {
        return _context.PlantsInFlowerbeds
        .FirstOrDefault(p => p.PlantId == plantId && p.FlowerbedId == flowerbedId);
    }

    // Update an existing plant in flowerbed
    public void UpdatePlantInFlowerbed(PlantInFlowerbed plantInFlowerbed)
    {
        _context.PlantsInFlowerbeds.Update(plantInFlowerbed);
        _context.SaveChanges();
    }

    // Delete a plant in flowerbed
    public void DeletePlantInFlowerbed(PlantInFlowerbed plantInFlowerbed)
    {
        if (plantInFlowerbed != null)
        {
            _context.PlantsInFlowerbeds.Remove(plantInFlowerbed);
            _context.SaveChanges();
        }
    }

    // List all plants in flowerbed
    public List<PlantInFlowerbed> GetAllPlantsInFlowerbed(string flowerbedId)
    {
        if (flowerbedId != null)
        {
            return _context.PlantsInFlowerbeds
            .Where(p => p.FlowerbedId == flowerbedId)
            .ToList();
        }

        else return new List<PlantInFlowerbed>();

    }

        public int GetTotalQuantity()
    {
        int totalQuantity = _context.PlantsInFlowerbeds.Sum(p => p.Quantity);
        return totalQuantity;
    }

    public bool IsPlantInFlowerbedExists(string plantId)
    {
        return _context.PlantsInFlowerbeds.Any(p => p.PlantId == plantId);
    }

}

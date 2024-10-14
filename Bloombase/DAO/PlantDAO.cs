namespace Bloombase;

public class PlantDAO
{
    private readonly BloombaseContext _context;
    public PlantDAO(BloombaseContext context)
    {
        _context = context;
    }
    // Add a new plant
    public void AddPlant(Plant plant)
    {
        _context.Plants.Add(plant);
        _context.SaveChanges();
    }
    // Get a plant by ID
    public Plant? GetPlantById(string id)
    {
        return _context.Plants
        .FirstOrDefault(p => p.PlantId == id);
    }
    // Update an existing plant
    public void UpdatePlant(Plant plant)
    {
        _context.Plants.Update(plant);
        _context.SaveChanges();
    }
    // Delete a plant
    public void DeletePlant(string id)
    {
        var plant = _context.Plants.Find(id);
        if (plant != null)
        {
            _context.Plants.Remove(plant);
            _context.SaveChanges();
        }
    }
    // List all plants, optionally including garden information
    public List<Plant> GetAllPlants()
    {
        return _context.Plants.ToList();
    }

    public List<Plant> GetPlantsByClimate(string climateId)
    {
        return _context.Plants
        .Where(p => p.ClimateId == climateId)
        .ToList();
    }
}
